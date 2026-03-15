namespace Mp.Mod.Calculation
{
    partial class SignalDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignalDlg));
            this.Cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.maximum = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.sampleRateUnit = new System.Windows.Forms.Label();
            this.minimum = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.sampleRate = new System.Windows.Forms.TextBox();
            this.dataTypeCtrl = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Comment = new System.Windows.Forms.TextBox();
            this.Unit = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SignalName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.Cancel, "Cancel");
            this.Cancel.Name = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
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
            this.groupBox1.Controls.Add(this.maximum);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.sampleRateUnit);
            this.groupBox1.Controls.Add(this.minimum);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.sampleRate);
            this.groupBox1.Controls.Add(this.dataTypeCtrl);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.Unit);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.SignalName);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // maximum
            // 
            resources.ApplyResources(this.maximum, "maximum");
            this.maximum.Name = "maximum";
            this.maximum.Validating += new System.ComponentModel.CancelEventHandler(this.Value_Validating);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // sampleRateUnit
            // 
            resources.ApplyResources(this.sampleRateUnit, "sampleRateUnit");
            this.sampleRateUnit.Name = "sampleRateUnit";
            // 
            // minimum
            // 
            resources.ApplyResources(this.minimum, "minimum");
            this.minimum.Name = "minimum";
            this.minimum.Validating += new System.ComponentModel.CancelEventHandler(this.Value_Validating);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // sampleRate
            // 
            resources.ApplyResources(this.sampleRate, "sampleRate");
            this.sampleRate.Name = "sampleRate";
            this.sampleRate.Validating += new System.ComponentModel.CancelEventHandler(this.sampleRate_Validating);
            // 
            // dataTypeCtrl
            // 
            resources.ApplyResources(this.dataTypeCtrl, "dataTypeCtrl");
            this.dataTypeCtrl.Name = "dataTypeCtrl";
            this.dataTypeCtrl.ReadOnly = true;
            this.dataTypeCtrl.TabStop = false;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Comment);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // Comment
            // 
            this.Comment.AcceptsReturn = true;
            resources.ApplyResources(this.Comment, "Comment");
            this.Comment.Name = "Comment";
            // 
            // Unit
            // 
            resources.ApplyResources(this.Unit, "Unit");
            this.Unit.Name = "Unit";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // SignalName
            // 
            resources.ApplyResources(this.SignalName, "SignalName");
            this.SignalName.Name = "SignalName";
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
            // SignalDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SignalDlg";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox Comment;
        private System.Windows.Forms.TextBox Unit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SignalName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.TextBox dataTypeCtrl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox maximum;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label sampleRateUnit;
        private System.Windows.Forms.TextBox minimum;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox sampleRate;
    }
}