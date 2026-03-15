using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Atesion.Utils;
using Mp.DataFile;
using Mp.Scheme.Sdk;
using System.Globalization;

namespace Mp.Scheme.Win
{
    public partial class XYChartPSDlg : Form
    {
        private class CurveInfo
        {
            public CurveInfo(StorageGroup stg, Mp.DataFile.Signal sig, string basePath)
            {
                StgInfo = stg;
                Signal = sig;
                BasePath = basePath;
            }

            public StorageGroup StgInfo;
            public Mp.DataFile.Signal Signal;
            public string BasePath;
        }
        

        private XmlElement _xmlPS;
        private Document _doc;
        private static List<string> _loadedFiles = new List<string>();

        public XYChartPSDlg(XmlElement xmlPS, Document doc)
        {
            _xmlPS = xmlPS;
            _doc = doc;
            InitializeComponent();
            name.Text = XmlHelper.GetParam(xmlPS, "name");
            FormStateHandler.Restore(this, Document.RegistryKey + "XYChartPSDlg");
            //Load the data
            foreach (XmlElement xmlElement in _xmlPS.ChildNodes)
            {
                string nameAttr = xmlElement.GetAttribute("name");
                
                if (nameAttr == null)
                    continue;

                if (nameAttr != "refCurve")
                    continue;

                int index = curves.Rows.Add();
                DataGridViewRow row = curves.Rows[index];
                row.Cells[0].Value = (index + 1);
                row.Cells[1].Value = xmlElement.GetAttribute("sigName");
                string strcolor = xmlElement.GetAttribute("color");

                string[] array = strcolor.Split(',');

                if (array.Length == 3)
                {
                    Color lineColor = Color.FromArgb(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]), Convert.ToInt32(array[2]));
                    row.Cells[1].Style.BackColor = lineColor;
                    row.Cells[2].Style.BackColor = lineColor;
                }

                row.Cells[2].Value = "...";

                row.Tag = xmlElement;
            }


