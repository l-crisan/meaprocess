namespace Mp.Visual.XYChart
{
    partial class XYChartDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XYChartDlg));
            this.ctrlName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.axisColor = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.points = new System.Windows.Forms.TextBox();
            this.pointColor = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lineColor = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.bkColor = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.xpre = new System.Windows.Forms.TextBox();
            this.xdiv = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.yLog = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.xmax = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.xmin = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.xLog = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ymax = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ymin = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.ypre = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.ydiv = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.gridStyle = new System.Windows.Forms.ComboBox();
            this.gridStylelabel = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.gridYdiv = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.gridXdiv = new System.Windows.Forms.TextBox();
            this.gridColor = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // ctrlName
            // 
            resources.ApplyResources(this.ctrlName, "ctrlName");
            this.errorProvider.SetError(this.ctrlName, resources.GetString("ctrlName.Error"));
            this.errorProvider.SetIconAlignment(this.ctrlName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("ctrlName.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.ctrlName, ((int)(resources.GetObject("ctrlName.IconPadding"))));
            this.ctrlName.Name = "ctrlName";
            this.ctrlName.ReadOnly = true;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.errorProvider.SetError(this.label11, resources.GetString("label11.Error"));
            this.errorProvider.SetIconAlignment(this.label11, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label11.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label11, ((int)(resources.GetObject("label11.IconPadding"))));
            this.label11.Name = "label11";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.axisColor);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.points);
            this.groupBox1.Controls.Add(this.pointColor);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lineColor);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.bkColor);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.ctrlName);
            this.groupBox1.Controls.Add(this.label11);
            this.errorProvider.SetError(this.groupBox1, resources.GetString("groupBox1.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox1, ((int)(resources.GetObject("groupBox1.IconPadding"))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // axisColor
            // 
            resources.ApplyResources(this.axisColor, "axisColor");
            this.errorProvider.SetError(this.axisColor, resources.GetString("axisColor.Error"));
            this.errorProvider.SetIconAlignment(this.axisColor, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("axisColor.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.axisColor, ((int)(resources.GetObject("axisColor.IconPadding"))));
            this.axisColor.Name = "axisColor";
            this.axisColor.UseVisualStyleBackColor = true;
            this.axisColor.Click += new System.EventHandler(this.axisColor_Click);
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.errorProvider.SetError(this.label17, resources.GetString("label17.Error"));
            this.errorProvider.SetIconAlignment(this.label17, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label17.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label17, ((int)(resources.GetObject("label17.IconPadding"))));
            this.label17.Name = "label17";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.errorProvider.SetError(this.label16, resources.GetString("label16.Error"));
            this.errorProvider.SetIconAlignment(this.label16, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label16.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label16, ((int)(resources.GetObject("label16.IconPadding"))));
            this.label16.Name = "label16";
            // 
            // points
            // 
            resources.ApplyResources(this.points, "points");
            this.errorProvider.SetError(this.points, resources.GetString("points.Error"));
            this.errorProvider.SetIconAlignment(this.points, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("points.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.points, ((int)(resources.GetObject("points.IconPadding"))));
            this.points.Name = "points";
            this.points.Validating += new System.ComponentModel.CancelEventHandler(this.points_Validating);
            // 
            // pointColor
            // 
            resources.ApplyResources(this.pointColor, "pointColor");
            this.errorProvider.SetError(this.pointColor, resources.GetString("pointColor.Error"));
            this.errorProvider.SetIconAlignment(this.pointColor, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pointColor.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.pointColor, ((int)(resources.GetObject("pointColor.IconPadding"))));
            this.pointColor.Name = "pointColor";
            this.pointColor.UseVisualStyleBackColor = true;
            this.pointColor.Click += new System.EventHandler(this.pointColor_Click);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.errorProvider.SetError(this.label6, resources.GetString("label6.Error"));
            this.errorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            // 
            // lineColor
            // 
            resources.ApplyResources(this.lineColor, "lineColor");
            this.errorProvider.SetError(this.lineColor, resources.GetString("lineColor.Error"));
            this.errorProvider.SetIconAlignment(this.lineColor, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lineColor.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.lineColor, ((int)(resources.GetObject("lineColor.IconPadding"))));
            this.lineColor.Name = "lineColor";
            this.lineColor.UseVisualStyleBackColor = true;
            this.lineColor.Click += new System.EventHandler(this.lineColor_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.errorProvider.SetError(this.label5, resources.GetString("label5.Error"));
            this.errorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label5, ((int)(resources.GetObject("label5.IconPadding"))));
            this.label5.Name = "label5";
            // 
            // bkColor
            // 
            resources.ApplyResources(this.bkColor, "bkColor");
            this.errorProvider.SetError(this.bkColor, resources.GetString("bkColor.Error"));
            this.errorProvider.SetIconAlignment(this.bkColor, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("bkColor.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.bkColor, ((int)(resources.GetObject("bkColor.IconPadding"))));
            this.bkColor.Name = "bkColor";
            this.bkColor.UseVisualStyleBackColor = true;
            this.bkColor.Click += new System.EventHandler(this.bkColor_Click);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.errorProvider.SetError(this.label7, resources.GetString("label7.Error"));
            this.errorProvider.SetIconAlignment(this.label7, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label7.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label7, ((int)(resources.GetObject("label7.IconPadding"))));
            this.label7.Name = "label7";
            // 
            // xpre
            // 
            resources.ApplyResources(this.xpre, "xpre");
            this.errorProvider.SetError(this.xpre, resources.GetString("xpre.Error"));
            this.errorProvider.SetIconAlignment(this.xpre, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("xpre.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.xpre, ((int)(resources.GetObject("xpre.IconPadding"))));
            this.xpre.Name = "xpre";
            this.xpre.Validating += new System.ComponentModel.CancelEventHandler(this.xpre_Validating);
            // 
            // xdiv
            // 
            resources.ApplyResources(this.xdiv, "xdiv");
            this.errorProvider.SetError(this.xdiv, resources.GetString("xdiv.Error"));
            this.errorProvider.SetIconAlignment(this.xdiv, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("xdiv.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.xdiv, ((int)(resources.GetObject("xdiv.IconPadding"))));
            this.xdiv.Name = "xdiv";
            this.xdiv.Validating += new System.ComponentModel.CancelEventHandler(this.xdiv_Validating);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.errorProvider.SetError(this.label3, resources.GetString("label3.Error"));
            this.errorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label3, ((int)(resources.GetObject("label3.IconPadding"))));
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.errorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.errorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.xLog);
            this.groupBox2.Controls.Add(this.xmax);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.xmin);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.xpre);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.xdiv);
            this.errorProvider.SetError(this.groupBox2, resources.GetString("groupBox2.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox2, ((int)(resources.GetObject("groupBox2.IconPadding"))));
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // yLog
            // 
            resources.ApplyResources(this.yLog, "yLog");
            this.errorProvider.SetError(this.yLog, resources.GetString("yLog.Error"));
            this.errorProvider.SetIconAlignment(this.yLog, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("yLog.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.yLog, ((int)(resources.GetObject("yLog.IconPadding"))));
            this.yLog.Name = "yLog";
            this.yLog.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.errorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            // 
            // xmax
            // 
            resources.ApplyResources(this.xmax, "xmax");
            this.errorProvider.SetError(this.xmax, resources.GetString("xmax.Error"));
            this.errorProvider.SetIconAlignment(this.xmax, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("xmax.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.xmax, ((int)(resources.GetObject("xmax.IconPadding"))));
            this.xmax.Name = "xmax";
            this.xmax.Validating += new System.ComponentModel.CancelEventHandler(this.xmax_Validating);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // xmin
            // 
            resources.ApplyResources(this.xmin, "xmin");
            this.errorProvider.SetError(this.xmin, resources.GetString("xmin.Error"));
            this.errorProvider.SetIconAlignment(this.xmin, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("xmin.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.xmin, ((int)(resources.GetObject("xmin.IconPadding"))));
            this.xmin.Name = "xmin";
            this.xmin.Validating += new System.ComponentModel.CancelEventHandler(this.xmin_Validating);
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.yLog);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.ymax);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.ymin);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.ypre);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.ydiv);
            this.errorProvider.SetError(this.groupBox3, resources.GetString("groupBox3.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox3, ((int)(resources.GetObject("groupBox3.IconPadding"))));
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // xLog
            // 
            resources.ApplyResources(this.xLog, "xLog");
            this.errorProvider.SetError(this.xLog, resources.GetString("xLog.Error"));
            this.errorProvider.SetIconAlignment(this.xLog, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("xLog.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.xLog, ((int)(resources.GetObject("xLog.IconPadding"))));
            this.xLog.Name = "xLog";
            this.xLog.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.errorProvider.SetError(this.label8, resources.GetString("label8.Error"));
            this.errorProvider.SetIconAlignment(this.label8, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label8.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label8, ((int)(resources.GetObject("label8.IconPadding"))));
            this.label8.Name = "label8";
            // 
            // ymax
            // 
            resources.ApplyResources(this.ymax, "ymax");
            this.errorProvider.SetError(this.ymax, resources.GetString("ymax.Error"));
            this.errorProvider.SetIconAlignment(this.ymax, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("ymax.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.ymax, ((int)(resources.GetObject("ymax.IconPadding"))));
            this.ymax.Name = "ymax";
            this.ymax.Validating += new System.ComponentModel.CancelEventHandler(this.ymax_Validating);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.errorProvider.SetError(this.label9, resources.GetString("label9.Error"));
            this.errorProvider.SetIconAlignment(this.label9, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label9.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label9, ((int)(resources.GetObject("label9.IconPadding"))));
            this.label9.Name = "label9";
            // 
            // ymin
            // 
            resources.ApplyResources(this.ymin, "ymin");
            this.errorProvider.SetError(this.ymin, resources.GetString("ymin.Error"));
            this.errorProvider.SetIconAlignment(this.ymin, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("ymin.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.ymin, ((int)(resources.GetObject("ymin.IconPadding"))));
            this.ymin.Name = "ymin";
            this.ymin.Validating += new System.ComponentModel.CancelEventHandler(this.ymin_Validating);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.errorProvider.SetError(this.label10, resources.GetString("label10.Error"));
            this.errorProvider.SetIconAlignment(this.label10, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label10.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label10, ((int)(resources.GetObject("label10.IconPadding"))));
            this.label10.Name = "label10";
            // 
            // ypre
            // 
            resources.ApplyResources(this.ypre, "ypre");
            this.errorProvider.SetError(this.ypre, resources.GetString("ypre.Error"));
            this.errorProvider.SetIconAlignment(this.ypre, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("ypre.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.ypre, ((int)(resources.GetObject("ypre.IconPadding"))));
            this.ypre.Name = "ypre";
            this.ypre.Validating += new System.ComponentModel.CancelEventHandler(this.ypre_Validating);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.errorProvider.SetError(this.label12, resources.GetString("label12.Error"));
            this.errorProvider.SetIconAlignment(this.label12, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label12.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label12, ((int)(resources.GetObject("label12.IconPadding"))));
            this.label12.Name = "label12";
            // 
            // ydiv
            // 
            resources.ApplyResources(this.ydiv, "ydiv");
            this.errorProvider.SetError(this.ydiv, resources.GetString("ydiv.Error"));
            this.errorProvider.SetIconAlignment(this.ydiv, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("ydiv.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.ydiv, ((int)(resources.GetObject("ydiv.IconPadding"))));
            this.ydiv.Name = "ydiv";
            this.ydiv.Validating += new System.ComponentModel.CancelEventHandler(this.ydiv_Validating);
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.gridStyle);
            this.groupBox4.Controls.Add(this.gridStylelabel);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.gridYdiv);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.gridXdiv);
            this.groupBox4.Controls.Add(this.gridColor);
            this.groupBox4.Controls.Add(this.label13);
            this.errorProvider.SetError(this.groupBox4, resources.GetString("groupBox4.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox4, ((int)(resources.GetObject("groupBox4.IconPadding"))));
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // gridStyle
            // 
            resources.ApplyResources(this.gridStyle, "gridStyle");
            this.gridStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider.SetError(this.gridStyle, resources.GetString("gridStyle.Error"));
            this.gridStyle.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.gridStyle, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("gridStyle.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.gridStyle, ((int)(resources.GetObject("gridStyle.IconPadding"))));
            this.gridStyle.Items.AddRange(new object[] {
            resources.GetString("gridStyle.Items"),
            resources.GetString("gridStyle.Items1"),
            resources.GetString("gridStyle.Items2"),
            resources.GetString("gridStyle.Items3"),
            resources.GetString("gridStyle.Items4")});
            this.gridStyle.Name = "gridStyle";
            // 
            // gridStylelabel
            // 
            resources.ApplyResources(this.gridStylelabel, "gridStylelabel");
            this.errorProvider.SetError(this.gridStylelabel, resources.GetString("gridStylelabel.Error"));
            this.errorProvider.SetIconAlignment(this.gridStylelabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("gridStylelabel.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.gridStylelabel, ((int)(resources.GetObject("gridStylelabel.IconPadding"))));
            this.gridStylelabel.Name = "gridStylelabel";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.errorProvider.SetError(this.label15, resources.GetString("label15.Error"));
            this.errorProvider.SetIconAlignment(this.label15, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label15.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label15, ((int)(resources.GetObject("label15.IconPadding"))));
            this.label15.Name = "label15";
            // 
            // gridYdiv
            // 
            resources.ApplyResources(this.gridYdiv, "gridYdiv");
            this.errorProvider.SetError(this.gridYdiv, resources.GetString("gridYdiv.Error"));
            this.errorProvider.SetIconAlignment(this.gridYdiv, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("gridYdiv.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.gridYdiv, ((int)(resources.GetObject("gridYdiv.IconPadding"))));
            this.gridYdiv.Name = "gridYdiv";
            this.gridYdiv.Validating += new System.ComponentModel.CancelEventHandler(this.gridYdiv_Validating);
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.errorProvider.SetError(this.label14, resources.GetString("label14.Error"));
            this.errorProvider.SetIconAlignment(this.label14, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label14.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label14, ((int)(resources.GetObject("label14.IconPadding"))));
            this.label14.Name = "label14";
            // 
            // gridXdiv
            // 
            resources.ApplyResources(this.gridXdiv, "gridXdiv");
            this.errorProvider.SetError(this.gridXdiv, resources.GetString("gridXdiv.Error"));
            this.errorProvider.SetIconAlignment(this.gridXdiv, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("gridXdiv.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.gridXdiv, ((int)(resources.GetObject("gridXdiv.IconPadding"))));
            this.gridXdiv.Name = "gridXdiv";
            this.gridXdiv.Validating += new System.ComponentModel.CancelEventHandler(this.gridXdiv_Validating);
            // 
            // gridColor
            // 
            resources.ApplyResources(this.gridColor, "gridColor");
            this.errorProvider.SetError(this.gridColor, resources.GetString("gridColor.Error"));
            this.errorProvider.SetIconAlignment(this.gridColor, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("gridColor.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.gridColor, ((int)(resources.GetObject("gridColor.IconPadding"))));
            this.gridColor.Name = "gridColor";
            this.gridColor.UseVisualStyleBackColor = true;
            this.gridColor.Click += new System.EventHandler(this.gridColor_Click);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.errorProvider.SetError(this.label13, resources.GetString("label13.Error"));
            this.errorProvider.SetIconAlignment(this.label13, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label13.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label13, ((int)(resources.GetObject("label13.IconPadding"))));
            this.label13.Name = "label13";
            // 
            // OK
            // 
            resources.ApplyResources(this.OK, "OK");
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
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // XYChartDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "XYChartDlg";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.XYChartDlg_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox ctrlName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox xpre;
        private System.Windows.Forms.TextBox xdiv;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox xmax;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox xmin;
        private System.Windows.Forms.Button bkColor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button pointColor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button lineColor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox ymax;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox ymin;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox ypre;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox ydiv;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox gridYdiv;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox gridXdiv;
        private System.Windows.Forms.Button gridColor;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label gridStylelabel;
        private System.Windows.Forms.ComboBox gridStyle;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox points;
        private System.Windows.Forms.Button axisColor;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox yLog;
        private System.Windows.Forms.CheckBox xLog;
    }
}