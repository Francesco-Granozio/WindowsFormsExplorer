using EnvDTE;
using EnvDTE80;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsExplorer
{
    public partial class MainForm : Form
    {
        private DTE2 debuggerInstance;
        private System.Diagnostics.Process targetProcess;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                int pid;
                if (!int.TryParse(txtPID.Text, out pid))
                {
                    MessageBox.Show("PID non valido!");
                    return;
                }

                ConnectToProcess(pid);
                RefreshOpenForms();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante la connessione: {ex.Message}");
            }
        }

        private void ConnectToProcess(int pid)
        {
            // Ottiene il processo target
            targetProcess = System.Diagnostics.Process.GetProcessById(pid);
            if (targetProcess == null)
            {
                throw new Exception("Processo non trovato!");
            }

            // Ottiene l'istanza del debugger
            string progID = "VisualStudio.DTE.17.0"; // Modificare in base alla versione di VS
            object obj = Marshal.GetActiveObject(progID);
            debuggerInstance = obj as DTE2;

            if (debuggerInstance == null)
            {
                throw new Exception("Impossibile connettersi al debugger!");
            }

            // Verifica che il processo sia in debug
            bool isDebugging = false;
            foreach (Process2 proc in debuggerInstance.Debugger.DebuggedProcesses)
            {
                if (proc.ProcessID == pid)
                {
                    isDebugging = true;
                    break;
                }
            }

            if (!isDebugging)
            {
                throw new Exception("Il processo specificato non è in modalità debug!");
            }
        }

        private void RefreshOpenForms()
        {
            listViewForms.Items.Clear();

            try
            {
                // Ottieni il processo debuggato corretto
                EnvDTE.Process debuggedProcess = null;
                foreach (EnvDTE.Process proc in debuggerInstance.Debugger.DebuggedProcesses)
                {
                    if (proc.ProcessID == targetProcess.Id)
                    {
                        debuggedProcess = proc;
                        break;
                    }
                }

                if (debuggedProcess == null)
                {
                    throw new Exception("Processo debuggato non trovato!");
                }

                // Accedi alla collezione threads in modo sicuro
                try
                {
                    // Selezioniamo il primo thread disponibile
                    Processes threads = debuggedProcess.Collection;
                    if (threads.Count > 0)
                    {
                        // Seleziona il primo thread
                        EnvDTE.Process mainThread = threads.Item(1);

                        // Assicuriamoci che il debugger sia in pausa
                        if (debuggerInstance.Debugger.CurrentMode == dbgDebugMode.dbgBreakMode)
                        {
                            // Valuta l'espressione per ottenere il conteggio dei forms
                            Expression countExpr = debuggerInstance.Debugger.GetExpression(
                                "System.Windows.Forms.Application.OpenForms.Count",
                                false, // No side effects
                                30000 // 30 secondi timeout
                            );

                            if (countExpr != null && !string.IsNullOrEmpty(countExpr.Value))
                            {
                                int formCount;
                                if (int.TryParse(countExpr.Value, out formCount))
                                {
                                    for (int i = 0; i < formCount; i++)
                                    {
                                        string baseExpr = $"System.Windows.Forms.Application.OpenForms[{i}]";

                                        Expression formExpr = debuggerInstance.Debugger.GetExpression(baseExpr);
                                        if (formExpr != null)
                                        {
                                            string text = GetExpressionValue($"{baseExpr}.Text") ?? "N/A";
                                            string type = formExpr.Type ?? "N/A";
                                            string visible = GetExpressionValue($"{baseExpr}.Visible") ?? "N/A";
                                            string handle = GetExpressionValue($"{baseExpr}.Handle.ToInt32()") ?? "N/A";
                                            string name = GetExpressionValue($"{baseExpr}.Name") ?? $"Form_{i}";

                                            ListViewItem item = new ListViewItem(new string[] {
                                                name,
                                                type,
                                                text,
                                                visible,
                                                handle
                                            });
                                            listViewForms.Items.Add(item);
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception($"Valore conteggio non valido: {countExpr.Value}");
                                }
                            }
                            else
                            {
                                throw new Exception("Impossibile ottenere il conteggio dei forms!");
                            }
                        }
                        else
                        {
                            throw new Exception("Il debugger deve essere in modalità break!");
                        }
                    }
                    else
                    {
                        throw new Exception("Nessun thread disponibile nel processo!");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Errore nell'accesso ai threads: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante la lettura dei forms: {ex.Message}\n{ex.StackTrace}");
            }
        }



        private string GetExpressionValue(string expression)
        {
            try
            {
                Expression expr = debuggerInstance.Debugger.GetExpression(
                    expression,
                    false,  // No side effects
                    30000   // Timeout 30 secondi
                );
                return expr?.Value;
            }
            catch
            {
                return null;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshOpenForms();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante l'aggiornamento: {ex.Message}");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (debuggerInstance != null)
            {
                Marshal.ReleaseComObject(debuggerInstance);
            }
        }
    }

}
