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
using System.Windows.Forms;
using Mp.Scheme.Sdk;
using Mp.Utils;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Actions;
using Mp.Visual.TextEditor;


namespace Mp.Mod.HTML.View
{
    internal partial class TextEditorDlg : Form
    {
        private Document _doc;

        Visual.TextEditor.FindAndReplace.Engine _findReplace;
        FindDlg _findDlg;
        FindReplaceDlg _replaceDlg;

        public TextEditorDlg(string text, Document doc)
        {            
            _doc = doc;

            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            toolStripMenuItemCut6.ShortcutKeys = Keys.Control | Keys.X;
            toolStripMenuItemCopy.ShortcutKeys = Keys.Control | Keys.C;
            toolStripMenuItemPaste.ShortcutKeys = Keys.Control | Keys.V;
            toolStripMenuItemDelete.ShortcutKeys = Keys.Delete;
            toolStripMenuItemSelectAll.ShortcutKeys = Keys.Control | Keys.A;
            findToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F;
            findAndReplaceToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.H;

            textEditorControl.SetHighlighting("HTML");
            textEditorControl.Text = text;
            _findReplace = new Visual.TextEditor.FindAndReplace.Engine();
            _findDlg = new FindDlg(_findReplace);
            _replaceDlg = new FindReplaceDlg(_findReplace);

            FormStateHandler.Restore(this, Document.RegistryKey + "ConfigTextDlg");
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

        public string TheText
        {
            get { return textEditorControl.Text; }
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1220);
        }

        private void ConfigEventDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }

        private void textEditorControl_TextChanged(object sender, EventArgs e)
        {
            string text = textEditorControl.Text;
            
            if (!text.Contains("<html>"))
                text = "<html>"+ text + "</html>";

            List<string> properties = new List<string>();

            bool propBegin = false;
            string prop = "";
            int lbreakClose = 0;

            for(int i = 0; i < text.Length - 1; ++i)
            {
                if (text[i] == '$' && text[i + 1] == '(')
                {
                    propBegin = true;
                    ++i;
                    lbreakClose++;
                    continue;
                }

                if (propBegin)
                    prop += text[i];

                if (text[i] == '(' && propBegin)
                    lbreakClose++;

                if (text[i] == ')' && propBegin)
                {
                    lbreakClose--;
                    if (lbreakClose == 0)
                    {
                        properties.Add(prop.TrimEnd(')'));
                        prop = "";
                        propBegin = false;
                    }
                }
            }

            if(propBegin)
                properties.Add(prop);

            foreach (string property in properties)
            {
                string value = _doc.GetPropertyValue(property);
                text = text.Replace("$(" + property + ")", value);
            }

            try
            {
                webBrowser.DocumentText = text;
                webBrowser.Invalidate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SchemePropertyDlg dlg = new SchemePropertyDlg(_doc);
            
            if(dlg.ShowDialog() != DialogResult.OK)
                return;

            if(dlg.SelectedProperties.Count != 0)
                Insert(textEditorControl, dlg.SelectedProperties[0]);
        }

        private void Insert(ICSharpCode.TextEditor.TextEditorControl textCtrl, string str)
        {
            int line = textCtrl.ActiveTextAreaControl.Caret.Line;
            int col = textCtrl.ActiveTextAreaControl.Caret.Column;

            int offset = textCtrl.Document.PositionToOffset(new TextLocation(col, line));

            textCtrl.Document.Insert(offset, str);
            LineSegment seg = textCtrl.Document.GetLineSegmentForOffset(offset + str.Length);
            SetCaret(textCtrl, seg.LineNumber, offset + str.Length);
        }

        private void SetCaret(ICSharpCode.TextEditor.TextEditorControl textCtrl, int line, int col)
        {
            textCtrl.ActiveTextAreaControl.Caret.Line = line;
            textCtrl.ActiveTextAreaControl.Caret.Column = col;
            textCtrl.Select();
            textCtrl.Focus();
        }

        private void ConfigTextDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            _findDlg.Close();
            _replaceDlg.Close();

            webBrowser.Dispose();

            FormStateHandler.Save(this, Document.RegistryKey + "ConfigTextDlg");
        }

        private void toolStripButtonCut_Click(object sender, EventArgs e)
        {
            ICSharpCode.TextEditor.Actions.Cut cut = new ICSharpCode.TextEditor.Actions.Cut();
            cut.Execute(textEditorControl.ActiveTextAreaControl.TextArea);

        }

        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            ICSharpCode.TextEditor.Actions.Copy copy = new ICSharpCode.TextEditor.Actions.Copy();
            copy.Execute(textEditorControl.ActiveTextAreaControl.TextArea);

        }

        private void toolStripButtonPaste_Click(object sender, EventArgs e)
        {
            ICSharpCode.TextEditor.Actions.Paste paste = new ICSharpCode.TextEditor.Actions.Paste();
            paste.Execute(textEditorControl.ActiveTextAreaControl.TextArea);
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            textEditorControl.ActiveTextAreaControl.TextArea.ClipboardHandler.Delete(null, null);
        }

        private void toolStripButtonUndo_Click(object sender, EventArgs e)
        {
            textEditorControl.Undo();
        }

        private void toolStripButtonRedo_Click(object sender, EventArgs e)
        {
            textEditorControl.Redo();
        }

        private void toolStripMenuItemSelectAll_Click(object sender, EventArgs e)
        {
            SelectWholeDocument sel = new SelectWholeDocument();
            sel.Execute(textEditorControl.ActiveTextAreaControl.TextArea);
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _findDlg.Show();
        }

        private void findAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _replaceDlg.Show();
        }
     
    }
}
