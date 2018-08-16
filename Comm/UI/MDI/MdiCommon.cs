using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Base.Conf;
using Base.Logger;

namespace Comm.UI.MDI
{
    public partial class MdiCommon : Form
    {
        public MdiCommon()
        {
            InitializeComponent();
            TrayMenu = null;
        }

        public virtual ContextMenuStrip TrayMenu { get; }

        /// <summary>
        /// MDI cannot be closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MdiAbstract_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        #region Event Log

        /// <summary>
        /// Add an event log into the list.
        /// !!! DO NOT call this method BEFORE form's handle is created !!!
        /// </summary>
        /// <param name="type"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public MdiCommon AddEventLog(EventType type, string content)
        {
            var typeName = @"Unknown";
            var textColor = Color.LightGray;
            var backColor = Color.White;

            switch (type)
            {
                case EventType.System:
                    typeName = @"System";
                    textColor = Color.DarkRed;
                    break;
                case EventType.UserOpt:
                    typeName = @"User";
                    textColor = Color.Black;
                    break;
                case EventType.Data:
                    typeName = @"Data";
                    textColor = Color.DarkBlue;
                    break;
            }

            var newItem = lsvEvents.Items.Add(typeName);

            newItem.SubItems.Add(DateTime.Now.ToString(SysConf.DateTimeFormat));
            newItem.SubItems.Add(content);
            newItem.ForeColor = textColor;
            newItem.BackColor = backColor;
            newItem.EnsureVisible();

            return this;
        }

        public MdiCommon InvokeAddEventLog(EventType type, string content)
        {
            BeginInvoke(new Action(() => AddEventLog(type, content)));

            return this;
        }

        /// <summary>
        /// Clear event logs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(
                    @"Are you sure?",
                    @"Comfirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                lsvEvents.Items.Clear();
            }
        }

        /// <summary>
        /// Save event logs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var savedlg = new SaveFileDialog
            {
                FileName = $"comm_log_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt",
                Filter = @"*.txt|*.txt"
            };

            if (savedlg.ShowDialog() != DialogResult.OK) return;

            var lines = (
                from ListViewItem item in lsvEvents.Items
                select $"{item.SubItems[0].Text} | {item.SubItems[1].Text}")
                .ToList();

            File.WriteAllLines(savedlg.FileName, lines, Encoding.UTF8);
        }
        #endregion
        
        /// <summary>
        /// Upload instrument result to LIS.
        /// </summary>
        /// <param name="dataSource">Source of result data</param>
        /// <returns></returns>
        protected virtual bool UploadResult(object dataSource)
        {
            const string errMsg =
                "Virtual method UploadResult(dataSource) called. To upload results correctly, this method must be overided.";
            LogHelper.WriteLogError(errMsg);

            throw new Exception(errMsg);
        }
    }

    public enum EventType
    {
        System,
        UserOpt,
        Data,
        Unknown
    }
}
