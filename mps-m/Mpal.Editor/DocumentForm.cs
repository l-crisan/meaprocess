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
using System.Drawing.Printing;
using System.Windows.Forms;
using System.IO;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Actions;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using Mp.Visual.Docking;
using Mp.Visual.TextEditor;
using Mp.Visual.TextEditor.FindAndReplace;
using Mpal.Model;


namespace Mpal.Editor
{
    public delegate void OnCheckCode();
    public delegate void OnBreakPointChangedDelegate(BreakPoint bp);

    internal partial class DocumentForm : DockContent
    {
        private FindReplaceDlg _replaceDlg;
        private FindDlg        _findDlg;
        private Engine          _findAndReplaceEngine;
        private bool _modified = false;
        private bool _canClose = false;
        private List<TextMarker> _marker = new List<TextMarker>();
        public event OnCheckCode CheckCode;
        public event OnBreakPointChangedDelegate InsertBreakPointEvent;
        public event OnBreakPointChangedDelegate RemoveBreakPointEvent;
        public event OnBreakPointChangedDelegate EnableBreakPointEvent;
        public event OnBreakPointChangedDelegate DisableBreakPointEvent;
        private Unit _unit;
        private System.Windows.Forms.Timer _completationTimer = new Timer();
        private System.Windows.Forms.Timer _checkCodeTimer = new Timer();
        private CodeCompletionProvider _codeComplProvider;
        private List<BreakPoint> _breakPoints = new List<BreakPoint>();
        private DebugArrow _debugArrow;
        private delegate void SetArrowToLineDelegate(int line);


        public void Exit()
        {
            _canClose = true;
        }

        public DocumentForm()
        {
            InitializeComponent();

            cutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.X;
            copyToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            pasteToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.V;
            deleteToolStripMenuItem.ShortcutKeys = Keys.Delete;
            selectAllToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;

            //Code completation provider            
            ImageList imageList = new ImageList();
            imageList.Images.Add(Resource.Variable);
            imageList.Images.Add(Resource.Struct);

            _codeComplProvider = new CodeCompletionProvider(imageList);

            //Syntax highlighting
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            FileSyntaxModeProvider fsmProvider; // Provider
            if (Directory.Exists(dir))
            {
                fsmProvider = new FileSyntaxModeProvider(dir); // Create new provider with the highlighting directory.
                HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider); // Attach to the text editor.
                textCtrl.SetHighlighting("MPAL"); // Activate the highlighting, use the name from the SyntaxDefinition node.
            }           

            //Find and replace
            _findAndReplaceEngine = new Engine();
            _replaceDlg = new FindReplaceDlg(_findAndReplaceEngine);
            _findDlg = new FindDlg(_findAndReplaceEngine);
            
            //Folding
            textCtrl.Document.FoldingManager.FoldingStrategy = new Folding();
            textCtrl.Document.FoldingManager.UpdateFoldings(null, null);
                        
            //Options
            UpdateOptions();

            //Code Check & Completation
            textCtrl.ActiveTextAreaControl.TextArea.MouseDown += new MouseEventHandler(TextArea_MouseDown);

            _checkCodeTimer.Interval = 700;
            _checkCodeTimer.Tick += new EventHandler(OnCheckCodeTimerTick);

            _completationTimer.Interval = 700;
            _completationTimer.Tick += new EventHandler(OnCompletationTimerTick);
          
            //BreakPoints
            textCtrl.ActiveTextAreaControl.TextArea.Paint += new PaintEventHandler(TextArea_Paint); 
   
            //Debug Arrow
            textCtrl.ActiveTextAreaControl.TextArea.IconBarMargin.MouseDown += new MarginMouseEventHandler(IconBarMargin_MouseDown);

            _debugArrow = new DebugArrow(textCtrl);            
        }

