using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsExplorer.Core.Common;
using WindowsFormsExplorer.Core.Domain;
using WindowsFormsExplorer.Core.Interfaces;
using WindowsFormsExplorer.Infrastructure.Debugger;
using WindowsFormsExplorer.Infrastructure.Discovery;

namespace WindowsFormsExplorer.UI.Forms
{
    public partial class MainForm : Form
    {
        private bool _canLoad = false;
        private IDebuggerService _debuggerService = null;
        private readonly IVisualStudioDiscovery _discoveryService;
        private BindingList<FormInfoRow> _formData;

        public MainForm()
        {
            InitializeComponent();
            _discoveryService = new VisualStudioDiscoveryService();
            _formData = new BindingList<FormInfoRow>();
            
            // Configura il DataGrid e TreeView
            ConfigureDataGrid();
            ConfigureTreeView();
        }

        private void ConfigureTreeView()
        {
            // Configura stile e aspetto del TreeView
            treeViewAdv1.Style = Syncfusion.Windows.Forms.Tools.TreeStyle.Office2016Colorful;
            treeViewAdv1.ShowLines = true;
            treeViewAdv1.ShowPlusMinus = true;
            treeViewAdv1.ShowRootLines = true;
            treeViewAdv1.FullRowSelect = true;
            treeViewAdv1.Font = new Font("Segoe UI", 9.5F);
            treeViewAdv1.ItemHeight = 28;
            
            // Colori per selezione
            treeViewAdv1.SelectedNodeBackground = new Syncfusion.Drawing.BrushInfo(Color.FromArgb(22, 165, 220));
            treeViewAdv1.SelectedNodeForeColor = Color.White;
        }

