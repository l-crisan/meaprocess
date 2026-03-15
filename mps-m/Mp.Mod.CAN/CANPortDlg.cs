using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

using Mp.Visual.Tree;
using Mp.Visual.Tree.Tree;
using Mp.Visual.Tree.Tree.NodeControls;

using Mp.Scheme.Sdk;
using Mp.Utils;
using Mp.Drv.CANdb;

namespace Mp.Mod.CAN
{
    internal enum SignalCols
    {
        Message,
        ID,
        ByteCount,
        Signal,
        Rate,
        ByteOrder,
        SignalType,
        ModeValue,
        PivotBit,
        BitCount,
        DataType,
        Factor,
        Offset,
        Min,
        Max,
        Unit,
        Comment
    }

    public partial class CANPortDlg : Form
    {
        private Port _port;
        private Manager _canManager = new Manager();
        private Document _doc;
        private List<string> _loadDbFiles = new List<string>();
        private MessageView _msgViewSignal = new MessageView();
        private CANdbInportView _candbView = new CANdbInportView();

        public CANPortDlg(Port port,int index, Document doc)
        {
            _doc = doc;
            _port = port;

            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            _candbView.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(_candbView);
            _msgViewSignal.Dock = DockStyle.Fill;
            panel4.Controls.Add(_msgViewSignal);


            canport.Text = (index + 1).ToString();

            int rate = (int) XmlHelper.GetParamNumber(port.XmlRep, "bitRate");

            if (rate == 0)
                rate = 1000000;

            bitrate.Text = rate.ToString();

            adrMode.SelectedIndex = (int) XmlHelper.GetParamNumber(port.XmlRep, "extendedId");
            tact.SelectedIndex =(int) XmlHelper.GetParamNumber(port.XmlRep, "tact");

            LoadSignals();
            FormStateHandler.Restore(this, Document.RegistryKey + "CANPortDlg");
        }

        private void UpdateSource()
        {
            uint id = (uint)XmlHelper.GetParamNumber(_port.XmlRep, "sourceID");
            XmlElement xmlSource = _doc.GetXmlObjectById(id);
            int index = Convert.ToInt32(canport.Text);

            if (xmlSource == null)
            {//register a source
                id = _doc.RegisterSource("CAN " + ((index).ToString()), (index), "CAN" + (index).ToString());
                XmlHelper.SetParamNumber(_port.XmlRep, "sourceID", "uint32_t", id);
            }
        }