        private void IconBarMargin_MouseDown(AbstractMargin sender, Point mousepos, MouseButtons mouseButtons)
        {
            int line = textCtrl.ActiveTextAreaControl.TextArea.TextView.GetLogicalLine(mousepos.Y);
            if (line < 0)
                return;
            textCtrl.ActiveTextAreaControl.Caret.Line = line;

            ToggleBreakPoint();
        }

        public List<BreakPoint> BreakPoints
        {
            get { return _breakPoints; }
        }


        public void MoveDebugArrowToLine(int line)
        {
            textCtrl.ActiveTextAreaControl.Caret.Line = line;
            _debugArrow.MoveToLine(line);

        }

        private void TextArea_Paint(object sender, PaintEventArgs e)
        {
            int maxLines = textCtrl.Document.TotalNumberOfLines;

            int line = textCtrl.ActiveTextAreaControl.Caret.Line + 1;
            System.Drawing.Drawing2D.SmoothingMode oldMode = e.Graphics.SmoothingMode;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            bool remove = false;
            bool invalidate = false;

            for (int i = 0; i < _breakPoints.Count; ++i)
            {
                BreakPoint bp = _breakPoints[i];
                Function function = _unit.GetFunctionByLine(bp.Line);

                if (function != null)
                {
                    if(!function.CanSetBreakPoint(bp.Line))
                        remove = true;
                }
                else
                {
                    remove = true;
                }

                if (bp.Line >= maxLines)
                    remove = true;

                if( remove )
                {
                    _breakPoints.RemoveAt(i);
                    --i;
                    remove = false;
                    invalidate = true;
                    continue;
                }
                
                bp.Draw(e.Graphics);
            }

            if( invalidate)
                textCtrl.ActiveTextAreaControl.TextArea.Invalidate();

            _debugArrow.Draw(e.Graphics);
            e.Graphics.SmoothingMode = oldMode;
        }

        public void DeleteAllBreakPoints()
        {
            _breakPoints.Clear();
            textCtrl.ActiveTextAreaControl.TextArea.Invalidate();
        }

        public void DisableAllBreakPoints()
        {
            foreach (BreakPoint bp in _breakPoints)
            {
                bp.Enabled = false;
                
                if(DisableBreakPointEvent != null)
                    DisableBreakPointEvent(bp);
            }

            textCtrl.ActiveTextAreaControl.TextArea.Invalidate();
        }

        public void InsertBreakPoint()
        {
            int line = textCtrl.ActiveTextAreaControl.Caret.Line;

            InsertBreakPoint(line);
        }

        private void InsertBreakPoint(int line)
        {
            if (_unit == null)
                return;

            line++;
            Function function = _unit.GetFunctionByLine(line);

            if (function == null)
                return;

            if (!function.CanSetBreakPoint(line))
                return;

            BreakPoint bp = GetBreakPointAtLine(line);

            if (bp != null)
                return;

            BreakPoint newBp = new BreakPoint(textCtrl, line, function.Name);
            _breakPoints.Add(newBp);

            textCtrl.ActiveTextAreaControl.TextArea.Invalidate();

            if (InsertBreakPointEvent != null)
                InsertBreakPointEvent(newBp);
        }

        public void RemoveBreakPoint()
        {
            int line = textCtrl.ActiveTextAreaControl.Caret.Line;
            RemoveBreakPoint(line);
        }

        private void RemoveBreakPoint(int line)
        {
            line++;

            for (int i = 0; i < _breakPoints.Count; ++i)
            {
                BreakPoint bp = _breakPoints[i];
                if (bp.Line == line)
                {
                    _breakPoints.RemoveAt(i);
                    
                    if (RemoveBreakPointEvent != null)
                        RemoveBreakPointEvent(bp);

                    --i;
                    break;
                }
            }

            textCtrl.ActiveTextAreaControl.TextArea.Invalidate();
        }

        public void ToggleBreakPoint()
        {
            int line = textCtrl.ActiveTextAreaControl.Caret.Line;
            
            BreakPoint breakPoint = null;

            foreach (BreakPoint bp in _breakPoints)
            {
                if ((bp.Line - 1) == line)
                {
                    breakPoint = bp;
                    break;
                }
            }

            if (breakPoint != null)
                RemoveBreakPoint(line);
            else
                InsertBreakPoint(line);
        }

