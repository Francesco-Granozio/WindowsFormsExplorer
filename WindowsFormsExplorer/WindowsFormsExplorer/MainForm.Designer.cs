using System.Windows.Forms;
using System;

namespace WindowsFormsExplorer
{
    public partial class MainForm
    {
        private TextBox txtPID;
        private Button btnConnect;
        private Button btnRefresh;
        private ListView listViewForms;

        private void InitializeComponent()
        {
            this.txtPID = new TextBox();
            this.btnConnect = new Button();
            this.btnRefresh = new Button();
            this.listViewForms = new ListView();

            // Form
            this.Text = "Form Inspector";
            this.Size = new System.Drawing.Size(800, 600);

            // PID TextBox
            this.txtPID.Location = new System.Drawing.Point(12, 12);
            this.txtPID.Size = new System.Drawing.Size(100, 20);
            this.Controls.Add(this.txtPID);

            // Connect Button
            this.btnConnect.Location = new System.Drawing.Point(118, 10);
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.Text = "Connetti";
            this.btnConnect.Click += new EventHandler(btnConnect_Click);
            this.Controls.Add(this.btnConnect);

            // Refresh Button
            this.btnRefresh.Location = new System.Drawing.Point(199, 10);
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.Text = "Aggiorna";
            this.btnRefresh.Click += new EventHandler(btnRefresh_Click);
            this.Controls.Add(this.btnRefresh);

            // ListView
            this.listViewForms.Location = new System.Drawing.Point(12, 45);
            this.listViewForms.Size = new System.Drawing.Size(760, 500);
            this.listViewForms.View = View.Details;
            this.listViewForms.FullRowSelect = true;
            this.listViewForms.GridLines = true;

            // Aggiungi colonne
            this.listViewForms.Columns.Add("Nome", 150);
            this.listViewForms.Columns.Add("Tipo", 200);
            this.listViewForms.Columns.Add("Testo", 200);
            this.listViewForms.Columns.Add("Visibile", 100);
            this.listViewForms.Columns.Add("Handle", 100);

            this.Controls.Add(this.listViewForms);
        }
    }

}

