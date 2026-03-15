namespace Mp.Visual.Digital
{
    partial class LabelLed
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
            this.label = new System.Windows.Forms.TextBox();
            this.led = new Mp.Visual.Digital.Led();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.label.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label.Location = new System.Drawing.Point(0, 36);
            this.label.Margin = new System.Windows.Forms.Padding(0);
            this.label.Name = "label";
            this.label.ReadOnly = true;
            this.label.Size = new System.Drawing.Size(72, 13);
            this.label.TabIndex = 0;
            this.label.Text = "Label";
            this.label.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // led
            // 
            this.led.BackColor = System.Drawing.Color.Transparent;
            this.led.ColorOff = System.Drawing.SystemColors.Control;
            this.led.ColorOn = System.Drawing.Color.Red;
            this.led.Dock = System.Windows.Forms.DockStyle.Fill;
            this.led.Location = new System.Drawing.Point(0, 0);
            this.led.Margin = new System.Windows.Forms.Padding(0);
            this.led.Name = "led";
            this.led.Size = new System.Drawing.Size(72, 36);
            this.led.TabIndex = 0;
            // 
            // LabelLed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.led);
            this.Controls.Add(this.label);
            this.DoubleBuffered = true;
            this.Name = "LabelLed";
            this.Size = new System.Drawing.Size(72, 49);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox label;
        private Led led;
    }
}
