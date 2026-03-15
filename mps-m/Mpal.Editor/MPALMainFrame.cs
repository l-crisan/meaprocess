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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Mp.Visual.Docking;
using Mp.Utils;
using Mpal.Compiler;
using Mpal.Debugger;
using Mpal.Model;

namespace Mpal.Editor
{
    public partial class MPALMainFrame : Form
    {
        private delegate void CallUpdateDebuggerStateDelegate();
        private OutputWindow _output = new OutputWindow();
        private ErrorListWindow _errorList = new ErrorListWindow();
        private DocumentForm _documentForm = new DocumentForm();
        private CallStackView _callStackView = new CallStackView();
        private DebugVariableView _variableView;
        private string _file;
        private MruStripMenuInline _mruMenu;
        private int _errorCount = 0;
        private bool _embeddedMode;
        static string _mruRegKey = "MruMenu";
        private int _curLine;
        private string _curFunc;
        private string _callStack;

        private string _title;
        private System.Windows.Forms.Timer _updateTimer = new System.Windows.Forms.Timer();
        public Mpal.Model.Unit _unit = new Mpal.Model.Unit();
        private bool _compiling = false;
        private Debugger.DebuggerSettings _debugerSettings = new DebuggerSettings();
        private Debugger.Debugger _debugger = new Mpal.Debugger.Debugger();
        private bool _debugging = false;
        private delegate void DebugerStoppedDelegate();
        private delegate void MessageDelegate(string msg);
        private VariableManager _variableManager;
        private OnTerminateDelegate _terminateEvent;
        private CompilerOptions _options;
                
