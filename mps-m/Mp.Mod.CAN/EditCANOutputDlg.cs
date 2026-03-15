using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Mp.Drv.CANdb;
using Mp.Visual.Tree;
using Mp.Visual.Tree.Tree;
using Mp.Visual.Tree.Tree.NodeControls;
using Mp.Scheme.Sdk;

namespace Mp.Mod.CAN
{
    public partial class EditCANOutputDlg : Form
    {
        private CANdbInportView _candbView = new CANdbInportView();
        private MessageView _msgViewSignal = new MessageView();
        public EditCANOutputDlg()
        {
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            _candbView.Dock = DockStyle.Fill;
            _msgViewSignal.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(_candbView);
            panel4.Controls.Add(_msgViewSignal);            
        }

        private void edit_Click(object sender, EventArgs e)
        {
            CANSignalEditorDlg dlg = new CANSignalEditorDlg();
            LoadInSigEditorMessages(dlg);
            if (dlg.ShowDialog() == DialogResult.OK)
                SaveCANSigEditorData(dlg);
        }

        public DataGridView Channels
        {
            get { return signalGrid; }
        }

        private void SaveCANSigEditorData(CANSignalEditorDlg dlg)
        {
            foreach (TreeNode msgNode in dlg.RootNode.Nodes)
            {
                CANSignalEditorDlg.MessageInfo msgInfo = (CANSignalEditorDlg.MessageInfo)msgNode.Tag;

                foreach (TreeNode sigNode in msgNode.Nodes)
                {
                    CANSignalEditorDlg.SignalInfo sigInfo = (CANSignalEditorDlg.SignalInfo)sigNode.Tag;
                    DataGridViewRow row = (DataGridViewRow)sigInfo.Tag;

                    if (row == null)
                    {
                        int index = signalGrid.Rows.Add();
                        row = signalGrid.Rows[index];
                    }

                    row.Cells[(int)SignalCols.Message].Value = msgInfo.Name;
                    row.Cells[(int)SignalCols.BitCount].Value = sigInfo.BitCount;
                    row.Cells[(int)SignalCols.ByteCount].Value = msgInfo.ByteCount;
                    row.Cells[(int)SignalCols.ByteOrder].Value = sigInfo.ByteOrder;
                    row.Cells[(int)SignalCols.Factor].Value = sigInfo.Factor;
                    row.Cells[(int)SignalCols.ID].Value = msgInfo.ID;
                    row.Cells[(int)SignalCols.Max].Value = sigInfo.Max;
                    row.Cells[(int)SignalCols.Min].Value = sigInfo.Min;
                    
                    if (row.Cells[(int)SignalCols.Rate].Value == null)
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
                }
            }

            RemoveUnusedSignals(dlg.RootNode);
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
                sigInfo.Factor = Convert.ToDouble(row.Cells[(int)SignalCols.Factor].Value);
                sigInfo.Offset = Convert.ToDouble(row.Cells[(int)SignalCols.Offset].Value);
                sigInfo.PivotBit = Convert.ToInt32(row.Cells[(int)SignalCols.PivotBit].Value);
                sigInfo.Rate = Convert.ToInt32(row.Cells[(int)SignalCols.Rate].Value);
                sigInfo.Signal = signal;
                sigInfo.SignalType = (string)row.Cells[(int)SignalCols.SignalType].Value;
                sigInfo.DataType = CANPortDlg.GetCANDataType((string)row.Cells[(int)SignalCols.DataType].Value);
                sigInfo.Max = Convert.ToDouble(row.Cells[(int)SignalCols.Max].Value);
                sigInfo.Min = Convert.ToDouble(row.Cells[(int)SignalCols.Min].Value);

                if (sigInfo.SignalType == "Mode depended")
                    msgInfo.ModeSignal = (ModeSignal)row.Cells[(int)SignalCols.SignalType].Tag;
            }

            dlg.RootNode.ExpandAll();
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

            int pivot = Convert.ToInt32(row.Cells[(int)SignalCols.PivotBit].Value);
            int bitCount = Convert.ToInt32(row.Cells[(int)SignalCols.BitCount].Value);

            bool intel = (GetByteOrder((string)row.Cells[(int)SignalCols.ByteOrder].Value) == (int)Signal.ByteType.Intel);
            _msgViewSignal.SetRange(pivot, bitCount, intel);
            _msgViewSignal.SetPivotBit(pivot, intel);
        }

        public static string GetByteOrder(int byteOrder)
        {
            switch ((Signal.ByteType)byteOrder)
            {
                case Signal.ByteType.Intel:
                    return "Intel";

                case Signal.ByteType.MotorolaBackward:
                    return "Motorola";
            }

            return "";
        }
        public static int GetByteOrder(string strByteOrder)
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

        private void OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void dataGridView1_DragOver(object sender, DragEventArgs e)
        {
            ArrayList ar = new ArrayList();
            if (e.Data.GetDataPresent(ar.GetType()))
                e.Effect = DragDropEffects.Move;
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
                case "IEEE Double":
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

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            ArrayList ar = new ArrayList();

            if (!e.Data.GetDataPresent(ar.GetType()))
                return;

            ar = (ArrayList)e.Data.GetData(ar.GetType());

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
        private void AddSignal(Mp.Drv.CANdb.Signal signal, Mp.Drv.CANdb.Message message)
        {
            int index = signalGrid.Rows.Add();
            DataGridViewRow row = signalGrid.Rows[index];
            row.Cells[(int)SignalCols.Message].Value = message.Name;
            row.Cells[(int)SignalCols.BitCount].Value = signal.BitCount;
            row.Cells[(int)SignalCols.ByteCount].Value = message.ByteCount;
            row.Cells[(int)SignalCols.ByteOrder].Value = GetByteOrder((int)signal.ByteOrder);
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
        }

        private void signalGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            errorProvider.Clear();

            if(e.RowIndex == -1)
                return;


            if (e.ColumnIndex == (int)SignalCols.Rate)
            {
                try
                {
                    uint rate = Convert.ToUInt32(e.FormattedValue);

                    if (rate == 0)
                    {
                        errorProvider.SetError(signalGrid, StringResource.OutRateNullErr);
                        e.Cancel = true;
                    }
                }
                catch (Exception ex)
                {
                    errorProvider.SetError(signalGrid, ex.Message);
                    e.Cancel = true;
                }
            }
        }

        private void signalGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
             if(e.RowIndex == -1)
                return;


             if (e.ColumnIndex == (int)SignalCols.Rate)
             {
                 DataGridViewRow row = signalGrid.Rows[e.RowIndex];
                 uint rate = Convert.ToUInt32(row.Cells[e.ColumnIndex].Value);
                 uint id = Convert.ToUInt32(row.Cells[(int) SignalCols.ID].Value);

                 foreach (DataGridViewRow nrow in signalGrid.Rows)
                 {
                     if (nrow == row)
                         continue;

                     uint curid = Convert.ToUInt32(nrow.Cells[(int)SignalCols.ID].Value);
                     if (curid == id)
                         nrow.Cells[e.ColumnIndex].Value = rate;
                 }
             }
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

                    if (cid == id)
                        if (!toRemove.Contains(crow))
                            toRemove.Add(crow);
                }
            }

            foreach (DataGridViewRow row in toRemove)
                signalGrid.Rows.Remove(row);
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 870);
        }

        private void EditCANOutputDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }

        private void EditCANOutputDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            _candbView.Close();
        }
    }
}
