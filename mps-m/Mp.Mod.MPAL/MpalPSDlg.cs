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
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Mp.Visual.Tree.Tree;
using Mp.Visual.Tree.Tree.NodeControls;
using Mp.Scheme.Sdk;
using Mp.Utils;
using Mpal.Model;
using Mpal.Editor;


namespace Mp.Mod.MPAL
{
    internal partial class MpalPSDlg : Form
    {
        private Document _doc;
        private ProcessStation _station;
        private bool _dragInput = false;
        private string _program;
        private string _programSrcFile = "";
        private Function _function;

        class TriggerSigItem
        {
            private XmlElement _xmlSignal;
            public TriggerSigItem(XmlElement xmlSignal)
            {
                _xmlSignal = xmlSignal;
            }

            public override string ToString()
            {
                string str;
                str = XmlHelper.GetParam(_xmlSignal, "name");
                str += " (" + XmlHelper.GetParam(_xmlSignal, "samplerate") + " Hz)";
                return str;
            }

            public XmlElement XmlSignal
            {
                get { return _xmlSignal; }
            }
        }

        public MpalPSDlg(ProcessStation station)
        {
            _doc = station.Document;
            _station = station;

            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            FormStateHandler.Restore(this, Document.RegistryKey + "MPALPropertiesDlg");
            InitilizeOutputTree();
            InitializeInputSignalTree(_station.InputPorts);
            IntitilizeInputVarTree();
            InitializeTriggerSigList();
            UpdateOutputSampleRate(outputTree.Root);
            outputTree.Update();

            LoadDataInTrees();
            inputVarTree.ExpandAll();
            outputTree.ExpandAll();
            signalInTree.ExpandAll();
            psName.Text = _station.Text;

            startInDebugger.Checked = XmlHelper.GetParamNumber(_station.XmlRep, "runInDebugger") > 0;

            serverIP.Text = XmlHelper.GetParam(_station.XmlRep, "debuggerIP");
            if (serverIP.Text == "")
                serverIP.Text = "127.0.0.1";

            serverPort.Text = XmlHelper.GetParam(_station.XmlRep, "debuggerPort");
            if (serverPort.Text == "")
                serverPort.Text = "8000";

            string memSizeValue = XmlHelper.GetParam(_station.XmlRep, "vmMemSize");

            if (memSizeValue == "")
                memSizeValue = "10";

            memSize.Text = memSizeValue;
        }

        private void LoadDataInTrees()
        {
            _program = XmlHelper.GetParam(_station.XmlRep, "program");
            _programSrcFile = XmlHelper.GetParam(_station.XmlRep, "prgSrcFile");

            if (_program == null)
                return;

            if (_program == "")
                return;

            byte[] buffer = Convert.FromBase64String(_program);

            MemoryStream mm = new MemoryStream(buffer);

            //Load program
            if (!LoadProgram(mm))
                return;

            mm.Close();
            buffer = null;
            GC.Collect();

            //Trigger signal
            uint triggerSigId = (uint) XmlHelper.GetParamNumber(_station.XmlRep, "triggerSigID");
            XmlElement xmlTriggerSignal = _doc.GetXmlObjectById(triggerSigId);
            if (xmlTriggerSignal == null)
            {
                XmlHelper.SetParamNumber(_station.XmlRep, "triggerSigID", "uint32_t", 0);
            }
            else
            {
                bool found = false;
                for (int i = 0; i < triggerSignal.Items.Count; ++i)
                {
                    TriggerSigItem trigItem = (TriggerSigItem) triggerSignal.Items[i];
                    if (trigItem.XmlSignal == xmlTriggerSignal)
                    {
                        found = true;
                        triggerSignal.SelectedIndex = i;
                        break;
                    }
                }

                if (!found)
                    XmlHelper.SetParamNumber(_station.XmlRep, "triggerSigID", "uint32_t", 0);
            }

            //Input/Output signal variable mapping
            for( int p = 0; p <_station.XmlRep.ChildNodes.Count; ++p)
            {
                XmlElement xmlSigMap = (XmlElement) _station.XmlRep.ChildNodes[p];
                XmlAttribute nameAttr = xmlSigMap.Attributes["name"];

                if (nameAttr == null)
                    continue;

                if (nameAttr.Value != "sigVarMap" && nameAttr.Value != "sigDefValue" && nameAttr.Value != "sigPropVarMap")
                    continue;

                string[] strArray = xmlSigMap.InnerText.Split('/');

                //Build the path.
                List<int> path = BuildTreePath(nameAttr, strArray);

                string[] paramIndex = strArray[1].Split('#');
                int index = Convert.ToInt32(paramIndex[0]);
                Parameter param = _function.Parameters[index];
                Parameter.Access access = param.ParamAccess;

                //Get the xml signal.
                XmlElement xmlSignal = null;
                int propType = -1;

                if (nameAttr.Value == "sigVarMap" || nameAttr.Value == "sigPropVarMap")
                {
                    uint id = 0;
                    if(nameAttr.Value == "sigPropVarMap")
                    {
                        string[] a = strArray[0].Split('~');
                        id = Convert.ToUInt32(a[0]);
                        propType = Convert.ToInt32(a[1]);
                    }
                    else
                    {
                        id = Convert.ToUInt32(strArray[0]);
                    }

                    xmlSignal = _doc.GetXmlObjectById(id);

                    if (xmlSignal == null)
                    {//Signal doesn't exist => remove the mapping.
                        _station.XmlRep.RemoveChild(xmlSigMap);
                        --p;
                        continue;
                    }
                }

                //Update the variable/signal mapping
                switch(access)
                {
                    case Parameter.Access.Input:
                        if( nameAttr.Value == "sigVarMap" || nameAttr.Value == "sigPropVarMap")
                            UpdateInVarItem(xmlSignal, path, (strArray[strArray.Length - 1] == "s"), propType);
                        else
                            SetDefaultValueInVarItem(path, strArray[0]);
                    break;
                
                    case Parameter.Access.Output:
                        UpdateOutVarItem(xmlSignal, path);
                    break;
                    
                    case Parameter.Access.InOut:
                        if (nameAttr.Value == "sigVarMap")
                            UpdateInVarItem(xmlSignal, path, (strArray[strArray.Length - 1] == "s"),-1);
                        else
                            SetDefaultValueInVarItem(path, strArray[0]);

                        UpdateOutVarItem(xmlSignal, path);
                    break;
                }
            }

            inputVarTree.Update();
            outputTree.Update();
        }

