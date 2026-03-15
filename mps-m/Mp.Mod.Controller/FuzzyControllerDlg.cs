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
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Mp.Scheme.Sdk;
using Mp.Utils;
using Mp.Visual.TextEditor;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Actions;
using Antlr.Runtime;
using Antlr.Runtime.Tree;

namespace Mp.Mod.Controller
{
    public partial class FuzzyControllerDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlPS;
        private XmlElement _xmlInSigList;
        private XmlElement _xmlOutSigList;
        private SignalInputView _signals;
        private List<TextMarker> _marker = new List<TextMarker>();
        private List<FuzzificationItem> _outputLigVar = new List<FuzzificationItem>();
        private System.Windows.Forms.Timer _checkCodeTimer = new Timer();
        private bool _syntaxError = false;
        private Mp.Visual.TextEditor.FindAndReplace.Engine _findReplace;
        private FindDlg _findDlg;
        private Mp.Visual.TextEditor.FindReplaceDlg _replaceDlg;

        public FuzzyControllerDlg(Document doc, XmlElement xmlPS, XmlElement xmlInSigList, XmlElement xmlOutSigList)
        {
            _doc = doc;
            _xmlPS = xmlPS;
            _xmlInSigList = xmlInSigList;
            _xmlOutSigList = xmlOutSigList;
            _signals = new SignalInputView(doc, _xmlInSigList);
            _signals.TabIndex = 10;
            _signals.Dock = DockStyle.Fill;

            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            toolStripMenuItemCut6.ShortcutKeys = Keys.Control | Keys.X;
            toolStripMenuItemCopy.ShortcutKeys = Keys.Control | Keys.C;
            toolStripMenuItemPaste.ShortcutKeys = Keys.Control | Keys.V;
            toolStripMenuItemDelete.ShortcutKeys = Keys.Delete;
            toolStripMenuItemSelectAll.ShortcutKeys = Keys.Control | Keys.A;
            findToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F;
            findAndReplaceToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.H;

            _findReplace = new Mp.Visual.TextEditor.FindAndReplace.Engine();            
            _findDlg = new FindDlg(_findReplace);
            _replaceDlg = new FindReplaceDlg(_findReplace);


            //Syntax highlighting
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            FileSyntaxModeProvider fsmProvider; // Provider
            if (Directory.Exists(dir))
            {
                fsmProvider = new FileSyntaxModeProvider(dir); // Create new provider with the highlighting directory.
                HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider); // Attach to the text editor.
                fuzzyRulesCtrl.SetHighlighting("FUZZY"); // Activate the highlighting, use the name from the SyntaxDefinition node.
            }

            splitContainer1.Panel1.Controls.Add(_signals);
            LoadData();
            FormStateHandler.Restore(this, "Mp.Controller.FuzzyControllerDlg");

