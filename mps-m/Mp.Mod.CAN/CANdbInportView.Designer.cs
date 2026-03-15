namespace Mp.Mod.CAN
{
    partial class CANdbInportView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CANdbInportView));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.canDbTree = new Mp.Visual.Tree.Tree.TreeViewAdv();
            this.contextMenuStripCANdb = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeDataBaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.propertyView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.onSearch = new System.Windows.Forms.Button();
            this.loadDb = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox4.SuspendLayout();
            this.contextMenuStripCANdb.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.canDbTree);
            this.groupBox4.Controls.Add(this.panel3);
            this.groupBox4.Controls.Add(this.panel2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // canDbTree
            // 
            resources.ApplyResources(this.canDbTree, "canDbTree");
            this.canDbTree.BackColor = System.Drawing.SystemColors.Window;
            this.canDbTree.ContextMenuStrip = this.contextMenuStripCANdb;
            this.canDbTree.DefaultToolTipProvider = null;
            this.canDbTree.DragDropMarkColor = System.Drawing.Color.Black;
            this.canDbTree.LineColor = System.Drawing.SystemColors.ControlDark;
            this.canDbTree.Model = null;
            this.canDbTree.Name = "canDbTree";
            this.canDbTree.SelectedNode = null;
            this.canDbTree.SelectionMode = Mp.Visual.Tree.Tree.TreeSelectionMode.Multi;
            this.canDbTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.canDbTree_ItemDrag);
            this.canDbTree.SelectionChanged += new System.EventHandler(this.canDbTree_SelectionChanged);
            // 
            // contextMenuStripCANdb
            // 
            resources.ApplyResources(this.contextMenuStripCANdb, "contextMenuStripCANdb");
            this.contextMenuStripCANdb.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeDataBaseToolStripMenuItem,
            this.toolStripMenuItem1,
            this.searchToolStripMenuItem});
            this.contextMenuStripCANdb.Name = "contextMenuStripCANdb";
            this.contextMenuStripCANdb.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripCANdb_Opening);
            // 
            // removeDataBaseToolStripMenuItem
            // 
            resources.ApplyResources(this.removeDataBaseToolStripMenuItem, "removeDataBaseToolStripMenuItem");
            this.removeDataBaseToolStripMenuItem.Name = "removeDataBaseToolStripMenuItem";
            this.removeDataBaseToolStripMenuItem.Click += new System.EventHandler(this.removeDataBaseToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            // 
            // searchToolStripMenuItem
            // 
            resources.ApplyResources(this.searchToolStripMenuItem, "searchToolStripMenuItem");
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Click += new System.EventHandler(this.onSearch_Click);
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.propertyView);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Name = "panel3";
            // 
            // propertyView
            // 
            resources.ApplyResources(this.propertyView, "propertyView");
            this.propertyView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.propertyView.GridLines = true;
            this.propertyView.Name = "propertyView";
            this.propertyView.UseCompatibleStateImageBehavior = false;
            this.propertyView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.onSearch);
            this.panel1.Controls.Add(this.loadDb);
            this.panel1.Name = "panel1";
            // 
            // onSearch
            // 
            resources.ApplyResources(this.onSearch, "onSearch");
            this.onSearch.Name = "onSearch";
            this.onSearch.UseVisualStyleBackColor = true;
            this.onSearch.Click += new System.EventHandler(this.onSearch_Click);
            // 
            // loadDb
            // 
            resources.ApplyResources(this.loadDb, "loadDb");
            this.loadDb.Name = "loadDb";
            this.loadDb.UseVisualStyleBackColor = true;
            this.loadDb.Click += new System.EventHandler(this.loadDb_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Name = "panel2";
            // 
            // CANdbInportView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Name = "CANdbInportView";
            this.groupBox4.ResumeLayout(false);
            this.contextMenuStripCANdb.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private Mp.Visual.Tree.Tree.TreeViewAdv canDbTree;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ListView propertyView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button loadDb;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripCANdb;
        private System.Windows.Forms.ToolStripMenuItem removeDataBaseToolStripMenuItem;
        private System.Windows.Forms.Button onSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
    }
}