        private static List<int> BuildTreePath(XmlAttribute nameAttr, string[] strArray)
        {
            List<int> path = new List<int>();
            int lenght = 0;

            //if (nameAttr.Value == "sigVarMap")
                lenght = strArray.Length - 1;
            //else
            //    lenght = strArray.Length;

            for (int i = 1; i < lenght; ++i)
            {
                string[] treeIndexStr = strArray[i].Split('#');
                path.Add(Convert.ToInt32(treeIndexStr[1]));
            }

            return path;
        }

        private void UpdateOutVarItem(XmlElement xmlSignal, List<int> path)
        {                        
            TreeNodeAdv node = outputTree.Root;
            
            foreach (int i in path)
                node = node.Children[i];

            MpalOutTreeItem item = (MpalOutTreeItem)node.Tag;

            item.SignalID =(uint)XmlHelper.GetObjectID(xmlSignal);
            item.Comment = XmlHelper.GetParam(xmlSignal, "comment");

            item.Signal = XmlHelper.GetParam(xmlSignal, "name");
            item.Unit = XmlHelper.GetParam(xmlSignal, "unit");
            //item.SampleRate = (uint) XmlHelper.GetParamDouble(xmlSignal, "samplerate");
            item.Min = (decimal) XmlHelper.GetParamDouble(xmlSignal, "physMin");
            item.Max = (decimal) XmlHelper.GetParamDouble(xmlSignal, "physMax");
        }

        private void UpdateInVarItem(XmlElement xmlSignal, List<int> path,  bool scaled, int propType)
        {
            TreeNodeAdv node = inputVarTree.Root;
            
            foreach (int i in path)
                node = node.Children[i];

            MpalInVarTreeItem item = (MpalInVarTreeItem)node.Tag;
            item.XmlSignal = xmlSignal;
            item.Scaled = scaled;

            if (propType != -1)
            {
                item.PropType = (PropertyType)propType;
                item.IsProperty = true;
            }
        }
        
        private void SetDefaultValueInVarItem(List<int> path, string defValue)
        {
            TreeNodeAdv node = inputVarTree.Root;

            foreach (int i in path)
                node = node.Children[i];

            MpalInVarTreeItem item = (MpalInVarTreeItem)node.Tag;

            if (defValue != "")
                item.DefValue = defValue;
        }


        private void UpdateOutputSampleRate(TreeNodeAdv parentNode)
        {
            if( parentNode.Children.Count == 0)
                return;

            if(triggerSignal.SelectedIndex == -1)
                return;
            TriggerSigItem triggerItem = (TriggerSigItem)triggerSignal.Items[triggerSignal.SelectedIndex];

            double sampleRate = XmlHelper.GetParamDouble(triggerItem.XmlSignal, "samplerate");

            foreach(TreeNodeAdv child in parentNode.Children)
            {
                MpalOutTreeItem item = (MpalOutTreeItem)child.Tag;
                item.SampleRate = (uint) sampleRate;
                UpdateOutputSampleRate(child);
            }
        }

        private void InitializeTriggerSigList()
        {
            foreach (Port port in _station.InputPorts)
            {
                if (port.SignalList == null)
                    continue;

                foreach (XmlElement xmlSignal in port.SignalList)
                {
                    if (XmlHelper.GetObjectID(xmlSignal) == 0)
                    {
                        XmlElement xmlSigFromRef = _doc.GetXmlObjectById(Convert.ToUInt32(xmlSignal.InnerText));
                        triggerSignal.Items.Add(new TriggerSigItem(xmlSigFromRef));
                    }
                    else
                    {
                        triggerSignal.Items.Add(new TriggerSigItem(xmlSignal));
                    }
                }
            }

            if( triggerSignal.Items.Count != 0)
                triggerSignal.SelectedIndex = 0;
        }

