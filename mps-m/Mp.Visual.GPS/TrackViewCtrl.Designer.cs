namespace Mp.Visual.GPS
{
    partial class TrackViewCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrackViewCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.distance = new System.Windows.Forms.Label();
            this.altitude = new System.Windows.Forms.Label();
            this.speed = new System.Windows.Forms.Label();
            this.longitude = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.latitude = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.time = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.date = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.localTime = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.led = new  Mp.Visual.GPS.Led();
            this.panel1 = new Mp.Visual.GPS.ControlPanel();
            this.onZoomP = new System.Windows.Forms.Button();
            this.onZoomM = new System.Windows.Forms.Button();
            this.contextMenuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // distance
            // 
            resources.ApplyResources(this.distance, "distance");
            this.distance.Name = "distance";
            // 
            // altitude
            // 
            resources.ApplyResources(this.altitude, "altitude");
            this.altitude.Name = "altitude";
            // 
            // speed
            // 
            resources.ApplyResources(this.speed, "speed");
            this.speed.Name = "speed";
            // 
            // longitude
            // 
            resources.ApplyResources(this.longitude, "longitude");
            this.longitude.Name = "longitude";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // latitude
            // 
            resources.ApplyResources(this.latitude, "latitude");
            this.latitude.Name = "latitude";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // statusLabel
            // 
            resources.ApplyResources(this.statusLabel, "statusLabel");
            this.statusLabel.BackColor = System.Drawing.Color.Transparent;
            this.statusLabel.Name = "statusLabel";
            // 
            // time
            // 
            resources.ApplyResources(this.time, "time");
            this.time.Name = "time";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // date
            // 
            resources.ApplyResources(this.date, "date");
            this.date.Name = "date";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // localTime
            // 
            resources.ApplyResources(this.localTime, "localTime");
            this.localTime.Name = "localTime";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showPointsToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            // 
            // showPointsToolStripMenuItem
            // 
            this.showPointsToolStripMenuItem.Name = "showPointsToolStripMenuItem";
            resources.ApplyResources(this.showPointsToolStripMenuItem, "showPointsToolStripMenuItem");
            this.showPointsToolStripMenuItem.Click += new System.EventHandler(this.showPointsToolStripMenuItem_Click);
            // 
            // led
            // 
            this.led.BackColor = System.Drawing.Color.Transparent;
            this.led.ColorOff = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.led.ColorOn = System.Drawing.Color.GreenYellow;
            resources.ApplyResources(this.led, "led");
            this.led.Name = "led";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.onZoomP);
            this.panel1.Controls.Add(this.onZoomM);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // onZoomP
            // 
            this.onZoomP.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.onZoomP, "onZoomP");
            this.onZoomP.Name = "onZoomP";
            this.onZoomP.UseVisualStyleBackColor = false;
            this.onZoomP.Click += new System.EventHandler(this.onZoomP_Click);
            // 
            // onZoomM
            // 
            this.onZoomM.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.onZoomM, "onZoomM");
            this.onZoomM.Name = "onZoomM";
            this.onZoomM.UseVisualStyleBackColor = false;
            this.onZoomM.Click += new System.EventHandler(this.onZoomM_Click);
            // 
            // TrackViewCtrl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSeaGreen;
            this.ContextMenuStrip = this.contextMenuStrip;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.localTime);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.date);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.led);
            this.Controls.Add(this.time);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.latitude);
            this.Controls.Add(this.distance);
            this.Controls.Add(this.altitude);
            this.Controls.Add(this.longitude);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.speed);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "TrackViewCtrl";
            this.BackColorChanged += new System.EventHandler(this.TrackViewCtrl_BackColorChanged);
            this.DoubleClick += new System.EventHandler(this.TrackViewCtrl_DoubleClick);
            this.contextMenuStrip.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button onZoomM;
        private System.Windows.Forms.Button onZoomP;
        private ControlPanel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label distance;
        private System.Windows.Forms.Label altitude;
        private System.Windows.Forms.Label speed;
        private System.Windows.Forms.Label longitude;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label latitude;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label time;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label date;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label localTime;
        private System.Windows.Forms.Label label5;
        private Led led;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showPointsToolStripMenuItem;
    }
}
