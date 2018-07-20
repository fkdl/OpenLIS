using System;
using System.Windows.Forms;
using Base.DB.Model.Models.SqlServer.V2008;
using Base.DB.Model.Models.SqlServer.CondExpr;

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

            var m = new M("Friends");
            m.Data("name", "KK").Data("contact", "9It59IIejKlv+-e").Data("birthday", "1999/3/14");
            
            MessageBox.Show(m.Save().ToString());
        }
    }
    
}