        private void IntitilizeInputVarTree()
        {
            NodeStateIcon nodeStateIcon = new NodeStateIcon();
            NodeTextBox nodeTextBoxVarName = new NodeTextBox();
            NodeTextBox nodeTextBoxVarType = new NodeTextBox();
            NodeTextBox nodeTextBoxSignal = new NodeTextBox();
            NodeCheckBox nodeCheckBoxScale = new NodeCheckBox();
            NodeTextBox nodeTextDefValue = new NodeTextBox();

            inputVarTree.Model = new MpalInVarTreeModel();
            inputVarTree.GridLineStyle = GridLineStyle.HorizontalAndVertical;

            inputVarTree.NodeControls.Add(nodeStateIcon);
            inputVarTree.NodeControls.Add(nodeTextBoxVarName);
            inputVarTree.NodeControls.Add(nodeTextBoxVarType);
            inputVarTree.NodeControls.Add(nodeTextBoxSignal);
            inputVarTree.NodeControls.Add(nodeCheckBoxScale);
            inputVarTree.NodeControls.Add(nodeTextDefValue);

            nodeStateIcon.DataPropertyName = "Icon";
            nodeStateIcon.ParentColumn = inputVarTree.Columns[0];

            nodeTextBoxVarName.DataPropertyName = "Name";
            nodeTextBoxVarName.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxVarName.DisplayHiddenContentInToolTip = true;
            nodeTextBoxVarName.EditEnabled = false;
            nodeTextBoxVarName.ParentColumn = inputVarTree.Columns[0];

            nodeTextBoxVarType.DataPropertyName = "Type";
            nodeTextBoxVarType.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxVarType.DisplayHiddenContentInToolTip = true;
            nodeTextBoxVarType.EditEnabled = false;
            nodeTextBoxVarType.ParentColumn = inputVarTree.Columns[1];

            nodeTextBoxSignal.DataPropertyName = "Signal";
            nodeTextBoxSignal.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxSignal.DisplayHiddenContentInToolTip = true;
            nodeTextBoxSignal.EditEnabled = false;
            nodeTextBoxSignal.ParentColumn = inputVarTree.Columns[2];

            nodeTextDefValue.DataPropertyName = "DefValue";
            nodeTextDefValue.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextDefValue.DisplayHiddenContentInToolTip = true;
            nodeTextDefValue.EditEnabled = true;
            nodeTextDefValue.ParentColumn = inputVarTree.Columns[3];

            nodeCheckBoxScale.DataPropertyName = "Scaled";
            nodeCheckBoxScale.EditEnabled = true;
            nodeCheckBoxScale.ParentColumn = inputVarTree.Columns[4];
        }

        private void InitializeInputSignalTree(List<Port> inputPorts)
        {
            NodeStateIcon nodeStateIcon = new NodeStateIcon();
            NodeTextBox nodeTextBoxSignal = new NodeTextBox();
            NodeTextBox nodeTextBoxDataType = new NodeTextBox();
            

            signalInTree.Model = new MpalInSignalTreeModel(inputPorts, _doc);
            signalInTree.GridLineStyle = GridLineStyle.HorizontalAndVertical;
            
            signalInTree.NodeControls.Add(nodeStateIcon);
            signalInTree.NodeControls.Add(nodeTextBoxSignal);
            signalInTree.NodeControls.Add(nodeTextBoxDataType);
            
            nodeStateIcon.DataPropertyName = "Icon";
            nodeStateIcon.ParentColumn = signalInTree.Columns[0];

            nodeTextBoxSignal.DataPropertyName = "Name";
            nodeTextBoxSignal.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxSignal.DisplayHiddenContentInToolTip = true;
            nodeTextBoxSignal.EditEnabled = false;
            nodeTextBoxSignal.ParentColumn = signalInTree.Columns[0];

            nodeTextBoxDataType.DataPropertyName = "DataType";
            nodeTextBoxDataType.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxDataType.DisplayHiddenContentInToolTip = true;
            nodeTextBoxDataType.EditEnabled = false;
            nodeTextBoxDataType.ParentColumn = signalInTree.Columns[1];

        }

