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
using Mp.Utils;

namespace Mp.Scheme.Sdk
{
    public partial class SignalInputView : UserControl
    {
        private bool _alowMultiDrag = false;

        public SignalInputView(Document doc, XmlElement xmlSignalList)
        {
            InitializeComponent();
            imageList.Images.Add(Images.Signal);
            LoadSignals(doc, xmlSignalList);
        }

        public bool AlowMultiDrag
        {
            get { return _alowMultiDrag; }
            set { _alowMultiDrag = value; }
        }


        private void LoadSignals(Document doc, XmlElement xmlSignalList)
        {
            if (xmlSignalList == null)
                return;

            XmlElement xmlSignal;

            foreach (XmlElement xmlElement in xmlSignalList.ChildNodes)
            {
                if (XmlHelper.GetObjectID(xmlElement) == 0)
                    xmlSignal = doc.GetXmlObjectById(Convert.ToUInt32(xmlElement.InnerText));
                else
                    xmlSignal = xmlElement;

                string[] data = new string[4];
                data[0] = XmlHelper.GetParam(xmlSignal, "name");
                data[1] = XmlHelper.GetParam(xmlSignal, "samplerate") + " (Hz)";
                data[2] = XmlHelper.GetParam(xmlSignal, "physMin");
                data[3] = XmlHelper.GetParam(xmlSignal, "physMax");

                ListViewItem item = new ListViewItem(data, 0);
                item.Tag = xmlSignal;
                signals.Items.Add(item);
            }
        }


        private void OnSignalsItemDrag(object sender, ItemDragEventArgs e)
        {
            if (_alowMultiDrag)
            {
                if( signals.SelectedItems.Count != 0)
                    DoDragDrop(signals.SelectedItems, DragDropEffects.Move);
            }
            else
            {
                if (signals.SelectedItems.Count != 1)
                    return;

                DoDragDrop(signals.SelectedItems[0], DragDropEffects.Move);
            }
        }
    }
}
