namespace Mp.Mod.Streaming
{
    partial class EditConnectionDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditConnectionDlg));
            this.conTab = new System.Windows.Forms.TabControl();
            this.network = new System.Windows.Forms.TabPage();
            this.server = new System.Windows.Forms.CheckBox();
            this.port = new System.Windows.Forms.TextBox();
            this.ip = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.serial = new System.Windows.Forms.TabPage();
            this.baudrate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.device = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.conName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.conTab.SuspendLayout();
            this.network.SuspendLayout();
            this.serial.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // conTab
            // 
            resources.ApplyResources(this.conTab, "conTab");
            this.conTab.Controls.Add(this.network);
            this.conTab.Controls.Add(this.serial);
            this.conTab.Controls.Add(this.tabPage1);
            this.conTab.Controls.Add(this.tabPage2);
            this.errorProvider.SetError(this.conTab, resources.GetString("conTab.Error"));
            this.errorProvider.SetIconAlignment(this.conTab, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conTab.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.conTab, ((int)(resources.GetObject("conTab.IconPadding"))));
            this.conTab.Name = "conTab";
            this.conTab.SelectedIndex = 0;
            // 
            // network
            // 
            resources.ApplyResources(this.network, "network");
            this.network.Controls.Add(this.server);
            this.network.Controls.Add(this.port);
            this.network.Controls.Add(this.ip);
            this.network.Controls.Add(this.label3);
            this.network.Controls.Add(this.label2);
            this.errorProvider.SetError(this.network, resources.GetString("network.Error"));
            this.errorProvider.SetIconAlignment(this.network, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("network.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.network, ((int)(resources.GetObject("network.IconPadding"))));
            this.network.Name = "network";
            this.network.UseVisualStyleBackColor = true;
            // 
            // server
            // 
            resources.ApplyResources(this.server, "server");
            this.server.Checked = true;
            this.server.CheckState = System.Windows.Forms.CheckState.Checked;
            this.errorProvider.SetError(this.server, resources.GetString("server.Error"));
            this.errorProvider.SetIconAlignment(this.server, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("server.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.server, ((int)(resources.GetObject("server.IconPadding"))));
            this.server.Name = "server";
            this.server.UseVisualStyleBackColor = true;
            // 
            // port
            // 
            resources.ApplyResources(this.port, "port");
            this.errorProvider.SetError(this.port, resources.GetString("port.Error"));
            this.errorProvider.SetIconAlignment(this.port, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("port.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.port, ((int)(resources.GetObject("port.IconPadding"))));
            this.port.Name = "port";
            this.port.Validating += new System.ComponentModel.CancelEventHandler(this.port_Validating);
            // 
            // ip
            // 
            resources.ApplyResources(this.ip, "ip");
            this.errorProvider.SetError(this.ip, resources.GetString("ip.Error"));
            this.errorProvider.SetIconAlignment(this.ip, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("ip.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.ip, ((int)(resources.GetObject("ip.IconPadding"))));
            this.ip.Name = "ip";
            this.ip.Validating += new System.ComponentModel.CancelEventHandler(this.ip_Validating);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.errorProvider.SetError(this.label3, resources.GetString("label3.Error"));
            this.errorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label3, ((int)(resources.GetObject("label3.IconPadding"))));
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.errorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.errorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            // 
            // serial
            // 
            resources.ApplyResources(this.serial, "serial");
            this.serial.Controls.Add(this.baudrate);
            this.serial.Controls.Add(this.label5);
            this.serial.Controls.Add(this.device);
            this.serial.Controls.Add(this.label4);
            this.errorProvider.SetError(this.serial, resources.GetString("serial.Error"));
            this.errorProvider.SetIconAlignment(this.serial, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("serial.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.serial, ((int)(resources.GetObject("serial.IconPadding"))));
            this.serial.Name = "serial";
            this.serial.UseVisualStyleBackColor = true;
            // 
            // baudrate
            // 
            resources.ApplyResources(this.baudrate, "baudrate");
            this.errorProvider.SetError(this.baudrate, resources.GetString("baudrate.Error"));
            this.errorProvider.SetIconAlignment(this.baudrate, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("baudrate.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.baudrate, ((int)(resources.GetObject("baudrate.IconPadding"))));
            this.baudrate.Name = "baudrate";
            this.baudrate.Validating += new System.ComponentModel.CancelEventHandler(this.baudrate_Validating);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.errorProvider.SetError(this.label5, resources.GetString("label5.Error"));
            this.errorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label5, ((int)(resources.GetObject("label5.IconPadding"))));
            this.label5.Name = "label5";
            // 
            // device
            // 
            resources.ApplyResources(this.device, "device");
            this.errorProvider.SetError(this.device, resources.GetString("device.Error"));
            this.errorProvider.SetIconAlignment(this.device, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("device.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.device, ((int)(resources.GetObject("device.IconPadding"))));
            this.device.Name = "device";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.errorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Controls.Add(this.label6);
            this.errorProvider.SetError(this.tabPage1, resources.GetString("tabPage1.Error"));
            this.errorProvider.SetIconAlignment(this.tabPage1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tabPage1, ((int)(resources.GetObject("tabPage1.IconPadding"))));
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.errorProvider.SetError(this.label6, resources.GetString("label6.Error"));
            this.errorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Controls.Add(this.label7);
            this.errorProvider.SetError(this.tabPage2, resources.GetString("tabPage2.Error"));
            this.errorProvider.SetIconAlignment(this.tabPage2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tabPage2, ((int)(resources.GetObject("tabPage2.IconPadding"))));
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.errorProvider.SetError(this.label7, resources.GetString("label7.Error"));
            this.errorProvider.SetIconAlignment(this.label7, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label7.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label7, ((int)(resources.GetObject("label7.IconPadding"))));
            this.label7.Name = "label7";
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // conName
            // 
            resources.ApplyResources(this.conName, "conName");
            this.errorProvider.SetError(this.conName, resources.GetString("conName.Error"));
            this.errorProvider.SetIconAlignment(this.conName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conName.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.conName, ((int)(resources.GetObject("conName.IconPadding"))));
            this.conName.Name = "conName";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.conName);
            this.groupBox1.Controls.Add(this.conTab);
            this.errorProvider.SetError(this.groupBox1, resources.GetString("groupBox1.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox1, ((int)(resources.GetObject("groupBox1.IconPadding"))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // EditConnectionDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.cancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditConnectionDlg";
            this.conTab.ResumeLayout(false);
            this.network.ResumeLayout(false);
            this.network.PerformLayout();
            this.serial.ResumeLayout(false);
            this.serial.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl conTab;
        private System.Windows.Forms.TabPage network;
        private System.Windows.Forms.CheckBox server;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.TextBox ip;
        private System.Windows.Forms.TextBox conName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage serial;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox baudrate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox device;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label7;
    }
}