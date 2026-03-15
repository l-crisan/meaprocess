namespace Mp.Conv.Data
{
    partial class DataConverterDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataConverterDlg));
            this.close = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.precision = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mmf = new System.Windows.Forms.RadioButton();
            this.targetBt = new System.Windows.Forms.Button();
            this.target = new System.Windows.Forms.TextBox();
            this.mdfzip = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.mdf = new System.Windows.Forms.RadioButton();
            this.tdmzip = new System.Windows.Forms.RadioButton();
            this.tdm = new System.Windows.Forms.RadioButton();
            this.csvzip = new System.Windows.Forms.RadioButton();
            this.csv = new System.Windows.Forms.RadioButton();
            this.srcBt = new System.Windows.Forms.Button();
            this.source = new System.Windows.Forms.TextBox();
            this.convert = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.percent = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.group = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // close
            // 
            resources.ApplyResources(this.close, "close");
            this.errorProvider.SetError(this.close, resources.GetString("close.Error"));
            this.errorProvider.SetIconAlignment(this.close, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("close.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.close, ((int)(resources.GetObject("close.IconPadding"))));
            this.close.Name = "close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.precision);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.mmf);
            this.groupBox3.Controls.Add(this.targetBt);
            this.groupBox3.Controls.Add(this.target);
            this.groupBox3.Controls.Add(this.mdfzip);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.mdf);
            this.groupBox3.Controls.Add(this.tdmzip);
            this.groupBox3.Controls.Add(this.tdm);
            this.groupBox3.Controls.Add(this.csvzip);
            this.groupBox3.Controls.Add(this.csv);
            this.errorProvider.SetError(this.groupBox3, resources.GetString("groupBox3.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox3, ((int)(resources.GetObject("groupBox3.IconPadding"))));
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // precision
            // 
            resources.ApplyResources(this.precision, "precision");
            this.errorProvider.SetError(this.precision, resources.GetString("precision.Error"));
            this.errorProvider.SetIconAlignment(this.precision, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("precision.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.precision, ((int)(resources.GetObject("precision.IconPadding"))));
            this.precision.Name = "precision";
            this.precision.Validating += new System.ComponentModel.CancelEventHandler(this.precision_Validating);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // mmf
            // 
            resources.ApplyResources(this.mmf, "mmf");
            this.errorProvider.SetError(this.mmf, resources.GetString("mmf.Error"));
            this.errorProvider.SetIconAlignment(this.mmf, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("mmf.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.mmf, ((int)(resources.GetObject("mmf.IconPadding"))));
            this.mmf.Name = "mmf";
            this.mmf.UseVisualStyleBackColor = true;
            this.mmf.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // targetBt
            // 
            resources.ApplyResources(this.targetBt, "targetBt");
            this.errorProvider.SetError(this.targetBt, resources.GetString("targetBt.Error"));
            this.errorProvider.SetIconAlignment(this.targetBt, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("targetBt.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.targetBt, ((int)(resources.GetObject("targetBt.IconPadding"))));
            this.targetBt.Name = "targetBt";
            this.targetBt.UseVisualStyleBackColor = true;
            this.targetBt.Click += new System.EventHandler(this.targetBt_Click);
            // 
            // target
            // 
            resources.ApplyResources(this.target, "target");
            this.errorProvider.SetError(this.target, resources.GetString("target.Error"));
            this.errorProvider.SetIconAlignment(this.target, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("target.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.target, ((int)(resources.GetObject("target.IconPadding"))));
            this.target.Name = "target";
            this.target.ReadOnly = true;
            this.target.TextChanged += new System.EventHandler(this.target_TextChanged);
            // 
            // mdfzip
            // 
            resources.ApplyResources(this.mdfzip, "mdfzip");
            this.errorProvider.SetError(this.mdfzip, resources.GetString("mdfzip.Error"));
            this.errorProvider.SetIconAlignment(this.mdfzip, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("mdfzip.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.mdfzip, ((int)(resources.GetObject("mdfzip.IconPadding"))));
            this.mdfzip.Name = "mdfzip";
            this.mdfzip.UseVisualStyleBackColor = true;
            this.mdfzip.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.errorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.errorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            // 
            // mdf
            // 
            resources.ApplyResources(this.mdf, "mdf");
            this.errorProvider.SetError(this.mdf, resources.GetString("mdf.Error"));
            this.errorProvider.SetIconAlignment(this.mdf, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("mdf.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.mdf, ((int)(resources.GetObject("mdf.IconPadding"))));
            this.mdf.Name = "mdf";
            this.mdf.UseVisualStyleBackColor = true;
            this.mdf.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // tdmzip
            // 
            resources.ApplyResources(this.tdmzip, "tdmzip");
            this.errorProvider.SetError(this.tdmzip, resources.GetString("tdmzip.Error"));
            this.errorProvider.SetIconAlignment(this.tdmzip, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tdmzip.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tdmzip, ((int)(resources.GetObject("tdmzip.IconPadding"))));
            this.tdmzip.Name = "tdmzip";
            this.tdmzip.UseVisualStyleBackColor = true;
            this.tdmzip.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // tdm
            // 
            resources.ApplyResources(this.tdm, "tdm");
            this.errorProvider.SetError(this.tdm, resources.GetString("tdm.Error"));
            this.errorProvider.SetIconAlignment(this.tdm, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tdm.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tdm, ((int)(resources.GetObject("tdm.IconPadding"))));
            this.tdm.Name = "tdm";
            this.tdm.UseVisualStyleBackColor = true;
            this.tdm.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // csvzip
            // 
            resources.ApplyResources(this.csvzip, "csvzip");
            this.errorProvider.SetError(this.csvzip, resources.GetString("csvzip.Error"));
            this.errorProvider.SetIconAlignment(this.csvzip, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("csvzip.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.csvzip, ((int)(resources.GetObject("csvzip.IconPadding"))));
            this.csvzip.Name = "csvzip";
            this.csvzip.UseVisualStyleBackColor = true;
            this.csvzip.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // csv
            // 
            resources.ApplyResources(this.csv, "csv");
            this.csv.Checked = true;
            this.errorProvider.SetError(this.csv, resources.GetString("csv.Error"));
            this.errorProvider.SetIconAlignment(this.csv, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("csv.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.csv, ((int)(resources.GetObject("csv.IconPadding"))));
            this.csv.Name = "csv";
            this.csv.TabStop = true;
            this.csv.UseVisualStyleBackColor = true;
            this.csv.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // srcBt
            // 
            resources.ApplyResources(this.srcBt, "srcBt");
            this.errorProvider.SetError(this.srcBt, resources.GetString("srcBt.Error"));
            this.errorProvider.SetIconAlignment(this.srcBt, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("srcBt.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.srcBt, ((int)(resources.GetObject("srcBt.IconPadding"))));
            this.srcBt.Name = "srcBt";
            this.srcBt.UseVisualStyleBackColor = true;
            this.srcBt.Click += new System.EventHandler(this.srcBt_Click);
            // 
            // source
            // 
            resources.ApplyResources(this.source, "source");
            this.errorProvider.SetError(this.source, resources.GetString("source.Error"));
            this.errorProvider.SetIconAlignment(this.source, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("source.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.source, ((int)(resources.GetObject("source.IconPadding"))));
            this.source.Name = "source";
            this.source.ReadOnly = true;
            this.source.TextChanged += new System.EventHandler(this.source_TextChanged);
            // 
            // convert
            // 
            resources.ApplyResources(this.convert, "convert");
            this.errorProvider.SetError(this.convert, resources.GetString("convert.Error"));
            this.errorProvider.SetIconAlignment(this.convert, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("convert.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.convert, ((int)(resources.GetObject("convert.IconPadding"))));
            this.convert.Name = "convert";
            this.convert.UseVisualStyleBackColor = true;
            this.convert.Click += new System.EventHandler(this.convert_Click);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.percent);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.convert);
            this.groupBox2.Controls.Add(this.group);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.progress);
            this.errorProvider.SetError(this.groupBox2, resources.GetString("groupBox2.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox2, ((int)(resources.GetObject("groupBox2.IconPadding"))));
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // percent
            // 
            resources.ApplyResources(this.percent, "percent");
            this.errorProvider.SetError(this.percent, resources.GetString("percent.Error"));
            this.errorProvider.SetIconAlignment(this.percent, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("percent.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.percent, ((int)(resources.GetObject("percent.IconPadding"))));
            this.percent.Name = "percent";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.errorProvider.SetError(this.label5, resources.GetString("label5.Error"));
            this.errorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label5, ((int)(resources.GetObject("label5.IconPadding"))));
            this.label5.Name = "label5";
            // 
            // group
            // 
            resources.ApplyResources(this.group, "group");
            this.errorProvider.SetError(this.group, resources.GetString("group.Error"));
            this.errorProvider.SetIconAlignment(this.group, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("group.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.group, ((int)(resources.GetObject("group.IconPadding"))));
            this.group.Name = "group";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.errorProvider.SetError(this.label3, resources.GetString("label3.Error"));
            this.errorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label3, ((int)(resources.GetObject("label3.IconPadding"))));
            this.label3.Name = "label3";
            // 
            // progress
            // 
            resources.ApplyResources(this.progress, "progress");
            this.errorProvider.SetError(this.progress, resources.GetString("progress.Error"));
            this.errorProvider.SetIconAlignment(this.progress, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("progress.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.progress, ((int)(resources.GetObject("progress.IconPadding"))));
            this.progress.Name = "progress";
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.srcBt);
            this.groupBox4.Controls.Add(this.source);
            this.errorProvider.SetError(this.groupBox4, resources.GetString("groupBox4.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox4, ((int)(resources.GetObject("groupBox4.IconPadding"))));
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // DataConverterDlg
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.close);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataConverterDlg";
            this.ShowInTaskbar = false;
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Button targetBt;
        private System.Windows.Forms.TextBox target;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button srcBt;
        private System.Windows.Forms.TextBox source;
        private System.Windows.Forms.Button convert;
        private System.Windows.Forms.Label group;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton mdfzip;
        private System.Windows.Forms.RadioButton mdf;
        private System.Windows.Forms.RadioButton tdmzip;
        private System.Windows.Forms.RadioButton tdm;
        private System.Windows.Forms.RadioButton csvzip;
        private System.Windows.Forms.RadioButton csv;
        private System.Windows.Forms.Label percent;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton mmf;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox precision;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}