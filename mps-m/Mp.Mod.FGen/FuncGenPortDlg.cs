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
using System.Xml;
using System.Collections;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.FGen
{
        /// <summary>
        /// The function generator data output port property dialog.
        /// </summary>
        internal partial class FuncGenPortDlg : Form
        {
            private ValueHastable _generationRates = new ValueHastable();
            private ValueHastable _functionTypes = new ValueHastable();
            private ArrayList _xmlSignalsToRemove = new ArrayList();
            private XmlElement _xmlSignalList;
            private Document _doc = null;

            private enum ColTypes
            {
                FunctionCol = 0,
                NameCol,
                DataType,
                MinCol,
                MaxCol,
                GenRateCol,
                PeriodeCol,
                DisplOfPhaseCol,
                OnPeriodeCol,
                ParameterCol,
                UnitCol,
                CommentCol
            }


            /// <summary>
            /// Default constructor.
            /// </summary>
            public FuncGenPortDlg(Document doc, XmlElement xmlSignalList)
            {
                _xmlSignalList = xmlSignalList;
                _doc = doc;
                InitializeComponent();
                this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

                DataGridViewComboBoxColumn genRateCol = (DataGridViewComboBoxColumn)_signalGrid.Columns[(int) ColTypes.GenRateCol];
                
                //Generation rate name mapping
                _generationRates.Add(1, "1 Hz");
                genRateCol.Items.Add("1 Hz");

                _generationRates.Add(2, "2 Hz");
                genRateCol.Items.Add("2 Hz");

                _generationRates.Add(5, "5 Hz");
                genRateCol.Items.Add("5 Hz");

                _generationRates.Add(10, "10 Hz");
                genRateCol.Items.Add("10 Hz");

                _generationRates.Add(20, "20 Hz");
                genRateCol.Items.Add("20 Hz");

                _generationRates.Add(50, "50 Hz");
                genRateCol.Items.Add("50 Hz");

                _generationRates.Add(100, "100 Hz");
                genRateCol.Items.Add("100 Hz");
                
                _generationRates.Add(200, "200 Hz");
                genRateCol.Items.Add("200 Hz");

                _generationRates.Add(500, "500 Hz");
                genRateCol.Items.Add("500 Hz");

                _generationRates.Add(1000, "1000 Hz");
                genRateCol.Items.Add("1000 Hz");

                _generationRates.Add(1200, "1200 Hz");
                genRateCol.Items.Add("1200 Hz");

                _generationRates.Add(1500, "1500 Hz");
                genRateCol.Items.Add("1500 Hz");

                _generationRates.Add(2000, "2000 Hz");
                genRateCol.Items.Add("2000 Hz");

                if (_doc.RuntimeEngine.MaxSignalRate > 2000)
                {
                    _generationRates.Add(5000, "5000 Hz");
                    genRateCol.Items.Add("5000 Hz");

                    _generationRates.Add(10000, "10000 Hz");
                    genRateCol.Items.Add("10000 Hz");

                    _generationRates.Add(20000, "20000 Hz");
                    genRateCol.Items.Add("20000 Hz");

                    _generationRates.Add(50000, "50000 Hz");
                    genRateCol.Items.Add("50000 Hz");

                    _generationRates.Add(100000, "100000 Hz");
                    genRateCol.Items.Add("100000 Hz");

                    _generationRates.Add(110000, "110000 Hz");
                    genRateCol.Items.Add("110000 Hz");

                    _generationRates.Add(120000, "120000 Hz");
                    genRateCol.Items.Add("120000 Hz");

                    _generationRates.Add(150000, "150000 Hz");
                    genRateCol.Items.Add("150000 Hz");

                    _generationRates.Add(200000, "200000 Hz");
                    genRateCol.Items.Add("200000 Hz");
                }

                //Function name mapping
                DataGridViewComboBoxColumn funcCol = (DataGridViewComboBoxColumn)_signalGrid.Columns[(int)ColTypes.FunctionCol];

                _functionTypes.Add(0, StringResource.Sine);
                funcCol.Items.Add(StringResource.Sine);

                _functionTypes.Add(1, StringResource.RampUp);
                funcCol.Items.Add(StringResource.RampUp);
                
                _functionTypes.Add(8, StringResource.RampDown);
                funcCol.Items.Add(StringResource.RampDown);

                _functionTypes.Add(2, StringResource.Rectangle);
                funcCol.Items.Add(StringResource.Rectangle);

                _functionTypes.Add(3, StringResource.Noise);
                funcCol.Items.Add(StringResource.Noise);

                _functionTypes.Add(15, StringResource.Random);
                funcCol.Items.Add(StringResource.Random);

                _functionTypes.Add(4, StringResource.Constant);
                funcCol.Items.Add(StringResource.Constant);
                
                _functionTypes.Add(5, StringResource.Sinc);
                funcCol.Items.Add(StringResource.Sinc);

                _functionTypes.Add(14, StringResource.SincMinus);
                funcCol.Items.Add(StringResource.SincMinus);

                _functionTypes.Add(6, StringResource.SinePlus);
                funcCol.Items.Add(StringResource.SinePlus);

                _functionTypes.Add(7, StringResource.SineMinus);
                funcCol.Items.Add(StringResource.SineMinus);

                _functionTypes.Add(9, StringResource.HalfRoundPlus);
                funcCol.Items.Add(StringResource.HalfRoundPlus);

                _functionTypes.Add(10, StringResource.HalfRoundMinus);
                funcCol.Items.Add(StringResource.HalfRoundMinus);

                _functionTypes.Add(12, StringResource.ExpPlus);
                funcCol.Items.Add(StringResource.ExpPlus);

                _functionTypes.Add(13, StringResource.ExpMinus);
                funcCol.Items.Add(StringResource.ExpMinus);
            }

            private void LoadData()
            {
                XmlElement firstSignal = XmlHelper.GetChildByType(_xmlSignalList, "Mp.Sig");
                int index = 0;

                if (firstSignal != null)
                {
                    XmlElement element;
                    DataGridViewRow row;
                    foreach (XmlNode child in _xmlSignalList.ChildNodes)
                    {
                        element = (child as XmlElement);
                        index = _signalGrid.Rows.Add();
                        row = _signalGrid.Rows[index];

                        if (element.GetAttribute("type") == "Mp.Sig")
                            InsertNewSignal(element, row);
                    }

                    index++;
                }

                InitEmtpyRow(index);
                UpdatePreview(0);
                UpdateRowApparence(0);
            }
    
            private void OK_Click(object sender, EventArgs e)
            {                
                XmlElement   xmlSignal;

                foreach (XmlElement node in _xmlSignalsToRemove)
                    _doc.RemoveXmlObject(node);

                foreach (DataGridViewRow row in  _signalGrid.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    if (row.Tag == null)
                        xmlSignal = _doc.CreateXmlObject(_xmlSignalList, "Mp.Sig", "Mp.FGen.Sig");
                    else
                        xmlSignal = (XmlElement) row.Tag;
                        

                    //Set the signal values
                    XmlHelper.SetParam(xmlSignal, "name","string", (string)row.Cells[(int)ColTypes.NameCol].Value);
                    XmlHelper.SetParam(xmlSignal, "unit","string", (string)row.Cells[(int)ColTypes.UnitCol].Value);
                    XmlHelper.SetParam(xmlSignal, "comment","string", (string)row.Cells[(int)ColTypes.CommentCol].Value);
                    
                    string rate = (string) row.Cells[(int)ColTypes.GenRateCol].Value;
                    XmlHelper.SetParamDouble(xmlSignal, "samplerate","double", Convert.ToDouble(_generationRates.GetKeyByValue(rate)) );

                    XmlHelper.SetParamNumber(xmlSignal, "valueDataType","uint8_t", 2);
                    XmlHelper.SetParamDouble(xmlSignal, "physMin","double", Convert.ToDouble( row.Cells[(int)ColTypes.MinCol].Value));
                    XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", Convert.ToDouble( row.Cells[(int)ColTypes.MaxCol].Value));
                    XmlHelper.SetParamDouble(xmlSignal, "periode", "double", Convert.ToDouble( row.Cells[(int)ColTypes.PeriodeCol].Value));
                    XmlHelper.SetParamDouble(xmlSignal, "displacementOfPhase", "double", Convert.ToDouble(row.Cells[(int)ColTypes.DisplOfPhaseCol].Value));
                    XmlHelper.SetParamDouble(xmlSignal, "onPeriode", "double", Convert.ToDouble(row.Cells[(int)ColTypes.OnPeriodeCol].Value));
                    XmlHelper.SetParamDouble(xmlSignal, "parameter", "double", Convert.ToDouble(row.Cells[(int)ColTypes.ParameterCol].Value));
                    string func = (string) row.Cells[(int) ColTypes.FunctionCol].Value;
                    
                    XmlHelper.SetParamNumber(xmlSignal, "functionType","uint8_t", (int)_functionTypes.GetKeyByValue( func ));
                    XmlHelper.SetParamNumber(xmlSignal, "resolution", "uint32_t", Convert.ToInt32( row.Cells[(int)ColTypes.ParameterCol].Value) );
                    XmlHelper.SetParam(xmlSignal, "sourceNumber", "uint32_t", "0");
              }

              _doc.Modified = true;

              Close();
            }

            private void Cancel_Click(object sender, EventArgs e)
            {
                Close();
            }

            private void InitEmtpyRow( int index )
            {
                DataGridViewRow row = _signalGrid.Rows[index];

                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)row.Cells[(int)ColTypes.FunctionCol];

                row.Cells[(int)ColTypes.FunctionCol].Value      = cell.Items[0];
                row.Cells[(int)ColTypes.NameCol].Value          = "";
                row.Cells[(int)ColTypes.MinCol].Value           = -10.0;
                row.Cells[(int)ColTypes.MaxCol].Value           = 10.0;
                
                cell = (DataGridViewComboBoxCell)row.Cells[(int)ColTypes.GenRateCol];

                row.Cells[(int)ColTypes.DataType].Value = "LREAL";
                row.Cells[(int)ColTypes.GenRateCol].Value = cell.Items[6];
                row.Cells[(int)ColTypes.PeriodeCol].Value       = 1000;
                row.Cells[(int)ColTypes.DisplOfPhaseCol].Value  = (int)0;
                row.Cells[(int)ColTypes.OnPeriodeCol].Value     = 500;
                row.Cells[(int)ColTypes.ParameterCol].Value         = 0;
                row.Cells[(int)ColTypes.UnitCol].Value          = "";
                row.Cells[(int)ColTypes.CommentCol].Value       = "";
            }

            private void InsertNewSignal( XmlElement element, DataGridViewRow row )
            {                
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)row.Cells[(int)ColTypes.FunctionCol];

                int func = (int) XmlHelper.GetParamNumber(element, "functionType");
                row.Cells[(int)ColTypes.FunctionCol].Value = _functionTypes[func];
                row.Cells[(int)ColTypes.NameCol].Value          = XmlHelper.GetParam(element, "name");
                row.Cells[(int)ColTypes.DataType].Value         = "LREAL";
                row.Cells[(int)ColTypes.MinCol].Value           = XmlHelper.GetParamDouble(element, "physMin");
                row.Cells[(int)ColTypes.MaxCol].Value           = XmlHelper.GetParamDouble(element, "physMax");
                row.Cells[(int)ColTypes.GenRateCol].Value       = _generationRates[(int)XmlHelper.GetParamDouble(element, "samplerate")];
                row.Cells[(int)ColTypes.PeriodeCol].Value       = XmlHelper.GetParamDouble(element, "periode");
                row.Cells[(int)ColTypes.DisplOfPhaseCol].Value  = XmlHelper.GetParamDouble(element, "displacementOfPhase");
                row.Cells[(int)ColTypes.OnPeriodeCol].Value     = XmlHelper.GetParamDouble(element, "onPeriode");
                row.Cells[(int)ColTypes.ParameterCol].Value         = XmlHelper.GetParamDouble(element, "parameter");
                row.Cells[(int)ColTypes.UnitCol].Value          = XmlHelper.GetParam(element, "unit");
                row.Cells[(int)ColTypes.CommentCol].Value       = XmlHelper.GetParam(element, "comment");
                row.Tag = element;
                UpdateRowApparence(row.Index);
            }
      
            private void UpdateRowApparence(int row)
            {                
                DataGridViewRow gridRow;
                DataGridViewCell gridCell;
   
                gridRow  = _signalGrid.Rows[row];
                gridCell = gridRow.Cells[(int)ColTypes.FunctionCol];
                string cellValue = (string)gridCell.Value;

                if (cellValue == StringResource.Sine ||
                    cellValue == StringResource.SinePlus || 
                    cellValue == StringResource.SineMinus ||
                    cellValue == StringResource.Sinc ||
                    cellValue == StringResource.SincMinus ||
                    cellValue == StringResource.RampUp||
                    cellValue == StringResource.RampDown ||
                    cellValue == StringResource.HalfRoundMinus ||
                    cellValue == StringResource.HalfRoundPlus)
                {
                        gridCell = gridRow.Cells[(int)ColTypes.DisplOfPhaseCol];
                        gridCell.ReadOnly = false;
                        gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.Window);

                        gridCell = gridRow.Cells[(int)ColTypes.OnPeriodeCol];
                        gridCell.ReadOnly = true;
                        gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);

                        if (cellValue == StringResource.Sinc ||
                            cellValue == StringResource.SincMinus)
                        {
                            gridCell = gridRow.Cells[(int)ColTypes.ParameterCol];
                            gridCell.ReadOnly = false;
                            gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                        }
                        else
                        {
                            gridCell = gridRow.Cells[(int)ColTypes.ParameterCol];
                            gridCell.ReadOnly = true;
                            gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                        }
                }
                else if (cellValue == StringResource.ExpPlus ||
                         cellValue == StringResource.ExpMinus)
                {
                    gridCell = gridRow.Cells[(int)ColTypes.DisplOfPhaseCol];
                    gridCell.ReadOnly = true;
                    gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);

                    gridCell = gridRow.Cells[(int)ColTypes.OnPeriodeCol];
                    gridCell.ReadOnly = true;
                    gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);

                    gridCell = gridRow.Cells[(int)ColTypes.ParameterCol];
                    gridCell.ReadOnly = true;
                    gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                }
                else if (cellValue == StringResource.Random)
                {
                    gridCell = gridRow.Cells[(int)ColTypes.DisplOfPhaseCol];
                    gridCell.ReadOnly = true;
                    gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);

                    gridCell = gridRow.Cells[(int)ColTypes.OnPeriodeCol];
                    gridCell.ReadOnly = true;
                    gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);

                    gridCell = gridRow.Cells[(int)ColTypes.ParameterCol];
                    gridCell.ReadOnly = false;
                    gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.Window);

                }
                else if (cellValue == StringResource.Noise)
                {
                    gridCell = gridRow.Cells[(int)ColTypes.DisplOfPhaseCol];
                    gridCell.ReadOnly = true;
                    gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);

                    gridCell = gridRow.Cells[(int)ColTypes.OnPeriodeCol];
                    gridCell.ReadOnly = true;
                    gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);

                    gridCell = gridRow.Cells[(int)ColTypes.ParameterCol];
                    gridCell.ReadOnly = true;
                    gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                }
                else if (cellValue == StringResource.Rectangle)
                {
                    gridCell = gridRow.Cells[(int)ColTypes.DisplOfPhaseCol];
                    gridCell.ReadOnly = false;
                    gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.Window);

                    gridCell = gridRow.Cells[(int)ColTypes.OnPeriodeCol];
                    gridCell.ReadOnly = false;
                    gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.Window);

                    gridCell = gridRow.Cells[(int)ColTypes.ParameterCol];
                    gridCell.ReadOnly = true;
                    gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                }
                else if (cellValue == StringResource.Constant)
                {
                    gridCell = gridRow.Cells[(int)ColTypes.DisplOfPhaseCol];
                    gridCell.ReadOnly = true;
                    gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);

                    gridCell = gridRow.Cells[(int)ColTypes.OnPeriodeCol];
                    gridCell.ReadOnly = true;
                    gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);

                    gridCell = gridRow.Cells[(int)ColTypes.ParameterCol];
                    gridCell.ReadOnly = false;
                    gridCell.Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                }
            }

            private void UpdatePreview( int row )
            {                               
                SignalGeneratorViewer.SignalData viewerData = new SignalGeneratorViewer.SignalData();
                DataGridViewRow dataRow = _signalGrid.Rows[row];
                string value;

                viewerData.Max = Convert.ToDouble( dataRow.Cells[(int)ColTypes.MaxCol].Value );
                viewerData.Min = Convert.ToDouble( dataRow.Cells[(int)ColTypes.MinCol].Value );
                viewerData.Periode = Convert.ToDouble( dataRow.Cells[(int)ColTypes.PeriodeCol].Value );
                viewerData.Phase = Convert.ToDouble( dataRow.Cells[(int)ColTypes.DisplOfPhaseCol].Value );
                viewerData.OnPeriode = Convert.ToDouble(dataRow.Cells[(int)ColTypes.OnPeriodeCol].Value);
                viewerData.Parameter = Convert.ToDouble(dataRow.Cells[(int)ColTypes.ParameterCol].Value);

                value = (string) dataRow.Cells[(int)ColTypes.FunctionCol].Value;
                viewerData.Type = (FunctionGenPS.FunctionType)(int)_functionTypes.GetKeyByValue(value);

                value = (string) dataRow.Cells[(int)ColTypes.GenRateCol].Value;
                viewerData.GenRate = Convert.ToDouble( _generationRates.GetKeyByValue(value) );

                _signalViewer.Data = viewerData;
            }



            private void _signalGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
            {
                UpdatePreview(e.RowIndex);
                _remove.Enabled = true;
            }

            private void FuncGenPortDlg_Load(object sender, EventArgs e)
            {
                LoadData();
            }

            private void _signalGrid_CellLeave(object sender, DataGridViewCellEventArgs e)
            {                
                UpdatePreview(e.RowIndex);
                UpdateRowApparence(e.RowIndex);
            }

            private void _signalGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
            {
                UpdatePreview(e.RowIndex);
                UpdateRowApparence(e.RowIndex);

                switch (_signalViewer.Data.Type)
                {
                    case FunctionGenPS.FunctionType.RampUp:
                    case FunctionGenPS.FunctionType.RampDown:
                    {
                        DataGridViewRow row = _signalGrid.Rows[e.RowIndex];
                        row.Cells[(int) ColTypes.PeriodeCol].Value = _signalViewer.CalcPeriode();
                        row.Cells[(int) ColTypes.DisplOfPhaseCol].Value = _signalViewer.CalcPhase();
                    }
                    break;
                }

            }

            private void _signalGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
            {
                InitEmtpyRow(e.RowIndex);
                UpdateRowApparence(e.RowIndex);
            }

            private void _signalGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
            {
                _errorProvider.SetError(_signalGrid, "");

                switch( (ColTypes) e.ColumnIndex )
                {
                    case ColTypes.NameCol:
                    {
                        string name = (string)e.FormattedValue;
                        if (name == "")
                        {
                            e.Cancel = true;
                            _errorProvider.SetError(_signalGrid, StringResource.SigNameErr);
                        }            
                    }
                    break;
                    case ColTypes.MinCol:
                    case ColTypes.MaxCol:
                    case ColTypes.ParameterCol:
                    {
                        try
                        {
                            string value = (string)e.FormattedValue;
                            double db = Convert.ToDouble( value);
                        }
                        catch( Exception ex )
                        {
                            e.Cancel = true;
                            _errorProvider.SetError(_signalGrid, ex.Message);
                        }              
                    }
                    break;
                    
                    case ColTypes.DisplOfPhaseCol:
                    {
                        try
                        {
                            string value = (string)e.FormattedValue;
                            double db = Convert.ToDouble( value);
                        }
                        catch( Exception ex )
                        {
                            e.Cancel = true;
                            _errorProvider.SetError(_signalGrid, ex.Message);
                        }    
                    }
                    break;

                    case ColTypes.PeriodeCol:
                    case ColTypes.OnPeriodeCol:
                    try
                    {
                        string value = (string)e.FormattedValue;
                        double db = Convert.ToDouble(value);
                        if (db == 0)
                        {
                            e.Cancel = true;
                            _errorProvider.SetError(_signalGrid, StringResource.PeriodeErr);
                        }
                    }
                    catch (Exception ex)
                    {
                        e.Cancel = true;
                        _errorProvider.SetError(_signalGrid, ex.Message);
                    }    
                    break;
                }            
            }

            private void _remove_Click(object sender, EventArgs e)
            {
                if (_signalGrid.SelectedCells.Count == 1)
                {
                    if (_signalGrid.SelectedCells[0].RowIndex == -1)
                        return;

                    DataGridViewRow row = _signalGrid.Rows[_signalGrid.SelectedCells[0].RowIndex];

                    if (row.IsNewRow)
                        return;
                }

                DialogResult res = MessageBox.Show(StringResource.RemoveSig, StringResource.RemoveSig, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (res != DialogResult.Yes)
                    return;

                foreach( DataGridViewCell cell in _signalGrid.SelectedCells)
                {
                    if (cell.RowIndex == -1)
                        continue;

                    DataGridViewRow row = _signalGrid.Rows[cell.RowIndex];

                    if (row.IsNewRow)
                        continue;

                    if (row.Tag != null)
                        _xmlSignalsToRemove.Add(row.Tag);

                    _signalGrid.Rows.RemoveAt(row.Index);
                }
            }

            private void _signalGrid_Validating(object sender, CancelEventArgs e)
            {
                foreach (DataGridViewRow row in _signalGrid.Rows)
                {
                    string name = (string) row.Cells[(int)ColTypes.NameCol].Value;

                    foreach (DataGridViewRow searchRow in _signalGrid.Rows)
                    {
                        if (row.Index == searchRow.Index)
                            continue;

                        string searchName = (string)searchRow.Cells[(int)ColTypes.NameCol].Value;
                        if (name == searchName)
                        {
                            e.Cancel = true;
                            _errorProvider.SetError(_signalGrid, String.Format(StringResource.SigNameDup,name));
                            return;
                        }
                    }
                }
            }

            private void FuncGenPortDlg_ResizeEnd(object sender, EventArgs e)
            {
                _signalViewer.Invalidate();
            }

            private void _signalGrid_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
            {
                SignalGeneratorViewer.SignalData viewerData = new SignalGeneratorViewer.SignalData();
                DataGridViewRow dataRow = _signalGrid.Rows[e.RowIndex];
                string value;

                viewerData.Max = Convert.ToDouble(dataRow.Cells[(int)ColTypes.MaxCol].Value);
                viewerData.Min = Convert.ToDouble(dataRow.Cells[(int)ColTypes.MinCol].Value);

                if (viewerData.Min >= viewerData.Max)
                {
                    _errorProvider.SetError(_signalGrid, StringResource.SigMinMaxErr);
                    e.Cancel = true;
                    return;
                }

                viewerData.Periode = Convert.ToDouble(dataRow.Cells[(int)ColTypes.PeriodeCol].Value);
                viewerData.Phase = Convert.ToDouble(dataRow.Cells[(int)ColTypes.DisplOfPhaseCol].Value);

                value = (string)dataRow.Cells[(int)ColTypes.FunctionCol].Value;
                viewerData.Type = (FunctionGenPS.FunctionType)(int)_functionTypes.GetKeyByValue(value);

                value = (string)dataRow.Cells[(int)ColTypes.GenRateCol].Value;
                viewerData.GenRate = Convert.ToDouble(_generationRates.GetKeyByValue(value));

                if (viewerData.Type == FunctionGenPS.FunctionType.RampUp ||
                    viewerData.Type == FunctionGenPS.FunctionType.RampDown ||
                    viewerData.Type == FunctionGenPS.FunctionType.Sine ||
                    viewerData.Type == FunctionGenPS.FunctionType.SinePlus ||
                    viewerData.Type == FunctionGenPS.FunctionType.SineMinus)

                {
                    double noOfPoints = viewerData.Periode / (1000/ viewerData.GenRate);
                    if (noOfPoints < 3)
                    {
                        _errorProvider.SetError(_signalGrid, StringResource.SampleRatePerErr);
                        e.Cancel = true;
                    }
                }
            }
            private void FuncGenPortDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
            {
                Document.ShowHelp(this, 320);
            }

            private void help_Click(object sender, EventArgs e)
            {
                FuncGenPortDlg_HelpRequested(null, null);
            }
    }
}
