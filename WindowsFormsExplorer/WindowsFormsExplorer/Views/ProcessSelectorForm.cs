using EnvDTE;
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
    public partial class ProcessSelectorForm : Form
    {
        private readonly Processes m_DebugProcesses;
        public EnvDTE80.Process2 SelectedProcess { get; private set; }


        public ProcessSelectorForm(Processes processes)
        {
            InitializeComponent();

            this.m_DebugProcesses = processes;

            if (m_DebugProcesses.Count == 0)
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
                return;
            }
        }


        private void ProcessSelectorForm_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }


        private void LoadGrid()
        {
            foreach (EnvDTE80.Process2 process in m_DebugProcesses)
            {
                if (process == null)
                {
                    continue;
                }
                this.processesDataGridView.Rows.Add(process.ProcessID, process.Name);
            }
        }

        private void selectBtn_Click(object sender, EventArgs e)
        {
            SelectProcess();
        }


        private void SelectProcess()
        {
            if (processesDataGridView.SelectedRows.Count <= 0)
            {
                MessageBox.Show("No row selected. Please select an instance.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = processesDataGridView.SelectedRows[0];

            if (!int.TryParse(selectedRow.Cells[0].Value?.ToString(), out int selectedPID))
            {
                MessageBox.Show("Invalid Process ID. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            foreach (EnvDTE80.Process2 process in m_DebugProcesses)
            {
                if (process.ProcessID == selectedPID)
                {
                    SelectedProcess = process;
                    break;
                }
            }

            if (SelectedProcess == null)
            {
                MessageBox.Show("Selected process not found. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
