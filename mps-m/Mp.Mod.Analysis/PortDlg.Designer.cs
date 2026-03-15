namespace Mp.Mod.Analysis
{
    partial class PortDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PortDlg));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.unit = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.max = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.min = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comment = new System.Windows.Forms.TextBox();
            this.OK = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.textBox6);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.unit);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.max);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.min);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.name);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.errorProvider.SetError(this.groupBox1, resources.GetString("groupBox1.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox1, ((int)(resources.GetObject("groupBox1.IconPadding"))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // textBox6
            // 
            resources.ApplyResources(this.textBox6, "textBox6");
            this.errorProvider.SetError(this.textBox6, resources.GetString("textBox6.Error"));
            this.errorProvider.SetIconAlignment(this.textBox6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("textBox6.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.textBox6, ((int)(resources.GetObject("textBox6.IconPadding"))));
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.errorProvider.SetError(this.label6, resources.GetString("label6.Error"));
            this.errorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            // 
            // unit
            // 
            resources.ApplyResources(this.unit, "unit");
            this.errorProvider.SetError(this.unit, resources.GetString("unit.Error"));
            this.errorProvider.SetIconAlignment(this.unit, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("unit.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.unit, ((int)(resources.GetObject("unit.IconPadding"))));
            this.unit.Name = "unit";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.errorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            // 
            // max
            // 
            resources.ApplyResources(this.max, "max");
            this.errorProvider.SetError(this.max, resources.GetString("max.Error"));
            this.errorProvider.SetIconAlignment(this.max, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("max.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.max, ((int)(resources.GetObject("max.IconPadding"))));
            this.max.Name = "max";
            this.max.Validating += new System.ComponentModel.CancelEventHandler(this.max_Validating);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.errorProvider.SetError(this.label3, resources.GetString("label3.Error"));
            this.errorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label3, ((int)(resources.GetObject("label3.IconPadding"))));
            this.label3.Name = "label3";
            // 
            // min
            // 
            resources.ApplyResources(this.min, "min");
            this.errorProvider.SetError(this.min, resources.GetString("min.Error"));
            this.errorProvider.SetIconAlignment(this.min, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("min.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.min, ((int)(resources.GetObject("min.IconPadding"))));
            this.min.Name = "min";
            this.min.Validating += new System.ComponentModel.CancelEventHandler(this.min_Validating);
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
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.comment);
            this.errorProvider.SetError(this.groupBox2, resources.GetString("groupBox2.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox2, ((int)(resources.GetObject("groupBox2.IconPadding"))));
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // comment
            // 
            this.comment.AcceptsReturn = true;
            resources.ApplyResources(this.comment, "comment");
            this.errorProvider.SetError(this.comment, resources.GetString("comment.Error"));
            this.errorProvider.SetIconAlignment(this.comment, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("comment.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.comment, ((int)(resources.GetObject("comment.IconPadding"))));
            this.comment.Name = "comment";
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
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // PortDlg
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
            this.Name = "PortDlg";
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox comment;
        private System.Windows.Forms.TextBox unit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox max;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox min;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}