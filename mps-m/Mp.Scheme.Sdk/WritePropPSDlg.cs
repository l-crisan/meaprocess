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
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Mp.Utils;

namespace Mp.Scheme.Sdk
{
    public partial class WritePropPSDlg : Form
    {
        private XmlElement _xmlPS;
        private Document _doc;
        private SignalInputView _signals;
        private XmlElement _xmlSigList;
         
        public WritePropPSDlg(XmlElement xmlPS, Document doc, XmlElement xmlSigList)
        {
            InitializeComponent();

            _xmlPS = xmlPS;
            _doc = doc;
            _xmlSigList = xmlSigList;
            _signals = new SignalInputView(_doc, _xmlSigList);
            _signals.Dock = DockStyle.Fill;
            this.Icon = Document.AppIcon;

            signalSplitContainer.Panel1.Controls.Add(_signals);


            psName.Text = XmlHelper.GetParam(_xmlPS,"name");

            LoadProperties();
            LoadMapping();
        }

        private DataGridViewRow GetRowByProperty(string name)
        {
            foreach(DataGridViewRow row in propertyMap.Rows)
            {
                string pname = row.Cells[1].Value.ToString();

                if( pname == name)
                    return row;
            }
            return null;
        }

        private void UpdateProperties()
        {
            XmlElement xmlDocument = _doc.XmlDoc.DocumentElement;

            XmlElement xmlProperties = XmlHelper.GetChildByType(xmlDocument, "Mp.Properties");

            if (xmlProperties == null)
                return;
                
            //Add new rows
            foreach (XmlElement xmlProperty in xmlProperties.ChildNodes)
            {
                string name = XmlHelper.GetParam(xmlProperty, "name");
                string type = XmlHelper.GetParam(xmlProperty, "type");
                DataGridViewRow row = GetRowByProperty(name);
                
                if( row == null)
                {
                    int index = propertyMap.Rows.Add();
                    row =  propertyMap.Rows[index];
                    row.Cells[0].Value = "";
                    row.Cells[1].Value = name;                
                }

                row.Cells[2].Value = type;
            }

            //Remove old rows
            for( int i = 0; i < propertyMap.Rows.Count; ++i)
            {
                DataGridViewRow row = propertyMap.Rows[i];
                string name = row.Cells[1].Value.ToString();
                string type = row.Cells[2].Value.ToString();

                XmlElement xmlProp = GetPropertyByName(name, xmlProperties);

                if(xmlProp == null)
                {
                    propertyMap.Rows.Remove(row);
                    --i;
                }       
            }
        }

        private static XmlElement GetPropertyByName(string name,  XmlElement xmlProperties)
        {
            foreach (XmlElement xmlProperty in xmlProperties.ChildNodes)
            {
                string pname = XmlHelper.GetParam(xmlProperty, "name");

                if( pname == name)
                    return xmlProperty;
            }
            return null;
        }

        private void LoadProperties()
        {
            XmlElement xmlDocument = _doc.XmlDoc.DocumentElement;

            XmlElement xmlProperties = XmlHelper.GetChildByType(xmlDocument, "Mp.Properties");

            if (xmlProperties == null)
            {
                xmlProperties = _doc.CreateXmlObject(xmlDocument, "Mp.Properties", "");
            }
            else
            {
                foreach (XmlElement xmlProperty in xmlProperties.ChildNodes)
                {
                    int index = propertyMap.Rows.Add();
                    DataGridViewRow row = propertyMap.Rows[index];

                    row.Cells[0].Value = "";
                    row.Cells[1].Value = XmlHelper.GetParam(xmlProperty, "name");
                    string type = XmlHelper.GetParam(xmlProperty, "type");

                    if (type == "STRING ARRAY")
                        type = "ENUMERATION";

                    row.Cells[2].Value = type;
                }
            }
        }
        
        private void OnPropertiesEditClick(object sender, EventArgs e)
        {
            SchemePropertyDlg dlg = new SchemePropertyDlg(_doc);
            
            if(dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            
            UpdateProperties();
        }

        private void OnPropertyMapDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.ListViewItem"))
            {
                ListViewItem item = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");

                if (item.Tag is XmlElement)
                    e.Effect = DragDropEffects.Move;
            }
        }

        private void OnPropertyMapDragDrop(object sender, DragEventArgs e)
        {
            ListViewItem item = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");
            Point p = propertyMap.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo info = propertyMap.HitTest(p.X, p.Y);
            if (info.RowIndex != -1)
            {
                DataGridViewRow row = propertyMap.Rows[info.RowIndex];
                XmlElement xmlSignal = (XmlElement)item.Tag;
                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Tag = xmlSignal;
            }
        }

        private void OnRemoveSignal(object sender, EventArgs e)
        {
            if(propertyMap.SelectedCells.Count == 0)
                return;

            int index = propertyMap.SelectedCells[0].RowIndex;

            if( index < 0)
                return;

            DataGridViewRow row = propertyMap.Rows[index];

            row.Tag = null;
            row.Cells[0].Value = "";
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void LoadMapping()
        {
            string mapping = XmlHelper.GetParam(_xmlPS, "propMap");
            string[] array = mapping.Split('#');
            
            foreach( string map in array)
            {
                if( map == "")
                    continue;

                string[] ar = map.Split(';');
                uint sigId = Convert.ToUInt32(ar[0]);
                string propName = ar[1];
                DataGridViewRow  row = GetRowByProperty(propName);
                
                if( row == null)
                    continue;

                XmlElement xmlSignal = GetXmlSignalById(sigId);
                
                if( xmlSignal == null)
                    continue;

                row.Tag = xmlSignal;
                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
            }
        }

        private XmlElement GetXmlSignalById(uint id)
        {
            foreach(XmlElement xmlElement in _xmlSigList.ChildNodes)
            {
                XmlElement xmlSignal = _doc.GetSignal(xmlElement);
                
                if(XmlHelper.GetObjectID(xmlSignal) == id)
                    return xmlSignal;
            }

            return  null;
        }

        private void OnOKClick(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach(DataGridViewRow row in propertyMap.Rows)
            {
                if(row.Tag == null)
                    continue;

                XmlElement xmlSignal = (XmlElement) row.Tag;
                uint id = XmlHelper.GetObjectID(xmlSignal);
                string map = id.ToString() + ";" + row.Cells[1].Value.ToString() + "#";
                sb.Append(map);
            }

            string mapping = sb.ToString();
            mapping = mapping.TrimEnd('#');
            
            if(mapping == null)
                mapping = "";

            XmlHelper.SetParam(_xmlPS, "propMap", "string", mapping);
            XmlHelper.SetParam(_xmlPS,"name","string",psName.Text);
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
