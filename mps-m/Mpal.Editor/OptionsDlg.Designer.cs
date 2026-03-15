namespace Mpal.Editor
{
    partial class OptionsDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDlg));
            this.OK = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.fullRowLineSel = new System.Windows.Forms.CheckBox();
            this.enableFolding = new System.Windows.Forms.CheckBox();
            this.showEOLMarkers = new System.Windows.Forms.CheckBox();
            this.showTabs = new System.Windows.Forms.CheckBox();
            this.showSpaces = new System.Windows.Forms.CheckBox();
            this.showLineNumbers = new System.Windows.Forms.CheckBox();
            this.showVRuler = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.memSize = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cfvvfv = new System.Windows.Forms.GroupBox();
            this.serverIP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.serverPort = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.useBuildInDebugger = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.supportMathLIB = new System.Windows.Forms.CheckBox();
            this.supportINT64 = new System.Windows.Forms.CheckBox();
            this.supportLREAL = new System.Windows.Forms.CheckBox();
            this.targetMachine = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.expNewLineAfter = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.expBytePostfix = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.expBytePrefix = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.expAsConst = new System.Windows.Forms.CheckBox();
            this.expAsStatic = new System.Windows.Forms.CheckBox();
            this.expVarName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.expIncGuardName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.help = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.cfvvfv.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // OK
            // 
            resources.ApplyResources(this.OK, "OK");
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.errorProvider.SetError(this.OK, resources.GetString("OK.Error"));
            this.errorProvider.SetIconAlignment(this.OK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("OK.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.OK, ((int)(resources.GetObject("OK.IconPadding"))));
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // cancel
            // 
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.errorProvider.SetError(this.cancel, resources.GetString("cancel.Error"));
            this.errorProvider.SetIconAlignment(this.cancel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("cancel.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.cancel, ((int)(resources.GetObject("cancel.IconPadding"))));
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.errorProvider.SetError(this.tabControl1, resources.GetString("tabControl1.Error"));
            this.errorProvider.SetIconAlignment(this.tabControl1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabControl1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tabControl1, ((int)(resources.GetObject("tabControl1.IconPadding"))));
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Controls.Add(this.fullRowLineSel);
            this.tabPage1.Controls.Add(this.enableFolding);
            this.tabPage1.Controls.Add(this.showEOLMarkers);
            this.tabPage1.Controls.Add(this.showTabs);
            this.tabPage1.Controls.Add(this.showSpaces);
            this.tabPage1.Controls.Add(this.showLineNumbers);
            this.tabPage1.Controls.Add(this.showVRuler);
            this.errorProvider.SetError(this.tabPage1, resources.GetString("tabPage1.Error"));
            this.errorProvider.SetIconAlignment(this.tabPage1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tabPage1, ((int)(resources.GetObject("tabPage1.IconPadding"))));
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // fullRowLineSel
            // 
            resources.ApplyResources(this.fullRowLineSel, "fullRowLineSel");
            this.errorProvider.SetError(this.fullRowLineSel, resources.GetString("fullRowLineSel.Error"));
            this.errorProvider.SetIconAlignment(this.fullRowLineSel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("fullRowLineSel.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.fullRowLineSel, ((int)(resources.GetObject("fullRowLineSel.IconPadding"))));
            this.fullRowLineSel.Name = "fullRowLineSel";
            this.fullRowLineSel.UseVisualStyleBackColor = true;
            // 
            // enableFolding
            // 
            resources.ApplyResources(this.enableFolding, "enableFolding");
            this.errorProvider.SetError(this.enableFolding, resources.GetString("enableFolding.Error"));
            this.errorProvider.SetIconAlignment(this.enableFolding, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("enableFolding.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.enableFolding, ((int)(resources.GetObject("enableFolding.IconPadding"))));
            this.enableFolding.Name = "enableFolding";
            this.enableFolding.UseVisualStyleBackColor = true;
            // 
            // showEOLMarkers
            // 
            resources.ApplyResources(this.showEOLMarkers, "showEOLMarkers");
            this.errorProvider.SetError(this.showEOLMarkers, resources.GetString("showEOLMarkers.Error"));
            this.errorProvider.SetIconAlignment(this.showEOLMarkers, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("showEOLMarkers.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.showEOLMarkers, ((int)(resources.GetObject("showEOLMarkers.IconPadding"))));
            this.showEOLMarkers.Name = "showEOLMarkers";
            this.showEOLMarkers.UseVisualStyleBackColor = true;
            // 
            // showTabs
            // 
            resources.ApplyResources(this.showTabs, "showTabs");
            this.errorProvider.SetError(this.showTabs, resources.GetString("showTabs.Error"));
            this.errorProvider.SetIconAlignment(this.showTabs, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("showTabs.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.showTabs, ((int)(resources.GetObject("showTabs.IconPadding"))));
            this.showTabs.Name = "showTabs";
            this.showTabs.UseVisualStyleBackColor = true;
            // 
            // showSpaces
            // 
            resources.ApplyResources(this.showSpaces, "showSpaces");
            this.errorProvider.SetError(this.showSpaces, resources.GetString("showSpaces.Error"));
            this.errorProvider.SetIconAlignment(this.showSpaces, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("showSpaces.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.showSpaces, ((int)(resources.GetObject("showSpaces.IconPadding"))));
            this.showSpaces.Name = "showSpaces";
            this.showSpaces.UseVisualStyleBackColor = true;
            // 
            // showLineNumbers
            // 
            resources.ApplyResources(this.showLineNumbers, "showLineNumbers");
            this.errorProvider.SetError(this.showLineNumbers, resources.GetString("showLineNumbers.Error"));
            this.errorProvider.SetIconAlignment(this.showLineNumbers, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("showLineNumbers.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.showLineNumbers, ((int)(resources.GetObject("showLineNumbers.IconPadding"))));
            this.showLineNumbers.Name = "showLineNumbers";
            this.showLineNumbers.UseVisualStyleBackColor = true;
            // 
            // showVRuler
            // 
            resources.ApplyResources(this.showVRuler, "showVRuler");
            this.errorProvider.SetError(this.showVRuler, resources.GetString("showVRuler.Error"));
            this.errorProvider.SetIconAlignment(this.showVRuler, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("showVRuler.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.showVRuler, ((int)(resources.GetObject("showVRuler.IconPadding"))));
            this.showVRuler.Name = "showVRuler";
            this.showVRuler.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.memSize);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.cfvvfv);
            this.tabPage2.Controls.Add(this.useBuildInDebugger);
            this.errorProvider.SetError(this.tabPage2, resources.GetString("tabPage2.Error"));
            this.errorProvider.SetIconAlignment(this.tabPage2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tabPage2, ((int)(resources.GetObject("tabPage2.IconPadding"))));
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.errorProvider.SetError(this.label10, resources.GetString("label10.Error"));
            this.errorProvider.SetIconAlignment(this.label10, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label10.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label10, ((int)(resources.GetObject("label10.IconPadding"))));
            this.label10.Name = "label10";
            // 
            // memSize
            // 
            resources.ApplyResources(this.memSize, "memSize");
            this.errorProvider.SetError(this.memSize, resources.GetString("memSize.Error"));
            this.errorProvider.SetIconAlignment(this.memSize, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("memSize.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.memSize, ((int)(resources.GetObject("memSize.IconPadding"))));
            this.memSize.Name = "memSize";
            this.memSize.Validating += new System.ComponentModel.CancelEventHandler(this.memSize_Validating);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.errorProvider.SetError(this.label9, resources.GetString("label9.Error"));
            this.errorProvider.SetIconAlignment(this.label9, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label9.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label9, ((int)(resources.GetObject("label9.IconPadding"))));
            this.label9.Name = "label9";
            // 
            // cfvvfv
            // 
            resources.ApplyResources(this.cfvvfv, "cfvvfv");
            this.cfvvfv.Controls.Add(this.serverIP);
            this.cfvvfv.Controls.Add(this.label5);
            this.cfvvfv.Controls.Add(this.serverPort);
            this.cfvvfv.Controls.Add(this.label6);
            this.errorProvider.SetError(this.cfvvfv, resources.GetString("cfvvfv.Error"));
            this.errorProvider.SetIconAlignment(this.cfvvfv, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("cfvvfv.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.cfvvfv, ((int)(resources.GetObject("cfvvfv.IconPadding"))));
            this.cfvvfv.Name = "cfvvfv";
            this.cfvvfv.TabStop = false;
            // 
            // serverIP
            // 
            resources.ApplyResources(this.serverIP, "serverIP");
            this.errorProvider.SetError(this.serverIP, resources.GetString("serverIP.Error"));
            this.errorProvider.SetIconAlignment(this.serverIP, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("serverIP.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.serverIP, ((int)(resources.GetObject("serverIP.IconPadding"))));
            this.serverIP.Name = "serverIP";
            this.serverIP.Validating += new System.ComponentModel.CancelEventHandler(this.serverIP_Validating);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.errorProvider.SetError(this.label5, resources.GetString("label5.Error"));
            this.errorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label5, ((int)(resources.GetObject("label5.IconPadding"))));
            this.label5.Name = "label5";
            // 
            // serverPort
            // 
            resources.ApplyResources(this.serverPort, "serverPort");
            this.errorProvider.SetError(this.serverPort, resources.GetString("serverPort.Error"));
            this.errorProvider.SetIconAlignment(this.serverPort, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("serverPort.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.serverPort, ((int)(resources.GetObject("serverPort.IconPadding"))));
            this.serverPort.Name = "serverPort";
            this.serverPort.Validating += new System.ComponentModel.CancelEventHandler(this.serverPort_Validating);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.errorProvider.SetError(this.label6, resources.GetString("label6.Error"));
            this.errorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            // 
            // useBuildInDebugger
            // 
            resources.ApplyResources(this.useBuildInDebugger, "useBuildInDebugger");
            this.errorProvider.SetError(this.useBuildInDebugger, resources.GetString("useBuildInDebugger.Error"));
            this.errorProvider.SetIconAlignment(this.useBuildInDebugger, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("useBuildInDebugger.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.useBuildInDebugger, ((int)(resources.GetObject("useBuildInDebugger.IconPadding"))));
            this.useBuildInDebugger.Name = "useBuildInDebugger";
            this.useBuildInDebugger.UseVisualStyleBackColor = true;
            this.useBuildInDebugger.CheckedChanged += new System.EventHandler(this.useBuildInDebugger_CheckedChanged);
            // 
            // tabPage3
            // 
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Controls.Add(this.checkBox2);
            this.tabPage3.Controls.Add(this.checkBox1);
            this.tabPage3.Controls.Add(this.supportMathLIB);
            this.tabPage3.Controls.Add(this.supportINT64);
            this.tabPage3.Controls.Add(this.supportLREAL);
            this.tabPage3.Controls.Add(this.targetMachine);
            this.tabPage3.Controls.Add(this.label1);
            this.errorProvider.SetError(this.tabPage3, resources.GetString("tabPage3.Error"));
            this.errorProvider.SetIconAlignment(this.tabPage3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tabPage3, ((int)(resources.GetObject("tabPage3.IconPadding"))));
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            resources.ApplyResources(this.checkBox2, "checkBox2");
            this.errorProvider.SetError(this.checkBox2, resources.GetString("checkBox2.Error"));
            this.errorProvider.SetIconAlignment(this.checkBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.checkBox2, ((int)(resources.GetObject("checkBox2.IconPadding"))));
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.errorProvider.SetError(this.checkBox1, resources.GetString("checkBox1.Error"));
            this.errorProvider.SetIconAlignment(this.checkBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.checkBox1, ((int)(resources.GetObject("checkBox1.IconPadding"))));
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // supportMathLIB
            // 
            resources.ApplyResources(this.supportMathLIB, "supportMathLIB");
            this.errorProvider.SetError(this.supportMathLIB, resources.GetString("supportMathLIB.Error"));
            this.errorProvider.SetIconAlignment(this.supportMathLIB, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("supportMathLIB.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.supportMathLIB, ((int)(resources.GetObject("supportMathLIB.IconPadding"))));
            this.supportMathLIB.Name = "supportMathLIB";
            this.supportMathLIB.UseVisualStyleBackColor = true;
            // 
            // supportINT64
            // 
            resources.ApplyResources(this.supportINT64, "supportINT64");
            this.errorProvider.SetError(this.supportINT64, resources.GetString("supportINT64.Error"));
            this.errorProvider.SetIconAlignment(this.supportINT64, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("supportINT64.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.supportINT64, ((int)(resources.GetObject("supportINT64.IconPadding"))));
            this.supportINT64.Name = "supportINT64";
            this.supportINT64.UseVisualStyleBackColor = true;
            // 
            // supportLREAL
            // 
            resources.ApplyResources(this.supportLREAL, "supportLREAL");
            this.errorProvider.SetError(this.supportLREAL, resources.GetString("supportLREAL.Error"));
            this.errorProvider.SetIconAlignment(this.supportLREAL, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("supportLREAL.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.supportLREAL, ((int)(resources.GetObject("supportLREAL.IconPadding"))));
            this.supportLREAL.Name = "supportLREAL";
            this.supportLREAL.UseVisualStyleBackColor = true;
            // 
            // targetMachine
            // 
            resources.ApplyResources(this.targetMachine, "targetMachine");
            this.targetMachine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider.SetError(this.targetMachine, resources.GetString("targetMachine.Error"));
            this.targetMachine.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.targetMachine, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("targetMachine.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.targetMachine, ((int)(resources.GetObject("targetMachine.IconPadding"))));
            this.targetMachine.Items.AddRange(new object[] {
            resources.GetString("targetMachine.Items"),
            resources.GetString("targetMachine.Items1")});
            this.targetMachine.Name = "targetMachine";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // tabPage4
            // 
            resources.ApplyResources(this.tabPage4, "tabPage4");
            this.tabPage4.Controls.Add(this.groupBox3);
            this.tabPage4.Controls.Add(this.groupBox2);
            this.tabPage4.Controls.Add(this.groupBox1);
            this.errorProvider.SetError(this.tabPage4, resources.GetString("tabPage4.Error"));
            this.errorProvider.SetIconAlignment(this.tabPage4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tabPage4, ((int)(resources.GetObject("tabPage4.IconPadding"))));
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.expNewLineAfter);
            this.groupBox3.Controls.Add(this.label11);
            this.errorProvider.SetError(this.groupBox3, resources.GetString("groupBox3.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox3, ((int)(resources.GetObject("groupBox3.IconPadding"))));
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.errorProvider.SetError(this.label8, resources.GetString("label8.Error"));
            this.errorProvider.SetIconAlignment(this.label8, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label8.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label8, ((int)(resources.GetObject("label8.IconPadding"))));
            this.label8.Name = "label8";
            // 
            // expNewLineAfter
            // 
            resources.ApplyResources(this.expNewLineAfter, "expNewLineAfter");
            this.errorProvider.SetError(this.expNewLineAfter, resources.GetString("expNewLineAfter.Error"));
            this.errorProvider.SetIconAlignment(this.expNewLineAfter, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("expNewLineAfter.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.expNewLineAfter, ((int)(resources.GetObject("expNewLineAfter.IconPadding"))));
            this.expNewLineAfter.Name = "expNewLineAfter";
            this.expNewLineAfter.Validating += new System.ComponentModel.CancelEventHandler(this.expNewLineAfter_Validating);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.errorProvider.SetError(this.label11, resources.GetString("label11.Error"));
            this.errorProvider.SetIconAlignment(this.label11, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label11.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label11, ((int)(resources.GetObject("label11.IconPadding"))));
            this.label11.Name = "label11";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.expBytePostfix);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.expBytePrefix);
            this.errorProvider.SetError(this.groupBox2, resources.GetString("groupBox2.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox2, ((int)(resources.GetObject("groupBox2.IconPadding"))));
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // expBytePostfix
            // 
            resources.ApplyResources(this.expBytePostfix, "expBytePostfix");
            this.errorProvider.SetError(this.expBytePostfix, resources.GetString("expBytePostfix.Error"));
            this.errorProvider.SetIconAlignment(this.expBytePostfix, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("expBytePostfix.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.expBytePostfix, ((int)(resources.GetObject("expBytePostfix.IconPadding"))));
            this.expBytePostfix.Name = "expBytePostfix";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.errorProvider.SetError(this.label7, resources.GetString("label7.Error"));
            this.errorProvider.SetIconAlignment(this.label7, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label7.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label7, ((int)(resources.GetObject("label7.IconPadding"))));
            this.label7.Name = "label7";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.errorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            // 
            // expBytePrefix
            // 
            resources.ApplyResources(this.expBytePrefix, "expBytePrefix");
            this.errorProvider.SetError(this.expBytePrefix, resources.GetString("expBytePrefix.Error"));
            this.errorProvider.SetIconAlignment(this.expBytePrefix, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("expBytePrefix.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.expBytePrefix, ((int)(resources.GetObject("expBytePrefix.IconPadding"))));
            this.expBytePrefix.Name = "expBytePrefix";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.expAsConst);
            this.groupBox1.Controls.Add(this.expAsStatic);
            this.groupBox1.Controls.Add(this.expVarName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.expIncGuardName);
            this.groupBox1.Controls.Add(this.label2);
            this.errorProvider.SetError(this.groupBox1, resources.GetString("groupBox1.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox1, ((int)(resources.GetObject("groupBox1.IconPadding"))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // expAsConst
            // 
            resources.ApplyResources(this.expAsConst, "expAsConst");
            this.expAsConst.Checked = true;
            this.expAsConst.CheckState = System.Windows.Forms.CheckState.Checked;
            this.errorProvider.SetError(this.expAsConst, resources.GetString("expAsConst.Error"));
            this.errorProvider.SetIconAlignment(this.expAsConst, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("expAsConst.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.expAsConst, ((int)(resources.GetObject("expAsConst.IconPadding"))));
            this.expAsConst.Name = "expAsConst";
            this.expAsConst.UseVisualStyleBackColor = true;
            // 
            // expAsStatic
            // 
            resources.ApplyResources(this.expAsStatic, "expAsStatic");
            this.expAsStatic.Checked = true;
            this.expAsStatic.CheckState = System.Windows.Forms.CheckState.Checked;
            this.errorProvider.SetError(this.expAsStatic, resources.GetString("expAsStatic.Error"));
            this.errorProvider.SetIconAlignment(this.expAsStatic, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("expAsStatic.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.expAsStatic, ((int)(resources.GetObject("expAsStatic.IconPadding"))));
            this.expAsStatic.Name = "expAsStatic";
            this.expAsStatic.UseVisualStyleBackColor = true;
            // 
            // expVarName
            // 
            resources.ApplyResources(this.expVarName, "expVarName");
            this.errorProvider.SetError(this.expVarName, resources.GetString("expVarName.Error"));
            this.errorProvider.SetIconAlignment(this.expVarName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("expVarName.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.expVarName, ((int)(resources.GetObject("expVarName.IconPadding"))));
            this.expVarName.Name = "expVarName";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.errorProvider.SetError(this.label3, resources.GetString("label3.Error"));
            this.errorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label3, ((int)(resources.GetObject("label3.IconPadding"))));
            this.label3.Name = "label3";
            // 
            // expIncGuardName
            // 
            resources.ApplyResources(this.expIncGuardName, "expIncGuardName");
            this.errorProvider.SetError(this.expIncGuardName, resources.GetString("expIncGuardName.Error"));
            this.errorProvider.SetIconAlignment(this.expIncGuardName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("expIncGuardName.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.expIncGuardName, ((int)(resources.GetObject("expIncGuardName.IconPadding"))));
            this.expIncGuardName.Name = "expIncGuardName";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.errorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.errorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            // 
            // help
            // 
            resources.ApplyResources(this.help, "help");
            this.errorProvider.SetError(this.help, resources.GetString("help.Error"));
            this.errorProvider.SetIconAlignment(this.help, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("help.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.help, ((int)(resources.GetObject("help.IconPadding"))));
            this.help.Name = "help";
            this.help.UseVisualStyleBackColor = true;
            this.help.Click += new System.EventHandler(this.help_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // OptionsDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.help);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsDlg";
            this.ShowInTaskbar = false;
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.cfvvfv.ResumeLayout(false);
            this.cfvvfv.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox showVRuler;
        private System.Windows.Forms.CheckBox showLineNumbers;
        private System.Windows.Forms.CheckBox showSpaces;
        private System.Windows.Forms.CheckBox showEOLMarkers;
        private System.Windows.Forms.CheckBox showTabs;
        private System.Windows.Forms.CheckBox enableFolding;
        private System.Windows.Forms.CheckBox fullRowLineSel;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox cfvvfv;
        private System.Windows.Forms.TextBox serverIP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox serverPort;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox useBuildInDebugger;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox memSize;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ComboBox targetMachine;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox supportMathLIB;
        private System.Windows.Forms.CheckBox supportINT64;
        private System.Windows.Forms.CheckBox supportLREAL;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox expBytePostfix;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox expBytePrefix;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox expAsConst;
        private System.Windows.Forms.CheckBox expAsStatic;
        private System.Windows.Forms.TextBox expVarName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox expIncGuardName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox expNewLineAfter;
        private System.Windows.Forms.Label label8;
    }
}