namespace Mp.Visual.Digital
{
    partial class DigitalMeter3
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
            this.name = new System.Windows.Forms.TextBox();
            this.segments = new Mp.Visual.Digital.SevenSegmentArray();
            this.SuspendLayout();
            // 
            // name
            // 
            this.name.BackColor = System.Drawing.SystemColors.Control;
            this.name.Dock = System.Windows.Forms.DockStyle.Top;
            this.name.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name.ForeColor = System.Drawing.Color.Black;
            this.name.Location = new System.Drawing.Point(2, 2);
            this.name.Margin = new System.Windows.Forms.Padding(36, 31, 36, 31);
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.ShortcutsEnabled = false;
            this.name.Size = new System.Drawing.Size(273, 29);
            this.name.TabIndex = 5;
            this.name.Text = "Temperature";
            // 
            // segments
            // 
            this.segments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.segments.ColorBackground = System.Drawing.Color.DarkGray;
            this.segments.ColorDark = System.Drawing.Color.DimGray;
            this.segments.ColorLight = System.Drawing.Color.Red;
            this.segments.DecimalShow = true;
            this.segments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.segments.ElementPadding = new System.Windows.Forms.Padding(4);
            this.segments.Elements = 4;
            this.segments.ElementWidth = 10;
            this.segments.ItalicFactor = 0F;
            this.segments.Location = new System.Drawing.Point(2, 31);
            this.segments.Margin = new System.Windows.Forms.Padding(6);
            this.segments.Name = "segments";
            this.segments.Size = new System.Drawing.Size(273, 89);
            this.segments.TabIndex = 6;
            this.segments.Value = null;
            // 
            // DigitalMeter7Seg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.segments);
            this.Controls.Add(this.name);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "DigitalMeter7Seg";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(277, 122);
            this.BackColorChanged += new System.EventHandler(this.DigitalMeter7Seg_BackColorChanged);
            this.FontChanged += new System.EventHandler(this.DigitalMeter7Seg_FontChanged);
            this.ForeColorChanged += new System.EventHandler(this.DigitalMeter7Seg_ForeColorChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox name;
        private SevenSegmentArray segments;
    }
}
