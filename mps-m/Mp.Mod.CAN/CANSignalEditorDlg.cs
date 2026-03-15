using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

using Mp.Scheme.Sdk;

namespace Mp.Mod.CAN
{
    public partial class CANSignalEditorDlg : Form
    {
        private MessageView _msgView = new MessageView();
        private TreeNode _rootNode;
        private ImageList _imgList = new ImageList();

        internal class MessageInfo
        {
            public MessageInfo(string name, uint id, int byteCount)
            {
                Name = name;
                ID = id;
                ByteCount = byteCount;
            }

            public string Name;
            public uint ID;
            public int ByteCount;
            public ModeSignal ModeSignal;
        }

        public class SignalInfo
        {
            public SignalInfo()
            {
            }
            
            public object Tag;
            public string Signal;
            public string ByteOrder;
            public string SignalType;
            public int ModeValue;
            public int PivotBit;
            public int BitCount;
            public Drv.CANdb.Signal.ValueType DataType;
            public double Factor;
            public double Offset;
            public double Min;
            public double Max;
            public string Unit;
            public string Comment;
            public int Rate;
        }

        public CANSignalEditorDlg()
        {
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            _imgList.Images.Add(Resource.DB);
            _imgList.Images.Add(Resource.Message);
            _imgList.Images.Add(Resource.Signal);
            msgTree.ImageList = _imgList;

            _msgView.Dock = DockStyle.Fill;
            panel1.Controls.Add(_msgView);
            Utils.FormStateHandler.Restore(this, "Mp.CAN.CANSignalEditorDlg");
        }


        public TreeNode RootNode
        {
            get { return _rootNode; }
            set { _rootNode = value; }
        }

        public TreeNode FindIDInTree(uint id, TreeNode dbNode)
        {
            foreach (TreeNode msgNode in dbNode.Nodes)
            {
                MessageInfo info = (MessageInfo)msgNode.Tag;
                if (info.ID == id)
                    return msgNode;
            }

            return null;
        }

        public TreeView CANdbTree
        {
            get { return msgTree; }
        }

