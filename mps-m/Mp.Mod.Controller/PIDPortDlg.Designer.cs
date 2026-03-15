namespace Mp.Mod.Controller
{
    partial class PIDPortDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PIDPortDlg));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.comment = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.unit = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.signalName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.maxValue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.minValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cancel = new System.Windows.Forms.Button();
            this.help = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.comment);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.unit);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.signalName);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.maxValue);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.minValue);
            this.groupBox2.Controls.Add(this.label4);
            this.errorProvider.SetError(this.groupBox2, resources.GetString("groupBox2.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox2, ((int)(resources.GetObject("groupBox2.IconPadding"))));
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.errorProvider.SetError(this.textBox1, resources.GetString("textBox1.Error"));
            this.errorProvider.SetIconAlignment(this.textBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("textBox1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.textBox1, ((int)(resources.GetObject("textBox1.IconPadding"))));
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            // 
            // comment
            // 
            resources.ApplyResources(this.comment, "comment");
            this.errorProvider.SetError(this.comment, resources.GetString("comment.Error"));
            this.errorProvider.SetIconAlignment(this.comment, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("comment.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.comment, ((int)(resources.GetObject("comment.IconPadding"))));
            this.comment.Name = "comment";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.errorProvider.SetError(this.label5, resources.GetString("label5.Error"));
            this.errorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label5, ((int)(resources.GetObject("label5.IconPadding"))));
            this.label5.Name = "label5";
            // 
            // unit
            // 
            resources.ApplyResources(this.unit, "unit");
            this.errorProvider.SetError(this.unit, resources.GetString("unit.Error"));
            this.errorProvider.SetIconAlignment(this.unit, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("unit.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.unit, ((int)(resources.GetObject("unit.IconPadding"))));
            this.unit.Name = "unit";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.errorProvider.SetError(this.label6, resources.GetString("label6.Error"));
            this.errorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            // 
            // signalName
            // 
            resources.ApplyResources(this.signalName, "signalName");
            this.errorProvider.SetError(this.signalName, resources.GetString("signalName.Error"));
            this.errorProvider.SetIconAlignment(this.signalName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("signalName.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.signalName, ((int)(resources.GetObject("signalName.IconPadding"))));
            this.signalName.Name = "signalName";
            this.signalName.Validating += new System.ComponentModel.CancelEventHandler(this.signalName_Validating);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.errorProvider.SetError(this.label8, resources.GetString("label8.Error"));
            this.errorProvider.SetIconAlignment(this.label8, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label8.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label8, ((int)(resources.GetObject("label8.IconPadding"))));
            this.label8.Name = "label8";
            // 
            // maxValue
            // 
            resources.ApplyResources(this.maxValue, "maxValue");
            this.errorProvider.SetError(this.maxValue, resources.GetString("maxValue.Error"));
            this.errorProvider.SetIconAlignment(this.maxValue, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("maxValue.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.maxValue, ((int)(resources.GetObject("maxValue.IconPadding"))));
            this.maxValue.Name = "maxValue";
            this.maxValue.Validating += new System.ComponentModel.CancelEventHandler(this.Value_Validating);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.errorProvider.SetError(this.label3, resources.GetString("label3.Error"));
            this.errorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label3, ((int)(resources.GetObject("label3.IconPadding"))));
            this.label3.Name = "label3";
            // 
            // minValue
            // 
            resources.ApplyResources(this.minValue, "minValue");
            this.errorProvider.SetError(this.minValue, resources.GetString("minValue.Error"));
            this.errorProvider.SetIconAlignment(this.minValue, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("minValue.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.minValue, ((int)(resources.GetObject("minValue.IconPadding"))));
            this.minValue.Name = "minValue";
            this.minValue.Validating += new System.ComponentModel.CancelEventHandler(this.Value_Validating);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.errorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
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
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // PIDPortDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.help);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PIDPortDlg";
            this.ShowInTaskbar = false;
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox comment;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox unit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox signalName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox maxValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox minValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}