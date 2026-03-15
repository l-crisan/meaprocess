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

namespace Mp.Visual.XYChart
{
    public partial class XYChartDlg : Form
    {
        private Chart _chart;

        public XYChartDlg(Chart chart)
        {
            _chart = chart;            
            InitializeComponent();

            ctrlName.Text = chart.Title;
            bkColor.BackColor = chart.BackColor;
            lineColor.BackColor = chart.LineColor;
            pointColor.BackColor = chart.PointColor;
            ydiv.Text = chart.YAxisDevision.ToString();
            ypre.Text = chart.YAxisPrecision.ToString();
            ymin.Text = chart.YMinimum.ToString();
            ymax.Text = chart.YMaximum.ToString();

            xdiv.Text = chart.XAxisDevision.ToString();
            xpre.Text = chart.XAxisPrecision.ToString();
            xmin.Text = chart.XMinimum.ToString();
            xmax.Text = chart.XMaximum.ToString();

            gridColor.BackColor = chart.GridColor;
            gridXdiv.Text = chart.GridXDevision.ToString();
            gridYdiv.Text = chart.GridYDevision.ToString();
            gridStyle.SelectedIndex = (int)chart.GridLineStyle;
            axisColor.BackColor = chart.AxisColor;
            points.Text = chart.MaxPoints.ToString();
            xLog.Checked = chart.XLogarithmic;
            yLog.Checked = chart.YLogarithmic;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            
            _chart.EnableRepaint = false;

            double min = Convert.ToDouble(ymin.Text);
            double max = Convert.ToDouble(ymax.Text);

            if (min >= max)
            {
                errorProvider.SetError(ymax, StringResource.YMinMaxErr);
                return;
            }

            if (yLog.Checked)
            {
                if (min < 1)
                {
                    errorProvider.SetError(ymin, StringResource.LogRangeErr);
                    return;
                }

                if (max < 1)
                {
                    errorProvider.SetError(ymax, StringResource.LogRangeErr);
                    return;
                }
                _chart.YLogMin = Math.Log10(min);
                _chart.YLogMax = Math.Log10(max);
            }

            min = Convert.ToDouble(xmin.Text);
            max = Convert.ToDouble(xmax.Text);

            if (min >= max)
            {
                errorProvider.SetError(xmax, StringResource.XMinMaxErr);
                return;
            }

            if (xLog.Checked)
            {
                if (min < 1)
                {
                    errorProvider.SetError(xmin, StringResource.LogRangeErr);
                    return;
                }

                if (max < 1)
                {
                    errorProvider.SetError(xmax, StringResource.LogRangeErr);
                    return;
                }

                _chart.XLogMin = Math.Log10(min);
                _chart.XLogMax = Math.Log10(max);
            }

            _chart.MaxPoints = Convert.ToInt32(points.Text);
            _chart.Text = ctrlName.Text;
            _chart.BackColor = bkColor.BackColor;
            _chart.LineColor = lineColor.BackColor;
            _chart.PointColor = pointColor.BackColor;
            _chart.YAxisDevision = Convert.ToInt32(ydiv.Text);
            _chart.YAxisPrecision = Convert.ToInt32(ypre.Text);
            _chart.YMinimum = Convert.ToDouble(ymin.Text);
            _chart.YMaximum = Convert.ToDouble(ymax.Text);

            _chart.XAxisDevision = Convert.ToInt32(xdiv.Text);
            _chart.XAxisPrecision = Convert.ToInt32(xpre.Text);
            _chart.XMinimum = Convert.ToDouble(xmin.Text);
            _chart.XMaximum = Convert.ToDouble(xmax.Text);
            _chart.AxisColor = axisColor.BackColor;

            _chart.GridColor = gridColor.BackColor;
            _chart.GridXDevision = Convert.ToInt32(gridXdiv.Text);
            _chart.GridYDevision = Convert.ToInt32(gridYdiv.Text);
            _chart.GridLineStyle = (System.Drawing.Drawing2D.DashStyle) gridStyle.SelectedIndex;
            _chart.XLogarithmic = xLog.Checked;
            _chart.YLogarithmic = yLog.Checked;
            _chart.EnableRepaint = true;
            _chart.SynchMutex.WaitOne();
            _chart.RepaintAll();
            _chart.SynchMutex.ReleaseMutex();            

            DialogResult = DialogResult.OK;
            Close();
        }

        private void bkColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.AllowFullOpen = true;
            dlg.Color = bkColor.BackColor;
            
            if (dlg.ShowDialog() == DialogResult.OK)
                bkColor.BackColor = dlg.Color;


        }

        private void lineColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.AllowFullOpen = true;
            dlg.Color = lineColor.BackColor;

            if (dlg.ShowDialog() == DialogResult.OK)
                lineColor.BackColor = dlg.Color;
        }

        private void pointColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.AllowFullOpen = true;
            dlg.Color = pointColor.BackColor;

            if (dlg.ShowDialog() == DialogResult.OK)
                pointColor.BackColor = dlg.Color;
        }

        private void gridColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.AllowFullOpen = true;
            dlg.Color = gridColor.BackColor;

            if (dlg.ShowDialog() == DialogResult.OK)
                gridColor.BackColor = dlg.Color;
        }

        private void ydiv_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                Convert.ToUInt32(ydiv.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(ydiv, ex.Message);
                e.Cancel = true;
            }
        }

        private void ypre_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                Convert.ToUInt32(ypre.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(ypre, ex.Message);
                e.Cancel = true;
            }
        }

        private void ymin_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                Convert.ToDouble(ymin.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(ymin, ex.Message);
                e.Cancel = true;
            }
        }

        private void ymax_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                Convert.ToDouble(ymax.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(ymax, ex.Message);
                e.Cancel = true;
            }
        }

        private void xdiv_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                Convert.ToUInt32(xdiv.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(xdiv, ex.Message);
                e.Cancel = true;
            }
        }

        private void xpre_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                Convert.ToUInt32(xpre.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(xpre, ex.Message);
                e.Cancel = true;
            }
        }

        private void xmin_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                Convert.ToDouble(xmin.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(xmin, ex.Message);
                e.Cancel = true;
            }
        }

        private void xmax_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                Convert.ToDouble(xmax.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(xmax, ex.Message);
                e.Cancel = true;
            }
        }

        private void gridXdiv_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                Convert.ToUInt32(gridXdiv.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(gridXdiv, ex.Message);
                e.Cancel = true;
            }
        }

        private void gridYdiv_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                Convert.ToUInt32(gridYdiv.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(gridYdiv, ex.Message);
                e.Cancel = true;
            }
        }

        private void points_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                Convert.ToUInt32(points.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(points, ex.Message);
                e.Cancel = true;
            }
        }

        private void axisColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.AllowFullOpen = true;
            dlg.Color = axisColor.BackColor;

            if (dlg.ShowDialog() == DialogResult.OK)
                axisColor.BackColor = dlg.Color;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void XYChartDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
