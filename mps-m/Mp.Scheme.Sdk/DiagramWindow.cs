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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Mp.Visual.Docking;
using Mp.Utils;

namespace Mp.Scheme.Sdk
{
    public partial class DiagramWindow : DockContent
    {
        private Timer _updateTimer = new Timer();
        private bool _mainScheme = false;

        public DiagramWindow()
        {
            InitializeComponent();
            undoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Z;
            redoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Y;
            selectAllToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;
            cutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.X;
            copyToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            pasteToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.V;
            deleteToolStripMenuItem.ShortcutKeys = Keys.Delete;

            _updateTimer.Interval = 300;
            _updateTimer.Tick += new EventHandler(OnUpdateTimerTick);
            _updateTimer.Start();
            TabText = Text;
        }


        public bool MainScheme
        {
            get { return _mainScheme; }
            set { _mainScheme = value; }
        }


        public void LoadResources()
        {            
            ResourceLoader.LoadResources(this);     
            
            if(_mainScheme)
                this.TabText = this.Text;
        }


        private void OnUpdateTimerTick(object sender, EventArgs e)
        {
            toolStripButtonDelete.Enabled = diagram.CanDeleteObject;
            toolStripButtonSelectAll.Enabled = diagram.Shapes.Count != 0;
            toolStripButtonUndo.Enabled = diagram.CanUndo;
            toolStripButtonRedo.Enabled = diagram.CanRedo;
            toolStripButtonCut.Enabled = diagram.IsObjSelected;
            toolStripButtonCopy.Enabled = diagram.IsObjSelected;
        }


        public Visual.Diagram.DiagramCtrl Diagram
        {
            get { return diagram; } 
        }


        private void OnDeleteClick(object sender, EventArgs e)
        {
            diagram.DeleteObject();
        }


        private void OnSelectAllClick(object sender, EventArgs e)
        {
            diagram.SelectAll();
        }


        private void OnShowGridClick(object sender, EventArgs e)
        {
            toolStripButtonShowGrid.Checked = !toolStripButtonShowGrid.Checked;
            diagram.ShowGrid = toolStripButtonShowGrid.Checked;
            //Properties.Settings.Default.GridVisible = toolStripButtonShowGrid.Checked;
        }


        private void OnDiagramKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                diagram.SelectAll();
                return;
            }

            if (e.KeyCode == Keys.Delete && toolStripButtonDelete.Enabled)
            {
                diagram.DeleteObject();
                return;
            }

            if(e.Control && e.KeyCode == Keys.G)
            {
                toolStripButtonShowGrid.Checked = !toolStripButtonShowGrid.Checked;
                
                if (toolStripButtonShowGrid.Checked)
                    toolStripButtonShowGrid.CheckState = CheckState.Checked;
                else
                    toolStripButtonShowGrid.CheckState = CheckState.Unchecked;

                diagram.ShowGrid = toolStripButtonShowGrid.Checked;
                return;
            }

            if (e.Control && e.KeyCode == Keys.Z)
            {
                diagram.Undo();
                return;
            }

            if (e.Control && e.KeyCode == Keys.Y)
            {
                diagram.Redo();
                return;
            }

            diagram.KeyDownEvent(e);
        }


        private void OnAlignLeftClick(object sender, EventArgs e)
        {
            diagram.AlignSelectionLeft();
        }


        private void OnAlignTopClick(object sender, EventArgs e)
        {
            diagram.AlignSelectionTop();
        }


        private void OnDiagramKeyUp(object sender, KeyEventArgs e)
        {
            diagram.KeyUpEvent(e);
        }


        private void OnUndoClick(object sender, EventArgs e)
        {
            diagram.Undo();
        }


        private void OnRedoClick(object sender, EventArgs e)
        {
            diagram.Redo();
        }


        private void OnSchemeInfoClick(object sender, EventArgs e)
        {

        }

        private void OnContextMenuStripOpening(object sender, CancelEventArgs e)
        {
            undoToolStripMenuItem.Enabled = diagram.CanUndo;
            redoToolStripMenuItem.Enabled = diagram.CanRedo;
            showGridToolStripMenuItem.Checked = diagram.ShowGrid;
            cutToolStripMenuItem.Enabled = diagram.IsObjSelected;
            copyToolStripMenuItem.Enabled = diagram.IsObjSelected;
            deleteToolStripMenuItem.Enabled = diagram.CanDeleteObject;
        }


        protected override string GetPersistString()
        {
            return base.GetPersistString() + "\n" + diagram.ID.ToString();
        }
        

        private void OnHelpRequested(object sender, HelpEventArgs hlpevent)
        {
            diagram.OnHelpRequested(sender, hlpevent);
        }

        private bool _closeForced = false;
        
        public void CloseForced()
        {
            _closeForced = true;
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            e.Cancel = !_closeForced;
        }

        private void OnCutClick(object sender, EventArgs e)
        {
            diagram.Cut();
        }

        private void OnPasteClick(object sender, EventArgs e)
        {
            diagram.Paste(new Point(50,50));
        }

        private void OnCopyClick(object sender, EventArgs e)
        {
            diagram.Copy();
        }

        private void OnPasteToolClick(object sender, EventArgs e)
        {
            diagram.Paste(PointToClient(MousePosition));
        }
    }
}
