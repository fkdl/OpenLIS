﻿using System;
using System.Windows.Forms;
using Base.DB.Model.Models.SqlServer.V2008;

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

            var eav = new Eav("test1");
            eav["01", "a1"] = DateTime.Now;
        }
    }
    
}