        private void InitilizeOutputTree()
        {
            NodeStateIcon nodeStateIcon = new NodeStateIcon();
            NodeTextBox nodeTextBoxVar = new NodeTextBox();
            NodeTextBox nodeTextBoxDataType = new NodeTextBox();
            NodeTextBox nodeTextBoxSignal = new NodeTextBox();
            NodeIntegerTextBox nodeTextBoxSampleRate = new NodeIntegerTextBox();
            NodeNumericUpDown nodeTextBoxMin = new NodeNumericUpDown();
            nodeTextBoxMin.Minimum = Decimal.MinValue;
            nodeTextBoxMin.Maximum = Decimal.MaxValue;

            NodeNumericUpDown nodeTextBoxMax = new NodeNumericUpDown();
            nodeTextBoxMax.Minimum = Decimal.MinValue;
            nodeTextBoxMax.Maximum = Decimal.MaxValue;
            NodeNumericUpDown nodeTextBoxFactor = new NodeNumericUpDown();
            NodeNumericUpDown nodeTextBoxOffset = new NodeNumericUpDown();
            NodeTextBox nodeTextBoxUnit = new NodeTextBox();
            NodeTextBox nodeTextBoxComment = new NodeTextBox();

            outputTree.Model = new MpalOutTreeModel();

            outputTree.GridLineStyle = GridLineStyle.HorizontalAndVertical;

            outputTree.NodeControls.Add(nodeStateIcon);
            outputTree.NodeControls.Add(nodeTextBoxVar);
            outputTree.NodeControls.Add(nodeTextBoxDataType);
            outputTree.NodeControls.Add(nodeTextBoxSignal);
            outputTree.NodeControls.Add(nodeTextBoxSampleRate);
            outputTree.NodeControls.Add(nodeTextBoxMin);
            outputTree.NodeControls.Add(nodeTextBoxMax);
            outputTree.NodeControls.Add(nodeTextBoxFactor);
            outputTree.NodeControls.Add(nodeTextBoxOffset);
            outputTree.NodeControls.Add(nodeTextBoxUnit);
            outputTree.NodeControls.Add(nodeTextBoxComment);

            nodeStateIcon.DataPropertyName = "Icon";
            nodeStateIcon.ParentColumn = outputTree.Columns[0];

            nodeTextBoxVar.EditEnabled = false;
            nodeTextBoxVar.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxVar.DisplayHiddenContentInToolTip = true;
            nodeTextBoxVar.DataPropertyName = "Variable";
            nodeTextBoxVar.ParentColumn = outputTree.Columns[0];

            nodeTextBoxDataType.EditEnabled = false;
            nodeTextBoxDataType.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxDataType.DisplayHiddenContentInToolTip = true;
            nodeTextBoxDataType.DataPropertyName = "Type";
            nodeTextBoxDataType.ParentColumn = outputTree.Columns[1];

            nodeTextBoxSampleRate.EditEnabled = false;
            nodeTextBoxSampleRate.DisplayHiddenContentInToolTip = true;
            nodeTextBoxSampleRate.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxSampleRate.DataPropertyName = "SampleRate";
            nodeTextBoxSampleRate.ParentColumn = outputTree.Columns[2];

            nodeTextBoxSignal.EditEnabled = true;
            nodeTextBoxSignal.DisplayHiddenContentInToolTip = true;
            nodeTextBoxSignal.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxSignal.EditOnClick = true;
            nodeTextBoxSignal.DataPropertyName = "Signal";
            nodeTextBoxSignal.ParentColumn = outputTree.Columns[3];

            nodeTextBoxFactor.EditEnabled = true;
            nodeTextBoxFactor.DisplayHiddenContentInToolTip = true;
            nodeTextBoxFactor.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxFactor.EditOnClick = true;
            nodeTextBoxFactor.DataPropertyName = "Factor";
            nodeTextBoxFactor.ParentColumn = outputTree.Columns[4];

            nodeTextBoxOffset.EditEnabled = true;
            nodeTextBoxOffset.DisplayHiddenContentInToolTip = true;
            nodeTextBoxOffset.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxOffset.EditOnClick = true;
            nodeTextBoxOffset.DataPropertyName = "Offset";
            nodeTextBoxOffset.ParentColumn = outputTree.Columns[5];

            nodeTextBoxMin.EditEnabled = true;
            nodeTextBoxMin.DisplayHiddenContentInToolTip = true;
            nodeTextBoxMin.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxMin.EditOnClick = true;
            nodeTextBoxMin.DataPropertyName = "Min";
            nodeTextBoxMin.ParentColumn = outputTree.Columns[6];

            nodeTextBoxMax.EditEnabled = true;
            nodeTextBoxMax.DisplayHiddenContentInToolTip = true;
            nodeTextBoxMax.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxMax.EditOnClick = true;
            nodeTextBoxMax.DataPropertyName = "Max";
            nodeTextBoxMax.ParentColumn = outputTree.Columns[7];

            nodeTextBoxUnit.EditEnabled = true;
            nodeTextBoxUnit.DisplayHiddenContentInToolTip = true;
            nodeTextBoxUnit.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxUnit.EditOnClick = true;
            nodeTextBoxUnit.DataPropertyName = "Unit";
            nodeTextBoxUnit.ParentColumn = outputTree.Columns[8];

            nodeTextBoxComment.EditEnabled = true;
            nodeTextBoxComment.DisplayHiddenContentInToolTip = true;
            nodeTextBoxComment.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxComment.EditOnClick = true;
            nodeTextBoxComment.DataPropertyName = "Comment";
            nodeTextBoxComment.ParentColumn = outputTree.Columns[9];

            outputTree.Update();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            FormStateHandler.Save(this, Document.RegistryKey + "MPALPropertiesDlg");
            Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            _doc.UpdateSource(_station.XmlRep);

            FormStateHandler.Save(this, Document.RegistryKey + "MPALPropertiesDlg");
            errorProvider.Clear();

            List<MpalOutTreeItem> items = new List<MpalOutTreeItem>();
            CollectOutTreeSignals(outputTree.Root, items, "");

            if (triggerSignal.SelectedIndex == -1)
            {
                errorProvider.SetError(triggerSignal, StringResource.TriggerSigExpectedErr);
                return;
            }

            //Check output signals.
            foreach (MpalOutTreeItem item in items)
            {
                if (item.Signal == null || item.Signal == "")
                    continue;

                if (item.Min == item.Max)
                {
                    errorProvider.SetError(outputTree, String.Format(StringResource.DefSigMinMaxErr,item.Signal));
                    return;
                }
            }

            CreateUpdateOutPort(items);

            XmlHelper.SetParam(_station.XmlRep, "program", "string", _program);
            XmlHelper.SetParam(_station.XmlRep, "prgSrcFile", "string", _programSrcFile);
            XmlHelper.SetParamNumber(_station.XmlRep, "vmMemSize", "uint32_t", Convert.ToUInt32(memSize.Text));
            

            //Trigger signal.
            TriggerSigItem triggerSigItem = (TriggerSigItem)triggerSignal.Items[triggerSignal.SelectedIndex];
            uint id = XmlHelper.GetObjectID(triggerSigItem.XmlSignal);
            XmlHelper.SetParamNumber(_station.XmlRep, "triggerSigID", "uint32_t", id);

            if( startInDebugger.Checked)
                XmlHelper.SetParamNumber(_station.XmlRep, "runInDebugger", "uint8_t", 1);
            else
                XmlHelper.SetParamNumber(_station.XmlRep, "runInDebugger", "uint8_t", 0);

            XmlHelper.SetParam(_station.XmlRep, "debuggerIP", "string", serverIP.Text);
            XmlHelper.SetParamNumber(_station.XmlRep, "debuggerPort", "uint32_t", Convert.ToUInt32(serverPort.Text));

            //Input Signal2Variable
            RemoveInputSig2Var();
            SaveInputSig2Var(inputVarTree.Root,"");

            //Output port            
            //Create the signal variable mapping for output
            foreach (MpalOutTreeItem item in items)
            {
                string path = item.SignalID.ToString() + "/" + item.Path;
                XmlHelper.CreateElement(_station.XmlRep, "string", "sigVarMap", path);
            }

            _station.Text = psName.Text;
            _station.Document.Modified = true;

            uint sourceId = (uint) XmlHelper.GetParamNumber(_station.XmlRep, "sourceId");
            XmlElement xmlSource = _doc.GetXmlObjectById(sourceId);
            XmlHelper.SetParam(xmlSource, "name", "string", _station.Text);
            Close();
        }

