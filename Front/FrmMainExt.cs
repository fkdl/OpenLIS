using System.Windows.Forms;
using Base.Utilities;

namespace Front
{
    public partial class FrmMain
    {
        public void Preloadinds()
        {
            var t = new ToolStripMenuItem();
            t.Text = "&New Module";
            var d1 = t.DropDownItems.Add("&New Form");
            d1.Click += (sender, args) =>
            {
                var d1Mdi = new Form();
                d1Mdi.MdiParent = this;
                d1Mdi.Show();
                d1Mdi.Text = "Form " + StaticCounter.Next;
                d1Mdi.WindowState = FormWindowState.Maximized;
            };

            menuStrip.Items.Insert(2, t);
        }

    }
}
