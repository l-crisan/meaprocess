namespace Mp.Mod.CAN
{
    partial class CANLoggerPortDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CANLoggerPortDlg));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.help = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.signals = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.acceptAll = new System.Windows.Forms.CheckBox();
            this.mask = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.bitRate = new System.Windows.Forms.ComboBox();
            this.code = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.adrMode = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.portNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.Channel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Signal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.signals)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
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
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // OK
            // 
            resources.ApplyResources(this.OK, "OK");
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.signals);
            this.groupBox1.Controls.Add(this.panel1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // signals
            // 
            this.signals.AllowUserToAddRows = false;
            this.signals.AllowUserToDeleteRows = false;
            this.signals.AllowUserToResizeRows = false;
            this.signals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.signals.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Channel,
            this.Signal,
            this.DataType,
            this.Column1,
            this.Column2});
            resources.ApplyResources(this.signals, "signals");
            this.signals.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.signals.Name = "signals";
            this.signals.RowHeadersVisible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rate);
            this.panel1.Controls.Add(this.label2);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // rate
            // 
            this.rate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rate.FormattingEnabled = true;
            this.rate.Items.AddRange(new object[] {
            resources.GetString("rate.Items"),
            resources.GetString("rate.Items1"),
            resources.GetString("rate.Items2"),
            resources.GetString("rate.Items3"),
            resources.GetString("rate.Items4"),
            resources.GetString("rate.Items5"),
            resources.GetString("rate.Items6"),
            resources.GetString("rate.Items7"),
            resources.GetString("rate.Items8"),
            resources.GetString("rate.Items9"),
            resources.GetString("rate.Items10"),
            resources.GetString("rate.Items11"),
            resources.GetString("rate.Items12"),
            resources.GetString("rate.Items13"),
            resources.GetString("rate.Items14"),
            resources.GetString("rate.Items15"),
            resources.GetString("rate.Items16"),
            resources.GetString("rate.Items17"),
            resources.GetString("rate.Items18")});
            resources.ApplyResources(this.rate, "rate");
            this.rate.Name = "rate";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.acceptAll);
            this.groupBox2.Controls.Add(this.mask);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.bitRate);
            this.groupBox2.Controls.Add(this.code);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.adrMode);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.portNo);
            this.groupBox2.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // acceptAll
            // 
            resources.ApplyResources(this.acceptAll, "acceptAll");
            this.acceptAll.Checked = true;
            this.acceptAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.acceptAll.Name = "acceptAll";
            this.acceptAll.UseVisualStyleBackColor = true;
            this.acceptAll.CheckedChanged += new System.EventHandler(this.acceptAll_CheckedChanged);
            // 
            // mask
            // 
            resources.ApplyResources(this.mask, "mask");
            this.mask.Name = "mask";
            this.mask.ReadOnly = true;
            this.mask.Validating += new System.ComponentModel.CancelEventHandler(this.mask_Validating);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // bitRate
            // 
            this.bitRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bitRate.FormattingEnabled = true;
            this.bitRate.Items.AddRange(new object[] {
            resources.GetString("bitRate.Items"),
            resources.GetString("bitRate.Items1"),
            resources.GetString("bitRate.Items2"),
            resources.GetString("bitRate.Items3"),
            resources.GetString("bitRate.Items4"),
            resources.GetString("bitRate.Items5"),
            resources.GetString("bitRate.Items6"),
            resources.GetString("bitRate.Items7"),
            resources.GetString("bitRate.Items8")});
            resources.ApplyResources(this.bitRate, "bitRate");
            this.bitRate.Name = "bitRate";
            // 
            // code
            // 
            resources.ApplyResources(this.code, "code");
            this.code.Name = "code";
            this.code.ReadOnly = true;
            this.code.Validating += new System.ComponentModel.CancelEventHandler(this.code_Validating);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
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
            this.adrMode.SelectedIndexChanged += new System.EventHandler(this.adrMode_SelectedIndexChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // portNo
            // 
            resources.ApplyResources(this.portNo, "portNo");
            this.portNo.Name = "portNo";
            this.portNo.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // Channel
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Channel.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.Channel, "Channel");
            this.Channel.Name = "Channel";
            this.Channel.ReadOnly = true;
            // 
            // Signal
            // 
            resources.ApplyResources(this.Signal, "Signal");
            this.Signal.Name = "Signal";
            // 
            // DataType
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.DataType.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.DataType, "DataType");
            this.DataType.Name = "DataType";
            this.DataType.ReadOnly = true;
            // 
            // Column1
            // 
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            // 
            // CANLoggerPortDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.groupBox2);
            this.Name = "CANLoggerPortDlg";
            this.ShowInTaskbar = false;
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.CANLoggerPortDlg_HelpRequested);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.signals)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView signals;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox rate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox portNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox adrMode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox bitRate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox mask;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox code;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox acceptAll;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn Channel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Signal;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    }
}