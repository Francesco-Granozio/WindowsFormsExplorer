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
        private BindingList<PropertyRow> _propertyData;

        public MainForm()
        {
            InitializeComponent();
            _discoveryService = new VisualStudioDiscoveryService();
            _formData = new BindingList<FormInfoRow>();
            _propertyData = new BindingList<PropertyRow>();
            
            // Configura il DataGrid, TreeView e PropertyGrid
            ConfigureDataGrid();
            ConfigureTreeView();
            ConfigurePropertyGrid();
        }

        private void ConfigurePropertyGrid()
        {
            sfDataGridProperties.DataSource = _propertyData;
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
            _propertyData.Clear();
            propertyGridLabel.Text = "Properties";
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

        private async void treeViewAdv1_AfterSelect(object sender, EventArgs e)
        {
            if (treeViewAdv1.SelectedNode == null)
                return;

            string expression = treeViewAdv1.SelectedNode.Tag as string;
            if (string.IsNullOrEmpty(expression))
                return;

            await LoadControlPropertiesAsync(expression);
        }

        private async Task LoadControlPropertiesAsync(string controlExpression)
        {
            _propertyData.Clear();
            
            if (_debuggerService == null)
                return;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                propertyGridLabel.Text = "Properties (Loading...)";

                // DISCOVERY DINAMICO: Ottiene tutte le proprietà usando reflection
                var stopwatch = Stopwatch.StartNew();
                
                // Ottiene il tipo del controllo
                Result<string> typeNameResult = _debuggerService.EvaluateExpression($"{controlExpression}.GetType().FullName");
                
                if (typeNameResult.IsFailure || string.IsNullOrEmpty(typeNameResult.Value))
                {
                    Debug.WriteLine("Unable to get control type");
                    propertyGridLabel.Text = "Properties";
                    return;
                }

                string typeName = typeNameResult.Value.Trim('"');
                Debug.WriteLine($"Control type: {typeName}");

                // Ottiene tutte le proprietà pubbliche
                List<string> propertyNames = await DiscoverPropertiesAsync(controlExpression);
                Debug.WriteLine($"Discovered {propertyNames.Count} properties");

                // Costruisce le espressioni da valutare
                var propertyExpressions = propertyNames
                    .Select(prop => $"{controlExpression}.{prop}")
                    .ToList();

                // Valuta tutte le proprietà in batch
                Result<Dictionary<string, string>> propertyResult = 
                    await _debuggerService.EvaluateExpressionsAsync(propertyExpressions);

                stopwatch.Stop();
                Debug.WriteLine($"Loaded properties in {stopwatch.ElapsedMilliseconds}ms");

                if (propertyResult.IsSuccess)
                {
                    foreach (var prop in propertyNames)
                    {
                        string fullExpression = $"{controlExpression}.{prop}";
                        if (propertyResult.Value.TryGetValue(fullExpression, out string value))
                        {
                            // Salta valori nulli, vuoti o errori di compilazione
                            if (!string.IsNullOrEmpty(value) && 
                                value != "null" && 
                                !value.Contains("error CS") &&
                                !value.Contains("Exception"))
                            {
                                _propertyData.Add(new PropertyRow
                                {
                                    PropertyName = prop,
                                    PropertyValue = value
                                });
                            }
                            else if (value.Contains("error CS"))
                            {
                                Debug.WriteLine($"Skipping {prop}: evaluation error - {value}");
                            }
                        }
                    }
                }

                // Aggiorna il titolo con il nome del controllo
                Result<string> nameResult = _debuggerService.EvaluateExpression($"{controlExpression}.Name");
                if (nameResult.IsSuccess && !string.IsNullOrEmpty(nameResult.Value))
                {
                    propertyGridLabel.Text = $"Properties - {nameResult.Value.Trim('"')}";
                }
                else
                {
                    propertyGridLabel.Text = "Properties";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading properties: {ex.Message}");
                propertyGridLabel.Text = "Properties (Error)";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private async Task<List<string>> DiscoverPropertiesAsync(string controlExpression)
        {
            var properties = new List<string>();

            try
            {
                // Usa reflection per ottenere tutte le proprietà pubbliche
                string getPropertiesExpr = $"{controlExpression}.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Length";
                Result<string> countResult = _debuggerService.EvaluateExpression(getPropertiesExpr);

                if (countResult.IsSuccess && int.TryParse(countResult.Value, out int propCount))
                {
                    Debug.WriteLine($"Total properties: {propCount}");

                    // Limita a un numero ragionevole per performance (max 100 proprietà)
                    int maxProps = Math.Min(propCount, 100);

                    for (int i = 0; i < maxProps; i++)
                    {
                        // Ottiene informazioni sulla proprietà
                        string propInfoExpr = $"{controlExpression}.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)[{i}]";
                        
                        // Nome proprietà
                        string propNameExpr = $"{propInfoExpr}.Name";
                        Result<string> propNameResult = _debuggerService.EvaluateExpression(propNameExpr);

                        if (propNameResult.IsSuccess && !string.IsNullOrEmpty(propNameResult.Value))
                        {
                            string propName = propNameResult.Value.Trim('"');
                            
                            // Verifica se la proprietà può essere letta (ha un getter pubblico)
                            string canReadExpr = $"{propInfoExpr}.CanRead";
                            Result<string> canReadResult = _debuggerService.EvaluateExpression(canReadExpr);
                            
                            if (canReadResult.IsFailure || canReadResult.Value != "true")
                            {
                                Debug.WriteLine($"Skipping {propName}: cannot read");
                                continue;
                            }

                            // Verifica se la proprietà ha parametri (è un indexer)
                            string hasParamsExpr = $"{propInfoExpr}.GetIndexParameters().Length";
                            Result<string> hasParamsResult = _debuggerService.EvaluateExpression(hasParamsExpr);
                            
                            if (hasParamsResult.IsSuccess && hasParamsResult.Value != "0")
                            {
                                Debug.WriteLine($"Skipping {propName}: is an indexer");
                                continue;
                            }
                            
                            // Filtra proprietà che potrebbero causare problemi
                            if (!ShouldSkipProperty(propName))
                            {
                                properties.Add(propName);
                            }
                            else
                            {
                                Debug.WriteLine($"Skipping {propName}: in skip list");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error discovering properties: {ex.Message}");
            }

            return properties;
        }

        private bool ShouldSkipProperty(string propertyName)
        {
            // Salta proprietà che potrebbero causare problemi o che non sono informative
            var skipList = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                // Proprietà che causano problemi con EnvDTE
                "Handle", "Parent", "Container", "Site", "BindingContext",
                "AccessibilityObject", "CreateParams", "DefaultImeMode",
                "DefaultMargin", "DefaultMaximumSize", "DefaultMinimumSize",
                "DefaultPadding", "DefaultSize", "FontHeight", "RenderRightToLeft",
                
                // Proprietà di layout interne
                "LayoutEngine", "ProductVersion", "CompanyName",
                "ProductName", "PreferredSize", "Controls",
                
                // Proprietà Windows Forms interne
                "WindowTarget", "Region", "AccessibleDefaultActionDescription",
                "CausesValidation", "DataBindings", "Tag",
                
                // Proprietà che richiedono contesto specifico
                "TopLevelControl", "ParentForm", "ActiveControl",
                "ContainsFocus", "Focused", "CanFocus", "CanSelect"
            };

            return skipList.Contains(propertyName);
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

    // Classe helper per il PropertyGrid
    public class PropertyRow
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
    }
}