        public MPALMainFrame(string file, bool embeddedMode)
        {
            _file = file;
            _embeddedMode = embeddedMode;
            
            InitializeComponent();

            SetMenuShortcutKeys();

            _documentForm.Unit = _unit;

            _variableView = new DebugVariableView(_unit);

            _documentForm.CheckCode += new OnCheckCode(OnCompileDocumentForm);
            _documentForm.InsertBreakPointEvent += new OnBreakPointChangedDelegate(OnInsertBreakPointEvent);
            _documentForm.EnableBreakPointEvent += new OnBreakPointChangedDelegate(OnEnableBreakPointEvent);
            _documentForm.DisableBreakPointEvent += new OnBreakPointChangedDelegate(OnDisableBreakPointEvent);
            _documentForm.RemoveBreakPointEvent += new OnBreakPointChangedDelegate(OnRemoveBreakPointEvent);
            _title = Text;            

            _mruMenu = new MruStripMenuInline(fileToolStripMenuItem, recentFileToolStripMenuItem, new MruStripMenu.ClickedHandler(OnMruFile), (RegistryKey + _mruRegKey + "\\MRU"), 16);
            _mruMenu.LoadFromRegistry();

            Properties.Settings.Default.Reload();

            string dockState = Mpal.Editor.Properties.Settings.Default.DockState;

            if (dockState != "")
            {
                try
                {
                    MemoryStream mm = new MemoryStream();
                    StreamWriter sw = new StreamWriter(mm);
                    sw.Write(dockState);
                    sw.Write(0);

                    sw.Flush();
                    mm.Flush();

                    mm.Seek(0, SeekOrigin.Begin);
                    dockPanel.LoadFromXml(mm, new DeserializeDockContent(GetContent), true);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else
            {
                _output.Show(dockPanel);
                _output.DockState = DockState.DockBottom;
                
                _callStackView.Show(dockPanel);
                _callStackView.DockState = DockState.DockBottom;
                _callStackView.Invalidate();

                _variableView.Show(dockPanel);
                _variableView.DockState = DockState.DockRight;

                _documentForm.Show(dockPanel);
                _documentForm.DockState = DockState.Document;                                

                _errorList.Show(dockPanel);
                _errorList.DockState = DockState.DockBottom;
            }
            _documentForm.TabText = StringResource.Unsaved;

            statusText.Text = StringResource.Ready;
            _output.Clear();
            _errorList.Clear();
            _output.OnLineDoubleClick += new LineDoubleClick(OnOutputLineDoubleClick);
            _errorList.OnErrorSelected += new ErrorSelected(OnErrorSelected);

            try
            {
                if (file != "")
                {
                    _documentForm.LoadFromFile(file);
                    _debugerSettings.InputVarValues.Clear();

                    OnCompileDocumentForm();
                    Text = _title + " - " + file;
                }
            }
            catch (Exception exc)
            {
                _output.WriteLine(String.Format(StringResource.FileLoadErr, file));
                _file = "";
                Console.WriteLine(exc.Message);
            }

            _updateTimer.Interval = 200;
            _updateTimer.Tick += new EventHandler(UpdateTimer_Tick);
            _updateTimer.Start();
            
            FormStateHandler.Restore(this, RegistryKey + "MPALMainFrame");

            _terminateEvent = new OnTerminateDelegate(CallDebuggerStopped);
            _debugger.TerminateEvent += _terminateEvent;
            _debugger.UpdateDebuggerStateEvent += new OnUpdateDebuggerStateDelegate(SetDebuggerState);
            _debugger.MessageEvent += new OnMessageDelegate(OnDebuggerMessageEvent);

            Properties.Settings.Default.Reload();

            UpdateCompOptions();

            unsafe { _options.VmAddressSize = (uint) sizeof(byte*); }

        }

        private void SetMenuShortcutKeys()
        {
            newToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            printToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.P;
            undoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Z;
            redoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Y;
            cutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.X;
            copyToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            pasteToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.V;
            selectAllToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;
            findToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F;
            findNextToolStripMenuItem.ShortcutKeys = Keys.F3;
            replaceToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.H;
            goToToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.G;
            outputToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.O;
            errorListToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.E;
            toolStripMenuItemBuild.ShortcutKeys = Keys.F6;
            addProgramToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.R;
            addFunctionToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Q;
            addFunctionBlockToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.B;
            variablesToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.W;
            callstackToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D;
            startDebugingToolStripMenuItem.ShortcutKeys = Keys.F5;
            stopDebuggingMenu.ShortcutKeys = Keys.Shift | Keys.F5;
            stepIntoToolStripMenuItem.ShortcutKeys = Keys.F11;
            stepOverToolStripMenuItem.ShortcutKeys = Keys.F10;
            toggleBreakpointToolStripMenuItem.ShortcutKeys = Keys.F9;
            deleteAllBreakpointsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F9;
            contentsToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F1;

        }

        private void OnDebuggerMessageEvent(string message)
        {
            this.Invoke(new OnMessageDelegate(CallOnMessage), new object[]{message});
        }

        private void CallOnMessage(string message)
        {
            _output.WriteLine(message);
        }

        private void OnDisableBreakPointEvent(BreakPoint bp)
        {
            if (!_debugging)
                return;

            _debugger.RemoveBreakPoint(new DebuggerBreakpoint(bp.Line, bp.Unit));
        }

        private void OnEnableBreakPointEvent(BreakPoint bp)
        {
            if (!_debugging)
                return;

            _debugger.InsertBreakPoint(new DebuggerBreakpoint(bp.Line, bp.Unit));            
        }

        private void OnRemoveBreakPointEvent(BreakPoint bp)
        {
            if (!_debugging)
                return;

            _debugger.RemoveBreakPoint(new DebuggerBreakpoint(bp.Line, bp.Unit));
        }

        private void OnInsertBreakPointEvent(BreakPoint bp)
        {
            if (!_debugging)
                return;

            _debugger.InsertBreakPoint(new DebuggerBreakpoint(bp.Line, bp.Unit));
        }


        private void SetDebuggerState(int line, string unit, string callStack)
        {
            _curLine = line;
            _curFunc = unit;
            _callStack = callStack;
            this.BeginInvoke(new CallUpdateDebuggerStateDelegate(UpdateDebuggerState));
        }

        private void UpdateVariableValueMarker(List<VariableInfo> variables,  Mpal.Model.Function function)
        {
            foreach (DictionaryEntry entry in function.Uid2Param)
            {
                ulong uid = (ulong) entry.Key;
                int line = (int) (uid>>32);
                int col = (int) (uid & 0xFFFFFFFFFFFFFF) -1;

                VariableInfo varInfo = _variableManager.GetValueByUID(uid);
                
                if (varInfo == null)
                    continue;

                switch (varInfo.DataType)
                {
                    case Mpal.Model.DataType.ARRAY:
                    break;

                    case Mpal.Model.DataType.UDT:
                    {
                        string name = varInfo.Name;
                        varInfo = varInfo.Variables[0];
                        string info = name + " := " + varInfo.Value;
                        _documentForm.AddMarker(line, col, info, ICSharpCode.TextEditor.Document.TextMarkerType.Invisible);
                    }

                    break;

                    case Mpal.Model.DataType.STRUCT:
                    break;

                    default:
                    {
                        string info = varInfo.Name + " := " + varInfo.Value;
                        _documentForm.AddMarker(line, col, info, ICSharpCode.TextEditor.Document.TextMarkerType.Invisible);
                    }
                    break;
                }
                
            }
            
            foreach (Parameter param in function.Parameters)
            {
                switch (param.ParamDataType)
                {
                    case DataType.ARRAY:
                    break;
                    case DataType.STRUCT:
                    break;
                    case DataType.UDT:
                    break;
                    default:
                    {
                        int line = (int)(param.UID >> 32);
                        int col = (int)(param.UID & 0xFFFFFFFFFFFFFF) - 1;
                        VariableInfo varInfo = _variableManager.GetValueByUID(param.UID);
                        string info = varInfo.Name + " := " + varInfo.Value; 
                        _documentForm.AddMarker(line, col, info, ICSharpCode.TextEditor.Document.TextMarkerType.Invisible);
                    }
                    break;

                }
            }

            _documentForm.RefreshText();

        }

        private void UpdateDebuggerState()
        {
            if (_curLine > -1)
                _documentForm.MoveDebugArrowToLine(_curLine - 1);                

            _documentForm.ClearAllMarker();

            if (_curFunc == null || _curFunc == "")
                return;

            Mpal.Model.Function func = (Mpal.Model.Function)_unit.Functions[_curFunc];

            if (_variableManager != null)
                _variableView.UpdateView(_variableManager.Variables, _curFunc);
            else
                _variableView.UpdateView(null, _curFunc);

            UpdateVariableValueMarker(_variableManager.Variables, func);

            _callStackView.CallStack = _callStack;
        }

        private void OnCompileDocumentForm()
        {
            if(!CompileInMemory())
                ScanInterface();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (_file != "")
            {
                if (_documentForm.Modified)
                    Text = _title + " - " + _file + "*";
                else
                    Text = _title + " - " + _file;
            }
            else
            {
                if (_documentForm.Modified)
                    Text = _title + " - " + StringResource.Unsaved;
            }
        }

        private DockContent GetContent(string padTypeName)
        {
            padTypeName = padTypeName.Split(',')[0];

            switch (padTypeName)
            {
                case "Mpal.Editor.OutputWindow":
                    return _output;
                case "Mpal.Editor.ErrorListWindow":
                    return _errorList;
                case "Mpal.Editor.DocumentForm":
                    return _documentForm;
                case "Mpal.Editor.DebugVariableView":
                    return _variableView;
                case "Mpal.Editor.CallStackView":
                    return _callStackView;
            }
            return null;
        }

        public string ProgramFile
        {
            get
            {
                return Path.ChangeExtension(_documentForm.FileName, ".mpp");
            }
        }

        private void OnErrorSelected(int line, int col)
        {
            _documentForm.SetCaret(line, col);
        }

        public static string RegistryKey
        {
            get { return "Atesion\\MPAL Editor\\"; }
        }

        private void OnMruFile(int number, String filename)
        {
            if (_debugging)
                return;

            _mruMenu.SetFirstFile(number);

            if (!CanCloseDocument())
                return;

            try
            {
                _documentForm.LoadFromFile(filename);
                _debugerSettings.InputVarValues.Clear();
                OnCompileDocumentForm();
                statusText.Text = "Ready";
                _output.Clear();
                _errorList.Clear();
                _file =  filename;
            }
            catch (Exception e)
            {
                _output.WriteLine(String.Format(StringResource.FileLoadErr, filename));
                Console.WriteLine(e.Message);
                _mruMenu.RemoveFile(number);
                _file = "";
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void outputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _output.Show(dockPanel);
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _documentForm.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _documentForm.Redo();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _documentForm.Delete();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _documentForm.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _documentForm.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _documentForm.Paste();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _documentForm.SelectAll();
        }

        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoToLineDlg dlg = new GoToLineDlg();

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            _documentForm.GoToLine(dlg.LineNo);
        }

        private void menuUpdateTimer_Tick(object sender, EventArgs e)
        {
            undoToolStripMenuItem.Enabled = _documentForm.CanUndo;
            undoToolStripButton.Enabled   = _documentForm.CanUndo;
            
            redoToolStripMenuItem.Enabled = _documentForm.CanRedo;
            redoToolStripButton.Enabled = _documentForm.CanRedo;

            cutToolStripMenuItem.Enabled = _documentForm.CanCut;
            cutToolStripButton.Enabled = _documentForm.CanCut;

            copyToolStripMenuItem.Enabled = _documentForm.CanCopy;
            copyToolStripButton.Enabled = _documentForm.CanCopy;

            deleteToolStripMenuItem.Enabled = _documentForm.CanDelete;
            
            toolStripStatusLabelLn.Text = _documentForm.Line.ToString();
            toolStripStatusLabelCol.Text = _documentForm.Column.ToString();
            pasteToolStripMenuItem.Enabled = !findBox.Focused;
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _documentForm.ShowSearchAndReplaceDlg();
        }

        private void findBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (findBox.Text == "")
                return;

            List<string> items = new List<string>();

            foreach (string item in findBox.Items)
            {
                if( item != findBox.Text)
                    items.Add(item);
            }

            findBox.Items.Clear();

            findBox.Items.Add(findBox.Text);

            foreach (string item in items)
                findBox.Items.Add(item);
           
           _documentForm.Find(findBox.Text);
        }

        private void findBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _documentForm.Find(findBox.Text);
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _documentForm.ShowSearchDlg();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file;
            if (_documentForm.FileName == "")
            {
                SaveFileDialog dlg = new SaveFileDialog();

                dlg.Filter = "*.mpal|*.mpal|*.*|*.*";

                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                file = dlg.FileName;
            }
            else
            {
                file = _documentForm.FileName;
            }

            try
            {                
                _documentForm.SaveToFile(file);
                _file = file;
                statusText.Text = StringResource.Saved;
                this.Text = _title + " - " + file;
                _mruMenu.AddFile(file);
            }
            catch (Exception ex)
            {
                //TODO: message
                Console.WriteLine(ex.Message);
            }

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "*.mpal|*.mpal|*.*|*.*";
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                _documentForm.SaveToFile(dlg.FileName);
                _mruMenu.AddFile(dlg.FileName);
                _file = dlg.FileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //TODO: message
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CanCloseDocument())
                return;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "*.mpal|*.mpal|*.*|*.*";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            try
            {                
                _documentForm.LoadFromFile(dlg.FileName);
                _debugerSettings.InputVarValues.Clear();
                OnCompileDocumentForm();
                statusText.Text = "Ready";
                _file = dlg.FileName;            
                _output.Clear();
                _errorList.Clear();
            }
            catch(Exception ex)
            {
                _file = "";
                Console.WriteLine(ex.Message);
                _output.WriteLine(String.Format(StringResource.FileLoadErr, dlg.FileName)); 
            }            
        }

