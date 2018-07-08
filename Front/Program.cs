using System;
using System.Windows.Forms;
using Base.DB.Model.Models;

namespace Front
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FrmMain());

            //MEav.Create();
            var eav = new MEav("experiment");

            MessageBox.Show(eav["e1", "a1"].ToString());
            MessageBox.Show(eav["e1", "a2"].ToString());
            MessageBox.Show(eav["e2", "a1"].ToString());
            MessageBox.Show(eav["e2", "a2"].ToString());
        }
    }
}
