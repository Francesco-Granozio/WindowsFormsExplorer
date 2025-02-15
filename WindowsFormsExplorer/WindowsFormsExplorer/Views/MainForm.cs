using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Windows.Forms;
using System.Xml.Linq;
using WindowsFormsExplorer.Domain;
using WindowsFormsExplorer.Services;
using WindowsFormsExplorer.Utility;
using Debugger = WindowsFormsExplorer.Services.Debugger;

namespace WindowsFormsExplorer.Views
{
    public partial class MainForm : Form
    {
        private bool m_CanLoad = false;
        private readonly List<ControlInfo> m_FormInfos = new List<ControlInfo>();
        private Debugger m_Debugger = null;

        public MainForm()
        {
            InitializeComponent();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            MessageFilter.Register();
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                m_CanLoad = false;
                ConnectToProcess();

                if (m_CanLoad)
                {
                    RefreshOpenForms();
                }
                else
                {
                    MessageBox.Show($"You must first connect to the debugger", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while connecting to debugger: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        
        private void ConnectToProcess()
        {
            EnvDTE80.DTE2 dte;

            try
            {
                ProcessInspector processInspector = new ProcessInspector();

                Result<List<EnvDTE80.DTE2>> visualStudioInstancesResult = processInspector.GetVisualStudioInstances();

                visualStudioInstancesResult.Match(
                    onSuccess: _ => { },
                    onFailure: error =>
                    {
                        if (error.IsIn(ErrorCode.Exception, ErrorCode.DebuggerConnectionError))
                            MessageBox.Show(error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        else if (error.Is(ErrorCode.NoVSIstanceFound))
                            MessageBox.Show(error.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                );

                if (visualStudioInstancesResult.IsFailure)
                    return;


                VSInstanceSelectorForm vsInstanceSelectorForm = new VSInstanceSelectorForm(visualStudioInstancesResult.Value);

                if (vsInstanceSelectorForm.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                //Mostra una finestra di dialogo per scegliere l'istanza
                dte = vsInstanceSelectorForm.SelectedInstance;

                ProcessSelectorForm processSelectorForm = new ProcessSelectorForm(dte.Debugger.DebuggedProcesses);

                DialogResult dialogResult = processSelectorForm.ShowDialog();
                if (dialogResult == DialogResult.Cancel)
                {
                    return;
                }
                else if (dialogResult == DialogResult.Abort)
                {
                    MessageBox.Show("No debug processes found in the selected instance.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int selectedPID = processSelectorForm.SelectedProcess.ProcessID;

                // Verifico che il processo sia in debug
                // e ottengo il processo debuggato corretto
                bool isDebugging = false;
                foreach (EnvDTE80.Process2 proc in dte.Debugger.DebuggedProcesses)
                {
                    if (proc.ProcessID == selectedPID)
                    {
                        isDebugging = true;
                        m_Debugger = new Debugger(proc, dte);
                        m_CanLoad = true;
                        break;
                    }
                }

                if (!isDebugging)
                {
                    MessageBox.Show("Selected process MUST be in debug mode!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (COMException ex)
            {
                MessageBox.Show($"Error while connecting to debugger: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void RefreshOpenForms()
        {
            formsDataGridView.Rows.Clear();

            try
            {

                if (m_Debugger == null)
                {
                    MessageBox.Show("Debugger not connected!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Accedo alla collezione threads in modo sicuro
                try
                {
                    int formCount = 0;
                    Result<bool> canQueryDebugger = m_Debugger.CanQuery(ref formCount);

                    canQueryDebugger.Match(
                    onSuccess: _ => { },
                    onFailure: error =>
                    {
                        if (error.Is(ErrorCode.Exception))
                            MessageBox.Show(error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        else if (
                            error.IsIn(
                                ErrorCode.NoThreadsAvailableInProcess,
                                ErrorCode.DebuggerMustBePaused,
                                ErrorCode.UnableToGetFormCount,
                                ErrorCode.UnableToGetFormCount
                                )
                            )
                            MessageBox.Show(error.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    );

                    if (canQueryDebugger.IsFailure)
                        return;

                    foreach (ControlInfo formInfo in m_Debugger.GetFormInfo(formCount))
                    {
                        m_FormInfos.Add(formInfo);
                        formsDataGridView.Rows.Add(formInfo.Name, formInfo.Type, formInfo.Text, formInfo.Visible, formInfo.Handle);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error in accessing threads: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while reading forms: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void formsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (formsDataGridView.SelectedRows.Count <= 0)
            {
                return;
            }

            int formIndex = formsDataGridView.SelectedRows[0].Index;
            ExploreFormControls(formIndex);
        }


        private void ExploreFormControls(int formIndex)
        {
            Cursor.Current = Cursors.WaitCursor;

            treeViewControls.Nodes.Clear();

            try
            {
                string baseExpr = $"System.Windows.Forms.Application.OpenForms[{formIndex}]";
                string formExpressionStr = m_Debugger.GetExpressionValue(baseExpr);

                if (string.IsNullOrEmpty(formExpressionStr))
                    return;

                TreeNode rootNode = new TreeNode(m_Debugger.GetExpressionValue($"{baseExpr}.Name") ?? $"Form_{formIndex}")
                {
                    Tag = baseExpr
                };
                ExploreControlsRecursively(baseExpr, rootNode);
                treeViewControls.Nodes.Add(rootNode);
                rootNode.Expand();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while exploring controls: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }


        private void ExploreControlsRecursively(string controlExpr, TreeNode parentNode)
        {
            Stopwatch stopwatch = new Stopwatch();  // Stopwatch per misurare i tempi

            try
            {
                stopwatch.Start();  // Inizia il cronometro

                // Ottengo la collezione Controls
                string controlsStr = m_Debugger.GetExpressionValue($"{controlExpr}.Controls");

                if (controlsStr == null)
                {
                    return;
                }

                // Stampa per sapere che stiamo eseguendo l'operazione
                Console.WriteLine($"Exploring Controls for: {controlExpr}");

                // Ottengo il Count dei controlli
                string controlCountStr = m_Debugger.GetExpressionValue($"{controlExpr}.Controls.Count");

                if (!int.TryParse(controlCountStr, out int controlCount))
                {
                    return;
                }

                Stopwatch controlLoopStopwatch = new Stopwatch();  // Cronometro per il ciclo dei controlli
                controlLoopStopwatch.Start();

                for (int i = 0; i < controlCount; i++)
                {
                    string childExpr = $"{controlExpr}.Controls[{i}]";

                    // Ottengo informazioni sul controllo
                    string name = m_Debugger.GetExpressionValue($"{childExpr}.Name") ?? $"Control_{i}";
                    string type = m_Debugger.GetExpressionValue($"{childExpr}.GetType().Name") ?? "Unknown";
                    string text = m_Debugger.GetExpressionValue($"{childExpr}.Text") ?? "";
                    bool visible = m_Debugger.GetExpressionValue($"{childExpr}.Visible") == "true";

                    // Stampa il nome del controllo
                    Console.WriteLine($"Processing Control: {name}");

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

                    // Ricorsione sui controlli figli
                    ExploreControlsRecursively(childExpr, node);

                    parentNode.Nodes.Add(node);
                }

                controlLoopStopwatch.Stop();  // Ferma il cronometro per il ciclo dei controlli
                Console.WriteLine($"Time taken to explore controls for {controlExpr}: {controlLoopStopwatch.ElapsedMilliseconds} ms" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                TreeNode errorNode = new TreeNode($"Error: {ex.Message}");
                parentNode.Nodes.Add(errorNode);
            }
            finally
            {
                stopwatch.Stop();  // Ferma il cronometro per l'intero processo
                Console.WriteLine($"Total time taken for {controlExpr}: {stopwatch.ElapsedMilliseconds} ms");
            }
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_CanLoad)
                {
                    RefreshOpenForms();
                }
                else
                {
                    MessageBox.Show($"You must first connect to the debugger", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", $"Error while updating: {ex.Message}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                base.OnFormClosing(e);
                m_Debugger?.ReleaseDTE();
            }
            finally
            {
                MessageFilter.Revoke();

            }
        }


    }

}
