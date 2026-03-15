namespace Mp.Visual.Digital
{
    partial class LimitsEditDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LimitsEditDlg));
            this.OK = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.useWarningLimits = new System.Windows.Forms.CheckBox();
            this.groupBoxWarning = new System.Windows.Forms.GroupBox();
            this.onWarningColor = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.warningColor = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.warningUpper = new System.Windows.Forms.TextBox();
            this.warningLower = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxAlarm = new System.Windows.Forms.GroupBox();
            this.onAlarmColor = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.alarmColor = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.alarmUpper = new System.Windows.Forms.TextBox();
            this.alarmLower = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.useAlarmLimits = new System.Windows.Forms.CheckBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBoxWarning.SuspendLayout();
            this.groupBoxAlarm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
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
            // useWarningLimits
            // 
            resources.ApplyResources(this.useWarningLimits, "useWarningLimits");
            this.errorProvider.SetError(this.useWarningLimits, resources.GetString("useWarningLimits.Error"));
            this.errorProvider.SetIconAlignment(this.useWarningLimits, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("useWarningLimits.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.useWarningLimits, ((int)(resources.GetObject("useWarningLimits.IconPadding"))));
            this.useWarningLimits.Name = "useWarningLimits";
            this.useWarningLimits.UseVisualStyleBackColor = true;
            this.useWarningLimits.CheckedChanged += new System.EventHandler(this.useWarningLimits_CheckedChanged);
            // 
            // groupBoxWarning
            // 
            resources.ApplyResources(this.groupBoxWarning, "groupBoxWarning");
            this.groupBoxWarning.Controls.Add(this.onWarningColor);
            this.groupBoxWarning.Controls.Add(this.label3);
            this.groupBoxWarning.Controls.Add(this.warningColor);
            this.groupBoxWarning.Controls.Add(this.label2);
            this.groupBoxWarning.Controls.Add(this.warningUpper);
            this.groupBoxWarning.Controls.Add(this.warningLower);
            this.groupBoxWarning.Controls.Add(this.label1);
            this.errorProvider.SetError(this.groupBoxWarning, resources.GetString("groupBoxWarning.Error"));
            this.errorProvider.SetIconAlignment(this.groupBoxWarning, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBoxWarning.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBoxWarning, ((int)(resources.GetObject("groupBoxWarning.IconPadding"))));
            this.groupBoxWarning.Name = "groupBoxWarning";
            this.groupBoxWarning.TabStop = false;
            // 
            // onWarningColor
            // 
            resources.ApplyResources(this.onWarningColor, "onWarningColor");
            this.errorProvider.SetError(this.onWarningColor, resources.GetString("onWarningColor.Error"));
            this.errorProvider.SetIconAlignment(this.onWarningColor, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("onWarningColor.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.onWarningColor, ((int)(resources.GetObject("onWarningColor.IconPadding"))));
            this.onWarningColor.Name = "onWarningColor";
            this.onWarningColor.UseVisualStyleBackColor = true;
            this.onWarningColor.Click += new System.EventHandler(this.onWarningColor_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.errorProvider.SetError(this.label3, resources.GetString("label3.Error"));
            this.errorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label3, ((int)(resources.GetObject("label3.IconPadding"))));
            this.label3.Name = "label3";
            // 
            // warningColor
            // 
            resources.ApplyResources(this.warningColor, "warningColor");
            this.warningColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorProvider.SetError(this.warningColor, resources.GetString("warningColor.Error"));
            this.errorProvider.SetIconAlignment(this.warningColor, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("warningColor.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.warningColor, ((int)(resources.GetObject("warningColor.IconPadding"))));
            this.warningColor.Name = "warningColor";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.errorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.errorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            // 
            // warningUpper
            // 
            resources.ApplyResources(this.warningUpper, "warningUpper");
            this.errorProvider.SetError(this.warningUpper, resources.GetString("warningUpper.Error"));
            this.errorProvider.SetIconAlignment(this.warningUpper, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("warningUpper.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.warningUpper, ((int)(resources.GetObject("warningUpper.IconPadding"))));
            this.warningUpper.Name = "warningUpper";
            this.warningUpper.Validating += new System.ComponentModel.CancelEventHandler(this.Value_Validating);
            // 
            // warningLower
            // 
            resources.ApplyResources(this.warningLower, "warningLower");
            this.errorProvider.SetError(this.warningLower, resources.GetString("warningLower.Error"));
            this.errorProvider.SetIconAlignment(this.warningLower, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("warningLower.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.warningLower, ((int)(resources.GetObject("warningLower.IconPadding"))));
            this.warningLower.Name = "warningLower";
            this.warningLower.Validating += new System.ComponentModel.CancelEventHandler(this.Value_Validating);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // groupBoxAlarm
            // 
            resources.ApplyResources(this.groupBoxAlarm, "groupBoxAlarm");
            this.groupBoxAlarm.Controls.Add(this.onAlarmColor);
            this.groupBoxAlarm.Controls.Add(this.label4);
            this.groupBoxAlarm.Controls.Add(this.alarmColor);
            this.groupBoxAlarm.Controls.Add(this.label5);
            this.groupBoxAlarm.Controls.Add(this.alarmUpper);
            this.groupBoxAlarm.Controls.Add(this.alarmLower);
            this.groupBoxAlarm.Controls.Add(this.label6);
            this.errorProvider.SetError(this.groupBoxAlarm, resources.GetString("groupBoxAlarm.Error"));
            this.errorProvider.SetIconAlignment(this.groupBoxAlarm, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBoxAlarm.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBoxAlarm, ((int)(resources.GetObject("groupBoxAlarm.IconPadding"))));
            this.groupBoxAlarm.Name = "groupBoxAlarm";
            this.groupBoxAlarm.TabStop = false;
            // 
            // onAlarmColor
            // 
            resources.ApplyResources(this.onAlarmColor, "onAlarmColor");
            this.errorProvider.SetError(this.onAlarmColor, resources.GetString("onAlarmColor.Error"));
            this.errorProvider.SetIconAlignment(this.onAlarmColor, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("onAlarmColor.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.onAlarmColor, ((int)(resources.GetObject("onAlarmColor.IconPadding"))));
            this.onAlarmColor.Name = "onAlarmColor";
            this.onAlarmColor.UseVisualStyleBackColor = true;
            this.onAlarmColor.Click += new System.EventHandler(this.onAlarmColor_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.errorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            // 
            // alarmColor
            // 
            resources.ApplyResources(this.alarmColor, "alarmColor");
            this.alarmColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorProvider.SetError(this.alarmColor, resources.GetString("alarmColor.Error"));
            this.errorProvider.SetIconAlignment(this.alarmColor, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("alarmColor.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.alarmColor, ((int)(resources.GetObject("alarmColor.IconPadding"))));
            this.alarmColor.Name = "alarmColor";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.errorProvider.SetError(this.label5, resources.GetString("label5.Error"));
            this.errorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label5, ((int)(resources.GetObject("label5.IconPadding"))));
            this.label5.Name = "label5";
            // 
            // alarmUpper
            // 
            resources.ApplyResources(this.alarmUpper, "alarmUpper");
            this.errorProvider.SetError(this.alarmUpper, resources.GetString("alarmUpper.Error"));
            this.errorProvider.SetIconAlignment(this.alarmUpper, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("alarmUpper.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.alarmUpper, ((int)(resources.GetObject("alarmUpper.IconPadding"))));
            this.alarmUpper.Name = "alarmUpper";
            this.alarmUpper.Validating += new System.ComponentModel.CancelEventHandler(this.Value_Validating);
            // 
            // alarmLower
            // 
            resources.ApplyResources(this.alarmLower, "alarmLower");
            this.errorProvider.SetError(this.alarmLower, resources.GetString("alarmLower.Error"));
            this.errorProvider.SetIconAlignment(this.alarmLower, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("alarmLower.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.alarmLower, ((int)(resources.GetObject("alarmLower.IconPadding"))));
            this.alarmLower.Name = "alarmLower";
            this.alarmLower.Validating += new System.ComponentModel.CancelEventHandler(this.Value_Validating);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.errorProvider.SetError(this.label6, resources.GetString("label6.Error"));
            this.errorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            // 
            // useAlarmLimits
            // 
            resources.ApplyResources(this.useAlarmLimits, "useAlarmLimits");
            this.errorProvider.SetError(this.useAlarmLimits, resources.GetString("useAlarmLimits.Error"));
            this.errorProvider.SetIconAlignment(this.useAlarmLimits, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("useAlarmLimits.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.useAlarmLimits, ((int)(resources.GetObject("useAlarmLimits.IconPadding"))));
            this.useAlarmLimits.Name = "useAlarmLimits";
            this.useAlarmLimits.UseVisualStyleBackColor = true;
            this.useAlarmLimits.CheckedChanged += new System.EventHandler(this.useAlarmLimits_CheckedChanged);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // LimitsEditDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.useAlarmLimits);
            this.Controls.Add(this.groupBoxAlarm);
            this.Controls.Add(this.groupBoxWarning);
            this.Controls.Add(this.useWarningLimits);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LimitsEditDlg";
            this.ShowInTaskbar = false;
            this.groupBoxWarning.ResumeLayout(false);
            this.groupBoxWarning.PerformLayout();
            this.groupBoxAlarm.ResumeLayout(false);
            this.groupBoxAlarm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.CheckBox useWarningLimits;
        private System.Windows.Forms.GroupBox groupBoxWarning;
        private System.Windows.Forms.Button onWarningColor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel warningColor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox warningUpper;
        private System.Windows.Forms.TextBox warningLower;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxAlarm;
        private System.Windows.Forms.Button onAlarmColor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel alarmColor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox alarmUpper;
        private System.Windows.Forms.TextBox alarmLower;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox useAlarmLimits;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}