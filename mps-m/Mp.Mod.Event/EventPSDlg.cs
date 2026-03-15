//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2015  Laurentiu-Gheorghe Crisan
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Mp.Utils;
using Mp.Scheme.Sdk;

namespace Mp.Mod.Event
{
    public partial class EventPSDlg : Form
    {
        private XmlElement _xmlSignalList;
        private XmlElement _xmlPS;
        private Document _doc;
        private ImageList _imgList = new ImageList();

        public EventPSDlg(XmlElement xmlSignalList, XmlElement xmlPS, Document doc)
        {
            _xmlSignalList = xmlSignalList;
            _xmlPS = xmlPS;
            _doc = doc;
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            _imgList.Images.Add(Images.Signal);
            signals.SmallImageList = _imgList;
            signals.LargeImageList = _imgList;

            psName.Text = XmlHelper.GetParam(xmlPS, "name");
            LoadSignals();            
        }

        private void LoadSignals()
        {
            if(_xmlSignalList == null)
                return;

            foreach (XmlElement xmlElement in _xmlSignalList.ChildNodes)
            {
                XmlElement xmlSignal = xmlElement;

                if (XmlHelper.GetObjectID(xmlSignal) == 0)
                    xmlSignal = _doc.GetXmlObjectById(Convert.ToUInt32(xmlSignal.InnerText));

                string[] items = new string[1];
                items[0] = XmlHelper.GetParam(xmlSignal,"name");

                ListViewItem item = new ListViewItem(items,0);
                item.Tag = xmlSignal;
                signals.Items.Add(item);
            }
        }

        private string GetOperation(int op)
        {
            switch (op)
            {
                case 0:
                    return "<>";
                case 1:
                    return "=";
                case 2:
                    return "<";
                case 3:
                    return "<=";
                case 4:
                    return ">";
                case 5:
                    return ">=";
            }
            return "<>";
        }

        private int GetOperation(string op)
        {
            switch (op)
            {
                case "<>":
                    return 0;
                
                case "=":
                    return 1;
                
                case "<":
                    return 2;
                
                case "<=":
                    return 3;
                
                case ">":
                    return 4;

                 case ">=":
                    return 5;
            }

            return 0;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            XmlHelper.SetParam(_xmlPS, "name","string", psName.Text);

            XmlElement xmlEvents = XmlHelper.GetChildByType(_xmlPS, "Mp.Event.RtEvents");

            if (xmlEvents != null)
                _doc.RemoveXmlObject(xmlEvents);

            //Create the new devices.

            xmlEvents = _doc.CreateXmlObject(_xmlPS, "Mp.Event.RtEvents", "");

            foreach (DataGridViewRow row in events.Rows)
            {
                XmlElement xmlEvent = _doc.CreateXmlObject(xmlEvents, "Mp.Event.RtEvent", "");
                XmlElement xmlSignal = (XmlElement) row.Tag;
                EventDescription evd = (EventDescription)row.Cells[1].Tag;

                uint id = XmlHelper.GetObjectID(xmlSignal);
                XmlHelper.SetParamNumber(xmlEvent, "signal", "uint32_t", (int)id);
                XmlHelper.SetParamNumber(xmlEvent, "operation","uint8_t", GetOperation((string) row.Cells[1].Value));
                XmlHelper.SetParamDouble(xmlEvent, "limit", "double", Convert.ToDouble(row.Cells[2].Value));

                row.Cells[2].Value = XmlHelper.GetParamDouble(xmlEvent, "limit");

                XmlHelper.SetParam(xmlEvent, "message", "string", evd.Message);
                XmlHelper.SetParamNumber(xmlEvent, "outputTarget", "uint8_t", evd.OutputTarget);
                XmlHelper.SetParamNumber(xmlEvent, "priority", "uint8_t", evd.Priority);

                XmlHelper.SetParam(xmlEvent, "audioFile", "string", evd.AudioFile);

                if (evd.AudioFile != null && evd.AudioFile != "")
                {
                    using (FileStream fs = new FileStream(evd.AudioFile, FileMode.Open, FileAccess.Read))
                    {
                        BinaryReader br = new BinaryReader(fs);
                        byte[] buffer = br.ReadBytes((int)fs.Length);
                        string base64 = Convert.ToBase64String(buffer);

                        XmlHelper.SetParam(xmlEvent, "audioData", "string", base64);
                        fs.Close();
                    }
                }
                else
                {
                    XmlHelper.SetParam(xmlEvent, "audioData", "string", "");
                }

                XmlHelper.SetParam(xmlEvent, "command", "string", evd.Command);
                XmlHelper.SetParam(xmlEvent, "commandParam", "string", evd.CommandParam);

            }

            DialogResult = DialogResult.OK;
            _doc.Modified = true;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void signals_ItemDrag(object sender, ItemDragEventArgs e)
        {            
            DoDragDrop(signals.SelectedItems, DragDropEffects.Move);
        }

        private void events_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.ListView+SelectedListViewItemCollection"))
                e.Effect = DragDropEffects.Move;
        }