        private bool ValidateTree()
        {
            errorProvider.Clear();

            foreach (TreeNode msgNode in _rootNode.Nodes)
            {
                MessageInfo msgInfo = (MessageInfo)msgNode.Tag;

                foreach (TreeNode sigNode in msgNode.Nodes)
                {
                    SignalInfo sigInfo = (SignalInfo)sigNode.Tag;
                    
                    if (!ValidateMsgSignal(msgInfo, sigInfo))
                        return false;
                }
            }

            return true;
        }

        
        private bool ValidateMsgSignal(MessageInfo msgInfo, SignalInfo sigInfo)
        {
            foreach (TreeNode msgNode in _rootNode.Nodes)
            {
                MessageInfo curMsgInfo = (MessageInfo)msgNode.Tag;

                if (curMsgInfo != msgInfo && curMsgInfo.ID == msgInfo.ID)
                {
                    errorProvider.SetError(msgTree, "The ID '" + curMsgInfo.ID.ToString() + "' of the message '" + msgInfo.Name +"' and '"+ curMsgInfo.Name+"' is duppicated.");
                    return false;
                }

                if( curMsgInfo == msgInfo)
                {
                    if(sigInfo.SignalType == "Mode depended")
                    {
                        bool modeSignalAvail = false;
                        foreach (TreeNode sigNode in msgNode.Nodes)
                        {
                            SignalInfo curSigInfo = (SignalInfo)sigNode.Tag;
                            if (curSigInfo.SignalType == "Mode")
                            {
                                modeSignalAvail = true;
                                break;
                            }
                        }

                        if (!modeSignalAvail && curMsgInfo.ModeSignal == null)
                        {                            
                            errorProvider.SetError(msgTree, String.Format(StringResource.ModSignalErr,sigInfo.Signal, msgInfo.Name));
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (!SaveCurrentData())
                return;

            if (!ValidateTree())
                return;

            DialogResult = DialogResult.OK;
            Close();
        }

        private bool SaveCurrentData()
        {
            if (msgTree.SelectedNode != null)
            {
                if (msgTree.SelectedNode.Tag is MessageInfo)
                {
                    SaveMessage(msgTree.SelectedNode);
                }

                if (msgTree.SelectedNode.Tag is SignalInfo)
                {
                    SaveMessage(msgTree.SelectedNode.Parent);
                    return SaveSignal(msgTree.SelectedNode);
                }
            }

            return true;
        }
        public SignalInfo GetModeSignalInfo(TreeNode msgNode)
        {
            foreach (TreeNode sigNode in msgNode.Nodes)
            {
                SignalInfo s = (SignalInfo)sigNode.Tag;
                if (s.SignalType == "Mode")
                    return s;
            }

            return null;
        } 
        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (msgTree.SelectedNode == null)
            {
                e.Cancel = true;
                return;
            }

            if (msgTree.SelectedNode.Tag == null)
            {
                contextMenuStrip.Items[0].Enabled = true;
                contextMenuStrip.Items[1].Enabled = false;
                contextMenuStrip.Items[2].Enabled = false;
                contextMenuStrip.Items[3].Enabled = false;
                contextMenuStrip.Items[4].Enabled = false;
                contextMenuStrip.Items[6].Enabled = true;
                return;
            }

            if (msgTree.SelectedNode.Tag is MessageInfo)
            {
                contextMenuStrip.Items[0].Enabled = false;
                contextMenuStrip.Items[1].Enabled = true;
                contextMenuStrip.Items[2].Enabled = false;
                contextMenuStrip.Items[3].Enabled = true;
                contextMenuStrip.Items[4].Enabled = false;
                contextMenuStrip.Items[6].Enabled = false;
                return;
            }

            if (msgTree.SelectedNode.Tag is SignalInfo)
            {
                contextMenuStrip.Items[0].Enabled = false;
                contextMenuStrip.Items[1].Enabled = false;
                contextMenuStrip.Items[2].Enabled = true;
                contextMenuStrip.Items[3].Enabled = false;
                contextMenuStrip.Items[4].Enabled = true;
                contextMenuStrip.Items[6].Enabled = false;
                return;
            }
        }

        private void msgTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _msgView.Clear();
            if (msgTree.SelectedNode == null)
            {
                msgGroup.Enabled = false;
                signalGroup.Enabled = false;
                return;
            }

            if (msgTree.SelectedNode.Tag == null)
            {
                msgGroup.Enabled = false;
                signalGroup.Enabled = false;
                return;
            }

            if (msgTree.SelectedNode.Tag is MessageInfo)
            {
                msgGroup.Enabled = true;
                signalGroup.Enabled = false;
                MessageInfo info = (MessageInfo)msgTree.SelectedNode.Tag;
                msgCtrl.Text = info.Name;
                idCtrl.Text = info.ID.ToString();
                byteCountCtrl.Text = info.ByteCount.ToString();

                foreach (TreeNode sigNode in msgTree.SelectedNode.Nodes)
                    UpdateCANMsgView((SignalInfo)sigNode.Tag,false);

                return;
            }

            if (msgTree.SelectedNode.Tag is SignalInfo)
            {
                msgGroup.Enabled = true;
                signalGroup.Enabled = true;

                TreeNode msgNode = msgTree.SelectedNode.Parent;

                MessageInfo info = (MessageInfo)msgNode.Tag;
                msgCtrl.Text = info.Name;
                idCtrl.Text = info.ID.ToString();
                byteCountCtrl.Text = info.ByteCount.ToString();

                SignalInfo sig = (SignalInfo)msgTree.SelectedNode.Tag;
                signalCtrl.Text = sig.Signal;
                byteOrderCtrl.Text = sig.ByteOrder;
                sigTypeCtrl.Text = sig.SignalType;
                modeValueCtrl.Text = sig.ModeValue.ToString();
                pivotBitCtrl.Text = sig.PivotBit.ToString();
                bitCountCtrl.Text = sig.BitCount.ToString();
                dataType.Text = CANPortDlg.GetCANDataType(sig.DataType);
                factorCtrl.Text = sig.Factor.ToString();
                offsetCtrl.Text = sig.Offset.ToString();
                minCtrl.Text = sig.Min.ToString();
                maxCtrl.Text = sig.Max.ToString();
                unitCtrl.Text = sig.Unit;
                commentCtrl.Text = sig.Comment;

                UpdateCANMsgView(sig,true);
                return;
            }

            

        }

        private void UpdateCANMsgView(SignalInfo sig, bool pivot)
        {
            _msgView.SetRange(sig.PivotBit, sig.BitCount, sig.ByteOrder == "Intel");

            if (pivot)
                _msgView.SetPivotBit(sig.PivotBit, sig.ByteOrder == "Intel");
        }

        private bool CheckBits(int pivot, int count, bool intel)
        {
            errorProvider.Clear();

            if (pivot > 63)
            {
                errorProvider.SetError(bitCountCtrl, StringResource.BitRangeErr);
                return false;
            }

            if (intel)
            {
                if((pivot + count) > 64)
                {
                    errorProvider.SetError(bitCountCtrl, StringResource.BitRangeErr);

                    return false;
                }
            }
            else
            {
                int msb = pivot / 8;
                int startBitInLsb = pivot % 8 + 1;                
                int restOfBits = count - startBitInLsb;     
                msb++;

                if (msb >= 8)
                {
                    errorProvider.SetError(bitCountCtrl, StringResource.BitRangeErr);

                    return false;
                }


                int byteCount = restOfBits / 8 + msb;

                if (restOfBits % 8 != 0)
                    byteCount++;

                if (byteCount > 8)
                {
                    errorProvider.SetError(bitCountCtrl, StringResource.BitRangeErr);

                    return false;
                }
            }

            return true;
        }
        private void msgTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (msgTree.SelectedNode == null)
                return;

            if (msgTree.SelectedNode.Tag == null)
                return;

            if (msgTree.SelectedNode.Tag is MessageInfo)
            {
                SaveMessage(msgTree.SelectedNode);
                return;
            }

            if (msgTree.SelectedNode.Tag is SignalInfo)
            {
                SaveMessage(msgTree.SelectedNode.Parent);

                e.Cancel = !SaveSignal(msgTree.SelectedNode);                   
            }
        }

        private bool SaveSignal(TreeNode sigNode)
        {
            SignalInfo sigInfo = (SignalInfo)sigNode.Tag;

            double min = Convert.ToDouble(minCtrl.Text);
            double max = Convert.ToDouble(maxCtrl.Text);
            errorProvider.Clear();
            /*
            if (min >= max)
            {

                errorProvider.SetError(maxCtrl, StringResource.MinMaxErr);
                return false;
            }*/

            if (sigInfo.Factor == 0)
            {
                errorProvider.SetError(factorCtrl, StringResource.FactorErr);
                return false;
            }

            sigInfo.BitCount = Convert.ToInt32(bitCountCtrl.Text);
            sigInfo.ByteOrder = byteOrderCtrl.Text;
            sigInfo.Comment = commentCtrl.Text;
            sigInfo.Factor = Convert.ToDouble(factorCtrl.Text);
            sigInfo.Offset = Convert.ToDouble(offsetCtrl.Text);
            sigInfo.PivotBit = Convert.ToInt32(pivotBitCtrl.Text);
            sigInfo.Signal = signalCtrl.Text;
            sigInfo.SignalType = sigTypeCtrl.Text;
            sigInfo.DataType = CANPortDlg.GetCANDataType(dataType.Text);
            sigInfo.Unit = unitCtrl.Text;
            sigInfo.Min = Convert.ToDouble(minCtrl.Text);
            sigInfo.Max = Convert.ToDouble(maxCtrl.Text);
            return CheckBits(sigInfo.PivotBit, sigInfo.BitCount, sigInfo.ByteOrder == "Intel");
        }

        private void SaveMessage(TreeNode msgNode)
        {
            MessageInfo msgInfo = (MessageInfo)msgNode.Tag;
            msgInfo.Name = msgCtrl.Text;
            msgInfo.ID = Convert.ToUInt32(idCtrl.Text);
            msgInfo.ByteCount = Convert.ToInt32(byteCountCtrl.Text);
        }

        private void msgCtrl_Validated(object sender, EventArgs e)
        {
            if (msgTree.SelectedNode == null)
                return;

            if (msgTree.SelectedNode.Tag == null)
                return;

            if (msgTree.SelectedNode.Tag is MessageInfo)
            {
                msgTree.SelectedNode.Text = msgCtrl.Text;
                return;
            }

            if (msgTree.SelectedNode.Tag is SignalInfo)
            {
                TreeNode msgNode = msgTree.SelectedNode.Parent;
                msgNode.Text = msgCtrl.Text;
                return;
            }

        }

        private void signalCtrl_Validated(object sender, EventArgs e)
        {
            if (msgTree.SelectedNode == null)
                return;

            if (msgTree.SelectedNode.Tag == null)
                return;

            if (msgTree.SelectedNode.Tag is SignalInfo)
            {
                msgTree.SelectedNode.Text = signalCtrl.Text;
                return;
            }
        }

        private void newMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode msgNode = _rootNode.Nodes.Add("Unnamed", "Unnamed", 1, 1);
            MessageInfo info = new MessageInfo("Unnamed",0,0);
            msgNode.Tag = info;
            msgNode.EnsureVisible();
            msgTree.SelectedNode = msgNode;
        }

