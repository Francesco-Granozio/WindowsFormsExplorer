# Checklist per Creare un Nuovo Rilascio

## Prima del Rilascio

- [ ] Testare l'applicazione in modalità Release
- [ ] Verificare che tutte le dipendenze siano incluse
- [ ] Controllare che il file `.config` sia presente e corretto
- [ ] Testare l'applicazione su un sistema pulito (senza Visual Studio/Syncfusion installati)
- [ ] Verificare che non ci siano riferimenti a percorsi locali hardcoded

## Preparazione File

- [ ] Compilare la soluzione in modalità **Release**
- [ ] Creare una nuova cartella `vX.X.X/` nella cartella `Releases/`
- [ ] Copiare i seguenti file dalla cartella `bin/Release/`:
  - [ ] `WindowsFormsExplorer.exe`
  - [ ] `WindowsFormsExplorer.exe.config`
  - [ ] `WindowsFormsExplorer.Core.dll`
  - [ ] `WindowsFormsExplorer.Infrastructure.dll`
  - [ ] Tutte le DLL Syncfusion necessarie:
    - [ ] `Syncfusion.Core.WinForms.dll`
    - [ ] `Syncfusion.SfDataGrid.WinForms.dll`
    - [ ] `Syncfusion.Tools.Windows.dll`
    - [ ] `Syncfusion.Shared.Base.dll`
    - [ ] `Syncfusion.Shared.Windows.dll`
    - [ ] Altre DLL Syncfusion dipendenti

## Documentazione

- [ ] Creare o aggiornare `Releases/vX.X.X/README.md` con:
  - [ ] Note di rilascio
  - [ ] Nuove funzionalità
  - [ ] Bug fix
  - [ ] Limitazioni note
  - [ ] Requisiti di sistema
  - [ ] Istruzioni di installazione

## Git e GitHub

- [ ] Assicurarsi che tutti i commit siano stati pushati
- [ ] Creare un tag Git: `git tag vX.X.X`
- [ ] Push del tag: `git push origin vX.X.X`
- [ ] Creare un GitHub Release:
  - [ ] Vai su GitHub → Releases → "Draft a new release"
  - [ ] Seleziona il tag appena creato
  - [ ] Titolo: `WindowsFormsExplorer vX.X.X`
  - [ ] Copia le note di rilascio dal README
  - [ ] Carica tutti i file dalla cartella `vX.X.X/` come file binari
  - [ ] Pubblica il rilascio

## Verifica Post-Rilascio

- [ ] Testare il download dal GitHub Release
- [ ] Verificare che l'applicazione si avvii correttamente
- [ ] Testare le funzionalità principali
- [ ] Aggiornare il README principale del repository se necessario

## Note Aggiuntive

- I file `.pdb` (debug symbols) sono opzionali ma utili per il debugging
- Se aggiungi file `.pdb`, includili separatamente o in un archivio zip separato
- Considera di creare anche un archivio ZIP con tutti i file per facilitare il download

