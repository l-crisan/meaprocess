using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Mp.Visual.Docking
{
    public delegate void LineDoubleClick(string line);

    public enum MessageType
    {
        Info = 0,
        Warning, 
        Error,	 
        Question,
        Stop,	 
        EventMsg,
    }

    public partial class OutputWindow : DockContent
    {
        private DockPanel _panel;
        public OutputWindow(DockPanel panel)
        {
            InitializeComponent();
            _panel = panel;
            this.ShowHint = DockState.DockBottomAutoHide;
            this.outputCtrl.SmallImageList = this.imageList;     
            this.TabText = StringResource.Out;
            this.Text = StringResource.Out;         
            this.Icon = Images.TextWindow;            

        }

        private string GetMessageType(MessageType index)
        {
            switch (index)
            {
                case MessageType.Info:
                    return StringResource.Info;
                case MessageType.Warning:
                    return StringResource.Warning;
                case MessageType.Error:
                    return StringResource.Error;
                case MessageType.EventMsg:
                    return StringResource.Event;
            }
            return ""; 
        }

        public void Clear()
        {
            outputCtrl.Items.Clear();
        }

        private delegate void WriteMessageLineDelegate(string[] line, int type);

        private void WriteMessage(string[] line, int type)
        {
            outputCtrl.Items.Add(new ListViewItem(line, type));
            outputCtrl.EnsureVisible(outputCtrl.Items.Count - 1);
            this.Activate();
            this.Focus();
            Show(_panel);
        }

        public void WriteLine(string text, MessageType type)
        {
            string[] msg = { "", GetMessageType(type), text };
            this.BeginInvoke(new WriteMessageLineDelegate(WriteMessage), new object[] { msg, (int)type });
        }

        private void cleaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "*.txt|*.txt|*.log|*.log|*.*|*.*";
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                using (StreamWriter fs = new StreamWriter(dlg.FileName))
                {
                    foreach(ListViewItem item in outputCtrl.Items)
                    {
                        fs.Write(item.SubItems[1].Text);
                        fs.Write(" \t");
                        fs.WriteLine(item.SubItems[2].Text);
                    }

                    fs.Close();
                }
            }
            catch(Exception ex)
            {
                WriteLine(ex.Message,MessageType.Error);
            }
        }

        public string Messages
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                foreach (ListViewItem item in outputCtrl.Items)
                {
                    sb.Append(item.SubItems[1].Text);
                    sb.Append(" \t");
                    sb.Append(item.SubItems[2].Text);
                    sb.Append("\n");
                }

                return sb.ToString();
            }
        }

        private void OutputWindow_Load(object sender, EventArgs e)
        {

        }
    }
}