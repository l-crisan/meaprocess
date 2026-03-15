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
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Mp.Utils;

namespace Mp.Runtime.App
{
    internal partial class PropertyDlg : Form
    {
        private XmlElement _xmlDoc;
        private XmlElement _xmlProperties;

        public PropertyDlg(XmlElement xmlDoc)
        {
            _xmlDoc = xmlDoc;
            InitializeComponent();

            this.Icon = Runtime.Sdk.RuntimeEngine.AppIcon;
            XmlElement xmlGUI = XmlHelper.GetChildByType(xmlDoc, "GUI");

            properties.Columns[3].Visible = XmlHelper.GetParamNumber(xmlGUI, "mandatoryFlagVisible") != 0;
            properties.Columns[2].Visible = XmlHelper.GetParamNumber(xmlGUI, "editPropBt") != 0;

            _xmlProperties = XmlHelper.GetChildByType(xmlDoc, "Mp.Properties");

            if (_xmlProperties == null)
                return;

            foreach (XmlElement xmlProperty in _xmlProperties.ChildNodes)
            {
                string type = XmlHelper.GetParam(xmlProperty, "type");
                
                if (type == "STRING ARRAY")
                    type = "ENUMERATION";

                if (XmlHelper.GetParamNumber(xmlProperty, "hidden") == 1)
                    continue;

                DataGridViewRow row = null;
                int index = properties.Rows.Add();
                row = properties.Rows[index];

                row.Cells[0].Value = XmlHelper.GetParam(xmlProperty, "name");
                row.Cells[1].Value = XmlHelper.GetParam(xmlProperty, "value");
                row.Cells[1].Tag = XmlHelper.GetParam(xmlProperty, "typeValue");
                row.Cells[2].Value = "...";
                row.Cells[3].Value = XmlHelper.GetParamNumber(xmlProperty, "mandatory") > 0;

                if (type == "ENUMERATION")
                {
                    DataGridViewComboBoxCell cbo = new DataGridViewComboBoxCell();

                    string data = row.Cells[1].Tag.ToString();
                    string[] array = data.Split('\n');

                    List<string> items = new List<string>();

                    for (int i = 0; i < array.Length; ++i)
                        items.Add(array[i].TrimEnd('\r'));

                    foreach (string s in items)
                        cbo.Items.Add(s);

                    try
                    {
                        if (items.Contains((string) row.Cells[1].Value))
                            cbo.Value = row.Cells[1].Value;
                        else if (items.Count > 0)
                            cbo.Value = items[0];                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    cbo.ReadOnly = false;
                    row.Cells[1] = cbo;
                    row.Cells[1].Tag = XmlHelper.GetParam(xmlProperty, "typeValue");
                }
                
                if(XmlHelper.GetParamNumber(xmlProperty, "readOnly") > 0)
                {
                    row.Cells[2].ReadOnly = true;
                    row.Cells[1].ReadOnly = true;
                }

                if (row.Cells[1].ReadOnly)
                    row.Cells[1].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);

                row.Tag = xmlProperty;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {                        
            errorProvider.Clear();

            if (_xmlProperties == null)
            {
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            foreach (DataGridViewRow row in properties.Rows)
            {
                XmlElement xmlProperty = (XmlElement)row.Tag;
                string value;

                if (row.Cells[1].Value != null)
                    value = row.Cells[1].Value.ToString();
                else
                    value = "";

                bool mandatory = (bool)row.Cells[3].Value;
                
                if (value == "" && mandatory)
                {
                    errorProvider.SetError(properties, StringResource.ManPropErr);
                    return;
                }
                XmlHelper.SetParam(xmlProperty, "value", "string", value);
            }

            DialogResult = DialogResult.OK;

            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void properties_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            
        }

        private void properties_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 2 || _xmlProperties == null || e.RowIndex == -1)
                return;

            DataGridViewRow row = properties.Rows[e.RowIndex];

            XmlElement xmlProperty = row.Tag as XmlElement;

            if (xmlProperty == null)
                return;

            string type = XmlHelper.GetParam(xmlProperty, "type");
            
            if (XmlHelper.GetParamNumber(xmlProperty, "readOnly") > 0)
                return;

            switch (type)
            {
                case "FILE":
                {
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.CheckFileExists = false;
                    dlg.CheckPathExists = false;
                    dlg.Filter = "*.*|*.*|*.mmf|*.mmf|*.tdm|*.tdm";
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        row.Cells[1].Value = dlg.FileName;
                    }
                }
                break;
                case "FOLDER":
                {
                    FolderBrowserDialog dlg = new FolderBrowserDialog();
                    dlg.ShowNewFolderButton = true;
                    if (dlg.ShowDialog() == DialogResult.OK)
                        row.Cells[1].Value = dlg.SelectedPath;
                }
                break;

                case "STRING":
                {

                    EditStringDlg dlg = new EditStringDlg();
                    dlg.TextValue = (string) row.Cells[1].Value;

                    if (dlg.ShowDialog() == DialogResult.OK)
                        row.Cells[1].Value = dlg.TextValue;
                }
                break;

                case "STRING ARRAY":
                case "ENUMERATION":
                {
                    string data = row.Cells[1].Tag.ToString();
                    EditStringArrayDlg dlg = new EditStringArrayDlg(row.Cells[1].Value.ToString(),data);
                    
                    if (dlg.ShowDialog() == DialogResult.OK)
                        row.Cells[1].Value = dlg.Value;

                }
                break;
            }
        }
    }
}
