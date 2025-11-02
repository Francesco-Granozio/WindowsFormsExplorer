using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsExplorer.Core.Domain;

namespace WindowsFormsExplorer.UI.Forms
{
    public partial class ProcessSelectorForm : Form
    {
        private readonly List<DebugProcess> _processes;
        private readonly BindingList<ProcessRow> _processData;
        public DebugProcess SelectedProcess { get; private set; }

        public ProcessSelectorForm(List<DebugProcess> processes)
        {
            InitializeComponent();
            _processes = processes ?? new List<DebugProcess>();
            _processData = new BindingList<ProcessRow>();
        }

        private void ProcessSelectorForm_Load(object sender, EventArgs e)
        {
            sfDataGrid1.DataSource = _processData;
            LoadGrid();
        }

        private void LoadGrid()
        {
            foreach (DebugProcess process in _processes)
            {
                if (process == null)
                    continue;

                _processData.Add(new ProcessRow
                {
                    ProcessId = process.ProcessId,
                    ProcessName = process.ProcessName,
                    NativeProcess = process
                });
            }
        }

        private void selectBtn_Click(object sender, EventArgs e)
        {
            SelectProcess();
        }

        private void sfDataGrid1_CellDoubleClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {
            SelectProcess();
        }

        private void SelectProcess()
        {
            if (sfDataGrid1.SelectedIndex < 0 || sfDataGrid1.SelectedIndex >= _processData.Count)
            {
                MessageBox.Show("No row selected. Please select a process.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ProcessRow selectedRow = _processData[sfDataGrid1.SelectedIndex];
            SelectedProcess = _processes.FirstOrDefault(p => p.ProcessId == selectedRow.ProcessId);

            if (SelectedProcess == null)
            {
                MessageBox.Show("Selected process not found. Please try again.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Classe helper per il binding
        private class ProcessRow
        {
            public int ProcessId { get; set; }
            public string ProcessName { get; set; }
            public DebugProcess NativeProcess { get; set; }
        }
    }
}
