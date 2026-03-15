namespace Mp.Drv.CAN
{
    partial class CANDrvPropDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CANDrvPropDlg));
            this.OK = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deviceNo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.device = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.driver = new System.Windows.Forms.ComboBox();
            this.name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.help = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OK
            // 
            resources.ApplyResources(this.OK, "OK");
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // cancel
            // 
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.deviceNo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.device);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.driver);
            this.groupBox1.Controls.Add(this.name);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // deviceNo
            // 
            resources.ApplyResources(this.deviceNo, "deviceNo");
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
            this.deviceNo.Name = "deviceNo";
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
            // device
            // 
            resources.ApplyResources(this.device, "device");
            this.device.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.device.FormattingEnabled = true;
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
            this.device.Name = "device";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // driver
            // 
            resources.ApplyResources(this.driver, "driver");
            this.driver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.driver.FormattingEnabled = true;
            this.driver.Items.AddRange(new object[] {
            resources.GetString("driver.Items"),
            resources.GetString("driver.Items1"),
            resources.GetString("driver.Items2"),
            resources.GetString("driver.Items3")});
            this.driver.Name = "driver";
            this.driver.SelectedIndexChanged += new System.EventHandler(this.driver_SelectedIndexChanged);
            // 
            // name
            // 
            resources.ApplyResources(this.name, "name");
            this.name.Name = "name";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // help
            // 
            resources.ApplyResources(this.help, "help");
            this.help.Name = "help";
            this.help.UseVisualStyleBackColor = true;
            this.help.Click += new System.EventHandler(this.help_Click);
            // 
            // CANDrvPropDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.help);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CANDrvPropDlg";
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox driver;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox device;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.ComboBox deviceNo;
        private System.Windows.Forms.Label label4;
    }
}