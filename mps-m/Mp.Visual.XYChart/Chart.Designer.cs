namespace Mp.Visual.XYChart
{
    partial class Chart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Chart));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.scaleToInitialSaluesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.editReferenceCurvesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.onShowPoints = new System.Windows.Forms.CheckBox();
            this.onShowLine = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.onDownPen = new System.Windows.Forms.Button();
            this.onRightPen = new System.Windows.Forms.Button();
            this.onLeftPan = new System.Windows.Forms.Button();
            this.onTopPan = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.onZoomReset = new System.Windows.Forms.Button();
            this.onZoomM = new System.Windows.Forms.Button();
            this.onZoomP = new System.Windows.Forms.Button();
            this.contextMenuStrip.SuspendLayout();
            this.controlPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem,
            this.toolStripMenuItem1,
            this.scaleToInitialSaluesToolStripMenuItem,
            this.showLineToolStripMenuItem,
            this.showPointsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.editReferenceCurvesToolStripMenuItem,
            this.toolStripMenuItem3,
            this.clearToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // propertiesToolStripMenuItem
            // 
            resources.ApplyResources(this.propertiesToolStripMenuItem, "propertiesToolStripMenuItem");
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            // 
            // scaleToInitialSaluesToolStripMenuItem
            // 
            resources.ApplyResources(this.scaleToInitialSaluesToolStripMenuItem, "scaleToInitialSaluesToolStripMenuItem");
            this.scaleToInitialSaluesToolStripMenuItem.Name = "scaleToInitialSaluesToolStripMenuItem";
            this.scaleToInitialSaluesToolStripMenuItem.Click += new System.EventHandler(this.scaleToInitialSaluesToolStripMenuItem_Click);
            // 
            // showLineToolStripMenuItem
            // 
            resources.ApplyResources(this.showLineToolStripMenuItem, "showLineToolStripMenuItem");
            this.showLineToolStripMenuItem.Name = "showLineToolStripMenuItem";
            this.showLineToolStripMenuItem.Click += new System.EventHandler(this.showLineToolStripMenuItem_Click);
            // 
            // showPointsToolStripMenuItem
            // 
            resources.ApplyResources(this.showPointsToolStripMenuItem, "showPointsToolStripMenuItem");
            this.showPointsToolStripMenuItem.Name = "showPointsToolStripMenuItem";
            this.showPointsToolStripMenuItem.Click += new System.EventHandler(this.showPointsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            // 
            // editReferenceCurvesToolStripMenuItem
            // 
            resources.ApplyResources(this.editReferenceCurvesToolStripMenuItem, "editReferenceCurvesToolStripMenuItem");
            this.editReferenceCurvesToolStripMenuItem.Name = "editReferenceCurvesToolStripMenuItem";
            this.editReferenceCurvesToolStripMenuItem.Click += new System.EventHandler(this.editReferenceCurvesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            // 
            // clearToolStripMenuItem
            // 
            resources.ApplyResources(this.clearToolStripMenuItem, "clearToolStripMenuItem");
            this.clearToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oKToolStripMenuItem});
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            // 
            // oKToolStripMenuItem
            // 
            resources.ApplyResources(this.oKToolStripMenuItem, "oKToolStripMenuItem");
            this.oKToolStripMenuItem.Name = "oKToolStripMenuItem";
            this.oKToolStripMenuItem.Click += new System.EventHandler(this.oKToolStripMenuItem_Click);
            // 
            // controlPanel
            // 
            resources.ApplyResources(this.controlPanel, "controlPanel");
            this.controlPanel.BackColor = System.Drawing.SystemColors.Control;
            this.controlPanel.Controls.Add(this.onShowPoints);
            this.controlPanel.Controls.Add(this.onShowLine);
            this.controlPanel.Controls.Add(this.groupBox2);
            this.controlPanel.Controls.Add(this.groupBox1);
            this.controlPanel.Name = "controlPanel";
            // 
            // onShowPoints
            // 
            resources.ApplyResources(this.onShowPoints, "onShowPoints");
            this.onShowPoints.Checked = true;
            this.onShowPoints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.onShowPoints.Name = "onShowPoints";
            this.onShowPoints.UseVisualStyleBackColor = true;
            this.onShowPoints.Click += new System.EventHandler(this.showPointsToolStripMenuItem_Click);
            // 
            // onShowLine
            // 
            resources.ApplyResources(this.onShowLine, "onShowLine");
            this.onShowLine.Checked = true;
            this.onShowLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.onShowLine.Name = "onShowLine";
            this.onShowLine.UseVisualStyleBackColor = true;
            this.onShowLine.Click += new System.EventHandler(this.showLineToolStripMenuItem_Click);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.onDownPen);
            this.groupBox2.Controls.Add(this.onRightPen);
            this.groupBox2.Controls.Add(this.onLeftPan);
            this.groupBox2.Controls.Add(this.onTopPan);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // onDownPen
            // 
            resources.ApplyResources(this.onDownPen, "onDownPen");
            this.onDownPen.Name = "onDownPen";
            this.onDownPen.UseVisualStyleBackColor = true;
            this.onDownPen.Click += new System.EventHandler(this.onDownPen_Click);
            // 
            // onRightPen
            // 
            resources.ApplyResources(this.onRightPen, "onRightPen");
            this.onRightPen.Name = "onRightPen";
            this.onRightPen.UseVisualStyleBackColor = true;
            this.onRightPen.Click += new System.EventHandler(this.onRightPen_Click);
            // 
            // onLeftPan
            // 
            resources.ApplyResources(this.onLeftPan, "onLeftPan");
            this.onLeftPan.Name = "onLeftPan";
            this.onLeftPan.UseVisualStyleBackColor = true;
            this.onLeftPan.Click += new System.EventHandler(this.onLeftPan_Click);
            // 
            // onTopPan
            // 
            resources.ApplyResources(this.onTopPan, "onTopPan");
            this.onTopPan.Name = "onTopPan";
            this.onTopPan.UseVisualStyleBackColor = true;
            this.onTopPan.Click += new System.EventHandler(this.onTopPan_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.onZoomReset);
            this.groupBox1.Controls.Add(this.onZoomM);
            this.groupBox1.Controls.Add(this.onZoomP);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // onZoomReset
            // 
            resources.ApplyResources(this.onZoomReset, "onZoomReset");
            this.onZoomReset.Name = "onZoomReset";
            this.onZoomReset.UseVisualStyleBackColor = true;
            this.onZoomReset.Click += new System.EventHandler(this.onZoomReset_Click);
            // 
            // onZoomM
            // 
            resources.ApplyResources(this.onZoomM, "onZoomM");
            this.onZoomM.Name = "onZoomM";
            this.onZoomM.UseVisualStyleBackColor = true;
            this.onZoomM.Click += new System.EventHandler(this.onZoomM_Click);
            // 
            // onZoomP
            // 
            resources.ApplyResources(this.onZoomP, "onZoomP");
            this.onZoomP.Name = "onZoomP";
            this.onZoomP.UseVisualStyleBackColor = true;
            this.onZoomP.Click += new System.EventHandler(this.onZoomP_Click);
            // 
            // Chart
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ContextMenuStrip = this.contextMenuStrip;
            this.Controls.Add(this.controlPanel);
            this.Name = "Chart";
            this.BackColorChanged += new System.EventHandler(this.Chart_BackColorChanged);
            this.DoubleClick += new System.EventHandler(this.Chart_DoubleClick);
            this.contextMenuStrip.ResumeLayout(false);
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem scaleToInitialSaluesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oKToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showPointsToolStripMenuItem;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button onZoomP;
        private System.Windows.Forms.Button onZoomM;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button onRightPen;
        private System.Windows.Forms.Button onLeftPan;
        private System.Windows.Forms.Button onTopPan;
        private System.Windows.Forms.Button onDownPen;
        private System.Windows.Forms.CheckBox onShowPoints;
        private System.Windows.Forms.CheckBox onShowLine;
        private System.Windows.Forms.Button onZoomReset;
        private System.Windows.Forms.ToolStripMenuItem editReferenceCurvesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
    }
}