        void TextArea_MouseDown(object sender, MouseEventArgs e)
        {
            _completationTimer.Stop();
            _checkCodeTimer.Stop();
        }

        public Unit Unit
        {
            set 
            { 
                _unit = value;
                _codeComplProvider.Unit = value;
            }
        }

        
		private string m_fileName = string.Empty;

        public PrintDocument PrintDoc
        {
            get { return textCtrl.PrintDocument; }
        }

		public string FileName
		{
			get	{	return m_fileName;	}
			set
			{
                m_fileName = value;

                if (value != string.Empty)
                    this.TabText = Path.GetFileName(m_fileName);
				
				this.ToolTipText = value;
			}
		}

        public void UpdateOptions()
        {
            textCtrl.ShowEOLMarkers = Mpal.Editor.Properties.Settings.Default.ShowEndOfLineMarkers;
            textCtrl.ShowHRuler = Mpal.Editor.Properties.Settings.Default.ShowHRuler;
            textCtrl.ShowLineNumbers = Mpal.Editor.Properties.Settings.Default.ShowLineNumbers;
            textCtrl.ShowSpaces = Mpal.Editor.Properties.Settings.Default.ShowSpaces;
            textCtrl.ShowTabs = Mpal.Editor.Properties.Settings.Default.ShowTabs;
            textCtrl.ShowVRuler = Mpal.Editor.Properties.Settings.Default.ShowVRuler;
            textCtrl.EnableFolding = Mpal.Editor.Properties.Settings.Default.EnableFolding;
            
            if (Mpal.Editor.Properties.Settings.Default.FullRowLineSel)
                textCtrl.LineViewerStyle = LineViewerStyle.FullRow;
            else
                textCtrl.LineViewerStyle = LineViewerStyle.None;

            textCtrl.Invalidate();
            
        }

        public void Insert(string str)
        {            
            int line = textCtrl.ActiveTextAreaControl.Caret.Line;
            int col = textCtrl.ActiveTextAreaControl.Caret.Column;
            int offset = textCtrl.ActiveTextAreaControl.Caret.Offset;
            
            textCtrl.Document.Insert( offset, str);
            LineSegment seg = textCtrl.Document.GetLineSegmentForOffset(offset+str.Length);
            SetCaret(seg.LineNumber+1, col + str.Length);
            _modified = true;
            textCtrl.Document.FoldingManager.UpdateFoldings(null, null);
        }

        public void New(string template)
        {
            m_fileName = "";
            this.TabText = "Unsaved*";
            textCtrl.Document.TextContent = template;
            int line = textCtrl.ActiveTextAreaControl.Caret.Line;
            int col = textCtrl.ActiveTextAreaControl.Caret.Column;

            int offset = textCtrl.Document.PositionToOffset(new TextLocation(col, line));

            LineSegment seg = textCtrl.Document.GetLineSegmentForOffset(offset + template.Length);
            SetCaret(seg.LineNumber, 0);
            _modified = true;
            textCtrl.Document.FoldingManager.UpdateFoldings(null, null);
        }

        public void LoadFromFile(string file)
        {            
            if (file != "")
            {
                if (!File.Exists(file))
                    throw new Exception("File not found");

                textCtrl.LoadFile(file);
                _modified = false;
            }           

            FileName = file;
        }

        public bool Modified
        {
            get { return _modified; }
        }

        public void GoToLine(int line)
        {
            textCtrl.ActiveTextAreaControl.Caret.Line = line - 1;
        }

        public bool CanCopy
        {
            get
            {
                return textCtrl.ActiveTextAreaControl.SelectionManager.HasSomethingSelected;
            }
        }

        public void ClearAllMarker()
        {
            foreach (TextMarker marker in _marker)
                textCtrl.Document.MarkerStrategy.RemoveMarker(marker);

            _marker.Clear();

            textCtrl.ActiveTextAreaControl.Refresh();
        }

