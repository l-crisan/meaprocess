namespace Mp.Mod.FGen
{
        partial class SignalGeneratorViewer
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
                this.components = new System.ComponentModel.Container();
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignalGeneratorViewer));
                this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
                this.showPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.contextMenu.SuspendLayout();
                this.SuspendLayout();
                // 
                // contextMenu
                // 
                resources.ApplyResources(this.contextMenu, "contextMenu");
                this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showPointsToolStripMenuItem});
                this.contextMenu.Name = "contextMenu";
                // 
                // showPointsToolStripMenuItem
                // 
                resources.ApplyResources(this.showPointsToolStripMenuItem, "showPointsToolStripMenuItem");
                this.showPointsToolStripMenuItem.CheckOnClick = true;
                this.showPointsToolStripMenuItem.Name = "showPointsToolStripMenuItem";
                this.showPointsToolStripMenuItem.Click += new System.EventHandler(this.showPointsToolStripMenuItem_Click);
                // 
                // SignalGeneratorViewer
                // 
                resources.ApplyResources(this, "$this");
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.BackColor = System.Drawing.Color.Black;
                this.ContextMenuStrip = this.contextMenu;
                this.Name = "SignalGeneratorViewer";
                this.contextMenu.ResumeLayout(false);
                this.ResumeLayout(false);

            }

            #endregion

            private System.Windows.Forms.ContextMenuStrip contextMenu;
            private System.Windows.Forms.ToolStripMenuItem showPointsToolStripMenuItem;
        }
}
