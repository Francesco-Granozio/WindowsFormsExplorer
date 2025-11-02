using System.Windows.Forms;
using System;

namespace WindowsFormsExplorer.UI.Forms
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Syncfusion.WinForms.DataGrid.GridTextColumn gridTextColumn1 = new Syncfusion.WinForms.DataGrid.GridTextColumn();
            Syncfusion.WinForms.DataGrid.GridTextColumn gridTextColumn2 = new Syncfusion.WinForms.DataGrid.GridTextColumn();
            Syncfusion.WinForms.DataGrid.GridTextColumn gridTextColumn3 = new Syncfusion.WinForms.DataGrid.GridTextColumn();
            Syncfusion.WinForms.DataGrid.GridTextColumn gridTextColumn4 = new Syncfusion.WinForms.DataGrid.GridTextColumn();
            Syncfusion.WinForms.DataGrid.GridTextColumn gridTextColumn5 = new Syncfusion.WinForms.DataGrid.GridTextColumn();
            Syncfusion.Windows.Forms.Tools.TreeNodeAdvStyleInfo treeNodeAdvStyleInfo1 = new Syncfusion.Windows.Forms.Tools.TreeNodeAdvStyleInfo();
            Syncfusion.WinForms.DataGrid.GridTextColumn gridTextColumn6 = new Syncfusion.WinForms.DataGrid.GridTextColumn();
            Syncfusion.WinForms.DataGrid.GridTextColumn gridTextColumn7 = new Syncfusion.WinForms.DataGrid.GridTextColumn();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.sfDataGrid1 = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.splitContainerRight = new System.Windows.Forms.SplitContainer();
            this.treeViewAdv1 = new Syncfusion.Windows.Forms.Tools.TreeViewAdv();
            this.propertyPanel = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.sfDataGridProperties = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.propertyGridLabel = new System.Windows.Forms.Label();
            this.gradientPanel1 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.btnConnect = new Syncfusion.Windows.Forms.ButtonAdv();
            this.btnRefresh = new Syncfusion.Windows.Forms.ButtonAdv();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).BeginInit();
            this.splitContainerRight.Panel1.SuspendLayout();
            this.splitContainerRight.Panel2.SuspendLayout();
            this.splitContainerRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeViewAdv1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyPanel)).BeginInit();
            this.propertyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGridProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel1)).BeginInit();
            this.gradientPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 60);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.sfDataGrid1);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.splitContainerRight);
            this.splitContainer.Size = new System.Drawing.Size(1184, 701);
            this.splitContainer.SplitterDistance = 400;
            this.splitContainer.TabIndex = 0;
            // 
            // sfDataGrid1
            // 
            this.sfDataGrid1.AccessibleName = "Table";
            this.sfDataGrid1.AllowEditing = false;
            this.sfDataGrid1.AllowFiltering = true;
            this.sfDataGrid1.AllowResizingColumns = true;
            this.sfDataGrid1.AutoGenerateColumns = false;
            this.sfDataGrid1.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.Fill;
            gridTextColumn1.AllowEditing = false;
            gridTextColumn1.AllowFiltering = true;
            gridTextColumn1.AllowResizing = true;
            gridTextColumn1.HeaderText = "Name";
            gridTextColumn1.MappingName = "Name";
            gridTextColumn1.Width = 150D;
            gridTextColumn2.AllowEditing = false;
            gridTextColumn2.AllowFiltering = true;
            gridTextColumn2.AllowResizing = true;
            gridTextColumn2.HeaderText = "Type";
            gridTextColumn2.MappingName = "Type";
            gridTextColumn2.Width = 200D;
            gridTextColumn3.AllowEditing = false;
            gridTextColumn3.AllowFiltering = true;
            gridTextColumn3.AllowResizing = true;
            gridTextColumn3.HeaderText = "Text";
            gridTextColumn3.MappingName = "Text";
            gridTextColumn3.Width = 250D;
            gridTextColumn4.AllowEditing = false;
            gridTextColumn4.AllowFiltering = true;
            gridTextColumn4.AllowResizing = true;
            gridTextColumn4.HeaderText = "Visible";
            gridTextColumn4.MappingName = "Visible";
            gridTextColumn4.Width = 80D;
            gridTextColumn5.AllowEditing = false;
            gridTextColumn5.AllowFiltering = true;
            gridTextColumn5.AllowResizing = true;
            gridTextColumn5.HeaderText = "Handle";
            gridTextColumn5.MappingName = "Handle";
            gridTextColumn5.Width = 100D;
            this.sfDataGrid1.Columns.Add(gridTextColumn1);
            this.sfDataGrid1.Columns.Add(gridTextColumn2);
            this.sfDataGrid1.Columns.Add(gridTextColumn3);
            this.sfDataGrid1.Columns.Add(gridTextColumn4);
            this.sfDataGrid1.Columns.Add(gridTextColumn5);
            this.sfDataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sfDataGrid1.Location = new System.Drawing.Point(0, 0);
            this.sfDataGrid1.Name = "sfDataGrid1";
            this.sfDataGrid1.Size = new System.Drawing.Size(400, 701);
            this.sfDataGrid1.TabIndex = 0;
            this.sfDataGrid1.SelectionChanged += new Syncfusion.WinForms.DataGrid.Events.SelectionChangedEventHandler(this.sfDataGrid1_SelectionChanged);
            // 
            // splitContainerRight
            // 
            this.splitContainerRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRight.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRight.Name = "splitContainerRight";
            // 
            // splitContainerRight.Panel1
            // 
            this.splitContainerRight.Panel1.Controls.Add(this.treeViewAdv1);
            // 
            // splitContainerRight.Panel2
            // 
            this.splitContainerRight.Panel2.Controls.Add(this.propertyPanel);
            this.splitContainerRight.Size = new System.Drawing.Size(780, 701);
            this.splitContainerRight.SplitterDistance = 480;
            this.splitContainerRight.TabIndex = 0;
            // 
            // treeViewAdv1
            // 
            this.treeViewAdv1.BackColor = System.Drawing.Color.White;
            this.treeViewAdv1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))));
            treeNodeAdvStyleInfo1.CheckBoxTickThickness = 1;
            treeNodeAdvStyleInfo1.CheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            treeNodeAdvStyleInfo1.EnsureDefaultOptionedChild = true;
            treeNodeAdvStyleInfo1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNodeAdvStyleInfo1.IntermediateCheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            treeNodeAdvStyleInfo1.OptionButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            treeNodeAdvStyleInfo1.SelectedOptionButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            treeNodeAdvStyleInfo1.ShowPlusMinus = true;
            treeNodeAdvStyleInfo1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.treeViewAdv1.BaseStylePairs.AddRange(new Syncfusion.Windows.Forms.Tools.StyleNamePair[] {
            new Syncfusion.Windows.Forms.Tools.StyleNamePair("Standard", treeNodeAdvStyleInfo1)});
            this.treeViewAdv1.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.treeViewAdv1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeViewAdv1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewAdv1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewAdv1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.treeViewAdv1.FullRowSelect = true;
            // 
            // 
            // 
            this.treeViewAdv1.HelpTextControl.BaseThemeName = null;
            this.treeViewAdv1.HelpTextControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeViewAdv1.HelpTextControl.Location = new System.Drawing.Point(0, 0);
            this.treeViewAdv1.HelpTextControl.Name = "helpText";
            this.treeViewAdv1.HelpTextControl.Size = new System.Drawing.Size(49, 15);
            this.treeViewAdv1.HelpTextControl.TabIndex = 0;
            this.treeViewAdv1.HelpTextControl.Text = "help text";
            this.treeViewAdv1.InactiveSelectedNodeForeColor = System.Drawing.SystemColors.ControlText;
            this.treeViewAdv1.ItemHeight = 28;
            this.treeViewAdv1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(94)))), ((int)(((byte)(94)))));
            this.treeViewAdv1.Location = new System.Drawing.Point(0, 0);
            this.treeViewAdv1.MetroColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(165)))), ((int)(((byte)(220)))));
            this.treeViewAdv1.Name = "treeViewAdv1";
            this.treeViewAdv1.SelectedNodeBackground = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197))))));
            this.treeViewAdv1.SelectedNodeForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.treeViewAdv1.ShowFocusRect = false;
            this.treeViewAdv1.Size = new System.Drawing.Size(480, 701);
            this.treeViewAdv1.Style = Syncfusion.Windows.Forms.Tools.TreeStyle.Office2016Colorful;
            this.treeViewAdv1.TabIndex = 0;
            this.treeViewAdv1.Text = "treeViewAdv1";
            this.treeViewAdv1.ThemeName = "Office2016Colorful";
            this.treeViewAdv1.ThemeStyle.TreeNodeAdvStyle.CheckBoxTickThickness = 0;
            this.treeViewAdv1.ThemeStyle.TreeNodeAdvStyle.EnsureDefaultOptionedChild = true;
            // 
            // 
            // 
            this.treeViewAdv1.ToolTipControl.BackColor = System.Drawing.SystemColors.Info;
            this.treeViewAdv1.ToolTipControl.BaseThemeName = null;
            this.treeViewAdv1.ToolTipControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeViewAdv1.ToolTipControl.Location = new System.Drawing.Point(0, 0);
            this.treeViewAdv1.ToolTipControl.Name = "toolTip";
            this.treeViewAdv1.ToolTipControl.Size = new System.Drawing.Size(41, 15);
            this.treeViewAdv1.ToolTipControl.TabIndex = 1;
            this.treeViewAdv1.ToolTipControl.Text = "toolTip";
            this.treeViewAdv1.TransparentControls = true;
            this.treeViewAdv1.AfterSelect += new System.EventHandler(this.treeViewAdv1_AfterSelect);
            // 
            // propertyPanel
            // 
            this.propertyPanel.BackColor = System.Drawing.Color.White;
            this.propertyPanel.BorderColor = System.Drawing.Color.Silver;
            this.propertyPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.propertyPanel.Controls.Add(this.sfDataGridProperties);
            this.propertyPanel.Controls.Add(this.propertyGridLabel);
            this.propertyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyPanel.Location = new System.Drawing.Point(0, 0);
            this.propertyPanel.Name = "propertyPanel";
            this.propertyPanel.Size = new System.Drawing.Size(296, 701);
            this.propertyPanel.TabIndex = 0;
            // 
            // sfDataGridProperties
            // 
            this.sfDataGridProperties.AccessibleName = "Table";
            this.sfDataGridProperties.AllowEditing = false;
            this.sfDataGridProperties.AllowResizingColumns = true;
            this.sfDataGridProperties.AutoGenerateColumns = false;
            this.sfDataGridProperties.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.Fill;
            gridTextColumn6.AllowEditing = false;
            gridTextColumn6.AllowResizing = true;
            gridTextColumn6.HeaderText = "Name";
            gridTextColumn6.MappingName = "PropertyName";
            gridTextColumn7.AllowEditing = false;
            gridTextColumn7.AllowResizing = true;
            gridTextColumn7.HeaderText = "Value";
            gridTextColumn7.MappingName = "PropertyValue";
            this.sfDataGridProperties.Columns.Add(gridTextColumn6);
            this.sfDataGridProperties.Columns.Add(gridTextColumn7);
            this.sfDataGridProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sfDataGridProperties.Location = new System.Drawing.Point(0, 35);
            this.sfDataGridProperties.Name = "sfDataGridProperties";
            this.sfDataGridProperties.RowHeight = 21;
            this.sfDataGridProperties.Size = new System.Drawing.Size(294, 664);
            this.sfDataGridProperties.Style.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(114)))), ((int)(((byte)(196)))));
            this.sfDataGridProperties.Style.HeaderStyle.Font.Bold = true;
            this.sfDataGridProperties.Style.HeaderStyle.TextColor = System.Drawing.Color.White;
            this.sfDataGridProperties.TabIndex = 1;
            // 
            // propertyGridLabel
            // 
            this.propertyGridLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.propertyGridLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.propertyGridLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyGridLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(114)))), ((int)(((byte)(196)))));
            this.propertyGridLabel.Location = new System.Drawing.Point(0, 0);
            this.propertyGridLabel.Name = "propertyGridLabel";
            this.propertyGridLabel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.propertyGridLabel.Size = new System.Drawing.Size(294, 35);
            this.propertyGridLabel.TabIndex = 0;
            this.propertyGridLabel.Text = "Properties";
            this.propertyGridLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.BorderColor = System.Drawing.Color.Silver;
            this.gradientPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientPanel1.Controls.Add(this.btnConnect);
            this.gradientPanel1.Controls.Add(this.btnRefresh);
            this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Size = new System.Drawing.Size(1184, 60);
            this.gradientPanel1.TabIndex = 1;
            // 
            // btnConnect
            // 
            this.btnConnect.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2016Colorful;
            this.btnConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(165)))), ((int)(((byte)(220)))));
            this.btnConnect.BeforeTouchSize = new System.Drawing.Size(110, 35);
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.Image = global::WindowsFormsExplorer.UI.Properties.Resources.connect;
            this.btnConnect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConnect.Location = new System.Drawing.Point(12, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(110, 35);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "  Connect";
            this.btnConnect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnConnect.ThemeName = "Office2016Colorful";
            this.btnConnect.UseVisualStyle = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2016Colorful;
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(114)))), ((int)(((byte)(196)))));
            this.btnRefresh.BeforeTouchSize = new System.Drawing.Size(110, 35);
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Image = global::WindowsFormsExplorer.UI.Properties.Resources.refresh;
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(128, 12);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(110, 35);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "  Refresh";
            this.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRefresh.ThemeName = "Office2016Colorful";
            this.btnRefresh.UseVisualStyle = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 761);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.gradientPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form Inspector";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid1)).EndInit();
            this.splitContainerRight.Panel1.ResumeLayout(false);
            this.splitContainerRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).EndInit();
            this.splitContainerRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeViewAdv1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyPanel)).EndInit();
            this.propertyPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGridProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel1)).EndInit();
            this.gradientPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel1;
        private Syncfusion.Windows.Forms.ButtonAdv btnConnect;
        private Syncfusion.Windows.Forms.ButtonAdv btnRefresh;
        private Syncfusion.WinForms.DataGrid.SfDataGrid sfDataGrid1;
        private Syncfusion.Windows.Forms.Tools.TreeViewAdv treeViewAdv1;
        private SplitContainer splitContainer;
        private SplitContainer splitContainerRight;
        private Syncfusion.Windows.Forms.Tools.GradientPanel propertyPanel;
        private System.Windows.Forms.Label propertyGridLabel;
        private Syncfusion.WinForms.DataGrid.SfDataGrid sfDataGridProperties;
    }
}
