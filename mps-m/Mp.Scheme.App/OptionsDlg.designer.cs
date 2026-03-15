namespace Mp.Scheme.App
{
    partial class OptionsDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDlg));
            this.OK = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.appaerance = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.language = new Mp.Utils.ImgComboBox();
            this.gridInterval = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.snapToGrid = new System.Windows.Forms.CheckBox();
            this.loadLastFile = new System.Windows.Forms.CheckBox();
            this.showSplashScreen = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OK
            // 
            resources.ApplyResources(this.OK, "OK");
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OnOKClick);
            // 
            // cancel
            // 
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.appaerance);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.language);
            this.groupBox1.Controls.Add(this.gridInterval);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.snapToGrid);
            this.groupBox1.Controls.Add(this.loadLastFile);
            this.groupBox1.Controls.Add(this.showSplashScreen);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // appaerance
            // 
            resources.ApplyResources(this.appaerance, "appaerance");
            this.appaerance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.appaerance.FormattingEnabled = true;
            this.appaerance.Items.AddRange(new object[] {
            resources.GetString("appaerance.Items")});
            this.appaerance.Name = "appaerance";
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
            // language
            // 
            resources.ApplyResources(this.language, "language");
            this.language.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.language.ImageList = null;
            this.language.Name = "language";
            // 
            // gridInterval
            // 
            resources.ApplyResources(this.gridInterval, "gridInterval");
            this.gridInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gridInterval.FormattingEnabled = true;
            this.gridInterval.Items.AddRange(new object[] {
            resources.GetString("gridInterval.Items"),
            resources.GetString("gridInterval.Items1"),
            resources.GetString("gridInterval.Items2")});
            this.gridInterval.Name = "gridInterval";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // snapToGrid
            // 
            resources.ApplyResources(this.snapToGrid, "snapToGrid");
            this.snapToGrid.Name = "snapToGrid";
            this.snapToGrid.UseVisualStyleBackColor = true;
            // 
            // loadLastFile
            // 
            resources.ApplyResources(this.loadLastFile, "loadLastFile");
            this.loadLastFile.Name = "loadLastFile";
            this.loadLastFile.UseVisualStyleBackColor = true;
            // 
            // showSplashScreen
            // 
            resources.ApplyResources(this.showSplashScreen, "showSplashScreen");
            this.showSplashScreen.Name = "showSplashScreen";
            this.showSplashScreen.UseVisualStyleBackColor = true;
            // 
            // OptionsDlg
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsDlg";
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox snapToGrid;
        private System.Windows.Forms.CheckBox loadLastFile;
        private System.Windows.Forms.CheckBox showSplashScreen;
        private System.Windows.Forms.ComboBox gridInterval;
        private System.Windows.Forms.Label label1;
        private Mp.Utils.ImgComboBox language;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox appaerance;
        private System.Windows.Forms.Label label3;
    }
}