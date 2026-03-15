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
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Mp.Utils;

namespace Mp.Scheme.Sdk
{
    public partial class SchemePropertyDlg : Form
    {
        private XmlElement _xmlProperties;
        private Document _doc;
        private List<string> _selProp = new List<string>();
        
        public SchemePropertyDlg(Document doc)
        {
            _doc = doc;
            
            XmlElement xmlDocument = _doc.XmlDoc.DocumentElement;

            InitializeComponent();
            this.Icon = Document.AppIcon;

            _xmlProperties = XmlHelper.GetChildByType(xmlDocument, "Mp.Properties");

            int emptyRowIdx = 0;

            if (_xmlProperties == null)
            {
                _xmlProperties = _doc.CreateXmlObject(xmlDocument, "Mp.Properties", "");
            }
            else
            {
                foreach (XmlElement xmlProperty in _xmlProperties.ChildNodes)
                {
                    
                    int index = properties.Rows.Add();
                    DataGridViewRow row = properties.Rows[index];
                    row.Cells[0].Value = XmlHelper.GetParam(xmlProperty, "name");
                    row.Cells[1].Value = XmlHelper.GetParam(xmlProperty, "value");
                    string type = XmlHelper.GetParam(xmlProperty, "type");
                    
                    if (type == "STRING ARRAY")
                        type = "ENUMERATION";

                    row.Cells[2].Value = type;
                    row.Cells[2].Tag = XmlHelper.GetParam(xmlProperty, "typeValue");
                    row.Cells[3].Value = "...";
                    row.Cells[4].Value = XmlHelper.GetParamNumber(xmlProperty, "mandatory") > 0;
                    row.Cells[5].Value = XmlHelper.GetParamNumber(xmlProperty, "readOnly") > 0;
                    row.Cells[6].Value = XmlHelper.GetParamNumber(xmlProperty, "hidden") == 0;                    
                    emptyRowIdx++;
                }
            }

            InitEmptyRow(emptyRowIdx);
        }


        private void OnOKClick(object sender, EventArgs e)
        {
            //Remove the properties.
            for(int i = 0; i < _xmlProperties.ChildNodes.Count; ++i)
            {
                XmlElement xml = _xmlProperties.ChildNodes[i] as XmlElement;

                if( xml == null)
                    continue;

                _doc.RemoveXmlObject(xml);

                --i;
            }
                
            //Save the new properties
            foreach (DataGridViewRow row in properties.Rows)
            {
                string name = (string) row.Cells[0].Value;
                
                if (name == null || name == "")
                    continue;

                XmlElement xmlProperty = _doc.CreateXmlObject(_xmlProperties, "Mp.Property", "");
                XmlHelper.SetParam(xmlProperty, "name", "string", name);
                XmlHelper.SetParam(xmlProperty, "value", "string", (string) row.Cells[1].Value);
                XmlHelper.SetParam(xmlProperty, "type", "string", (string)row.Cells[2].Value);
                XmlHelper.SetParam(xmlProperty, "typeValue", "string", (string)row.Cells[2].Tag);
                
                
                if((bool) row.Cells[4].Value)
                    XmlHelper.SetParamNumber(xmlProperty, "mandatory", "bool", 1);
                else
                    XmlHelper.SetParamNumber(xmlProperty, "mandatory", "bool", 0);

                if ((bool)row.Cells[5].Value)
                    XmlHelper.SetParamNumber(xmlProperty, "readOnly", "bool", 1);
                else
                    XmlHelper.SetParamNumber(xmlProperty, "readOnly", "bool", 0);


                if ((bool)row.Cells[6].Value)
                    XmlHelper.SetParamNumber(xmlProperty, "hidden", "bool", 0);
                else
                    XmlHelper.SetParamNumber(xmlProperty, "hidden", "bool", 1);
            }


            _selProp.Clear();

            if( properties.SelectedRows.Count != 0)
            {                
                foreach (DataGridViewRow row in properties.SelectedRows)
                {
                    string name = (string)row.Cells[0].Value;

                    if (name != null && name != "")
                        _selProp.Add("$(" + name + ")");
                }
            }
            DialogResult = DialogResult.OK;
            _doc.Modified = true;
            Close();
        }


        private void OnCancelClick(object sender, EventArgs e)
        {
            _selProp.Clear();
            DialogResult = DialogResult.Cancel;
            Close();
        }


        private void OnPropertyRowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            InitEmptyRow(e.RowIndex);         
        }


        private void InitEmptyRow(int rowIndex)
        {
            DataGridViewRow emptyRow = properties.Rows[rowIndex];
            DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)emptyRow.Cells[2];
            emptyRow.Cells[4].Value = false;
            emptyRow.Cells[5].Value = false;
            emptyRow.Cells[3].Value = "...";
            emptyRow.Cells[2].Value = cell.Items[0];
            emptyRow.Cells[6].Value = true;
        }


        public List<string> SelectedProperties
        {
            get { return _selProp; }
        }


        private void OnRemoveClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in properties.SelectedRows)
            {
                if(!row.IsNewRow)
                    properties.Rows.Remove(row);
            }
        }


        private void OnPropertyCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 3)
                return;

            if( e.RowIndex == -1)
                return;

            DataGridViewRow row = properties.Rows[e.RowIndex];

            string s = row.Cells[2].Value.ToString();
            if (s == "ENUMERATION")
            {
                string text = (string)row.Cells[2].Tag;
                if (text == null)
                    text = "";

                EditStringArrayPropType dlg = new EditStringArrayPropType(text);

                if (dlg.ShowDialog() == DialogResult.OK)
                    row.Cells[2].Tag = dlg.EnumText;
            }
        }


        private void OnHelpClick(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 270);
        }


        private void OnHelpRequested(object sender, HelpEventArgs hlpevent)
        {
            OnHelpClick(null, null);
        }
    }
}
