namespace Mp.Scheme.Sdk
{
    partial class SplitterPSDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplitterPSDlg));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._inputTree = new Mp.Visual.Tree.Tree.TreeViewAdv();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this._outputTree = new Mp.Visual.Tree.Tree.TreeViewAdv();
            this.panel1 = new System.Windows.Forms.Panel();
            this.add = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.help = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._psName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._treeImages = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._inputTree);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // _inputTree
            // 
            this._inputTree.AllowDrop = true;
            this._inputTree.BackColor = System.Drawing.SystemColors.Window;
            this._inputTree.Cursor = System.Windows.Forms.Cursors.Default;
            this._inputTree.DefaultToolTipProvider = null;
            resources.ApplyResources(this._inputTree, "_inputTree");
            this._inputTree.DragDropMarkColor = System.Drawing.Color.Black;
            this._inputTree.LineColor = System.Drawing.SystemColors.ControlDark;
            this._inputTree.Model = null;
            this._inputTree.Name = "_inputTree";
            this._inputTree.SelectedNode = null;
            this._inputTree.SelectionMode = Mp.Visual.Tree.Tree.TreeSelectionMode.MultiSameParent;
            this._inputTree.ShowNodeToolTips = true;
            this._inputTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.OnInputTreeItemDrag);
            this._inputTree.SelectionChanged += new System.EventHandler(this.OnInputTreeSelectionChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox3);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this._outputTree);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // _outputTree
            // 
            this._outputTree.AllowDrop = true;
            this._outputTree.BackColor = System.Drawing.SystemColors.Window;
            this._outputTree.Cursor = System.Windows.Forms.Cursors.Default;
            this._outputTree.DefaultToolTipProvider = null;
            resources.ApplyResources(this._outputTree, "_outputTree");
            this._outputTree.DragDropMarkColor = System.Drawing.Color.Black;
            this._outputTree.LineColor = System.Drawing.SystemColors.ControlDark;
            this._outputTree.Model = null;
            this._outputTree.Name = "_outputTree";
            this._outputTree.SelectedNode = null;
            this._outputTree.SelectionMode = Mp.Visual.Tree.Tree.TreeSelectionMode.MultiSameParent;
            this._outputTree.ShowNodeToolTips = true;
            this._outputTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.OnOutputTreeItemDrag);
            this._outputTree.SelectionChanged += new System.EventHandler(this.OnOutputTreeSelectionChanged);
            this._outputTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnOutputTreeDragDrop);
            this._outputTree.DragOver += new System.Windows.Forms.DragEventHandler(this.OnOutputTreeDragOver);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.add);
            this.panel1.Controls.Add(this.remove);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // add
            // 
            resources.ApplyResources(this.add, "add");
            this.add.Name = "add";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.OnAddClick);
            // 
            // remove
            // 
            resources.ApplyResources(this.remove, "remove");
            this.remove.Name = "remove";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.OnRemoveClick);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.help);
            this.flowLayoutPanel1.Controls.Add(this.Cancel);
            this.flowLayoutPanel1.Controls.Add(this.OK);
            this.flowLayoutPanel1.Controls.Add(this.panel3);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // help
            // 
            resources.ApplyResources(this.help, "help");
            this.help.Name = "help";
            this.help.UseVisualStyleBackColor = true;
            this.help.Click += new System.EventHandler(this.OnHelpClick);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.Cancel, "Cancel");
            this.Cancel.Name = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.OK, "OK");
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OnOKClick);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label2);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._psName);
            this.groupBox2.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // _psName
            // 
            resources.ApplyResources(this._psName, "_psName");
            this._psName.Name = "_psName";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // _treeImages
            // 
            this._treeImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this._treeImages, "_treeImages");
            this._treeImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // SplitterPSDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "SplitterPSDlg";
            this.ShowInTaskbar = false;
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.OnHelpRequested);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox _psName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.ImageList _treeImages;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private Mp.Visual.Tree.Tree.TreeViewAdv _inputTree;
        private Mp.Visual.Tree.Tree.TreeViewAdv _outputTree;
        private System.Windows.Forms.Button help;


    }
}