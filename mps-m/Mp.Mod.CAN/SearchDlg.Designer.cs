namespace Mp.Mod.CAN
{
    partial class SearchDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchDlg));
            this.label1 = new System.Windows.Forms.Label();
            this.matchCase = new System.Windows.Forms.CheckBox();
            this.findWhat = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.matchWholeWord = new System.Windows.Forms.CheckBox();
            this.onFindAll = new System.Windows.Forms.Button();
            this.signalsFound = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // matchCase
            // 
            resources.ApplyResources(this.matchCase, "matchCase");
            this.matchCase.Name = "matchCase";
            this.matchCase.UseVisualStyleBackColor = true;
            // 
            // findWhat
            // 
            resources.ApplyResources(this.findWhat, "findWhat");
            this.findWhat.Name = "findWhat";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.matchWholeWord);
            this.groupBox1.Controls.Add(this.matchCase);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.findWhat);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // matchWholeWord
            // 
            resources.ApplyResources(this.matchWholeWord, "matchWholeWord");
            this.matchWholeWord.Name = "matchWholeWord";
            this.matchWholeWord.UseVisualStyleBackColor = true;
            // 
            // onFindAll
            // 
            resources.ApplyResources(this.onFindAll, "onFindAll");
            this.onFindAll.Name = "onFindAll";
            this.onFindAll.UseVisualStyleBackColor = true;
            this.onFindAll.Click += new System.EventHandler(this.onFindAll_Click);
            // 
            // signalsFound
            // 
            resources.ApplyResources(this.signalsFound, "signalsFound");
            this.signalsFound.ForeColor = System.Drawing.Color.Blue;
            this.signalsFound.Name = "signalsFound";
            // 
            // SearchDlg
            // 
            this.AcceptButton = this.onFindAll;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.signalsFound);
            this.Controls.Add(this.onFindAll);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchDlg";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SignalSearchDlg_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox matchCase;
        private System.Windows.Forms.TextBox findWhat;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button onFindAll;
        private System.Windows.Forms.CheckBox matchWholeWord;
        private System.Windows.Forms.Label signalsFound;
    }
}