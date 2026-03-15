namespace Mpal.Debugger
{
    partial class DebuggerSettingsDlg
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebuggerSettingsDlg));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.help = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.inputVarTree = new Mp.Visual.Tree.Tree.TreeViewAdv();
            this.treeColumn1 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn3 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn2 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn4 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn5 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn6 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.memSize = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.useBuildInDebugger = new System.Windows.Forms.CheckBox();
            this.cfvvfv = new System.Windows.Forms.GroupBox();
            this.serverIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.serverPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.cfvvfv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.help);
            this.flowLayoutPanel1.Controls.Add(this.cancel);
            this.flowLayoutPanel1.Controls.Add(this.OK);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // help
            // 
            resources.ApplyResources(this.help, "help");
            this.help.Name = "help";
            this.help.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // OK
            // 
            resources.ApplyResources(this.OK, "OK");
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.inputVarTree);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // inputVarTree
            // 
            this.inputVarTree.BackColor = System.Drawing.SystemColors.Window;
            this.inputVarTree.Columns.Add(this.treeColumn1);
            this.inputVarTree.Columns.Add(this.treeColumn3);
            this.inputVarTree.Columns.Add(this.treeColumn2);
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
            // treeColumn1
            // 
            resources.ApplyResources(this.treeColumn1, "treeColumn1");
            this.treeColumn1.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // treeColumn3
            // 
            resources.ApplyResources(this.treeColumn3, "treeColumn3");
            this.treeColumn3.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // treeColumn2
            // 
            resources.ApplyResources(this.treeColumn2, "treeColumn2");
            this.treeColumn2.SortOrder = System.Windows.Forms.SortOrder.None;
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
            // treeColumn6
            // 
            resources.ApplyResources(this.treeColumn6, "treeColumn6");
            this.treeColumn6.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.memSize);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.useBuildInDebugger);
            this.tabPage2.Controls.Add(this.cfvvfv);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // memSize
            // 
            resources.ApplyResources(this.memSize, "memSize");
            this.memSize.Name = "memSize";
            this.memSize.Validating += new System.ComponentModel.CancelEventHandler(this.memSize_Validating);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // useBuildInDebugger
            // 
            resources.ApplyResources(this.useBuildInDebugger, "useBuildInDebugger");
            this.useBuildInDebugger.Name = "useBuildInDebugger";
            this.useBuildInDebugger.UseVisualStyleBackColor = true;
            this.useBuildInDebugger.CheckedChanged += new System.EventHandler(this.useBuildInDebugger_CheckedChanged);
            // 
            // cfvvfv
            // 
            this.cfvvfv.Controls.Add(this.serverIP);
            this.cfvvfv.Controls.Add(this.label2);
            this.cfvvfv.Controls.Add(this.serverPort);
            this.cfvvfv.Controls.Add(this.label4);
            resources.ApplyResources(this.cfvvfv, "cfvvfv");
            this.cfvvfv.Name = "cfvvfv";
            this.cfvvfv.TabStop = false;
            // 
            // serverIP
            // 
            resources.ApplyResources(this.serverIP, "serverIP");
            this.serverIP.Name = "serverIP";
            this.serverIP.Validating += new System.ComponentModel.CancelEventHandler(this.serverIP_Validating);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // serverPort
            // 
            resources.ApplyResources(this.serverPort, "serverPort");
            this.serverPort.Name = "serverPort";
            this.serverPort.Validating += new System.ComponentModel.CancelEventHandler(this.serverPort_Validating);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // DebuggerSettingsDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "DebuggerSettingsDlg";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DebugerSettingsDlg_FormClosing);
            this.Load += new System.EventHandler(this.DebugerSettingsDlg_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.cfvvfv.ResumeLayout(false);
            this.cfvvfv.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.GroupBox groupBox1;
        private Mp.Visual.Tree.Tree.TreeViewAdv inputVarTree;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn1;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn2;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn3;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn4;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn5;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn6;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.GroupBox cfvvfv;
        private System.Windows.Forms.TextBox serverIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox serverPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox useBuildInDebugger;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox memSize;
        private System.Windows.Forms.Label label9;
    }
}