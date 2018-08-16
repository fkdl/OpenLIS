using System;
using System.Threading;
using System.Windows.Forms;
using Comm.UI;

namespace Comm
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            bool created;

            // ReSharper disable once ObjectCreationAsStatement
            new Mutex(true, Application.ProductName, out created);

            if (created)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var frmMain = new FrmMain();
                Application.Run(frmMain);
            }
            else
            {
                MessageBox.Show(@"Instance of this program is already running.", @"Alert",
                    MessageBoxButtons.OK, MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                Application.Exit();
            }
        }
    }
}
