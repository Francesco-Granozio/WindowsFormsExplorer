using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace WindowsFormsExplorer
{
    public partial class MainForm : Form
    {
        private EnvDTE80.DTE2 dte;
        private System.Diagnostics.Process targetProcess;



        public MainForm()
        {
            InitializeComponent();

            this.txtPID.Text = "11708";

            //non so perchè ma il designer le rimuove
            this.listViewForms.Columns.Add("Nome", 150);
            this.listViewForms.Columns.Add("Tipo", 200);
            this.listViewForms.Columns.Add("Testo", 200);
            this.listViewForms.Columns.Add("Visibile", 100);
            this.listViewForms.Columns.Add("Handle", 100);
        }


        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable pprot);

        [DllImport("ole32.dll")]
        private static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);


        private void MainForm_Load(object sender, EventArgs e)
        {
            MessageFilter.Register();
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
            try
            {
                targetProcess = System.Diagnostics.Process.GetProcessById(pid);

                // Enumerare tutte le istanze di Visual Studio attive
                List<EnvDTE80.DTE2> visualStudioInstances = GetRunningVisualStudioInstances();

                if (visualStudioInstances.Count == 0)
                {
                    MessageBox.Show("Nessuna istanza di Visual Studio trovata.");
                    return;
                }

                // Mostrare una finestra di dialogo per scegliere l'istanza
                EnvDTE80.DTE2 selectedDTE = ChooseVisualStudioInstance(visualStudioInstances);

                if (selectedDTE == null)
                {
                    MessageBox.Show("Nessuna istanza selezionata.");
                    return;
                }

                dte = selectedDTE;

                // Verifica che il processo sia in debug
                bool isDebugging = false;
                foreach (EnvDTE80.Process2 proc in dte.Debugger.DebuggedProcesses)
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
            catch (COMException ex)
            {
                MessageBox.Show("Errore durante la connessione al debugger: " + ex.Message);
            }
        }

        private List<EnvDTE80.DTE2> GetRunningVisualStudioInstances()
        {
            List<EnvDTE80.DTE2> instances = new List<EnvDTE80.DTE2>();

            GetRunningObjectTable(0, out IRunningObjectTable rot);
            rot.EnumRunning(out IEnumMoniker enumMoniker);
            enumMoniker.Reset();

            IMoniker[] monikers = new IMoniker[1];
            IntPtr fetched = IntPtr.Zero;
            while (enumMoniker.Next(1, monikers, fetched) == 0)
            {
                CreateBindCtx(0, out IBindCtx bindCtx);
                monikers[0].GetDisplayName(bindCtx, null, out string displayName);

                if (displayName.StartsWith("!VisualStudio.DTE"))
                {
                    rot.GetObject(monikers[0], out object obj);
                    if (obj is EnvDTE80.DTE2 dte)
                    {
                        instances.Add(dte);
                    }
                }
            }

            return instances;
        }

        private EnvDTE80.DTE2 ChooseVisualStudioInstance(List<EnvDTE80.DTE2> instances)
        {
            string[] instanceNames = new string[instances.Count];
            for (int i = 0; i < instances.Count; i++)
            {
                instanceNames[i] = $"Instance {i + 1}: {instances[i].Solution?.FullName ?? "Senza soluzione aperta"}";
            }

            using (Form form = new Form())
            {
                ListBox listBox = new ListBox();
                listBox.Items.AddRange(instanceNames);
                listBox.Dock = DockStyle.Fill;
                form.Controls.Add(listBox);
                form.Text = "Seleziona un'istanza di Visual Studio";
                form.ClientSize = new System.Drawing.Size(400, 300);

                Button okButton = new Button() { Text = "OK", DialogResult = DialogResult.OK, Dock = DockStyle.Bottom };
                form.Controls.Add(okButton);
                form.AcceptButton = okButton;

                if (form.ShowDialog() == DialogResult.OK && listBox.SelectedIndex >= 0)
                {
                    return instances[listBox.SelectedIndex];
                }
            }

            return null;
        }


        private void RefreshOpenForms()
        {
            listViewForms.Items.Clear();

            try
            {
                // Ottieni il processo debuggato corretto
                EnvDTE.Process debuggedProcess = null;
                foreach (EnvDTE.Process proc in dte.Debugger.DebuggedProcesses)
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
                    EnvDTE.Processes threads = debuggedProcess.Collection;
                    if (threads.Count <= 0)
                    {
                        throw new Exception("Nessun thread disponibile nel processo!");
                    }

                    // Seleziona il primo thread
                    EnvDTE.Process mainThread = threads.Item(1);

                    // Il debugger deve essere in pausa
                    if (dte.Debugger.CurrentMode != EnvDTE.dbgDebugMode.dbgBreakMode)
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

                        EnvDTE.Expression formExpr = dte.Debugger.GetExpression(baseExpr);
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
            if (listViewForms.SelectedItems.Count <= 0)
            {
                return;
            }

            int formIndex = listViewForms.SelectedItems[0].Index;
            ExploreFormControls(formIndex);
        }


        private void ExploreFormControls(int formIndex)
        {
            Cursor.Current = Cursors.WaitCursor;

            treeViewControls.Nodes.Clear();

            try
            {
                string baseExpr = $"System.Windows.Forms.Application.OpenForms[{formIndex}]";
                EnvDTE.Expression formExpr = dte.Debugger.GetExpression(baseExpr);

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
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }


        private void ExploreControlsRecursively(string controlExpr, TreeNode parentNode)
        {

            try
            {
                // Ottengo la collezione Controls
                string controlsStr = GetExpressionValue($"{controlExpr}.Controls");

                if (controlsStr == null)
                {
                    return;
                }

                // Ottengo il Count dei controlli
                string controlCountStr = GetExpressionValue($"{controlExpr}.Controls.Count");

                if (!int.TryParse(controlCountStr, out int controlCount))
                {
                    return;
                }

                for (int i = 0; i < controlCount; i++)
                {
                    string childExpr = $"{controlExpr}.Controls[{i}]";

                    // Ottengo informazioni sul controllo
                    string name = GetExpressionValue($"{childExpr}.Name") ?? $"Control_{i}";
                    string type = GetExpressionValue($"{childExpr}.GetType().Name") ?? "Unknown";
                    string text = GetExpressionValue($"{childExpr}.Text") ?? "";
                    bool visible = GetExpressionValue($"{childExpr}.Visible") == "true";

                    // Creo il nodo per questo controllo
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

                    // Aggungo proprietà aggiuntive (abbastanza oneroso da vedere magari con un sistema di Lazy Loading
                    //AddControlProperties(childExpr, node);

                    // Ricorsione sui controlli figli
                    ExploreControlsRecursively(childExpr, node);

                    parentNode.Nodes.Add(node);
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
                string[] properties = new[]
                {
                    "Location", "Size", "Dock", "Anchor", "TabIndex", "Enabled",
                    "BackColor", "ForeColor", "Font", "Padding", "Margin"
                };

                foreach (string prop in properties)
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
                    EnvDTE.Expression expr = dte.Debugger.GetExpression(expression, false, 30000);
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
            try
            {
                base.OnFormClosing(e);

                if (dte != null)
                {
                    Marshal.ReleaseComObject(dte);
                    dte = null;
                }
            }
            finally
            {
                MessageFilter.Revoke();

            }
        }




    }

}