        private bool CanCloseDocument()
        {
            if (!_documentForm.Modified)
                return true;

            DialogResult res = MessageBox.Show(StringResource.SaveChanges, this.Text, MessageBoxButtons.YesNoCancel,
                                                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            try
            {
                switch (res)
                {
                    case DialogResult.Yes:
                        saveToolStripMenuItem_Click(null, null);
                    break;
                    case DialogResult.Cancel:
                        return false;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CanCloseDocument())
                return;

            _documentForm.New("");
            _file = "";
            statusText.Text = StringResource.Ready;
            _output.Clear();
            _errorList.Clear();
        }

        private void MainFrame_FormClosed(object sender, FormClosedEventArgs e)
        {
            using (MemoryStream mm = new MemoryStream())
            {
                dockPanel.SaveAsXml(mm, Encoding.UTF8,true);

                mm.Flush();
                mm.Seek(0, SeekOrigin.Begin);

                StreamReader sr = new StreamReader(mm);
                string ss = sr.ReadToEnd();
                Properties.Settings.Default.DockState = ss;
                Properties.Settings.Default.Save();
            }
            _mruMenu.SaveToRegistry();
        }

        private void OnCompileClicked(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            _compiling = true;
            _output.Clear();
            _errorList.Clear();

            try
            {
                saveToolStripMenuItem_Click(null, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                this.Cursor = Cursors.Arrow;
                //return;
            }

            _documentForm.ClearAllMarker();

            if (Compile())
            {
                try
                {                    
                    string file = Path.ChangeExtension(_documentForm.FileName, ".mpp");
                    using (FileStream stream = File.Create(file))
                    {
                        _unit.Serialise(stream);
                        stream.Flush();
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    _output.WriteLine(ex.Message);
                }

                _output.WriteLine("");
                _output.WriteLine(StringResource.BuildSucceeded + "!");
                statusText.Text = StringResource.BuildSucceeded;
            }
            else
            {
                _output.WriteLine("");
                _output.WriteLine(String.Format(StringResource.CompileComplete, _errorCount.ToString()));                
                statusText.Text = StringResource.BuildFailed;
            }

            _compiling = false;
            this.Cursor = Cursors.Arrow;
        }

        private bool Compile()
        {
            if (_documentForm.FileName == "" || _documentForm.FileName == null)
                return false;                       

            string ext = Path.GetExtension(_documentForm.FileName).ToUpper();

            if (ext != ".MPAL")
            {
                _output.WriteLine(String.Format(StringResource.UnknowFileType, _documentForm.FileName));
                statusText.Text = StringResource.BuildFailed;
                this.Cursor = Cursors.Arrow;
                //return;
            }

            //Delete the old binary file
            string binaryFile = Path.ChangeExtension(_documentForm.FileName, ".mpp");
            try
            {
                File.Delete(binaryFile);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            Compiler.Compiler comp = new Mpal.Compiler.Compiler("", "");            

            comp.OnMessage += new Mpal.Model.Message(OnCompilerMessage);

            _errorCount = 0;


            UpdateCompOptions();

            if (!comp.Compile(_documentForm.FileName, _unit, _options)) 
                return false;

            return (_errorCount == 0);
        }


        private bool CompileInMemory()
        {
            Compiler.Compiler comp = new Mpal.Compiler.Compiler("", "");

            comp.OnMessage += new Mpal.Model.Message(OnCompilerMessage);

            _errorCount = 0;
            UpdateCompOptions();

            return comp.CompileText(_documentForm.DocumentText, _unit, _options);
        }

        private void UpdateCompOptions()
        {
            uint machineWordSize = 4;

            if (Properties.Settings.Default.TargetMachine == 1)
                machineWordSize = 8;

            _options = new CompilerOptions(machineWordSize, Properties.Settings.Default.SupportLREAL,
                Properties.Settings.Default.SupportINT64, Properties.Settings.Default.SupportMathLIB);
        }

        private bool ScanInterface()
        {
            Compiler.Compiler comp = new Mpal.Compiler.Compiler("", "");
            return comp.ScanInterface(_documentForm.DocumentText, _unit);
        }

        private void OnCompilerMessage(string msg)
        {
            _errorCount++;
            string[] message;
            if (_compiling)
            {
                _output.WriteLine(msg);
                message = _errorList.WriteLine(msg);
            }
            else
            {
                message = ErrorListWindow.GetErrorInfo(msg);
            }

            try
            {
                int line = Convert.ToInt32(message[3]);
                int col = Convert.ToInt32(message[4]);

                _documentForm.AddMarker(line, col, message[0] + ": " + message[1], ICSharpCode.TextEditor.Document.TextMarkerType.WaveLine);
                _documentForm.RefreshText();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnOutputLineDoubleClick(string line)
        {
            int startIdx = line.IndexOf('(');
            int endIdx = line.IndexOf(')');

            if (startIdx == -1 || endIdx == -1)
                return;

            string posInfo = line.Substring(startIdx + 1, endIdx - startIdx - 1);

            string[] array = posInfo.Split(',');

            if (array.Length != 2)
                return;
            
            _documentForm.SetCaret( Convert.ToInt32(array[0]), Convert.ToInt32(array[1]));
        }

        private void aboutMPALEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutDlg = new AboutBox();
            aboutDlg.ShowDialog();
        }

        private void statusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _errorList.Show(dockPanel);
        }

        private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _documentForm.Find(findBox.Text);
        }

        private void MainFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CanCloseDocument();

            if (e.Cancel)
                return;

            if (_debugging)
            {
                DialogResult res =  MessageBox.Show( StringResource.DebuggerRunMsg, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if ( res != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }                              
            }

            _debugger.TerminateEvent -= _terminateEvent; // No Event 
            _debugger.Terminate();
            _documentForm.Exit();
            Properties.Settings.Default.Save();
            FormStateHandler.Save(this, RegistryKey + "MPALMainFrame");            
        }

        private void addProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddProgramDlg dlg = new AddProgramDlg();
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            _documentForm.Insert(dlg.ProgramCode);
        }

        private void addFunctionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFunctionDlg dlg = new AddFunctionDlg();

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            _documentForm.Insert(dlg.FunctionCode);
        }

        private void addFunctionBlockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFBDlg dlg = new AddFBDlg();

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            _documentForm.Insert(dlg.ProgramCode);
        }

        private void cleanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsDlg dlg = new OptionsDlg();
            
            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;

            _documentForm.UpdateOptions();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            using (PrintDialog dlg = new PrintDialog())
            {
                dlg.Document = _documentForm.PrintDoc;
                dlg.UseEXDialog = true;
                dlg.AllowSelection = true;
                dlg.AllowSomePages = true;
                dlg.AllowCurrentPage = true;

                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

                dlg.Document.Print();
            }
        }

        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PageSetupDialog dlg = new PageSetupDialog();
            dlg.Document = _documentForm.PrintDoc;

