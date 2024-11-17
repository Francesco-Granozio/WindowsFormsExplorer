#include <windows.h>
#include <comdef.h>
#include <atlbase.h>
#include <iostream>
#include <vector>
#include <string>
#pragma comment(lib, "ole32.lib")
#pragma comment(lib, "oleaut32.lib")

// GUID per le interfacce DTE
static const IID ID_DTE = { 0x04A72314, 0x32E9, 0x48E2, {0x93, 0x43, 0x77, 0x39, 0xFB, 0xAE, 0x8A, 0xA8} };

struct VSInstance {
    IDispatch* dispatch;
    std::wstring solutionPath;
    bool isOpen;
};

std::wstring GetLastErrorAsString()
{
    DWORD errorMessageID = GetLastError();
    if (errorMessageID == 0) {
        return std::wstring();
    }

    LPWSTR messageBuffer = nullptr;
    size_t size = FormatMessageW(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
        NULL, errorMessageID, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPWSTR)&messageBuffer, 0, NULL);

    std::wstring message(messageBuffer, size);
    LocalFree(messageBuffer);
    return message;
}

std::vector<VSInstance> GetRunningVisualStudioInstances()
{
    std::vector<VSInstance> instances;
    IRunningObjectTable* pROT = nullptr;
    IEnumMoniker* pEnumMoniker = nullptr;

    if (GetRunningObjectTable(0, &pROT) != S_OK)
    {
        std::wcout << L"Errore nel recuperare la Running Object Table: " << GetLastErrorAsString() << std::endl;
        return instances;
    }

    if (pROT->EnumRunning(&pEnumMoniker) != S_OK)
    {
        std::wcout << L"Errore nel recuperare l'enumeratore degli oggetti in esecuzione: " << GetLastErrorAsString() << std::endl;
        pROT->Release();
        return instances;
    }

    pEnumMoniker->Reset();
    IMoniker* pMoniker = nullptr;
    ULONG fetched = 0;

    while (pEnumMoniker->Next(1, &pMoniker, &fetched) == S_OK)
    {
        CComPtr<IBindCtx> pBindCtx;
        CreateBindCtx(0, &pBindCtx);
        LPOLESTR displayName = nullptr;

        if (pMoniker->GetDisplayName(pBindCtx, nullptr, &displayName) == S_OK)
        {
            std::wstring ws(displayName);
            std::wcout << L"Found ROT entry: " << ws << std::endl;

            if (ws.find(L"!VisualStudio.DTE") != std::wstring::npos)
            {
                IUnknown* pObj = nullptr;
                HRESULT hr = pROT->GetObject(pMoniker, &pObj);
                if (SUCCEEDED(hr) && pObj != nullptr)
                {
                    IDispatch* pDispatch = nullptr;
                    hr = pObj->QueryInterface(IID_IDispatch, (void**)&pDispatch);
                    if (SUCCEEDED(hr) && pDispatch != nullptr)
                    {
                        VSInstance instance;
                        instance.dispatch = pDispatch;
                        instance.isOpen = false;
                        instance.solutionPath = L"";
                        instances.push_back(instance);
                        std::wcout << L"Added VS instance from: " << ws << std::endl;
                    }
                    pObj->Release();
                }
            }
            CoTaskMemFree(displayName);
        }
        pMoniker->Release();
    }

    pEnumMoniker->Release();
    pROT->Release();
    return instances;
}

