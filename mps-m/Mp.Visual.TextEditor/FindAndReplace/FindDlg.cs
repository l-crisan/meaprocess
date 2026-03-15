//Copyright (C) 2010-2016 ATESiON GmbH. All rights reserved.
using System;
using System.Windows.Forms;


namespace Mp.Visual.TextEditor
{
    public partial class FindDlg : Form
    {
        private FindAndReplace.Engine _engine;

        public FindDlg(FindAndReplace.Engine engine)
        {
            _engine = engine;
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
            this.Visible = false;
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

            if (!_engine.FindNext(findWhat.Text, matchCase.Checked, matchWholeWord.Checked))
            {
                _engine.Reset();
                return;
            }

            _engine.SelectSearchResult();
        }

        private void OnFindReplaceDlgVisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            _engine.Reset();
        }

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			findWhat.Focus();
		}

		private void OnFindAllClick(object sender, EventArgs e)
		{
			Close();
			Cursor = Cursors.WaitCursor;
			_engine.FindAll(findWhat.Text, matchCase.Checked, matchWholeWord.Checked);
			Cursor = Cursors.Default;
		}

		private void OnLookInSelectedIndexChanged(object sender, EventArgs e)
		{
            _engine.FindIn = (FindAndReplace.Engine.FindInto)lookIn.SelectedIndex;
            findNext.Enabled = _engine.FindIn != FindAndReplace.Engine.FindInto.Project;
        }
  }
}
