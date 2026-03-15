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

namespace Mp.Scheme.App
{
    public partial class SignalsDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlDoc;


        public SignalsDlg(Document doc)
        {
            _doc = doc;
            _xmlDoc = _doc.XmlDoc.DocumentElement;
            Icon = Document.AppIcon;

            InitializeComponent();

            XmlElement xmlSignals = XmlHelper.GetChildByType(_xmlDoc, "Mp.Signals");

            foreach (XmlElement xmlSigList in xmlSignals.ChildNodes)
            {
                foreach (XmlElement xmlSignal in xmlSigList.ChildNodes)
                {
                    if (XmlHelper.GetObjectID(xmlSignal) == 0)
                        continue;

                    int i = signalGrid.Rows.Add();
                    DataGridViewRow row = signalGrid.Rows[i];

                    row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                    row.Cells[1].Value = XmlHelper.GetParam(xmlSignal, "unit");
                    row.Cells[2].Value = XmlHelper.GetParam(xmlSignal, "comment");
                    row.Cells[3].Value = XmlHelper.GetParamDouble(xmlSignal, "samplerate").ToString() + " Hz";
                    row.Cells[4].Value = GetDataTypeString((int)XmlHelper.GetParamNumber(xmlSignal, "valueDataType"));
                    row.Cells[5].Value = XmlHelper.GetParamDouble(xmlSignal, "physMin");
                    row.Cells[6].Value = XmlHelper.GetParamDouble(xmlSignal, "physMax");
                    row.Cells[7].Value = GetCat(XmlHelper.GetParam(xmlSignal, "cat"));
                    row.Cells[8].Value = XmlHelper.GetParam(_doc.GetSignalElement(xmlSignal), "name");

                    XmlElement xmlSource = _doc.GetXmlObjectById((uint)XmlHelper.GetParamNumber(xmlSignal, "sourceNumber"));
                    
                    if( xmlSource == null)
                        row.Cells[9].Value = "MeaProcess";
                    else
                        row.Cells[9].Value = XmlHelper.GetParam(xmlSource,"name");

                    row.Tag = xmlSignal;
                }
            }
        }


        private string GetCat(string cat)
        {
            switch (cat)
            {
                case "Mp.Sig.Video":
                    return "Video";
                
                case "Mp.Sig.Audio":
                    return "Audio";
            }

            return "Data";
        }


        private string GetDataTypeString(int type)
        {
            SignalDataType sigType = (SignalDataType)type;
            return sigType.ToString();
        }


        private void OnOKClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in signalGrid.Rows)
            {
                XmlElement xmlSignal = (XmlElement)row.Tag;

                XmlHelper.SetParam(xmlSignal, "name","string", (string) row.Cells[0].Value);
                XmlHelper.SetParam(xmlSignal, "unit","string", (string) row.Cells[1].Value);
                XmlHelper.SetParam(xmlSignal, "comment", "string", (string)row.Cells[2].Value);
            }

            _doc.Modified = true;
            DialogResult = DialogResult.OK;
            Close();
        }


        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }


        private void OnDupplicatedClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in signalGrid.Rows)
            {
                string name = (string)row.Cells[0].Value;

                foreach (DataGridViewRow row1 in signalGrid.Rows)
                {
                    string name2 = (string)row1.Cells[0].Value;
                    
                    if (name == name2 && row != row1)
                    {
                        row.Cells[0].Selected = true;
                        row1.Cells[0].Selected = true;
                    }
                }
            }
        }


        private void OnExportClick(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "*.csv|*.csv|*.xml|*.xml";
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;


            string file = dlg.FileName;

            string ext = Path.GetExtension(file);

            FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write);
            StreamWriter wr = new StreamWriter(fs);
            if (ext == ".csv")
            {
                wr.Write("Name;Unit;Comment;Sample Rate;Data Type;Minimum;Maximum;Source\r\n"); 
            }
            else if (ext == ".xml")
            {
                wr.Write("<?xml version=\"1.0\"?> \r\n");
                wr.Write("<Signals>\r\n");
            }

            foreach (DataGridViewRow row in signalGrid.Rows)
            {
                if (ext == ".csv")
                {                    
                    wr.Write((string)row.Cells[0].Value);
                    wr.Write(";");
                    wr.Write((string)row.Cells[1].Value);
                    wr.Write(";");
                    wr.Write((string)row.Cells[2].Value);
                    wr.Write(";");
                    wr.Write((string)row.Cells[3].Value);
                    wr.Write(";");
                    wr.Write((string)row.Cells[4].Value);
                    wr.Write(";");
                    wr.Write((double)row.Cells[5].Value);
                    wr.Write(";");
                    wr.Write((double)row.Cells[6].Value);
                    wr.Write(";");
                    wr.Write((string)row.Cells[7].Value);
                    wr.Write(";");
                    wr.Write((string)row.Cells[8].Value);
                    wr.Write("\r\n");
                }
                else if (ext == ".xml")
                {
                    wr.Write("<Signal>\r\n");
                    
                    wr.Write("<name>");
                    wr.Write((string)row.Cells[0].Value);
                    wr.Write("</name>\r\n");

                    wr.Write("<unit>");
                    wr.Write((string)row.Cells[1].Value);
                    wr.Write("</unit>\r\n");

                    wr.Write("<comment>");
                    wr.Write((string)row.Cells[2].Value);
                    wr.Write("</comment>\r\n");

                    wr.Write("<sampleRate>");
                    wr.Write((string)row.Cells[3].Value);
                    wr.Write("</sampleRate>\r\n");

                    wr.Write("<dataType>");
                    wr.Write((string)row.Cells[4].Value);
                    wr.Write("</dataType>\r\n");

                    wr.Write("<minimum>");
                    wr.Write((double)row.Cells[5].Value);
                    wr.Write("</minimum>\r\n");

                    wr.Write("<maximum>");
                    wr.Write((double)row.Cells[6].Value);
                    wr.Write("</maximum>\r\n");

                    wr.Write("<category>");
                    wr.Write((string)row.Cells[7].Value);
                    wr.Write("</category>\r\n");

                    wr.Write("<source>");
                    wr.Write((string)row.Cells[8].Value);
                    wr.Write("</source>\r\n");

                    wr.Write("<clockSource>");
                    wr.Write((string)row.Cells[9].Value);
                    wr.Write("</clockSource>\r\n");
                    
                    wr.Write("</Signal>\r\n");
                }
            }

            if (ext == ".xml")
            {
                wr.Write("</Signals>\r\n");
            }
            wr.Flush();
            fs.Flush();
        }


        private void OnSignalGridCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            errorProvider.Clear();

            if (e.ColumnIndex != 0)
                return;
            
            string name = (string) e.FormattedValue;
            e.Cancel = false;

            if (name == null || name == "")
            {
                errorProvider.SetError(signalGrid, "A name for the signal is expected!");
                e.Cancel = true;
            }            
        }
    }
}
