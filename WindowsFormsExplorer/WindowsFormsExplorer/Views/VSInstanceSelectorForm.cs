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
    public partial class VSInstanceSelectorForm : Form
    {
        private IReadOnlyList<EnvDTE80.DTE2> m_Instances;
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
            int i = 0; ;
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
            }

            int selectedRowIndex = instancesDataGridView.SelectedRows[0].Index;

            if (selectedRowIndex < 0 || selectedRowIndex >= m_Instances.Count)
            {
                MessageBox.Show("Invalid selection. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            SelectedInstance = m_Instances[selectedRowIndex];

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
