namespace Mp.Scheme.Designer
{
    partial class DesignerOptionDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesignerOptionDlg));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gridSize = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.snapGrid = new System.Windows.Forms.CheckBox();
            this.snapLines = new System.Windows.Forms.CheckBox();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gridSize);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.snapGrid);
            this.groupBox1.Controls.Add(this.snapLines);
            this.groupBox1.Location = new System.Drawing.Point(12, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 104);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options:";
            // 
            // gridSize
            // 
            this.gridSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gridSize.FormattingEnabled = true;
            this.gridSize.Items.AddRange(new object[] {
            "4 Pixel",
            "8 Pixel",
            "16 Pixel",
            "32 Pixel"});
            this.gridSize.Location = new System.Drawing.Point(80, 72);
            this.gridSize.Name = "gridSize";
            this.gridSize.Size = new System.Drawing.Size(81, 21);
            this.gridSize.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Grid size:";
            // 
            // snapGrid
            // 
            this.snapGrid.AutoSize = true;
            this.snapGrid.Location = new System.Drawing.Point(23, 47);
            this.snapGrid.Name = "snapGrid";
            this.snapGrid.Size = new System.Drawing.Size(83, 17);
            this.snapGrid.TabIndex = 2;
            this.snapGrid.Text = "Snap to grid";
            this.snapGrid.UseVisualStyleBackColor = true;
            // 
            // snapLines
            // 
            this.snapLines.AutoSize = true;
            this.snapLines.Location = new System.Drawing.Point(23, 24);
            this.snapLines.Name = "snapLines";
            this.snapLines.Size = new System.Drawing.Size(87, 17);
            this.snapLines.TabIndex = 1;
            this.snapLines.Text = "Snap to lines";
            this.snapLines.UseVisualStyleBackColor = true;
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(124, 115);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 1;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OnOKClick);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(205, 115);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // DesignerOptionDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 143);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DesignerOptionDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Designer Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox gridSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox snapGrid;
        private System.Windows.Forms.CheckBox snapLines;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
    }
}