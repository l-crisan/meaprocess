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
    public partial class ReadPropPortDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlSigList;
        private ImageList _images = new ImageList();

        public ReadPropPortDlg(Document doc, XmlElement xmlSigList)
        {
            InitializeComponent();
            _doc = doc;
            _xmlSigList = xmlSigList;
            Icon = Document.AppIcon;
            _images.Images.Add(Images.Property);
            _images.Images.Add(Images.Signal);
            propertyView.ImageList = _images;
            LoadProperties();
            LoadSignals();
        }


        private SignalDataType GetSignalType(string type)
        {
             return SignalDataType.LREAL;
        }


        private void UpdateSignals()
        {
            for(int i = 0; i < signals.Rows.Count; ++i)
            {            
                DataGridViewRow row  = signals.Rows[i];
            
                string propName = row.Cells[0].Value.ToString();
                
                if(GetPropertyByName(propName, _doc) == null)
                {
                   signals.Rows.Remove(row);
                   --i;
                }                
            }
        }        


        private void SaveSignals()
        {
            foreach(DataGridViewRow row in signals.Rows)
            {
                XmlElement xmlSignal = (XmlElement) row.Tag;
                
                if(xmlSignal  == null)
                    xmlSignal = _doc.CreateXmlObject(_xmlSigList, "Mp.Sig", "Mp.Sig.Prop");

                XmlHelper.SetParam(xmlSignal, "name", "string", row.Cells[2].Value.ToString());
                XmlHelper.SetParam(xmlSignal, "propName", "string", row.Cells[0].Value.ToString());

                SignalDataType type =  GetSignalType(row.Cells[1].Value.ToString());

                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", 100);

                XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", (int)SignalDataType.LREAL);

                XmlHelper.SetParam(xmlSignal, "unit", "string", row.Cells[5].Value.ToString());
                XmlHelper.SetParam(xmlSignal, "comment", "string", row.Cells[6].Value.ToString());
                XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", 0);

                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", Convert.ToDouble(row.Cells[3].Value));
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", Convert.ToDouble(row.Cells[4].Value));
            }

            RemoveUnsusedSignals(_xmlSigList, _doc);
        }


        private void LoadSignals()
        {
            signals.Rows.Clear();

            foreach(XmlElement xmlSignal in _xmlSigList.ChildNodes)
            {
                string propName = XmlHelper.GetParam(xmlSignal, "propName");
                
                XmlElement xmlProp = GetPropertyByName(propName, _doc);
                
                if( xmlProp == null)
                    continue;

                int index = signals.Rows.Add();
                DataGridViewRow row  = signals.Rows[index];
                row.Tag = xmlSignal;
                row.Cells[0].Value = propName;
                row.Cells[1].Value = XmlHelper.GetParam(xmlProp, "type");
                row.Cells[2].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Cells[3].Value = XmlHelper.GetParamDouble(xmlSignal, "physMin");
                row.Cells[4].Value = XmlHelper.GetParamDouble(xmlSignal, "physMax");
                row.Cells[5].Value = XmlHelper.GetParam(xmlSignal, "unit");
                row.Cells[6].Value = XmlHelper.GetParam(xmlSignal, "comment");
            }
        }


        private void OnOKClick(object sender, EventArgs e)
        {
            SaveSignals();
            _doc.Modified = true;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }


        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }


        private static XmlElement GetPropertyByName(string name, Document doc)
        {
            XmlElement xmlDocument = doc.XmlDoc.DocumentElement;

            XmlElement xmlProperties = XmlHelper.GetChildByType(xmlDocument, "Mp.Properties");

            if (xmlProperties == null)
                return null;

            foreach (XmlElement xmlProperty in xmlProperties.ChildNodes)
            {
                if(XmlHelper.GetParam(xmlProperty, "name") == name)
                    return xmlProperty;
            }        
            return null;
        }


        public static void RemoveUnsusedSignals(XmlElement xmlSigList, Document doc)
        {
            for (int i = 0; i < xmlSigList.ChildNodes.Count; ++i)
            {
                XmlElement xmlSignal = (XmlElement) xmlSigList.ChildNodes[i];


                string propName = XmlHelper.GetParam(xmlSignal, "propName");

                if (GetPropertyByName(propName, doc) == null)
                {
                    doc.RemoveXmlObject(xmlSignal);
                    --i;
                }
            }
        }


        private void LoadProperties()
        {
            XmlElement xmlDocument = _doc.XmlDoc.DocumentElement;

            XmlElement xmlProperties = XmlHelper.GetChildByType(xmlDocument, "Mp.Properties");
            propertyView.Nodes.Clear();

            if (xmlProperties == null)
            {
                xmlProperties = _doc.CreateXmlObject(xmlDocument, "Mp.Properties", "");
            }
            else
            {                
                foreach (XmlElement xmlProperty in xmlProperties.ChildNodes)
                {
                     TreeNode propNode = new TreeNode(XmlHelper.GetParam(xmlProperty, "name"));
                     propNode.Tag = xmlProperty;
                     propertyView.Nodes.Add( propNode);
                }
            }
        }


        private void OnEditProperties(object sender, EventArgs e)
        {
            SchemePropertyDlg dlg = new SchemePropertyDlg(_doc);
            
            if(dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            LoadProperties();
            UpdateSignals();
        }


        private void OnPropertyViewItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode node = e.Item as TreeNode;

            if (node == null)
                return;

            DoDragDrop(node.Tag, DragDropEffects.All);
        }


        private void OnSignalsDragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("System.Xml.XmlElement"))
                return;

            XmlElement xmlProp = (XmlElement) e.Data.GetData("System.Xml.XmlElement");
            int index = signals.Rows.Add();
            DataGridViewRow row = signals.Rows[index];
            row.Cells[0].Value = XmlHelper.GetParam(xmlProp,"name");
            row.Cells[1].Value = "LREAL";
            row.Cells[2].Value = "";
            row.Cells[3].Value = (double)0;
            row.Cells[4].Value = (double)1;
            row.Cells[5].Value = "";
            row.Cells[6].Value = "";
            row.Tag =null;            
        }


        private void OnSignalsDragOver(object sender, DragEventArgs e)
        {            
            if (e.Data.GetDataPresent("System.Xml.XmlElement"))
                    e.Effect = DragDropEffects.Move;
        }


        private void OnRemoveSignalClick(object sender, EventArgs e)
        {
            if(signals.SelectedCells.Count ==0)
                return;

            if(signals.SelectedCells[0].RowIndex  < 0)
                return;

            signals.Rows.RemoveAt(signals.SelectedCells[0].RowIndex);
        }


        private void OnSignalsCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            errorProvider.Clear();
            if( e.ColumnIndex  == 3 || e.ColumnIndex  == 4)
            {
                try
                {
                    Convert.ToDouble(e.FormattedValue);
                }
                catch(Exception ex)
                {
                    errorProvider.SetError(signals, ex.Message);
                    e.Cancel = true;
                }
            }
        }


        private void OnSignalsRowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            errorProvider.Clear();

            if(e.RowIndex < 0)
                return;

            DataGridViewRow row = signals.Rows[e.RowIndex];
            double min = Convert.ToDouble(row.Cells[3].Value);
            double max = Convert.ToDouble(row.Cells[4].Value);

            if( min < max)
                return;

            errorProvider.SetError(signals, StringResource.SigMinMaxErr);
            e.Cancel = true;
        }
    }
}
