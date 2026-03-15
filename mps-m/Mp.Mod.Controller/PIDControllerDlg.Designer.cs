namespace Mp.Mod.Controller
{
    partial class PIDControllerDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PIDControllerDlg));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.setPoint = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dParam = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.iParam = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pParam = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.psName = new System.Windows.Forms.TextBox();
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
            this.groupBox1.Controls.Add(this.setPoint);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dParam);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.iParam);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.pParam);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.psName);
            this.groupBox1.Controls.Add(this.label1);
            this.errorProvider.SetError(this.groupBox1, resources.GetString("groupBox1.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox1, ((int)(resources.GetObject("groupBox1.IconPadding"))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // setPoint
            // 
            resources.ApplyResources(this.setPoint, "setPoint");
            this.errorProvider.SetError(this.setPoint, resources.GetString("setPoint.Error"));
            this.errorProvider.SetIconAlignment(this.setPoint, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("setPoint.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.setPoint, ((int)(resources.GetObject("setPoint.IconPadding"))));
            this.setPoint.Name = "setPoint";
            this.setPoint.Validating += new System.ComponentModel.CancelEventHandler(this.Value_Validating);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.errorProvider.SetError(this.label5, resources.GetString("label5.Error"));
            this.errorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label5, ((int)(resources.GetObject("label5.IconPadding"))));
            this.label5.Name = "label5";
            // 
            // dParam
            // 
            resources.ApplyResources(this.dParam, "dParam");
            this.errorProvider.SetError(this.dParam, resources.GetString("dParam.Error"));
            this.errorProvider.SetIconAlignment(this.dParam, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("dParam.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.dParam, ((int)(resources.GetObject("dParam.IconPadding"))));
            this.dParam.Name = "dParam";
            this.dParam.Validating += new System.ComponentModel.CancelEventHandler(this.Value_Validating);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.errorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            // 
            // iParam
            // 
            resources.ApplyResources(this.iParam, "iParam");
            this.errorProvider.SetError(this.iParam, resources.GetString("iParam.Error"));
            this.errorProvider.SetIconAlignment(this.iParam, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("iParam.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.iParam, ((int)(resources.GetObject("iParam.IconPadding"))));
            this.iParam.Name = "iParam";
            this.iParam.Validating += new System.ComponentModel.CancelEventHandler(this.Value_Validating);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.errorProvider.SetError(this.label3, resources.GetString("label3.Error"));
            this.errorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label3, ((int)(resources.GetObject("label3.IconPadding"))));
            this.label3.Name = "label3";
            // 
            // pParam
            // 
            resources.ApplyResources(this.pParam, "pParam");
            this.errorProvider.SetError(this.pParam, resources.GetString("pParam.Error"));
            this.errorProvider.SetIconAlignment(this.pParam, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pParam.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.pParam, ((int)(resources.GetObject("pParam.IconPadding"))));
            this.pParam.Name = "pParam";
            this.pParam.Validating += new System.ComponentModel.CancelEventHandler(this.Value_Validating);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.errorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.errorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            // 
            // psName
            // 
            resources.ApplyResources(this.psName, "psName");
            this.errorProvider.SetError(this.psName, resources.GetString("psName.Error"));
            this.errorProvider.SetIconAlignment(this.psName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("psName.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.psName, ((int)(resources.GetObject("psName.IconPadding"))));
            this.psName.Name = "psName";
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
            // PIDControllerDlg
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
            this.Name = "PIDControllerDlg";
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox setPoint;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox dParam;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox iParam;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox pParam;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox psName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}