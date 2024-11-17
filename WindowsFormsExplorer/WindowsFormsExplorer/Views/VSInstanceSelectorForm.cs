using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsExplorer.Views
{
    /// <summary>
    /// Questa form mostra una finestra di dialogo per permettere all'utente di scegliere
    /// quale istanza di Visual Studio connessa desidera utilizzare, tra quelle attualmente in esecuzione.
    /// La lista delle istanze viene passata come parametro alla funzione.
    /// </summary>
    public partial class VSInstanceSelectorForm : Form
    {
        private readonly IReadOnlyList<EnvDTE80.DTE2> m_Instances;
        public EnvDTE80.DTE2 SelectedInstance { get; private set; }


        public VSInstanceSelectorForm(IReadOnlyList<EnvDTE80.DTE2> instances)
        {
            InitializeComponent();

            this.m_Instances = (IReadOnlyList<EnvDTE80.DTE2>)(instances ?? Enumerable.Empty<EnvDTE80.DTE2>());
        }

        private void VSInstanceSelectorForm_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid()
        {
            int i = 0;
            foreach (EnvDTE80.DTE2 instance in m_Instances)
            {

                if (instance == null
                    || instance.Solution == null)
                {
                    continue;
                }

                string name = $"{instance.Solution?.FullName ?? "Without open solution (.sln)"}";

                this.instancesDataGridView.Rows.Add(i + 1, name, instance.Solution?.FullName != null);

                i++;
            }


        }

        private void selectBtn_Click(object sender, EventArgs e)
        {
            SelectInstance();
        }

        private void SelectInstance()
        {
            if (instancesDataGridView.SelectedRows.Count <= 0)
            {
                MessageBox.Show("No row selected. Please select an instance.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = instancesDataGridView.SelectedRows[0];

            if (!int.TryParse(selectedRow.Cells[0].Value?.ToString(), out int instanceIndex) ||
                instanceIndex - 1 < 0 || instanceIndex - 1 >= m_Instances.Count)
            {
                MessageBox.Show("Invalid selection. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SelectedInstance = m_Instances[instanceIndex - 1];

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
