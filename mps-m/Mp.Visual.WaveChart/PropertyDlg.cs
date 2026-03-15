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
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Mp.Visual.WaveChart
{
    /// <summary>
    /// The wave chart property dialog.
    /// </summary>
    public partial class PropertyDlg : Form
    {
        /// <summary>
        /// The dialog data
        /// </summary>
        public class DialogData
        {
            /// <summary>
            /// The wave chart name.
            /// </summary>
            public string    Name;

            /// <summary>
            /// The wave char background color.
            /// </summary>
            public Color     BackgroundColor;

            /// <summary>
            /// The wave chart grid line color.
            /// </summary>
            public Color     LineColor;

            /// <summary>
            /// The wave chart grid margine.
            /// </summary>
            public int       Margin;

            /// <summary>
            /// The wave chart grid x devision.
            /// </summary>
            public int       XDevision;

            /// <summary>
            /// The wave chart grid y devision.
            /// </summary>
            public int       YDevision;

            /// <summary>
            /// The wave chart grid line width.
            /// </summary>
            public int       LineWidth;

            /// <summary>
            /// The wave chart grid line style.
            /// </summary>
            public DashStyle LineStyle;

            public uint ChartRate;
        }

        /// <summary>
        /// The data change delegate.
        /// </summary>
        /// <param name="properties"></param>
        public delegate void UpdateProperties(DialogData properties);

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PropertyDlg()
        { InitializeComponent(); }

        private void PoChartPropDlg_Load(object sender, EventArgs e)
        {
            ApplyOnChange.Checked = false;
            _name.Text = _data.Name;
            _bkColor.BackColor = _data.BackgroundColor;
            _margine.Text = _data.Margin.ToString();
            ctrlLineStyle.SelectedIndex = 0;

            switch (_data.LineStyle)
            {
                case DashStyle.Dash:
                    ctrlLineStyle.SelectedIndex = 0;
                break;
                case DashStyle.DashDot:
                    ctrlLineStyle.SelectedIndex = 1;
                break;
                case DashStyle.DashDotDot:
                    ctrlLineStyle.SelectedIndex = 2;
                break;
                case DashStyle.Dot:
                    ctrlLineStyle.SelectedIndex = 3;
                break;
                case DashStyle.Solid:
                    ctrlLineStyle.SelectedIndex = 4;
                break;
            }

            _lineWidth.Text = _data.LineWidth.ToString();
            _yDevision.Text = _data.YDevision.ToString();
            _xDevision.Text = _data.XDevision.ToString();
            _lineColor.BackColor = _data.LineColor;
            rate.Text = _data.ChartRate.ToString();
            ApplyOnChange.Checked = true;
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

        /// <summary>
        /// Gets the dialof data.
        /// </summary>
        public DialogData Data
        {
            get{ return _data; }
        }

        /// <summary>
        /// Emited when the dialog data is changed.
        /// </summary>
        public UpdateProperties OnUpdateProperties;

        private void Apply_Click(object sender, EventArgs e)
        {
            OnApply();
        }

        private bool OnApply()
        {
            _errorProvider.Clear();

            _data.BackgroundColor = _bkColor.BackColor;
            _data.LineColor = _lineColor.BackColor;

            try
            {
                _data.Margin = Convert.ToInt32( _margine.Text );
            }
            catch (Exception ex)
            {
                _errorProvider.SetError(_margine, ex.Message);
                return false;
            }

            try
            {
                _data.XDevision = Convert.ToInt32( _xDevision.Text );
            }
            catch (Exception ex)
            {
                _errorProvider.SetError(_xDevision, ex.Message);
                return false;
            }

            try
            {
                _data.YDevision = Convert.ToInt32( _yDevision.Text);
            }
            catch (Exception ex)
            {
                _errorProvider.SetError(_yDevision, ex.Message);
                return false;
            }

            try
            {

                _data.LineWidth = Convert.ToInt32( _lineWidth.Text);
            }
            catch (Exception ex)
            {
                _errorProvider.SetError(_lineWidth, ex.Message);
                return false;
            }
            try
            {
                _data.ChartRate = Convert.ToUInt32(rate.Text);
            }
            catch (Exception ex)
            {
                _errorProvider.SetError(rate, ex.Message);
                return false;
            }

            switch (ctrlLineStyle.SelectedIndex)
            {
                case 0:
                    _data.LineStyle = DashStyle.Dash;
                    break;
                case 1:
                    _data.LineStyle = DashStyle.DashDot;
                    break;
                case 2:
                    _data.LineStyle = DashStyle.DashDotDot;
                    break;
                case 3:
                    _data.LineStyle = DashStyle.Dot;
                    break;
                case 4:
                    _data.LineStyle = DashStyle.Solid;
                    break;
            }

            if( OnUpdateProperties != null )
                OnUpdateProperties(_data);
            return true;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (!ApplyOnChange.Checked)
                return;

            Apply_Click(sender, e);
        }

        private DialogData _data = new DialogData();

        private void _bkColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = _data.BackgroundColor;
            DialogResult res = dlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                _bkColor.BackColor = dlg.Color;
                OnValueChanged(sender, e);
            }
        }

        private void _lineColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = _lineColor.BackColor;
            DialogResult res = dlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                _lineColor.BackColor = dlg.Color;
                OnValueChanged(sender, e);
            }
        }

        private void rate_TextChanged(object sender, EventArgs e)
        {
            _errorProvider.Clear();

            try
            {
                Convert.ToUInt32(rate.Text);
            }
            catch(Exception ex)
            {
                _errorProvider.SetError(rate, ex.Message);
            }
        }
     }
}