        private void LoadSignals()
        {
            foreach(XmlElement xmlSignal in _port.SignalList.ChildNodes)
            {
                int index = signalGrid.Rows.Add();
                DataGridViewRow row = signalGrid.Rows[index];

                row.Tag = xmlSignal;

                row.Cells[(int)SignalCols.Signal].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Cells[(int)SignalCols.Unit].Value = XmlHelper.GetParam(xmlSignal, "unit");
                row.Cells[(int)SignalCols.Comment].Value = XmlHelper.GetParam(xmlSignal, "comment");

                row.Cells[(int)SignalCols.Rate].Value = XmlHelper.GetParamDouble(xmlSignal, "samplerate");


                row.Cells[(int)SignalCols.Min].Value = XmlHelper.GetParamDouble(xmlSignal, "physMin");
                row.Cells[(int)SignalCols.Max].Value = XmlHelper.GetParamDouble(xmlSignal, "physMax");

                XmlElement xmlScaling = XmlHelper.GetChildByType(xmlSignal, "Mp.Scaling");

                if (xmlScaling != null)
                {
                    row.Cells[(int)SignalCols.Factor].Value = XmlHelper.GetParamDouble(xmlScaling, "factor");
                    row.Cells[(int)SignalCols.Offset].Value = XmlHelper.GetParamDouble(xmlScaling, "offset");
                }

                row.Cells[(int)SignalCols.Message].Value = XmlHelper.GetParam(xmlSignal, "message");
                row.Cells[(int)SignalCols.ID].Value = XmlHelper.GetParamNumber(xmlSignal, "id");
                row.Cells[(int)SignalCols.ByteCount].Value = XmlHelper.GetParamNumber(xmlSignal, "byteCount");
                row.Cells[(int)SignalCols.ByteOrder].Value = GetByteOrder((int)XmlHelper.GetParamNumber(xmlSignal, "byteOrder"));

                int signalType = (int)XmlHelper.GetParamNumber(xmlSignal, "signalType");
                row.Cells[(int)SignalCols.SignalType].Value = GetSignalType(signalType);
                row.Cells[(int)SignalCols.ModeValue].Value = XmlHelper.GetParamNumber(xmlSignal, "modeValue");
                row.Cells[(int)SignalCols.PivotBit].Value = XmlHelper.GetParamNumber(xmlSignal, "pivotBit");
                row.Cells[(int)SignalCols.BitCount].Value = XmlHelper.GetParamNumber(xmlSignal, "bitCount");
                row.Cells[(int)SignalCols.DataType].Value = GetCANDataType((Signal.ValueType) XmlHelper.GetParamNumber(xmlSignal, "canDataType"));

                if (signalType == (int)Signal.SignalType.ModeDepended)
                { //Save mode signal data.

                    ModeSignal modeSignal = new ModeSignal();

                    modeSignal.BitCount = (int) XmlHelper.GetParamNumber(xmlSignal, "modeBitCount");
                    modeSignal.ByteOrder = (Signal.ByteType) XmlHelper.GetParamNumber(xmlSignal, "modeByteOrder");
                    modeSignal.Factor = XmlHelper.GetParamDouble(xmlSignal, "modeFactor");
                    modeSignal.Offset = XmlHelper.GetParamDouble(xmlSignal, "modeOffset");
                    modeSignal.DataType = (Drv.CANdb.Signal.ValueType)XmlHelper.GetParamNumber(xmlSignal, "modeDataType");
                    modeSignal.PivotBit = (int) XmlHelper.GetParamNumber(xmlSignal, "modePivotBit");
                    row.Cells[(int)SignalCols.SignalType].Tag = modeSignal;
                }
            }
        }

