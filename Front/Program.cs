using System;
using System.Windows.Forms;
using Base.DB.Model.Models;
using Base.DB.Model.Conditions;

namespace Front
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form());
            
            var m = new MPlain("T1", "Id");

            // TODO: CondIn, CondGroup
            var condIn = new CondIn("id", new object[] {1300, 1313, 1314, 1350});
            var condIn2 = new CondIn("id", new object[] {1313, 1350, 1320});
            var condGroup = new CondGroup();
            condGroup.JoinWith("or");
            condGroup.Add(condIn).Add(condIn2);

            var table = m.
                Field("id").Field("attr1").
                Where(condGroup).
                Select();
            
            MessageBox.Show(string.Format("{0}", table.Rows.Count));

        }
    }
}
