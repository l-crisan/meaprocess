namespace Mpal.Editor
{
    partial class DebugVariableView
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugVariableView));
            this.Variable = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn1 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn2 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.inputVarTree = new Mp.Visual.Tree.Tree.TreeViewAdv();
            this.treeColumn3 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.SuspendLayout();
            // 
            // Variable
            // 
            resources.ApplyResources(this.Variable, "Variable");
            this.Variable.SortOrder = System.Windows.Forms.SortOrder.None;
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
            // inputVarTree
            // 
            this.inputVarTree.BackColor = System.Drawing.SystemColors.Window;
            this.inputVarTree.Columns.Add(this.treeColumn1);
            this.inputVarTree.Columns.Add(this.treeColumn2);
            this.inputVarTree.Columns.Add(this.treeColumn3);
            this.inputVarTree.DefaultToolTipProvider = null;
            resources.ApplyResources(this.inputVarTree, "inputVarTree");
            this.inputVarTree.DragDropMarkColor = System.Drawing.Color.Black;
            this.inputVarTree.LineColor = System.Drawing.SystemColors.ControlDark;
            this.inputVarTree.Model = null;
            this.inputVarTree.Name = "inputVarTree";
            this.inputVarTree.SelectedNode = null;
            this.inputVarTree.ShowNodeToolTips = true;
            this.inputVarTree.UseColumns = true;
            // 
            // treeColumn3
            // 
            resources.ApplyResources(this.treeColumn3, "treeColumn3");
            this.treeColumn3.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // DebugVariableView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.inputVarTree);
            this.HideOnClose = true;
            this.Name = "DebugVariableView";
            this.TabText = "Variablen";
            this.ResumeLayout(false);

        }

        #endregion

        private Mp.Visual.Tree.Tree.TreeColumn Variable;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn1;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn2;
        private Mp.Visual.Tree.Tree.TreeViewAdv inputVarTree;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn3;
    }
}