        private void CreateUpdateOutPort(List<MpalOutTreeItem> items)
        {
            Port port = null;

            if (_station.OutputPorts.Count == 0)
            {
                //Create the data out port.
                port = new Port(new Point(_station.Rect.Right + _station.PortWidth, (int)(_station.Rect.Top + _station.PortTopOffset)), "Mp.Port.Out", false, true);

                port.SignalList = _doc.CreateSignalList();
                _station.AddPort(port);
            }
            else
            {
                port = _station.OutputPorts[0];
            }

            //Remove from port the undefined signals.
            RemoveOutPortSignals(port, items);

            //Update the output Signals.
            AddOrUpdateOutPortSignals(port, items);
        }

        private void AddOrUpdateOutPortSignals(Port port, List<MpalOutTreeItem> items)
        {
            XmlElement xmlSignal = null;

            foreach (MpalOutTreeItem item in items)
            {
                if (item.SignalID == 0)
                {//Create
                    xmlSignal = _doc.CreateXmlObject(port.SignalList, "Mp.Sig", "Mp.Sig");
                    item.SignalID = XmlHelper.GetObjectID(xmlSignal);
                }
                else
                {
                    xmlSignal = _doc.GetXmlObjectById(item.SignalID);
                }

                XmlHelper.SetParam(xmlSignal, "name", "string", item.Signal);
                XmlHelper.SetParam(xmlSignal, "unit", "string", item.Unit);
                XmlHelper.SetParam(xmlSignal, "comment", "string", item.Comment);
                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", Convert.ToDouble(item.SampleRate));
                XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", GetDataType(item.Param));
                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", Convert.ToDouble(item.Min));
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", Convert.ToDouble(item.Max));                
                
                XmlElement xmlScaling = XmlHelper.GetChildByType(xmlSignal, "Mp.Scaling");

                if( item.Factor == 1 && item.Offset == 0)
                {
                    if( xmlScaling != null)
                        _doc.RemoveXmlObject(xmlScaling);
                }
                else
                {
                    if (xmlScaling == null)
                        xmlScaling = _doc.CreateXmlObject(xmlSignal, "Mp.Scaling", "Mp.Scaling.FactorOffset");
                    
                    XmlHelper.SetParamDouble(xmlScaling,"factor","double",(double) item.Factor);
                    XmlHelper.SetParamDouble(xmlScaling,"offset","double",(double) item.Offset);
                }
                
                uint sourceId = (uint) XmlHelper.GetParamNumber(_station.XmlRep, "sourceId");
                XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", (long) sourceId);
            }
        }

        private int GetDataType(Parameter param)
        {
            switch (param.ParamDataType)
            {
                case DataType.BOOL:
                    return 1;
                
                case DataType.BYTE:
                case DataType.USINT:
                    return 4;

                
                case DataType.WORD:
                case DataType.UINT:
                    return 6;

                case DataType.DWORD:
                case DataType.UDINT:
                    return 8;

                case DataType.ULINT:
                case DataType.LWORD:
                    return 10;

                case DataType.SINT:
                    return 5;

                case DataType.INT:
                    return 7;

                case DataType.DINT:
                    return 9;

                case DataType.LINT:
                    return 11;

                case DataType.REAL:
                    return 3;

                case DataType.LREAL:
                    return 2;                
                
                case DataType.STRING:
                    return 12;
            }

            return 0;
        }

        private void RemoveOutPortSignals(Port port, List<MpalOutTreeItem> items)
        {
            for (int i = 0; i < port.SignalList.ChildNodes.Count; ++i)
            {
                XmlElement xmlSignal = port.SignalList.ChildNodes[i] as XmlElement;

                if (xmlSignal == null)
                    continue;

                if (FindSignalInItemList(xmlSignal, items))
                    continue;

                _doc.RemoveXmlObject(xmlSignal);
                --i;
            }
        }

        private bool FindSignalInItemList(XmlElement xmlSignal, List<MpalOutTreeItem> items)
        {
            uint sigId = XmlHelper.GetObjectID(xmlSignal);

            foreach (MpalOutTreeItem item in items)
            {
                if (sigId == item.SignalID)
                    return true;
            }

            return false;
        }

