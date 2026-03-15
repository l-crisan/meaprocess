namespace Mp.Mod.Storage
{
    partial class DataStoragePSDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataStoragePSDlg));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.storageDescription = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.desFromProperty = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.stProp = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel2 = new System.Windows.Forms.Panel();
            this.remove = new System.Windows.Forms.Button();
            this.addProperty = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this._PSName = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.command = new System.Windows.Forms.TextBox();
            this.cmdFromProp = new System.Windows.Forms.Button();
            this.onCommandFile = new System.Windows.Forms.Button();
            this.commandParam = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.askUserForOverwrite = new System.Windows.Forms.RadioButton();
            this.metaFileFormat = new System.Windows.Forms.ComboBox();
            this.radioButtonCreateNewFile = new System.Windows.Forms.RadioButton();
            this.genarateTimeStampSignal = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButtonOverwrite = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.fromProperty = new System.Windows.Forms.Button();
            this.fileCtrl = new System.Windows.Forms.TextBox();
            this.AddFile = new System.Windows.Forms.Button();
            this.help = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.storageDescription);
            this.groupBox2.Controls.Add(this.panel1);
            this.errorProvider.SetIconAlignment(this.groupBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox2.IconAlignment"))));
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // storageDescription
            // 
            this.storageDescription.AcceptsReturn = true;
            resources.ApplyResources(this.storageDescription, "storageDescription");
            this.errorProvider.SetIconAlignment(this.storageDescription, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("storageDescription.IconAlignment"))));
            this.storageDescription.Name = "storageDescription";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.desFromProperty);
            resources.ApplyResources(this.panel1, "panel1");
            this.errorProvider.SetIconAlignment(this.panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel1.IconAlignment"))));
            this.panel1.Name = "panel1";
            // 
            // desFromProperty
            // 
            this.errorProvider.SetIconAlignment(this.desFromProperty, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("desFromProperty.IconAlignment"))));
            resources.ApplyResources(this.desFromProperty, "desFromProperty");
            this.desFromProperty.Name = "desFromProperty";
            this.desFromProperty.UseVisualStyleBackColor = true;
            this.desFromProperty.Click += new System.EventHandler(this.desFromProperty_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.stProp);
            this.groupBox5.Controls.Add(this.panel2);
            this.errorProvider.SetIconAlignment(this.groupBox5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox5.IconAlignment"))));
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // stProp
            // 
            this.stProp.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            resources.ApplyResources(this.stProp, "stProp");
            this.errorProvider.SetIconAlignment(this.stProp, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("stProp.IconAlignment"))));
            this.stProp.Name = "stProp";
            this.stProp.UseCompatibleStateImageBehavior = false;
            this.stProp.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.remove);
            this.panel2.Controls.Add(this.addProperty);
            resources.ApplyResources(this.panel2, "panel2");
            this.errorProvider.SetIconAlignment(this.panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel2.IconAlignment"))));
            this.panel2.Name = "panel2";
            // 
            // remove
            // 
            this.errorProvider.SetIconAlignment(this.remove, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("remove.IconAlignment"))));
            resources.ApplyResources(this.remove, "remove");
            this.remove.Name = "remove";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.remove_Click);
            // 
            // addProperty
            // 
            this.errorProvider.SetIconAlignment(this.addProperty, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("addProperty.IconAlignment"))));
            resources.ApplyResources(this.addProperty, "addProperty");
            this.addProperty.Name = "addProperty";
            this.addProperty.UseVisualStyleBackColor = true;
            this.addProperty.Click += new System.EventHandler(this.addProperty_Click);
            // 
            // OK
            // 
            this.errorProvider.SetIconAlignment(this.OK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("OK.IconAlignment"))));
            resources.ApplyResources(this.OK, "OK");
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.errorProvider.SetIconAlignment(this.Cancel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("Cancel.IconAlignment"))));
            resources.ApplyResources(this.Cancel, "Cancel");
            this.Cancel.Name = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this._PSName);
            this.errorProvider.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.label1.Name = "label1";
            // 
            // _PSName
            // 
            this.errorProvider.SetIconAlignment(this._PSName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("_PSName.IconAlignment"))));
            resources.ApplyResources(this._PSName, "_PSName");
            this._PSName.Name = "_PSName";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.groupBox7);
            this.groupBox6.Controls.Add(this.commandParam);
            this.groupBox6.Controls.Add(this.label4);
            this.errorProvider.SetIconAlignment(this.groupBox6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox6.IconAlignment"))));
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.command);
            this.groupBox7.Controls.Add(this.cmdFromProp);
            this.groupBox7.Controls.Add(this.onCommandFile);
            this.errorProvider.SetIconAlignment(this.groupBox7, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox7.IconAlignment"))));
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // command
            // 
            this.errorProvider.SetIconAlignment(this.command, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("command.IconAlignment"))));
            resources.ApplyResources(this.command, "command");
            this.command.Name = "command";
            // 
            // cmdFromProp
            // 
            this.errorProvider.SetIconAlignment(this.cmdFromProp, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("cmdFromProp.IconAlignment"))));
            resources.ApplyResources(this.cmdFromProp, "cmdFromProp");
            this.cmdFromProp.Name = "cmdFromProp";
            this.cmdFromProp.UseVisualStyleBackColor = true;
            this.cmdFromProp.Click += new System.EventHandler(this.cmdFromProp_Click);
            // 
            // onCommandFile
            // 
            this.errorProvider.SetIconAlignment(this.onCommandFile, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("onCommandFile.IconAlignment"))));
            resources.ApplyResources(this.onCommandFile, "onCommandFile");
            this.onCommandFile.Name = "onCommandFile";
            this.onCommandFile.UseVisualStyleBackColor = true;
            this.onCommandFile.Click += new System.EventHandler(this.onCommandFile_Click);
            // 
            // commandParam
            // 
            this.errorProvider.SetIconAlignment(this.commandParam, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("commandParam.IconAlignment"))));
            resources.ApplyResources(this.commandParam, "commandParam");
            this.commandParam.Name = "commandParam";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.label4.Name = "label4";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.askUserForOverwrite);
            this.groupBox4.Controls.Add(this.metaFileFormat);
            this.groupBox4.Controls.Add(this.radioButtonCreateNewFile);
            this.groupBox4.Controls.Add(this.genarateTimeStampSignal);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.radioButtonOverwrite);
            this.errorProvider.SetIconAlignment(this.groupBox4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox4.IconAlignment"))));
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // askUserForOverwrite
            // 
            resources.ApplyResources(this.askUserForOverwrite, "askUserForOverwrite");
            this.errorProvider.SetIconAlignment(this.askUserForOverwrite, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("askUserForOverwrite.IconAlignment"))));
            this.askUserForOverwrite.Name = "askUserForOverwrite";
            this.askUserForOverwrite.TabStop = true;
            this.askUserForOverwrite.UseVisualStyleBackColor = true;
            // 
            // metaFileFormat
            // 
            this.metaFileFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.metaFileFormat.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.metaFileFormat, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("metaFileFormat.IconAlignment"))));
            this.metaFileFormat.Items.AddRange(new object[] {
            resources.GetString("metaFileFormat.Items"),
            resources.GetString("metaFileFormat.Items1")});
            resources.ApplyResources(this.metaFileFormat, "metaFileFormat");
            this.metaFileFormat.Name = "metaFileFormat";
            // 
            // radioButtonCreateNewFile
            // 
            resources.ApplyResources(this.radioButtonCreateNewFile, "radioButtonCreateNewFile");
            this.errorProvider.SetIconAlignment(this.radioButtonCreateNewFile, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("radioButtonCreateNewFile.IconAlignment"))));
            this.radioButtonCreateNewFile.Name = "radioButtonCreateNewFile";
            this.radioButtonCreateNewFile.TabStop = true;
            this.radioButtonCreateNewFile.UseVisualStyleBackColor = true;
            // 
            // genarateTimeStampSignal
            // 
            resources.ApplyResources(this.genarateTimeStampSignal, "genarateTimeStampSignal");
            this.errorProvider.SetIconAlignment(this.genarateTimeStampSignal, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("genarateTimeStampSignal.IconAlignment"))));
            this.genarateTimeStampSignal.Name = "genarateTimeStampSignal";
            this.genarateTimeStampSignal.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.errorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.label3.Name = "label3";
            // 
            // radioButtonOverwrite
            // 
            resources.ApplyResources(this.radioButtonOverwrite, "radioButtonOverwrite");
            this.errorProvider.SetIconAlignment(this.radioButtonOverwrite, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("radioButtonOverwrite.IconAlignment"))));
            this.radioButtonOverwrite.Name = "radioButtonOverwrite";
            this.radioButtonOverwrite.TabStop = true;
            this.radioButtonOverwrite.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.fromProperty);
            this.groupBox3.Controls.Add(this.fileCtrl);
            this.groupBox3.Controls.Add(this.AddFile);
            this.errorProvider.SetIconAlignment(this.groupBox3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox3.IconAlignment"))));
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // fromProperty
            // 
            this.errorProvider.SetIconAlignment(this.fromProperty, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("fromProperty.IconAlignment"))));
            resources.ApplyResources(this.fromProperty, "fromProperty");
            this.fromProperty.Name = "fromProperty";
            this.fromProperty.UseVisualStyleBackColor = true;
            this.fromProperty.Click += new System.EventHandler(this.fromProperty_Click);
            // 
            // fileCtrl
            // 
            this.errorProvider.SetIconAlignment(this.fileCtrl, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("fileCtrl.IconAlignment"))));
            resources.ApplyResources(this.fileCtrl, "fileCtrl");
            this.fileCtrl.Name = "fileCtrl";
            // 
            // AddFile
            // 
            this.errorProvider.SetIconAlignment(this.AddFile, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("AddFile.IconAlignment"))));
            resources.ApplyResources(this.AddFile, "AddFile");
            this.AddFile.Name = "AddFile";
            this.AddFile.UseVisualStyleBackColor = true;
            this.AddFile.Click += new System.EventHandler(this.AddFile_Click);
            // 
            // help
            // 
            this.errorProvider.SetIconAlignment(this.help, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("help.IconAlignment"))));
            resources.ApplyResources(this.help, "help");
            this.help.Name = "help";
            this.help.UseVisualStyleBackColor = true;
            this.help.Click += new System.EventHandler(this.help_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // DataStoragePSDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.Controls.Add(this.help);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataStoragePSDlg";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.PoDataStoragePSDlg_Load);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.DataStoragePSDlg_HelpRequested);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button AddFile;
        private System.Windows.Forms.TextBox fileCtrl;
        private System.Windows.Forms.TextBox _PSName;
        private System.Windows.Forms.CheckBox genarateTimeStampSignal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox storageDescription;
        private System.Windows.Forms.ComboBox metaFileFormat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button fromProperty;
        private System.Windows.Forms.RadioButton radioButtonCreateNewFile;
        private System.Windows.Forms.RadioButton radioButtonOverwrite;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button desFromProperty;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button addProperty;
        private System.Windows.Forms.ListView stProp;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox command;
        private System.Windows.Forms.Button cmdFromProp;
        private System.Windows.Forms.Button onCommandFile;
        private System.Windows.Forms.TextBox commandParam;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton askUserForOverwrite;
    }
}