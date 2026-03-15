namespace Mp.Scheme.App
{
    partial class StationPalletWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StationPalletWindow));
            this.stationPallet = new Mp.Visual.Tree.Tree.TreeViewAdv();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colapsAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.searchBlock = new System.Windows.Forms.TextBox();
            this.contextMenuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // stationPallet
            // 
            this.stationPallet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.stationPallet.ContextMenuStrip = this.contextMenuStrip;
            this.stationPallet.DefaultToolTipProvider = null;
            resources.ApplyResources(this.stationPallet, "stationPallet");
            this.stationPallet.DragDropMarkColor = System.Drawing.Color.Black;
            this.stationPallet.FullRowSelect = true;
            this.stationPallet.LineColor = System.Drawing.SystemColors.ControlDark;
            this.stationPallet.Model = null;
            this.stationPallet.Name = "stationPallet";
            this.stationPallet.RowHeight = 18;
            this.stationPallet.SelectedNode = null;
            this.stationPallet.ShowLines = false;
            this.stationPallet.ShowNodeToolTips = true;
            this.stationPallet.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.OnStationPalletItemDrag);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expandAllToolStripMenuItem,
            this.colapsAllToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            resources.ApplyResources(this.expandAllToolStripMenuItem, "expandAllToolStripMenuItem");
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.OnExpandAllClick);
            // 
            // colapsAllToolStripMenuItem
            // 
            this.colapsAllToolStripMenuItem.Name = "colapsAllToolStripMenuItem";
            resources.ApplyResources(this.colapsAllToolStripMenuItem, "colapsAllToolStripMenuItem");
            this.colapsAllToolStripMenuItem.Click += new System.EventHandler(this.OnColapseAllClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.searchBlock);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // searchBlock
            // 
            resources.ApplyResources(this.searchBlock, "searchBlock");
            this.searchBlock.Name = "searchBlock";
            this.searchBlock.TextChanged += new System.EventHandler(this.OnSearchBlockTextChanged);
            // 
            // StationPalletWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CloseButton = false;
            this.Controls.Add(this.stationPallet);
            this.Controls.Add(this.groupBox1);
            this.DockAreas = ((Mp.Visual.Docking.DockAreas)(((((Mp.Visual.Docking.DockAreas.Float | Mp.Visual.Docking.DockAreas.DockLeft) 
            | Mp.Visual.Docking.DockAreas.DockRight) 
            | Mp.Visual.Docking.DockAreas.DockTop) 
            | Mp.Visual.Docking.DockAreas.DockBottom)));
            this.Name = "StationPalletWindow";
            this.TabText = "Pallet";
            this.contextMenuStrip.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Mp.Visual.Tree.Tree.TreeViewAdv stationPallet;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colapsAllToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox searchBlock;
    }
}