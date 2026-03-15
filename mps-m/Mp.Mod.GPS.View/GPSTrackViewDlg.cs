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
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.GPS.View
{
    public partial class GPSTrackViewDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlPS;
        private XmlElement _xmlSignalList;
        
        public GPSTrackViewDlg(Document doc, XmlElement xmlPS,  XmlElement xmlSignalList)
        {
            _doc = doc;
            _xmlPS = xmlPS;
            _xmlSignalList = xmlSignalList;

            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            psName.Text = XmlHelper.GetParam(xmlPS, "name");

            ImageList imgList = new ImageList();
            imgList.Images.Add(Resource.Signal);
            signalView.SmallImageList = imgList;

            InitGPSChannels();
            InitSignals();
            LoadData();
        }

        private void LoadData()
        {
            foreach (XmlElement xmlElement in _xmlPS.ChildNodes)
            {
                if (!xmlElement.HasAttribute("name"))
                    continue;

                string name = xmlElement.GetAttribute("name");

                if (name != "sigMaping")
                    continue;

                string[] mapping = xmlElement.InnerText.Split('/');

                uint id = Convert.ToUInt32(mapping[0]);
                int index = Convert.ToInt32(mapping[1]);
                XmlElement xmlSignal = _doc.GetXmlObjectById(id);
                DataGridViewRow row = gpsChannels.Rows[index];

                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Tag = xmlSignal;
            }
        }

        private void InitSignals()
        {
            if (_xmlSignalList == null)
                return;

            string[] array = new string[3];

            XmlElement xmlSignal;
            foreach (XmlElement xmlNode in _xmlSignalList.ChildNodes)
            {
                if (XmlHelper.GetObjectID(xmlNode) == 0)
                    xmlSignal = _doc.GetXmlObjectById(Convert.ToUInt32(xmlNode.InnerText));
                else
                    xmlSignal = xmlNode;

                array[0] = XmlHelper.GetParam(xmlSignal, "name");

                string type = xmlSignal.GetAttribute("subType");
                if (type == "SIGNAL_GPS")
                    array[1] = "GPS";
                else
                    array[1] = "Unknown";

                array[2] = XmlHelper.GetParam(xmlSignal, "samplerate") + " Hz";
                ListViewItem item = new ListViewItem(array, 0);
                item.Tag = xmlSignal;
                signalView.Items.Add(item);
            }
        }

        private void InitGPSChannels()
        {
            int index = gpsChannels.Rows.Add();
            DataGridViewRow row = gpsChannels.Rows[index];
            row.Cells[1].Value = StringResource.Latitude;

            index = gpsChannels.Rows.Add();
            row = gpsChannels.Rows[index];
            row.Cells[1].Value = StringResource.Longitude;

            index = gpsChannels.Rows.Add();
            row = gpsChannels.Rows[index];
            row.Cells[1].Value = StringResource.Altitude;

            index = gpsChannels.Rows.Add();
            row = gpsChannels.Rows[index];
            row.Cells[1].Value = StringResource.Status;

            index = gpsChannels.Rows.Add();
            row = gpsChannels.Rows[index];
            row.Cells[1].Value = StringResource.Speed;

            index = gpsChannels.Rows.Add();
            row = gpsChannels.Rows[index];
            row.Cells[1].Value = StringResource.TrackAngle;

            index = gpsChannels.Rows.Add();
            row = gpsChannels.Rows[index];
            row.Cells[1].Value = StringResource.Day;

            index = gpsChannels.Rows.Add();
            row = gpsChannels.Rows[index];
            row.Cells[1].Value = StringResource.Month;

            index = gpsChannels.Rows.Add();
            row = gpsChannels.Rows[index];
            row.Cells[1].Value = StringResource.Year;

            index = gpsChannels.Rows.Add();
            row = gpsChannels.Rows[index];
            row.Cells[1].Value = StringResource.Hour;

            index = gpsChannels.Rows.Add();
            row = gpsChannels.Rows[index];
            row.Cells[1].Value = StringResource.Minute;

            index = gpsChannels.Rows.Add();
            row = gpsChannels.Rows[index];
            row.Cells[1].Value = StringResource.Second;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            //Check 
            if (gpsChannels.Rows[0].Tag == null || gpsChannels.Rows[1].Tag == null)
            {
                errorProvider.SetError(gpsChannels, StringResource.LatLonErr);
                return;
            }

            bool firstSignal = true;
            uint lastSrcID = 0;
            double lastRate = 0;

           //Save the mapping
            foreach (DataGridViewRow row in gpsChannels.Rows)
            {
                if (row.Tag == null)
                    continue;

                XmlElement xmlSignal = (XmlElement)row.Tag;

                uint srcID = (uint) XmlHelper.GetParamNumber(xmlSignal, "sourceNumber");
                double rate =XmlHelper.GetParamDouble(xmlSignal,"rate");

                if (firstSignal)
                {
                    firstSignal = false;
                    lastRate = rate;
                    lastSrcID = srcID;
                }
                else
                {
                    if (lastRate != rate || lastSrcID != srcID)
                    {
                        errorProvider.SetError(gpsChannels, StringResource.SrcRateErr);
                        return;
                    }
                }
            }

            //Remove the old mapping
            for (int i = 0; i < _xmlPS.ChildNodes.Count; ++i)
            {
                XmlElement xmlElement = (XmlElement)_xmlPS.ChildNodes[i];

                if (!xmlElement.HasAttribute("name"))
                    continue;

                string name = xmlElement.GetAttribute("name");
                
                if (name != "sigMaping")
                    continue;

                _xmlPS.RemoveChild(xmlElement);
                --i;
            }


            //Save the mapping
            foreach (DataGridViewRow row in gpsChannels.Rows)
            {
                if (row.Tag == null)
                    continue;

                XmlElement xmlSignal = (XmlElement)row.Tag;
                uint id = XmlHelper.GetObjectID(xmlSignal);
                XmlHelper.CreateElement(_xmlPS, "string", "sigMaping", id.ToString() + "/" + row.Index.ToString());
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

        private void signalView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(signalView.SelectedItems, DragDropEffects.Move);
        }

        private void gpsChannels_DragOver(object sender, DragEventArgs e)
        {
            Type type = typeof(ListView.SelectedListViewItemCollection);

            string[] dataType = e.Data.GetFormats();
            if (dataType.Length == 0)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            if (type.FullName == dataType[0])
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void gpsChannels_DragDrop(object sender, DragEventArgs e)
        {
            Type type = typeof(ListView.SelectedListViewItemCollection);

            string[] dataType = e.Data.GetFormats();
            if (dataType.Length == 0)
                return;

            if (type.FullName != dataType[0])
                return;

            ListView.SelectedListViewItemCollection items = (ListView.SelectedListViewItemCollection)e.Data.GetData(type);

            Point p = gpsChannels.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo info = gpsChannels.HitTest(p.X, p.Y);
            
            if (info.RowIndex == -1)
                return;

            DataGridViewRow row = gpsChannels.Rows[info.RowIndex];

            ListViewItem item = items[0];
            row.Tag = item.Tag;
            XmlElement xmlSignal = item.Tag as XmlElement;
            row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
        }

        private void remove_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gpsChannels.SelectedRows)
            {
                row.Cells[0].Value = "";
                row.Tag = null;
            }
        }
    }
}
