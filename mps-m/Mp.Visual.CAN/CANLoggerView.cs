using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace Mp.Visual.CAN
{
    public enum CANLoggerViewMode
    {
        Logging,
        Counting
    }


    public partial class CANLoggerView : UserControl
    {
        private Hashtable _messages = new Hashtable();
        private Hashtable _highlighting = new Hashtable();
        private Mutex _mutex = new Mutex();
        private System.Windows.Forms.Timer _updateTimer = new System.Windows.Forms.Timer();
        private List<string[]> _itemsToUpdate = new List<string[]>();
        private int _noOfMsg = 1024;
        private bool _freeze = false;
        private Hashtable _idCounter = new Hashtable();
        private Hashtable _id2Item = new Hashtable();
        private Hashtable _id2TimeStamp = new Hashtable();

        public CANLoggerView()
        {
            InitializeComponent();
            _updateTimer.Interval = 250;
            _updateTimer.Tick += new EventHandler(OnUpdateTimer);
            countingToolStripMenuItem.Checked = false;
            traceToolStripMenuItem.Checked = true;
        }

        private void OnUpdateTimer(object sender, EventArgs e)
        {
            _mutex.WaitOne();

            if (_freeze)
            {
                _itemsToUpdate.Clear();
                _mutex.ReleaseMutex();
                return;
            }

            messageView.BeginUpdate();
            Hashtable id2data = new Hashtable();

            foreach (string[] itemData in _itemsToUpdate)
            {
                uint id = Convert.ToUInt32(itemData[1]);

                if (countingToolStripMenuItem.Checked)
                {                    
                   id2data[id] = itemData;
                }
                else
                {
                    if (showIDHexadezimalToolStripMenuItem.Checked)
                        itemData[1] = id.ToString("X");

                    ListViewItem item = new ListViewItem(itemData);

                    if (_highlighting.ContainsKey(id))
                        item.BackColor = (Color)_highlighting[id];

                    messageView.Items.Add(item);
                }
            }

            if (countingToolStripMenuItem.Checked)
            {
                foreach (DictionaryEntry entry in id2data)
                {
                    uint id = (uint) entry.Key;
                    string[] itemData = (string[])entry.Value;

                    if (showIDHexadezimalToolStripMenuItem.Checked)
                        itemData[1] = id.ToString("X");

                    if (_id2Item.ContainsKey(id))
                    {
                        ListViewItem item = (ListViewItem)_id2Item[id];

                        for (int i = 0; i < item.SubItems.Count; ++i)
                            item.SubItems[i].Text = itemData[i];

                        if (_highlighting.ContainsKey(id))
                            item.BackColor = (Color)_highlighting[id];
                        else
                            item.BackColor = Color.White;
                    }
                    else
                    {
                        ListViewItem item = new ListViewItem(itemData);
                        messageView.Items.Add(item);
                        _id2Item[id] = item;

                        if (_highlighting.ContainsKey(id))
                            item.BackColor = (Color)_highlighting[id];
                    }
                }
            }

            if(messageView.Items.Count > _noOfMsg)
            {
                int count = messageView.Items.Count;
                
                for (int i = 0; i < (count - _noOfMsg); ++i)
                    messageView.Items.RemoveAt(0);
            }

            if(traceToolStripMenuItem.Checked)
                if (messageView.Items.Count > 0 && _itemsToUpdate.Count > 0)
                    messageView.EnsureVisible(messageView.Items.Count - 1);

            _itemsToUpdate.Clear();
            messageView.EndUpdate();            
            _mutex.ReleaseMutex();
        }

        public bool ShowIDHexadecimal
        {
            get { return showIDHexadezimalToolStripMenuItem.Checked; }
            set 
            { 
                showIDHexadezimalToolStripMenuItem.Checked = value;

                if (showIDHexadezimalToolStripMenuItem.Checked)
                    messageView.Columns[1].Text = "ID (Hex)";
                else
                    messageView.Columns[1].Text = "ID";
            }
        }

        public CANLoggerViewMode ViewMode
        {
            set
            {
                switch (value)
                {
                    case CANLoggerViewMode.Counting:
                        traceToolStripMenuItem.Checked = false;
                        countingToolStripMenuItem.Checked = true;
                        break;
                    case CANLoggerViewMode.Logging:
                        countingToolStripMenuItem.Checked = false;
                        traceToolStripMenuItem.Checked = true;
                        break;
                }
            }
            get
            {
                if (traceToolStripMenuItem.Checked)
                    return CANLoggerViewMode.Logging;
                else
                    return CANLoggerViewMode.Counting;
            }
        }

        public void Start()
        {
            _updateTimer.Start();
        }

        public void Stop()
        {
            _updateTimer.Stop();
        }

        public void Clear()
        {
            _mutex.WaitOne();
            _id2TimeStamp.Clear();
            _itemsToUpdate.Clear();
            messageView.Items.Clear();
            _messages.Clear();
            _idCounter.Clear();
            _id2Item.Clear();
            _mutex.ReleaseMutex();
        }

        public void AddMessage(ulong timeStamp, int id, int dlc, byte[] data)
        {
            if (id == 0)
                return;

            if (timeStamp != 0)
            {
                string key = timeStamp.ToString() + id.ToString();

                if (_messages.Contains(key))
                    return;

                _messages.Add(key, id);
            }

            ulong count = 0;

            if (_idCounter.ContainsKey(id))
            {
                count = (ulong)_idCounter[id];
                count++;
                _idCounter[id] = count;
            }
            else
            {
                count++;
                _idCounter[id] = count;
            }

            string[] dataArray = new string[8];

            dataArray[0] = (timeStamp / 100000.0).ToString();
            dataArray[1] = id.ToString();
            dataArray[2] = dlc.ToString();
            dataArray[3] = GetHexData(data);
            dataArray[4] = GetDezData(data);
            dataArray[5] = GetASCIIData(data);
            dataArray[6] = count.ToString();

            int periode = 0;

            if (_id2TimeStamp.Contains(id))
            {
                ulong lastTimeStamp = (ulong) _id2TimeStamp[id];
                periode = (int) (timeStamp - lastTimeStamp) / 100;
            }
            
            _id2TimeStamp[id] = timeStamp;

            dataArray[7] = periode.ToString();
            
            _mutex.WaitOne();
            _itemsToUpdate.Add(dataArray);
            _mutex.ReleaseMutex();            
        }


        [System.ComponentModel.Browsable(false)]
        public int HexDataWidth
        {
            get { return messageView.Columns[3].Width; }
            set { messageView.Columns[3].Width = value; }
        }

        [System.ComponentModel.Browsable(false)]
        public int DezDataWidth
        {
            get { return messageView.Columns[4].Width; }
            set { messageView.Columns[4].Width = value; }
        }

        [System.ComponentModel.Browsable(false)]
        public int AsciiDataWidth
        {
            get { return messageView.Columns[5].Width; }
            set { messageView.Columns[5].Width = value; }
        }

        [System.ComponentModel.Browsable(false)]
        public Hashtable HighLight
        {
            get { return _highlighting; }
            set { _highlighting = value; }
        }
        
        public int NoOfMessages
        {
            get { return _noOfMsg; }
            set { _noOfMsg = value; }
        }

        private string GetHexData(byte[] data)
        {
            StringBuilder strdata = new StringBuilder();

            for (int i = 0; i < data.Length; ++i)
            {
                strdata.Append(data[i].ToString("X2"));
                strdata.Append(" ");
            }

            return strdata.ToString();
        }

        private string GetDezData(byte[] data)
        {
            StringBuilder strdata = new StringBuilder();

            for (int i = 0; i < data.Length; ++i)
            {
                strdata.Append(data[i].ToString("D3"));
                strdata.Append(" ");
            }
            return strdata.ToString();
        }

        private string GetASCIIData(byte[] data)
        {
            StringBuilder strdata = new StringBuilder();

            for (int i = 0; i < data.Length; ++i)
            {
                strdata.Append(((char)data[i]));
                strdata.Append(" ");
            }
            return strdata.ToString();
        }

        private void showHexDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showHexDataToolStripMenuItem.Checked = !showHexDataToolStripMenuItem.Checked;
            if (showHexDataToolStripMenuItem.Checked)
                messageView.Columns[3].Width = 200;
            else
                messageView.Columns[3].Width = 0;
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (messageView.Columns[3].Width > 0)
                showHexDataToolStripMenuItem.Checked = true;
            else
                showHexDataToolStripMenuItem.Checked = false;

            if (messageView.Columns[4].Width > 0)
                showDezDataToolStripMenuItem.Checked = true;
            else
                showDezDataToolStripMenuItem.Checked = false;

            if (messageView.Columns[5].Width > 0)
                showAToolStripMenuItem.Checked = true;
            else
                showAToolStripMenuItem.Checked = false;
        }

        private void showDezDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showDezDataToolStripMenuItem.Checked = !showDezDataToolStripMenuItem.Checked;
            if (showDezDataToolStripMenuItem.Checked)
                messageView.Columns[4].Width = 300;
            else
                messageView.Columns[4].Width = 0;
        }

        private void showAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showAToolStripMenuItem.Checked = !showAToolStripMenuItem.Checked;
            if (showAToolStripMenuItem.Checked)
                messageView.Columns[5].Width = 150;
            else
                messageView.Columns[5].Width = 0;
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CANLoggerViewDlg dlg = new CANLoggerViewDlg(_highlighting, _noOfMsg);
            
            if (dlg.ShowDialog() == DialogResult.OK)
                _noOfMsg = dlg.NoOfMessages;
        }

        private void messageView_DoubleClick(object sender, EventArgs e)
        {
            propertiesToolStripMenuItem_Click(sender, e);
        }

        private void oKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = "";

            foreach (ListViewItem item in messageView.Items)
            {
                text += item.SubItems[0].Text + "\t";
                text += item.SubItems[1].Text + "\t";
                text += item.SubItems[2].Text + "\t";

                if(messageView.Columns[3].Width > 0)
                    text += item.SubItems[3].Text + "\t";

                if (messageView.Columns[4].Width > 0)
                    text += item.SubItems[4].Text + "\t";

                if (messageView.Columns[5].Width > 0)
                    text += item.SubItems[5].Text + "\t";

                text += item.SubItems[6].Text + "\t";
                text += item.SubItems[7].Text + "\t";
                text += "\r\n";

            }

            if (text != "")
                Clipboard.SetText(text);
        }

        private void copySelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = "";

            foreach (ListViewItem item in messageView.SelectedItems)
            {
                text += item.SubItems[0].Text + "\t";
                text += item.SubItems[1].Text + "\t";
                text += item.SubItems[2].Text + "\t";

                if (messageView.Columns[3].Width > 0)
                    text += item.SubItems[3].Text + "\t";

                if (messageView.Columns[4].Width > 0)
                    text += item.SubItems[4].Text + "\t";

                if (messageView.Columns[5].Width > 0)
                    text += item.SubItems[5].Text + "\t";


                text += item.SubItems[6].Text + "\t";
                text += item.SubItems[7].Text + "\t";

                text += "\r\n";
            }

            if (text != "")
                Clipboard.SetText(text);
        }

        private void freezeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            freezeToolStripMenuItem.Checked = !freezeToolStripMenuItem.Checked;
            _freeze = freezeToolStripMenuItem.Checked;
        }

        private void traceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearNoCount();

            traceToolStripMenuItem.Checked = !traceToolStripMenuItem.Checked;
            countingToolStripMenuItem.Checked = !traceToolStripMenuItem.Checked;
        }

        private void ClearNoCount()
        {
            _mutex.WaitOne();
            _id2TimeStamp.Clear();
            _itemsToUpdate.Clear();
            _id2Item.Clear();
            messageView.Items.Clear();
            _messages.Clear();
            _mutex.ReleaseMutex();
        }

        private void countingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearNoCount();

            countingToolStripMenuItem.Checked = !countingToolStripMenuItem.Checked;
            traceToolStripMenuItem.Checked = !countingToolStripMenuItem.Checked;

        }

        private void showIDHexadezimalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showIDHexadezimalToolStripMenuItem.Checked = !showIDHexadezimalToolStripMenuItem.Checked;
            if (showIDHexadezimalToolStripMenuItem.Checked)
                messageView.Columns[1].Text = "ID (Hex)";
            else
                messageView.Columns[1].Text = "ID";
        }
    }
}

