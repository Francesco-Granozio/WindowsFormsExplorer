#include "pch.h"

//// GUID per le interfacce DTE (inutilizzate per ora)
//static const IID ID_DTE = { 0x04A72314, 0x32E9, 0x48E2, {0x93, 0x43, 0x77, 0x39, 0xFB, 0xAE, 0x8A, 0xA8} };

struct VisualStudioInstanceInfo {
	IDispatch* dispatch;
	wchar_t name[256]; // Nome dell'istanza (path comleto)
	bool isOpen;       // Indica se la soluzione � aperta

	VisualStudioInstanceInfo(IDispatch* pDispatch = nullptr, const wchar_t* pName = L"", bool open = false)
		: dispatch(pDispatch), isOpen(open) {

		wcscpy_s(name, sizeof(name) / sizeof(wchar_t), pName);
	}
};

struct VisualStudioInstance {
	wchar_t name[256]; // Nome dell'istanza (path comleto)
	bool isOpen;       // Indica se la soluzione � aperta

	VisualStudioInstance(const wchar_t* pName = L"", bool open = false)
		: isOpen(open) {

		wcscpy_s(name, sizeof(name) / sizeof(wchar_t), pName);
	}

	VisualStudioInstance(const VisualStudioInstanceInfo& other)
		: isOpen(other.isOpen) {

		wcscpy_s(name, sizeof(name) / sizeof(wchar_t), other.name);
	}
};


//recupera l'ultimo errore 
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



int GetSolutionInfo(VisualStudioInstanceInfo& instance)
{
	if (instance.dispatch == nullptr) {
		std::wcout << L"Dispatch is null" << std::endl;
		return -1;
	}

	try {
		DISPID dispidSolution;
		OLECHAR* memberName = (OLECHAR*)L"Solution";
		HRESULT hr = instance.dispatch->GetIDsOfNames(IID_NULL, &memberName, 1, LOCALE_USER_DEFAULT, &dispidSolution);
		if (FAILED(hr)) {
			std::wcout << L"Failed to get Solution DISPID: " << _com_error(hr).ErrorMessage() << std::endl;
			return -2;
		}

		DISPPARAMS dispparamsNoArgs = { NULL, NULL, 0, 0 };
		VARIANT result;
		VariantInit(&result);

		hr = instance.dispatch->Invoke(dispidSolution, IID_NULL, LOCALE_USER_DEFAULT, DISPATCH_PROPERTYGET,
			&dispparamsNoArgs, &result, NULL, NULL);
		if (FAILED(hr) || result.vt != VT_DISPATCH || result.pdispVal == nullptr) {
			VariantClear(&result);
			return -2;  // Non sono riuscito ad ottenere l'oggetto soluzione
		}

		IDispatch* pSolution = result.pdispVal;

		// Get IsOpen
		DISPID dispidIsOpen;
		memberName = (OLECHAR*)L"IsOpen";
		hr = pSolution->GetIDsOfNames(IID_NULL, &memberName, 1, LOCALE_USER_DEFAULT, &dispidIsOpen);
		if (FAILED(hr)) {
			pSolution->Release();
			VariantClear(&result);
			return -2;
		}

		VARIANT varIsOpen;
		VariantInit(&varIsOpen);
		hr = pSolution->Invoke(dispidIsOpen, IID_NULL, LOCALE_USER_DEFAULT, DISPATCH_PROPERTYGET,
			&dispparamsNoArgs, &varIsOpen, NULL, NULL);
		if (FAILED(hr) || varIsOpen.vt != VT_BOOL) {
			VariantClear(&varIsOpen);
			pSolution->Release();
			VariantClear(&result);
			return -2;
		}

		instance.isOpen = varIsOpen.boolVal != VARIANT_FALSE;
		VariantClear(&varIsOpen);

		// Get FullName solo se la soluzione � apperta
		if (instance.isOpen) {
			DISPID dispidFullName;
			memberName = (OLECHAR*)L"FullName";
			hr = pSolution->GetIDsOfNames(IID_NULL, &memberName, 1, LOCALE_USER_DEFAULT, &dispidFullName);
			if (FAILED(hr)) {
				pSolution->Release();
				VariantClear(&result);
				return -2;
			}

			VARIANT varFullName;
			VariantInit(&varFullName);
			hr = pSolution->Invoke(dispidFullName, IID_NULL, LOCALE_USER_DEFAULT, DISPATCH_PROPERTYGET,
				&dispparamsNoArgs, &varFullName, NULL, NULL);
			if (FAILED(hr) || varFullName.vt != VT_BSTR) {
				VariantClear(&varFullName);
				pSolution->Release();
				VariantClear(&result);
				return -2;
			}

			wcscpy_s(instance.name, varFullName.bstrVal);
			VariantClear(&varFullName);
		}

		pSolution->Release();
		VariantClear(&result);
	}
	catch (_com_error& e) {
		std::wcout << L"COM Error: " << e.ErrorMessage() << std::endl;
		return -3;
	}
	catch (...) {
		std::wcout << L"Unknown error occurred" << std::endl;
		return -4;
	}

	return 0;
}



