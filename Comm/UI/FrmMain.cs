using System;
using System.Reflection;
using System.Windows.Forms;
using Base.Logger;
using Comm.Class;
using Comm.Conf;

namespace Comm.UI
{
    public sealed partial class FrmMain : Form
    {
        private readonly AComm _comm;

        public FrmMain()
        {
            InitializeComponent();
            
            // load DLL
            try
            {
                var asm = Assembly.LoadFrom($"{Environment.CurrentDirectory}\\{CommConf.DllFileName}");
                var classType = asm.GetType($"{CommConf.NameSpace}.{CommConf.ClassName}");
                _comm = (AComm) Activator.CreateInstance(classType);
            }
            catch (Exception ex)
            {
                var errMsg = $"Error occured on loading model: {ex.Message}. Unable to continue.";
                MessageBox.Show(errMsg, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                LogHelper.WriteLogError(errMsg);
            }
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            // close program if DLL is not loaded
            if (_comm?.MdiChild == null)
            {
                _blockClose = false;
                Close();
                return;
            }

            // window state
            WindowVisible(!CommConf.HideOnLoad);

            // window title
            Text = string.Format(CommConf.FormText, _comm.ModuleName);

            // tray text
            trayIcon.Text = string.Format(CommConf.TrayText, _comm.ModuleName);

            // tray ballon
            trayIcon.BalloonTipTitle = string.Format(CommConf.BallonTitle, _comm.ModuleName);
            trayIcon.BalloonTipText = string.Format(CommConf.BallonText, _comm.ModuleName);
            trayIcon.ShowBalloonTip(1000);

            // set MDI child
            _comm.MdiChild.MdiParent = this;
            _comm.MdiChild.Show();

            // merge tray menu
            if (_comm.MdiChild.TrayMenu != null && _comm.MdiChild.TrayMenu.Items.Count > 0)
            {
                var itemList = new ToolStripItem[_comm.MdiChild.TrayMenu.Items.Count];
                _comm.MdiChild.TrayMenu.Items.CopyTo(itemList, 0);
                for (var i = 0; i < itemList.Length; i++)
                {
                    trayMenu.Items.Insert(i, itemList[i]);
                }
                trayMenu.Items.Insert(trayMenu.Items.Count - 1, new ToolStripSeparator());
            }
        }
        
        #region Form Visibility
        /// <summary>
        /// Toggle window visibility.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            WindowVisible(!Visible);
        }

        private bool _blockClose = true;
        
        /// <summary>
        /// Allow close only if _blockClose is false.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_blockClose)
            {
                e.Cancel = true; // block close
                WindowVisible(false); // hide window
                trayIcon.ShowBalloonTip(1000); // ballon tip
            }
            else
            {
                e.Cancel = false; // allow close
                trayIcon.Visible = false; // clear tray icon
            }
        }

        /// <summary>
        /// "Exit" item of tray contex menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctmExit_Click(object sender, EventArgs e)
        {
            _blockClose = false;
            Close();
        }

        /// <summary>
        /// Sets window visibility.
        /// </summary>
        /// <param name="visible"></param>
        private void WindowVisible(bool visible = true)
        {
            if (visible)
            {
                Show();
                WindowState = FormWindowState.Normal;
            }
            else
            {
                WindowState = FormWindowState.Minimized;
                Hide();
            }
        }
        #endregion

    }
}
