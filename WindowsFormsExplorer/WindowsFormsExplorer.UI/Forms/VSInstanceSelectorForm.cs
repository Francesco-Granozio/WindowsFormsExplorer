using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using WindowsFormsExplorer.Core.Domain;

namespace WindowsFormsExplorer.UI.Forms
{
    public partial class VSInstanceSelectorForm : Form
    {
        private readonly List<VisualStudioInstance> _instances;
        private readonly BindingList<InstanceRow> _instanceData;
        public VisualStudioInstance SelectedInstance { get; private set; }

        public VSInstanceSelectorForm(List<VisualStudioInstance> instances)
        {
            InitializeComponent();
            _instances = instances ?? new List<VisualStudioInstance>();
            _instanceData = new BindingList<InstanceRow>();
        }

        private void VSInstanceSelectorForm_Load(object sender, EventArgs e)
        {
            sfDataGrid1.DataSource = _instanceData;
            LoadGrid();
        }

        private void LoadGrid()
        {
            int index = 1;
            foreach (VisualStudioInstance instance in _instances)
            {
                if (instance == null)
                    continue;

                _instanceData.Add(new InstanceRow
                {
                    InstanceNumber = index,
                    SolutionName = instance.HasOpenSolution ?
                        instance.SolutionName : "No solution open",
                    HasOpenSolution = instance.HasOpenSolution,
                    Instance = instance
                });

                index++;
            }
        }

        private void selectBtn_Click(object sender, EventArgs e)
        {
            SelectInstance();
        }

        private void sfDataGrid1_CellDoubleClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {
            SelectInstance();
        }

        private void SelectInstance()
        {
            if (sfDataGrid1.SelectedIndex < 0 || sfDataGrid1.SelectedIndex >= _instanceData.Count)
            {
                MessageBox.Show("No row selected. Please select an instance.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            InstanceRow selectedRow = _instanceData[sfDataGrid1.SelectedIndex];
            SelectedInstance = selectedRow.Instance;

            if (SelectedInstance == null)
            {
                MessageBox.Show("Invalid selection. Please try again.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Classe helper per il binding
        private class InstanceRow
        {
            public int InstanceNumber { get; set; }
            public string SolutionName { get; set; }
            public bool HasOpenSolution { get; set; }
            public VisualStudioInstance Instance { get; set; }
        }
    }
}
