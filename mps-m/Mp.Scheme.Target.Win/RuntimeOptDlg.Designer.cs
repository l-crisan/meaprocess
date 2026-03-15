namespace Mp.Scheme.Win
{
    partial class RuntimeOptDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RuntimeOptDlg));
            this.OK = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.language = new Atesion.Utils.ImgComboBox();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.showPanelTab = new System.Windows.Forms.CheckBox();
            this.ctrlBarEditProp = new System.Windows.Forms.CheckBox();
            this.title = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fixedWinSize = new System.Windows.Forms.CheckBox();
            this.editPrpBtFlag = new System.Windows.Forms.CheckBox();
            this.closeOnStop = new System.Windows.Forms.CheckBox();
            this.resetPropOnStart = new System.Windows.Forms.CheckBox();
            this.mandatoryPropFlag = new System.Windows.Forms.CheckBox();
            this.undockPanels = new System.Windows.Forms.CheckBox();
            this.startOnOpen = new System.Windows.Forms.CheckBox();
            this.showControlBar = new System.Windows.Forms.CheckBox();
            this.showStatusBar = new System.Windows.Forms.CheckBox();
            this.showMenu = new System.Windows.Forms.CheckBox();
            this.groupBoxSize = new System.Windows.Forms.GroupBox();
            this.defineSize = new System.Windows.Forms.Button();
            this.height = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.width = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.onDefaultIcon = new System.Windows.Forms.Button();
            this.icon = new System.Windows.Forms.PictureBox();
            this.onIcon = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.help = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBoxSize.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // OK
            // 
            resources.ApplyResources(this.OK, "OK");
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.language);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.showPanelTab);
            this.groupBox1.Controls.Add(this.ctrlBarEditProp);
            this.groupBox1.Controls.Add(this.title);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.fixedWinSize);
            this.groupBox1.Controls.Add(this.editPrpBtFlag);
            this.groupBox1.Controls.Add(this.closeOnStop);
            this.groupBox1.Controls.Add(this.resetPropOnStart);
            this.groupBox1.Controls.Add(this.mandatoryPropFlag);
            this.groupBox1.Controls.Add(this.undockPanels);
            this.groupBox1.Controls.Add(this.startOnOpen);
            this.groupBox1.Controls.Add(this.showControlBar);
            this.groupBox1.Controls.Add(this.showStatusBar);
            this.groupBox1.Controls.Add(this.showMenu);
            this.groupBox1.Controls.Add(this.groupBoxSize);
            this.groupBox1.Controls.Add(this.groupBox2);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // language
            // 
            this.language.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.language.FormattingEnabled = true;
            this.language.ImageList = this.imageList;
            resources.ApplyResources(this.language, "language");
            this.language.Name = "language";
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "English.ico");
            this.imageList.Images.SetKeyName(1, "Deutsch.ico");
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // showPanelTab
            // 
            resources.ApplyResources(this.showPanelTab, "showPanelTab");
            this.showPanelTab.Name = "showPanelTab";
            this.showPanelTab.UseVisualStyleBackColor = true;
            // 
            // ctrlBarEditProp
            // 
            resources.ApplyResources(this.ctrlBarEditProp, "ctrlBarEditProp");
            this.ctrlBarEditProp.Checked = true;
            this.ctrlBarEditProp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ctrlBarEditProp.Name = "ctrlBarEditProp";
            this.ctrlBarEditProp.UseVisualStyleBackColor = true;
            // 
            // title
            // 
            resources.ApplyResources(this.title, "title");
            this.title.Name = "title";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // fixedWinSize
            // 
            resources.ApplyResources(this.fixedWinSize, "fixedWinSize");
            this.fixedWinSize.Name = "fixedWinSize";
            this.fixedWinSize.UseVisualStyleBackColor = true;
            this.fixedWinSize.CheckedChanged += new System.EventHandler(this.fixetWinSize_CheckedChanged);
            // 
            // editPrpBtFlag
            // 
            resources.ApplyResources(this.editPrpBtFlag, "editPrpBtFlag");
            this.editPrpBtFlag.Checked = true;
            this.editPrpBtFlag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.editPrpBtFlag.Name = "editPrpBtFlag";
            this.editPrpBtFlag.UseVisualStyleBackColor = true;
            // 
            // closeOnStop
            // 
            resources.ApplyResources(this.closeOnStop, "closeOnStop");
            this.closeOnStop.Name = "closeOnStop";
            this.closeOnStop.UseVisualStyleBackColor = true;
            // 
            // resetPropOnStart
            // 
            resources.ApplyResources(this.resetPropOnStart, "resetPropOnStart");
            this.resetPropOnStart.Name = "resetPropOnStart";
            this.resetPropOnStart.UseVisualStyleBackColor = true;
            // 
            // mandatoryPropFlag
            // 
            resources.ApplyResources(this.mandatoryPropFlag, "mandatoryPropFlag");
            this.mandatoryPropFlag.Checked = true;
            this.mandatoryPropFlag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mandatoryPropFlag.Name = "mandatoryPropFlag";
            this.mandatoryPropFlag.UseVisualStyleBackColor = true;
            // 
            // undockPanels
            // 
            resources.ApplyResources(this.undockPanels, "undockPanels");
            this.undockPanels.Checked = true;
            this.undockPanels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.undockPanels.Name = "undockPanels";
            this.undockPanels.UseVisualStyleBackColor = true;
            // 
            // startOnOpen
            // 
            resources.ApplyResources(this.startOnOpen, "startOnOpen");
            this.startOnOpen.Name = "startOnOpen";
            this.startOnOpen.UseVisualStyleBackColor = true;
            // 
            // showControlBar
            // 
            resources.ApplyResources(this.showControlBar, "showControlBar");
            this.showControlBar.Checked = true;
            this.showControlBar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showControlBar.Name = "showControlBar";
            this.showControlBar.UseVisualStyleBackColor = true;
            // 
            // showStatusBar
            // 
            resources.ApplyResources(this.showStatusBar, "showStatusBar");
            this.showStatusBar.Checked = true;
            this.showStatusBar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showStatusBar.Name = "showStatusBar";
            this.showStatusBar.UseVisualStyleBackColor = true;
            // 
            // showMenu
            // 
            resources.ApplyResources(this.showMenu, "showMenu");
            this.showMenu.Checked = true;
            this.showMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showMenu.Name = "showMenu";
            this.showMenu.UseVisualStyleBackColor = true;
            // 
            // groupBoxSize
            // 
            this.groupBoxSize.Controls.Add(this.defineSize);
            this.groupBoxSize.Controls.Add(this.height);
            this.groupBoxSize.Controls.Add(this.label2);
            this.groupBoxSize.Controls.Add(this.width);
            this.groupBoxSize.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBoxSize, "groupBoxSize");
            this.groupBoxSize.Name = "groupBoxSize";
            this.groupBoxSize.TabStop = false;
            // 
            // defineSize
            // 
            resources.ApplyResources(this.defineSize, "defineSize");
            this.defineSize.Name = "defineSize";
            this.defineSize.UseVisualStyleBackColor = true;
            this.defineSize.Click += new System.EventHandler(this.defineSize_Click);
            // 
            // height
            // 
            resources.ApplyResources(this.height, "height");
            this.height.Name = "height";
            this.height.Validating += new System.ComponentModel.CancelEventHandler(this.height_Validating);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // width
            // 
            resources.ApplyResources(this.width, "width");
            this.width.Name = "width";
            this.width.Validating += new System.ComponentModel.CancelEventHandler(this.width_Validating);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.onDefaultIcon);
            this.groupBox2.Controls.Add(this.icon);
            this.groupBox2.Controls.Add(this.onIcon);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // onDefaultIcon
            // 
            resources.ApplyResources(this.onDefaultIcon, "onDefaultIcon");
            this.onDefaultIcon.Name = "onDefaultIcon";
            this.onDefaultIcon.UseVisualStyleBackColor = true;
            this.onDefaultIcon.Click += new System.EventHandler(this.button1_Click);
            // 
            // icon
            // 
            this.icon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.icon, "icon");
            this.icon.Name = "icon";
            this.icon.TabStop = false;
            // 
            // onIcon
            // 
            resources.ApplyResources(this.onIcon, "onIcon");
            this.onIcon.Name = "onIcon";
            this.onIcon.UseVisualStyleBackColor = true;
            this.onIcon.Click += new System.EventHandler(this.onIcon_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // help
            // 
            resources.ApplyResources(this.help, "help");
            this.help.Name = "help";
            this.help.UseVisualStyleBackColor = true;
            this.help.Click += new System.EventHandler(this.help_Click);
            // 
            // RuntimeOptDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.help);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RuntimeOptDlg";
            this.ShowInTaskbar = false;
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.RuntimeOptDlg_HelpRequested);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxSize.ResumeLayout(false);
            this.groupBoxSize.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBoxSize;
        private System.Windows.Forms.TextBox height;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox width;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox fixedWinSize;
        private System.Windows.Forms.CheckBox showStatusBar;
        private System.Windows.Forms.CheckBox showMenu;
        private System.Windows.Forms.Button defineSize;
        private System.Windows.Forms.CheckBox startOnOpen;
        private System.Windows.Forms.CheckBox showControlBar;
        private System.Windows.Forms.CheckBox undockPanels;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.CheckBox mandatoryPropFlag;
        private System.Windows.Forms.CheckBox resetPropOnStart;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.CheckBox editPrpBtFlag;
        private System.Windows.Forms.CheckBox closeOnStop;
        private System.Windows.Forms.TextBox title;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button onIcon;
        private System.Windows.Forms.PictureBox icon;
        private System.Windows.Forms.Button onDefaultIcon;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ctrlBarEditProp;
        private System.Windows.Forms.CheckBox showPanelTab;
        private Atesion.Utils.ImgComboBox language;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ImageList imageList;
    }
}