        private void CollectOutTreeSignals(TreeNodeAdv parentNode, List<MpalOutTreeItem> items, string parentPath)
        {
            int treeIndex = 0;

            foreach (TreeNodeAdv childNode in parentNode.Children)
            {
                MpalOutTreeItem item = (MpalOutTreeItem)childNode.Tag;

                switch(item.Param.ParamDataType)
                {
                    case DataType.STRUCT:
                    case DataType.UDT:                
                        string path;
                        if (parentPath == "")
                            path = item.Param.Index.ToString() + "#" + treeIndex.ToString();
                        else
                            path = parentPath + "/" + item.Param.Index.ToString() + "#" + treeIndex.ToString();

                        CollectOutTreeSignals(childNode, items, path);
                    break;
                    
                    case DataType.ARRAY:
                    case DataType.FB:
                    break;
                    
                    default:
                        if (item.Signal == null)
                        {
                            treeIndex++;
                            continue;
                        }

                        if (item.Signal == "")
                        {
                            treeIndex++;
                            continue;
                        }

                        if( parentPath == "")
                            item.Path = item.Param.Index.ToString() + "#" + treeIndex.ToString() + "/u";
                        else
                            item.Path = (parentPath + "/" + item.Param.Index.ToString()) + "#" + treeIndex.ToString() +"/u";

                        items.Add(item);
                    break;
                }

                treeIndex++;
            }
        }

        private void RemoveInputSig2Var()
        {
            for( int i = 0; i < _station.XmlRep.ChildNodes.Count; ++i)
            {
                XmlElement xmlSigVar = (XmlElement)_station.XmlRep.ChildNodes[i];
                XmlAttribute nameAttr = xmlSigVar.Attributes["name"];
                
                if (nameAttr == null)
                    continue;

                if (nameAttr.Value != "sigVarMap" && nameAttr.Value != "sigDefValue" && nameAttr.Value != "sigPropVarMap")
                    continue;

                _station.XmlRep.RemoveChild(xmlSigVar);
                --i;
            }
        }