            dlg.ShowDialog();
        }

        private void printPreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog dlg = new PrintPreviewDialog();           
            dlg.Document = _documentForm.PrintDoc;
            
            dlg.ShowDialog();
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "mpalhelp.chm";
            System.Windows.Forms.Help.ShowHelp(this, file, HelpNavigator.Topic);
        }

        private void webToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.atesion.de");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void toggleBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _documentForm.ToggleBreakPoint();
        }

        private void deleteAllBreakpointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _documentForm.DeleteAllBreakPoints();
        }

        private void disableAllBreakpointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _documentForm.DisableAllBreakPoints();
        }

        private void startDebugging(object sender, EventArgs e)
        {
            if (!_debugging)
            {
                _output.Clear();
                _errorList.Clear();
                _callStackView.CallStack = "";
                _variableView.UpdateView(null,"");

                Hashtable units = new Hashtable();

                OnCompileClicked(null, null);

                if (_errorCount != 0)
                    return;

                if (_unit.Program == null)
                {
                    MessageBox.Show(StringResource.NoProgErr, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DebuggerSettingsDlg dlg = new DebuggerSettingsDlg(_debugerSettings, _unit);
                dlg.ServerIP = Properties.Settings.Default.DebuggerServerIP;
                dlg.ServerPort = (uint) Properties.Settings.Default.DebuggerServerPort;

                dlg.BuildInDebugger = Properties.Settings.Default.UseBuildInDebugger;
                dlg.MemSize = Properties.Settings.Default.VmMemSize;

                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                Properties.Settings.Default.DebuggerServerIP = dlg.ServerIP;
                Properties.Settings.Default.DebuggerServerPort = (int) dlg.ServerPort;

                Properties.Settings.Default.UseBuildInDebugger = dlg.BuildInDebugger;
                Properties.Settings.Default.VmMemSize = dlg.MemSize;

                try
                {
                    _debugger.Setup( Properties.Settings.Default.DebuggerServerIP, Properties.Settings.Default.DebuggerServerPort,
                                     Properties.Settings.Default.UseBuildInDebugger, Properties.Settings.Default.VmMemSize);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    MessageBox.Show(StringResource.ConnectDebuggerError, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _debugerSettings.BreakPoints.Clear();

                foreach (BreakPoint bp in _documentForm.BreakPoints)
                {
                    if(bp.Enabled)
                        _debugerSettings.BreakPoints.Add(new Debugger.DebuggerBreakpoint(bp.Line, bp.Unit));
                }

                string mppFile = Path.ChangeExtension(_documentForm.FileName, ".mpp");
                _variableManager = _debugger.Load(_unit, mppFile);
                try
                {
                    _output.Clear();
                    _debugger.Start(_debugerSettings);
                    _debugging = true;

                    startDebugingToolStripMenuItem.Enabled = true;

                    startDebugingToolStripMenuItem.Text = StringResource.Continue;
                    debuggingStart.Enabled = true;
                    debuggingStart.Text = StringResource.Continue;

                    toolStripButtonTerminateDebug.Enabled = true;
                    stopDebuggingMenu.Enabled = true;
                    stepIntoToolStripMenuItem.Enabled = true;
                    stepOverToolStripMenuItem.Enabled = true;
                    _documentForm.TextCtrl.Document.ReadOnly = true;
                    toolStripButtonStepOver.Enabled = true;
                    toolStripButtonStepInto.Enabled = true;
                    toolStripButtonNew.Enabled = false;
                    toolStripButtonOpen.Enabled = false;
                    toolStripButtonBuild.Enabled = false;
                    newToolStripMenuItem.Enabled = false;
                    openToolStripMenuItem.Enabled = false;
                    toolStripMenuItemBuild.Enabled = false;
                    sourceToolStripMenuItem.Enabled = false;
                    statusText.Text = StringResource.Debugging;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    MessageBox.Show(StringResource.ConnectDebuggerError, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _debugger.Terminate();
                }
            }
            else
            {
                _documentForm.MoveDebugArrowToLine(-1);
                _debugger.Continue();
            }

            _variableView.UpdateView(null,"");
        }

        private void CallDebuggerStopped()
        {
            this.Invoke(new OnTerminateDelegate(DebuggerStopped));
        }

        private void DebuggerStopped()
        {
            _debugging = false;            
            startDebugingToolStripMenuItem.Text = StringResource.StartDebugging;
            debuggingStart.Text = StringResource.StartDebugging;
            debuggingStart.Enabled = true;

            toolStripButtonTerminateDebug.Enabled = false;
            toolStripButtonNew.Enabled = true;
            stopDebuggingMenu.Enabled = false;
            stepIntoToolStripMenuItem.Enabled = false;
            stepOverToolStripMenuItem.Enabled = false;
            _documentForm.TextCtrl.Document.ReadOnly = false;
            toolStripButtonStepOver.Enabled = false;
            toolStripButtonStepInto.Enabled = false;
            toolStripButtonOpen.Enabled = true;
            toolStripButtonBuild.Enabled = true;
            newToolStripMenuItem.Enabled = true;
            openToolStripMenuItem.Enabled = true;
            toolStripMenuItemBuild.Enabled = true;
            sourceToolStripMenuItem.Enabled = true;
            statusText.Text = StringResource.Ready;
            _documentForm.MoveDebugArrowToLine(-1);

        }

        private void stepIntoToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            _debugger.StepInto();
        }

        private void stepOverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _debugger.StepOver();
        }

        private void variablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _variableView.Show(dockPanel);
        }

        private void callstackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _callStackView.Show(dockPanel);
        }

        private void toolStripButtonTerminateDebug_Click(object sender, EventArgs e)
        {
            _debugger.Terminate();
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void buildAndExportAsHEXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            _compiling = true;
            _output.Clear();
            _errorList.Clear();

            try
            {
                saveToolStripMenuItem_Click(null, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                this.Cursor = Cursors.Arrow;
                //return;
            }

            _documentForm.ClearAllMarker();

            if (Compile())
            {
                _output.WriteLine("");
                _output.WriteLine(StringResource.BuildSucceeded + "!");
                statusText.Text = StringResource.BuildSucceeded;

                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "*.h|*.h|*.hex|*.hex";

                if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(dlg.FileName))
                    {
                        MemoryStream mm = new MemoryStream();
                        _unit.Strip();
                        _unit.Serialise(mm);
                        mm.Seek(0, SeekOrigin.Begin);
                        BinaryReader br = new BinaryReader(mm);

                        if (Path.GetExtension(dlg.FileName) == ".h")
                        {
                            sw.WriteLine("//Generated with Atesion GmbH MPAL-Editor.(" + DateTime.Now.ToString() + ")");

                            sw.WriteLine("#ifndef " + Properties.Settings.Default.ExpIncGuardName);
                            sw.WriteLine("#define " + Properties.Settings.Default.ExpIncGuardName);
                            sw.WriteLine();

                            if(Properties.Settings.Default.ExpAsStatic)
                                sw.Write("static ");
                            
                            if( Properties.Settings.Default.ExpAsConst)
                                sw.Write("const ");

                             sw.Write("unsigned char " +  Properties.Settings.Default.ExpVarName + "[] = {");

                            for (int i = 0; i < mm.Length; ++i)
                            {
                                if (i % Properties.Settings.Default.ExpNewLineAfter == 0 && (i != 0))
                                    sw.WriteLine();

                                sw.Write("0x" + mm.ReadByte().ToString("X2") + ",");
                            }

                            sw.Write("0x00 ");
                            sw.WriteLine("};");
                            sw.WriteLine();

                            sw.WriteLine("#endif");
                            sw.WriteLine();
                            sw.Close();
                        }
                        else
                        {

                            for (int i = 0; i < mm.Length; ++i)
                            {
                                if (i % Properties.Settings.Default.ExpNewLineAfter == 0 && (i != 0))
                                    sw.WriteLine();

                                sw.Write(Properties.Settings.Default.ExpBytePrefix + mm.ReadByte().ToString("X2") + Properties.Settings.Default.ExpBytePostfix);
                            }

                            sw.Close();
                        }
                    }

                    _unit.Clear();
                    Compile();
                    
                }
            }
            else
            {
                _output.WriteLine("");
                _output.WriteLine(String.Format(StringResource.CompileComplete, _errorCount.ToString()));
                statusText.Text = StringResource.BuildFailed;
            }

            _compiling = false;
            this.Cursor = Cursors.Arrow;
        }
    }
}
