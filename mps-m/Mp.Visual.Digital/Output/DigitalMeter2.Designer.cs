namespace Mp.Visual.Digital
{
    partial class DigitalMeter2
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataValue = new System.Windows.Forms.TextBox();
            this.name = new System.Windows.Forms.TextBox();
            this.dummy = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dataValue
            // 
            this.dataValue.BackColor = System.Drawing.SystemColors.Control;
            this.dataValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataValue.ForeColor = System.Drawing.Color.Black;
            this.dataValue.Location = new System.Drawing.Point(5, 34);
            this.dataValue.Margin = new System.Windows.Forms.Padding(18, 17, 18, 17);
            this.dataValue.Multiline = true;
            this.dataValue.Name = "dataValue";
            this.dataValue.ReadOnly = true;
            this.dataValue.ShortcutsEnabled = false;
            this.dataValue.Size = new System.Drawing.Size(271, 90);
            this.dataValue.TabIndex = 2;
            this.dataValue.Text = "23,4 °C";
            this.dataValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // name
            // 
            this.name.BackColor = System.Drawing.SystemColors.Control;
            this.name.Dock = System.Windows.Forms.DockStyle.Top;
            this.name.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name.ForeColor = System.Drawing.Color.Black;
            this.name.Location = new System.Drawing.Point(5, 5);
            this.name.Margin = new System.Windows.Forms.Padding(18, 17, 18, 17);
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.ShortcutsEnabled = false;
            this.name.Size = new System.Drawing.Size(271, 29);
            this.name.TabIndex = 3;
            this.name.Text = "Temperature";
            // 
            // dummy
            // 
            this.dummy.AutoSize = true;
            this.dummy.Location = new System.Drawing.Point(8, 51);
            this.dummy.Name = "dummy";
            this.dummy.Size = new System.Drawing.Size(0, 73);
            this.dummy.TabIndex = 4;
            // 
            // DigitalMeter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(37F, 73F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dummy);
            this.Controls.Add(this.dataValue);
            this.Controls.Add(this.name);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(18, 17, 18, 17);
            this.Name = "DigitalMeter";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(281, 129);
            this.BackColorChanged += new System.EventHandler(this.DigitalMeter_BackColorChanged);
            this.FontChanged += new System.EventHandler(this.DigitalMeter_FontChanged);
            this.ForeColorChanged += new System.EventHandler(this.DigitalMeter_ForeColorChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox dataValue;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label dummy;


    }
}
