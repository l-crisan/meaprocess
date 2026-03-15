namespace Mpal.Editor
{
    partial class DocumentForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentForm));
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.textCtrl = new ICSharpCode.TextEditor.TextEditorControl();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.haltepunktToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertBPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteBPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.enableBPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableBPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // textCtrl
            // 
            resources.ApplyResources(this.textCtrl, "textCtrl");
            this.textCtrl.AllowDrop = true;
            this.textCtrl.ContextMenuStrip = this.contextMenuStrip;
            this.textCtrl.IsIconBarVisible = true;
            this.textCtrl.IsReadOnly = false;
            this.textCtrl.LineViewerStyle = ICSharpCode.TextEditor.Document.LineViewerStyle.FullRow;
            this.textCtrl.Name = "textCtrl";
            this.toolTip.SetToolTip(this.textCtrl, resources.GetString("textCtrl.ToolTip"));
            this.textCtrl.TextChanged += new System.EventHandler(this.textCtrl_TextChanged);
            this.textCtrl.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.textCtrl_HelpRequested);
            // 
            // contextMenuStrip
            // 
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this.toolStripMenuItem2,
            this.haltepunktToolStripMenuItem,
            this.toolStripMenuItem3,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripMenuItem1,
            this.selectAllToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.toolTip.SetToolTip(this.contextMenuStrip, resources.GetString("contextMenuStrip.ToolTip"));
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // helpToolStripMenuItem
            // 
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            // 
            // haltepunktToolStripMenuItem
            // 
            resources.ApplyResources(this.haltepunktToolStripMenuItem, "haltepunktToolStripMenuItem");
            this.haltepunktToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertBPToolStripMenuItem,
            this.deleteBPToolStripMenuItem,
            this.toolStripMenuItem4,
            this.enableBPToolStripMenuItem,
            this.disableBPToolStripMenuItem});
            this.haltepunktToolStripMenuItem.Name = "haltepunktToolStripMenuItem";
            // 
            // insertBPToolStripMenuItem
            // 
            resources.ApplyResources(this.insertBPToolStripMenuItem, "insertBPToolStripMenuItem");
            this.insertBPToolStripMenuItem.Name = "insertBPToolStripMenuItem";
            this.insertBPToolStripMenuItem.Click += new System.EventHandler(this.insertBPToolStripMenuItem_Click);
            // 
            // deleteBPToolStripMenuItem
            // 
            resources.ApplyResources(this.deleteBPToolStripMenuItem, "deleteBPToolStripMenuItem");
            this.deleteBPToolStripMenuItem.Name = "deleteBPToolStripMenuItem";
            this.deleteBPToolStripMenuItem.Click += new System.EventHandler(this.deleteBPToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            resources.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            // 
            // enableBPToolStripMenuItem
            // 
            resources.ApplyResources(this.enableBPToolStripMenuItem, "enableBPToolStripMenuItem");
            this.enableBPToolStripMenuItem.Name = "enableBPToolStripMenuItem";
            this.enableBPToolStripMenuItem.Click += new System.EventHandler(this.enableBPToolStripMenuItem_Click);
            // 
            // disableBPToolStripMenuItem
            // 
            resources.ApplyResources(this.disableBPToolStripMenuItem, "disableBPToolStripMenuItem");
            this.disableBPToolStripMenuItem.Name = "disableBPToolStripMenuItem";
            this.disableBPToolStripMenuItem.Click += new System.EventHandler(this.disableBPToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            // 
            // cutToolStripMenuItem
            // 
            resources.ApplyResources(this.cutToolStripMenuItem, "cutToolStripMenuItem");
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            resources.ApplyResources(this.pasteToolStripMenuItem, "pasteToolStripMenuItem");
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            // 
            // selectAllToolStripMenuItem
            // 
            resources.ApplyResources(this.selectAllToolStripMenuItem, "selectAllToolStripMenuItem");
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // DocumentForm
            // 
            resources.ApplyResources(this, "$this");
            this.AllowDrop = true;
            this.Controls.Add(this.textCtrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "DocumentForm";
            this.ShowInTaskbar = false;
            this.toolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DocumentForm_FormClosing);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        private System.Windows.Forms.ToolTip toolTip;
        private ICSharpCode.TextEditor.TextEditorControl textCtrl;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem haltepunktToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertBPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteBPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableBPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableBPToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
    }
}