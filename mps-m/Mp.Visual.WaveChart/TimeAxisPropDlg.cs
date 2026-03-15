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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Mp.Visual.WaveChart
{
    public partial class TimeAxisPropDlg : Form
    {
        public class PoDialogProperties
        {
            public PoDialogProperties(){}
            public string Name;
            public uint TimeSlot;
            public int Division;
            public int Precision;
            public int Representation;
            public string AxisText;
            public Color LineColor;
            public Color DegreeTxtColor;
            public Color AxisTextColor;
            public Color BackColor;            
        }
        private bool _running = false;

        public delegate void ChangeProperties(PoDialogProperties DialogProperties);

        public TimeAxisPropDlg(bool running)
        {
            _running = running;
            InitializeComponent();
            
            if (_running)
                _timeSlot.Enabled = false;
        }

        public PoDialogProperties DialogProperties = new PoDialogProperties();
        public ChangeProperties OnChangeProperties;

        private void PoXAxisPropDlg_Load(object sender, EventArgs e)
        {
            cntrlApplyOnChange.Checked = false;

            ctrlName.Text                   = DialogProperties.Name;
            _axisDivision.Text              = DialogProperties.Division.ToString();
            _axisText.Text                  = DialogProperties.AxisText.ToString();
            _bkColor.BackColor              = DialogProperties.BackColor;
            _degreeTextColor.BackColor      = DialogProperties.DegreeTxtColor;
            _lineColor.BackColor            = DialogProperties.LineColor;
            _textColor.BackColor            = DialogProperties.AxisTextColor;
            _precision.Text                 = DialogProperties.Precision.ToString();
            _representation.SelectedIndex   = DialogProperties.Representation;
            _timeSlot.Text                  = DialogProperties.TimeSlot.ToString();
            cntrlApplyOnChange.Checked      = true;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if( OnApply() )
                Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OnPropertieChange(object sender, EventArgs e)
        {
            if (!cntrlApplyOnChange.Checked)
                return;

            OnApply();
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            OnApply();
        }
        private bool OnApply()
        {
            _errorProvider.Clear();
            DialogProperties.Name           = ctrlName.Text;

            if (_axisDivision.Text != "")
            {
                try
                {
                    DialogProperties.Division = Convert.ToInt32(_axisDivision.Text);
                }
                catch (Exception ex)
                {
                    _errorProvider.SetError(_axisDivision, ex.Message);
                    return false;
                }
            }

            DialogProperties.AxisText       = _axisText.Text;
            DialogProperties.BackColor      = _bkColor.BackColor;
            DialogProperties.DegreeTxtColor = _degreeTextColor.BackColor;
            DialogProperties.LineColor      = _lineColor.BackColor;

            if (_precision.Text != "")
            {
                try
                {
                    DialogProperties.Precision = Convert.ToInt32(_precision.Text);
                }
                catch (Exception ex)
                {
                    _errorProvider.SetError(_precision, ex.Message);
                    return false;
                }
            }

            DialogProperties.Representation = _representation.SelectedIndex;
            if (_timeSlot.Text != "")
            {
                try
                {
                    int value = Convert.ToInt32(_timeSlot.Text);
                    
                    if (value == 0)
                        value = 1;

                    DialogProperties.TimeSlot = (uint)value;
                }
                catch (Exception ex)
                {
                    _errorProvider.SetError(_timeSlot, ex.Message);
                    return false;
                }
            }

            DialogProperties.AxisTextColor  = _textColor.BackColor;
            
            if(OnChangeProperties != null)
                OnChangeProperties(DialogProperties);

            return true;
        }

        private void _bkColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = DialogProperties.BackColor;
            DialogResult res = dlg.ShowDialog();
            if( res == DialogResult.OK )
            {
                _bkColor.BackColor = dlg.Color;
               OnPropertieChange( sender, e);
            }
        }

        private void _textColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = DialogProperties.AxisTextColor;
            DialogResult res = dlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                _textColor.BackColor = dlg.Color;
                OnPropertieChange(sender, e);
            }
        }

        private void _degreeTextColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = DialogProperties.DegreeTxtColor;
            DialogResult res = dlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                _degreeTextColor.BackColor = dlg.Color;
                OnPropertieChange(sender, e);
            }
        }

        private void _lineColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = DialogProperties.LineColor;
            DialogResult res = dlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                _lineColor.BackColor = dlg.Color;
                OnPropertieChange(sender, e);
            }
        }
    }
}