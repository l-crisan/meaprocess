namespace Mp.Runtime.App
{
    partial class AboutBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
            this.textBoxDescription = new System.Windows.Forms.Label();
            this.copyRight = new System.Windows.Forms.Label();
            this.version = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxDescription
            // 
            resources.ApplyResources(this.textBoxDescription, "textBoxDescription");
            this.textBoxDescription.BackColor = System.Drawing.Color.Transparent;
            this.textBoxDescription.Name = "textBoxDescription";
            // 
            // copyRight
            // 
            resources.ApplyResources(this.copyRight, "copyRight");
            this.copyRight.BackColor = System.Drawing.Color.Transparent;
            this.copyRight.Name = "copyRight";
            // 
            // version
            // 
            resources.ApplyResources(this.version, "version");
            this.version.BackColor = System.Drawing.Color.Transparent;
            this.version.Name = "version";
            // 
            // OK
            // 
            resources.ApplyResources(this.OK, "OK");
            this.OK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            // 
            // AboutBox
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.OK;
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.copyRight);
            this.Controls.Add(this.version);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.AboutBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label textBoxDescription;
        private System.Windows.Forms.Label copyRight;
        private System.Windows.Forms.Label version;
        private System.Windows.Forms.Button OK;



    }
}
