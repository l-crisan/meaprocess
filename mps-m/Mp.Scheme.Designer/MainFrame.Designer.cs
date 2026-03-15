namespace Mp.Scheme.Designer
{
    partial class MainFrame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrame));
            Mp.Visual.Docking.DockPanelSkin dockPanelSkin1 = new Mp.Visual.Docking.DockPanelSkin();
            Mp.Visual.Docking.AutoHideStripSkin autoHideStripSkin1 = new Mp.Visual.Docking.AutoHideStripSkin();
            Mp.Visual.Docking.DockPanelGradient dockPanelGradient1 = new Mp.Visual.Docking.DockPanelGradient();
            Mp.Visual.Docking.TabGradient tabGradient1 = new Mp.Visual.Docking.TabGradient();
            Mp.Visual.Docking.DockPaneStripSkin dockPaneStripSkin1 = new Mp.Visual.Docking.DockPaneStripSkin();
            Mp.Visual.Docking.DockPaneStripGradient dockPaneStripGradient1 = new Mp.Visual.Docking.DockPaneStripGradient();
            Mp.Visual.Docking.TabGradient tabGradient2 = new Mp.Visual.Docking.TabGradient();
            Mp.Visual.Docking.DockPanelGradient dockPanelGradient2 = new Mp.Visual.Docking.DockPanelGradient();
            Mp.Visual.Docking.TabGradient tabGradient3 = new Mp.Visual.Docking.TabGradient();
            Mp.Visual.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new Mp.Visual.Docking.DockPaneStripToolWindowGradient();
            Mp.Visual.Docking.TabGradient tabGradient4 = new Mp.Visual.Docking.TabGradient();
            Mp.Visual.Docking.TabGradient tabGradient5 = new Mp.Visual.Docking.TabGradient();
            Mp.Visual.Docking.DockPanelGradient dockPanelGradient3 = new Mp.Visual.Docking.DockPanelGradient();
            Mp.Visual.Docking.TabGradient tabGradient6 = new Mp.Visual.Docking.TabGradient();
            Mp.Visual.Docking.TabGradient tabGradient7 = new Mp.Visual.Docking.TabGradient();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonInsertImage = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonInsertLabel = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFrame = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonAlignLefts = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAlignRights = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAlignsTop = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAlignBottoms = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSameWidth = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSameHeight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSameDistanceH = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSameDistanceV = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonBringToFront = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSendToBack = new System.Windows.Forms.ToolStripButton();
            this.dockPanel = new Mp.Visual.Docking.DockPanel();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripDelete,
            this.toolStripSeparator1,
            this.toolStripButtonInsertImage,
            this.toolStripButtonInsertLabel,
            this.toolStripButtonFrame,
            this.toolStripSeparator2,
            this.toolStripButtonAlignLefts,
            this.toolStripButtonAlignRights,
            this.toolStripButtonAlignsTop,
            this.toolStripButtonAlignBottoms,
            this.toolStripSeparator3,
            this.toolStripButtonSameWidth,
            this.toolStripButtonSameHeight,
            this.toolStripSeparator4,
            this.toolStripButtonSameDistanceH,
            this.toolStripButtonSameDistanceV,
            this.toolStripSeparator5,
            this.toolStripButtonBringToFront,
            this.toolStripButtonSendToBack});
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Name = "toolStrip";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.OnNewClick);
            // 
            // toolStripDelete
            // 
            this.toolStripDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripDelete, "toolStripDelete");
            this.toolStripDelete.Name = "toolStripDelete";
            this.toolStripDelete.Click += new System.EventHandler(this.OnDeleteClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripButtonInsertImage
            // 
            this.toolStripButtonInsertImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonInsertImage, "toolStripButtonInsertImage");
            this.toolStripButtonInsertImage.Name = "toolStripButtonInsertImage";
            this.toolStripButtonInsertImage.Click += new System.EventHandler(this.OnInsertImageClick);
            // 
            // toolStripButtonInsertLabel
            // 
            this.toolStripButtonInsertLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonInsertLabel, "toolStripButtonInsertLabel");
            this.toolStripButtonInsertLabel.Name = "toolStripButtonInsertLabel";
            this.toolStripButtonInsertLabel.Click += new System.EventHandler(this.OnInsertLabelClick);
            // 
            // toolStripButtonFrame
            // 
            this.toolStripButtonFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonFrame, "toolStripButtonFrame");
            this.toolStripButtonFrame.Name = "toolStripButtonFrame";
            this.toolStripButtonFrame.Click += new System.EventHandler(this.OnAddFrameClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // toolStripButtonAlignLefts
            // 
            this.toolStripButtonAlignLefts.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonAlignLefts, "toolStripButtonAlignLefts");
            this.toolStripButtonAlignLefts.Name = "toolStripButtonAlignLefts";
            this.toolStripButtonAlignLefts.Click += new System.EventHandler(this.OnAlignLeftsClick);
            // 
            // toolStripButtonAlignRights
            // 
            this.toolStripButtonAlignRights.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonAlignRights, "toolStripButtonAlignRights");
            this.toolStripButtonAlignRights.Name = "toolStripButtonAlignRights";
            this.toolStripButtonAlignRights.Click += new System.EventHandler(this.OnAlignRightsClick);
            // 
            // toolStripButtonAlignsTop
            // 
            this.toolStripButtonAlignsTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonAlignsTop, "toolStripButtonAlignsTop");
            this.toolStripButtonAlignsTop.Name = "toolStripButtonAlignsTop";
            this.toolStripButtonAlignsTop.Click += new System.EventHandler(this.OnAlignsTopClick);
            // 
            // toolStripButtonAlignBottoms
            // 
            this.toolStripButtonAlignBottoms.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonAlignBottoms, "toolStripButtonAlignBottoms");
            this.toolStripButtonAlignBottoms.Name = "toolStripButtonAlignBottoms";
            this.toolStripButtonAlignBottoms.Click += new System.EventHandler(this.OnAlignBottomsClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // toolStripButtonSameWidth
            // 
            this.toolStripButtonSameWidth.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonSameWidth, "toolStripButtonSameWidth");
            this.toolStripButtonSameWidth.Name = "toolStripButtonSameWidth";
            this.toolStripButtonSameWidth.Click += new System.EventHandler(this.OnSameWidthClick);
            // 
            // toolStripButtonSameHeight
            // 
            this.toolStripButtonSameHeight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonSameHeight, "toolStripButtonSameHeight");
            this.toolStripButtonSameHeight.Name = "toolStripButtonSameHeight";
            this.toolStripButtonSameHeight.Click += new System.EventHandler(this.OnSameHeightClick);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // toolStripButtonSameDistanceH
            // 
            this.toolStripButtonSameDistanceH.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonSameDistanceH, "toolStripButtonSameDistanceH");
            this.toolStripButtonSameDistanceH.Name = "toolStripButtonSameDistanceH";
            this.toolStripButtonSameDistanceH.Click += new System.EventHandler(this.OnSameDistanceHClick);
            // 
            // toolStripButtonSameDistanceV
            // 
            this.toolStripButtonSameDistanceV.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonSameDistanceV, "toolStripButtonSameDistanceV");
            this.toolStripButtonSameDistanceV.Name = "toolStripButtonSameDistanceV";
            this.toolStripButtonSameDistanceV.Click += new System.EventHandler(this.OnSameDistanceVClick);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // toolStripButtonBringToFront
            // 
            this.toolStripButtonBringToFront.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonBringToFront, "toolStripButtonBringToFront");
            this.toolStripButtonBringToFront.Name = "toolStripButtonBringToFront";
            this.toolStripButtonBringToFront.Click += new System.EventHandler(this.OnBringToFrontClick);
            // 
            // toolStripButtonSendToBack
            // 
            this.toolStripButtonSendToBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonSendToBack, "toolStripButtonSendToBack");
            this.toolStripButtonSendToBack.Name = "toolStripButtonSendToBack";
            this.toolStripButtonSendToBack.Click += new System.EventHandler(this.OnSendToBackClick);
            // 
            // dockPanel
            // 
            this.dockPanel.ActivateOnDragOver = false;
            this.dockPanel.ActiveAutoHideContent = null;
            this.dockPanel.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.dockPanel, "dockPanel");
            this.dockPanel.DockBackColor = System.Drawing.SystemColors.Control;
            this.dockPanel.DocumentStyle = Mp.Visual.Docking.DocumentStyle.DockingWindow;
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.ShowDocumentIcon = true;
            dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
            tabGradient1.EndColor = System.Drawing.SystemColors.Control;
            tabGradient1.StartColor = System.Drawing.SystemColors.Control;
            tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin1.TabGradient = tabGradient1;
            dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
            tabGradient2.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
            dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
            tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
            dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
            tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
            tabGradient5.EndColor = System.Drawing.SystemColors.Control;
            tabGradient5.StartColor = System.Drawing.SystemColors.Control;
            tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
            dockPanelGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
            tabGradient6.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient6.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient6.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
            tabGradient7.EndColor = System.Drawing.Color.Transparent;
            tabGradient7.StartColor = System.Drawing.Color.Transparent;
            tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
            dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
            dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
            this.dockPanel.Skin = dockPanelSkin1;
            // 
            // MainFrame
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.toolStrip);
            this.Name = "MainFrame";
            this.ShowInTaskbar = false;
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonInsertLabel;
        private Mp.Visual.Docking.DockPanel dockPanel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlignLefts;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlignRights;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlignsTop;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlignBottoms;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonSameWidth;
        private System.Windows.Forms.ToolStripButton toolStripButtonSameHeight;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButtonBringToFront;
        private System.Windows.Forms.ToolStripButton toolStripButtonSendToBack;
        private System.Windows.Forms.ToolStripButton toolStripButtonFrame;
        private System.Windows.Forms.ToolStripButton toolStripButtonInsertImage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButtonSameDistanceH;
        private System.Windows.Forms.ToolStripButton toolStripButtonSameDistanceV;
    }
}

