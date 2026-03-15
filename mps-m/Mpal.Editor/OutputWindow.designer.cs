namespace Mpal.Editor
{
    partial class OutputWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputWindow));
            this.outTextCtrl = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cleaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // outTextCtrl
            // 
            this.outTextCtrl.AccessibleDescription = null;
            this.outTextCtrl.AccessibleName = null;
            resources.ApplyResources(this.outTextCtrl, "outTextCtrl");
            this.outTextCtrl.BackColor = System.Drawing.SystemColors.Window;
            this.outTextCtrl.BackgroundImage = null;
            this.outTextCtrl.ContextMenuStrip = this.contextMenuStrip1;
            this.outTextCtrl.Name = "outTextCtrl";
            this.outTextCtrl.ReadOnly = true;
            this.outTextCtrl.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.outTextCtrl_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.AccessibleDescription = null;
            this.contextMenuStrip1.AccessibleName = null;
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.BackgroundImage = null;
            this.contextMenuStrip1.Font = null;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cleaToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            // 
            // cleaToolStripMenuItem
            // 
            this.cleaToolStripMenuItem.AccessibleDescription = null;
            this.cleaToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.cleaToolStripMenuItem, "cleaToolStripMenuItem");
            this.cleaToolStripMenuItem.BackgroundImage = null;
            this.cleaToolStripMenuItem.Name = "cleaToolStripMenuItem";
            this.cleaToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.cleaToolStripMenuItem.Click += new System.EventHandler(this.cleaToolStripMenuItem_Click);
            // 
            // OutputWindow
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.outTextCtrl);
            this.Font = null;
            this.HideOnClose = true;
            this.Name = "OutputWindow";
            this.ShowHint = Mp.Visual.Docking.DockState.DockBottomAutoHide;
            this.ToolTipText = null;
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        private System.Windows.Forms.RichTextBox outTextCtrl;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cleaToolStripMenuItem;
    }
}