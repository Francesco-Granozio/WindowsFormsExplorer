using System.Windows.Forms;

namespace WindowsFormsExplorer.UI.Forms
{
    partial class VSInstanceSelectorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSInstanceSelectorForm));
            this.sfDataGrid1 = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.gradientPanel1 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.selectBtn = new Syncfusion.Windows.Forms.ButtonAdv();
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel1)).BeginInit();
            this.gradientPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sfDataGrid1
            // 
            this.sfDataGrid1.AccessibleName = "Table";
            this.sfDataGrid1.AllowEditing = false;
            this.sfDataGrid1.AllowResizingColumns = true;
            this.sfDataGrid1.AutoGenerateColumns = false;
            this.sfDataGrid1.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.Fill;
            this.sfDataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sfDataGrid1.Location = new System.Drawing.Point(0, 0);
            this.sfDataGrid1.Name = "sfDataGrid1";
            this.sfDataGrid1.SelectionMode = Syncfusion.WinForms.DataGrid.Enums.GridSelectionMode.Single;
            this.sfDataGrid1.Size = new System.Drawing.Size(654, 331);
            this.sfDataGrid1.Style.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(114)))), ((int)(((byte)(196)))));
            this.sfDataGrid1.Style.HeaderStyle.Font.Bold = true;
            this.sfDataGrid1.Style.HeaderStyle.TextColor = System.Drawing.Color.White;
            this.sfDataGrid1.TabIndex = 0;
            // 
            // Columns configuration
            // 
            this.sfDataGrid1.Columns.Add(new Syncfusion.WinForms.DataGrid.GridTextColumn()
            {
                MappingName = "InstanceNumber",
                HeaderText = "Instance",
                Width = 80,
            });
            this.sfDataGrid1.Columns.Add(new Syncfusion.WinForms.DataGrid.GridTextColumn()
            {
                MappingName = "SolutionName",
                HeaderText = "Solution Name",
                Width = 400
            });
            this.sfDataGrid1.Columns.Add(new Syncfusion.WinForms.DataGrid.GridCheckBoxColumn()
            {
                MappingName = "HasOpenSolution",
                HeaderText = "Has Solution",
                Width = 120,
                AllowEditing = false
            });
            this.sfDataGrid1.CellDoubleClick += new Syncfusion.WinForms.DataGrid.Events.CellClickEventHandler(this.sfDataGrid1_CellDoubleClick);
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.BorderColor = System.Drawing.Color.Silver;
            this.gradientPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientPanel1.Controls.Add(this.selectBtn);
            this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gradientPanel1.Location = new System.Drawing.Point(0, 331);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Size = new System.Drawing.Size(654, 50);
            this.gradientPanel1.TabIndex = 1;
            // 
            // selectBtn
            // 
            this.selectBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectBtn.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2016Colorful;
            this.selectBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(114)))), ((int)(((byte)(196)))));
            this.selectBtn.BeforeTouchSize = new System.Drawing.Size(100, 32);
            this.selectBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectBtn.ForeColor = System.Drawing.Color.White;
            this.selectBtn.Image = Properties.Resources.select;
            this.selectBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.selectBtn.Location = new System.Drawing.Point(540, 9);
            this.selectBtn.Name = "selectBtn";
            this.selectBtn.Size = new System.Drawing.Size(100, 32);
            this.selectBtn.TabIndex = 0;
            this.selectBtn.Text = "  Select";
            this.selectBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.selectBtn.UseVisualStyle = false;
            this.selectBtn.Click += new System.EventHandler(this.selectBtn_Click);
            // 
            // VSInstanceSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 381);
            this.Controls.Add(this.sfDataGrid1);
            this.Controls.Add(this.gradientPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VSInstanceSelectorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select a Visual Studio Instance";
            this.Load += new System.EventHandler(this.VSInstanceSelectorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel1)).EndInit();
            this.gradientPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Syncfusion.WinForms.DataGrid.SfDataGrid sfDataGrid1;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel1;
        private Syncfusion.Windows.Forms.ButtonAdv selectBtn;
    }
}
