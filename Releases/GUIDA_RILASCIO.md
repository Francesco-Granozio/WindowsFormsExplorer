# Guida Completa per Creare un Rilascio GitHub

## 📋 Passo 1: Preparazione File (GIÀ FATTO ✅)

I file sono già nella cartella `Releases/v1.0.0/`. Tutti i file necessari sono pronti.

---

## 🏷️ Passo 2: Creare un Tag Git

Hai due opzioni:

### Opzione A: Usando la Linea di Comando Git

1. **Apri PowerShell o Git Bash** nella cartella del progetto

2. **Assicurati di essere nella branch corretta** (di solito `main` o `master`):
   ```bash
   git status
   git checkout main  # o master, se quella è la tua branch principale
   ```

3. **Fai commit di eventuali modifiche pendenti**:
   ```bash
   git add .
   git commit -m "Preparazione rilascio v1.0.0"
   ```

4. **Crea il tag**:
   ```bash
   git tag v1.0.0
   ```

5. **Push del tag su GitHub**:
   ```bash
   git push origin v1.0.0
   ```

### Opzione B: Usando Visual Studio

1. **Apri Visual Studio** e il progetto
2. Vai su **Team Explorer** → **Tags**
3. Click destro → **New Tag**
4. Nome: `v1.0.0`
5. Click su **Create Tag**
6. Click destro sul tag → **Push Tag**

---

## 📦 Passo 3: Creare il GitHub Release

### Metodo Completo Passo-Passo:

1. **Vai sul tuo repository GitHub** nel browser
   - URL tipo: `https://github.com/tuo-username/WindowsFormsExplorer`

2. **Clicca su "Releases"** (di solito sulla destra, nella sezione "About")

3. **Clicca su "Draft a new release"** (pulsante verde)

4. **Compila i campi**:
   - **Choose a tag**: Seleziona `v1.0.0` (se l'hai già creato) oppure digita `v1.0.0` per crearne uno nuovo
   - **Release title**: `WindowsFormsExplorer v1.0.0`
   - **Description**: Copia e incolla il contenuto del file `Releases/v1.0.0/README.md`

5. **Carica i file**:
   - **Opzione A - File singoli**: 
     - Clicca su "Attach binaries by dropping them here or selecting them"
     - Trascina tutti i file dalla cartella `Releases/v1.0.0/` o selezionali
     - Carica almeno: `WindowsFormsExplorer.exe` e `WindowsFormsExplorer.exe.config`
   
   - **Opzione B - Archivio ZIP** (consigliato):
     - Crea un file ZIP con tutti i file dalla cartella `Releases/v1.0.0/`
     - Carica il file ZIP (più semplice per l'utente finale)

6. **Spunta "Set as the latest release"** (se è il primo rilascio)

7. **Clicca su "Publish release"** (pulsante verde in basso)

---

## 📝 Esempio di Contenuto per la Description

Copia questo nel campo Description del GitHub Release:

```markdown
# WindowsFormsExplorer v1.0.0

## Prima Versione Pubblica

Questa è la prima versione pubblica di WindowsFormsExplorer.

### Funzionalità Principali

- 🔍 **Esplorazione Struttura UI**: Esplora la gerarchia completa dei controlli di form Windows Forms mentre sono in debug
- 📊 **Visualizzazione Proprietà**: Visualizza tutte le proprietà pubbliche dei controlli in tempo reale
- 🌳 **Navigazione ad Albero**: Naviga la struttura dei controlli con un albero colorato e stilizzato
- ⚡ **Performance Ottimizzate**: Utilizza batch queries e caching per migliorare le performance con EnvDTE
- 🎨 **UI Moderna**: Interfaccia utente moderna e intuitiva con controlli Syncfusion

### Requisiti di Sistema

- **Sistema Operativo**: Windows 7 o superiore
- **.NET Framework**: 4.7.2 o superiore
- **Visual Studio**: Installato (richiesto per EnvDTE)

### Installazione

1. Scarica e estrai tutti i file
2. Assicurati di avere .NET Framework 4.7.2 o superiore
3. Esegui `WindowsFormsExplorer.exe`

### Utilizzo

1. Avvia l'applicazione: Esegui `WindowsFormsExplorer.exe`
2. Connetti a Visual Studio: Clicca su "Connect" e seleziona l'istanza di Visual Studio
3. Visualizza le Form: Dopo la connessione, le form aperte appariranno nella tabella
4. Esplora i Controlli: Seleziona una form per visualizzare la struttura dei controlli
5. Ispeziona Proprietà: Clicca su un controllo per vedere tutte le sue proprietà

### Limitazioni Note

- Richiede che l'applicazione target sia in modalità debug e fermata
- Non è possibile ispezionare eventi (limitazione di EnvDTE)
- Alcune proprietà potrebbero non essere valutabili in determinati contesti

### Note Importanti

⚠️ **Licenza Syncfusion**: Le DLL di Syncfusion incluse sono fornite solo per scopi di valutazione/dimostrazione. Per uso commerciale, è necessario avere una licenza valida di Syncfusion Essential Studio.
```

---

## 🎯 Comandi Rapidi (Copia e Incolla)

Se preferisci usare la linea di comando Git, ecco i comandi in sequenza:

```bash
# Assicurati di essere nella root del progetto
cd C:\Shared\CSharp\WindowsFormsExplorer

# Verifica lo stato
git status

# Fai commit delle modifiche se necessario
git add .
git commit -m "Preparazione rilascio v1.0.0"

# Crea il tag
git tag v1.0.0

# Push del tag
git push origin v1.0.0

# Push delle modifiche (se ci sono commit nuovi)
git push origin main
```

---

## ✅ Verifica Post-Rilascio

Dopo aver pubblicato il rilascio:

1. Vai alla pagina **Releases** del tuo repository
2. Verifica che il rilascio sia visibile
3. Clicca sul rilascio per vedere i dettagli
4. Testa il download del file ZIP/exe per assicurarti che funzioni

---

## 🆘 Troubleshooting

### Il tag non appare su GitHub
- Assicurati di aver fatto `git push origin v1.0.0` e non solo `git push`

### Non vedo "Releases" nel menu
- Potrebbe essere un repository nuovo. Crea un primo rilascio e la sezione apparirà automaticamente

### Errore durante il push del tag
- Verifica di avere i permessi di scrittura sul repository
- Assicurati di essere autenticato: `git config --global user.name` e `git config --global user.email`