        public void AddMarker(int line, int col, string text,TextMarkerType type)
        {
            int offset = textCtrl.Document.PositionToOffset(new TextLocation(col, line - 1));
            int lenght = Math.Min(textCtrl.Document.TextLength, offset + 100);
            string str = textCtrl.Document.GetText(offset, lenght - offset);
            string[] array = str.Split(new char[] { ' ', '\n', '\r', '\t', '\b', '(', ')','{', '}', '[',']', ',', ';', '+',
                                                    '-','*','/',':','=', '<','>'});
            
            if( array.Length == 0)
                return;

            lenght = array[0].Length;

            TextMarker marker = new TextMarker(offset, lenght, type);
            marker.ToolTip = text;
            _marker.Add(marker);
            textCtrl.Document.MarkerStrategy.AddMarker(marker);
        }

        public void RefreshText()
        {
            textCtrl.ActiveTextAreaControl.Refresh();
        }


        public string DocumentText
        {
            get             
            {
                if (textCtrl.Document.TextLength == 0)
                    return "";

                return textCtrl.Document.GetText(0, textCtrl.Document.TextLength); 
            }
        }

        public void ShowSearchAndReplaceDlg()
        {
            _replaceDlg.Show();
            _replaceDlg.Focus();            
        }

        public void ShowSearchDlg()
        {
            _findDlg.Show();
            _findDlg.Focus();
        }

        public bool CanUndo
        {
            get
            {
                return textCtrl.Document.UndoStack.CanUndo;
            }
        }

        public void Find(string pattern)
        {
            if (pattern == "")
                return;

            _findAndReplaceEngine.Find(pattern);
        }

        public void Cut()
        {
            ICSharpCode.TextEditor.Actions.Cut cut = new ICSharpCode.TextEditor.Actions.Cut();
            cut.Execute(textCtrl.ActiveTextAreaControl.TextArea);
        }

        public void SelectAll()
        {
            SelectWholeDocument sel = new SelectWholeDocument();
            sel.Execute(textCtrl.ActiveTextAreaControl.TextArea);                      
        }

        public void SetCaret(int line, int col)
        {
            textCtrl.ActiveTextAreaControl.Caret.Line = line- 1;
            textCtrl.ActiveTextAreaControl.Caret.Column = col;
            textCtrl.Select();
            textCtrl.Focus();
        }

        public void Copy()
        {
            ICSharpCode.TextEditor.Actions.Copy copy = new ICSharpCode.TextEditor.Actions.Copy();
            copy.Execute(textCtrl.ActiveTextAreaControl.TextArea);
        }

        public void Paste()
        {
            ICSharpCode.TextEditor.Actions.Paste paste = new ICSharpCode.TextEditor.Actions.Paste();
            paste.Execute(textCtrl.ActiveTextAreaControl.TextArea);
        }

        public void Delete()
        {
            textCtrl.ActiveTextAreaControl.TextArea.ClipboardHandler.Delete(null,null);
        }

        public void Undo()
        {
            textCtrl.Undo();
        }

        public void Redo()
        {
            textCtrl.Redo();
        }

        public bool CanRedo
        {
            get
            {
                return textCtrl.Document.UndoStack.CanRedo;
            }
        }

        public bool CanCut
        {
            get
            {
                return textCtrl.ActiveTextAreaControl.SelectionManager.HasSomethingSelected;
            }
        }

        public bool CanDelete
        {
            get
            {
                return textCtrl.ActiveTextAreaControl.SelectionManager.HasSomethingSelected;
            }
        }

