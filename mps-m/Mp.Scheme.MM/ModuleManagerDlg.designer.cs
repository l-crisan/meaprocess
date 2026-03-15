namespace Mp.Scheme.MM
{
    partial class ModuleManagerDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModuleManagerDlg));
            this.OK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.help = new System.Windows.Forms.Button();
            this.onCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.moduleTree = new Mp.Visual.Tree.Tree.TreeViewAdv();
            this.treeColumn1 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn2 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn6 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn3 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn4 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn5 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.onRemove = new System.Windows.Forms.Button();
            this.onAdd = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.OK, "OK");
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OnCloseClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.help);
            this.flowLayoutPanel1.Controls.Add(this.onCancel);
            this.flowLayoutPanel1.Controls.Add(this.OK);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // help
            // 
            this.help.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.help, "help");
            this.help.Name = "help";
            this.help.UseVisualStyleBackColor = true;
            this.help.Click += new System.EventHandler(this.OnHelpClick);
            // 
            // onCancel
            // 
            this.onCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.onCancel, "onCancel");
            this.onCancel.Name = "onCancel";
            this.onCancel.UseVisualStyleBackColor = true;
            this.onCancel.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.moduleTree);
            this.groupBox1.Controls.Add(this.panel2);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // moduleTree
            // 
            this.moduleTree.BackColor = System.Drawing.SystemColors.Window;
            this.moduleTree.Columns.Add(this.treeColumn1);
            this.moduleTree.Columns.Add(this.treeColumn2);
            this.moduleTree.Columns.Add(this.treeColumn6);
            this.moduleTree.Columns.Add(this.treeColumn3);
            this.moduleTree.Columns.Add(this.treeColumn4);
            this.moduleTree.Columns.Add(this.treeColumn5);
            this.moduleTree.DefaultToolTipProvider = null;
            resources.ApplyResources(this.moduleTree, "moduleTree");
            this.moduleTree.DragDropMarkColor = System.Drawing.Color.Black;
            this.moduleTree.LineColor = System.Drawing.SystemColors.ControlDark;
            this.moduleTree.Model = null;
            this.moduleTree.Name = "moduleTree";
            this.moduleTree.SelectedNode = null;
            this.moduleTree.ShowNodeToolTips = true;
            this.moduleTree.UseColumns = true;
            // 
            // treeColumn1
            // 
            resources.ApplyResources(this.treeColumn1, "treeColumn1");
            this.treeColumn1.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // treeColumn2
            // 
            resources.ApplyResources(this.treeColumn2, "treeColumn2");
            this.treeColumn2.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // treeColumn6
            // 
            resources.ApplyResources(this.treeColumn6, "treeColumn6");
            this.treeColumn6.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // treeColumn3
            // 
            resources.ApplyResources(this.treeColumn3, "treeColumn3");
            this.treeColumn3.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // treeColumn4
            // 
            resources.ApplyResources(this.treeColumn4, "treeColumn4");
            this.treeColumn4.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // treeColumn5
            // 
            resources.ApplyResources(this.treeColumn5, "treeColumn5");
            this.treeColumn5.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.onRemove);
            this.panel2.Controls.Add(this.onAdd);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // onRemove
            // 
            resources.ApplyResources(this.onRemove, "onRemove");
            this.onRemove.Name = "onRemove";
            this.onRemove.UseVisualStyleBackColor = true;
            this.onRemove.Click += new System.EventHandler(this.OnRemoveClick);
            // 
            // onAdd
            // 
            resources.ApplyResources(this.onAdd, "onAdd");
            this.onAdd.Name = "onAdd";
            this.onAdd.UseVisualStyleBackColor = true;
            this.onAdd.Click += new System.EventHandler(this.OnnAddClick);
            // 
            // ModuleManagerDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.onCancel;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "ModuleManagerDlg";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Panel panel1;
        //        private Infragistics.Win.UltraWinDataSource.UltraDataSource _dataSource;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private Mp.Visual.Tree.Tree.TreeViewAdv moduleTree;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button onRemove;
        private System.Windows.Forms.Button onAdd;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn1;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn2;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn3;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn4;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn5;
        private System.Windows.Forms.Button help;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn6;
        private System.Windows.Forms.Button onCancel;
    }
}