        private XmlElement GetXmlSignalByData(DataGridViewRow row)
        {
            uint id = Convert.ToUInt32(row.Cells[(int)SignalCols.ID].Value);
            string signal = (string)row.Cells[(int)SignalCols.Signal].Value;

            
            foreach (XmlElement xmlSignal in _port.SignalList.ChildNodes)
            {
                string curSignal  = XmlHelper.GetParam(xmlSignal, "name");
                uint curId = (uint)XmlHelper.GetParamNumber(xmlSignal, "id");
                
                if (curId == id && signal == curSignal)
                    return xmlSignal;

            }

            return null; 
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if(tact.SelectedIndex != 0)
                UpdateSource();

            XmlHelper.SetParamNumber(_port.XmlRep, "bitRate", "uint32_t", Convert.ToInt32(bitrate.Text));
            XmlHelper.SetParamNumber(_port.XmlRep, "extendedId", "uint8_t", adrMode.SelectedIndex);
            XmlHelper.SetParamNumber(_port.XmlRep, "tact", "uint8_t", tact.SelectedIndex);

            DialogResult = DialogResult.OK;

            foreach (DataGridViewRow row in signalGrid.Rows)
            {
                XmlElement xmlSignal;

                if (row.Tag == null)
                {
                    xmlSignal = GetXmlSignalByData(row);
                    
                    if( xmlSignal == null)
                        xmlSignal = _doc.CreateXmlObject(_port.SignalList, "Mp.Sig", "Mp.CAN.Sig");
                }
                else
                {
                    xmlSignal = (XmlElement)row.Tag;
                }

                XmlHelper.SetParam(xmlSignal, "name", "string", (string) row.Cells[(int) SignalCols.Signal].Value);
                XmlHelper.SetParam(xmlSignal, "unit", "string", (string)row.Cells[(int)SignalCols.Unit].Value);
                XmlHelper.SetParam(xmlSignal, "comment", "string", (string)row.Cells[(int)SignalCols.Comment].Value);
                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", Convert.ToDouble(row.Cells[(int)SignalCols.Rate].Value));
                XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", GetValueType(row));
                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", Convert.ToDouble(row.Cells[(int)SignalCols.Min].Value));
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", Convert.ToDouble(row.Cells[(int)SignalCols.Max].Value));

                if (tact.SelectedIndex == 0)
                {
                    XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", 0);
                }
                else
                {
                    long id = XmlHelper.GetParamNumber(_port.XmlRep, "sourceID");
                    XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", id);
                }

                XmlElement xmlScaling = XmlHelper.GetChildByType(xmlSignal, "Mp.Scaling");

                if (xmlScaling == null)
                    xmlScaling = _doc.CreateXmlObject(xmlSignal, "Mp.Scaling", "Mp.Scaling.FactorOffset");

                XmlHelper.SetParamDouble(xmlScaling, "factor", "double", Convert.ToDouble(row.Cells[(int)SignalCols.Factor].Value));
                XmlHelper.SetParamDouble(xmlScaling, "offset", "double", Convert.ToDouble(row.Cells[(int)SignalCols.Offset].Value));

                XmlHelper.SetParam(xmlSignal, "message", "string", (string)row.Cells[(int)SignalCols.Message].Value);
                XmlHelper.SetParamNumber(xmlSignal, "id", "uint32_t", (long)Convert.ToUInt32(row.Cells[(int)SignalCols.ID].Value));                
                XmlHelper.SetParamNumber(xmlSignal, "byteCount", "uint32_t", (long)Convert.ToUInt32(row.Cells[(int)SignalCols.ByteCount].Value));

                string strByteOrder = (string) row.Cells[(int)SignalCols.ByteOrder].Value;
                XmlHelper.SetParamNumber(xmlSignal, "byteOrder", "uint8_t", GetByteOrder(strByteOrder));

                string strSigType = (string)row.Cells[(int)SignalCols.SignalType].Value;
                int signalType = GetSignalType(strSigType);

                XmlHelper.SetParamNumber(xmlSignal, "signalType", "uint8_t", signalType);

                XmlHelper.SetParamNumber(xmlSignal, "modeValue", "int32_t", Convert.ToInt32(row.Cells[(int)SignalCols.ModeValue].Value));

                XmlHelper.SetParamNumber(xmlSignal, "pivotBit", "uint8_t", Convert.ToInt32(row.Cells[(int)SignalCols.PivotBit].Value));
                XmlHelper.SetParamNumber(xmlSignal, "bitCount", "uint8_t", Convert.ToInt32(row.Cells[(int)SignalCols.BitCount].Value));
                XmlHelper.SetParamNumber(xmlSignal, "canDataType", "uint8_t", (int)GetCANDataType((string)row.Cells[(int)SignalCols.DataType].Value));

                if (signalType == (int)Signal.SignalType.ModeDepended)
                { //Save mode signal data.

                    ModeSignal modeSignal = (ModeSignal) row.Cells[(int)SignalCols.SignalType].Tag;

                    XmlHelper.SetParamNumber(xmlSignal, "modeBitCount", "uint8_t", modeSignal.BitCount);
                    XmlHelper.SetParamNumber(xmlSignal, "modeByteOrder", "uint8_t", (int) modeSignal.ByteOrder);
                    XmlHelper.SetParamDouble(xmlSignal, "modeFactor", "double", modeSignal.Factor);
                    XmlHelper.SetParamDouble(xmlSignal, "modeOffset", "double", modeSignal.Offset);                    
                    XmlHelper.SetParamNumber(xmlSignal, "modeDataType", "uint8_t", (int)modeSignal.DataType);
                    XmlHelper.SetParamNumber(xmlSignal, "modePivotBit", "uint8_t", modeSignal.PivotBit);                    
                }
            }
            RemoveUnusedSignals();

            _candbView.SaveLoadedFilesToReg();
            _doc.Modified = true;
            Close();
        }

           
    

