namespace Mp.Visual.WaveChart
{
    partial class TimeAxisPropDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeAxisPropDlg));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._lineColor = new System.Windows.Forms.Button();
            this._degreeTextColor = new System.Windows.Forms.Button();
            this._textColor = new System.Windows.Forms.Button();
            this._bkColor = new System.Windows.Forms.Button();
            this._precision = new System.Windows.Forms.TextBox();
            this._axisDivision = new System.Windows.Forms.TextBox();
            this._timeSlot = new System.Windows.Forms.TextBox();
            this.ctrlName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this._axisText = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this._representation = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cntrlApplyOnChange = new System.Windows.Forms.CheckBox();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.Apply = new System.Windows.Forms.Button();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this._lineColor);
            this.groupBox1.Controls.Add(this._degreeTextColor);
            this.groupBox1.Controls.Add(this._textColor);
            this.groupBox1.Controls.Add(this._bkColor);
            this.groupBox1.Controls.Add(this._precision);
            this.groupBox1.Controls.Add(this._axisDivision);
            this.groupBox1.Controls.Add(this._timeSlot);
            this.groupBox1.Controls.Add(this.ctrlName);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this._axisText);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this._representation);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // _lineColor
            // 
            resources.ApplyResources(this._lineColor, "_lineColor");
            this._lineColor.Name = "_lineColor";
            this._lineColor.UseVisualStyleBackColor = true;
            this._lineColor.Click += new System.EventHandler(this._lineColor_Click);
            // 
            // _degreeTextColor
            // 
            resources.ApplyResources(this._degreeTextColor, "_degreeTextColor");
            this._degreeTextColor.Name = "_degreeTextColor";
            this._degreeTextColor.UseVisualStyleBackColor = true;
            this._degreeTextColor.Click += new System.EventHandler(this._degreeTextColor_Click);
            // 
            // _textColor
            // 
            resources.ApplyResources(this._textColor, "_textColor");
            this._textColor.Name = "_textColor";
            this._textColor.UseVisualStyleBackColor = true;
            this._textColor.Click += new System.EventHandler(this._textColor_Click);
            // 
            // _bkColor
            // 
            resources.ApplyResources(this._bkColor, "_bkColor");
            this._bkColor.Name = "_bkColor";
            this._bkColor.UseVisualStyleBackColor = true;
            this._bkColor.Click += new System.EventHandler(this._bkColor_Click);
            // 
            // _precision
            // 
            resources.ApplyResources(this._precision, "_precision");
            this._precision.Name = "_precision";
            this._precision.TextChanged += new System.EventHandler(this.OnPropertieChange);
            // 
            // _axisDivision
            // 
            resources.ApplyResources(this._axisDivision, "_axisDivision");
            this._axisDivision.Name = "_axisDivision";
            this._axisDivision.TextChanged += new System.EventHandler(this.OnPropertieChange);
            // 
            // _timeSlot
            // 
            resources.ApplyResources(this._timeSlot, "_timeSlot");
            this._timeSlot.Name = "_timeSlot";
            this._timeSlot.TextChanged += new System.EventHandler(this.OnPropertieChange);
            // 
            // ctrlName
            // 
            resources.ApplyResources(this.ctrlName, "ctrlName");
            this.ctrlName.Name = "ctrlName";
            this.ctrlName.ReadOnly = true;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // _axisText
            // 
            resources.ApplyResources(this._axisText, "_axisText");
            this._axisText.Name = "_axisText";
            this._axisText.VisibleChanged += new System.EventHandler(this.OnPropertieChange);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // _representation
            // 
            this._representation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._representation.FormattingEnabled = true;
            this._representation.Items.AddRange(new object[] {
            resources.GetString("_representation.Items"),
            resources.GetString("_representation.Items1"),
            resources.GetString("_representation.Items2")});
            resources.ApplyResources(this._representation, "_representation");
            this._representation.Name = "_representation";
            this._representation.SelectedIndexChanged += new System.EventHandler(this.OnPropertieChange);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cntrlApplyOnChange
            // 
            resources.ApplyResources(this.cntrlApplyOnChange, "cntrlApplyOnChange");
            this.cntrlApplyOnChange.Checked = true;
            this.cntrlApplyOnChange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cntrlApplyOnChange.Name = "cntrlApplyOnChange";
            this.cntrlApplyOnChange.UseVisualStyleBackColor = true;
            // 
            // OK
            // 
            resources.ApplyResources(this.OK, "OK");
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.Cancel, "Cancel");
            this.Cancel.Name = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Apply
            // 
            resources.ApplyResources(this.Apply, "Apply");
            this.Apply.Name = "Apply";
            this.Apply.UseVisualStyleBackColor = true;
            this.Apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // TimeAxisPropDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.Controls.Add(this.Apply);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.cntrlApplyOnChange);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TimeAxisPropDlg";
            this.Load += new System.EventHandler(this.PoXAxisPropDlg_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox _representation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox _axisText;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cntrlApplyOnChange;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Apply;
        private System.Windows.Forms.TextBox ctrlName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.Button _lineColor;
        private System.Windows.Forms.Button _degreeTextColor;
        private System.Windows.Forms.Button _textColor;
        private System.Windows.Forms.Button _bkColor;
        private System.Windows.Forms.TextBox _precision;
        private System.Windows.Forms.TextBox _axisDivision;
        private System.Windows.Forms.TextBox _timeSlot;
    }
}