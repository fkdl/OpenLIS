namespace Comm.UI.MDI.CachedData
{
    partial class MdiOleDb
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.dtpResultDate = new System.Windows.Forms.DateTimePicker();
            this.lblResultDdate = new System.Windows.Forms.Label();
            this.btnUpload = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnInvertSelection = new System.Windows.Forms.Button();
            this.btnRetrieve = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.txtConnStr = new System.Windows.Forms.TextBox();
            this.txtSqlForSearch = new System.Windows.Forms.TextBox();
            this.lblConnStr = new System.Windows.Forms.Label();
            this.lblSqlForSearch = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtSqlForMark = new System.Windows.Forms.TextBox();
            this.lblSqlForMark = new System.Windows.Forms.Label();
            this.lblAutoUpload = new System.Windows.Forms.Label();
            this.chkAutoUpload = new System.Windows.Forms.CheckBox();
            this.lblIntervalSec = new System.Windows.Forms.Label();
            this.mtxtIntervalSec = new System.Windows.Forms.MaskedTextBox();
            this.tmrAutoUpload = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPageEvents.SuspendLayout();
            this.tabPageConfigs.SuspendLayout();
            this.tabPageResults.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageConfigs
            // 
            this.tabPageConfigs.Controls.Add(this.tableLayoutPanel5);
            // 
            // tabPageResults
            // 
            this.tabPageResults.Controls.Add(this.tableLayoutPanel3);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.dgvResults, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(634, 430);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 6;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.Controls.Add(this.dtpResultDate, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblResultDdate, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnUpload, 5, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblCount, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.btnInvertSelection, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnRetrieve, 4, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnSelectAll, 3, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 373);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(628, 54);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // dtpResultDate
            // 
            this.dtpResultDate.CustomFormat = "yyyy-MM-dd";
            this.dtpResultDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpResultDate.Location = new System.Drawing.Point(103, 3);
            this.dtpResultDate.Name = "dtpResultDate";
            this.dtpResultDate.Size = new System.Drawing.Size(94, 21);
            this.dtpResultDate.TabIndex = 0;
            // 
            // lblResultDdate
            // 
            this.lblResultDdate.AutoSize = true;
            this.lblResultDdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResultDdate.Location = new System.Drawing.Point(3, 0);
            this.lblResultDdate.Name = "lblResultDdate";
            this.lblResultDdate.Size = new System.Drawing.Size(94, 27);
            this.lblResultDdate.TabIndex = 1;
            this.lblResultDdate.Text = "Results on";
            this.lblResultDdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(531, 3);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 21);
            this.btnUpload.TabIndex = 3;
            this.btnUpload.Text = "&Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.lblCount, 2);
            this.lblCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCount.Location = new System.Drawing.Point(3, 27);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(194, 27);
            this.lblCount.TabIndex = 5;
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnInvertSelection
            // 
            this.btnInvertSelection.Location = new System.Drawing.Point(203, 3);
            this.btnInvertSelection.Name = "btnInvertSelection";
            this.btnInvertSelection.Size = new System.Drawing.Size(75, 21);
            this.btnInvertSelection.TabIndex = 4;
            this.btnInvertSelection.Text = "Invert";
            this.btnInvertSelection.UseVisualStyleBackColor = true;
            this.btnInvertSelection.Click += new System.EventHandler(this.btnInvertSelection_Click);
            // 
            // btnRetrieve
            // 
            this.btnRetrieve.Location = new System.Drawing.Point(431, 3);
            this.btnRetrieve.Name = "btnRetrieve";
            this.btnRetrieve.Size = new System.Drawing.Size(75, 21);
            this.btnRetrieve.TabIndex = 2;
            this.btnRetrieve.Text = "&Retrieve";
            this.btnRetrieve.UseVisualStyleBackColor = true;
            this.btnRetrieve.Click += new System.EventHandler(this.btnRetrieve_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(303, 3);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 21);
            this.btnSelectAll.TabIndex = 6;
            this.btnSelectAll.Text = "All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected,
            this.ID});
            this.dgvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResults.Location = new System.Drawing.Point(3, 3);
            this.dgvResults.MultiSelect = false;
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.RowHeadersVisible = false;
            this.dgvResults.Size = new System.Drawing.Size(628, 364);
            this.dgvResults.TabIndex = 1;
            this.dgvResults.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResults_CellContentClick);
            // 
            // Selected
            // 
            this.Selected.Frozen = true;
            this.Selected.HeaderText = "";
            this.Selected.Name = "Selected";
            this.Selected.ReadOnly = true;
            this.Selected.Width = 30;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.Frozen = true;
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.txtConnStr, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.txtSqlForSearch, 1, 3);
            this.tableLayoutPanel5.Controls.Add(this.lblConnStr, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblSqlForSearch, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel1, 1, 12);
            this.tableLayoutPanel5.Controls.Add(this.txtSqlForMark, 1, 7);
            this.tableLayoutPanel5.Controls.Add(this.lblSqlForMark, 0, 7);
            this.tableLayoutPanel5.Controls.Add(this.lblAutoUpload, 0, 6);
            this.tableLayoutPanel5.Controls.Add(this.chkAutoUpload, 1, 6);
            this.tableLayoutPanel5.Controls.Add(this.lblIntervalSec, 0, 10);
            this.tableLayoutPanel5.Controls.Add(this.mtxtIntervalSec, 1, 10);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 13;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(640, 436);
            this.tableLayoutPanel5.TabIndex = 3;
            // 
            // txtConnStr
            // 
            this.txtConnStr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConnStr.Location = new System.Drawing.Point(153, 3);
            this.txtConnStr.Multiline = true;
            this.txtConnStr.Name = "txtConnStr";
            this.tableLayoutPanel5.SetRowSpan(this.txtConnStr, 3);
            this.txtConnStr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConnStr.Size = new System.Drawing.Size(484, 84);
            this.txtConnStr.TabIndex = 0;
            // 
            // txtSqlForSearch
            // 
            this.txtSqlForSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSqlForSearch.Location = new System.Drawing.Point(153, 93);
            this.txtSqlForSearch.Multiline = true;
            this.txtSqlForSearch.Name = "txtSqlForSearch";
            this.tableLayoutPanel5.SetRowSpan(this.txtSqlForSearch, 3);
            this.txtSqlForSearch.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSqlForSearch.Size = new System.Drawing.Size(484, 84);
            this.txtSqlForSearch.TabIndex = 3;
            // 
            // lblConnStr
            // 
            this.lblConnStr.AutoSize = true;
            this.lblConnStr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblConnStr.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblConnStr.Location = new System.Drawing.Point(3, 0);
            this.lblConnStr.Name = "lblConnStr";
            this.lblConnStr.Size = new System.Drawing.Size(144, 30);
            this.lblConnStr.TabIndex = 4;
            this.lblConnStr.Text = "Connection String";
            this.lblConnStr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSqlForSearch
            // 
            this.lblSqlForSearch.AutoSize = true;
            this.lblSqlForSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSqlForSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSqlForSearch.Location = new System.Drawing.Point(3, 90);
            this.lblSqlForSearch.Name = "lblSqlForSearch";
            this.lblSqlForSearch.Size = new System.Drawing.Size(144, 30);
            this.lblSqlForSearch.TabIndex = 7;
            this.lblSqlForSearch.Text = "SQL for Search";
            this.lblSqlForSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnLoad, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(153, 379);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(484, 54);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(287, 3);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 21);
            this.btnLoad.TabIndex = 6;
            this.btnLoad.Text = "&Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(387, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 21);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtSqlForMark
            // 
            this.txtSqlForMark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSqlForMark.Location = new System.Drawing.Point(153, 213);
            this.txtSqlForMark.Multiline = true;
            this.txtSqlForMark.Name = "txtSqlForMark";
            this.tableLayoutPanel5.SetRowSpan(this.txtSqlForMark, 3);
            this.txtSqlForMark.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSqlForMark.Size = new System.Drawing.Size(484, 84);
            this.txtSqlForMark.TabIndex = 5;
            // 
            // lblSqlForMark
            // 
            this.lblSqlForMark.AutoSize = true;
            this.lblSqlForMark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSqlForMark.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSqlForMark.Location = new System.Drawing.Point(3, 210);
            this.lblSqlForMark.Name = "lblSqlForMark";
            this.lblSqlForMark.Size = new System.Drawing.Size(144, 30);
            this.lblSqlForMark.TabIndex = 10;
            this.lblSqlForMark.Text = "SQL for Mark";
            this.lblSqlForMark.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAutoUpload
            // 
            this.lblAutoUpload.AutoSize = true;
            this.lblAutoUpload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAutoUpload.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAutoUpload.Location = new System.Drawing.Point(3, 180);
            this.lblAutoUpload.Name = "lblAutoUpload";
            this.lblAutoUpload.Size = new System.Drawing.Size(144, 30);
            this.lblAutoUpload.TabIndex = 11;
            this.lblAutoUpload.Text = "Auto Upload";
            this.lblAutoUpload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkAutoUpload
            // 
            this.chkAutoUpload.AutoSize = true;
            this.chkAutoUpload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkAutoUpload.Location = new System.Drawing.Point(153, 183);
            this.chkAutoUpload.Name = "chkAutoUpload";
            this.chkAutoUpload.Size = new System.Drawing.Size(484, 24);
            this.chkAutoUpload.TabIndex = 4;
            this.chkAutoUpload.UseVisualStyleBackColor = true;
            // 
            // lblIntervalSec
            // 
            this.lblIntervalSec.AutoSize = true;
            this.lblIntervalSec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIntervalSec.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblIntervalSec.Location = new System.Drawing.Point(3, 300);
            this.lblIntervalSec.Name = "lblIntervalSec";
            this.lblIntervalSec.Size = new System.Drawing.Size(144, 30);
            this.lblIntervalSec.TabIndex = 12;
            this.lblIntervalSec.Text = "Interval (Sec)";
            this.lblIntervalSec.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mtxtIntervalSec
            // 
            this.mtxtIntervalSec.Culture = new System.Globalization.CultureInfo("");
            this.mtxtIntervalSec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtxtIntervalSec.Location = new System.Drawing.Point(153, 303);
            this.mtxtIntervalSec.Mask = "99999";
            this.mtxtIntervalSec.Name = "mtxtIntervalSec";
            this.mtxtIntervalSec.PromptChar = ' ';
            this.mtxtIntervalSec.Size = new System.Drawing.Size(484, 21);
            this.mtxtIntervalSec.TabIndex = 13;
            this.mtxtIntervalSec.ValidatingType = typeof(int);
            // 
            // tmrAutoUpload
            // 
            this.tmrAutoUpload.Tick += new System.EventHandler(this.tmrAutoUpload_Tick);
            // 
            // MdiOleDb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 462);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "MdiOleDb";
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.tabControl1.ResumeLayout(false);
            this.tabPageEvents.ResumeLayout(false);
            this.tabPageConfigs.ResumeLayout(false);
            this.tabPageResults.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        public System.Windows.Forms.DateTimePicker dtpResultDate;
        public System.Windows.Forms.Label lblResultDdate;
        public System.Windows.Forms.Button btnRetrieve;
        public System.Windows.Forms.Button btnUpload;
        public System.Windows.Forms.DataGridView dgvResults;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        public System.Windows.Forms.TextBox txtConnStr;
        public System.Windows.Forms.TextBox txtSqlForSearch;
        public System.Windows.Forms.Label lblConnStr;
        public System.Windows.Forms.Label lblSqlForSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtSqlForMark;
        private System.Windows.Forms.Label lblSqlForMark;
        private System.Windows.Forms.Label lblAutoUpload;
        private System.Windows.Forms.CheckBox chkAutoUpload;
        private System.Windows.Forms.Label lblIntervalSec;
        private System.Windows.Forms.MaskedTextBox mtxtIntervalSec;
        private System.Windows.Forms.Timer tmrAutoUpload;
        private System.Windows.Forms.Button btnInvertSelection;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Button btnSelectAll;
    }
}