        private void ConfigureDataGrid()
        {
            sfDataGrid1.DataSource = _formData;
            sfDataGrid1.AutoGenerateColumns = false;
            
            // Pulisci colonne esistenti prima di aggiungerne di nuove
            sfDataGrid1.Columns.Clear();
            
            // Definisci colonne personalizzate
            sfDataGrid1.Columns.Add(new Syncfusion.WinForms.DataGrid.GridTextColumn()
            {
                MappingName = "Name",
                HeaderText = "Name",
                Width = 150
            });
            
            sfDataGrid1.Columns.Add(new Syncfusion.WinForms.DataGrid.GridTextColumn()
            {
                MappingName = "Type",
                HeaderText = "Type",
                Width = 200
            });
            
            sfDataGrid1.Columns.Add(new Syncfusion.WinForms.DataGrid.GridTextColumn()
            {
                MappingName = "Text",
                HeaderText = "Text",
                Width = 250
            });
            
            sfDataGrid1.Columns.Add(new Syncfusion.WinForms.DataGrid.GridTextColumn()
            {
                MappingName = "Visible",
                HeaderText = "Visible",
                Width = 80
            });
            
            sfDataGrid1.Columns.Add(new Syncfusion.WinForms.DataGrid.GridTextColumn()
            {
                MappingName = "Handle",
                HeaderText = "Handle",
                Width = 100
            });
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                _canLoad = false;
                await ConnectToProcessAsync();

                if (_canLoad)
                {
                    await RefreshOpenFormsAsync();
                }
                else
                {
                    MessageBox.Show("You must first connect to the debugger", 
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while connecting to debugger: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ConnectToProcessAsync()
        {
            try
            {
                // Ottiene le istanze di Visual Studio
                Result<List<VisualStudioInstance>> vsInstancesResult = 
                    _discoveryService.GetRunningInstances();

                vsInstancesResult.Match(
                    onSuccess: _ => { },
                    onFailure: error =>
                    {
                        if (error.IsIn(ErrorCode.Exception, ErrorCode.DebuggerConnectionError))
                            MessageBox.Show(error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else if (error.Is(ErrorCode.NoVSInstanceFound))
                            MessageBox.Show(error.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                );

                if (vsInstancesResult.IsFailure)
                    return;

                // Mostra dialog per selezione istanza VS
                VSInstanceSelectorForm vsSelector = new VSInstanceSelectorForm(vsInstancesResult.Value);
                if (vsSelector.ShowDialog() == DialogResult.Cancel)
                    return;

                VisualStudioInstance selectedInstance = vsSelector.SelectedInstance;

                // Ottiene i processi in debug
                Result<List<DebugProcess>> processesResult = 
                    _discoveryService.GetDebugProcesses(selectedInstance);

                if (processesResult.IsFailure)
                {
                    MessageBox.Show(processesResult.Error.Message, "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (processesResult.Value.Count == 0)
                {
                    MessageBox.Show("No debug processes found in the selected instance.", 
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Mostra dialog per selezione processo
                ProcessSelectorForm processSelector = new ProcessSelectorForm(processesResult.Value);
                if (processSelector.ShowDialog() == DialogResult.Cancel)
                    return;

                DebugProcess selectedProcess = processSelector.SelectedProcess;

                // Crea il servizio di debugging ottimizzato
                _debuggerService?.Dispose();
                _debuggerService = new EnvDteDebuggerService(
                    selectedProcess.NativeProcess,
                    selectedInstance.NativeInstance
                );

                _canLoad = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while connecting to debugger: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task RefreshOpenFormsAsync()
        {
            _formData.Clear();
            SetControlsEnabled(false);

            try
            {
                if (_debuggerService == null)
                {
                    MessageBox.Show("Debugger not connected!", "Warning", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Verifica che possiamo interrogare il debugger
                Result<int> canQueryResult = _debuggerService.CanQuery();

                canQueryResult.Match(
                    onSuccess: _ => { },
                    onFailure: error =>
                    {
                        if (error.Is(ErrorCode.Exception))
                            MessageBox.Show(error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else if (error.IsIn(
                            ErrorCode.NoThreadsAvailableInProcess,
                            ErrorCode.DebuggerMustBePaused,
                            ErrorCode.UnableToGetFormCount,
                            ErrorCode.InvalidFormCountValue))
                            MessageBox.Show(error.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                );

                if (canQueryResult.IsFailure)
                    return;

                // OTTIMIZZAZIONE: Carica le form in modo asincrono
                var stopwatch = Stopwatch.StartNew();

                Result<List<ControlInfo>> formsResult = await _debuggerService.GetOpenFormsAsync();

                stopwatch.Stop();
                Debug.WriteLine($"Loaded {formsResult.Value?.Count ?? 0} forms in {stopwatch.ElapsedMilliseconds}ms");

                if (formsResult.IsSuccess)
                {
                    foreach (var form in formsResult.Value)
                    {
                        _formData.Add(new FormInfoRow
                        {
                            Name = form.Name,
                            Type = form.Type,
                            Text = form.Text,
                            Visible = form.Visible ? "true" : "false",
                            Handle = form.Handle,
                            Expression = form.Expression
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while reading forms: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

        private async void sfDataGrid1_SelectionChanged(object sender, Syncfusion.WinForms.DataGrid.Events.SelectionChangedEventArgs e)
        {
            if (sfDataGrid1.SelectedIndex < 0 || sfDataGrid1.SelectedIndex >= _formData.Count)
                return;

            await ExploreFormControlsAsync(sfDataGrid1.SelectedIndex);
        }

        private async Task ExploreFormControlsAsync(int formIndex)
        {
            Cursor.Current = Cursors.WaitCursor;
            treeViewAdv1.Nodes.Clear();
            SetControlsEnabled(false);

            try
            {
                var formData = _formData[formIndex];
                string baseExpr = formData.Expression;

                Syncfusion.Windows.Forms.Tools.TreeNodeAdv rootNode = new Syncfusion.Windows.Forms.Tools.TreeNodeAdv(formData.Name)
                {
                    Tag = baseExpr,
                    Expanded = true
                };

                // OTTIMIZZAZIONE: Esplora i controlli in modo asincrono
                var stopwatch = Stopwatch.StartNew();

                Result<ControlInfo> controlsResult = await _debuggerService.ExploreControlsAsync(baseExpr);

                stopwatch.Stop();
                Debug.WriteLine($"Explored controls in {stopwatch.ElapsedMilliseconds}ms");

                if (controlsResult.IsSuccess)
                {
                    BuildTreeNodes(rootNode, controlsResult.Value.Children);
                }

                treeViewAdv1.Nodes.Add(rootNode);
                rootNode.Expand();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while exploring controls: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                SetControlsEnabled(true);
            }
        }

        private void BuildTreeNodes(Syncfusion.Windows.Forms.Tools.TreeNodeAdv parentNode, List<ControlInfo> controls)
        {
            foreach (var control in controls)
            {
                // Crea testo formattato con informazioni principali
                string nodeText = FormatNodeText(control);
                
                Syncfusion.Windows.Forms.Tools.TreeNodeAdv node = new Syncfusion.Windows.Forms.Tools.TreeNodeAdv(nodeText)
                {
                    Tag = control.Expression,
                    ExpandImageIndex = -1,
                    CollapseImageIndex = -1
                };

                // Applica stile basato sul tipo di controllo
                ApplyNodeStyle(node, control);

                // Ricorsione per i controlli figli
                if (control.Children != null && control.Children.Count > 0)
                {
                    BuildTreeNodes(node, control.Children);
                }

                parentNode.Nodes.Add(node);
            }
        }

        private string FormatNodeText(ControlInfo control)
        {
            // Crea un testo ben formattato con tutte le informazioni
            string text = $"{control.Name}";
            
            // Aggiungi tipo con padding
            text += $" [{control.Type}]";
            
            // Aggiungi testo se presente
            if (!string.IsNullOrEmpty(control.Text))
            {
                string displayText = control.Text;
                if (displayText.Length > 40)
                    displayText = displayText.Substring(0, 37) + "...";
                text += $" - \"{displayText}\"";
            }
            
            // Aggiungi indicatore visibilità
            if (!control.Visible)
            {
                text += " [HIDDEN]";
            }
            
            return text;
        }

        private void ApplyNodeStyle(Syncfusion.Windows.Forms.Tools.TreeNodeAdv node, ControlInfo control)
        {
            // Colori e font basati sul tipo di controllo
            var style = GetControlStyle(control.Type);
            
            node.Font = style.Font;
            node.TextColor = style.ForeColor;
            
            // Se il controllo è nascosto, usa stile diverso
            if (!control.Visible)
            {
                node.TextColor = Color.Gray;
                node.Font = new Font(node.Font, FontStyle.Italic);
            }
        }

        private ControlStyle GetControlStyle(string controlType)
        {
            // Definisci stili per tipo di controllo
            if (controlType.Contains("Button"))
                return new ControlStyle 
                { 
                    ForeColor = Color.FromArgb(0, 120, 215),    // Blu
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
                };
            
            if (controlType.Contains("Label"))
                return new ControlStyle 
                { 
                    ForeColor = Color.FromArgb(16, 124, 16),    // Verde scuro
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Regular)
                };
            
            if (controlType.Contains("TextBox") || controlType.Contains("RichTextBox") || controlType.Contains("MaskedTextBox"))
                return new ControlStyle 
                { 
                    ForeColor = Color.FromArgb(139, 69, 19),    // Marrone
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Regular)
                };
            
            if (controlType.Contains("Panel") || controlType.Contains("GroupBox") || controlType.Contains("TabControl"))
                return new ControlStyle 
                { 
                    ForeColor = Color.FromArgb(75, 75, 75),     // Grigio scuro
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
                };
            
            if (controlType.Contains("ComboBox") || controlType.Contains("ListBox") || controlType.Contains("CheckedListBox"))
                return new ControlStyle 
                { 
                    ForeColor = Color.FromArgb(128, 0, 128),    // Viola
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Regular)
                };
            
            if (controlType.Contains("DataGrid") || controlType.Contains("ListView") || controlType.Contains("TreeView"))
                return new ControlStyle 
                { 
                    ForeColor = Color.FromArgb(0, 100, 100),    // Teal
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
                };
            
            if (controlType.Contains("PictureBox") || controlType.Contains("Image"))
                return new ControlStyle 
                { 
                    ForeColor = Color.FromArgb(255, 140, 0),    // Arancione scuro
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Regular)
                };
            
            // Default per altri controlli
            return new ControlStyle 
            { 
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular)
            };
        }

        // Classe helper per gli stili
        private class ControlStyle
        {
            public Color ForeColor { get; set; }
            public Font Font { get; set; }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (_canLoad)
                {
                    await RefreshOpenFormsAsync();
                }
                else
                {
                    MessageBox.Show("You must first connect to the debugger", 
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while updating: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            btnConnect.Enabled = enabled;
            btnRefresh.Enabled = enabled;
            sfDataGrid1.Enabled = enabled;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                base.OnFormClosing(e);
                _debuggerService?.Dispose();
            }
            catch { }
        }
    }

    // Classe helper per il DataBinding con SfDataGrid
    public class FormInfoRow
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string Visible { get; set; }
        public string Handle { get; set; }
        public string Expression { get; set; }
    }
}
