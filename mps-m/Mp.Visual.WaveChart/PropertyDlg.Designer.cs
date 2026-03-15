namespace Mp.Visual.WaveChart
{
    partial class PropertyDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyDlg));
            this.OK = new System.Windows.Forms.Button();
            this.Apply = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.ApplyOnChange = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.rate = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this._margine = new System.Windows.Forms.TextBox();
            this._bkColor = new System.Windows.Forms.Button();
            this._name = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._lineColor = new System.Windows.Forms.Button();
            this._lineWidth = new System.Windows.Forms.TextBox();
            this._yDevision = new System.Windows.Forms.TextBox();
            this._xDevision = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ctrlLineStyle = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // OK
            // 
            resources.ApplyResources(this.OK, "OK");
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Apply
            // 
            resources.ApplyResources(this.Apply, "Apply");
            this.Apply.Name = "Apply";
            this.Apply.UseVisualStyleBackColor = true;
            this.Apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.Cancel, "Cancel");
            this.Cancel.Name = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // ApplyOnChange
            // 
            resources.ApplyResources(this.ApplyOnChange, "ApplyOnChange");
            this.ApplyOnChange.Checked = true;
            this.ApplyOnChange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ApplyOnChange.Name = "ApplyOnChange";
            this.ApplyOnChange.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
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
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.rate);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this._margine);
            this.groupBox1.Controls.Add(this._bkColor);
            this.groupBox1.Controls.Add(this._name);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // rate
            // 
            resources.ApplyResources(this.rate, "rate");
            this.rate.Name = "rate";
            this.rate.TextChanged += new System.EventHandler(this.rate_TextChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // _margine
            // 
            resources.ApplyResources(this._margine, "_margine");
            this._margine.Name = "_margine";
            this._margine.TextChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // _bkColor
            // 
            resources.ApplyResources(this._bkColor, "_bkColor");
            this._bkColor.Name = "_bkColor";
            this._bkColor.UseVisualStyleBackColor = true;
            this._bkColor.Click += new System.EventHandler(this._bkColor_Click);
            // 
            // _name
            // 
            resources.ApplyResources(this._name, "_name");
            this._name.Name = "_name";
            this._name.ReadOnly = true;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this._lineColor);
            this.groupBox2.Controls.Add(this._lineWidth);
            this.groupBox2.Controls.Add(this._yDevision);
            this.groupBox2.Controls.Add(this._xDevision);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.ctrlLineStyle);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // _lineColor
            // 
            resources.ApplyResources(this._lineColor, "_lineColor");
            this._lineColor.Name = "_lineColor";
            this._lineColor.UseVisualStyleBackColor = true;
            this._lineColor.Click += new System.EventHandler(this._lineColor_Click);
            // 
            // _lineWidth
            // 
            resources.ApplyResources(this._lineWidth, "_lineWidth");
            this._lineWidth.Name = "_lineWidth";
            this._lineWidth.TextChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // _yDevision
            // 
            resources.ApplyResources(this._yDevision, "_yDevision");
            this._yDevision.Name = "_yDevision";
            this._yDevision.TextChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // _xDevision
            // 
            resources.ApplyResources(this._xDevision, "_xDevision");
            this._xDevision.Name = "_xDevision";
            this._xDevision.TextChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // ctrlLineStyle
            // 
            this.ctrlLineStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ctrlLineStyle.FormattingEnabled = true;
            this.ctrlLineStyle.Items.AddRange(new object[] {
            resources.GetString("ctrlLineStyle.Items"),
            resources.GetString("ctrlLineStyle.Items1"),
            resources.GetString("ctrlLineStyle.Items2"),
            resources.GetString("ctrlLineStyle.Items3"),
            resources.GetString("ctrlLineStyle.Items4")});
            resources.ApplyResources(this.ctrlLineStyle, "ctrlLineStyle");
            this.ctrlLineStyle.Name = "ctrlLineStyle";
            this.ctrlLineStyle.SelectedValueChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // PropertyDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ApplyOnChange);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Apply);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PropertyDlg";
            this.Load += new System.EventHandler(this.PoChartPropDlg_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Apply;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.CheckBox ApplyOnChange;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox ctrlLineStyle;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox _name;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button _bkColor;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.TextBox _margine;
        private System.Windows.Forms.Button _lineColor;
        private System.Windows.Forms.TextBox _lineWidth;
        private System.Windows.Forms.TextBox _yDevision;
        private System.Windows.Forms.TextBox _xDevision;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox rate;
        private System.Windows.Forms.Label label9;
    }
}