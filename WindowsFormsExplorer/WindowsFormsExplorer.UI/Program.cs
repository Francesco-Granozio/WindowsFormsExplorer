using System;
using System.Windows.Forms;
using WindowsFormsExplorer.Infrastructure.COM;
using WindowsFormsExplorer.UI.Forms;

namespace WindowsFormsExplorer.UI
{
    internal static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // Registra il message filter per gestire chiamate COM
            MessageFilter.Register();

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            finally
            {
                MessageFilter.Revoke();
            }
        }
    }
}