        private void events_DragDrop(object sender, DragEventArgs e)
        {
            ListView.SelectedListViewItemCollection selected = (ListView.SelectedListViewItemCollection) e.Data.GetData("System.Windows.Forms.ListView+SelectedListViewItemCollection");

            foreach (ListViewItem item in selected)
            {
                XmlElement xmlSignal = (XmlElement) item.Tag;

                int index = events.Rows.Add();
                DataGridViewRow row = events.Rows[index];
                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Cells[1].Value = "<>";
                row.Cells[2].Value = "0";
                row.Cells[3].Value = "...";
                row.Cells[1].Tag = new EventDescription();
                row.Tag = xmlSignal;
            }

        }

        private void events_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 3)
                return;
            
            if(e.RowIndex == -1)
                return;

            DataGridViewRow row = events.Rows[e.RowIndex];
            EventDescription descr = (EventDescription) row.Cells[1].Tag;

            ConfigEventDlg dlg = new ConfigEventDlg(descr,_doc);
            dlg.ShowDialog();
        }

        private void EventPSDlg_Load(object sender, EventArgs e)
        {
            XmlElement xmlEvents = XmlHelper.GetChildByType(_xmlPS, "Mp.Event.RtEvents");

            if( xmlEvents == null)
                return;

            foreach (XmlElement xmlEvent in xmlEvents.ChildNodes)
            {
                int index = events.Rows.Add();
                DataGridViewRow row = events.Rows[index];
                uint sigId = (uint)XmlHelper.GetParamNumber(xmlEvent, "signal");
                row.Cells[1].Value = GetOperation((int)XmlHelper.GetParamNumber(xmlEvent, "operation"));
                row.Cells[2].Value = XmlHelper.GetParamDouble(xmlEvent, "limit");
                row.Cells[3].Value = "...";
                XmlElement  xmlSignal = _doc.GetXmlObjectById(sigId);
                EventDescription ev = new EventDescription();

                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Tag = xmlSignal;

                ev.Message = XmlHelper.GetParam(xmlEvent, "message");
                ev.OutputTarget = (int) XmlHelper.GetParamNumber(xmlEvent, "outputTarget");
                ev.Priority = (int) XmlHelper.GetParamNumber(xmlEvent, "priority");
                ev.AudioFile = XmlHelper.GetParam(xmlEvent, "audioFile");
                ev.Command = XmlHelper.GetParam(xmlEvent, "command");
                ev.CommandParam = XmlHelper.GetParam(xmlEvent, "commandParam");
                row.Cells[1].Tag = ev;

            }
        }

        private void removeEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if( events.SelectedCells.Count == 0)
                return;

            DataGridViewRow row  = events.Rows[events.SelectedCells[0].RowIndex];
            events.Rows.Remove(row);
        }

        private void events_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            errorProvider.Clear();

            if (e.ColumnIndex != 2)
                return;

            try
            {
                Convert.ToDouble(e.FormattedValue);
            }
            catch(Exception ex)
            {
                errorProvider.SetError(events, ex.Message);
                e.Cancel = true;
            }
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1100);
        }

        private void EventPSDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }
    }
}
