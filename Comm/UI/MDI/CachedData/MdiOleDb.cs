using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Base.Conf;
using Base.Logger;
using Comm.Class.Comm.CachedData;

namespace Comm.UI.MDI.CachedData
{
    public partial class MdiOleDb : MdiCommon
    {
        private readonly OleDbAccesser _oleDbAccesser = new OleDbAccesser();

        public MdiOleDb()
        {
            InitializeComponent();
            LoadConfigs();
            ApplyConfigs();
        }
        
        #region Retrieve & Upload
        /// <summary>
        /// Retrieve data from OLEDB, and fill into result grid.
        /// </summary>
        /// <param name="showMsgBox">Specify if to prompt summerize message.</param>
        private void Retrieve(bool showMsgBox = true)
        {
            try
            {
                // retrieve data by SQL. Parameter "@date" is available.
                var sql = OleDbConf.SqlForSearch.Replace("@date", dtpResultDate.Value.ToString(SysConf.DateFormat));
                var result = _oleDbAccesser.ExecuteDataTable(sql);
                ASyncSetGridData(result);

                // log message
                var msg = $"{result.Rows.Count} record(s) retrieved.";
                InvokeAddEventLog(EventType.Data, msg);
                LogHelper.WriteLogInfo(msg);

                if (showMsgBox)
                    MessageBox.Show(msg, @"Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                InvokeAddEventLog(EventType.System, ex.Message);
                LogHelper.WriteLogError(ex.Message);
            }
        }

        /// <summary>
        /// Choose checked rows and upload.
        /// </summary>
        /// <param name="showMsgBox">Specify if to prompt summerize message.</param>
        private void UploadSelected(bool showMsgBox = true)
        {
            var successCount = 0;
            InvokeLockRetrieve(); // lock results
            
            var selectedRows = dgvResults.Rows.Cast<DataGridViewRow>().Where(row => (bool) row.Cells[0].Value);
            foreach (var row in selectedRows)
            {
                try
                {
                    // get source
                    var source = (from DataGridViewCell cell in row.Cells select cell.Value).ToList();
                    source.RemoveAt(0); // remove first column checkbox

                    // upload
                    var id = row.Cells["ID"].Value.ToString();
                    var success = UploadResult(source);

                    // log
                    if (success)
                    {
                        if (OleDbConf.AutoUpload) // mark only on auto mode
                        {
                            var sql = OleDbConf.SqlForMark.Replace("@id", id);
                            _oleDbAccesser.ExecuteNonQuery(sql);
                        }
                        
                        successCount++;

                        var logMsg = $"Record uploaded. ID = {id}";
                        InvokeAddEventLog(EventType.Data, logMsg);
                        LogHelper.WriteLogInfo(logMsg);
                    }
                    else
                    {
                        var logMsg = $"Record upload failed. ID = {id}";
                        InvokeAddEventLog(EventType.Data, logMsg);
                        LogHelper.WriteLogError(logMsg);
                    }
                }
                catch (Exception ex)
                {
                    InvokeAddEventLog(EventType.System, ex.Message);
                    LogHelper.WriteLogInfo(ex.Message);
                }
            }

            InvokeLockRetrieve(false); // unlock results

            // summerize message
            var msg = $"{successCount} record(s) uploaded.";
            InvokeAddEventLog(EventType.Data, msg);
            LogHelper.WriteLogInfo(msg);
            if (showMsgBox)
                MessageBox.Show(msg, @"Total", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        /// <summary>
        /// Lock or unlock "Retrieve"
        /// </summary>
        /// <param name="isLock"></param>
        private void InvokeLockRetrieve(bool isLock = true)
        {
            BeginInvoke(new Action(() =>
            {
                btnRetrieve.Enabled = !isLock;
                dgvResults.Enabled = !isLock;
            }));
        }

        /// <summary>
        /// Manual retrieve
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            var thread = new Thread(() => { Retrieve(); });

            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// Manual upload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpload_Click(object sender, EventArgs e)
        {
            var thread = new Thread(() => { UploadSelected(); });

            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// Auto upload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrAutoUpload_Tick(object sender, EventArgs e)
        {
            var thread = new Thread(() =>
            {
                UploadSelected(showMsgBox: false);
                Retrieve(showMsgBox: false);
            });

            thread.IsBackground = true;
            thread.Start();
        }
        #endregion

        #region Grid Control
        /// <summary>
        /// Allow selections changing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0) return;

            var grid = (DataGridView) sender;
            var cell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.Value = !(bool) cell.EditedFormattedValue;
        }

        private void ASyncSetGridData(DataTable sourceTable)
        {
            BeginInvoke(new Action(() =>
            {
                dgvResults.DataSource = sourceTable;

                // set all rows checked
                foreach (DataGridViewRow row in dgvResults.Rows)
                    row.Cells[0].Value = true;
            }));
        }

        /// <summary>
        /// Invert selection in the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInvertSelection_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvResults.Rows)
            {
                var cell = row.Cells[0];
                cell.Value = !(bool)cell.EditedFormattedValue;
            }
        }
        
        /// <summary>
        /// Select all items in the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvResults.Rows)
            {
                var cell = row.Cells[0];
                cell.Value = true;
            }
        }
        #endregion

        #region Configurations
        /// <summary>
        /// Load configurations.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadConfigs();
            ApplyConfigs();
        }

        /// <summary>
        /// Save configurations.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfigs();
            ApplyConfigs();
        }

        private void LoadConfigs()
        {
            txtConnStr.Text = OleDbConf.ConnectionString;
            txtSqlForSearch.Text = OleDbConf.SqlForSearch;
            chkAutoUpload.Checked = OleDbConf.AutoUpload;
            txtSqlForMark.Text = OleDbConf.SqlForMark;
            mtxtIntervalSec.Text = Convert.ToString(OleDbConf.AutoUploadIntervalSec);

            const string logMsg = "Configurations Loaded.";
            AddEventLog(EventType.System, logMsg);
            LogHelper.WriteLogInfo(logMsg);
        }

        private void SaveConfigs()
        {
            OleDbConf.ConnectionString = txtConnStr.Text;
            OleDbConf.SqlForSearch = txtSqlForSearch.Text;
            OleDbConf.AutoUpload = chkAutoUpload.Checked;
            OleDbConf.SqlForMark = txtSqlForMark.Text;
            int interval;
            int.TryParse(mtxtIntervalSec.Text, out interval);
            OleDbConf.AutoUploadIntervalSec = interval;

            const string logMsg = "Configurations Saved.";
            AddEventLog(EventType.System, logMsg);
            LogHelper.WriteLogInfo(logMsg);
        }

        private void ApplyConfigs()
        {
            try
            {
                // oledb
                _oleDbAccesser.ConnectionString = OleDbConf.ConnectionString;

                // auto uploading
                tmrAutoUpload.Enabled = chkAutoUpload.Checked;
                if (OleDbConf.AutoUploadIntervalSec > 0)
                    tmrAutoUpload.Interval = OleDbConf.AutoUploadIntervalSec*1000;
                else
                    tmrAutoUpload.Enabled = false;

                const string logMsg = "Configurations Applied.";
                AddEventLog(EventType.System, logMsg);
                LogHelper.WriteLogInfo(logMsg);
            }
            catch (Exception ex)
            {
                AddEventLog(EventType.System, ex.Message);
                LogHelper.WriteLogError(ex.Message);
            }
        }

        #endregion

    }
}
