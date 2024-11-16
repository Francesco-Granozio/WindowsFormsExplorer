using System.Windows.Forms;
using System;

namespace WindowsFormsExplorer
{
    public partial class MainForm : Form
    {
        private TextBox txtPID;
        private Button btnConnect;
        private Button btnRefresh;
        private ListView listViewForms;
        private TreeView treeViewControls;
        private SplitContainer splitContainer;


        private void InitializeComponent()
        {
            this.txtPID = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.listViewForms = new System.Windows.Forms.ListView();
            this.treeViewControls = new System.Windows.Forms.TreeView();
            this.controlsPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.controlsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPID
            // 
            this.txtPID.Location = new System.Drawing.Point(12, 12);
            this.txtPID.Name = "txtPID";
            this.txtPID.Size = new System.Drawing.Size(100, 20);
            this.txtPID.TabIndex = 0;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(118, 10);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connetti";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(199, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Aggiorna";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 45);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.listViewForms);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.treeViewControls);
            this.splitContainer.Size = new System.Drawing.Size(1184, 716);
            this.splitContainer.SplitterDistance = 592;
            this.splitContainer.TabIndex = 0;
            // 
            // listViewForms
            // 
            this.listViewForms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewForms.FullRowSelect = true;
            this.listViewForms.GridLines = true;
            this.listViewForms.HideSelection = false;
            this.listViewForms.Location = new System.Drawing.Point(0, 0);
            this.listViewForms.Name = "listViewForms";
            this.listViewForms.Size = new System.Drawing.Size(592, 716);
            this.listViewForms.TabIndex = 0;
            this.listViewForms.UseCompatibleStateImageBehavior = false;
            this.listViewForms.View = System.Windows.Forms.View.Details;
            this.listViewForms.SelectedIndexChanged += new System.EventHandler(this.listViewForms_SelectedIndexChanged);
            this.listViewForms.Columns.Add("Nome", 150);
            this.listViewForms.Columns.Add("Tipo", 200);
            this.listViewForms.Columns.Add("Testo", 200);
            this.listViewForms.Columns.Add("Visibile", 100);
            this.listViewForms.Columns.Add("Handle", 100);

            // 
            // treeViewControls
            // 
            this.treeViewControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewControls.Location = new System.Drawing.Point(0, 0);
            this.treeViewControls.Name = "treeViewControls";
            this.treeViewControls.Size = new System.Drawing.Size(588, 716);
            this.treeViewControls.TabIndex = 0;
            // 
            // controlsPanel
            // 
            this.controlsPanel.Controls.Add(this.txtPID);
            this.controlsPanel.Controls.Add(this.btnConnect);
            this.controlsPanel.Controls.Add(this.btnRefresh);
            this.controlsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlsPanel.Location = new System.Drawing.Point(0, 0);
            this.controlsPanel.Name = "controlsPanel";
            this.controlsPanel.Size = new System.Drawing.Size(1184, 45);
            this.controlsPanel.TabIndex = 1;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1184, 761);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.controlsPanel);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form Inspector";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.controlsPanel.ResumeLayout(false);
            this.controlsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        private Panel controlsPanel;
    }

}

