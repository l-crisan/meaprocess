namespace Mp.Visual.TextEditor
{
    partial class FindReplaceDlg
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindReplaceDlg));
      this.label1 = new System.Windows.Forms.Label();
      this.findWhat = new System.Windows.Forms.TextBox();
      this.replaceWith = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.findNext = new System.Windows.Forms.Button();
      this.replace = new System.Windows.Forms.Button();
      this.replaceAll = new System.Windows.Forms.Button();
      this.matchCase = new System.Windows.Forms.CheckBox();
      this.matchWholeWord = new System.Windows.Forms.CheckBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.label3 = new System.Windows.Forms.Label();
      this.lookIn = new System.Windows.Forms.ComboBox();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      resources.ApplyResources(this.label1, "label1");
      this.label1.Name = "label1";
      // 
      // findWhat
      // 
      resources.ApplyResources(this.findWhat, "findWhat");
      this.findWhat.Name = "findWhat";
      // 
      // replaceWith
      // 
      resources.ApplyResources(this.replaceWith, "replaceWith");
      this.replaceWith.Name = "replaceWith";
      // 
      // label2
      // 
      resources.ApplyResources(this.label2, "label2");
      this.label2.Name = "label2";
      // 
      // findNext
      // 
      resources.ApplyResources(this.findNext, "findNext");
      this.findNext.Name = "findNext";
      this.findNext.UseVisualStyleBackColor = true;
      this.findNext.Click += new System.EventHandler(this.OnFindNextClick);
      // 
      // replace
      // 
      resources.ApplyResources(this.replace, "replace");
      this.replace.Name = "replace";
      this.replace.UseVisualStyleBackColor = true;
      this.replace.Click += new System.EventHandler(this.OnReplaceClick);
      // 
      // replaceAll
      // 
      resources.ApplyResources(this.replaceAll, "replaceAll");
      this.replaceAll.Name = "replaceAll";
      this.replaceAll.UseVisualStyleBackColor = true;
      this.replaceAll.Click += new System.EventHandler(this.OnReplaceAllClick);
      // 
      // matchCase
      // 
      resources.ApplyResources(this.matchCase, "matchCase");
      this.matchCase.Name = "matchCase";
      this.matchCase.UseVisualStyleBackColor = true;
      // 
      // matchWholeWord
      // 
      resources.ApplyResources(this.matchWholeWord, "matchWholeWord");
      this.matchWholeWord.Name = "matchWholeWord";
      this.matchWholeWord.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this.lookIn);
      this.groupBox1.Controls.Add(this.matchWholeWord);
      this.groupBox1.Controls.Add(this.matchCase);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.findWhat);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.replaceWith);
      resources.ApplyResources(this.groupBox1, "groupBox1");
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.TabStop = false;
      // 
      // label3
      // 
      resources.ApplyResources(this.label3, "label3");
      this.label3.Name = "label3";
      // 
      // lookIn
      // 
      this.lookIn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.lookIn.FormattingEnabled = true;
      this.lookIn.Items.AddRange(new object[] {
            resources.GetString("lookIn.Items"),
            resources.GetString("lookIn.Items1")});
      resources.ApplyResources(this.lookIn, "lookIn");
      this.lookIn.Name = "lookIn";
      this.lookIn.SelectedIndexChanged += new System.EventHandler(this.OnLookInSelectedIndexChanged);
      // 
      // FindReplaceDlg
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.replaceAll);
      this.Controls.Add(this.replace);
      this.Controls.Add(this.findNext);
      this.Controls.Add(this.groupBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FindReplaceDlg";
      this.ShowInTaskbar = false;
      this.TopMost = true;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
      this.VisibleChanged += new System.EventHandler(this.OnVisibleChanged);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox findWhat;
        private System.Windows.Forms.TextBox replaceWith;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button findNext;
        private System.Windows.Forms.Button replace;
        private System.Windows.Forms.Button replaceAll;
        private System.Windows.Forms.CheckBox matchCase;
        private System.Windows.Forms.CheckBox matchWholeWord;
        private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox lookIn;
  }
}