        private void SaveInputSig2Var(TreeNodeAdv parentNode, string parentPath)
        {
            int treeIndex = 0;

            foreach (TreeNodeAdv childNode in parentNode.Children)
            {
                MpalInVarTreeItem item = (MpalInVarTreeItem)childNode.Tag;

                switch(item.Param.ParamDataType)
                {
                    case DataType.STRUCT:
                    case DataType.UDT:
                        SaveInputSig2Var(childNode, parentPath + "/" + item.Param.Index.ToString() + "#" + treeIndex.ToString());
                    break;
                
                    case DataType.ARRAY:
                    case DataType.FB:
                    break;

                    default:
                        if (item.XmlSignal == null && !item.DefValueChanged)
                        {
                            treeIndex++;
                            continue;
                        }
                    
                        uint sigId = 0;

                        if( item.XmlSignal != null)
                            sigId = XmlHelper.GetObjectID(item.XmlSignal);

                        string varPath;
                        if(parentPath == "")
                            varPath = "/" + item.Param.Index.ToString() + "#" + treeIndex.ToString();
                        else
                            varPath = parentPath + "/" + item.Param.Index.ToString() + "#" + treeIndex.ToString();

                        if (item.Scaled)
                            varPath += "/s";
                        else
                            varPath += "/u";

                        if (item.XmlSignal != null && !item.IsProperty)
                            XmlHelper.CreateElement(_station.XmlRep, "string", "sigVarMap", sigId.ToString() + varPath);

                        if (item.XmlSignal != null && item.IsProperty)
                            XmlHelper.CreateElement(_station.XmlRep, "string", "sigPropVarMap", sigId.ToString() + "~" +((int)item.PropType).ToString() + varPath);

                        if( item.DefValueChanged)
                            XmlHelper.CreateElement(_station.XmlRep, "string", "sigDefValue", item.DefValue + varPath);
                    break;
                }
                treeIndex++;
            }
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = "*.mpp";
            dlg.Filter = "*.mpp|*.mpp|*.*|*.*";
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                using (FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open))
                {
                    if (LoadProgram(fileStream))
                    {
                        fileStream.Seek(0, SeekOrigin.Begin);
                        byte[] buffer = new byte[fileStream.Length];
                        fileStream.Read(buffer, 0, (int)fileStream.Length);
                        _program = Convert.ToBase64String(buffer);
                        buffer = null;
                        GC.Collect();
                    }
                    else
                    {
                        MessageBox.Show(StringResource.ProgLoadErr, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    fileStream.Close();
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(StringResource.ProgLoadErr, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Hashtable SaveSig2VarMapping()
        {
            Hashtable sig2var = new Hashtable();
            SaveSig2VarMapping(sig2var, inputVarTree.Root,"");
            return sig2var;
        }

        private void SaveSig2VarMapping(Hashtable mapping, TreeNodeAdv parent, string path)
        {
            
            foreach (TreeNodeAdv node in parent.Children)
            {
                MpalInVarTreeItem item = (MpalInVarTreeItem)node.Tag;
                if (node.Children.Count == 0)
                {                    
                    if (item.XmlSignal != null || item.DefValue != "")
                        mapping.Add(path + "/" + item.Name, item);
                }
                else
                {
                    SaveSig2VarMapping(mapping, node, path + "/" + item.Name);
                }
            }
        }

        private Hashtable SaveVar2SigMapping()
        {
            Hashtable var2SigMap = new Hashtable();
            SaveVar2SigMapping(var2SigMap, outputTree.Root, "");
            return var2SigMap;
        }

        private void SaveVar2SigMapping(Hashtable var2SigMap, TreeNodeAdv parent, string path)
        {
            foreach (TreeNodeAdv node in parent.Children)
            {
                MpalOutTreeItem item = (MpalOutTreeItem)node.Tag;

                if (node.Children.Count == 0)
                {
                    if (item.Variable != "" && item.Variable != null)
                        var2SigMap.Add(path + "/" + item.Param.Name, item);
                    
                }
                else
                {
                   SaveVar2SigMapping(var2SigMap, node, path +"/" + item.Param.Name);
                }
            }
        }

        private void RestoreVar2SigMapping(Hashtable var2SigMap)
        {
            foreach (DictionaryEntry entry in var2SigMap)
            {
                string var = (string)entry.Key;
                var = var.TrimStart('/');
                MpalOutTreeItem item = GetOutVarTreeItemByName(var);

                if (item != null)
                {
                    MpalOutTreeItem oldItem = (MpalOutTreeItem)entry.Value;
                    item.Comment = oldItem.Comment;
                    item.Unit = oldItem.Unit;
                    item.Factor = oldItem.Factor;
                    item.Max = oldItem.Max;
                    item.Min = oldItem.Min;
                    item.Offset = oldItem.Offset;
                    item.Signal = oldItem.Signal;
                    item.SignalID = oldItem.SignalID;                    
                }
            }
        }

        private void RestoreSig2VarMapping(Hashtable mapping)
        {
            foreach (DictionaryEntry entry in mapping)
            {
                string var = (string)entry.Key;
                var = var.TrimStart('/');
                MpalInVarTreeItem item = GetInVarTreeItemByName(var);

                if (item != null)
                {
                    MpalInVarTreeItem oldItem = (MpalInVarTreeItem)entry.Value;
                    item.DefValue = oldItem.DefValue;
                    item.PropType = oldItem.PropType;
                    item.IsProperty = oldItem.IsProperty;                    
                    item.XmlSignal = oldItem.XmlSignal;                    
                }
            }
        }

        private MpalOutTreeItem GetOutVarTreeItemByName(string varNamePath)
        {
            varNamePath = varNamePath.TrimStart('/');

            string[] array = varNamePath.Split('/');


            TreeNodeAdv parent = outputTree.Root;
            MpalOutTreeItem varItem = null;
            bool found = false;

            foreach (string var in array)
            {
                foreach (TreeNodeAdv node in parent.Children)
                {
                    varItem = (MpalOutTreeItem)node.Tag;
                    if (varItem.Param.Name == var)
                    {
                        found = true;
                        parent = node;
                        break;
                    }
                }

                if (!found)
                    return null;
            }

            return varItem;
        }

        private MpalInVarTreeItem GetInVarTreeItemByName(string varNamePath)
        {
            varNamePath = varNamePath.TrimStart('/');

            string[] array = varNamePath.Split('/');


            TreeNodeAdv parent = inputVarTree.Root;
            MpalInVarTreeItem varItem = null;
            bool found = false;

            foreach (string var in array)
            {            
                foreach (TreeNodeAdv node in parent.Children)
                {
                    varItem = (MpalInVarTreeItem)node.Tag;
                    if (varItem.Name == var)
                    {
                        found = true;
                        parent = node;
                        break;
                    }
                }

                if (!found)
                    return null;
            }

            return varItem;
        }

        private bool LoadProgram(Stream stream)
        {            

            try
            {
                Unit module = new Unit();
                module.Deserialise(stream);
                
                if (module.Program == null)
                    return false;

                _function = module.Program;

                //Try to save the signal to variable mapping
                Hashtable sig2VarMap = SaveSig2VarMapping();

                MpalInVarTreeModel model = (MpalInVarTreeModel)inputVarTree.Model;
                model.MpalUnit = module;
                
                RestoreSig2VarMapping(sig2VarMap);

                inputVarTree.FullUpdate();

                MpalOutTreeModel outModel = (MpalOutTreeModel)outputTree.Model;

                Hashtable var2SigMap = SaveVar2SigMapping();

                outModel.MpalUnit = module;

                RestoreVar2SigMapping(var2SigMap);

                UpdateOutputSampleRate(outputTree.Root);
                outputTree.FullUpdate();

                inputVarTree.ExpandAll();
                outputTree.ExpandAll();
                signalInTree.ExpandAll();                
                programDescription.Text = module.Description.TrimStart('\n');
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        private void triggerSignal_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateOutputSampleRate(outputTree.Root);
            outputTree.Invalidate();
        }

        private void signalInTree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            _dragInput = true;

            if (signalInTree.SelectedNodes.Count == 0)
                return;

            MpalInSignalTreeItem sigItem = (MpalInSignalTreeItem)signalInTree.SelectedNodes[0].Tag;
            
            if (sigItem.IsPort)
                return;

            TreeNodeAdv[] nodes = new TreeNodeAdv[signalInTree.SelectedNodes.Count];
            signalInTree.SelectedNodes.CopyTo(nodes, 0);
            DoDragDrop(nodes, DragDropEffects.Copy);
        }

        private void inputVarTree_DragOver(object sender, DragEventArgs e)
        {
            if (_dragInput)
                e.Effect = DragDropEffects.Copy;
        }

        private bool IsDataTypeCompatible(XmlElement xmlSignal, DataType varType)
        {
            return true;
        }

        private void inputVarTree_DragDrop(object sender, DragEventArgs e)
        {
            if (!_dragInput)
                return;

            _dragInput = false;

            TreeNodeAdv targetNode = inputVarTree.DropPosition.Node;
            
            if (targetNode == null)
                return;

            MpalInVarTreeItem varItem = (MpalInVarTreeItem)targetNode.Tag;

            TreeNodeAdv[] selectedNodes = (TreeNodeAdv[])e.Data.GetData(typeof(TreeNodeAdv[]));

            if( selectedNodes.Length == 0)
                return;

            MpalInSignalTreeItem sigItem = (MpalInSignalTreeItem)selectedNodes[0].Tag;
            
            if(!IsDataTypeCompatible(sigItem.Signal, varItem.Param.ParamDataType))
            {
                MessageBox.Show(StringResource.SigVarDataTypeErr, "Program", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            varItem.IsProperty = false;

            if (sigItem.IsSignalProp)
               varItem.PropType = sigItem.PropType;

            varItem.XmlSignal = sigItem.Signal;
            
            inputVarTree.Update();
        }

        private void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (signalInTree.SelectedNode == null)
                return;

            signalInTree.SelectedNode.ExpandAll();
        }

        private void reduceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (signalInTree.SelectedNode == null)
                return;

            signalInTree.SelectedNode.CollapseAll();
        }

        private void expandToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (inputVarTree.SelectedNode == null)
                return;

            inputVarTree.SelectedNode.ExpandAll();
        }

        private void reduceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (inputVarTree.SelectedNode == null)
                return;

            inputVarTree.SelectedNode.CollapseAll();
        }

        private void removeSignalFromVariableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputVarTree.SelectedNode == null)
                return;

            MpalInVarTreeItem item = (MpalInVarTreeItem) inputVarTree.SelectedNode.Tag;
            
            if (item.XmlSignal == null)
                return;

            item.XmlSignal = null;
            item.Scaled = false;
            item.IsProperty = false;
            inputVarTree.Invalidate();
        }

