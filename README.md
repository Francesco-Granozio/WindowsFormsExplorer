# WindowsFormsExplorer

Strumento per esplorare la struttura UI di applicazioni Windows Forms mentre sono in debug, utilizzando Visual Studio e EnvDTE.

## Caratteristiche

- 🔍 Esplora la gerarchia dei controlli di form Windows Forms in debug
- 📊 Visualizza tutte le proprietà dei controlli in tempo reale
- 🌳 Navigazione ad albero con supporto per controlli nidificati
- ⚡ Ottimizzato per performance con batch queries e caching

## Requisiti

- .NET Framework 4.7.2 o superiore
- Syncfusion Essential Studio per WinForms (versione 30.1.37)
- Visual Studio installato (per EnvDTE)

## Utilizzo

1. Avvia l'applicazione
2. Clicca su "Connect" per connetterti a Visual Studio
3. Seleziona l'istanza di Visual Studio desiderata
4. Seleziona il processo in debug
5. Scegli una form dalla lista per visualizzare la struttura dei controlli
6. Clicca su un controllo nell'albero per vedere le sue proprietà

## Rilasci

I file di rilascio sono disponibili nella cartella [Releases](Releases/) o come [GitHub Releases](https://github.com/your-username/WindowsFormsExplorer/releases).

## Sviluppo

Per compilare il progetto:

1. Clona il repository
2. Apri la soluzione `WindowsFormsExplorer.sln`
3. Restaura i pacchetti NuGet
4. Compila in modalità Release

## Licenza

Vedi il file [LICENSE.txt](LICENSE.txt) per i dettagli.