            _checkCodeTimer.Interval = 700;
            _checkCodeTimer.Tick += new EventHandler(OnCheckCodeTimerTick);
        }


        private void LoadData()
        {
            name.Text = XmlHelper.GetParam(_xmlPS, "name");

            //Input signal
            if (_xmlOutSigList.ChildNodes.Count == 0)
            {
                outRate.Text = "100";
                outMin.Text = "-10";
                outMax.Text = "10";
            }
            else
            {
                XmlElement xmlSignal = (XmlElement) _xmlOutSigList.ChildNodes[0];
                sigName.Text = XmlHelper.GetParam(xmlSignal, "name");
                outRate.Text = XmlHelper.GetParamDouble(xmlSignal, "samplerate").ToString();
                outMin.Text = XmlHelper.GetParamDouble(xmlSignal, "physMin").ToString();
                outMax.Text = XmlHelper.GetParamDouble(xmlSignal, "physMax").ToString();
                outUnit.Text = XmlHelper.GetParam(xmlSignal, "unit");
                outComment.Text = XmlHelper.GetParam(xmlSignal, "comment");
                   
                Hashtable table = LoadLinguisticVar(xmlSignal);
                
                foreach(DictionaryEntry entry in table)
                    _outputLigVar = (List<FuzzificationItem>)entry.Value;

                UpdateOutputLingVars();
            }

            //Output signals

            Hashtable variables = LoadLinguisticVar(_xmlPS);

            foreach(DictionaryEntry entry in variables)
            {
                uint id = (uint) entry.Key;
                XmlElement xmlSignal = _doc.GetXmlObjectById(id);
                int index = channels.Rows.Add();
                DataGridViewRow row = channels.Rows[index];
                row.Tag = xmlSignal;
                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal,"name");
                List<FuzzificationItem> items = (List<FuzzificationItem>)entry.Value;
                row.Cells[0].Tag = items;
                StringBuilder sb = new StringBuilder();

                foreach (FuzzificationItem item in items)
                {
                    sb.Append(item.ItemName);
                    sb.Append("; ");
                }

                row.Cells[1].Value = sb.ToString().TrimEnd(' ').TrimEnd(';');
                row.Cells[2].Value = "...";
           }

            fuzzyRulesCtrl.Text = XmlHelper.GetParam(_xmlPS, "fuzzyRulesText");
            ValidateRules();
        }

        private void UpdateOutputLingVars()
        {
            StringBuilder sb = new StringBuilder();

            foreach (FuzzificationItem item in _outputLigVar)
            {
                sb.Append(item.ItemName);
                sb.Append("; ");
            }

            outLingVar.Text = sb.ToString().TrimEnd(' ').TrimEnd(';');
        }

        private Hashtable LoadLinguisticVar(XmlElement xmlObj)
        {
            Hashtable variables = new Hashtable();

            XmlElement xmlLinguisticVars = XmlHelper.GetChildByType(xmlObj, "Mp.Controller.LinguisticVars");
            
            if (xmlLinguisticVars == null)
                return variables;

            foreach (XmlElement xmlLigVar in xmlLinguisticVars.ChildNodes)
            {
                FuzzificationItem item = new FuzzificationItem();
                item.ItemName = XmlHelper.GetParam(xmlLigVar, "name");
                item.P1x = XmlHelper.GetParamDouble(xmlLigVar, "x1");
                item.P2x = XmlHelper.GetParamDouble(xmlLigVar, "x2");
                item.P3x = XmlHelper.GetParamDouble(xmlLigVar, "x3");
                item.P4x = XmlHelper.GetParamDouble(xmlLigVar, "x4");
                uint id = (uint)XmlHelper.GetParamNumber(xmlLigVar, "signal");
                List<FuzzificationItem> items = null;

                if (variables.Contains(id))
                {
                    items = (List<FuzzificationItem>)variables[id];
                }
                else
                {
                    items = new List<FuzzificationItem>();
                    variables.Add(id, items);
                }

                items.Add(item);
            }
            return variables;
        }

        private void channels_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.ListViewItem"))
            {
                ListViewItem item = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");

                if (item.Tag is XmlElement)
                    e.Effect = DragDropEffects.Move;
            }
        }

        private void channels_DragDrop(object sender, DragEventArgs e)
        {
            ListViewItem item = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");

            XmlElement xmlSignal = (XmlElement)item.Tag;

            foreach (DataGridViewRow r in channels.Rows)
            {
                if (r.Tag == xmlSignal)
                    return;
            }

            int index = channels.Rows.Add();
            DataGridViewRow row = channels.Rows[index];
            
            List<FuzzificationItem> items = new List<FuzzificationItem>();
            row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
            row.Cells[0].Tag = items;
            row.Cells[1].Value = "";
            row.Cells[2].Value = "...";
            row.Tag = xmlSignal;
        }

        private void channels_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            if (e.ColumnIndex == 2)
            {
                DataGridViewRow row = channels.Rows[e.RowIndex];
                List<FuzzificationItem> list = (List<FuzzificationItem>) row.Cells[0].Tag;
                XmlElement xmlSignal = (XmlElement) row.Tag;
                string unit = XmlHelper.GetParam(xmlSignal, "unit");
                string sigName = XmlHelper.GetParam(xmlSignal, "name");

                if(unit != "")
                    sigName += " (" + unit + ")";

                double min = XmlHelper.GetParamDouble(xmlSignal, "physMin");
                double max = XmlHelper.GetParamDouble(xmlSignal, "physMax");

                FuzzificationDlg dlg = new FuzzificationDlg(list, sigName, min, max, false);
                dlg.ShowDialog();

                StringBuilder sb = new StringBuilder();
                foreach (FuzzificationItem item in list)
                {
                    sb.Append(item.ItemName);
                    sb.Append("; ");
                }

                string variables = sb.ToString().TrimEnd(' ').TrimEnd(';');
                row.Cells[1].Value = variables;

                ValidateRules();
            }
        }

        private void ValidateRules()
        {
            try
            {
                ClearAllMarker();

                _syntaxError = false;
                BaseTree tree = ParseRules();

                if (_syntaxError)
                    return;

                for (int i = 0; i < tree.ChildCount / 3; ++i)
                {
                    checkExpresion((ITree)tree.Children[i*3 + 1], true);
                    ITree thenNode = (ITree)tree.Children[i*3 + 2];
                    checkExpresion(thenNode.GetChild(0), false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private BaseTree ParseRules()
        {            
            FuzzyRuleParser.rule_return rules;

            ANTLRStringStream inputStream = new ANTLRStringStream(fuzzyRulesCtrl.Text);
            FuzzyRuleLexer lexer = new FuzzyRuleLexer(inputStream);
            lexer.OnNewError += new ErrorMessage(ScanerParserOnMessage);

            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            FuzzyRuleParser parser = new FuzzyRuleParser(tokenStream);
            parser.OnNewError += new ErrorMessage(ScanerParserOnMessage);

            rules = parser.rule();

            if (_syntaxError)
                return null;

            return (BaseTree) rules.Tree;
        }

        private bool IsLingVarAvailable(string var, bool condition)
        {
            if (!condition)
            {
                foreach (FuzzificationItem item in _outputLigVar)
                {
                    if (item.ItemName == var)
                        return true;
                }
            }
            else
            {

                foreach (DataGridViewRow row in channels.Rows)
                {
                    List<FuzzificationItem> items = (List<FuzzificationItem>)row.Cells[0].Tag;
                    foreach (FuzzificationItem item in items)
                    {
                        if (item.ItemName == var)
                            return true;
                    }
                }
            }

            return false;
        }

        private void checkExpresion(ITree expNode, bool condition)
        {
            if (expNode.Type == FuzzyRuleParser.STRING_LITERAL_UNI)
            {
                string var = expNode.Text.TrimStart('"').TrimEnd('"');
                if (!IsLingVarAvailable(var, condition))
                {
                    _syntaxError = true;
                    AddMarker(expNode.Line, expNode.CharPositionInLine, StringResource.UndefinedSymbol, TextMarkerType.WaveLine);
                    fuzzyRulesCtrl.ActiveTextAreaControl.Refresh();
                }
            }
            else
            {
                checkExpresion(expNode.GetChild(0), condition);
                checkExpresion(expNode.GetChild(1), condition);
            }
        }

        public void ClearAllMarker()
        {
            foreach (TextMarker marker in _marker)
                fuzzyRulesCtrl.Document.MarkerStrategy.RemoveMarker(marker);

            _marker.Clear();

            fuzzyRulesCtrl.ActiveTextAreaControl.Refresh();
        }

        public void AddMarker(int line, int col, string text, TextMarkerType type)
        {
            int offset = fuzzyRulesCtrl.Document.PositionToOffset(new TextLocation(col, line - 1));
            int lenght = Math.Min(fuzzyRulesCtrl.Document.TextLength, offset + 100);
            string str = fuzzyRulesCtrl.Document.GetText(offset, lenght - offset);
            string[] array = str.Split(new char[] { ' ', '\n', '\r', '\t', '\b', '(', ')','{', '}', '[',']', ',', ';', '+',
                                                    '-','*','/',':','=', '<','>'});

            if (array.Length == 0)
                return;

            lenght = array[0].Length;

            TextMarker marker = new TextMarker(offset, lenght, type);
            _marker.Add(marker);
            marker.ToolTip = text;
            fuzzyRulesCtrl.Document.MarkerStrategy.AddMarker(marker);
        }

        public static string[] GetErrorInfo(string text)
        {
            string[] array = text.Split(':');

            string[] message = new string[5];

            //C:\Users\cr\Desktop\test.mpal (47,4): error C1002: Undefined symbol 'INCv'

            if (array.Length < 3)
                return null;

            if (array[0].Length == 1)
            {
                message[0] = array[2].Replace("error", "");
                message[1] = array[3];
                string[] filePosArr = array[1].Split('(');
                message[2] = array[0] + ":" + filePosArr[0];
                string pos = filePosArr[1].TrimEnd(')');
                message[3] = pos.Split(',')[0];
                message[4] = pos.Split(',')[1];
            }
            else
            {
                message[0] = array[1].Replace("error", "");
                message[1] = array[2];
                string[] filePosArr = array[0].Split('(');
                message[2] = filePosArr[0];
                string pos = filePosArr[1].TrimEnd(')');
                message[3] = pos.Split(',')[0];
                message[4] = pos.Split(',')[1];
            }
            return message;
        }

        private void ScanerParserOnMessage(string msg)
        {
            _syntaxError = true;

            string[] message = GetErrorInfo(msg);
            try
            {
                int line = Convert.ToInt32(message[3]);
                int col = Convert.ToInt32(message[4]);

                AddMarker(line, col, message[0] + ": " + message[1], ICSharpCode.TextEditor.Document.TextMarkerType.WaveLine);
                fuzzyRulesCtrl.ActiveTextAreaControl.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private bool IsVariableDupplicated(FuzzificationItem item)
        {
            //Check input
            foreach (DataGridViewRow row in channels.Rows)
            {
                List<FuzzificationItem> items = (List<FuzzificationItem>)row.Cells[0].Tag;

                foreach (FuzzificationItem curItem in items)
                {
                    if (item != curItem && curItem.ItemName == item.ItemName)
                        return true;
                }
            }

            //Check output 
            foreach (FuzzificationItem curItem in _outputLigVar)
            {
                if (item != curItem && curItem.ItemName == item.ItemName)
                    return true;
            }

            return false;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            if (_syntaxError)
            {
                errorProvider.SetError(fuzzyRulesCtrl, StringResource.RulesErr);
                return;
            }

            if (fuzzyRulesCtrl.Text == null || fuzzyRulesCtrl.Text == "")
            {
                errorProvider.SetError(fuzzyRulesCtrl, StringResource.NoRulesDefErr);
                return;
            }

            if (outLingVar.Text == null || outLingVar.Text == "")
            {
                errorProvider.SetError(onLingOutput, StringResource.NoLingVarDef);
                return;
            }

            double outSigMin = Convert.ToDouble(outMin.Text);
            double outSigMax = Convert.ToDouble(outMax.Text);

            if (outSigMin >= outSigMax)
            {
                errorProvider.SetError(outMax, StringResource.MinMaxError);
                return;
            }

            foreach(DataGridViewRow row in channels.Rows)
            {
                List<FuzzificationItem> items = (List<FuzzificationItem>) row.Cells[0].Tag;

                foreach( FuzzificationItem item in items)
                {
                    if (IsVariableDupplicated(item))
                    {
                        string message = String.Format(StringResource.NoLingVarDuppErr, item.ItemName);
                        errorProvider.SetError(fuzzyRulesCtrl, message);
                        return;
                    }
                }
            }

            _doc.UpdateSource(_xmlPS);

            //Output signal
            SaveOutputSignal();

            //Input signals
            SaveInputSignals();
            
            //Save rules
            SaveRules();

            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);                
            _doc.Modified = true;
            Close();
        }

        private void SaveRules()
        {
            BaseTree tree = ParseRules();

            if (tree == null)
                return;

            XmlElement xmlFuzzyRules = XmlHelper.GetChildByType(_xmlPS, "Mp.Controller.FuzzyRules");

            if (xmlFuzzyRules != null)
                _doc.RemoveXmlObject(xmlFuzzyRules);

            xmlFuzzyRules = _doc.CreateXmlObject(_xmlPS, "Mp.Controller.FuzzyRules", "");

            for (int i = 0; i < tree.ChildCount / 3; ++i)
            {
                XmlElement xmlRule = _doc.CreateXmlObject(xmlFuzzyRules, "Mp.Controller.FuzzyRule", "");

                evalExpresion((ITree)tree.Children[i*3 + 1], xmlRule);
                ITree thenNode = (ITree)tree.Children[i*3 + 2];
                evalExpresion(thenNode.GetChild(0), xmlRule);
            }

            XmlHelper.SetParam(_xmlPS, "fuzzyRulesText", "string", fuzzyRulesCtrl.Text);
        }

        private void evalExpresion(ITree node, XmlElement xmlParent)
        {
            if (node.Type == FuzzyRuleParser.STRING_LITERAL_UNI)
            {
                string var = node.Text.TrimStart('"').TrimEnd('"');
                XmlElement xmlFuzzyLingVar = _doc.CreateXmlObject(xmlParent, "Mp.Controller.FuzzyOperation", "");
                XmlHelper.SetParam(xmlFuzzyLingVar,"name", "string", var);
                XmlHelper.SetParamNumber(xmlFuzzyLingVar,"type", "uint8_t", 0);
            }
            else
            {
                XmlElement xmlFuzzyOperantion = _doc.CreateXmlObject(xmlParent, "Mp.Controller.FuzzyOperation", "");

                if (node.Type == FuzzyRuleParser.AND || node.Type == FuzzyRuleParser.UND)
                    XmlHelper.SetParamNumber(xmlFuzzyOperantion, "type", "uint8_t", 1);
                else
                    XmlHelper.SetParamNumber(xmlFuzzyOperantion, "type", "uint8_t", 2);

                evalExpresion(node.GetChild(0), xmlFuzzyOperantion);
                evalExpresion(node.GetChild(1), xmlFuzzyOperantion);
            }
        }

        private void SaveInputSignals()
        {
            XmlElement xmlLinguisticsVars = XmlHelper.GetChildByType(_xmlPS, "Mp.Controller.LinguisticVars");

            if (xmlLinguisticsVars != null)
                _doc.RemoveXmlObject(xmlLinguisticsVars);

            xmlLinguisticsVars = _doc.CreateXmlObject(_xmlPS, "Mp.Controller.LinguisticVars", "");

            foreach (DataGridViewRow row in channels.Rows)
            {
                XmlElement xmlSignal = (XmlElement)row.Tag;
                List<FuzzificationItem> items = (List<FuzzificationItem>)row.Cells[0].Tag;
                SaveLingVars(_xmlPS, items, XmlHelper.GetObjectID(xmlSignal));
            }
        }

        private void SaveOutputSignal()
        {
            XmlElement xmlOutSignal = null;

            if (_xmlOutSigList.ChildNodes.Count != 0)
                xmlOutSignal = (XmlElement)_xmlOutSigList.ChildNodes[0];
            else
                xmlOutSignal = _doc.CreateXmlObject(_xmlOutSigList, "Mp.Sig", "Mp.Controller.Sig.Fuzzy");

            XmlHelper.SetParam(xmlOutSignal, "name", "string", sigName.Text);
            XmlHelper.SetParam(xmlOutSignal, "unit", "string", outUnit.Text);
            XmlHelper.SetParam(xmlOutSignal, "comment", "string", outComment.Text);
            XmlHelper.SetParamDouble(xmlOutSignal, "samplerate", "double", Convert.ToDouble(outRate.Text));
            XmlHelper.SetParamDouble(xmlOutSignal, "physMin", "double", Convert.ToDouble(outMin.Text));
            XmlHelper.SetParamDouble(xmlOutSignal, "physMax", "double", Convert.ToDouble(outMax.Text));

            uint srcID = (uint)XmlHelper.GetParamNumber(_xmlPS, "sourceId");
            XmlHelper.SetParamNumber(xmlOutSignal, "sourceNumber", "uint32_t", (long)srcID);
            XmlHelper.SetParamNumber(xmlOutSignal, "valueDataType", "uint8_t", (int)SignalDataType.LREAL);

            XmlElement xmlLinguisticsVars = XmlHelper.GetChildByType(xmlOutSignal, "Mp.Controller.LinguisticVars");

            if (xmlLinguisticsVars != null)
                _doc.RemoveXmlObject(xmlLinguisticsVars);

            xmlLinguisticsVars = _doc.CreateXmlObject(xmlOutSignal, "Mp.Controller.LinguisticVars", "");


            SaveLingVars(xmlOutSignal, _outputLigVar, 0);
        }

        private void SaveLingVars(XmlElement xmlObj, List<FuzzificationItem> items, uint sigId)
        {
            XmlElement xmlLinguisticsVars = XmlHelper.GetChildByType(xmlObj, "Mp.Controller.LinguisticVars");

            foreach (FuzzificationItem item in items)
            {
                XmlElement xmlLingVar = _doc.CreateXmlObject(xmlLinguisticsVars, "Mp.Controller.LinguisticVar", "");
                XmlHelper.SetParam(xmlLingVar, "name", "string", item.ItemName);
                XmlHelper.SetParamDouble(xmlLingVar, "x1", "double", item.P1x);
                XmlHelper.SetParamDouble(xmlLingVar, "x2", "double", item.P2x);
                XmlHelper.SetParamDouble(xmlLingVar, "x3", "double", item.P3x);
                XmlHelper.SetParamDouble(xmlLingVar, "x4", "double", item.P4x);
                XmlHelper.SetParamNumber(xmlLingVar, "signal", "uint32_t", sigId);
            }
        }

        private void OnLingOutout_Click(object sender, EventArgs e)
        {
            double min = Convert.ToDouble(outMin.Text);
            double max = Convert.ToDouble(outMax.Text);

            string signalName = sigName.Text;

            if (outUnit.Text != null && outUnit.Text != "")
                signalName += " (" + outUnit.Text + ")";

            FuzzificationDlg dlg = new FuzzificationDlg(_outputLigVar, signalName, min, max, true);
            dlg.ShowDialog();
            UpdateOutputLingVars();
            ValidateRules();
        }

        private void Value_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            Control ctrl = (Control)sender;
            try
            {
                Convert.ToDouble(ctrl.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(ctrl, ex.Message);
                e.Cancel = true;
            }
        }

        private void outRate_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            Control ctrl = (Control)sender;
            try
            {
                uint value = Convert.ToUInt32(ctrl.Text);
                if (value == 0)
                {
                    errorProvider.SetError(ctrl, StringResource.ZeroErr);
                    e.Cancel = true;
                }

            }
            catch (Exception ex)
            {
                errorProvider.SetError(ctrl, ex.Message);
                e.Cancel = true;
            }
        }

        private void FuzzyControllerDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            _findDlg.Close();
            _replaceDlg.Close();

            FormStateHandler.Save(this, "Mp.Controller.FuzzyControllerDlg");
        }

        private void removeSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (channels.SelectedCells.Count == 0)
                return;

            int index = channels.SelectedCells[0].RowIndex;
            DataGridViewRow row = channels.Rows[index];
            channels.Rows.RemoveAt(index);
        }

        private void fuzzyRulesCtrl_TextChanged(object sender, EventArgs e)
        {
            _checkCodeTimer.Stop();
            ClearAllMarker();
            _checkCodeTimer.Start();
        }

        private void OnCheckCodeTimerTick(object sender, EventArgs e)
        {
            _checkCodeTimer.Stop();
            ValidateRules();
        }

        private void toolStripButtonCut_Click(object sender, EventArgs e)
        {
            ICSharpCode.TextEditor.Actions.Cut cut = new ICSharpCode.TextEditor.Actions.Cut();
            cut.Execute(fuzzyRulesCtrl.ActiveTextAreaControl.TextArea);

        }

        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            ICSharpCode.TextEditor.Actions.Copy copy = new ICSharpCode.TextEditor.Actions.Copy();
            copy.Execute(fuzzyRulesCtrl.ActiveTextAreaControl.TextArea);

        }

        private void toolStripButtonPaste_Click(object sender, EventArgs e)
        {
            ICSharpCode.TextEditor.Actions.Paste paste = new ICSharpCode.TextEditor.Actions.Paste();
            paste.Execute(fuzzyRulesCtrl.ActiveTextAreaControl.TextArea);
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            fuzzyRulesCtrl.ActiveTextAreaControl.TextArea.ClipboardHandler.Delete(null, null);
        }

        private void toolStripButtonUndo_Click(object sender, EventArgs e)
        {
            fuzzyRulesCtrl.Undo();
        }

        private void toolStripButtonRedo_Click(object sender, EventArgs e)
        {
            fuzzyRulesCtrl.Redo();
        }

        private void toolStripMenuItemSelectAll_Click(object sender, EventArgs e)
        {
            SelectWholeDocument sel = new SelectWholeDocument();
            sel.Execute(fuzzyRulesCtrl.ActiveTextAreaControl.TextArea);
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _findDlg.Show();
        }

        private void findAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _replaceDlg.Show();
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1600);
        }
    }
}
