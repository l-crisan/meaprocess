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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Mp.Utils;
using Mp.Drv.DataFile;
using Mp.Scheme.Sdk;

namespace Mp.Mod.Storage
{
    public partial class ReadDataDlg : Form
    {
        private XmlElement _xmlPs;
        private XmlElement _xmlSignalList;
        private Document   _doc;
        private bool       _byteOrderLittle = true;

        public ReadDataDlg(XmlElement xmlPs, XmlElement xmlSignalList, Document doc)
        {
            _xmlPs = xmlPs;
            _xmlSignalList = xmlSignalList;
            _doc = doc;
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            name.Text = XmlHelper.GetParam(_xmlPs, "name");
            file.Text = XmlHelper.GetParam(_xmlPs, "file");
            runtimeFile.Text = XmlHelper.GetParam(_xmlPs, "rfile");
            loopBack.Checked = XmlHelper.GetParamNumber(_xmlPs,"loopBack") > 0;
        }

        private uint GetMetaFileHash()
        {
            uint hash = 5381;

            try
            {
                FileStream metaFile = File.Open(file.Text, FileMode.Open);
                byte[] data = new byte[metaFile.Length];
                metaFile.Read(data, 0, (int)metaFile.Length);

                foreach (byte b in data)
                    hash = ((hash << 5) + hash) + b;

                metaFile.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return hash;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (file.Text == null || file.Text == "")
            {
                errorProvider.SetError(open, StringResource.DataFileExpected);
                return;
            }

            if (runtimeFile.Text == null || runtimeFile.Text == "")
            {
                errorProvider.SetError(openFileRuntime, StringResource.DataFileExpected);
                return;
            }
            
            DialogResult = DialogResult.OK;

            LoadSignals(file.Text);

            XmlHelper.SetParam(_xmlPs, "name", "string", name.Text);
            XmlHelper.SetParam(_xmlPs, "file", "string", file.Text);
            XmlHelper.SetParam(_xmlPs, "rfile", "string", runtimeFile.Text);


            XmlHelper.SetParamNumber(_xmlPs, "hash", "uint32_t", GetMetaFileHash());

            if(_byteOrderLittle)
                XmlHelper.SetParamNumber(_xmlPs, "byteOrder", "uint8_t",1);
            else
                XmlHelper.SetParamNumber(_xmlPs, "byteOrder", "uint8_t", 0);


           if(loopBack.Checked)
                XmlHelper.SetParamNumber(_xmlPs, "loopBack", "uint8_t", 1);
            else
                XmlHelper.SetParamNumber(_xmlPs, "loopBack", "uint8_t", 0);

            _doc.Modified = true;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void open_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "*.mmf|*.mmf";
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            file.Text = dlg.FileName;

            if (runtimeFile.Text == null || runtimeFile.Text == "")
                runtimeFile.Text = file.Text;            
        }


        private bool LoadSignals(string file)
        {
            try
            {
                //Remove the signals
                for (int i = 0; i < _xmlSignalList.ChildNodes.Count; ++i)
                {
                    XmlElement xmlSignal = _xmlSignalList.ChildNodes[i] as XmlElement;
                    
                    if (xmlSignal == null)
                        continue;

                    uint sourceNo = (uint) XmlHelper.GetParamNumber(xmlSignal,"sourceNumber");
                    _doc.UnregisterSource(sourceNo);
                    _doc.RemoveXmlObject(xmlSignal);
                    --i;
                }

                //Remove the source to file mapping
                for (int i = 0; i < _xmlPs.ChildNodes.Count; ++i)
                {
                    XmlElement xmlSrcMap =  _xmlPs.ChildNodes[i] as XmlElement;
                    
                    if(xmlSrcMap == null)
                        continue;

                    if (!xmlSrcMap.HasAttribute("name"))
                        continue;

                    _xmlPs.RemoveChild(xmlSrcMap);
                    --i;
                }      

                //Create the signal list
                MMFMetaFileReader reader = new MMFMetaFileReader();
                reader.Read(file);
                
                _byteOrderLittle = reader.ByteOrder == MMFMetaFileReader.ByteOrderType.LittleEndian;

                foreach (StorageGroup group in reader.StorageGroups)
                {
                    uint srcId = _doc.RegisterSource(group.Source, 0, runtimeFile.Text + group.SourceId.ToString());
                    string srcMap = srcId.ToString() + "#" + group.TargetDataFile;

                    XmlHelper.CreateElement(_xmlPs, "string", "sourceMap", srcMap);

                    foreach (Signal signal in group.Signals)
                    {
                        XmlElement xmlSignal = _doc.CreateXmlObject(_xmlSignalList, "Mp.Sig", "Mp.Sig");
                                      
                        XmlHelper.SetParam(xmlSignal, "name", "string", signal.Name);
                        XmlHelper.SetParam(xmlSignal, "unit", "string", signal.Unit);
                        XmlHelper.SetParam(xmlSignal, "comment", "string", signal.Comment);
                        XmlHelper.SetParam(xmlSignal, "cat", "string", signal.Cat);
                        XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", signal.Min);
                        XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", signal.Max);
                        XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", GetDataType(signal));
                        XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", srcId);
                        XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", group.SampleRate);
                        XmlHelper.SetParamNumber(xmlSignal, "objSize", "uint32_t", signal.ObjectSize);

                        string parameters = "";

                        foreach (DictionaryEntry entry in signal.Parameters)
                        {
                            parameters += entry.Key.ToString() + "=";
                            parameters += entry.Value.ToString() + ";";

                        }

                        XmlHelper.SetParam(xmlSignal, "parameters", "string", parameters);


                        if (signal.Factor != 0)
                        {
                            XmlElement xmlScaling = _doc.CreateXmlObject(xmlSignal, "Mp.Scaling", "Mp.Scaling.FactorOffset");
                            XmlHelper.SetParamDouble(xmlScaling, "factor", "double", signal.Factor);
                            XmlHelper.SetParamDouble(xmlScaling, "offset", "double", signal.Offset);
                        }
                    }
                }                
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }

        private int GetDataType(Signal signal)
        {
            switch (signal.DataType)
            {
                case "USINT":
                case "BYTE":
                    return (int) SignalDataType.USINT;
                
                case "UINT":
                case "WORD":
                    return (int)SignalDataType.UINT;

                case "UDINT":
                case "DWORD":
                    return (int)SignalDataType.UDINT;

                case "ULINT":
                case "LWORD":
                    return (int)SignalDataType.ULINT;

                case "SINT":
                    return (int) SignalDataType.SINT;

                case "INT":
                    return (int) SignalDataType.INT;

                case "DINT":
                    return (int) SignalDataType.DINT;

                case "LINT":
                    return (int) SignalDataType.LINT;

                case "BOOL":
                    return (int) SignalDataType.BOOL;

                case "REAL":
                    return (int) SignalDataType.REAL;

                case "LREAL":
                    return (int) SignalDataType.LREAL;
                
                case "object":
                    return (int)SignalDataType.OBJECT;
            }

            return 0;
        }

        private void openFileRuntime_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "*.mmf|*.mmf";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            runtimeFile.Text = dlg.FileName;
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 610);
        }

        private void ReadDataDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }
    }
}
