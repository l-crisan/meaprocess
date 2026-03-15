namespace Mp.Mod.Statistics
{
    partial class ClassingDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassingDlg));
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.sampleRate = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.comment = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.unit = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.resetNValues = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.upperLimit = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lowerLimit = new System.Windows.Forms.TextBox();
            this.refValue = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.type = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.hysterese = new System.Windows.Forms.TextBox();
            this.classes = new System.Windows.Forms.TextBox();
            this.OK = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.help = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.errorProvider.SetError(this.groupBox1, resources.GetString("groupBox1.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox1, ((int)(resources.GetObject("groupBox1.IconPadding"))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.sampleRate);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.textBox2);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.comment);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.unit);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.name);
            this.errorProvider.SetError(this.groupBox3, resources.GetString("groupBox3.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox3, ((int)(resources.GetObject("groupBox3.IconPadding"))));
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // sampleRate
            // 
            resources.ApplyResources(this.sampleRate, "sampleRate");
            this.sampleRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider.SetError(this.sampleRate, resources.GetString("sampleRate.Error"));
            this.sampleRate.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.sampleRate, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sampleRate.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.sampleRate, ((int)(resources.GetObject("sampleRate.IconPadding"))));
            this.sampleRate.Items.AddRange(new object[] {
            resources.GetString("sampleRate.Items"),
            resources.GetString("sampleRate.Items1"),
            resources.GetString("sampleRate.Items2"),
            resources.GetString("sampleRate.Items3"),
            resources.GetString("sampleRate.Items4"),
            resources.GetString("sampleRate.Items5"),
            resources.GetString("sampleRate.Items6"),
            resources.GetString("sampleRate.Items7")});
            this.sampleRate.Name = "sampleRate";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.errorProvider.SetError(this.label12, resources.GetString("label12.Error"));
            this.errorProvider.SetIconAlignment(this.label12, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label12.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label12, ((int)(resources.GetObject("label12.IconPadding"))));
            this.label12.Name = "label12";
            // 
            // textBox2
            // 
            resources.ApplyResources(this.textBox2, "textBox2");
            this.errorProvider.SetError(this.textBox2, resources.GetString("textBox2.Error"));
            this.errorProvider.SetIconAlignment(this.textBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("textBox2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.textBox2, ((int)(resources.GetObject("textBox2.IconPadding"))));
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.errorProvider.SetError(this.label11, resources.GetString("label11.Error"));
            this.errorProvider.SetIconAlignment(this.label11, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label11.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label11, ((int)(resources.GetObject("label11.IconPadding"))));
            this.label11.Name = "label11";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.errorProvider.SetError(this.label10, resources.GetString("label10.Error"));
            this.errorProvider.SetIconAlignment(this.label10, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label10.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label10, ((int)(resources.GetObject("label10.IconPadding"))));
            this.label10.Name = "label10";
            // 
            // comment
            // 
            resources.ApplyResources(this.comment, "comment");
            this.errorProvider.SetError(this.comment, resources.GetString("comment.Error"));
            this.errorProvider.SetIconAlignment(this.comment, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("comment.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.comment, ((int)(resources.GetObject("comment.IconPadding"))));
            this.comment.Name = "comment";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.errorProvider.SetError(this.label9, resources.GetString("label9.Error"));
            this.errorProvider.SetIconAlignment(this.label9, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label9.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label9, ((int)(resources.GetObject("label9.IconPadding"))));
            this.label9.Name = "label9";
            // 
            // unit
            // 
            resources.ApplyResources(this.unit, "unit");
            this.errorProvider.SetError(this.unit, resources.GetString("unit.Error"));
            this.errorProvider.SetIconAlignment(this.unit, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("unit.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.unit, ((int)(resources.GetObject("unit.IconPadding"))));
            this.unit.Name = "unit";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.errorProvider.SetError(this.label8, resources.GetString("label8.Error"));
            this.errorProvider.SetIconAlignment(this.label8, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label8.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label8, ((int)(resources.GetObject("label8.IconPadding"))));
            this.label8.Name = "label8";
            // 
            // name
            // 
            resources.ApplyResources(this.name, "name");
            this.errorProvider.SetError(this.name, resources.GetString("name.Error"));
            this.errorProvider.SetIconAlignment(this.name, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("name.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.name, ((int)(resources.GetObject("name.IconPadding"))));
            this.name.Name = "name";
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.resetNValues);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.upperLimit);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.lowerLimit);
            this.groupBox4.Controls.Add(this.refValue);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.type);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.hysterese);
            this.groupBox4.Controls.Add(this.classes);
            this.errorProvider.SetError(this.groupBox4, resources.GetString("groupBox4.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox4, ((int)(resources.GetObject("groupBox4.IconPadding"))));
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.errorProvider.SetError(this.label14, resources.GetString("label14.Error"));
            this.errorProvider.SetIconAlignment(this.label14, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label14.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label14, ((int)(resources.GetObject("label14.IconPadding"))));
            this.label14.Name = "label14";
            // 
            // resetNValues
            // 
            resources.ApplyResources(this.resetNValues, "resetNValues");
            this.errorProvider.SetError(this.resetNValues, resources.GetString("resetNValues.Error"));
            this.errorProvider.SetIconAlignment(this.resetNValues, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("resetNValues.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.resetNValues, ((int)(resources.GetObject("resetNValues.IconPadding"))));
            this.resetNValues.Name = "resetNValues";
            this.resetNValues.Validating += new System.ComponentModel.CancelEventHandler(this.resetNValues_Validating);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.errorProvider.SetError(this.label13, resources.GetString("label13.Error"));
            this.errorProvider.SetIconAlignment(this.label13, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label13.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label13, ((int)(resources.GetObject("label13.IconPadding"))));
            this.label13.Name = "label13";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.errorProvider.SetError(this.label6, resources.GetString("label6.Error"));
            this.errorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            // 
            // upperLimit
            // 
            resources.ApplyResources(this.upperLimit, "upperLimit");
            this.errorProvider.SetError(this.upperLimit, resources.GetString("upperLimit.Error"));
            this.errorProvider.SetIconAlignment(this.upperLimit, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("upperLimit.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.upperLimit, ((int)(resources.GetObject("upperLimit.IconPadding"))));
            this.upperLimit.Name = "upperLimit";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.errorProvider.SetError(this.label7, resources.GetString("label7.Error"));
            this.errorProvider.SetIconAlignment(this.label7, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label7.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label7, ((int)(resources.GetObject("label7.IconPadding"))));
            this.label7.Name = "label7";
            // 
            // lowerLimit
            // 
            resources.ApplyResources(this.lowerLimit, "lowerLimit");
            this.errorProvider.SetError(this.lowerLimit, resources.GetString("lowerLimit.Error"));
            this.errorProvider.SetIconAlignment(this.lowerLimit, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lowerLimit.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.lowerLimit, ((int)(resources.GetObject("lowerLimit.IconPadding"))));
            this.lowerLimit.Name = "lowerLimit";
            // 
            // refValue
            // 
            resources.ApplyResources(this.refValue, "refValue");
            this.errorProvider.SetError(this.refValue, resources.GetString("refValue.Error"));
            this.errorProvider.SetIconAlignment(this.refValue, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("refValue.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.refValue, ((int)(resources.GetObject("refValue.IconPadding"))));
            this.refValue.Name = "refValue";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.errorProvider.SetError(this.label5, resources.GetString("label5.Error"));
            this.errorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label5, ((int)(resources.GetObject("label5.IconPadding"))));
            this.label5.Name = "label5";
            // 
            // type
            // 
            resources.ApplyResources(this.type, "type");
            this.type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider.SetError(this.type, resources.GetString("type.Error"));
            this.type.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.type, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("type.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.type, ((int)(resources.GetObject("type.IconPadding"))));
            this.type.Items.AddRange(new object[] {
            resources.GetString("type.Items"),
            resources.GetString("type.Items1"),
            resources.GetString("type.Items2"),
            resources.GetString("type.Items3"),
            resources.GetString("type.Items4")});
            this.type.Name = "type";
            this.type.SelectedIndexChanged += new System.EventHandler(this.type_SelectedIndexChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.errorProvider.SetError(this.label3, resources.GetString("label3.Error"));
            this.errorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label3, ((int)(resources.GetObject("label3.IconPadding"))));
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.errorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.errorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.errorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            // 
            // hysterese
            // 
            resources.ApplyResources(this.hysterese, "hysterese");
            this.errorProvider.SetError(this.hysterese, resources.GetString("hysterese.Error"));
            this.errorProvider.SetIconAlignment(this.hysterese, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("hysterese.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.hysterese, ((int)(resources.GetObject("hysterese.IconPadding"))));
            this.hysterese.Name = "hysterese";
            // 
            // classes
            // 
            resources.ApplyResources(this.classes, "classes");
            this.errorProvider.SetError(this.classes, resources.GetString("classes.Error"));
            this.errorProvider.SetIconAlignment(this.classes, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("classes.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.classes, ((int)(resources.GetObject("classes.IconPadding"))));
            this.classes.Name = "classes";
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
            this.errorProvider.SetError(this.cancel, resources.GetString("cancel.Error"));
            this.errorProvider.SetIconAlignment(this.cancel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("cancel.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.cancel, ((int)(resources.GetObject("cancel.IconPadding"))));
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // help
            // 
            resources.ApplyResources(this.help, "help");
            this.errorProvider.SetError(this.help, resources.GetString("help.Error"));
            this.errorProvider.SetIconAlignment(this.help, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("help.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.help, ((int)(resources.GetObject("help.IconPadding"))));
            this.help.Name = "help";
            this.help.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // ClassingDlg
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.help);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClassingDlg";
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox type;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox hysterese;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox classes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.TextBox refValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox lowerLimit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox upperLimit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox comment;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox unit;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox sampleRate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox resetNValues;
        private System.Windows.Forms.Label label13;
    }
}