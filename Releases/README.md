# Releases

Questa cartella contiene i file di rilascio per WindowsFormsExplorer.

## Struttura

```
Releases/
├── README.md (questo file)
└── v1.0.0/
    ├── WindowsFormsExplorer.exe
    ├── WindowsFormsExplorer.pdb
    └── README.md (note di rilascio)
```

## Come creare un nuovo rilascio

1. Compilare il progetto in modalità **Release**
2. Creare una nuova cartella con il numero di versione (es. `v1.0.0`)
3. Copiare i file necessari dalla cartella `bin/Release` del progetto UI
4. Creare un file `README.md` nella cartella della versione con le note di rilascio
5. Creare un tag Git con il numero di versione
6. Caricare i file come **GitHub Release** usando l'interfaccia web di GitHub

## File da includere

- `WindowsFormsExplorer.exe` - Eseguibile principale
- `WindowsFormsExplorer.pdb` - File di debug (opzionale)
- Tutte le DLL dipendenti necessarie
- Eventuali file di configurazione

## Note

I file di rilascio NON sono versionati nel repository. Questa cartella serve solo come riferimento locale per preparare i rilasci GitHub.

Per creare un rilascio GitHub:
1. Vai su GitHub → Releases → "Draft a new release"
2. Seleziona il tag Git corrispondente
3. Aggiungi le note di rilascio
4. Carica i file dalla cartella della versione

