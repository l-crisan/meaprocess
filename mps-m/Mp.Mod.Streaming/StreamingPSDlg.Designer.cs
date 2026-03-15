namespace Mp.Mod.Streaming
{
    partial class StreamingPSDlg
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StreamingPSDlg));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.conTab = new System.Windows.Forms.TabControl();
            this.network = new System.Windows.Forms.TabPage();
            this.clearIPs = new System.Windows.Forms.Button();
            this.ip = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.port = new System.Windows.Forms.TextBox();
            this.server = new System.Windows.Forms.CheckBox();
            this.serial = new System.Windows.Forms.TabPage();
            this.baudrate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.device = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.psName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.conTab.SuspendLayout();
            this.network.SuspendLayout();
            this.serial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.conTab);
            this.groupBox1.Controls.Add(this.psName);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // conTab
            // 
            this.conTab.Controls.Add(this.network);
            this.conTab.Controls.Add(this.serial);
            resources.ApplyResources(this.conTab, "conTab");
            this.conTab.Name = "conTab";
            this.conTab.SelectedIndex = 0;
            // 
            // network
            // 
            this.network.Controls.Add(this.label2);
            this.network.Controls.Add(this.clearIPs);
            this.network.Controls.Add(this.ip);
            this.network.Controls.Add(this.label3);
            this.network.Controls.Add(this.port);
            this.network.Controls.Add(this.server);
            resources.ApplyResources(this.network, "network");
            this.network.Name = "network";
            this.network.UseVisualStyleBackColor = true;
            // 
            // clearIPs
            // 
            resources.ApplyResources(this.clearIPs, "clearIPs");
            this.clearIPs.Name = "clearIPs";
            this.clearIPs.UseVisualStyleBackColor = true;
            // 
            // ip
            // 
            this.ip.FormattingEnabled = true;
            resources.ApplyResources(this.ip, "ip");
            this.ip.Name = "ip";
            this.ip.Validating += new System.ComponentModel.CancelEventHandler(this.ip_Validating);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.errorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // port
            // 
            resources.ApplyResources(this.port, "port");
            this.port.Name = "port";
            this.port.Validating += new System.ComponentModel.CancelEventHandler(this.port_Validating);
            // 
            // server
            // 
            resources.ApplyResources(this.server, "server");
            this.server.Checked = true;
            this.server.CheckState = System.Windows.Forms.CheckState.Checked;
            this.server.Name = "server";
            this.server.UseVisualStyleBackColor = true;
            // 
            // serial
            // 
            this.serial.Controls.Add(this.baudrate);
            this.serial.Controls.Add(this.label5);
            this.serial.Controls.Add(this.device);
            this.serial.Controls.Add(this.label6);
            resources.ApplyResources(this.serial, "serial");
            this.serial.Name = "serial";
            this.serial.UseVisualStyleBackColor = true;
            // 
            // baudrate
            // 
            resources.ApplyResources(this.baudrate, "baudrate");
            this.baudrate.Name = "baudrate";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // device
            // 
            resources.ApplyResources(this.device, "device");
            this.device.Name = "device";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // psName
            // 
            resources.ApplyResources(this.psName, "psName");
            this.psName.Name = "psName";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // StreamingPSDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StreamingPSDlg";
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.conTab.ResumeLayout(false);
            this.network.ResumeLayout(false);
            this.network.PerformLayout();
            this.serial.ResumeLayout(false);
            this.serial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.TextBox psName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl conTab;
        private System.Windows.Forms.TabPage network;
        private System.Windows.Forms.CheckBox server;
        private System.Windows.Forms.TabPage serial;
        private System.Windows.Forms.TextBox baudrate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox device;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button clearIPs;
        private System.Windows.Forms.ComboBox ip;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox port;
    }
}