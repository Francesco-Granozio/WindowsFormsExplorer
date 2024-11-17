using System.Windows.Forms;

namespace WindowsFormsExplorer.Views
{
    partial class ProcessSelectorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessSelectorForm));
            this.processesDataGridView = new System.Windows.Forms.DataGridView();
            this.colPID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIstanceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.selectBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.processesDataGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // processesDataGridView
            // 
            this.processesDataGridView.AllowUserToAddRows = false;
            this.processesDataGridView.AllowUserToDeleteRows = false;
            this.processesDataGridView.AllowUserToOrderColumns = true;
            this.processesDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.processesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.processesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPID,
            this.colIstanceName});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.processesDataGridView.DefaultCellStyle = dataGridViewCellStyle1;
            this.processesDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processesDataGridView.Location = new System.Drawing.Point(0, 0);
            this.processesDataGridView.MultiSelect = false;
            this.processesDataGridView.Name = "processesDataGridView";
            this.processesDataGridView.ReadOnly = true;
            this.processesDataGridView.RowHeadersVisible = false;
            this.processesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.processesDataGridView.Size = new System.Drawing.Size(497, 381);
            this.processesDataGridView.TabIndex = 0;
            // 
            // colPID
            // 
            this.colPID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colPID.FillWeight = 189.6907F;
            this.colPID.HeaderText = "PID";
            this.colPID.Name = "colPID";
            this.colPID.ReadOnly = true;
            this.colPID.Width = 50;
            // 
            // colIstanceName
            // 
            this.colIstanceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colIstanceName.FillWeight = 10.30928F;
            this.colIstanceName.HeaderText = "Name";
            this.colIstanceName.Name = "colIstanceName";
            this.colIstanceName.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.selectBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 341);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(497, 40);
            this.panel1.TabIndex = 1;
            // 
            // selectBtn
            // 
            this.selectBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectBtn.Image = global::WindowsFormsExplorer.Properties.Resources.select;
            this.selectBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.selectBtn.Location = new System.Drawing.Point(418, 9);
            this.selectBtn.Name = "selectBtn";
            this.selectBtn.Size = new System.Drawing.Size(64, 24);
            this.selectBtn.TabIndex = 0;
            this.selectBtn.Text = "Select";
            this.selectBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.selectBtn.UseVisualStyleBackColor = true;
            this.selectBtn.Click += new System.EventHandler(this.selectBtn_Click);
            // 
            // ProcessSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 381);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.processesDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProcessSelectorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select a process";
            this.Load += new System.EventHandler(this.ProcessSelectorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.processesDataGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView processesDataGridView;
        private Panel panel1;
        private Button selectBtn;
        private DataGridViewTextBoxColumn colPID;
        private DataGridViewTextBoxColumn colIstanceName;
    }
}