namespace Mp.Mod.OBD2
{
    partial class PsPropDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PsPropDlg));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deviceNo = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.device = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.serialBaudRate = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.rate = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.addressMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.port = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.driver = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.help = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.deviceNo);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.device);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.serialBaudRate);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.rate);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.addressMode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.port);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.driver);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.name);
            this.groupBox1.Controls.Add(this.label1);
            this.errorProvider.SetError(this.groupBox1, resources.GetString("groupBox1.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox1, ((int)(resources.GetObject("groupBox1.IconPadding"))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // deviceNo
            // 
            resources.ApplyResources(this.deviceNo, "deviceNo");
            this.deviceNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider.SetError(this.deviceNo, resources.GetString("deviceNo.Error"));
            this.deviceNo.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.deviceNo, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("deviceNo.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.deviceNo, ((int)(resources.GetObject("deviceNo.IconPadding"))));
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
            this.deviceNo.Name = "deviceNo";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.errorProvider.SetError(this.label8, resources.GetString("label8.Error"));
            this.errorProvider.SetIconAlignment(this.label8, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label8.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label8, ((int)(resources.GetObject("label8.IconPadding"))));
            this.label8.Name = "label8";
            // 
            // device
            // 
            resources.ApplyResources(this.device, "device");
            this.device.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider.SetError(this.device, resources.GetString("device.Error"));
            this.device.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.device, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("device.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.device, ((int)(resources.GetObject("device.IconPadding"))));
            this.device.Name = "device";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.errorProvider.SetError(this.label7, resources.GetString("label7.Error"));
            this.errorProvider.SetIconAlignment(this.label7, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label7.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label7, ((int)(resources.GetObject("label7.IconPadding"))));
            this.label7.Name = "label7";
            // 
            // serialBaudRate
            // 
            resources.ApplyResources(this.serialBaudRate, "serialBaudRate");
            this.errorProvider.SetError(this.serialBaudRate, resources.GetString("serialBaudRate.Error"));
            this.serialBaudRate.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.serialBaudRate, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("serialBaudRate.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.serialBaudRate, ((int)(resources.GetObject("serialBaudRate.IconPadding"))));
            this.serialBaudRate.Items.AddRange(new object[] {
            resources.GetString("serialBaudRate.Items"),
            resources.GetString("serialBaudRate.Items1"),
            resources.GetString("serialBaudRate.Items2"),
            resources.GetString("serialBaudRate.Items3"),
            resources.GetString("serialBaudRate.Items4"),
            resources.GetString("serialBaudRate.Items5"),
            resources.GetString("serialBaudRate.Items6"),
            resources.GetString("serialBaudRate.Items7"),
            resources.GetString("serialBaudRate.Items8")});
            this.serialBaudRate.Name = "serialBaudRate";
            this.serialBaudRate.Validating += new System.ComponentModel.CancelEventHandler(this.serialBaudRate_Validating);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.errorProvider.SetError(this.label6, resources.GetString("label6.Error"));
            this.errorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            // 
            // rate
            // 
            resources.ApplyResources(this.rate, "rate");
            this.rate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider.SetError(this.rate, resources.GetString("rate.Error"));
            this.rate.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.rate, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("rate.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.rate, ((int)(resources.GetObject("rate.IconPadding"))));
            this.rate.Items.AddRange(new object[] {
            resources.GetString("rate.Items"),
            resources.GetString("rate.Items1"),
            resources.GetString("rate.Items2"),
            resources.GetString("rate.Items3"),
            resources.GetString("rate.Items4"),
            resources.GetString("rate.Items5"),
            resources.GetString("rate.Items6"),
            resources.GetString("rate.Items7")});
            this.rate.Name = "rate";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.errorProvider.SetError(this.label5, resources.GetString("label5.Error"));
            this.errorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label5, ((int)(resources.GetObject("label5.IconPadding"))));
            this.label5.Name = "label5";
            // 
            // addressMode
            // 
            resources.ApplyResources(this.addressMode, "addressMode");
            this.addressMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider.SetError(this.addressMode, resources.GetString("addressMode.Error"));
            this.addressMode.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.addressMode, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("addressMode.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.addressMode, ((int)(resources.GetObject("addressMode.IconPadding"))));
            this.addressMode.Items.AddRange(new object[] {
            resources.GetString("addressMode.Items"),
            resources.GetString("addressMode.Items1")});
            this.addressMode.Name = "addressMode";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.errorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            // 
            // port
            // 
            resources.ApplyResources(this.port, "port");
            this.port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider.SetError(this.port, resources.GetString("port.Error"));
            this.port.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.port, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("port.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.port, ((int)(resources.GetObject("port.IconPadding"))));
            this.port.Items.AddRange(new object[] {
            resources.GetString("port.Items"),
            resources.GetString("port.Items1"),
            resources.GetString("port.Items2"),
            resources.GetString("port.Items3")});
            this.port.Name = "port";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.errorProvider.SetError(this.label3, resources.GetString("label3.Error"));
            this.errorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label3, ((int)(resources.GetObject("label3.IconPadding"))));
            this.label3.Name = "label3";
            // 
            // driver
            // 
            resources.ApplyResources(this.driver, "driver");
            this.driver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider.SetError(this.driver, resources.GetString("driver.Error"));
            this.driver.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.driver, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("driver.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.driver, ((int)(resources.GetObject("driver.IconPadding"))));
            this.driver.Items.AddRange(new object[] {
            resources.GetString("driver.Items"),
            resources.GetString("driver.Items1"),
            resources.GetString("driver.Items2"),
            resources.GetString("driver.Items3"),
            resources.GetString("driver.Items4")});
            this.driver.Name = "driver";
            this.driver.SelectedIndexChanged += new System.EventHandler(this.driver_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.errorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.errorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            // 
            // name
            // 
            resources.ApplyResources(this.name, "name");
            this.errorProvider.SetError(this.name, resources.GetString("name.Error"));
            this.errorProvider.SetIconAlignment(this.name, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("name.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.name, ((int)(resources.GetObject("name.IconPadding"))));
            this.name.Name = "name";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // OK
            // 
            resources.ApplyResources(this.OK, "OK");
            this.errorProvider.SetError(this.OK, resources.GetString("OK.Error"));
            this.errorProvider.SetIconAlignment(this.OK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("OK.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.OK, ((int)(resources.GetObject("OK.IconPadding"))));
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // cancel
            // 
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.errorProvider.SetError(this.cancel, resources.GetString("cancel.Error"));
            this.errorProvider.SetIconAlignment(this.cancel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("cancel.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.cancel, ((int)(resources.GetObject("cancel.IconPadding"))));
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // help
            // 
            resources.ApplyResources(this.help, "help");
            this.errorProvider.SetError(this.help, resources.GetString("help.Error"));
            this.errorProvider.SetIconAlignment(this.help, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("help.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.help, ((int)(resources.GetObject("help.IconPadding"))));
            this.help.Name = "help";
            this.help.UseVisualStyleBackColor = true;
            this.help.Click += new System.EventHandler(this.help_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // PsPropDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.help);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PsPropDlg";
            this.ShowInTaskbar = false;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.PsPropDlg_HelpButtonClicked);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.PsPropDlg_HelpRequested);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox driver;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox addressMode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox port;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox rate;
        private System.Windows.Forms.ComboBox serialBaudRate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ComboBox device;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox deviceNo;
        private System.Windows.Forms.Label label8;
    }
}