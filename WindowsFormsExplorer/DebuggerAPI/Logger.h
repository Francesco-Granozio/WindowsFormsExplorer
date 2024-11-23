#pragma once

#include <ctime>
#include <fstream>
#include <iostream>
#include <sstream>

enum LogLevel { DEBUGLEVEL, INFOLEVEL, WARNINGLEVEL, ERRORLEVEL, CRITICALLEVEL };


class Logger {
public:
    // Constructor: Opens the log file in append mode
    Logger(const std::string& filename)
    {
        logFile.open(filename, std::ios::app);
        if (!logFile.is_open()) {
            std::cerr << "Error opening log file." << std::endl;
        }
    }

    // Destructor: Closes the log file
    ~Logger() { logFile.close(); }

    // Logs a message with a given log level
    void log(LogLevel level, const std::string& message)
    {
        // Ottieni il timestamp corrente
        time_t now = time(0);

        // Struttura tm per contenere i dati temporali
        tm timeinfo;

        // Usa localtime_s per popolare la struttura tm in modo sicuro
        if (localtime_s(&timeinfo, &now) != 0) {
            std::cerr << "Error while converting time." << std::endl;
            return;
        }

        // Formatta il timestamp come stringa
        char timestamp[20];
        strftime(timestamp, sizeof(timestamp), "%Y-%m-%d %H:%M:%S", &timeinfo);

        // Crea l'entry di log
        std::ostringstream logEntry;
        logEntry << "[" << timestamp << "] "
            << levelToString(level) << ": " << message
            << std::endl;

        // Output su console
        std::cout << logEntry.str();

        // Output su file di log
        if (logFile.is_open()) {
            logFile << logEntry.str();
            logFile.flush(); // Scrittura immediata su file
        }
    }

    void log(LogLevel level, const std::wstring& message)
    {
        // Ottieni il timestamp corrente
        time_t now = time(0);

        // Struttura tm per contenere i dati temporali
        tm timeinfo;

        // Usa localtime_s per popolare la struttura tm in modo sicuro
        if (localtime_s(&timeinfo, &now) != 0) {
            std::wcerr << L"Errore durante la conversione del tempo." << std::endl;
            return;
        }

        // Formatta il timestamp come stringa wide
        wchar_t timestamp[20];
        wcsftime(timestamp, sizeof(timestamp) / sizeof(wchar_t), L"%Y-%m-%d %H:%M:%S", &timeinfo);

        // Crea l'entry di log in formato wide
        std::wostringstream logEntry;
        logEntry << L"[" << timestamp << L"] "
            << levelToWString(level) << L": " << message
            << std::endl;

        // Output su console (wide)
        std::wcout << logEntry.str();

        // Output su file di log
        if (logFile.is_open()) {
            // Converti in stringa standard per scrivere nel file
            std::string utf8Log = wideStringToUtf8(logEntry.str());
            logFile << utf8Log;
            logFile.flush(); // Scrittura immediata su file
        }
    }



private:
    std::ofstream logFile; // File stream for the log file

    // Converts log level to a string for output
    std::string levelToString(LogLevel level)
    {
        switch (level) {
        case DEBUGLEVEL:
            return "DEBUG";
        case INFOLEVEL:
            return "INFO";
        case WARNINGLEVEL:
            return "WARNING";
        case ERRORLEVEL:
            return "ERROR";
        case CRITICALLEVEL:
            return "CRITICAL";
        default:
            return "UNKNOWN";
        }
    }

    std::wstring levelToWString(LogLevel level)
    {
        switch (level) {
        case DEBUGLEVEL:
            return L"DEBUG";
        case INFOLEVEL:
            return L"INFO";
        case WARNINGLEVEL:
            return L"WARNING";
        case ERRORLEVEL:
            return L"ERROR";
        case CRITICALLEVEL:
            return L"CRITICAL";
        default:
            return L"UNKNOWN";
        }
    }

    std::string wideStringToUtf8(const std::wstring& wstr)
    {
        if (wstr.empty()) return std::string();

        int sizeNeeded = WideCharToMultiByte(CP_UTF8, 0, wstr.c_str(), (int)wstr.size(), nullptr, 0, nullptr, nullptr);
        std::string strTo(sizeNeeded, 0);
        WideCharToMultiByte(CP_UTF8, 0, wstr.c_str(), (int)wstr.size(), &strTo[0], sizeNeeded, nullptr, nullptr);

        return strTo;
    }
};