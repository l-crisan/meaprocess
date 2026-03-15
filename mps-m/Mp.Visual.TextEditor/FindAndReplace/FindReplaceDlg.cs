//Copyright (C) 2010-2016 ATESiON GmbH. All rights reserved.
using System;
using System.Windows.Forms;

namespace Mp.Visual.TextEditor
{
    public partial class FindReplaceDlg : Form
    {
        private FindAndReplace.Engine _engine;

        public FindReplaceDlg(FindAndReplace.Engine searchEngine)
        {
            _engine = searchEngine;
            InitializeComponent();
            lookIn.SelectedIndex = 0;
        }

        public string FindWhat
        {
            get 
            { 
                return findWhat.Text;
            }
            set
            {
                findWhat.Text = value;
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            Visible = false;
            e.Cancel = true;
        }

        private void OnCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OnFindNextClick(object sender, EventArgs e)
        {
            if (findWhat.Text == "")
                return;

            if( !_engine.FindNext(findWhat.Text, matchCase.Checked, matchWholeWord.Checked) )
            {
                _engine.Reset();
                return;
            }

            _engine.SelectSearchResult();
        }

        private void OnReplaceClick(object sender, EventArgs e)
        {
            if(!_engine.Selected)
                return;

            _engine.Replace(replaceWith.Text);
        }

        private void OnVisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
                return;

            _engine.Reset();
        }

        private void OnReplaceAllClick(object sender, EventArgs e)
        {
            Close();
            _engine.Reset();
            _engine.ReplaceAll( findWhat.Text, replaceWith.Text, matchCase.Checked, matchWholeWord.Checked );
            
        }

		    protected override void OnShown(EventArgs e)
		    {
			    base.OnShown(e);
			    findWhat.Focus();
		    }

        private void OnLookInSelectedIndexChanged(object sender, EventArgs e)
        {
            _engine.FindIn = (FindAndReplace.Engine.FindInto)lookIn.SelectedIndex;
            findNext.Enabled = _engine.FindIn != FindAndReplace.Engine.FindInto.Project;
            replace.Enabled = _engine.FindIn != FindAndReplace.Engine.FindInto.Project;
    }
  }
}
