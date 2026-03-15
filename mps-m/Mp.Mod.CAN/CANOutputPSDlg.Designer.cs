namespace Mp.Mod.CAN
{
    partial class CANOutputPSDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CANOutputPSDlg));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.channels = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeSignalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.edit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.driver = new System.Windows.Forms.ComboBox();
            this.OK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.deviceNo = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.device = new System.Windows.Forms.ComboBox();
            this.adrMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.port = new System.Windows.Forms.ComboBox();
            this.bitrate = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cancel = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.help = new System.Windows.Forms.Button();
            this.treeColumn1 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn2 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn3 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.channels)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            resources.ApplyResources(this.splitContainer, "splitContainer");
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.groupBox2);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.channels);
            this.groupBox2.Controls.Add(this.panel1);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // channels
            // 
            this.channels.AllowDrop = true;
            this.channels.AllowUserToAddRows = false;
            this.channels.AllowUserToDeleteRows = false;
            this.channels.AllowUserToResizeRows = false;
            this.channels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.channels.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column4,
            this.Column5,
            this.Column2,
            this.Column3});
            this.channels.ContextMenuStrip = this.contextMenuStrip;
            resources.ApplyResources(this.channels, "channels");
            this.channels.Name = "channels";
            this.channels.RowHeadersVisible = false;
            this.channels.DragDrop += new System.Windows.Forms.DragEventHandler(this.channels_DragDrop);
            this.channels.DragOver += new System.Windows.Forms.DragEventHandler(this.channels_DragOver);
            // 
            // Column1
            // 
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column4
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Column4.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.Column4, "Column4");
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.Column5, "Column5");
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column2
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.Column3, "Column3");
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeSignalToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            // 
            // removeSignalToolStripMenuItem
            // 
            this.removeSignalToolStripMenuItem.Name = "removeSignalToolStripMenuItem";
            resources.ApplyResources(this.removeSignalToolStripMenuItem, "removeSignalToolStripMenuItem");
            this.removeSignalToolStripMenuItem.Click += new System.EventHandler(this.removeSignalToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.edit);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // edit
            // 
            resources.ApplyResources(this.edit, "edit");
            this.edit.Name = "edit";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.Click += new System.EventHandler(this.edit_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // name
            // 
            resources.ApplyResources(this.name, "name");
            this.name.Name = "name";
            // 
            // driver
            // 
            this.driver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.driver.FormattingEnabled = true;
            this.driver.Items.AddRange(new object[] {
            resources.GetString("driver.Items"),
            resources.GetString("driver.Items1"),
            resources.GetString("driver.Items2"),
            resources.GetString("driver.Items3")});
            resources.ApplyResources(this.driver, "driver");
            this.driver.Name = "driver";
            this.driver.SelectedIndexChanged += new System.EventHandler(this.driver_SelectedIndexChanged);
            // 
            // OK
            // 
            resources.ApplyResources(this.OK, "OK");
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.deviceNo);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.device);
            this.groupBox1.Controls.Add(this.adrMode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.port);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.bitrate);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.driver);
            this.groupBox1.Controls.Add(this.name);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // deviceNo
            // 
            this.deviceNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deviceNo.FormattingEnabled = true;
            this.deviceNo.Items.AddRange(new object[] {
            resources.GetString("deviceNo.Items"),
            resources.GetString("deviceNo.Items1"),
            resources.GetString("deviceNo.Items2"),
            resources.GetString("deviceNo.Items3"),
            resources.GetString("deviceNo.Items4"),
            resources.GetString("deviceNo.Items5"),
            resources.GetString("deviceNo.Items6"),
            resources.GetString("deviceNo.Items7"),
            resources.GetString("deviceNo.Items8"),
            resources.GetString("deviceNo.Items9")});
            resources.ApplyResources(this.deviceNo, "deviceNo");
            this.deviceNo.Name = "deviceNo";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // device
            // 
            this.device.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.device.FormattingEnabled = true;
            resources.ApplyResources(this.device, "device");
            this.device.Name = "device";
            // 
            // adrMode
            // 
            this.adrMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.adrMode.FormattingEnabled = true;
            this.adrMode.Items.AddRange(new object[] {
            resources.GetString("adrMode.Items"),
            resources.GetString("adrMode.Items1")});
            resources.ApplyResources(this.adrMode, "adrMode");
            this.adrMode.Name = "adrMode";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // port
            // 
            this.port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.port.FormattingEnabled = true;
            this.port.Items.AddRange(new object[] {
            resources.GetString("port.Items"),
            resources.GetString("port.Items1")});
            resources.ApplyResources(this.port, "port");
            this.port.Name = "port";
            // 
            // bitrate
            // 
            this.bitrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bitrate.FormattingEnabled = true;
            this.bitrate.Items.AddRange(new object[] {
            resources.GetString("bitrate.Items"),
            resources.GetString("bitrate.Items1"),
            resources.GetString("bitrate.Items2"),
            resources.GetString("bitrate.Items3"),
            resources.GetString("bitrate.Items4"),
            resources.GetString("bitrate.Items5"),
            resources.GetString("bitrate.Items6"),
            resources.GetString("bitrate.Items7"),
            resources.GetString("bitrate.Items8")});
            resources.ApplyResources(this.bitrate, "bitrate");
            this.bitrate.Name = "bitrate";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.help);
            this.flowLayoutPanel1.Controls.Add(this.cancel);
            this.flowLayoutPanel1.Controls.Add(this.OK);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // help
            // 
            resources.ApplyResources(this.help, "help");
            this.help.Name = "help";
            this.help.UseVisualStyleBackColor = true;
            this.help.Click += new System.EventHandler(this.help_Click);
            // 
            // treeColumn1
            // 
            resources.ApplyResources(this.treeColumn1, "treeColumn1");
            this.treeColumn1.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // treeColumn2
            // 
            resources.ApplyResources(this.treeColumn2, "treeColumn2");
            this.treeColumn2.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // treeColumn3
            // 
            resources.ApplyResources(this.treeColumn3, "treeColumn3");
            this.treeColumn3.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // CANOutputPSDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "CANOutputPSDlg";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CANOutputPSDlg_FormClosing);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.CANOutputPSDlg_HelpRequested);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.channels)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.ComboBox driver;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox port;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.ComboBox bitrate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox adrMode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button edit;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn1;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn2;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn3;
        private System.Windows.Forms.DataGridView channels;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem removeSignalToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox device;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox deviceNo;
    }
}