        private void RemoveUnusedSignals()
        {
            for( int i  = 0; i < _port.SignalList.ChildNodes.Count; ++i)
            {
                XmlElement xmlSignal = (XmlElement) _port.SignalList.ChildNodes[i];
                if (!IsSignalInGrid(xmlSignal))
                {
                    _doc.RemoveXmlObject(xmlSignal);
                    --i;
                }
            }
        }

        private bool IsSignalInGrid(XmlElement xmlSignal)
        {
            uint id = (uint) XmlHelper.GetParamNumber(xmlSignal, "id");
            string sigName = XmlHelper.GetParam(xmlSignal, "name");

            foreach (DataGridViewRow row in signalGrid.Rows)
            {
                uint curid = Convert.ToUInt32(row.Cells[(int)SignalCols.ID].Value);
                string curName = (string)row.Cells[(int)SignalCols.Signal].Value;
                
                if (curid == id && sigName == curName)
                    return true;
            }

            return false;
        }

        private static int GetByteOrder(string strByteOrder)
        {
            int byteOrder = 0;

            switch (strByteOrder)
            {
                case "Intel":
                    byteOrder = (int)Signal.ByteType.Intel;
                break;

                case "Motorola":
                    byteOrder = (int)Signal.ByteType.MotorolaBackward;
                break;
            }
            return byteOrder;
        }

        private static string GetSignalType(int type)
        {
            switch ((Signal.SignalType)type)
            {
                case Signal.SignalType.Standard:
                    return "Standard";

                case Signal.SignalType.ModeSignal:
                    return "Mode";

                case Signal.SignalType.ModeDepended:
                    return "Mode depended";
            }
            return "";
        }

        private static int GetSignalType(string type)
        {
            switch (type)
            {
                case "Standard":
                    return (int)Signal.SignalType.Standard;

                case "Mode":
                    return (int)Signal.SignalType.ModeSignal;

                case "Mode depended":
                    return (int)Signal.SignalType.ModeDepended;
            }
            return 0;
        }


        private int GetValueType(DataGridViewRow row)
        {
            Drv.CANdb.Signal.ValueType dataType = GetCANDataType((string)row.Cells[(int)SignalCols.DataType].Value);
            int bitCount = Convert.ToInt32(row.Cells[(int) SignalCols.BitCount].Value);

            if( dataType == Signal.ValueType.Signed)
            {
                if (bitCount <= 8)
                    return (int) Mp.Scheme.Sdk.SignalDataType.SINT;

                if( bitCount <= 16)
                    return (int) Mp.Scheme.Sdk.SignalDataType.INT;

                if (bitCount <= 32)
                    return (int) Mp.Scheme.Sdk.SignalDataType.DINT;

                if( bitCount <= 64)
                    return (int) Mp.Scheme.Sdk.SignalDataType.LINT;
            }
            else if( dataType == Signal.ValueType.Unsigned)
            {
                if (bitCount <= 8)
                    return (int) Mp.Scheme.Sdk.SignalDataType.USINT;

                if (bitCount <= 16)
                    return (int) Mp.Scheme.Sdk.SignalDataType.UINT;

                if (bitCount <= 32)
                    return (int) Mp.Scheme.Sdk.SignalDataType.UDINT;

                if (bitCount <= 64)
                    return (int) Mp.Scheme.Sdk.SignalDataType.ULINT;
            }
            else if( dataType == Signal.ValueType.Float)
            {
                return (int) Mp.Scheme.Sdk.SignalDataType.REAL;
            }
            else if( dataType == Signal.ValueType.Double)
            {
                return (int) Mp.Scheme.Sdk.SignalDataType.LREAL;
            }
            return 0;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }


        private void dataGridView1_DragOver(object sender, DragEventArgs e)
        {
            ArrayList ar = new ArrayList();
            if(e.Data.GetDataPresent(ar.GetType()))
                e.Effect = DragDropEffects.Move;
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            ArrayList ar = new ArrayList();

            if (!e.Data.GetDataPresent(ar.GetType()))
                return;

            ar = (ArrayList) e.Data.GetData(ar.GetType());

            foreach (TreeNodeAdv ndav in ar)
            {
                Node node = (Node)ndav.Tag;
                CANdbInportView.TreeItemInfo info = (CANdbInportView.TreeItemInfo)node.Tag;

                if (info.Signal == null && info.Message != null)
                {//Drag the message
                    Mp.Drv.CANdb.Message message = info.Message;

                    foreach (Mp.Drv.CANdb.Signal sig in message.Signals)
                        AddSignal(sig, message);
                }
                else if (info.Signal != null)
                {//Signal
                    Mp.Drv.CANdb.Signal signal = info.Signal;
                    AddSignal(info.Signal, info.Message);                   
                }
            }
        }

