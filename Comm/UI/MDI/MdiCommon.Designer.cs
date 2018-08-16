namespace Comm.UI.MDI
{
    partial class MdiCommon
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageResults = new System.Windows.Forms.TabPage();
            this.tabPageEvents = new System.Windows.Forms.TabPage();
            this.lsvEvents = new System.Windows.Forms.ListView();
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colContent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ctmEvents = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageConfigs = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPageEvents.SuspendLayout();
            this.ctmEvents.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageResults);
            this.tabControl1.Controls.Add(this.tabPageEvents);
            this.tabControl1.Controls.Add(this.tabPageConfigs);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(648, 462);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageResults
            // 
            this.tabPageResults.Location = new System.Drawing.Point(4, 22);
            this.tabPageResults.Name = "tabPageResults";
            this.tabPageResults.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageResults.Size = new System.Drawing.Size(640, 436);
            this.tabPageResults.TabIndex = 1;
            this.tabPageResults.Text = "Results";
            this.tabPageResults.UseVisualStyleBackColor = true;
            // 
            // tabPageEvents
            // 
            this.tabPageEvents.Controls.Add(this.lsvEvents);
            this.tabPageEvents.Location = new System.Drawing.Point(4, 22);
            this.tabPageEvents.Name = "tabPageEvents";
            this.tabPageEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEvents.Size = new System.Drawing.Size(640, 436);
            this.tabPageEvents.TabIndex = 0;
            this.tabPageEvents.Text = "Events";
            this.tabPageEvents.UseVisualStyleBackColor = true;
            // 
            // lsvEvents
            // 
            this.lsvEvents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colType,
            this.colTime,
            this.colContent});
            this.lsvEvents.ContextMenuStrip = this.ctmEvents;
            this.lsvEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvEvents.FullRowSelect = true;
            this.lsvEvents.Location = new System.Drawing.Point(3, 3);
            this.lsvEvents.MultiSelect = false;
            this.lsvEvents.Name = "lsvEvents";
            this.lsvEvents.Size = new System.Drawing.Size(634, 430);
            this.lsvEvents.TabIndex = 0;
            this.lsvEvents.UseCompatibleStateImageBehavior = false;
            this.lsvEvents.View = System.Windows.Forms.View.Details;
            // 
            // colType
            // 
            this.colType.Text = "Type";
            this.colType.Width = 100;
            // 
            // colTime
            // 
            this.colTime.Text = "Time";
            this.colTime.Width = 150;
            // 
            // colContent
            // 
            this.colContent.Text = "Content";
            this.colContent.Width = 450;
            // 
            // ctmEvents
            // 
            this.ctmEvents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.clearToolStripMenuItem});
            this.ctmEvents.Name = "ctmEvents";
            this.ctmEvents.Size = new System.Drawing.Size(107, 48);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.clearToolStripMenuItem.Text = "&Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // tabPageConfigs
            // 
            this.tabPageConfigs.Location = new System.Drawing.Point(4, 22);
            this.tabPageConfigs.Name = "tabPageConfigs";
            this.tabPageConfigs.Size = new System.Drawing.Size(640, 436);
            this.tabPageConfigs.TabIndex = 2;
            this.tabPageConfigs.Text = "Configurations";
            this.tabPageConfigs.UseVisualStyleBackColor = true;
            // 
            // MdiCommon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(648, 462);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.Name = "MdiCommon";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MdiAbstract_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPageEvents.ResumeLayout(false);
            this.ctmEvents.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TabControl tabControl1;
        public System.Windows.Forms.TabPage tabPageEvents;
        public System.Windows.Forms.TabPage tabPageConfigs;
        public System.Windows.Forms.ListView lsvEvents;
        public System.Windows.Forms.ColumnHeader colType;
        public System.Windows.Forms.ColumnHeader colContent;
        public System.Windows.Forms.ContextMenuStrip ctmEvents;
        public System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        public System.Windows.Forms.TabPage tabPageResults;
        private System.Windows.Forms.ColumnHeader colTime;
    }
}