namespace Mpal.Editor
{
    partial class CallStackView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CallStackView));
            this.Variable = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn1 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn2 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn3 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.callView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            // treeColumn3
            // 
            resources.ApplyResources(this.treeColumn3, "treeColumn3");
            this.treeColumn3.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // callView
            // 
            this.callView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            resources.ApplyResources(this.callView, "callView");
            this.callView.Name = "callView";
            this.callView.UseCompatibleStateImageBehavior = false;
            this.callView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // CallStackView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.callView);
            this.HideOnClose = true;
            this.Name = "CallStackView";
            this.TabText = "Aufrufliste";
            this.ResumeLayout(false);

        }

        #endregion

        private Mp.Visual.Tree.Tree.TreeColumn Variable;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn1;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn2;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn3;
        private System.Windows.Forms.ListView callView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}