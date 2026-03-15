namespace Mp.Visual.Oscilloscope
{
    partial class OscilloscopeView
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OscilloscopeView));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.updateXScale = new System.Windows.Forms.Button();
            this.freeze = new System.Windows.Forms.CheckBox();
            this.scaleXUnit = new System.Windows.Forms.ComboBox();
            this.display = new System.Windows.Forms.ComboBox();
            this.auto = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.osciModeCtrl = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.scaleX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.legendCh1 = new System.Windows.Forms.Label();
            this.legendCh2 = new System.Windows.Forms.Label();
            this.meaPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.ch2VoltUnit = new System.Windows.Forms.Label();
            this.ch2Volt = new System.Windows.Forms.Label();
            this.ch2VoltLabel = new System.Windows.Forms.Label();
            this.ch1VoltUnit = new System.Windows.Forms.Label();
            this.ch1Volt = new System.Windows.Forms.Label();
            this.ch1VoltLabel = new System.Windows.Forms.Label();
            this.ch2FreqCtrl = new System.Windows.Forms.Label();
            this.ch2FreqCtrlUnit = new System.Windows.Forms.Label();
            this.ch2FreqLabel = new System.Windows.Forms.Label();
            this.ch1FreqCtrlUnit = new System.Windows.Forms.Label();
            this.ch1FreqCtrl = new System.Windows.Forms.Label();
            this.ch1FreqLabel = new System.Windows.Forms.Label();
            this.panelChControls = new System.Windows.Forms.Panel();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.copyImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.view = new Mp.Visual.Oscilloscope.DoubleBufferedPanel();
            this.groupBox3.SuspendLayout();
            this.meaPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.updateXScale);
            this.groupBox3.Controls.Add(this.freeze);
            this.groupBox3.Controls.Add(this.scaleXUnit);
            this.groupBox3.Controls.Add(this.display);
            this.groupBox3.Controls.Add(this.auto);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.osciModeCtrl);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.scaleX);
            this.groupBox3.Controls.Add(this.label2);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // updateXScale
            // 
            resources.ApplyResources(this.updateXScale, "updateXScale");
            this.updateXScale.Name = "updateXScale";
            this.updateXScale.UseVisualStyleBackColor = true;
            this.updateXScale.Click += new System.EventHandler(this.OnUpdateXScaleClick);
            // 
            // freeze
            // 
            resources.ApplyResources(this.freeze, "freeze");
            this.freeze.Name = "freeze";
            this.freeze.UseVisualStyleBackColor = true;
            this.freeze.CheckedChanged += new System.EventHandler(this.freeze_CheckedChanged);
            // 
            // scaleXUnit
            // 
            this.scaleXUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scaleXUnit.FormattingEnabled = true;
            this.scaleXUnit.Items.AddRange(new object[] {
            resources.GetString("scaleXUnit.Items"),
            resources.GetString("scaleXUnit.Items1"),
            resources.GetString("scaleXUnit.Items2")});
            resources.ApplyResources(this.scaleXUnit, "scaleXUnit");
            this.scaleXUnit.Name = "scaleXUnit";
            this.scaleXUnit.Validated += new System.EventHandler(this.scaleXUnit_Validated);
            // 
            // display
            // 
            this.display.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.display.FormattingEnabled = true;
            this.display.Items.AddRange(new object[] {
            resources.GetString("display.Items"),
            resources.GetString("display.Items1")});
            resources.ApplyResources(this.display, "display");
            this.display.Name = "display";
            this.display.SelectedIndexChanged += new System.EventHandler(this.display_SelectedIndexChanged);
            // 
            // auto
            // 
            resources.ApplyResources(this.auto, "auto");
            this.auto.Name = "auto";
            this.auto.UseVisualStyleBackColor = true;
            this.auto.Click += new System.EventHandler(this.OnAutoClick);
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // osciModeCtrl
            // 
            this.osciModeCtrl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.osciModeCtrl.FormattingEnabled = true;
            this.osciModeCtrl.Items.AddRange(new object[] {
            resources.GetString("osciModeCtrl.Items"),
            resources.GetString("osciModeCtrl.Items1")});
            resources.ApplyResources(this.osciModeCtrl, "osciModeCtrl");
            this.osciModeCtrl.Name = "osciModeCtrl";
            this.osciModeCtrl.SelectedIndexChanged += new System.EventHandler(this.osciMode_SelectedIndexChanged);
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // scaleX
            // 
            resources.ApplyResources(this.scaleX, "scaleX");
            this.scaleX.Name = "scaleX";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // legendCh1
            // 
            resources.ApplyResources(this.legendCh1, "legendCh1");
            this.legendCh1.ForeColor = System.Drawing.Color.White;
            this.legendCh1.Name = "legendCh1";
            // 
            // legendCh2
            // 
            resources.ApplyResources(this.legendCh2, "legendCh2");
            this.legendCh2.ForeColor = System.Drawing.Color.Yellow;
            this.legendCh2.Name = "legendCh2";
            // 
            // meaPanel
            // 
            this.meaPanel.BackColor = System.Drawing.Color.Black;
            this.meaPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.meaPanel.Controls.Add(this.label1);
            this.meaPanel.Controls.Add(this.ch2VoltUnit);
            this.meaPanel.Controls.Add(this.ch2Volt);
            this.meaPanel.Controls.Add(this.ch2VoltLabel);
            this.meaPanel.Controls.Add(this.ch1VoltUnit);
            this.meaPanel.Controls.Add(this.ch1Volt);
            this.meaPanel.Controls.Add(this.ch1VoltLabel);
            this.meaPanel.Controls.Add(this.ch2FreqCtrl);
            this.meaPanel.Controls.Add(this.ch2FreqCtrlUnit);
            this.meaPanel.Controls.Add(this.ch2FreqLabel);
            this.meaPanel.Controls.Add(this.ch1FreqCtrlUnit);
            this.meaPanel.Controls.Add(this.ch1FreqCtrl);
            this.meaPanel.Controls.Add(this.ch1FreqLabel);
            this.meaPanel.Controls.Add(this.legendCh2);
            this.meaPanel.Controls.Add(this.legendCh1);
            resources.ApplyResources(this.meaPanel, "meaPanel");
            this.meaPanel.Name = "meaPanel";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // ch2VoltUnit
            // 
            resources.ApplyResources(this.ch2VoltUnit, "ch2VoltUnit");
            this.ch2VoltUnit.ForeColor = System.Drawing.Color.Yellow;
            this.ch2VoltUnit.Name = "ch2VoltUnit";
            // 
            // ch2Volt
            // 
            resources.ApplyResources(this.ch2Volt, "ch2Volt");
            this.ch2Volt.ForeColor = System.Drawing.Color.Yellow;
            this.ch2Volt.Name = "ch2Volt";
            // 
            // ch2VoltLabel
            // 
            resources.ApplyResources(this.ch2VoltLabel, "ch2VoltLabel");
            this.ch2VoltLabel.ForeColor = System.Drawing.Color.Yellow;
            this.ch2VoltLabel.Name = "ch2VoltLabel";
            // 
            // ch1VoltUnit
            // 
            resources.ApplyResources(this.ch1VoltUnit, "ch1VoltUnit");
            this.ch1VoltUnit.ForeColor = System.Drawing.Color.White;
            this.ch1VoltUnit.Name = "ch1VoltUnit";
            // 
            // ch1Volt
            // 
            resources.ApplyResources(this.ch1Volt, "ch1Volt");
            this.ch1Volt.ForeColor = System.Drawing.Color.White;
            this.ch1Volt.Name = "ch1Volt";
            // 
            // ch1VoltLabel
            // 
            resources.ApplyResources(this.ch1VoltLabel, "ch1VoltLabel");
            this.ch1VoltLabel.ForeColor = System.Drawing.Color.White;
            this.ch1VoltLabel.Name = "ch1VoltLabel";
            // 
            // ch2FreqCtrl
            // 
            resources.ApplyResources(this.ch2FreqCtrl, "ch2FreqCtrl");
            this.ch2FreqCtrl.ForeColor = System.Drawing.Color.Yellow;
            this.ch2FreqCtrl.Name = "ch2FreqCtrl";
            // 
            // ch2FreqCtrlUnit
            // 
            resources.ApplyResources(this.ch2FreqCtrlUnit, "ch2FreqCtrlUnit");
            this.ch2FreqCtrlUnit.ForeColor = System.Drawing.Color.Yellow;
            this.ch2FreqCtrlUnit.Name = "ch2FreqCtrlUnit";
            // 
            // ch2FreqLabel
            // 
            resources.ApplyResources(this.ch2FreqLabel, "ch2FreqLabel");
            this.ch2FreqLabel.ForeColor = System.Drawing.Color.Yellow;
            this.ch2FreqLabel.Name = "ch2FreqLabel";
            // 
            // ch1FreqCtrlUnit
            // 
            resources.ApplyResources(this.ch1FreqCtrlUnit, "ch1FreqCtrlUnit");
            this.ch1FreqCtrlUnit.ForeColor = System.Drawing.Color.White;
            this.ch1FreqCtrlUnit.Name = "ch1FreqCtrlUnit";
            // 
            // ch1FreqCtrl
            // 
            resources.ApplyResources(this.ch1FreqCtrl, "ch1FreqCtrl");
            this.ch1FreqCtrl.ForeColor = System.Drawing.Color.White;
            this.ch1FreqCtrl.Name = "ch1FreqCtrl";
            // 
            // ch1FreqLabel
            // 
            resources.ApplyResources(this.ch1FreqLabel, "ch1FreqLabel");
            this.ch1FreqLabel.ForeColor = System.Drawing.Color.White;
            this.ch1FreqLabel.Name = "ch1FreqLabel";
            // 
            // panelChControls
            // 
            resources.ApplyResources(this.panelChControls, "panelChControls");
            this.panelChControls.Name = "panelChControls";
            this.panelChControls.TabStop = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem,
            this.toolStripMenuItem1,
            this.copyImageToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            // 
            // propertiesToolStripMenuItem
            // 
            resources.ApplyResources(this.propertiesToolStripMenuItem, "propertiesToolStripMenuItem");
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // copyImageToolStripMenuItem
            // 
            this.copyImageToolStripMenuItem.Name = "copyImageToolStripMenuItem";
            resources.ApplyResources(this.copyImageToolStripMenuItem, "copyImageToolStripMenuItem");
            this.copyImageToolStripMenuItem.Click += new System.EventHandler(this.copyImageToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.view);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // view
            // 
            this.view.BackColor = System.Drawing.Color.Black;
            this.view.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.view.ContextMenuStrip = this.contextMenuStrip;
            resources.ApplyResources(this.view, "view");
            this.view.Name = "view";
            this.view.Paint += new System.Windows.Forms.PaintEventHandler(this.OnViewPaint);
            this.view.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.view_MouseDoubleClick);
            this.view.Resize += new System.EventHandler(this.view_Resize);
            // 
            // OscilloscopeView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.meaPanel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.panelChControls);
            this.Name = "OscilloscopeView";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.meaPanel.ResumeLayout(false);
            this.meaPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferedPanel view;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox scaleX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox osciModeCtrl;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button auto;
        private System.Windows.Forms.ComboBox display;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label legendCh1;
        private System.Windows.Forms.Label legendCh2;
        private System.Windows.Forms.Panel meaPanel;
        private System.Windows.Forms.Panel panelChControls;
        private System.Windows.Forms.ComboBox scaleXUnit;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label ch1FreqCtrlUnit;
        private System.Windows.Forms.Label ch1FreqCtrl;
        private System.Windows.Forms.Label ch1FreqLabel;
        private System.Windows.Forms.Label ch2FreqCtrl;
        private System.Windows.Forms.Label ch2FreqCtrlUnit;
        private System.Windows.Forms.Label ch2FreqLabel;
        private System.Windows.Forms.Label ch1VoltUnit;
        private System.Windows.Forms.Label ch1Volt;
        private System.Windows.Forms.Label ch1VoltLabel;
        private System.Windows.Forms.Label ch2VoltUnit;
        private System.Windows.Forms.Label ch2Volt;
        private System.Windows.Forms.Label ch2VoltLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.CheckBox freeze;
        private System.Windows.Forms.ToolStripMenuItem copyImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.Button updateXScale;
        private System.Windows.Forms.Panel panel1;
    }
}