            foreach (string file in _loadedFiles)
                LoadMMFFile(file);
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (!SaveCurves())
                return;

            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);
            _doc.Modify = true;
            
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void open_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "*.mmf|*.mmf";
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            LoadMMFFile(dlg.FileName);
            _loadedFiles.Add(dlg.FileName);
        }

        private void LoadMMFFile(string file)
        {
            MMFMetaFileReader mmfReader = new MMFMetaFileReader();
            mmfReader.Read(file);
            
            string fileName = Path.GetFileNameWithoutExtension(mmfReader.MetaFile);
            TreeNode rootNode = fileView.Nodes.Add(fileName, fileName, 0,0);
            rootNode.Tag = file;

            foreach (StorageGroup stg in mmfReader.StorageGroups)
            {
                string grpName = stg.Source + "(" + stg.SampleRate.ToString() + " Hz)";
                TreeNode grpNode = rootNode.Nodes.Add(grpName, grpName, 1, 1);
                grpNode.Tag = stg;

                foreach (Mp.DataFile.Signal signal in stg.Signals)
                {
                    TreeNode signalNode = grpNode.Nodes.Add(signal.Name, signal.Name, 2,2);
                    signalNode.Tag = signal;
                }
            }

            rootNode.ExpandAll();
        }

        private void removeFile_Click(object sender, EventArgs e)
        {
            if (fileView.SelectedNode == null)
                return;

            if (!(fileView.SelectedNode.Tag is string))
                return;

            string file = (string)fileView.SelectedNode.Tag;
            fileView.Nodes.Remove(fileView.SelectedNode);
            file = file.ToUpper();
            for(int i =0 ; i < _loadedFiles.Count; ++i)
            {
                if(_loadedFiles[i].ToUpper() == file)
                {
                    _loadedFiles.RemoveAt(i);
                    --i;
                }
           }
        }

        private void addCurve_Click(object sender, EventArgs e)
        {
            int index = curves.Rows.Add();
            curves.Rows[index].Cells[0].Value = (index + 1);
        }

        private void removeCurve_Click(object sender, EventArgs e)
        {
            if (curves.SelectedRows.Count == 0)
                return;

            if (MessageBox.Show(StringResource.RemoveRefCurve, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();
            for (int i = 0; i < curves.SelectedRows.Count; ++i)
                rowsToRemove.Add(curves.SelectedRows[i]);

            for (int i = 0; i < rowsToRemove.Count; ++i)
                curves.Rows.Remove(rowsToRemove[i]);

            int index = 1;
            foreach (DataGridViewRow row in curves.Rows)
            {
                row.Cells[0].Value = index;
                ++index;
            }
        }

        private void fileView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode node = (TreeNode)e.Item;
            if (node.Tag is Mp.DataFile.Signal)
            {
                TreeNode fileNode = node.Parent.Parent;
                string basePath = Path.GetDirectoryName((string)fileNode.Tag);

                CurveInfo cvi = new CurveInfo((StorageGroup)node.Parent.Tag, (Mp.DataFile.Signal)node.Tag, basePath);
                fileView.DoDragDrop(cvi, DragDropEffects.Move);
            }
        }

        private void curves_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("Mp.Scheme.Win.XYChartPSDlg+CurveInfo"))
                return;

            e.Effect = DragDropEffects.Move;
        }

        private void curves_DragDrop(object sender, DragEventArgs e)
        {
            CurveInfo cvi = (CurveInfo)e.Data.GetData("Mp.Scheme.Win.XYChartPSDlg+CurveInfo");

            int index = curves.Rows.Add();
            DataGridViewRow row = curves.Rows[index];

            row.Cells[0].Value = (index + 1);
            row.Cells[1].Tag = cvi;
            row.Cells[1].Style.BackColor = Color.Red;
            row.Cells[1].Value = cvi.Signal.Name;
            row.Cells[2].Style.BackColor = Color.Red;
            row.Cells[2].Value = "...";
        }

        private bool SaveCurves()
        {
            errorProvider.Clear();
            NumberFormatInfo info = new NumberFormatInfo();
            info.NumberDecimalSeparator = ".";
            
            XmlAttribute xmlSigName;
            XmlAttribute xmlSigColor;
            foreach (DataGridViewRow row in curves.Rows)
            {
                if (row.Tag != null)
                {
                    XmlElement xmlElement = (XmlElement)row.Tag;
                    
                    //Update the color color
                    xmlSigColor = xmlElement.Attributes["color"];
                    if (xmlSigColor == null)
                    {
                        xmlSigColor = _xmlPS.OwnerDocument.CreateAttribute("color");
                        xmlElement.Attributes.Append(xmlSigColor);
                    }

                    xmlSigColor.Value = row.Cells[1].Style.BackColor.R.ToString() + "," +
                                        row.Cells[1].Style.BackColor.G.ToString() + "," +
                                        row.Cells[1].Style.BackColor.B.ToString();


                    //Update the sig name
                    xmlSigName = xmlElement.Attributes["sigName"];
                    xmlSigName.Value = row.Cells[1].Value.ToString();   
                    continue;
                }

                CurveInfo cvi = (CurveInfo)row.Cells[1].Tag;

                long records = cvi.StgInfo.GetNoOfRecords(cvi.BasePath);
                XmlElement xmlRefCurve = XmlHelper.CreateElement(_xmlPS,"string","refCurve","");
                row.Tag = xmlRefCurve;

                //SigName
                xmlSigName = _xmlPS.OwnerDocument.CreateAttribute("sigName");
                xmlSigName.Value = cvi.Signal.Name;
                xmlRefCurve.Attributes.Append(xmlSigName);

                //Unit
                XmlAttribute xmlSigUnit = _xmlPS.OwnerDocument.CreateAttribute("sigUnit");
                xmlSigUnit.Value = cvi.Signal.Unit;
                xmlRefCurve.Attributes.Append(xmlSigUnit);

                //Samples
                XmlAttribute xmlSamples = _xmlPS.OwnerDocument.CreateAttribute("samples");
                xmlSamples.Value = records.ToString();
                xmlRefCurve.Attributes.Append(xmlSamples);

                //Rate
                XmlAttribute xmlRate = _xmlPS.OwnerDocument.CreateAttribute("rate");
                xmlRate.Value = cvi.StgInfo.SampleRate.ToString(info);
                xmlRefCurve.Attributes.Append(xmlRate);

                //Color
                xmlSigColor = _xmlPS.OwnerDocument.CreateAttribute("color");
                xmlRefCurve.Attributes.Append(xmlSigColor);
                xmlSigColor.Value = row.Cells[1].Style.BackColor.R.ToString() + "," +
                                    row.Cells[1].Style.BackColor.G.ToString() + "," +
                                    row.Cells[1].Style.BackColor.B.ToString();

                int recordSize = cvi.StgInfo.RecordSize;

                Mp.DataFile.Signal sig = cvi.Signal;
                double data = 0.0;
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);

                using (FileStream dataFile = new FileStream(Path.Combine(cvi.BasePath, cvi.StgInfo.DataFile), FileMode.Open))
                {
                    BinaryReader br = new BinaryReader(dataFile);

                    for (long i = 0; i < records; ++i)
                    {
                        br.BaseStream.Seek(i * recordSize + cvi.Signal.OffsetInRecord, SeekOrigin.Begin);
                        switch (cvi.Signal.DataType)
                        {
                            case "LREAL":
                                data = sig.Factor * br.ReadDouble() + sig.Offset;
                                break;

                            case "REAL":
                                data = sig.Factor * br.ReadSingle() + sig.Offset;
                                break;

                            case "USINT":
                            case "BYTE":
                                data = sig.Factor * br.ReadByte() + sig.Offset;
                                break;
                            case "SINT":
                                data = sig.Factor * br.ReadSByte() + sig.Offset;
                                break;
                            case "UINT":
                            case "WORD":
                                data = sig.Factor * br.ReadUInt16() + sig.Offset;
                                break;
                            case "INT":
                                data = sig.Factor * br.ReadInt16() + sig.Offset;
                                break;
                            case "UDINT":
                            case "DWORD":
                                data = sig.Factor * br.ReadUInt32() + sig.Offset;
                                break;
                            case "DINT":
                                data = sig.Factor * br.ReadInt32() + sig.Offset;
                                break;
                            case "ULINT":
                            case "LWORD":
                                data = sig.Factor * br.ReadUInt64() + sig.Offset;
                                break;
                            case "LINT":
                                data = sig.Factor * br.ReadInt64() + sig.Offset;
                                break;
                            case "BOOL":
                                data = (double)br.ReadByte();
                                break;

                        }
                        bw.Write(data);
                    }
                }
                xmlRefCurve.InnerText = Convert.ToBase64String(ms.GetBuffer());
            }

            //Load the data
            for (int i = 0; i < _xmlPS.ChildNodes.Count; ++i )
            {
                XmlElement xmlElement = (XmlElement) _xmlPS.ChildNodes[i];
                string nameAttr = xmlElement.GetAttribute("name");

                if (nameAttr == null)
                    continue;

                if (nameAttr != "refCurve")
                    continue;

                bool found = false;
                foreach (DataGridViewRow row in curves.Rows)
                {
                    if (row.Tag == xmlElement)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    _xmlPS.RemoveChild(xmlElement);
                    --i;
                }
            }            

            return true;
        }

        private void curves_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 2)
                return;

            if (e.RowIndex == -1)
                return;

            ColorDialog dlg = new ColorDialog();
            DataGridViewRow row = curves.Rows[e.RowIndex];
            DataGridViewButtonCell cell = (DataGridViewButtonCell) row.Cells[e.ColumnIndex];
            dlg.Color = cell.Style.BackColor;
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            cell.Style.BackColor = dlg.Color;
            row.Cells[1].Style.BackColor = dlg.Color;
        }

        private void XYChartPSDlg_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormStateHandler.Save(this, Document.RegistryKey + "XYChartPSDlg");
        }
    }
}
