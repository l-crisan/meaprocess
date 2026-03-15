namespace Mp.Mod.Event
{
    partial class ConfigEventDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigEventDlg));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.priority = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.outputTarget = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.msgFromProp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.message = new System.Windows.Forms.TextBox();
            this.audioPanel = new System.Windows.Forms.GroupBox();
            this.onAudioFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.audioFile = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.command = new System.Windows.Forms.TextBox();
            this.cmdFromProp = new System.Windows.Forms.Button();
            this.onCommandFile = new System.Windows.Forms.Button();
            this.paramFromProp = new System.Windows.Forms.Button();
            this.commandParam = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.help = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.audioPanel.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.priority);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.outputTarget);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.msgFromProp);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.message);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // priority
            // 
            this.priority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priority.FormattingEnabled = true;
            this.priority.Items.AddRange(new object[] {
            resources.GetString("priority.Items"),
            resources.GetString("priority.Items1"),
            resources.GetString("priority.Items2")});
            resources.ApplyResources(this.priority, "priority");
            this.priority.Name = "priority";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // outputTarget
            // 
            this.outputTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.outputTarget.FormattingEnabled = true;
            this.outputTarget.Items.AddRange(new object[] {
            resources.GetString("outputTarget.Items"),
            resources.GetString("outputTarget.Items1")});
            resources.ApplyResources(this.outputTarget, "outputTarget");
            this.outputTarget.Name = "outputTarget";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // msgFromProp
            // 
            resources.ApplyResources(this.msgFromProp, "msgFromProp");
            this.msgFromProp.Name = "msgFromProp";
            this.msgFromProp.UseVisualStyleBackColor = true;
            this.msgFromProp.Click += new System.EventHandler(this.msgFromProp_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // message
            // 
            resources.ApplyResources(this.message, "message");
            this.message.Name = "message";
            // 
            // audioPanel
            // 
            this.audioPanel.Controls.Add(this.onAudioFile);
            this.audioPanel.Controls.Add(this.label2);
            this.audioPanel.Controls.Add(this.audioFile);
            resources.ApplyResources(this.audioPanel, "audioPanel");
            this.audioPanel.Name = "audioPanel";
            this.audioPanel.TabStop = false;
            // 
            // onAudioFile
            // 
            resources.ApplyResources(this.onAudioFile, "onAudioFile");
            this.onAudioFile.Name = "onAudioFile";
            this.onAudioFile.UseVisualStyleBackColor = true;
            this.onAudioFile.Click += new System.EventHandler(this.onAudioFile_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // audioFile
            // 
            resources.ApplyResources(this.audioFile, "audioFile");
            this.audioFile.Name = "audioFile";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.paramFromProp);
            this.groupBox3.Controls.Add(this.commandParam);
            this.groupBox3.Controls.Add(this.label4);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.command);
            this.groupBox4.Controls.Add(this.cmdFromProp);
            this.groupBox4.Controls.Add(this.onCommandFile);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // command
            // 
            resources.ApplyResources(this.command, "command");
            this.command.Name = "command";
            // 
            // cmdFromProp
            // 
            resources.ApplyResources(this.cmdFromProp, "cmdFromProp");
            this.cmdFromProp.Name = "cmdFromProp";
            this.cmdFromProp.UseVisualStyleBackColor = true;
            this.cmdFromProp.Click += new System.EventHandler(this.cmdFromProp_Click);
            // 
            // onCommandFile
            // 
            resources.ApplyResources(this.onCommandFile, "onCommandFile");
            this.onCommandFile.Name = "onCommandFile";
            this.onCommandFile.UseVisualStyleBackColor = true;
            this.onCommandFile.Click += new System.EventHandler(this.onCommandFile_Click);
            // 
            // paramFromProp
            // 
            resources.ApplyResources(this.paramFromProp, "paramFromProp");
            this.paramFromProp.Name = "paramFromProp";
            this.paramFromProp.UseVisualStyleBackColor = true;
            this.paramFromProp.Click += new System.EventHandler(this.paramFromProp_Click);
            // 
            // commandParam
            // 
            resources.ApplyResources(this.commandParam, "commandParam");
            this.commandParam.Name = "commandParam";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.OK, "OK");
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // cancel
            // 
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // help
            // 
            resources.ApplyResources(this.help, "help");
            this.help.Name = "help";
            this.help.UseVisualStyleBackColor = true;
            this.help.Click += new System.EventHandler(this.help_Click);
            // 
            // ConfigEventDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.OK;
            this.Controls.Add(this.help);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.audioPanel);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigEventDlg";
            this.ShowInTaskbar = false;
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.ConfigEventDlg_HelpRequested);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.audioPanel.ResumeLayout(false);
            this.audioPanel.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox message;
        private System.Windows.Forms.GroupBox audioPanel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button onAudioFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox audioFile;
        private System.Windows.Forms.Button onCommandFile;
        private System.Windows.Forms.TextBox command;
        private System.Windows.Forms.TextBox commandParam;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button cmdFromProp;
        private System.Windows.Forms.Button paramFromProp;
        private System.Windows.Forms.Button msgFromProp;
        private System.Windows.Forms.ComboBox outputTarget;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox priority;
        private System.Windows.Forms.Label label5;
    }
}