        private static string GetByteOrder(int byteOrder)
        {
            switch((Signal.ByteType)byteOrder)
            {
                case Signal.ByteType.Intel:
                    return "Intel";
                
                case Signal.ByteType.MotorolaBackward:
                    return "Motorola";
            }

            return "";
        }

        private void AddSignal(Mp.Drv.CANdb.Signal signal, Mp.Drv.CANdb.Message message)
        {
            int index = signalGrid.Rows.Add();
            DataGridViewRow row = signalGrid.Rows[index];
            row.Cells[(int)SignalCols.Message].Value = message.Name;
            row.Cells[(int)SignalCols.BitCount].Value = signal.BitCount;
            row.Cells[(int)SignalCols.ByteCount].Value = message.ByteCount;
            row.Cells[(int)SignalCols.ByteOrder].Value = GetByteOrder((int) signal.ByteOrder);
            row.Cells[(int)SignalCols.Comment].Value = "";
            row.Cells[(int)SignalCols.Factor].Value = signal.Factor;
            row.Cells[(int)SignalCols.ID].Value = message.ID;
            row.Cells[(int)SignalCols.Max].Value = signal.Max;
            row.Cells[(int)SignalCols.Min].Value = signal.Min;
            row.Cells[(int)SignalCols.Rate].Value = 10;
            row.Cells[(int)SignalCols.ModeValue].Value = signal.ModeValue;
            row.Cells[(int)SignalCols.Offset].Value = signal.Offset;
            row.Cells[(int)SignalCols.PivotBit].Value = signal.PivotBit;
            row.Cells[(int)SignalCols.Signal].Value = signal.Name;
            row.Cells[(int)SignalCols.DataType].Value = GetCANDataType(signal.DataType);

            switch (signal.Type)
            {
                case Signal.SignalType.Standard:
                    row.Cells[(int)SignalCols.SignalType].Value = "Standard";
                    break;
                case Signal.SignalType.ModeSignal:
                    row.Cells[(int)SignalCols.SignalType].Value = "Mode";
                    break;
                case Signal.SignalType.ModeDepended:
                    {
                        row.Cells[(int)SignalCols.SignalType].Value = "Mode depended";
                        ModeSignal modeSignal = new ModeSignal();

                        foreach (Mp.Drv.CANdb.Signal msignal in message.Signals)
                        {
                            if (msignal.Type == Signal.SignalType.ModeSignal)
                            {
                                modeSignal.BitCount = msignal.BitCount;
                                modeSignal.ByteOrder = msignal.ByteOrder;
                                modeSignal.Factor = msignal.Factor;
                                modeSignal.DataType = msignal.DataType;
                                modeSignal.Offset = msignal.Offset;
                                modeSignal.PivotBit = msignal.PivotBit;
                                row.Cells[(int)SignalCols.SignalType].Tag = modeSignal;
                                break;
                            }
                        }
                    }
                    break;
            }
            row.Cells[(int)SignalCols.Unit].Value = signal.Unit;
        }

