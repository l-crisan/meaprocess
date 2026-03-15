namespace Mp.Mod.CAN
{
    partial class CANEventPSDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CANEventPSDlg));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.signals = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.events = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Limit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CANID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.deviceNo = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.device = new System.Windows.Forms.ComboBox();
            this.adrMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.port = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bitrate = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.driver = new System.Windows.Forms.ComboBox();
            this.name = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.help = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.events)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.errorProvider.SetIconAlignment(this.splitContainer1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.IconAlignment"))));
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.errorProvider.SetIconAlignment(this.splitContainer1.Panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.Panel1.IconAlignment"))));
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.errorProvider.SetIconAlignment(this.splitContainer1.Panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.Panel2.IconAlignment"))));
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.signals);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.errorProvider.SetIconAlignment(this.groupBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox2.IconAlignment"))));
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // signals
            // 
            this.signals.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            resources.ApplyResources(this.signals, "signals");
            this.errorProvider.SetIconAlignment(this.signals, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("signals.IconAlignment"))));
            this.signals.Name = "signals";
            this.signals.UseCompatibleStateImageBehavior = false;
            this.signals.View = System.Windows.Forms.View.Details;
            this.signals.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.signals_ItemDrag);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.events);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.errorProvider.SetIconAlignment(this.groupBox3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox3.IconAlignment"))));
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // events
            // 
            this.events.AllowDrop = true;
            this.events.AllowUserToAddRows = false;
            this.events.AllowUserToResizeRows = false;
            this.events.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.events.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column3,
            this.Limit,
            this.CANID,
            this.Column4});
            this.events.ContextMenuStrip = this.contextMenuStrip;
            resources.ApplyResources(this.events, "events");
            this.events.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.errorProvider.SetIconAlignment(this.events, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("events.IconAlignment"))));
            this.events.Name = "events";
            this.events.RowHeadersVisible = false;
            this.events.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.events_CellClick);
            this.events.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.events_CellValidating);
            this.events.DragDrop += new System.Windows.Forms.DragEventHandler(this.events_DragDrop);
            this.events.DragOver += new System.Windows.Forms.DragEventHandler(this.events_DragOver);
            // 
            // Column1
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column3
            // 
            resources.ApplyResources(this.Column3, "Column3");
            this.Column3.Items.AddRange(new object[] {
            "<>",
            "=",
            "<",
            "<=",
            ">",
            ">="});
            this.Column3.Name = "Column3";
            // 
            // Limit
            // 
            resources.ApplyResources(this.Limit, "Limit");
            this.Limit.Name = "Limit";
            // 
            // CANID
            // 
            resources.ApplyResources(this.CANID, "CANID");
            this.CANID.Name = "CANID";
            // 
            // Column4
            // 
            resources.ApplyResources(this.Column4, "Column4");
            this.Column4.Name = "Column4";
            // 
            // contextMenuStrip
            // 
            this.errorProvider.SetIconAlignment(this.contextMenuStrip, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("contextMenuStrip.IconAlignment"))));
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeEventToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            // 
            // removeEventToolStripMenuItem
            // 
            this.removeEventToolStripMenuItem.Name = "removeEventToolStripMenuItem";
            resources.ApplyResources(this.removeEventToolStripMenuItem, "removeEventToolStripMenuItem");
            this.removeEventToolStripMenuItem.Click += new System.EventHandler(this.removeEventToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.deviceNo);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.device);
            this.groupBox1.Controls.Add(this.adrMode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.port);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.bitrate);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.driver);
            this.groupBox1.Controls.Add(this.name);
            this.groupBox1.Controls.Add(this.label5);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.errorProvider.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label8
            // 
            this.errorProvider.SetIconAlignment(this.label8, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label8.IconAlignment"))));
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // deviceNo
            // 
            this.deviceNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deviceNo.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.deviceNo, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("deviceNo.IconAlignment"))));
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
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.errorProvider.SetIconAlignment(this.label7, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label7.IconAlignment"))));
            this.label7.Name = "label7";
            // 
            // device
            // 
            this.device.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.device.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.device, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("device.IconAlignment"))));
            this.device.Items.AddRange(new object[] {
            resources.GetString("device.Items"),
            resources.GetString("device.Items1"),
            resources.GetString("device.Items2"),
            resources.GetString("device.Items3"),
            resources.GetString("device.Items4"),
            resources.GetString("device.Items5"),
            resources.GetString("device.Items6"),
            resources.GetString("device.Items7"),
            resources.GetString("device.Items8"),
            resources.GetString("device.Items9"),
            resources.GetString("device.Items10"),
            resources.GetString("device.Items11"),
            resources.GetString("device.Items12"),
            resources.GetString("device.Items13"),
            resources.GetString("device.Items14")});
            resources.ApplyResources(this.device, "device");
            this.device.Name = "device";
            // 
            // adrMode
            // 
            this.adrMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.adrMode.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.adrMode, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("adrMode.IconAlignment"))));
            this.adrMode.Items.AddRange(new object[] {
            resources.GetString("adrMode.Items"),
            resources.GetString("adrMode.Items1")});
            resources.ApplyResources(this.adrMode, "adrMode");
            this.adrMode.Name = "adrMode";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.errorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.label3.Name = "label3";
            // 
            // port
            // 
            this.port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.port.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.port, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("port.IconAlignment"))));
            this.port.Items.AddRange(new object[] {
            resources.GetString("port.Items"),
            resources.GetString("port.Items1")});
            resources.ApplyResources(this.port, "port");
            this.port.Name = "port";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.label1.Name = "label1";
            // 
            // bitrate
            // 
            this.bitrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bitrate.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.bitrate, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("bitrate.IconAlignment"))));
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
            this.errorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.label6.Name = "label6";
            // 
            // driver
            // 
            this.driver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.driver.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.driver, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("driver.IconAlignment"))));
            this.driver.Items.AddRange(new object[] {
            resources.GetString("driver.Items"),
            resources.GetString("driver.Items1"),
            resources.GetString("driver.Items2"),
            resources.GetString("driver.Items3")});
            resources.ApplyResources(this.driver, "driver");
            this.driver.Name = "driver";
            this.driver.SelectedIndexChanged += new System.EventHandler(this.driver_SelectedIndexChanged);
            // 
            // name
            // 
            this.errorProvider.SetIconAlignment(this.name, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("name.IconAlignment"))));
            resources.ApplyResources(this.name, "name");
            this.name.Name = "name";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.errorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.label5.Name = "label5";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.help);
            this.flowLayoutPanel1.Controls.Add(this.Cancel);
            this.flowLayoutPanel1.Controls.Add(this.OK);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.errorProvider.SetIconAlignment(this.flowLayoutPanel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("flowLayoutPanel1.IconAlignment"))));
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // help
            // 
            this.errorProvider.SetIconAlignment(this.help, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("help.IconAlignment"))));
            resources.ApplyResources(this.help, "help");
            this.help.Name = "help";
            this.help.UseVisualStyleBackColor = true;
            this.help.Click += new System.EventHandler(this.help_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.errorProvider.SetIconAlignment(this.Cancel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("Cancel.IconAlignment"))));
            resources.ApplyResources(this.Cancel, "Cancel");
            this.Cancel.Name = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // OK
            // 
            this.errorProvider.SetIconAlignment(this.OK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("OK.IconAlignment"))));
            resources.ApplyResources(this.OK, "OK");
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.errorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.label2.Name = "label2";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // CANEventPSDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "CANEventPSDlg";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.EventPSDlg_Load);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.EventPSDlg_HelpRequested);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.events)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView signals;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.DataGridView events;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem removeEventToolStripMenuItem;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ComboBox adrMode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox port;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox bitrate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox driver;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Limit;
        private System.Windows.Forms.DataGridViewTextBoxColumn CANID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox device;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox deviceNo;
    }
}