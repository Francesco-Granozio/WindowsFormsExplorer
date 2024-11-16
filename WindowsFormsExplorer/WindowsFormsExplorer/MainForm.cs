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


        private void MainForm_Load(object sender, EventArgs e)
        {
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtPID.Text, out int pid))
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
            //string progID = "VisualStudio.DTE.15.0"; // Visual studio 2017
            //string progID = "VisualStudio.DTE.16.0"; // Visual studio 2019
            string progID = "VisualStudio.DTE.17.0"; // Visual studio 2022

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

                // Accedo alla collezione threads in modo sicuro
                try
                {
                    // Seleziono il primo thread disponibile
                    Processes threads = debuggedProcess.Collection;
                    if (threads.Count <= 0)
                    {
                        throw new Exception("Nessun thread disponibile nel processo!");
                    }

                    // Seleziona il primo thread
                    EnvDTE.Process mainThread = threads.Item(1);

                    // Il debugger deve essere in pausa
                    if (debuggerInstance.Debugger.CurrentMode != dbgDebugMode.dbgBreakMode)
                    {
                        throw new Exception("Il debugger deve essere in modalità break!");
                    }


                    string formCountStr = GetExpressionValue("System.Windows.Forms.Application.OpenForms.Count");


                    if (string.IsNullOrEmpty(formCountStr))
                    {
                        throw new Exception("Impossibile ottenere il conteggio dei forms!");
                    }

                    if (!int.TryParse(formCountStr, out int formCount))
                    {
                        throw new Exception($"Valore conteggio non valido: {formCountStr}");
                    }

                    for (int i = 0; i < formCount; i++)
                    {
                        string baseExpr = $"System.Windows.Forms.Application.OpenForms[{i}]";

                        Expression formExpr = debuggerInstance.Debugger.GetExpression(baseExpr);
                        if (formExpr == null)
                        {
                            continue;
                        }

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


        private void listViewForms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewForms.SelectedItems.Count > 0)
            {
                int formIndex = listViewForms.SelectedItems[0].Index;
                ExploreFormControls(formIndex);
            }
        }


        private void ExploreFormControls(int formIndex)
        {
            treeViewControls.Nodes.Clear();

            try
            {
                string baseExpr = $"System.Windows.Forms.Application.OpenForms[{formIndex}]";
                Expression formExpr = debuggerInstance.Debugger.GetExpression(baseExpr);

                if (formExpr != null)
                {
                    TreeNode rootNode = new TreeNode(GetExpressionValue($"{baseExpr}.Name") ?? $"Form_{formIndex}")
                    {
                        Tag = baseExpr
                    };
                    ExploreControlsRecursively(baseExpr, rootNode);
                    treeViewControls.Nodes.Add(rootNode);
                    rootNode.Expand();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante l'esplorazione dei controlli: {ex.Message}");
            }
        }


        private void ExploreControlsRecursively(string controlExpr, TreeNode parentNode)
        {

            try
            {
                // Ottieni la collezione Controls
                Expression controlsExpr = debuggerInstance.Debugger.GetExpression(
                    $"{controlExpr}.Controls",
                    false,
                    30000
                );

                if (controlsExpr != null)
                {
                    // Ottieni il conteggio dei controlli
                    Expression countExpr = debuggerInstance.Debugger.GetExpression(
                        $"{controlExpr}.Controls.Count",
                        false,
                        30000
                    );

                    if (countExpr != null && int.TryParse(countExpr.Value, out int controlCount))
                    {
                        for (int i = 0; i < controlCount; i++)
                        {
                            string childExpr = $"{controlExpr}.Controls[{i}]";

                            // Ottieni informazioni sul controllo
                            string name = GetExpressionValue($"{childExpr}.Name") ?? $"Control_{i}";
                            string type = GetExpressionValue($"{childExpr}.GetType().Name") ?? "Unknown";
                            string text = GetExpressionValue($"{childExpr}.Text") ?? "";
                            bool visible = GetExpressionValue($"{childExpr}.Visible") == "true";

                            // Crea il nodo per questo controllo
                            string nodeText = $"{name} ({type})";
                            if (!string.IsNullOrEmpty(text))
                            {
                                nodeText += $" - '{text}'";
                            }
                            if (!visible)
                            {
                                nodeText += " [Hidden]";
                            }

                            TreeNode node = new TreeNode(nodeText)
                            {
                                Tag = childExpr
                            };

                            // Aggiungi proprietà aggiuntive
                            //AddControlProperties(childExpr, node);

                            // Ricorsione sui controlli figli
                            ExploreControlsRecursively(childExpr, node);

                            parentNode.Nodes.Add(node);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TreeNode errorNode = new TreeNode($"Error: {ex.Message}");
                parentNode.Nodes.Add(errorNode);
            }
        }


        private void AddControlProperties(string controlExpr, TreeNode node)
        {
            try
            {
                // Lista delle proprietà comuni da ispezionare
                var properties = new[]
                {
                    "Location", "Size", "Dock", "Anchor", "TabIndex", "Enabled",
                    "BackColor", "ForeColor", "Font", "Padding", "Margin"
                };

                foreach (var prop in properties)
                {
                    string value = GetExpressionValue($"{controlExpr}.{prop}");
                    if (!string.IsNullOrEmpty(value))
                    {
                        TreeNode propNode = new TreeNode($"{prop}: {value}");
                        node.Nodes.Add(propNode);
                    }
                }

                // Proprietà specifiche per diversi tipi di controlli
                string type = GetExpressionValue($"{controlExpr}.GetType().Name");
                switch (type)
                {
                    case "TextBox":
                        AddProperty(node, controlExpr, "MultiLine");
                        AddProperty(node, controlExpr, "MaxLength");
                        AddProperty(node, controlExpr, "ReadOnly");
                        break;
                    case "Button":
                        AddProperty(node, controlExpr, "DialogResult");
                        AddProperty(node, controlExpr, "FlatStyle");
                        break;
                    case "ComboBox":
                        AddProperty(node, controlExpr, "DropDownStyle");
                        AddProperty(node, controlExpr, "Items.Count");
                        AddProperty(node, controlExpr, "SelectedIndex");
                        break;
                        // altri casi specifici per tipo...
                }
            }
            catch (Exception ex)
            {
                TreeNode errorNode = new TreeNode($"Error getting properties: {ex.Message}");
                node.Nodes.Add(errorNode);
            }
        }


        private void AddProperty(TreeNode node, string controlExpr, string propertyName)
        {
            string value = GetExpressionValue($"{controlExpr}.{propertyName}");
            if (!string.IsNullOrEmpty(value))
            {
                TreeNode propNode = new TreeNode($"{propertyName}: {value}");
                node.Nodes.Add(propNode);
            }
        }


        private string GetExpressionValue(string expression)
        {
            int retryCount = 3;
            int delayBetweenRetries = 100; // Millisecondi

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    Expression expr = debuggerInstance.Debugger.GetExpression(expression, false, 300000);
                    return expr?.Value;
                }
                catch (COMException ex) when ((uint)ex.HResult == 0x80010001) // RPC_E_CALL_REJECTED
                {
                    if (i == retryCount - 1)
                    {
                        Console.WriteLine($"Eccezione durante il recupero dell'espressione: {ex.Message}");
                        throw; // Rilancia l'eccezione se siamo all'ultimo tentativo
                    }

                    // Aspetta prima del prossimo tentativo
                    System.Threading.Thread.Sleep(delayBetweenRetries);
                }
            }
            return null;
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
