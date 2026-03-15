namespace Mp.Visual.Oscilloscope
{
    partial class PropertyDlg
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyDlg));
            this.cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gridColor = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.editGridColor = new System.Windows.Forms.Button();
            this.ch2Color = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.editCh2Color = new System.Windows.Forms.Button();
            this.ch1Color = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.editCh1Color = new System.Windows.Forms.Button();
            this.bkColor = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.editBkColor = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancel
            // 
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
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
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.gridColor);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.editGridColor);
            this.groupBox1.Controls.Add(this.ch2Color);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.editCh2Color);
            this.groupBox1.Controls.Add(this.ch1Color);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.editCh1Color);
            this.groupBox1.Controls.Add(this.bkColor);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.editBkColor);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // gridColor
            // 
            resources.ApplyResources(this.gridColor, "gridColor");
            this.gridColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gridColor.Name = "gridColor";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // editGridColor
            // 
            resources.ApplyResources(this.editGridColor, "editGridColor");
            this.editGridColor.Name = "editGridColor";
            this.editGridColor.UseVisualStyleBackColor = true;
            this.editGridColor.Click += new System.EventHandler(this.editGridColor_Click);
            // 
            // ch2Color
            // 
            resources.ApplyResources(this.ch2Color, "ch2Color");
            this.ch2Color.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ch2Color.Name = "ch2Color";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // editCh2Color
            // 
            resources.ApplyResources(this.editCh2Color, "editCh2Color");
            this.editCh2Color.Name = "editCh2Color";
            this.editCh2Color.UseVisualStyleBackColor = true;
            this.editCh2Color.Click += new System.EventHandler(this.editCh2Color_Click);
            // 
            // ch1Color
            // 
            resources.ApplyResources(this.ch1Color, "ch1Color");
            this.ch1Color.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ch1Color.Name = "ch1Color";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // editCh1Color
            // 
            resources.ApplyResources(this.editCh1Color, "editCh1Color");
            this.editCh1Color.Name = "editCh1Color";
            this.editCh1Color.UseVisualStyleBackColor = true;
            this.editCh1Color.Click += new System.EventHandler(this.editCh1Color_Click);
            // 
            // bkColor
            // 
            resources.ApplyResources(this.bkColor, "bkColor");
            this.bkColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bkColor.Name = "bkColor";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // editBkColor
            // 
            resources.ApplyResources(this.editBkColor, "editBkColor");
            this.editBkColor.Name = "editBkColor";
            this.editBkColor.UseVisualStyleBackColor = true;
            this.editBkColor.Click += new System.EventHandler(this.editBkColor_Click);
            // 
            // PropertyDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.cancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PropertyDlg";
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel ch1Color;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button editCh1Color;
        private System.Windows.Forms.Panel bkColor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button editBkColor;
        private System.Windows.Forms.Panel ch2Color;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button editCh2Color;
        private System.Windows.Forms.Panel gridColor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button editGridColor;
    }
}