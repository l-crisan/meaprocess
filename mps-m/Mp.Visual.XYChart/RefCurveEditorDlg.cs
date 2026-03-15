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
using System.IO;
using System.ComponentModel;

namespace Mp.Visual.XYChart
{
    public partial class RefCurveEditorDlg : Form
    {
        private CurveList _refCurves = new CurveList();
        private static List<string> _loadedFiles = new List<string>();
        private ImageList _imgList = new ImageList();

        public RefCurveEditorDlg(CurveList refCurves)
        {
            _refCurves = refCurves;
            InitializeComponent();

            _imgList.Images.Add(Resource.DD_09_1);
            _imgList.Images.Add(Resource.StGroup);
            _imgList.Images.Add(Resource.Signal);
            fileView.ImageList = _imgList;
            /*
            foreach (string file in _loadedFiles)
                LoadMMFFile(file);
                */
            foreach (Curve curve in refCurves)
            {
                int index = curves.Rows.Add();

                DataGridViewRow row = curves.Rows[index];
                row.Cells[0].Value = index + 1;
                row.Cells[0].Style.BackColor = curve.LineColor;
                row.Cells[1].Value = curve.Name;
                row.Cells[2].Value = curve.XName;
                row.Cells[3].Value = curve.YName;                
                row.Cells[4].Value = "...";                                
                
                row.Tag = curve;
            }
        }


        private void OnOpenClick(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "*.mmf|*.mmf";
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            
            _loadedFiles.Add(dlg.FileName);
        }


        private void OnRemoveFileClick(object sender, EventArgs e)
        {
            if (fileView.SelectedNode == null)
                return;

            if (!(fileView.SelectedNode.Tag is string))
                return;

            string file = (string)fileView.SelectedNode.Tag;
            fileView.Nodes.Remove(fileView.SelectedNode);
            file = file.ToUpper();
            for (int i = 0; i < _loadedFiles.Count; ++i)
            {
                if (_loadedFiles[i].ToUpper() == file)
                {
                    _loadedFiles.RemoveAt(i);
                    --i;
                }
            }
        }


        private void OnOKClick(object sender, EventArgs e)
        {            
            DialogResult = DialogResult.OK;
            Close();
        }


        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }


        private void OnAddClick(object sender, EventArgs e)
        {
            int index = curves.Rows.Add();

            DataGridViewRow row = curves.Rows[index];
            row.Cells[0].Value = index + 1;
            row.Cells[1].Value = "";
            row.Cells[4].Value = "...";
            row.Tag = new Curve();
        }


        private void OnRemoveCurveClick(object sender, EventArgs e)
        {
            if (curves.SelectedRows.Count == 0)
                return;

            if (MessageBox.Show(StringResource.RemoveRefCurve, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();
            for (int i = 0; i < curves.SelectedRows.Count; ++i)
                rowsToRemove.Add(curves.SelectedRows[i]);

            for (int i = 0; i < rowsToRemove.Count; ++i)
                curves.Rows.Remove(rowsToRemove[i]);

            int index = 1;
            foreach (DataGridViewRow row in curves.Rows)
            {
                row.Cells[0].Value = index;
                ++index;
            }
        }

        private void OnFileViewItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode node = (TreeNode)e.Item;
            /*
            if (node.Tag is Mp.Drv.DataFile.Signal)
            {
                TreeNode fileNode = node.Parent.Parent;
                string basePath = Path.GetDirectoryName((string)fileNode.Tag);
                
                CurveInfo cvi = new CurveInfo((StorageGroup)node.Parent.Tag, (Mp.Drv.DataFile.Signal)node.Tag, basePath);
                fileView.DoDragDrop(cvi, DragDropEffects.Move);
            }
        */
        }

        private void OnCurvesDragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("Mp.XYChart.RefCurveEditorDlg+CurveInfo"))
                return;

            e.Effect = DragDropEffects.Move;

        }

        /*
        private double[] GetSignalData(CurveInfo cvi)
        {
            CopySignalDialog dlg = new CopySignalDialog(cvi);
            dlg.ShowDialog();
            return dlg.SignalData;
        }
        */

        private void OnCurvesDragDrop(object sender, DragEventArgs e)
        {
          /*
            CurveInfo cvi = (CurveInfo)e.Data.GetData("Mp.XYChart.RefCurveEditorDlg+CurveInfo");

            Point pt = new Point(e.X,e.Y);
            pt = curves.PointToClient(pt);

            DataGridView.HitTestInfo hitInfo = curves.HitTest(pt.X, pt.Y);

            if (hitInfo.RowIndex == -1)
                return;

            DataGridViewRow row = curves.Rows[hitInfo.RowIndex];
            Curve curve = (Curve) row.Tag;

            if( hitInfo.ColumnIndex == 2 )
            {                
                curve.XName = cvi.Signal.Name;
                curve.X = GetSignalData(cvi);                
                row.Cells[2].Value = curve.XName;                
            }

            if( hitInfo.ColumnIndex == 3 )
            {
                curve.YName = cvi.Signal.Name;
                curve.Y = GetSignalData(cvi);
                curve.YRate = cvi.StgInfo.SampleRate;
                row.Cells[3].Value = curve.YName;                
            }*/
        }

        protected override void OnClosing(CancelEventArgs e)
        {            
            errorProvider.Clear();
            foreach (DataGridViewRow row in curves.Rows)
            {
                Curve curve = (Curve)row.Tag;

                if (curve.Y == null)
                {
                    string msg = String.Format(StringResource.CurveYSigAssgn, row.Index + 1);
                    errorProvider.SetError(curves, msg);
                    e.Cancel = true;
                    return;
                }
            }

            _refCurves.Clear();
            foreach (DataGridViewRow row in curves.Rows)
            {
                Curve curve = (Curve)row.Tag;
                curve.Name = (string) row.Cells[1].Value;
                _refCurves.Add(curve);
            }
        }

        private void OnCurvesCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 4)
                return;

            if (e.RowIndex == -1)
                return;

            ColorDialog dlg = new ColorDialog();
            DataGridViewRow row = curves.Rows[e.RowIndex];
            Curve curve = (Curve)row.Tag;

            DataGridViewButtonCell cell = (DataGridViewButtonCell)row.Cells[e.ColumnIndex];
            dlg.Color = curve.LineColor;

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            row.Cells[0].Style.BackColor = dlg.Color;
            curve.LineColor = dlg.Color;            
        }
    }
}