        private void edit_Click(object sender, EventArgs e)
        {
            MPALMainFrame mf = new MPALMainFrame(_programSrcFile, true);
            mf.StartPosition = FormStartPosition.CenterScreen;
            mf.ShowInTaskbar = false;            
           
            if (mf.ShowDialog(this) != DialogResult.OK)
                return;

            if (mf.ProgramFile == null)
                return;

            if (mf.ProgramFile == "")
                return;

            _programSrcFile = Path.ChangeExtension(mf.ProgramFile,".mpal");
            try
            {
                using (FileStream fileStream = new FileStream(mf.ProgramFile, FileMode.Open))
                {
                    if (LoadProgram(fileStream))
                    {
                        fileStream.Seek(0, SeekOrigin.Begin);
                        byte[] buffer = new byte[fileStream.Length];
                        fileStream.Read(buffer, 0, (int)fileStream.Length);
                        _program = Convert.ToBase64String(buffer);
                        buffer = null;
                        GC.Collect();
                    }
                    else
                    {
                        MessageBox.Show(StringResource.ProgLoadErr, "Program", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(StringResource.ProgLoadErr, "Program", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void programDescription_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }       
        }

        private void defaultValueFromPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputVarTree.SelectedNode == null)
                return;


            SchemePropertyDlg dlg = new SchemePropertyDlg(_doc);
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            MpalInVarTreeItem item = (MpalInVarTreeItem)inputVarTree.SelectedNode.Tag;

            if (dlg.SelectedProperties.Count != 0)
            {                 
                 string type = _doc.GetPropertyType(dlg.SelectedProperties[0]);
                 if (IsNumericType(type))
                     item.DefValue = dlg.SelectedProperties[0];
                 else
                     MessageBox.Show(StringResource.NumVarTypeErr, "Program", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }

            inputVarTree.Invalidate();
        }

        private bool IsNumericType(string type)
        {
            switch (type)
            {
                case "USINT":
                case "UINT":
                case "UDINT":
                case "ULINT":
                case "BYTE":
                case "WORD":
                case "DWORD":
                case "LWORD":
                case "SINT":
                case "INT":
                case "DINT":
                case "LINT":
                case "REAL":
                case "LREAL":
                case "BOOL":
                case "ENUMERATION":
                    return true;
                
            }

            return false;
        }
        private void inputVarTree_DoubleClick(object sender, EventArgs e)
        {
            defaultValueFromPropertiesToolStripMenuItem_Click(sender, e);
        }

        private void MpalPSDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Document.ShowHelp(this, 350);
        }

        private void help_Click(object sender, EventArgs e)
        {
            MpalPSDlg_HelpRequested(null, null);
        }

        private void serverIP_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            string[] data = serverIP.Text.Split('.');

            if(data.Length != 4)
            {
                errorProvider.SetError(serverIP, StringResource.InvalidIP);
                e.Cancel = true;
                return;
            }

            try
            {
                Convert.ToByte(data[0]);
                Convert.ToByte(data[1]);
                Convert.ToByte(data[2]);
                Convert.ToByte(data[3]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                errorProvider.SetError(serverIP, StringResource.InvalidIP);
                e.Cancel = true;
                return;
            }
        }

        private void serverPort_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                errorProvider.Clear();
                Convert.ToUInt32(serverPort.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(serverPort, ex.Message);
                e.Cancel = true;
            }

        }

        private void memSize_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                uint size = Convert.ToUInt32(memSize.Text);
                if (size == 0)
                {
                    e.Cancel = true;
                    errorProvider.SetError(memSize, StringResource.MPALMemSizeErr);
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                errorProvider.SetError(memSize, ex.Message);
            }
        }
    }
}
