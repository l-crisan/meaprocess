namespace Mp.Visual.FFT
{
    partial class FFTViewCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FFTViewCtrl));
            this.groupBoxObserver = new System.Windows.Forms.Panel();
            this.trackBarObsZ = new System.Windows.Forms.TrackBar();
            this.trackBarObsX = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonCopy = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxColor = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxResolution = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.groupBoxObserver.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarObsZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarObsX)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxObserver
            // 
            this.groupBoxObserver.BackColor = System.Drawing.SystemColors.Control;
            this.groupBoxObserver.Controls.Add(this.trackBarObsZ);
            this.groupBoxObserver.Controls.Add(this.trackBarObsX);
            this.groupBoxObserver.Controls.Add(this.label1);
            this.groupBoxObserver.Controls.Add(this.label3);
            this.groupBoxObserver.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBoxObserver.Location = new System.Drawing.Point(686, 25);
            this.groupBoxObserver.Name = "groupBoxObserver";
            this.groupBoxObserver.Size = new System.Drawing.Size(129, 491);
            this.groupBoxObserver.TabIndex = 16;
            // 
            // trackBarObsZ
            // 
            this.trackBarObsZ.Location = new System.Drawing.Point(72, 16);
            this.trackBarObsZ.Maximum = 1200;
            this.trackBarObsZ.Minimum = 200;
            this.trackBarObsZ.Name = "trackBarObsZ";
            this.trackBarObsZ.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarObsZ.Size = new System.Drawing.Size(45, 275);
            this.trackBarObsZ.TabIndex = 2;
            this.trackBarObsZ.TickFrequency = 25;
            this.trackBarObsZ.Value = 600;
            // 
            // trackBarObsX
            // 
            this.trackBarObsX.LargeChange = 1;
            this.trackBarObsX.Location = new System.Drawing.Point(16, 16);
            this.trackBarObsX.Maximum = 2000;
            this.trackBarObsX.Name = "trackBarObsX";
            this.trackBarObsX.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarObsX.Size = new System.Drawing.Size(45, 275);
            this.trackBarObsX.TabIndex = 0;
            this.trackBarObsX.TickFrequency = 50;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Incline";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(69, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Zoom";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonCopy,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.toolStripComboBoxColor,
            this.toolStripSeparator3,
            this.toolStripLabel2,
            this.toolStripComboBoxResolution,
            this.toolStripSeparator4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(815, 25);
            this.toolStrip1.TabIndex = 17;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonCopy
            // 
            this.toolStripButtonCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCopy.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCopy.Image")));
            this.toolStripButtonCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCopy.Name = "toolStripButtonCopy";
            this.toolStripButtonCopy.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonCopy.Text = "Copy";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(39, 22);
            this.toolStripLabel1.Text = "Color:";
            // 
            // toolStripComboBoxColor
            // 
            this.toolStripComboBoxColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxColor.Items.AddRange(new object[] {
            "Red",
            "Blue",
            "Green",
            "Yellow"});
            this.toolStripComboBoxColor.Name = "toolStripComboBoxColor";
            this.toolStripComboBoxColor.Size = new System.Drawing.Size(75, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(66, 22);
            this.toolStripLabel2.Text = "Resolution:";
            // 
            // toolStripComboBoxResolution
            // 
            this.toolStripComboBoxResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxResolution.Items.AddRange(new object[] {
            "5 Pixel",
            "10 Pixel",
            "15 Pixel",
            "20 Pixel"});
            this.toolStripComboBoxResolution.Name = "toolStripComboBoxResolution";
            this.toolStripComboBoxResolution.Size = new System.Drawing.Size(75, 25);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.Location = new System.Drawing.Point(669, 25);
            this.vScrollBar.Maximum = 800;
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 474);
            this.vScrollBar.TabIndex = 19;
            this.vScrollBar.Value = 200;
            // 
            // hScrollBar
            // 
            this.hScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar.Location = new System.Drawing.Point(0, 499);
            this.hScrollBar.Maximum = 800;
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(686, 17);
            this.hScrollBar.TabIndex = 18;
            this.hScrollBar.Value = 100;
            // 
            // FFTViewCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.vScrollBar);
            this.Controls.Add(this.hScrollBar);
            this.Controls.Add(this.groupBoxObserver);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FFTViewCtrl";
            this.Size = new System.Drawing.Size(815, 516);
            this.groupBoxObserver.ResumeLayout(false);
            this.groupBoxObserver.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarObsZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarObsX)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel groupBoxObserver;
        private System.Windows.Forms.TrackBar trackBarObsZ;
        private System.Windows.Forms.TrackBar trackBarObsX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonCopy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxColor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxResolution;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.HScrollBar hScrollBar;
    }
}