        private void removeMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (msgTree.SelectedNode == null)
                return;

            msgTree.Nodes.Remove(msgTree.SelectedNode);
        }

        private void newSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode signalNode = msgTree.SelectedNode.Nodes.Add("Unnamed", "Unnamed", 2, 2);
            SignalInfo sigInfo = new SignalInfo();
            sigInfo.Signal = "Unnamed";
            sigInfo.SignalType = "Standard";
            sigInfo.ByteOrder = "Intel";
            sigInfo.Factor = 1;
            sigInfo.BitCount = 8;
            sigInfo.Min = -10;
            sigInfo.Max = 10;
            signalNode.Tag = sigInfo;
            signalNode.EnsureVisible();
            msgTree.SelectedNode = signalNode;
        }

        private void removeSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (msgTree.SelectedNode == null)
                return;

            msgTree.Nodes.Remove(msgTree.SelectedNode);
        }

        private void idCtrl_Validating(object sender, CancelEventArgs e)            
        {
            errorProvider.Clear();

            try
            {
                Convert.ToUInt32(idCtrl.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(idCtrl, ex.Message);
                e.Cancel = true;
            }
        }

        private void byteCountCtrl_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                uint count = Convert.ToUInt32(byteCountCtrl.Text);
                if (count > 8)
                {
                    errorProvider.SetError(byteCountCtrl, StringResource.ByteCountErr);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                errorProvider.SetError(byteCountCtrl, ex.Message);
                e.Cancel = true;
            }
        }

        private void modeValueCtrl_Validating(object sender, CancelEventArgs e)
        {

            errorProvider.Clear();
            try
            {
                Convert.ToInt32(modeValueCtrl.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(modeValueCtrl, ex.Message);
                e.Cancel = true;

            }
        }

        private void pivotBitCtrl_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToByte(pivotBitCtrl.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(pivotBitCtrl, ex.Message);
                e.Cancel = true;

            }
        }

        private void bitCountCtrl_Validating(object sender, CancelEventArgs e)
        {

            errorProvider.Clear();
            try
            {
                Convert.ToByte(bitCountCtrl.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(bitCountCtrl, ex.Message);
                e.Cancel = true;

            }
        }

        private void factorCtrl_Validating(object sender, CancelEventArgs e)
        {

            errorProvider.Clear();
            try
            {
                Convert.ToDouble(factorCtrl.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(factorCtrl, ex.Message);
                e.Cancel = true;

            }
        }

        private void offsetCtrl_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToDouble(offsetCtrl.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(offsetCtrl, ex.Message);
                e.Cancel = true;

            }
        }

        private void minCtrl_Validating(object sender, CancelEventArgs e)
        {

            errorProvider.Clear();
            try
            {
                Convert.ToDouble(minCtrl.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(minCtrl, ex.Message);
                e.Cancel = true;
            }
        }

        private void maxCtrl_Validating(object sender, CancelEventArgs e)
        {

            errorProvider.Clear();
            try
            {
                Convert.ToDouble(maxCtrl.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(maxCtrl, ex.Message);
                e.Cancel = true;

            }
        }

        private void pivotBitCtrl_Validated(object sender, EventArgs e)
        {
            SignalInfo sigInfo = (SignalInfo)msgTree.SelectedNode.Tag;
            sigInfo.PivotBit = Convert.ToInt32(pivotBitCtrl.Text);

            _msgView.Clear();

            UpdateCANMsgView(sigInfo, true);
        }

        private void bitCountCtrl_Validated(object sender, EventArgs e)
        {
            SignalInfo sigInfo = (SignalInfo)msgTree.SelectedNode.Tag;
            sigInfo.BitCount = Convert.ToInt32(bitCountCtrl.Text);

            _msgView.Clear();

            UpdateCANMsgView(sigInfo, true);
        }

        private void byteOrderCtrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            SignalInfo sigInfo = (SignalInfo)msgTree.SelectedNode.Tag;
            sigInfo.ByteOrder = byteOrderCtrl.Text;

            _msgView.Clear();

            if (dataType.SelectedIndex == 3)
            {
                if (sigInfo.ByteOrder == "Intel")
                {
                    sigInfo.PivotBit = 0;
                    pivotBitCtrl.Text = "0";
                }
                else
                {
                    sigInfo.PivotBit = 7;
                    pivotBitCtrl.Text = "7";
                }
            }

            UpdateCANMsgView(sigInfo, true);
        }

        private class ValueType
        {
            public ValueType()
            {
            }

            public uint ID;
            public string Signal;
            public Drv.CANdb.Signal.ValueType DataType;
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SaveCurrentData())
                return;

            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "*.dbc|*.dbc";
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            List<ValueType> valueType = new List<ValueType>();

            using (StreamWriter sw = new StreamWriter(dlg.FileName, false, Encoding.UTF8))
            {
                sw.WriteLine("VERSION \"HIPBNYYYYYYYYYYYYYYYYYYYYYYYYYYYNNNNNNNNNN/4/%%%/4/'%**4NNN///\"");
                sw.WriteLine();
                sw.WriteLine("NS_ : ");
                sw.WriteLine("	NS_DESC_");
                sw.WriteLine("	CM_");
                sw.WriteLine("	BA_DEF_");
                sw.WriteLine("	BA_");
                sw.WriteLine("	VAL_");
                sw.WriteLine("	CAT_DEF_");
                sw.WriteLine("	CAT_");
                sw.WriteLine("	FILTER");
                sw.WriteLine("	BA_DEF_DEF_");
                sw.WriteLine("	EV_DATA_");
                sw.WriteLine("	SGTYPE_");
                sw.WriteLine("	SGTYPE_VAL_");
                sw.WriteLine("	BA_DEF_SGTYPE_");
                sw.WriteLine("	BA_SGTYPE_");
                sw.WriteLine("	SIG_TYPE_REF_");
                sw.WriteLine("	VAL_TABLE_");
                sw.WriteLine("	SIG_GROUP_");
                sw.WriteLine("	SIG_VALTYPE_");
                sw.WriteLine("	SIGTYPE_VALTYPE_");
                sw.WriteLine();
                sw.WriteLine("BS_:");
                sw.WriteLine();
                sw.WriteLine("BU_: CAN1");

                foreach (TreeNode msgNode in _rootNode.Nodes)
                {
                    sw.WriteLine();
                    MessageInfo msgInfo = (MessageInfo)msgNode.Tag;
                    sw.Write("BO_ ");
                    sw.Write(msgInfo.ID);
                    sw.Write(" ");
                    sw.Write(msgInfo.Name.Replace(' ' , '_'));
                    sw.Write(": ");
                    sw.Write(msgInfo.ByteCount);
                    sw.Write(" CAN1");
                    sw.WriteLine();
                    
                    foreach (TreeNode sigNode in msgNode.Nodes)
                    {
                        SignalInfo sigInfo = (SignalInfo)sigNode.Tag;
                        sw.Write(" SG_ ");
                        sw.Write(sigInfo.Signal.Replace(' ','_'));

                        if (sigInfo.SignalType == "Mode depended")
                            sw.Write(" m" + sigInfo.ModeValue.ToString() + " : ");
                        else if( sigInfo.SignalType == "Mode")
                            sw.Write(" M : ");
                        else
                            sw.Write(" : ");

                        sw.Write(sigInfo.PivotBit);
                        sw.Write("|");
                        sw.Write(sigInfo.BitCount);
                        sw.Write("@");

                        if (sigInfo.ByteOrder == "Intel")
                            sw.Write("1");
                        else
                            sw.Write("0");

                        if (sigInfo.DataType == Mp.Drv.CANdb.Signal.ValueType.Signed)
                            sw.Write("- ");
                        else
                            sw.Write("+ ");

                        sw.Write("(");
                        NumberFormatInfo ninfo = new NumberFormatInfo();
                        ninfo.NumberDecimalSeparator = ".";

                        sw.Write(Convert.ToString(sigInfo.Factor,ninfo));
                        sw.Write(",");
                        sw.Write(Convert.ToString(sigInfo.Offset, ninfo));
                        sw.Write(") ");



                        sw.Write("[");
                        sw.Write(Convert.ToString(sigInfo.Min, ninfo));
                        sw.Write("|");
                        sw.Write(Convert.ToString(sigInfo.Max, ninfo));
                        sw.Write("] ");
                        
                        sw.Write("\"" + sigInfo.Unit+"\"");
                        sw.Write(" CAN1");
                        sw.WriteLine();

                        if (sigInfo.DataType == Mp.Drv.CANdb.Signal.ValueType.Float ||
                            sigInfo.DataType == Mp.Drv.CANdb.Signal.ValueType.Double)
                        {
                            ValueType vt = new ValueType();
                            vt.ID = msgInfo.ID;
                            vt.Signal = sigInfo.Signal;
                            vt.DataType = sigInfo.DataType;
                            valueType.Add(vt);
                        }
                    }
                }

                sw.WriteLine();

                foreach (ValueType vt in valueType)
                {
                    string signal = vt.Signal.Replace(' ','_');

                    string dt = "1";

                    if (vt.DataType == Mp.Drv.CANdb.Signal.ValueType.Double)
                        dt = "2";
                    
                    //SIG_VALTYPE_ 18 New_Signal_2 : 2;
                    sw.WriteLine("SIG_VALTYPE_ " + vt.ID.ToString() + " " + signal + " : " + dt+";");
                }
                sw.Close();
            }
        }

        private void dataType_SelectedIndexChanged(object sender, EventArgs e)
        {

            pivotBitCtrl.Enabled = true;
            bitCountCtrl.Enabled = true;

            SignalInfo sigInfo = (SignalInfo)msgTree.SelectedNode.Tag;

            switch (dataType.SelectedIndex)                
            {
                case 3:
                    if (byteOrderCtrl.SelectedIndex == 0)
                        pivotBitCtrl.Text = "0";
                    else
                        pivotBitCtrl.Text = "7";

                    pivotBitCtrl.Enabled = false;

                    bitCountCtrl.Text = "64";
                    bitCountCtrl.Enabled = false;

                    sigInfo.BitCount = 64;
                    sigInfo.PivotBit = Convert.ToInt32(pivotBitCtrl.Text);
                break;

                case 2:
                    bitCountCtrl.Text = "32";
                    bitCountCtrl.Enabled = false;
                    sigInfo.BitCount = 32;
                break;
            }            

            _msgView.Clear();

            UpdateCANMsgView(sigInfo, true);
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 850);
        }

        private void CANSignalEditorDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }

        private void CANSignalEditorDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Utils.FormStateHandler.Save(this, "Mp.CAN.CANSignalEditorDlg");
        }
    }
}

