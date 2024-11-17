using System.Windows.Forms;

namespace WindowsFormsExplorer.Views
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSInstanceSelectorForm));
            this.instancesDataGridView = new System.Windows.Forms.DataGridView();
            this.colInstanceNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIstanceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOpeneSolution = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.selectBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.instancesDataGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // instancesDataGridView
            // 
            this.instancesDataGridView.AllowUserToAddRows = false;
            this.instancesDataGridView.AllowUserToDeleteRows = false;
            this.instancesDataGridView.AllowUserToOrderColumns = true;
            this.instancesDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.instancesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.instancesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInstanceNumber,
            this.colIstanceName,
            this.colOpeneSolution});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.instancesDataGridView.DefaultCellStyle = dataGridViewCellStyle1;
            this.instancesDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.instancesDataGridView.Location = new System.Drawing.Point(0, 0);
            this.instancesDataGridView.MultiSelect = false;
            this.instancesDataGridView.Name = "instancesDataGridView";
            this.instancesDataGridView.ReadOnly = true;
            this.instancesDataGridView.RowHeadersVisible = false;
            this.instancesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.instancesDataGridView.Size = new System.Drawing.Size(658, 381);
            this.instancesDataGridView.TabIndex = 0;
            // 
            // colInstanceNumber
            // 
            this.colInstanceNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colInstanceNumber.FillWeight = 189.6907F;
            this.colInstanceNumber.HeaderText = "Instance";
            this.colInstanceNumber.Name = "colInstanceNumber";
            this.colInstanceNumber.ReadOnly = true;
            this.colInstanceNumber.Width = 73;
            // 
            // colIstanceName
            // 
            this.colIstanceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colIstanceName.FillWeight = 10.30928F;
            this.colIstanceName.HeaderText = "Name";
            this.colIstanceName.Name = "colIstanceName";
            this.colIstanceName.ReadOnly = true;
            // 
            // colOpeneSolution
            // 
            this.colOpeneSolution.HeaderText = "Opened solution";
            this.colOpeneSolution.Name = "colOpeneSolution";
            this.colOpeneSolution.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.selectBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 341);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(658, 40);
            this.panel1.TabIndex = 1;
            // 
            // selectBtn
            // 
            this.selectBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectBtn.Image = global::WindowsFormsExplorer.Properties.Resources.select;
            this.selectBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.selectBtn.Location = new System.Drawing.Point(579, 9);
            this.selectBtn.Name = "selectBtn";
            this.selectBtn.Size = new System.Drawing.Size(64, 24);
            this.selectBtn.TabIndex = 0;
            this.selectBtn.Text = "Select";
            this.selectBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.selectBtn.UseVisualStyleBackColor = true;
            this.selectBtn.Click += new System.EventHandler(this.selectBtn_Click);
            // 
            // VSInstanceSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 381);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.instancesDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VSInstanceSelectorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select an instance";
            this.Load += new System.EventHandler(this.VSInstanceSelectorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.instancesDataGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView instancesDataGridView;
        private Panel panel1;
        private Button selectBtn;
        private DataGridViewTextBoxColumn colInstanceNumber;
        private DataGridViewTextBoxColumn colIstanceName;
        private DataGridViewCheckBoxColumn colOpeneSolution;
    }
}