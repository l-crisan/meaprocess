namespace Mp.Mod.CAN
{
    partial class CANSignalEditorDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CANSignalEditorDlg));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.msgTree = new System.Windows.Forms.TreeView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSignalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.removeMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSignalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oKToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.signalGroup = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.dataType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.commentCtrl = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.unitCtrl = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.maxCtrl = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.minCtrl = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.offsetCtrl = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.factorCtrl = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.bitCountCtrl = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.pivotBitCtrl = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.modeValueCtrl = new System.Windows.Forms.TextBox();
            this.sigTypeCtrl = new System.Windows.Forms.ComboBox();
            this.signalCtrl = new System.Windows.Forms.TextBox();
            this.byteOrderCtrl = new System.Windows.Forms.ComboBox();
            this.msgGroup = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.idCtrl = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.byteCountCtrl = new System.Windows.Forms.TextBox();
            this.msgCtrl = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.help = new System.Windows.Forms.Button();
            this.onExport = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.signalGroup.SuspendLayout();
            this.msgGroup.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
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
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.msgTree);
            this.groupBox1.Controls.Add(this.panel1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // msgTree
            // 
            this.msgTree.ContextMenuStrip = this.contextMenuStrip;
            resources.ApplyResources(this.msgTree, "msgTree");
            this.msgTree.HideSelection = false;
            this.msgTree.Name = "msgTree";
            this.msgTree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.msgTree_BeforeSelect);
            this.msgTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.msgTree_AfterSelect);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMessageToolStripMenuItem,
            this.newSignalToolStripMenuItem,
            this.toolStripMenuItem1,
            this.removeMessageToolStripMenuItem,
            this.removeSignalToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exportToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // newMessageToolStripMenuItem
            // 
            resources.ApplyResources(this.newMessageToolStripMenuItem, "newMessageToolStripMenuItem");
            this.newMessageToolStripMenuItem.Name = "newMessageToolStripMenuItem";
            this.newMessageToolStripMenuItem.Click += new System.EventHandler(this.newMessageToolStripMenuItem_Click);
            // 
            // newSignalToolStripMenuItem
            // 
            resources.ApplyResources(this.newSignalToolStripMenuItem, "newSignalToolStripMenuItem");
            this.newSignalToolStripMenuItem.Name = "newSignalToolStripMenuItem";
            this.newSignalToolStripMenuItem.Click += new System.EventHandler(this.newSignalToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // removeMessageToolStripMenuItem
            // 
            this.removeMessageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oKToolStripMenuItem});
            this.removeMessageToolStripMenuItem.Name = "removeMessageToolStripMenuItem";
            resources.ApplyResources(this.removeMessageToolStripMenuItem, "removeMessageToolStripMenuItem");
            // 
            // oKToolStripMenuItem
            // 
            this.oKToolStripMenuItem.Name = "oKToolStripMenuItem";
            resources.ApplyResources(this.oKToolStripMenuItem, "oKToolStripMenuItem");
            this.oKToolStripMenuItem.Click += new System.EventHandler(this.removeMessageToolStripMenuItem_Click);
            // 
            // removeSignalToolStripMenuItem
            // 
            this.removeSignalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oKToolStripMenuItem1});
            this.removeSignalToolStripMenuItem.Name = "removeSignalToolStripMenuItem";
            resources.ApplyResources(this.removeSignalToolStripMenuItem, "removeSignalToolStripMenuItem");
            // 
            // oKToolStripMenuItem1
            // 
            this.oKToolStripMenuItem1.Name = "oKToolStripMenuItem1";
            resources.ApplyResources(this.oKToolStripMenuItem1, "oKToolStripMenuItem1");
            this.oKToolStripMenuItem1.Click += new System.EventHandler(this.removeSignalToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            // 
            // exportToolStripMenuItem
            // 
            resources.ApplyResources(this.exportToolStripMenuItem, "exportToolStripMenuItem");
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.signalGroup);
            this.groupBox2.Controls.Add(this.msgGroup);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // signalGroup
            // 
            this.signalGroup.Controls.Add(this.label16);
            this.signalGroup.Controls.Add(this.dataType);
            this.signalGroup.Controls.Add(this.label4);
            this.signalGroup.Controls.Add(this.label15);
            this.signalGroup.Controls.Add(this.commentCtrl);
            this.signalGroup.Controls.Add(this.label14);
            this.signalGroup.Controls.Add(this.label5);
            this.signalGroup.Controls.Add(this.unitCtrl);
            this.signalGroup.Controls.Add(this.label13);
            this.signalGroup.Controls.Add(this.maxCtrl);
            this.signalGroup.Controls.Add(this.label6);
            this.signalGroup.Controls.Add(this.minCtrl);
            this.signalGroup.Controls.Add(this.label12);
            this.signalGroup.Controls.Add(this.offsetCtrl);
            this.signalGroup.Controls.Add(this.label7);
            this.signalGroup.Controls.Add(this.factorCtrl);
            this.signalGroup.Controls.Add(this.label11);
            this.signalGroup.Controls.Add(this.label8);
            this.signalGroup.Controls.Add(this.bitCountCtrl);
            this.signalGroup.Controls.Add(this.label10);
            this.signalGroup.Controls.Add(this.pivotBitCtrl);
            this.signalGroup.Controls.Add(this.label9);
            this.signalGroup.Controls.Add(this.modeValueCtrl);
            this.signalGroup.Controls.Add(this.sigTypeCtrl);
            this.signalGroup.Controls.Add(this.signalCtrl);
            this.signalGroup.Controls.Add(this.byteOrderCtrl);
            resources.ApplyResources(this.signalGroup, "signalGroup");
            this.signalGroup.Name = "signalGroup";
            this.signalGroup.TabStop = false;
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // dataType
            // 
            this.dataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataType.FormattingEnabled = true;
            this.dataType.Items.AddRange(new object[] {
            resources.GetString("dataType.Items"),
            resources.GetString("dataType.Items1"),
            resources.GetString("dataType.Items2"),
            resources.GetString("dataType.Items3")});
            resources.ApplyResources(this.dataType, "dataType");
            this.dataType.Name = "dataType";
            this.dataType.SelectedIndexChanged += new System.EventHandler(this.dataType_SelectedIndexChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // commentCtrl
            // 
            resources.ApplyResources(this.commentCtrl, "commentCtrl");
            this.commentCtrl.Name = "commentCtrl";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // unitCtrl
            // 
            resources.ApplyResources(this.unitCtrl, "unitCtrl");
            this.unitCtrl.Name = "unitCtrl";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // maxCtrl
            // 
            resources.ApplyResources(this.maxCtrl, "maxCtrl");
            this.maxCtrl.Name = "maxCtrl";
            this.maxCtrl.Validating += new System.ComponentModel.CancelEventHandler(this.maxCtrl_Validating);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // minCtrl
            // 
            resources.ApplyResources(this.minCtrl, "minCtrl");
            this.minCtrl.Name = "minCtrl";
            this.minCtrl.Validating += new System.ComponentModel.CancelEventHandler(this.minCtrl_Validating);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // offsetCtrl
            // 
            resources.ApplyResources(this.offsetCtrl, "offsetCtrl");
            this.offsetCtrl.Name = "offsetCtrl";
            this.offsetCtrl.Validating += new System.ComponentModel.CancelEventHandler(this.offsetCtrl_Validating);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // factorCtrl
            // 
            resources.ApplyResources(this.factorCtrl, "factorCtrl");
            this.factorCtrl.Name = "factorCtrl";
            this.factorCtrl.Validating += new System.ComponentModel.CancelEventHandler(this.factorCtrl_Validating);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // bitCountCtrl
            // 
            resources.ApplyResources(this.bitCountCtrl, "bitCountCtrl");
            this.bitCountCtrl.Name = "bitCountCtrl";
            this.bitCountCtrl.Validating += new System.ComponentModel.CancelEventHandler(this.bitCountCtrl_Validating);
            this.bitCountCtrl.Validated += new System.EventHandler(this.bitCountCtrl_Validated);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // pivotBitCtrl
            // 
            resources.ApplyResources(this.pivotBitCtrl, "pivotBitCtrl");
            this.pivotBitCtrl.Name = "pivotBitCtrl";
            this.pivotBitCtrl.Validating += new System.ComponentModel.CancelEventHandler(this.pivotBitCtrl_Validating);
            this.pivotBitCtrl.Validated += new System.EventHandler(this.pivotBitCtrl_Validated);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // modeValueCtrl
            // 
            resources.ApplyResources(this.modeValueCtrl, "modeValueCtrl");
            this.modeValueCtrl.Name = "modeValueCtrl";
            this.modeValueCtrl.Validating += new System.ComponentModel.CancelEventHandler(this.modeValueCtrl_Validating);
            // 
            // sigTypeCtrl
            // 
            this.sigTypeCtrl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sigTypeCtrl.FormattingEnabled = true;
            this.sigTypeCtrl.Items.AddRange(new object[] {
            resources.GetString("sigTypeCtrl.Items"),
            resources.GetString("sigTypeCtrl.Items1"),
            resources.GetString("sigTypeCtrl.Items2")});
            resources.ApplyResources(this.sigTypeCtrl, "sigTypeCtrl");
            this.sigTypeCtrl.Name = "sigTypeCtrl";
            // 
            // signalCtrl
            // 
            resources.ApplyResources(this.signalCtrl, "signalCtrl");
            this.signalCtrl.Name = "signalCtrl";
            this.signalCtrl.Validated += new System.EventHandler(this.signalCtrl_Validated);
            // 
            // byteOrderCtrl
            // 
            this.byteOrderCtrl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.byteOrderCtrl.FormattingEnabled = true;
            this.byteOrderCtrl.Items.AddRange(new object[] {
            resources.GetString("byteOrderCtrl.Items"),
            resources.GetString("byteOrderCtrl.Items1")});
            resources.ApplyResources(this.byteOrderCtrl, "byteOrderCtrl");
            this.byteOrderCtrl.Name = "byteOrderCtrl";
            this.byteOrderCtrl.SelectedIndexChanged += new System.EventHandler(this.byteOrderCtrl_SelectedIndexChanged);
            // 
            // msgGroup
            // 
            this.msgGroup.Controls.Add(this.label3);
            this.msgGroup.Controls.Add(this.label1);
            this.msgGroup.Controls.Add(this.idCtrl);
            this.msgGroup.Controls.Add(this.label2);
            this.msgGroup.Controls.Add(this.byteCountCtrl);
            this.msgGroup.Controls.Add(this.msgCtrl);
            resources.ApplyResources(this.msgGroup, "msgGroup");
            this.msgGroup.Name = "msgGroup";
            this.msgGroup.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // idCtrl
            // 
            resources.ApplyResources(this.idCtrl, "idCtrl");
            this.idCtrl.Name = "idCtrl";
            this.idCtrl.Validating += new System.ComponentModel.CancelEventHandler(this.idCtrl_Validating);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // byteCountCtrl
            // 
            resources.ApplyResources(this.byteCountCtrl, "byteCountCtrl");
            this.byteCountCtrl.Name = "byteCountCtrl";
            this.byteCountCtrl.Validating += new System.ComponentModel.CancelEventHandler(this.byteCountCtrl_Validating);
            // 
            // msgCtrl
            // 
            resources.ApplyResources(this.msgCtrl, "msgCtrl");
            this.msgCtrl.Name = "msgCtrl";
            this.msgCtrl.Validated += new System.EventHandler(this.msgCtrl_Validated);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.help);
            this.flowLayoutPanel1.Controls.Add(this.onExport);
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
            this.help.Click += new System.EventHandler(this.help_Click);
            // 
            // onExport
            // 
            resources.ApplyResources(this.onExport, "onExport");
            this.onExport.Name = "onExport";
            this.onExport.UseVisualStyleBackColor = true;
            this.onExport.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // OK
            // 
            resources.ApplyResources(this.OK, "OK");
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // CANSignalEditorDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "CANSignalEditorDlg";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CANSignalEditorDlg_FormClosing);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.CANSignalEditorDlg_HelpRequested);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.signalGroup.ResumeLayout(false);
            this.signalGroup.PerformLayout();
            this.msgGroup.ResumeLayout(false);
            this.msgGroup.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView msgTree;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem newMessageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSignalToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem removeMessageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeSignalToolStripMenuItem;
        private System.Windows.Forms.TextBox idCtrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox byteOrderCtrl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox signalCtrl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox msgCtrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox byteCountCtrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox commentCtrl;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox unitCtrl;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox maxCtrl;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox minCtrl;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox offsetCtrl;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox factorCtrl;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox bitCountCtrl;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox pivotBitCtrl;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox modeValueCtrl;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox sigTypeCtrl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox msgGroup;
        private System.Windows.Forms.GroupBox signalGroup;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ToolStripMenuItem oKToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oKToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox dataType;
        private System.Windows.Forms.Button onExport;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}