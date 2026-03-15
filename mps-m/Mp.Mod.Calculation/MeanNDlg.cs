//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2010-2016  Laurentiu-Gheorghe Crisan
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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Calculation
{
    public partial class MeanNDlg : Form
    {
        private SignalInputView _signals;
        private XmlElement _xmlPS;
        private Document _doc;
        private XmlElement _xmlInSignalList;
        private ImageList _imgList = new ImageList();

        public MeanNDlg(XmlElement xmlPS, Document doc, XmlElement xmlInSignalList)
        {
            _xmlPS = xmlPS;
            _doc = doc;
            _xmlInSignalList = xmlInSignalList;
            _signals = new SignalInputView(doc, xmlInSignalList);
            _signals.AlowMultiDrag = true;

            _signals.TabIndex = 2;
            _signals.Dock = DockStyle.Fill;
            InitializeComponent();
            this.Icon = Document.AppIcon;
            _imgList.Images.Add(Resource.Signal);
            signals.SmallImageList = _imgList;
            signals.LargeImageList = _imgList;

            splitContainer1.Panel1.Controls.Add(_signals);
            name.Text = XmlHelper.GetParam(_xmlPS, "name");
            string signalList = XmlHelper.GetParam(_xmlPS, "signals");

            if (signalList != "")
            {
                string[] array = signalList.Split(';');

                foreach (string idStr in array)
                {
                    uint id = Convert.ToUInt32(idStr);
                    XmlElement xmlSignal = _doc.GetXmlObjectById(id);
                    AddSignal(xmlSignal);
                }
            }

            FormStateHandler.Restore(this, "Mp.Calculation.MeanNDlg");
        }

        private void MeanNDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormStateHandler.Save(this, "Mp.Calculation.MeanNDlg");
        }

        private void channels_DragOver(object sender, DragEventArgs e)
        {
            List<string> formats = new List<string>(e.Data.GetFormats());
            if (formats.Contains("System.Windows.Forms.ListViewItem"))
            {
                ListViewItem item = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");

                if (item.Tag is XmlElement)
                    e.Effect = DragDropEffects.Move;
            }
            else if (formats.Contains("System.Windows.Forms.ListView+SelectedListViewItemCollection"))
            {
                ListView.SelectedListViewItemCollection items = (ListView.SelectedListViewItemCollection)e.Data.GetData(formats[0]);

                foreach (ListViewItem item in items)
                {
                    if (item.Tag is XmlElement)
                        e.Effect = DragDropEffects.Move;
                }
            }
        }

        private void channels_DragDrop(object sender, DragEventArgs e)
        {
            List<string> formats = new List<string>(e.Data.GetFormats());

            if (formats.Contains("System.Windows.Forms.ListViewItem"))
            {
                System.Windows.Forms.ListViewItem item = (System.Windows.Forms.ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");

                XmlElement xmlSignal = item.Tag as XmlElement;

                if (xmlSignal == null)
                    return;

                AddSignal(xmlSignal);
            }
            else if(formats.Contains("System.Windows.Forms.ListView+SelectedListViewItemCollection"))
            {

                System.Windows.Forms.ListView.SelectedListViewItemCollection items = (System.Windows.Forms.ListView.SelectedListViewItemCollection) e.Data.GetData(formats[0]);

                foreach (ListViewItem item in items)
                {
                    XmlElement xmlSignal = item.Tag as XmlElement;

                    if (xmlSignal == null)
                        continue;

                    AddSignal(xmlSignal);
                }
            }
        }

        private void AddSignal(XmlElement xmlSignal)
        {
            string[] items = new string[1];
            items[0] = XmlHelper.GetParam(xmlSignal, "name");

            ListViewItem signalItem = new ListViewItem(items, 0);
            signalItem.Tag = xmlSignal;
            signals.Items.Add(signalItem);
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (signals.Items.Count < 2)
            {
                errorProvider.SetError(signals, StringResource.Min2SignalsErr);
                return;
            }

            StringBuilder sb = new StringBuilder();

            foreach(ListViewItem item in signals.Items)
            {
                XmlElement xmlSignal = (XmlElement) item.Tag;
                uint id = (uint) XmlHelper.GetObjectID(xmlSignal);
                sb.Append(id.ToString());
                sb.Append(";");
            }
            string signalList = sb.ToString().TrimEnd(';');

            XmlHelper.SetParam(_xmlPS, "signals", "string", signalList);
             
            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);
            _doc.Modified = true;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void removeSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ListViewItem> items = new List<ListViewItem>();

            foreach (ListViewItem item in signals.SelectedItems)
                items.Add(item);
            
            foreach( ListViewItem item in items)
                signals.Items.Remove(item);
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1420);
        }
    }
}
