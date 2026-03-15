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
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Globalization;
using Mp.Utils;
using Mp.Scheme.Sdk;

namespace Mp.Mod.HTML.View
{
    public partial class TextPSDlg : Form
    {
        private XmlElement _xmlSignalList;
        private XmlElement _xmlPS;
        private Document _doc;
        private NumberFormatInfo _info = new NumberFormatInfo();            


        public TextPSDlg(XmlElement xmlSignalList, XmlElement xmlPS, Document doc)
        {
            _info.NumberDecimalSeparator = ".";

            _xmlSignalList = xmlSignalList;
            _xmlPS = xmlPS;
            _doc = doc;
            InitializeComponent();
            this.Icon = Document.AppIcon;

            ImageList imgList = new ImageList();
            imgList.Images.Add(Resource.Signal);
            signals.SmallImageList = imgList;

            psName.Text = XmlHelper.GetParam(xmlPS, "name");

            int index = events.Rows.Add();
            DataGridViewRow row = events.Rows[index];
            row.Cells[0].Value = "On start";
            
            row.Cells[1].Value = "<>";
            row.Cells[1].Tag = "";
            row.Cells[1].ReadOnly = true;
            row.Cells[1].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);

            row.Cells[2].Value = "0";
            row.Cells[2].ReadOnly = true;
            row.Cells[2].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
            
            row.Cells[3].Value = "...";


            LoadSignals();            
        }

        private void LoadSignals()
        {
            if(_xmlSignalList == null)
                return;

            int i = 0;
            foreach (XmlElement xmlElement in _xmlSignalList.ChildNodes)
            {
                XmlElement xmlSignal = xmlElement;

                if (XmlHelper.GetObjectID(xmlSignal) == 0)
                    xmlSignal = _doc.GetXmlObjectById(Convert.ToUInt32(xmlSignal.InnerText));

                string[] items = new string[1];
                items[0] = i.ToString() + ". " + XmlHelper.GetParam(xmlSignal,"name");

                ListViewItem item = new ListViewItem(items,0);
                item.Tag = xmlSignal;
                signals.Items.Add(item);
                ++i;
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
            XmlHelper.SetParam(_xmlPS, "name", "string", psName.Text);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.NewLineOnAttributes = false;
            settings.OmitXmlDeclaration = false;
            settings.CloseOutput = true;
            
            MemoryStream mm = new MemoryStream();
            XmlWriter xmlWriter = XmlWriter.Create(mm, settings);

            xmlWriter.WriteStartElement("evDoc");
            
            foreach (DataGridViewRow row in events.Rows)
            {
                xmlWriter.WriteStartElement("event");                
                
                string text = (string)row.Cells[1].Tag;

                XmlElement xmlSignal = (XmlElement)row.Tag;
                
                xmlWriter.WriteStartElement("signal");
                if (xmlSignal == null)
                {
                    xmlWriter.WriteString("0");       
                }
                else
                {
                    uint id = XmlHelper.GetObjectID(xmlSignal);
                    xmlWriter.WriteString(id.ToString());       
                }
                xmlWriter.WriteEndElement();
                
                xmlWriter.WriteStartElement("operation");
                xmlWriter.WriteString(GetOperation((string)row.Cells[1].Value).ToString());
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("limit");
                xmlWriter.WriteString(Convert.ToDouble(row.Cells[2].Value).ToString(_info));
                xmlWriter.WriteEndElement();
                
                xmlWriter.WriteStartElement("text");
                xmlWriter.WriteString(text);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
            xmlWriter.WriteEndElement();
            xmlWriter.Flush();            

            mm.Seek(0,SeekOrigin.Begin);
            StreamReader sr = new StreamReader(mm);

            XmlHelper.SetParam(_xmlPS, "events", "string", sr.ReadToEnd());
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
                row.Cells[1].Tag = "";
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
            TextEditorDlg dlg = new TextEditorDlg( (string)row.Cells[1].Tag, _doc);
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            row.Cells[1].Tag = dlg.TheText;
        }

        private void EventPSDlg_Load(object sender, EventArgs e)
        {

            string data = XmlHelper.GetParam(_xmlPS, "events");

            XmlDocument eventsDoc = new XmlDocument();
            
            if (data == "")
                return;

            eventsDoc.LoadXml(data);

            for (int i = 0; i < eventsDoc.DocumentElement.ChildNodes.Count; ++i)
            {
                XmlElement xmlEvent = eventsDoc.DocumentElement.ChildNodes[i] as XmlElement;
                
                if (xmlEvent == null)
                    continue;

                uint sigId = Convert.ToUInt32(xmlEvent["signal"].InnerText);
                string text = xmlEvent["text"].InnerText;

                if (sigId != 0)
                {                
                    int index = events.Rows.Add();
                    DataGridViewRow row = events.Rows[index];
                
                    row.Cells[1].Value = GetOperation(Convert.ToInt32(xmlEvent["operation"].InnerText));
                    row.Cells[2].Value = Convert.ToDouble(xmlEvent["limit"].InnerText,_info);
                    row.Cells[3].Value = "...";
                    XmlElement xmlSignal = _doc.GetXmlObjectById(sigId);
                    row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                    row.Tag = xmlSignal;                    
                    row.Cells[1].Tag = text;
                }
                else
                {
                    DataGridViewRow row = events.Rows[0];
                    row.Cells[1].Tag = text;
                }                                
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
            Document.ShowHelp(this, 1210);
        }

        private void EventPSDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }
    }
}
