namespace Mp.Visual.Digital
{
    partial class Switch
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.on = new System.Windows.Forms.CheckBox();
            this.off = new System.Windows.Forms.CheckBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.on);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.off);
            this.splitContainer1.Size = new System.Drawing.Size(182, 58);
            this.splitContainer1.SplitterDistance = 88;
            this.splitContainer1.TabIndex = 2;
            // 
            // on
            // 
            this.on.Appearance = System.Windows.Forms.Appearance.Button;
            this.on.AutoSize = true;
            this.on.Dock = System.Windows.Forms.DockStyle.Fill;
            this.on.Location = new System.Drawing.Point(0, 0);
            this.on.Name = "on";
            this.on.Size = new System.Drawing.Size(88, 58);
            this.on.TabIndex = 0;
            this.on.Text = "On";
            this.on.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.on.UseVisualStyleBackColor = true;
            this.on.CheckedChanged += new System.EventHandler(this.on_CheckedChanged);
            // 
            // off
            // 
            this.off.Appearance = System.Windows.Forms.Appearance.Button;
            this.off.AutoSize = true;
            this.off.Checked = true;
            this.off.CheckState = System.Windows.Forms.CheckState.Checked;
            this.off.Dock = System.Windows.Forms.DockStyle.Fill;
            this.off.Location = new System.Drawing.Point(0, 0);
            this.off.Name = "off";
            this.off.Size = new System.Drawing.Size(90, 58);
            this.off.TabIndex = 0;
            this.off.Text = "Off";
            this.off.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.off.UseVisualStyleBackColor = true;
            this.off.CheckedChanged += new System.EventHandler(this.off_CheckedChanged);
            // 
            // Switch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "Switch";
            this.Size = new System.Drawing.Size(182, 58);
            this.FontChanged += new System.EventHandler(this.Switch_FontChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox on;
        private System.Windows.Forms.CheckBox off;
    }
}