void GetSolutionInfo(VSInstance& instance)
{
    if (instance.dispatch == nullptr) {
        std::wcout << L"Dispatch is null" << std::endl;
        return;
    }

    try {
        DISPID dispidSolution;
        OLECHAR* memberName = (OLECHAR*)L"Solution";
        HRESULT hr = instance.dispatch->GetIDsOfNames(IID_NULL, &memberName, 1, LOCALE_USER_DEFAULT, &dispidSolution);
        if (FAILED(hr)) {
            std::wcout << L"Failed to get Solution DISPID: " << _com_error(hr).ErrorMessage() << std::endl;
            return;
        }

        DISPPARAMS dispparamsNoArgs = { NULL, NULL, 0, 0 };
        VARIANT result;
        VariantInit(&result);

        hr = instance.dispatch->Invoke(dispidSolution, IID_NULL, LOCALE_USER_DEFAULT, DISPATCH_PROPERTYGET,
            &dispparamsNoArgs, &result, NULL, NULL);

        if (SUCCEEDED(hr) && result.vt == VT_DISPATCH && result.pdispVal != nullptr) {
            IDispatch* pSolution = result.pdispVal;

            // Get IsOpen
            DISPID dispidIsOpen;
            memberName = (OLECHAR*)L"IsOpen";
            hr = pSolution->GetIDsOfNames(IID_NULL, &memberName, 1, LOCALE_USER_DEFAULT, &dispidIsOpen);
            if (SUCCEEDED(hr)) {
                VARIANT varIsOpen;
                VariantInit(&varIsOpen);
                hr = pSolution->Invoke(dispidIsOpen, IID_NULL, LOCALE_USER_DEFAULT, DISPATCH_PROPERTYGET,
                    &dispparamsNoArgs, &varIsOpen, NULL, NULL);
                if (SUCCEEDED(hr) && varIsOpen.vt == VT_BOOL) {
                    instance.isOpen = varIsOpen.boolVal != VARIANT_FALSE;
                }
                VariantClear(&varIsOpen);
            }

            // Get FullName if solution is open
            if (instance.isOpen) {
                DISPID dispidFullName;
                memberName = (OLECHAR*)L"FullName";
                hr = pSolution->GetIDsOfNames(IID_NULL, &memberName, 1, LOCALE_USER_DEFAULT, &dispidFullName);
                if (SUCCEEDED(hr)) {
                    VARIANT varFullName;
                    VariantInit(&varFullName);
                    hr = pSolution->Invoke(dispidFullName, IID_NULL, LOCALE_USER_DEFAULT, DISPATCH_PROPERTYGET,
                        &dispparamsNoArgs, &varFullName, NULL, NULL);
                    if (SUCCEEDED(hr) && varFullName.vt == VT_BSTR) {
                        instance.solutionPath = varFullName.bstrVal;
                    }
                    VariantClear(&varFullName);
                }
            }

            pSolution->Release();
        }
        VariantClear(&result);
    }
    catch (_com_error& e) {
        std::wcout << L"COM Error: " << e.ErrorMessage() << std::endl;
    }
    catch (...) {
        std::wcout << L"Unknown error occurred" << std::endl;
    }
}

int main()
{
    HRESULT hr = CoInitializeEx(nullptr, COINIT_MULTITHREADED);
    if (FAILED(hr)) {
        std::wcout << L"Failed to initialize COM: " << _com_error(hr).ErrorMessage() << std::endl;
        return 1;
    }

    auto instances = GetRunningVisualStudioInstances();
    std::wcout << L"Trovate " << instances.size() << L" istanze di Visual Studio." << std::endl;
    std::wcout << L"----------------------------------------" << std::endl;

    int instanceNumber = 0;
    for (auto& instance : instances)
    {
        std::wcout << L"Analisi istanza #" << ++instanceNumber << std::endl;
        GetSolutionInfo(instance);

        std::wcout << L"Stato Solution: "
            << (instance.isOpen ? L"Aperta" : L"Chiusa") << std::endl;

        if (instance.isOpen && !instance.solutionPath.empty())
        {
            std::wcout << L"Percorso Solution: " << instance.solutionPath << std::endl;
        }

        std::wcout << L"----------------------------------------" << std::endl;

        if (instance.dispatch != nullptr)
        {
            instance.dispatch->Release();
        }
    }

    CoUninitialize();
    return 0;
}