//Il metodo recupera prima il numero di istanze 
std::vector<VisualStudioInstanceInfo> GetRunningVisualStudioInstancesCore()
{
	std::vector<VisualStudioInstanceInfo> instances;
	IRunningObjectTable* pROT = nullptr;
	IEnumMoniker* pEnumMoniker = nullptr;

	// Recupera il Running Object Table
	if (GetRunningObjectTable(0, &pROT) != S_OK) {
		std::wcout << L"Error while retrieving Running Object Table: " << GetLastErrorAsString() << std::endl;
		return instances; 
	}

	// Recupera l'enumeratore degli oggetti in esecuzione
	if (pROT->EnumRunning(&pEnumMoniker) != S_OK) {
		std::wcout << L"Errore nel recuperare l'enumeratore degli oggetti in esecuzione: " << GetLastErrorAsString() << std::endl;
		pROT->Release();
		return instances;
	}

	// Reset dell'enumeratore
	pEnumMoniker->Reset();
	IMoniker* pMoniker = nullptr;
	ULONG fetched = 0;

	// Ciclo attraverso gli oggetti in esecuzione
	while (pEnumMoniker->Next(1, &pMoniker, &fetched) == S_OK)
	{
		CComPtr<IBindCtx> pBindCtx;
		CreateBindCtx(0, &pBindCtx);
		LPOLESTR displayName = nullptr;

		// Recupera il nome dell'oggetto
		if (pMoniker->GetDisplayName(pBindCtx, nullptr, &displayName) != S_OK) {
			pMoniker->Release();
			continue; 
		}

		std::wstring ws(displayName);
		std::wcout << L"Found ROT entry: " << ws << std::endl;

		// Se � una sessione Visual Studio
		if (ws.find(L"!VisualStudio.DTE") != std::wstring::npos)
		{
			IUnknown* pObj = nullptr;
			HRESULT hr = pROT->GetObject(pMoniker, &pObj);
			if (FAILED(hr) || pObj == nullptr) {
				CoTaskMemFree(displayName);
				pMoniker->Release();
				continue; // Ritorna al ciclo in caso di errore
			}

			IDispatch* pDispatch = nullptr;
			hr = pObj->QueryInterface(IID_IDispatch, (void**)&pDispatch);
			if (FAILED(hr) || pDispatch == nullptr) {
				pObj->Release();
				CoTaskMemFree(displayName);
				pMoniker->Release();
				continue; // Ritorna al ciclo in caso di errore
			}

			VisualStudioInstanceInfo instance(pDispatch, L"", false);
			GetSolutionInfo(instance);

			// Rilascia il dispatch prima di aggiungere l'istanza
			if (instance.dispatch != nullptr) {
				instance.dispatch->Release();
			}

			instances.emplace_back(instance);
			std::wcout << L"Added VS instance from: " << ws << std::endl;

			pObj->Release();
		}

		CoTaskMemFree(displayName);
		pMoniker->Release();
	}

	pEnumMoniker->Release();
	pROT->Release();
	return instances;
}



// Funzione principale per ottenere le istanze di Visual Studio
extern "C" __declspec(dllexport) int GetRunningVisualStudioInstances(long* len, VisualStudioInstance** data) {

	HRESULT hr = CoInitializeEx(nullptr, COINIT_MULTITHREADED);

	if (FAILED(hr)) {
		std::wcout << L"Failed to initialize COM: " << _com_error(hr).ErrorMessage() << std::endl;
		return -1;
	}

	std::vector<VisualStudioInstanceInfo> temp_instances = GetRunningVisualStudioInstancesCore();

	*len = static_cast<long>(temp_instances.size());

	if (*len > 0) {
		// Alloca l'array di istanze nella memoria per riferimento
		*data = new VisualStudioInstance[*len];

		// Copia le istanze nel nuovo array
		for (long i = 0; i < *len; i++) {
			(*data)[i] = VisualStudioInstance(temp_instances[i]); 
		}
	}
	else {
		*data = nullptr;
	}

	CoUninitialize();
}


