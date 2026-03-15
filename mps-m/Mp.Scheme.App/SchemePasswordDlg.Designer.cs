namespace Mp.Scheme.App
{
    partial class SchemePasswordDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchemePasswordDlg));
            this.schemeGroup = new System.Windows.Forms.GroupBox();
            this.schemePassword2 = new System.Windows.Forms.TextBox();
            this.schemePassword1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.scheme = new System.Windows.Forms.CheckBox();
            this.runtimeGroup = new System.Windows.Forms.GroupBox();
            this.runtimePassword2 = new System.Windows.Forms.TextBox();
            this.runtimePassword1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.runtime = new System.Windows.Forms.CheckBox();
            this.OK = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.help = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.schemeGroup.SuspendLayout();
            this.runtimeGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // schemeGroup
            // 
            this.schemeGroup.Controls.Add(this.schemePassword2);
            this.schemeGroup.Controls.Add(this.schemePassword1);
            this.schemeGroup.Controls.Add(this.label2);
            this.schemeGroup.Controls.Add(this.label1);
            resources.ApplyResources(this.schemeGroup, "schemeGroup");
            this.schemeGroup.Name = "schemeGroup";
            this.schemeGroup.TabStop = false;
            // 
            // schemePassword2
            // 
            resources.ApplyResources(this.schemePassword2, "schemePassword2");
            this.schemePassword2.Name = "schemePassword2";
            this.schemePassword2.UseSystemPasswordChar = true;
            // 
            // schemePassword1
            // 
            resources.ApplyResources(this.schemePassword1, "schemePassword1");
            this.schemePassword1.Name = "schemePassword1";
            this.schemePassword1.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // scheme
            // 
            resources.ApplyResources(this.scheme, "scheme");
            this.scheme.Name = "scheme";
            this.scheme.UseVisualStyleBackColor = true;
            this.scheme.CheckedChanged += new System.EventHandler(this.OnSchemeCheckedChanged);
            // 
            // runtimeGroup
            // 
            this.runtimeGroup.Controls.Add(this.runtimePassword2);
            this.runtimeGroup.Controls.Add(this.runtimePassword1);
            this.runtimeGroup.Controls.Add(this.label3);
            this.runtimeGroup.Controls.Add(this.label4);
            resources.ApplyResources(this.runtimeGroup, "runtimeGroup");
            this.runtimeGroup.Name = "runtimeGroup";
            this.runtimeGroup.TabStop = false;
            // 
            // runtimePassword2
            // 
            resources.ApplyResources(this.runtimePassword2, "runtimePassword2");
            this.runtimePassword2.Name = "runtimePassword2";
            this.runtimePassword2.UseSystemPasswordChar = true;
            // 
            // runtimePassword1
            // 
            resources.ApplyResources(this.runtimePassword1, "runtimePassword1");
            this.runtimePassword1.Name = "runtimePassword1";
            this.runtimePassword1.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // runtime
            // 
            resources.ApplyResources(this.runtime, "runtime");
            this.runtime.Name = "runtime";
            this.runtime.UseVisualStyleBackColor = true;
            this.runtime.CheckedChanged += new System.EventHandler(this.OnRuntimeCheckedChanged);
            // 
            // OK
            // 
            resources.ApplyResources(this.OK, "OK");
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OnOKClick);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // help
            // 
            resources.ApplyResources(this.help, "help");
            this.help.Name = "help";
            this.help.UseVisualStyleBackColor = true;
            this.help.Click += new System.EventHandler(this.OnHelpClick);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // SchemePasswordDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.scheme);
            this.Controls.Add(this.runtime);
            this.Controls.Add(this.help);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.runtimeGroup);
            this.Controls.Add(this.schemeGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SchemePasswordDlg";
            this.ShowInTaskbar = false;
            this.schemeGroup.ResumeLayout(false);
            this.schemeGroup.PerformLayout();
            this.runtimeGroup.ResumeLayout(false);
            this.runtimeGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox schemeGroup;
        private System.Windows.Forms.CheckBox scheme;
        private System.Windows.Forms.TextBox schemePassword2;
        private System.Windows.Forms.TextBox schemePassword1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox runtimeGroup;
        private System.Windows.Forms.TextBox runtimePassword2;
        private System.Windows.Forms.TextBox runtimePassword1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox runtime;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}