namespace Mp.Visual.Oscilloscope
{
    partial class ChannelControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelControl));
            this.channelPanelCtrl = new System.Windows.Forms.GroupBox();
            this.singleShotBt = new System.Windows.Forms.CheckBox();
            this.horPosGroup = new System.Windows.Forms.GroupBox();
            this.resetHorPosCtrl = new System.Windows.Forms.Button();
            this.horizontalPosCtrl = new Mp.Visual.Analog.Knob();
            this.verPosGroup = new System.Windows.Forms.GroupBox();
            this.resetVerPosCtrl = new System.Windows.Forms.Button();
            this.verticalPosCtrl = new Mp.Visual.Analog.Knob();
            this.invertCtrl = new System.Windows.Forms.CheckBox();
            this.couplingCtrl = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.triggerCtrl = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.visibleCtrl = new System.Windows.Forms.CheckBox();
            this.scaleYCtrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.triggerLevelGroup = new System.Windows.Forms.GroupBox();
            this.resetTriggerCtrl = new System.Windows.Forms.Button();
            this.triggerLevelCtrl = new Mp.Visual.Analog.Knob();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.channelPanelCtrl.SuspendLayout();
            this.horPosGroup.SuspendLayout();
            this.verPosGroup.SuspendLayout();
            this.triggerLevelGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // channelPanelCtrl
            // 
            resources.ApplyResources(this.channelPanelCtrl, "channelPanelCtrl");
            this.channelPanelCtrl.Controls.Add(this.singleShotBt);
            this.channelPanelCtrl.Controls.Add(this.horPosGroup);
            this.channelPanelCtrl.Controls.Add(this.verPosGroup);
            this.channelPanelCtrl.Controls.Add(this.invertCtrl);
            this.channelPanelCtrl.Controls.Add(this.couplingCtrl);
            this.channelPanelCtrl.Controls.Add(this.label9);
            this.channelPanelCtrl.Controls.Add(this.triggerCtrl);
            this.channelPanelCtrl.Controls.Add(this.label4);
            this.channelPanelCtrl.Controls.Add(this.visibleCtrl);
            this.channelPanelCtrl.Controls.Add(this.scaleYCtrl);
            this.channelPanelCtrl.Controls.Add(this.label1);
            this.channelPanelCtrl.Controls.Add(this.triggerLevelGroup);
            this.errorProvider.SetError(this.channelPanelCtrl, resources.GetString("channelPanelCtrl.Error"));
            this.errorProvider.SetIconAlignment(this.channelPanelCtrl, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("channelPanelCtrl.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.channelPanelCtrl, ((int)(resources.GetObject("channelPanelCtrl.IconPadding"))));
            this.channelPanelCtrl.Name = "channelPanelCtrl";
            this.channelPanelCtrl.TabStop = false;
            // 
            // singleShotBt
            // 
            resources.ApplyResources(this.singleShotBt, "singleShotBt");
            this.errorProvider.SetError(this.singleShotBt, resources.GetString("singleShotBt.Error"));
            this.errorProvider.SetIconAlignment(this.singleShotBt, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("singleShotBt.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.singleShotBt, ((int)(resources.GetObject("singleShotBt.IconPadding"))));
            this.singleShotBt.Name = "singleShotBt";
            this.singleShotBt.UseVisualStyleBackColor = true;
            this.singleShotBt.CheckedChanged += new System.EventHandler(this.singleShotBt_CheckedChanged);
            // 
            // horPosGroup
            // 
            resources.ApplyResources(this.horPosGroup, "horPosGroup");
            this.horPosGroup.Controls.Add(this.resetHorPosCtrl);
            this.horPosGroup.Controls.Add(this.horizontalPosCtrl);
            this.errorProvider.SetError(this.horPosGroup, resources.GetString("horPosGroup.Error"));
            this.errorProvider.SetIconAlignment(this.horPosGroup, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("horPosGroup.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.horPosGroup, ((int)(resources.GetObject("horPosGroup.IconPadding"))));
            this.horPosGroup.Name = "horPosGroup";
            this.horPosGroup.TabStop = false;
            // 
            // resetHorPosCtrl
            // 
            resources.ApplyResources(this.resetHorPosCtrl, "resetHorPosCtrl");
            this.errorProvider.SetError(this.resetHorPosCtrl, resources.GetString("resetHorPosCtrl.Error"));
            this.errorProvider.SetIconAlignment(this.resetHorPosCtrl, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("resetHorPosCtrl.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.resetHorPosCtrl, ((int)(resources.GetObject("resetHorPosCtrl.IconPadding"))));
            this.resetHorPosCtrl.Name = "resetHorPosCtrl";
            this.resetHorPosCtrl.UseVisualStyleBackColor = true;
            this.resetHorPosCtrl.Click += new System.EventHandler(this.resetHorPosCtrl_Click);
            // 
            // horizontalPosCtrl
            // 
            resources.ApplyResources(this.horizontalPosCtrl, "horizontalPosCtrl");
            this.horizontalPosCtrl.BackColor = System.Drawing.Color.Transparent;
            this.errorProvider.SetError(this.horizontalPosCtrl, resources.GetString("horizontalPosCtrl.Error"));
            this.errorProvider.SetIconAlignment(this.horizontalPosCtrl, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("horizontalPosCtrl.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.horizontalPosCtrl, ((int)(resources.GetObject("horizontalPosCtrl.IconPadding"))));
            this.horizontalPosCtrl.IndicatorColor = System.Drawing.Color.Black;
            this.horizontalPosCtrl.IndicatorOffset = 10F;
            this.horizontalPosCtrl.KnobColor = System.Drawing.Color.LightGray;
            this.horizontalPosCtrl.MaxValue = 100F;
            this.horizontalPosCtrl.MinValue = -100F;
            this.horizontalPosCtrl.Name = "horizontalPosCtrl";
            this.horizontalPosCtrl.Renderer = null;
            this.horizontalPosCtrl.ScaleColor = System.Drawing.Color.DarkGray;
            this.horizontalPosCtrl.StepValue = 1F;
            this.horizontalPosCtrl.Style = Mp.Visual.Analog.Knob.KnobStyle.Circular;
            this.horizontalPosCtrl.Value = 0F;
            this.horizontalPosCtrl.KnobChangeValue += new Mp.Visual.Analog.KnobChangeValue(this.horizontalPosCtrl_KnobChangeValue);
            // 
            // verPosGroup
            // 
            resources.ApplyResources(this.verPosGroup, "verPosGroup");
            this.verPosGroup.Controls.Add(this.resetVerPosCtrl);
            this.verPosGroup.Controls.Add(this.verticalPosCtrl);
            this.errorProvider.SetError(this.verPosGroup, resources.GetString("verPosGroup.Error"));
            this.errorProvider.SetIconAlignment(this.verPosGroup, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("verPosGroup.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.verPosGroup, ((int)(resources.GetObject("verPosGroup.IconPadding"))));
            this.verPosGroup.Name = "verPosGroup";
            this.verPosGroup.TabStop = false;
            // 
            // resetVerPosCtrl
            // 
            resources.ApplyResources(this.resetVerPosCtrl, "resetVerPosCtrl");
            this.errorProvider.SetError(this.resetVerPosCtrl, resources.GetString("resetVerPosCtrl.Error"));
            this.errorProvider.SetIconAlignment(this.resetVerPosCtrl, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("resetVerPosCtrl.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.resetVerPosCtrl, ((int)(resources.GetObject("resetVerPosCtrl.IconPadding"))));
            this.resetVerPosCtrl.Name = "resetVerPosCtrl";
            this.resetVerPosCtrl.UseVisualStyleBackColor = true;
            this.resetVerPosCtrl.Click += new System.EventHandler(this.resetVerPosCtrl_Click);
            // 
            // verticalPosCtrl
            // 
            resources.ApplyResources(this.verticalPosCtrl, "verticalPosCtrl");
            this.verticalPosCtrl.BackColor = System.Drawing.Color.Transparent;
            this.errorProvider.SetError(this.verticalPosCtrl, resources.GetString("verticalPosCtrl.Error"));
            this.errorProvider.SetIconAlignment(this.verticalPosCtrl, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("verticalPosCtrl.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.verticalPosCtrl, ((int)(resources.GetObject("verticalPosCtrl.IconPadding"))));
            this.verticalPosCtrl.IndicatorColor = System.Drawing.Color.Black;
            this.verticalPosCtrl.IndicatorOffset = 10F;
            this.verticalPosCtrl.KnobColor = System.Drawing.Color.LightGray;
            this.verticalPosCtrl.MaxValue = 100F;
            this.verticalPosCtrl.MinValue = -100F;
            this.verticalPosCtrl.Name = "verticalPosCtrl";
            this.verticalPosCtrl.Renderer = null;
            this.verticalPosCtrl.ScaleColor = System.Drawing.Color.DarkGray;
            this.verticalPosCtrl.StepValue = 1F;
            this.verticalPosCtrl.Style = Mp.Visual.Analog.Knob.KnobStyle.Circular;
            this.verticalPosCtrl.Value = 0F;
            this.verticalPosCtrl.KnobChangeValue += new Mp.Visual.Analog.KnobChangeValue(this.verticalPosCtrl_KnobChangeValue);
            // 
            // invertCtrl
            // 
            resources.ApplyResources(this.invertCtrl, "invertCtrl");
            this.errorProvider.SetError(this.invertCtrl, resources.GetString("invertCtrl.Error"));
            this.errorProvider.SetIconAlignment(this.invertCtrl, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("invertCtrl.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.invertCtrl, ((int)(resources.GetObject("invertCtrl.IconPadding"))));
            this.invertCtrl.Name = "invertCtrl";
            this.invertCtrl.UseVisualStyleBackColor = true;
            this.invertCtrl.CheckedChanged += new System.EventHandler(this.invertCtrl_CheckedChanged);
            // 
            // couplingCtrl
            // 
            resources.ApplyResources(this.couplingCtrl, "couplingCtrl");
            this.couplingCtrl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider.SetError(this.couplingCtrl, resources.GetString("couplingCtrl.Error"));
            this.couplingCtrl.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.couplingCtrl, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("couplingCtrl.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.couplingCtrl, ((int)(resources.GetObject("couplingCtrl.IconPadding"))));
            this.couplingCtrl.Items.AddRange(new object[] {
            resources.GetString("couplingCtrl.Items"),
            resources.GetString("couplingCtrl.Items1"),
            resources.GetString("couplingCtrl.Items2")});
            this.couplingCtrl.Name = "couplingCtrl";
            this.couplingCtrl.SelectionChangeCommitted += new System.EventHandler(this.couplingCtrl_SelectionChangeCommitted);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.errorProvider.SetError(this.label9, resources.GetString("label9.Error"));
            this.errorProvider.SetIconAlignment(this.label9, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label9.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label9, ((int)(resources.GetObject("label9.IconPadding"))));
            this.label9.Name = "label9";
            // 
            // triggerCtrl
            // 
            resources.ApplyResources(this.triggerCtrl, "triggerCtrl");
            this.triggerCtrl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider.SetError(this.triggerCtrl, resources.GetString("triggerCtrl.Error"));
            this.triggerCtrl.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.triggerCtrl, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("triggerCtrl.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.triggerCtrl, ((int)(resources.GetObject("triggerCtrl.IconPadding"))));
            this.triggerCtrl.Items.AddRange(new object[] {
            resources.GetString("triggerCtrl.Items"),
            resources.GetString("triggerCtrl.Items1"),
            resources.GetString("triggerCtrl.Items2"),
            resources.GetString("triggerCtrl.Items3")});
            this.triggerCtrl.Name = "triggerCtrl";
            this.triggerCtrl.SelectedIndexChanged += new System.EventHandler(this.triggerCtrl_SelectedIndexChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.errorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            // 
            // visibleCtrl
            // 
            resources.ApplyResources(this.visibleCtrl, "visibleCtrl");
            this.errorProvider.SetError(this.visibleCtrl, resources.GetString("visibleCtrl.Error"));
            this.errorProvider.SetIconAlignment(this.visibleCtrl, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("visibleCtrl.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.visibleCtrl, ((int)(resources.GetObject("visibleCtrl.IconPadding"))));
            this.visibleCtrl.Name = "visibleCtrl";
            this.visibleCtrl.UseVisualStyleBackColor = true;
            // 
            // scaleYCtrl
            // 
            resources.ApplyResources(this.scaleYCtrl, "scaleYCtrl");
            this.errorProvider.SetError(this.scaleYCtrl, resources.GetString("scaleYCtrl.Error"));
            this.errorProvider.SetIconAlignment(this.scaleYCtrl, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("scaleYCtrl.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.scaleYCtrl, ((int)(resources.GetObject("scaleYCtrl.IconPadding"))));
            this.scaleYCtrl.Name = "scaleYCtrl";
            this.scaleYCtrl.Validating += new System.ComponentModel.CancelEventHandler(this.scaleYCtrl_Validating);
            this.scaleYCtrl.Validated += new System.EventHandler(this.scaleYCtrl_Validated);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // triggerLevelGroup
            // 
            resources.ApplyResources(this.triggerLevelGroup, "triggerLevelGroup");
            this.triggerLevelGroup.Controls.Add(this.resetTriggerCtrl);
            this.triggerLevelGroup.Controls.Add(this.triggerLevelCtrl);
            this.errorProvider.SetError(this.triggerLevelGroup, resources.GetString("triggerLevelGroup.Error"));
            this.errorProvider.SetIconAlignment(this.triggerLevelGroup, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("triggerLevelGroup.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.triggerLevelGroup, ((int)(resources.GetObject("triggerLevelGroup.IconPadding"))));
            this.triggerLevelGroup.Name = "triggerLevelGroup";
            this.triggerLevelGroup.TabStop = false;
            // 
            // resetTriggerCtrl
            // 
            resources.ApplyResources(this.resetTriggerCtrl, "resetTriggerCtrl");
            this.errorProvider.SetError(this.resetTriggerCtrl, resources.GetString("resetTriggerCtrl.Error"));
            this.errorProvider.SetIconAlignment(this.resetTriggerCtrl, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("resetTriggerCtrl.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.resetTriggerCtrl, ((int)(resources.GetObject("resetTriggerCtrl.IconPadding"))));
            this.resetTriggerCtrl.Name = "resetTriggerCtrl";
            this.resetTriggerCtrl.UseVisualStyleBackColor = true;
            this.resetTriggerCtrl.Click += new System.EventHandler(this.resetTriggerCtrl_Click);
            // 
            // triggerLevelCtrl
            // 
            resources.ApplyResources(this.triggerLevelCtrl, "triggerLevelCtrl");
            this.triggerLevelCtrl.BackColor = System.Drawing.Color.Transparent;
            this.errorProvider.SetError(this.triggerLevelCtrl, resources.GetString("triggerLevelCtrl.Error"));
            this.errorProvider.SetIconAlignment(this.triggerLevelCtrl, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("triggerLevelCtrl.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.triggerLevelCtrl, ((int)(resources.GetObject("triggerLevelCtrl.IconPadding"))));
            this.triggerLevelCtrl.IndicatorColor = System.Drawing.Color.Black;
            this.triggerLevelCtrl.IndicatorOffset = 10F;
            this.triggerLevelCtrl.KnobColor = System.Drawing.Color.LightGray;
            this.triggerLevelCtrl.MaxValue = 100F;
            this.triggerLevelCtrl.MinValue = -100F;
            this.triggerLevelCtrl.Name = "triggerLevelCtrl";
            this.triggerLevelCtrl.Renderer = null;
            this.triggerLevelCtrl.ScaleColor = System.Drawing.Color.DarkGray;
            this.triggerLevelCtrl.StepValue = 1F;
            this.triggerLevelCtrl.Style = Mp.Visual.Analog.Knob.KnobStyle.Circular;
            this.triggerLevelCtrl.Value = 0F;
            this.triggerLevelCtrl.KnobChangeValue += new Mp.Visual.Analog.KnobChangeValue(this.triggerLevelCtrl_KnobChangeValue);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // ChannelControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.channelPanelCtrl);
            this.errorProvider.SetError(this, resources.GetString("$this.Error"));
            this.errorProvider.SetIconAlignment(this, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("$this.IconAlignment"))));
            this.errorProvider.SetIconPadding(this, ((int)(resources.GetObject("$this.IconPadding"))));
            this.Name = "ChannelControl";
            this.channelPanelCtrl.ResumeLayout(false);
            this.channelPanelCtrl.PerformLayout();
            this.horPosGroup.ResumeLayout(false);
            this.verPosGroup.ResumeLayout(false);
            this.triggerLevelGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox channelPanelCtrl;
        private System.Windows.Forms.GroupBox horPosGroup;
        private System.Windows.Forms.Button resetHorPosCtrl;
        private Mp.Visual.Analog.Knob horizontalPosCtrl;
        private System.Windows.Forms.GroupBox verPosGroup;
        private System.Windows.Forms.Button resetVerPosCtrl;
        private Mp.Visual.Analog.Knob verticalPosCtrl;
        private System.Windows.Forms.CheckBox invertCtrl;
        private System.Windows.Forms.ComboBox couplingCtrl;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox triggerCtrl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox visibleCtrl;
        private System.Windows.Forms.TextBox scaleYCtrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox triggerLevelGroup;
        private System.Windows.Forms.Button resetTriggerCtrl;
        private Mp.Visual.Analog.Knob triggerLevelCtrl;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.CheckBox singleShotBt;
    }
}
