namespace Mp.Scheme.Deploy
{
    partial class DeployDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeployDlg));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.connectionTab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.clearIPs = new System.Windows.Forms.Button();
            this.ip = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.port = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.comPort = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.export = new System.Windows.Forms.Button();
            this.fileToExport = new System.Windows.Forms.TextBox();
            this.statusConnect = new System.Windows.Forms.Panel();
            this.connect = new System.Windows.Forms.Button();
            this.initStatus = new System.Windows.Forms.Panel();
            this.groupBoxDeplay = new System.Windows.Forms.GroupBox();
            this.loadedScheme = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.statusDeploy = new System.Windows.Forms.Panel();
            this.deploy = new System.Windows.Forms.Button();
            this.schemeName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxRuntime = new System.Windows.Forms.GroupBox();
            this.runStatus = new System.Windows.Forms.Panel();
            this.stop = new System.Windows.Forms.Button();
            this.start = new System.Windows.Forms.Button();
            this.deinitialize = new System.Windows.Forms.Button();
            this.initialize = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.help = new System.Windows.Forms.Button();
            this.close = new System.Windows.Forms.Button();
            this.groupBoxOutput = new System.Windows.Forms.GroupBox();
            this.messages = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.readLogFile = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.properties = new System.Windows.Forms.DataGridView();
            this.nameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.detailButtonCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.mandatoryCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.groupBox1.SuspendLayout();
            this.connectionTab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBoxDeplay.SuspendLayout();
            this.groupBoxRuntime.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBoxOutput.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.connectionTab);
            this.groupBox1.Controls.Add(this.statusConnect);
            this.groupBox1.Controls.Add(this.connect);
            this.errorProvider.SetError(this.groupBox1, resources.GetString("groupBox1.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox1, ((int)(resources.GetObject("groupBox1.IconPadding"))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // connectionTab
            // 
            resources.ApplyResources(this.connectionTab, "connectionTab");
            this.connectionTab.Controls.Add(this.tabPage1);
            this.connectionTab.Controls.Add(this.tabPage2);
            this.connectionTab.Controls.Add(this.tabPage3);
            this.connectionTab.Controls.Add(this.tabPage4);
            this.connectionTab.Controls.Add(this.tabPage5);
            this.errorProvider.SetError(this.connectionTab, resources.GetString("connectionTab.Error"));
            this.errorProvider.SetIconAlignment(this.connectionTab, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("connectionTab.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.connectionTab, ((int)(resources.GetObject("connectionTab.IconPadding"))));
            this.connectionTab.Name = "connectionTab";
            this.connectionTab.SelectedIndex = 0;
            this.connectionTab.SelectedIndexChanged += new System.EventHandler(this.OnConnectionTabSelectedIndexChanged);
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Controls.Add(this.clearIPs);
            this.tabPage1.Controls.Add(this.ip);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.port);
            this.errorProvider.SetError(this.tabPage1, resources.GetString("tabPage1.Error"));
            this.errorProvider.SetIconAlignment(this.tabPage1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tabPage1, ((int)(resources.GetObject("tabPage1.IconPadding"))));
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // clearIPs
            // 
            resources.ApplyResources(this.clearIPs, "clearIPs");
            this.errorProvider.SetError(this.clearIPs, resources.GetString("clearIPs.Error"));
            this.errorProvider.SetIconAlignment(this.clearIPs, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("clearIPs.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.clearIPs, ((int)(resources.GetObject("clearIPs.IconPadding"))));
            this.clearIPs.Name = "clearIPs";
            this.clearIPs.UseVisualStyleBackColor = true;
            this.clearIPs.Click += new System.EventHandler(this.OnClearIPClick);
            // 
            // ip
            // 
            resources.ApplyResources(this.ip, "ip");
            this.errorProvider.SetError(this.ip, resources.GetString("ip.Error"));
            this.ip.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.ip, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("ip.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.ip, ((int)(resources.GetObject("ip.IconPadding"))));
            this.ip.Name = "ip";
            this.ip.Validating += new System.ComponentModel.CancelEventHandler(this.OnIPValidating);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.errorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.errorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            // 
            // port
            // 
            resources.ApplyResources(this.port, "port");
            this.errorProvider.SetError(this.port, resources.GetString("port.Error"));
            this.errorProvider.SetIconAlignment(this.port, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("port.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.port, ((int)(resources.GetObject("port.IconPadding"))));
            this.port.Name = "port";
            this.port.Validating += new System.ComponentModel.CancelEventHandler(this.OnPortValidating);
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.comPort);
            this.errorProvider.SetError(this.tabPage2, resources.GetString("tabPage2.Error"));
            this.errorProvider.SetIconAlignment(this.tabPage2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tabPage2, ((int)(resources.GetObject("tabPage2.IconPadding"))));
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.errorProvider.SetError(this.label5, resources.GetString("label5.Error"));
            this.errorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label5, ((int)(resources.GetObject("label5.IconPadding"))));
            this.label5.Name = "label5";
            // 
            // comPort
            // 
            resources.ApplyResources(this.comPort, "comPort");
            this.errorProvider.SetError(this.comPort, resources.GetString("comPort.Error"));
            this.errorProvider.SetIconAlignment(this.comPort, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("comPort.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.comPort, ((int)(resources.GetObject("comPort.IconPadding"))));
            this.comPort.Name = "comPort";
            // 
            // tabPage3
            // 
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Controls.Add(this.label6);
            this.errorProvider.SetError(this.tabPage3, resources.GetString("tabPage3.Error"));
            this.errorProvider.SetIconAlignment(this.tabPage3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tabPage3, ((int)(resources.GetObject("tabPage3.IconPadding"))));
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.errorProvider.SetError(this.label6, resources.GetString("label6.Error"));
            this.errorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            // 
            // tabPage4
            // 
            resources.ApplyResources(this.tabPage4, "tabPage4");
            this.tabPage4.Controls.Add(this.label7);
            this.errorProvider.SetError(this.tabPage4, resources.GetString("tabPage4.Error"));
            this.errorProvider.SetIconAlignment(this.tabPage4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tabPage4, ((int)(resources.GetObject("tabPage4.IconPadding"))));
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.errorProvider.SetError(this.label7, resources.GetString("label7.Error"));
            this.errorProvider.SetIconAlignment(this.label7, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label7.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label7, ((int)(resources.GetObject("label7.IconPadding"))));
            this.label7.Name = "label7";
            // 
            // tabPage5
            // 
            resources.ApplyResources(this.tabPage5, "tabPage5");
            this.tabPage5.Controls.Add(this.label8);
            this.tabPage5.Controls.Add(this.export);
            this.tabPage5.Controls.Add(this.fileToExport);
            this.errorProvider.SetError(this.tabPage5, resources.GetString("tabPage5.Error"));
            this.errorProvider.SetIconAlignment(this.tabPage5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage5.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tabPage5, ((int)(resources.GetObject("tabPage5.IconPadding"))));
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.errorProvider.SetError(this.label8, resources.GetString("label8.Error"));
            this.errorProvider.SetIconAlignment(this.label8, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label8.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label8, ((int)(resources.GetObject("label8.IconPadding"))));
            this.label8.Name = "label8";
            // 
            // export
            // 
            resources.ApplyResources(this.export, "export");
            this.errorProvider.SetError(this.export, resources.GetString("export.Error"));
            this.errorProvider.SetIconAlignment(this.export, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("export.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.export, ((int)(resources.GetObject("export.IconPadding"))));
            this.export.Name = "export";
            this.export.UseVisualStyleBackColor = true;
            this.export.Click += new System.EventHandler(this.OnExportClick);
            // 
            // fileToExport
            // 
            resources.ApplyResources(this.fileToExport, "fileToExport");
            this.errorProvider.SetError(this.fileToExport, resources.GetString("fileToExport.Error"));
            this.errorProvider.SetIconAlignment(this.fileToExport, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("fileToExport.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.fileToExport, ((int)(resources.GetObject("fileToExport.IconPadding"))));
            this.fileToExport.Name = "fileToExport";
            this.fileToExport.ReadOnly = true;
            // 
            // statusConnect
            // 
            resources.ApplyResources(this.statusConnect, "statusConnect");
            this.statusConnect.BackColor = System.Drawing.Color.DarkSlateGray;
            this.statusConnect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorProvider.SetError(this.statusConnect, resources.GetString("statusConnect.Error"));
            this.errorProvider.SetIconAlignment(this.statusConnect, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("statusConnect.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.statusConnect, ((int)(resources.GetObject("statusConnect.IconPadding"))));
            this.statusConnect.Name = "statusConnect";
            // 
            // connect
            // 
            resources.ApplyResources(this.connect, "connect");
            this.errorProvider.SetError(this.connect, resources.GetString("connect.Error"));
            this.errorProvider.SetIconAlignment(this.connect, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("connect.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.connect, ((int)(resources.GetObject("connect.IconPadding"))));
            this.connect.Name = "connect";
            this.connect.UseVisualStyleBackColor = true;
            this.connect.Click += new System.EventHandler(this.OnConnectClick);
            // 
            // initStatus
            // 
            resources.ApplyResources(this.initStatus, "initStatus");
            this.initStatus.BackColor = System.Drawing.Color.DarkSlateGray;
            this.initStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorProvider.SetError(this.initStatus, resources.GetString("initStatus.Error"));
            this.errorProvider.SetIconAlignment(this.initStatus, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("initStatus.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.initStatus, ((int)(resources.GetObject("initStatus.IconPadding"))));
            this.initStatus.Name = "initStatus";
            // 
            // groupBoxDeplay
            // 
            resources.ApplyResources(this.groupBoxDeplay, "groupBoxDeplay");
            this.groupBoxDeplay.Controls.Add(this.loadedScheme);
            this.groupBoxDeplay.Controls.Add(this.label4);
            this.groupBoxDeplay.Controls.Add(this.statusDeploy);
            this.groupBoxDeplay.Controls.Add(this.deploy);
            this.groupBoxDeplay.Controls.Add(this.schemeName);
            this.groupBoxDeplay.Controls.Add(this.label3);
            this.errorProvider.SetError(this.groupBoxDeplay, resources.GetString("groupBoxDeplay.Error"));
            this.errorProvider.SetIconAlignment(this.groupBoxDeplay, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBoxDeplay.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBoxDeplay, ((int)(resources.GetObject("groupBoxDeplay.IconPadding"))));
            this.groupBoxDeplay.Name = "groupBoxDeplay";
            this.groupBoxDeplay.TabStop = false;
            // 
            // loadedScheme
            // 
            resources.ApplyResources(this.loadedScheme, "loadedScheme");
            this.errorProvider.SetError(this.loadedScheme, resources.GetString("loadedScheme.Error"));
            this.errorProvider.SetIconAlignment(this.loadedScheme, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("loadedScheme.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.loadedScheme, ((int)(resources.GetObject("loadedScheme.IconPadding"))));
            this.loadedScheme.Name = "loadedScheme";
            this.loadedScheme.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.errorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            // 
            // statusDeploy
            // 
            resources.ApplyResources(this.statusDeploy, "statusDeploy");
            this.statusDeploy.BackColor = System.Drawing.Color.DarkSlateGray;
            this.statusDeploy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorProvider.SetError(this.statusDeploy, resources.GetString("statusDeploy.Error"));
            this.errorProvider.SetIconAlignment(this.statusDeploy, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("statusDeploy.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.statusDeploy, ((int)(resources.GetObject("statusDeploy.IconPadding"))));
            this.statusDeploy.Name = "statusDeploy";
            // 
            // deploy
            // 
            resources.ApplyResources(this.deploy, "deploy");
            this.errorProvider.SetError(this.deploy, resources.GetString("deploy.Error"));
            this.errorProvider.SetIconAlignment(this.deploy, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("deploy.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.deploy, ((int)(resources.GetObject("deploy.IconPadding"))));
            this.deploy.Name = "deploy";
            this.deploy.UseVisualStyleBackColor = true;
            this.deploy.Click += new System.EventHandler(this.OnDeployClick);
            // 
            // schemeName
            // 
            resources.ApplyResources(this.schemeName, "schemeName");
            this.errorProvider.SetError(this.schemeName, resources.GetString("schemeName.Error"));
            this.errorProvider.SetIconAlignment(this.schemeName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("schemeName.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.schemeName, ((int)(resources.GetObject("schemeName.IconPadding"))));
            this.schemeName.Name = "schemeName";
            this.schemeName.ReadOnly = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.errorProvider.SetError(this.label3, resources.GetString("label3.Error"));
            this.errorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label3, ((int)(resources.GetObject("label3.IconPadding"))));
            this.label3.Name = "label3";
            // 
            // groupBoxRuntime
            // 
            resources.ApplyResources(this.groupBoxRuntime, "groupBoxRuntime");
            this.groupBoxRuntime.Controls.Add(this.runStatus);
            this.groupBoxRuntime.Controls.Add(this.stop);
            this.groupBoxRuntime.Controls.Add(this.initStatus);
            this.groupBoxRuntime.Controls.Add(this.start);
            this.groupBoxRuntime.Controls.Add(this.deinitialize);
            this.groupBoxRuntime.Controls.Add(this.initialize);
            this.errorProvider.SetError(this.groupBoxRuntime, resources.GetString("groupBoxRuntime.Error"));
            this.errorProvider.SetIconAlignment(this.groupBoxRuntime, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBoxRuntime.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBoxRuntime, ((int)(resources.GetObject("groupBoxRuntime.IconPadding"))));
            this.groupBoxRuntime.Name = "groupBoxRuntime";
            this.groupBoxRuntime.TabStop = false;
            // 
            // runStatus
            // 
            resources.ApplyResources(this.runStatus, "runStatus");
            this.runStatus.BackColor = System.Drawing.Color.DarkSlateGray;
            this.runStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorProvider.SetError(this.runStatus, resources.GetString("runStatus.Error"));
            this.errorProvider.SetIconAlignment(this.runStatus, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("runStatus.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.runStatus, ((int)(resources.GetObject("runStatus.IconPadding"))));
            this.runStatus.Name = "runStatus";
            // 
            // stop
            // 
            resources.ApplyResources(this.stop, "stop");
            this.errorProvider.SetError(this.stop, resources.GetString("stop.Error"));
            this.errorProvider.SetIconAlignment(this.stop, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("stop.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.stop, ((int)(resources.GetObject("stop.IconPadding"))));
            this.stop.Name = "stop";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.OnStopClick);
            // 
            // start
            // 
            resources.ApplyResources(this.start, "start");
            this.errorProvider.SetError(this.start, resources.GetString("start.Error"));
            this.errorProvider.SetIconAlignment(this.start, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("start.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.start, ((int)(resources.GetObject("start.IconPadding"))));
            this.start.Name = "start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.OnStartClick);
            // 
            // deinitialize
            // 
            resources.ApplyResources(this.deinitialize, "deinitialize");
            this.errorProvider.SetError(this.deinitialize, resources.GetString("deinitialize.Error"));
            this.errorProvider.SetIconAlignment(this.deinitialize, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("deinitialize.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.deinitialize, ((int)(resources.GetObject("deinitialize.IconPadding"))));
            this.deinitialize.Name = "deinitialize";
            this.deinitialize.UseVisualStyleBackColor = true;
            this.deinitialize.Click += new System.EventHandler(this.OnDeinitializeClick);
            // 
            // initialize
            // 
            resources.ApplyResources(this.initialize, "initialize");
            this.errorProvider.SetError(this.initialize, resources.GetString("initialize.Error"));
            this.errorProvider.SetIconAlignment(this.initialize, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("initialize.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.initialize, ((int)(resources.GetObject("initialize.IconPadding"))));
            this.initialize.Name = "initialize";
            this.initialize.UseVisualStyleBackColor = true;
            this.initialize.Click += new System.EventHandler(this.OnInitializeClick);
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.help);
            this.flowLayoutPanel1.Controls.Add(this.close);
            this.errorProvider.SetError(this.flowLayoutPanel1, resources.GetString("flowLayoutPanel1.Error"));
            this.errorProvider.SetIconAlignment(this.flowLayoutPanel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("flowLayoutPanel1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.flowLayoutPanel1, ((int)(resources.GetObject("flowLayoutPanel1.IconPadding"))));
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // help
            // 
            resources.ApplyResources(this.help, "help");
            this.errorProvider.SetError(this.help, resources.GetString("help.Error"));
            this.errorProvider.SetIconAlignment(this.help, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("help.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.help, ((int)(resources.GetObject("help.IconPadding"))));
            this.help.Name = "help";
            this.help.UseVisualStyleBackColor = true;
            // 
            // close
            // 
            resources.ApplyResources(this.close, "close");
            this.errorProvider.SetError(this.close, resources.GetString("close.Error"));
            this.errorProvider.SetIconAlignment(this.close, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("close.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.close, ((int)(resources.GetObject("close.IconPadding"))));
            this.close.Name = "close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.OnCloseClick);
            // 
            // groupBoxOutput
            // 
            resources.ApplyResources(this.groupBoxOutput, "groupBoxOutput");
            this.groupBoxOutput.Controls.Add(this.messages);
            this.groupBoxOutput.Controls.Add(this.flowLayoutPanel3);
            this.errorProvider.SetError(this.groupBoxOutput, resources.GetString("groupBoxOutput.Error"));
            this.errorProvider.SetIconAlignment(this.groupBoxOutput, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBoxOutput.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBoxOutput, ((int)(resources.GetObject("groupBoxOutput.IconPadding"))));
            this.groupBoxOutput.Name = "groupBoxOutput";
            this.groupBoxOutput.TabStop = false;
            // 
            // messages
            // 
            resources.ApplyResources(this.messages, "messages");
            this.messages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader4,
            this.columnHeader2,
            this.columnHeader3});
            this.messages.ContextMenuStrip = this.contextMenuStrip;
            this.errorProvider.SetError(this.messages, resources.GetString("messages.Error"));
            this.errorProvider.SetIconAlignment(this.messages, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("messages.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.messages, ((int)(resources.GetObject("messages.IconPadding"))));
            this.messages.Name = "messages";
            this.messages.UseCompatibleStateImageBehavior = false;
            this.messages.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // contextMenuStrip
            // 
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            this.errorProvider.SetError(this.contextMenuStrip, resources.GetString("contextMenuStrip.Error"));
            this.errorProvider.SetIconAlignment(this.contextMenuStrip, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("contextMenuStrip.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.contextMenuStrip, ((int)(resources.GetObject("contextMenuStrip.IconPadding"))));
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            // 
            // clearToolStripMenuItem
            // 
            resources.ApplyResources(this.clearToolStripMenuItem, "clearToolStripMenuItem");
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.OnClearClick);
            // 
            // flowLayoutPanel3
            // 
            resources.ApplyResources(this.flowLayoutPanel3, "flowLayoutPanel3");
            this.flowLayoutPanel3.Controls.Add(this.readLogFile);
            this.errorProvider.SetError(this.flowLayoutPanel3, resources.GetString("flowLayoutPanel3.Error"));
            this.errorProvider.SetIconAlignment(this.flowLayoutPanel3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("flowLayoutPanel3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.flowLayoutPanel3, ((int)(resources.GetObject("flowLayoutPanel3.IconPadding"))));
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            // 
            // readLogFile
            // 
            resources.ApplyResources(this.readLogFile, "readLogFile");
            this.errorProvider.SetError(this.readLogFile, resources.GetString("readLogFile.Error"));
            this.errorProvider.SetIconAlignment(this.readLogFile, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("readLogFile.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.readLogFile, ((int)(resources.GetObject("readLogFile.IconPadding"))));
            this.readLogFile.Name = "readLogFile";
            this.readLogFile.UseVisualStyleBackColor = true;
            this.readLogFile.Click += new System.EventHandler(this.OnReadLogFileClick);
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Controls.Add(this.properties);
            this.errorProvider.SetError(this.groupBox5, resources.GetString("groupBox5.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox5.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox5, ((int)(resources.GetObject("groupBox5.IconPadding"))));
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // properties
            // 
            resources.ApplyResources(this.properties, "properties");
            this.properties.AllowUserToAddRows = false;
            this.properties.AllowUserToDeleteRows = false;
            this.properties.AllowUserToResizeRows = false;
            this.properties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.properties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameCol,
            this.valueCol,
            this.detailButtonCol,
            this.mandatoryCol});
            this.properties.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.errorProvider.SetError(this.properties, resources.GetString("properties.Error"));
            this.errorProvider.SetIconAlignment(this.properties, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("properties.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.properties, ((int)(resources.GetObject("properties.IconPadding"))));
            this.properties.Name = "properties";
            this.properties.RowHeadersVisible = false;
            this.properties.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnPropertiesCellClick);
            // 
            // nameCol
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.nameCol.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.nameCol, "nameCol");
            this.nameCol.Name = "nameCol";
            this.nameCol.ReadOnly = true;
            // 
            // valueCol
            // 
            resources.ApplyResources(this.valueCol, "valueCol");
            this.valueCol.Name = "valueCol";
            // 
            // detailButtonCol
            // 
            resources.ApplyResources(this.detailButtonCol, "detailButtonCol");
            this.detailButtonCol.Name = "detailButtonCol";
            this.detailButtonCol.Text = "...";
            // 
            // mandatoryCol
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            dataGridViewCellStyle2.NullValue = false;
            this.mandatoryCol.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.mandatoryCol, "mandatoryCol");
            this.mandatoryCol.Name = "mandatoryCol";
            this.mandatoryCol.ReadOnly = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // serialPort
            // 
            this.serialPort.PortName = "COM5";
            // 
            // DeployDlg
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxOutput);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.groupBoxRuntime);
            this.Controls.Add(this.groupBoxDeplay);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox1);
            this.Name = "DeployDlg";
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            this.connectionTab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.groupBoxDeplay.ResumeLayout(false);
            this.groupBoxDeplay.PerformLayout();
            this.groupBoxRuntime.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBoxOutput.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel statusConnect;
        private System.Windows.Forms.Button connect;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel initStatus;
        private System.Windows.Forms.GroupBox groupBoxDeplay;
        private System.Windows.Forms.Panel statusDeploy;
        private System.Windows.Forms.Button deploy;
        private System.Windows.Forms.TextBox schemeName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBoxRuntime;
        private System.Windows.Forms.Panel runStatus;
        private System.Windows.Forms.Button stop;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Button deinitialize;
        private System.Windows.Forms.Button initialize;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.GroupBox groupBoxOutput;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView properties;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Button readLogFile;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.TextBox loadedScheme;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView messages;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueCol;
        private System.Windows.Forms.DataGridViewButtonColumn detailButtonCol;
        private System.Windows.Forms.DataGridViewCheckBoxColumn mandatoryCol;
        private System.Windows.Forms.TabControl connectionTab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox comPort;
        private System.IO.Ports.SerialPort serialPort;
        private System.Windows.Forms.ComboBox ip;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button export;
        private System.Windows.Forms.TextBox fileToExport;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button clearIPs;
    }
}