        private void removeSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in signalGrid.SelectedRows)
                signalGrid.Rows.Remove(row);
        }

        private void removeMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> toRemove = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in signalGrid.SelectedRows)
            {
                int id = Convert.ToInt32(row.Cells[(int)SignalCols.ID].Value);

                foreach (DataGridViewRow crow in signalGrid.Rows)
                {
                    int cid = Convert.ToInt32(crow.Cells[(int)SignalCols.ID].Value);

                    if( cid == id)
                        if(!toRemove.Contains(crow))
                            toRemove.Add(crow);
                }
            }

            foreach (DataGridViewRow row in toRemove)
                signalGrid.Rows.Remove(row);
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = (signalGrid.SelectedRows.Count == 0);            
        }

        private void CANPortDlg_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormStateHandler.Save(this, Document.RegistryKey + "CANPortDlg");
        }

        private void signalGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            errorProvider.Clear();

            if (e.RowIndex == -1)
                return;

            if (e.ColumnIndex == (int)SignalCols.Rate)
            {
                try
                {
                    uint value = Convert.ToUInt32(e.FormattedValue);
                    if (value == 0)
                    {
                        e.Cancel = true;
                        errorProvider.SetError(signalGrid, StringResource.NullRateErr);
                    }
                }
                catch(Exception ex)
                {
                    errorProvider.SetError(signalGrid,ex.Message);
                    e.Cancel = true;
                }
            }
        }

        private void bitrate_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                uint value = Convert.ToUInt32(bitrate.Text);
                if (value == 0)
                {
                    e.Cancel = true;
                    errorProvider.SetError(bitrate, StringResource.NullBitrateErr);
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                errorProvider.SetError(bitrate, ex.Message);
            }
        }

        public static Drv.CANdb.Signal.ValueType GetCANDataType(string dataType)
        {
            switch (dataType)
            {
                case "Signed":
                    return Mp.Drv.CANdb.Signal.ValueType.Signed;
                case "Unsigned":
                    return Mp.Drv.CANdb.Signal.ValueType.Unsigned;
                case "IEEE Float":
                    return Mp.Drv.CANdb.Signal.ValueType.Float;
                case  "IEEE Double":
                    return Mp.Drv.CANdb.Signal.ValueType.Double;
            }
            return Mp.Drv.CANdb.Signal.ValueType.Unsigned;
        }

        public static string GetCANDataType(Drv.CANdb.Signal.ValueType dataType)
        {
            switch (dataType)
            {
                case Mp.Drv.CANdb.Signal.ValueType.Unsigned:
                    return "Unsigned";

                case Mp.Drv.CANdb.Signal.ValueType.Signed:
                    return "Signed";

                case Mp.Drv.CANdb.Signal.ValueType.Float:
                    return "IEEE Float";
                case Mp.Drv.CANdb.Signal.ValueType.Double:
                    return "IEEE Double";
            }

            return "Unsigned";
        }


    

        private void signalGrid_SelectionChanged(object sender, EventArgs e)
        {
            _msgViewSignal.Clear();
            DataGridViewRow row = null;

            if (signalGrid.SelectedCells.Count == 1)
            {
                if (signalGrid.SelectedCells[0].RowIndex != -1)
                {
                    row = signalGrid.Rows[signalGrid.SelectedCells[0].RowIndex];
                }
            }

            if (signalGrid.SelectedRows.Count == 1)
                row = signalGrid.SelectedRows[0];

            if (row == null)
                return;

            int pivot =  Convert.ToInt32(row.Cells[(int) SignalCols.PivotBit].Value);
            int bitCount = Convert.ToInt32(row.Cells[(int) SignalCols.BitCount].Value);

            bool intel = (GetByteOrder((string)row.Cells[(int)SignalCols.ByteOrder].Value) == (int)Signal.ByteType.Intel);
            _msgViewSignal.SetRange(pivot, bitCount,intel);
            _msgViewSignal.SetPivotBit(pivot, intel);
        }

        /*private void createSignals_Click(object sender, EventArgs e)
        {
            EditMsgDlg dlg = new EditMsgDlg(signalGrid);

            dlg.ShowDialog();
        }*/

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 520);    
        }

        private void CANPortDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }

        private void edit_Click(object sender, EventArgs e)
        {
            CANSignalEditorDlg dlg = new CANSignalEditorDlg();
            LoadInSigEditorMessages(dlg);
        
            if (dlg.ShowDialog() == DialogResult.OK)
                SaveCANSigEditorData(dlg);
        }

        private void SaveCANSigEditorData(CANSignalEditorDlg dlg)
        {
            foreach (TreeNode msgNode in dlg.RootNode.Nodes)
            {
                CANSignalEditorDlg.MessageInfo msgInfo = (CANSignalEditorDlg.MessageInfo)msgNode.Tag;

                foreach (TreeNode sigNode in msgNode.Nodes)
                {
                    CANSignalEditorDlg.SignalInfo sigInfo = (CANSignalEditorDlg.SignalInfo)sigNode.Tag;
                    DataGridViewRow row = (DataGridViewRow) sigInfo.Tag;

                    if (row == null)
                    {
                        int index = signalGrid.Rows.Add();
                        row = signalGrid.Rows[index];
                    }

                    row.Cells[(int)SignalCols.Message].Value = msgInfo.Name;
                    row.Cells[(int)SignalCols.BitCount].Value = sigInfo.BitCount;
                    row.Cells[(int)SignalCols.ByteCount].Value = msgInfo.ByteCount;
                    row.Cells[(int)SignalCols.ByteOrder].Value = sigInfo.ByteOrder;
                    row.Cells[(int)SignalCols.Comment].Value = sigInfo.Comment;
                    row.Cells[(int)SignalCols.Factor].Value = sigInfo.Factor;
                    row.Cells[(int)SignalCols.ID].Value = msgInfo.ID;
                    row.Cells[(int)SignalCols.Max].Value = sigInfo.Max;
                    row.Cells[(int)SignalCols.Min].Value = sigInfo.Min;
                    row.Cells[(int)SignalCols.Rate].Value = 10;
                    row.Cells[(int)SignalCols.ModeValue].Value = sigInfo.ModeValue;
                    row.Cells[(int)SignalCols.Offset].Value = sigInfo.Offset;
                    row.Cells[(int)SignalCols.PivotBit].Value = sigInfo.PivotBit;
                    row.Cells[(int)SignalCols.Signal].Value = sigInfo.Signal;
                    row.Cells[(int)SignalCols.SignalType].Value = sigInfo.SignalType;
                    sigInfo.Tag = row;

                    if (sigInfo.SignalType == "Mode depended")
                    {
                        ModeSignal modeSignal = new ModeSignal();

                        CANSignalEditorDlg.SignalInfo modeSignalInfo = dlg.GetModeSignalInfo(msgNode);

                        if (modeSignalInfo != null)
                        {
                            modeSignal.BitCount = modeSignalInfo.BitCount;
                            if (modeSignalInfo.ByteOrder == "Intel")
                                modeSignal.ByteOrder = Mp.Drv.CANdb.Signal.ByteType.Intel;
                            else
                                modeSignal.ByteOrder = Mp.Drv.CANdb.Signal.ByteType.MotorolaBackward;

                            modeSignal.Factor = modeSignalInfo.Factor;
                            modeSignal.DataType = modeSignalInfo.DataType;
                            modeSignal.Offset = modeSignalInfo.Offset;
                            modeSignal.PivotBit = modeSignalInfo.PivotBit;
                            row.Cells[(int)SignalCols.SignalType].Tag = modeSignal;
                        }
                        else
                        {
                            row.Cells[(int)SignalCols.SignalType].Tag = msgInfo.ModeSignal;
                        }
                    }

                    row.Cells[(int)SignalCols.DataType].Value = CANPortDlg.GetCANDataType(sigInfo.DataType);
                    row.Cells[(int)SignalCols.Unit].Value = sigInfo.Unit;
                }
            }

            RemoveUnusedSignals(dlg.RootNode);
        }

        private void RemoveUnusedSignals(TreeNode root)
        {
            for (int i = 0; i < signalGrid.Rows.Count; ++i)
            {
                DataGridViewRow row = signalGrid.Rows[i];

                if (IsSignalInUse(row, root))
                    continue;

                signalGrid.Rows.Remove(row);
                --i;
            }
        }

        private bool IsSignalInUse(DataGridViewRow row, TreeNode root)
        {
            foreach (TreeNode msg in root.Nodes)
            {
                foreach (TreeNode sig in msg.Nodes)
                {
                    CANSignalEditorDlg.SignalInfo sigInfo = (CANSignalEditorDlg.SignalInfo)sig.Tag;
                    if (sigInfo.Tag == row)
                        return true;
                }
            }

            return false;
        }


        private void LoadInSigEditorMessages(CANSignalEditorDlg dlg)
        {
            dlg.RootNode = dlg.CANdbTree.Nodes.Add("CAN", "CAN", 0, 0);
            foreach (DataGridViewRow row in signalGrid.Rows)
            {
                uint id = Convert.ToUInt32(row.Cells[(int)SignalCols.ID].Value);

                TreeNode msgNode = dlg.FindIDInTree(id, dlg.RootNode);
                CANSignalEditorDlg.MessageInfo msgInfo = null;

                if (msgNode == null)
                {
                    string msg = (string)row.Cells[(int)SignalCols.Message].Value;
                    int byteCount = Convert.ToInt32(row.Cells[(int)SignalCols.ByteCount].Value);
                    msgNode = dlg.RootNode.Nodes.Add(id.ToString(), msg, 1, 1);
                    msgInfo = new CANSignalEditorDlg.MessageInfo(msg, id, byteCount);
                    msgNode.Tag = msgInfo;
                }
                else
                {
                    msgInfo = (CANSignalEditorDlg.MessageInfo)msgNode.Tag;
                }

                string signal = (string)row.Cells[(int)SignalCols.Signal].Value;
                TreeNode signalNode = msgNode.Nodes.Add(signal, signal, 2, 2);
                CANSignalEditorDlg.SignalInfo sigInfo = new CANSignalEditorDlg.SignalInfo();
                sigInfo.Tag = row;
                signalNode.Tag = sigInfo;
                sigInfo.BitCount = Convert.ToInt32(row.Cells[(int)SignalCols.BitCount].Value);
                sigInfo.ByteOrder = (string)row.Cells[(int)SignalCols.ByteOrder].Value;
                sigInfo.Comment = (string)row.Cells[(int)SignalCols.Comment].Value;
                sigInfo.Factor = Convert.ToDouble(row.Cells[(int)SignalCols.Factor].Value);
                sigInfo.Offset = Convert.ToDouble(row.Cells[(int)SignalCols.Offset].Value);
                sigInfo.PivotBit = Convert.ToInt32(row.Cells[(int)SignalCols.PivotBit].Value);
                sigInfo.Rate = Convert.ToInt32(row.Cells[(int)SignalCols.Rate].Value);
                sigInfo.Signal = signal;
                sigInfo.SignalType = (string)row.Cells[(int)SignalCols.SignalType].Value;
                sigInfo.DataType = CANPortDlg.GetCANDataType((string)row.Cells[(int)SignalCols.DataType].Value);
                sigInfo.Unit = (string)(row.Cells[(int)SignalCols.Unit].Value);
                sigInfo.Max = Convert.ToDouble(row.Cells[(int)SignalCols.Max].Value);
                sigInfo.Min = Convert.ToDouble(row.Cells[(int)SignalCols.Min].Value);

                if (sigInfo.SignalType == "Mode depended")
                    msgInfo.ModeSignal = (ModeSignal)row.Cells[(int)SignalCols.SignalType].Tag;
            }

            dlg.RootNode.ExpandAll();
        }
        private void setThisSamplarateForAllSignalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (signalGrid.SelectedRows.Count == 0)
                return;

            DataGridViewRow row = signalGrid.SelectedRows[0];
            uint rate = Convert.ToUInt32(row.Cells[(int)SignalCols.Rate].Value);
            foreach (DataGridViewRow crow in signalGrid.Rows)
            {
                if (crow != row)
                    crow.Cells[(int)SignalCols.Rate].Value = rate;
            }
        }

        private void CANPortDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            _candbView.Close();
        }

    }

    internal class ModeSignal
    {
        public int PivotBit;
        public int BitCount;
        public Mp.Drv.CANdb.Signal.ByteType ByteOrder;
        public Drv.CANdb.Signal.ValueType DataType;
        public double Factor;
        public double Offset;
    }
}