		// workaround of RichTextbox control's bug:
		// If load file before the control showed, all the text format will be lost
		// re-load the file after it get showed.
		private bool m_resetText = true;
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (m_resetText)
			{
				m_resetText = false;
				FileName = FileName;
			}
		}

		protected override string GetPersistString()
		{
			return GetType().ToString() + "," + FileName + "," + Text;
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged (e);
			if (FileName == string.Empty)
				this.textCtrl.Text = Text;            
		}

        public int Line
        {
            get { return textCtrl.ActiveTextAreaControl.Caret.Line + 1; }
        }

        public int Column
        {
            get { return textCtrl.ActiveTextAreaControl.Caret.Column + 1; }
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            helpToolStripMenuItem.Enabled = CanCut;
            cutToolStripMenuItem.Enabled = CanCut;
            copyToolStripMenuItem.Enabled = CanCopy;
            deleteToolStripMenuItem.Enabled = CanDelete;

            int line = textCtrl.ActiveTextAreaControl.Caret.Line;
            
            BreakPoint curBP = GetBreakPointAtLine(line+1);

            if (curBP != null)
            {
                insertBPToolStripMenuItem.Enabled = false;
                deleteBPToolStripMenuItem.Enabled = true;
                enableBPToolStripMenuItem.Enabled = !curBP.Enabled;
                disableBPToolStripMenuItem.Enabled = curBP.Enabled;
            }
            else
            {
                insertBPToolStripMenuItem.Enabled = true;
                deleteBPToolStripMenuItem.Enabled = false;
                enableBPToolStripMenuItem.Enabled = false;
                disableBPToolStripMenuItem.Enabled = false;
            }
        }

        private BreakPoint GetBreakPointAtLine(int line)
        {
            foreach (BreakPoint bp in _breakPoints)
            {
                if (bp.Line == line)
                    return bp;
            }
            return null;
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAll();
        }

        private void textCtrl_TextChanged(object sender, EventArgs e)
        {
            _completationTimer.Stop();
            _checkCodeTimer.Stop();

            textCtrl.Document.FoldingManager.UpdateFoldings(null, null);
            _modified = true;
            
            ClearAllMarker();

            _checkCodeTimer.Start();

            if (textCtrl.Document.TextLength == 0)
                return;
            
            _completationTimer.Start();
        }

        private void OnCheckCodeTimerTick(object sender, EventArgs e)
        {
            _checkCodeTimer.Stop();

            if (CheckCode != null)
                CheckCode();
        }

        private void OnCompletationTimerTick(object sender, EventArgs e)
        {
            _completationTimer.Stop();
             CodeCompletionWindow.ShowCompletionWindow(this, textCtrl, FileName, _codeComplProvider, '.');
        }
      


        public ICSharpCode.TextEditor.TextEditorControl TextCtrl
        {
            get { return textCtrl; } 
        }

        public void SaveToFile(string file)
        {
            FileStream stream = File.Create(file);
            textCtrl.SaveFile(stream);
            m_fileName = file;
            this.TabText = Path.GetFileName(m_fileName);
            this.ToolTipText = file;
            _modified = false;
            stream.Close();
        }

        private void DocumentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !_canClose;
        }

        private void textCtrl_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            string text = textCtrl.ActiveTextAreaControl.SelectionManager.SelectedText;

            if( text == null)
                return;

            if (text == "")
                return;

            HelpHandler.SearchInHelp(this, text);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textCtrl_HelpRequested(null, null);
        }

        private void insertBPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertBreakPoint();
        }

        private void deleteBPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveBreakPoint();
        }

        private void enableBPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int line = textCtrl.ActiveTextAreaControl.Caret.Line;
            BreakPoint bp = GetBreakPointAtLine(line);
            if (bp == null)
                return;
            bp.Enabled = true;
            textCtrl.ActiveTextAreaControl.TextArea.Invalidate();

            if (EnableBreakPointEvent != null)
                EnableBreakPointEvent(bp);
        }

        private void disableBPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int line = textCtrl.ActiveTextAreaControl.Caret.Line;
            BreakPoint bp = GetBreakPointAtLine(line);
            if (bp == null)
                return;
            bp.Enabled = false;
            textCtrl.ActiveTextAreaControl.TextArea.Invalidate();

            if (DisableBreakPointEvent != null)
                DisableBreakPointEvent(bp);

        }
    }
}