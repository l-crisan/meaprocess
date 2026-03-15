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
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Mp.Visual.Analog;

namespace Mp.Visual.Oscilloscope
{
    public delegate void TriggerChangedDelegate();

    public partial class ChannelControl : UserControl
    {
        public enum TriggerType
        {
            RisingEdge = 0,
            FallingEdge,
            ExternOnOff,
            ExternRisingEdge, 
            ExternFallingEdge
        }
        private Rectangle _drawRect;
        private int _axisDivision = 10;
        private Mutex _mutex;
        private int _index = 0;
        private double _triggerRate;
        private double _verticalFactor = 0;
        private double _ymin = 0;
        private TriggerType _chTriggerType = TriggerType.RisingEdge;        

        public Queue<double> DataQueue = new Queue<double>();
        public double[] Data;
        public double LastValue = 0;
        public double LastTriggerValue = 0;
        public double TriggerLevelValue = 0;
        public int HorizontalPosValue = 0;
        public Color ChColor = Color.White;
        public double YScaleValue = 0.0;        
        public Bitmap BackBuffer = new Bitmap(100, 100);
        public int VerticalPosValue = 0;
        public int Coupling = 1;
        public double SampleRate;
        public double MinValue = 0;
        public double MaxValue = 1;
        public double CalcMinValue = 0;
        public double CalcMaxValue = 1;
        public string ChName = "";
        public bool InvertChn;
        public int PeriodeCounter;
        public double Periode;
        public int TriggerPos = -1;
        public int SampleCounter = 0;
        public bool SingleShot = false;
        public double Average = 0;
        public bool CopyRecordFlag = false;

        public event TriggerChangedDelegate OnTriggerChanged;

        public ChannelControl()
        {
            InitializeComponent();
            triggerCtrl.SelectedIndex = 0;
            couplingCtrl.SelectedIndex = 1;
            scaleYCtrl.Text = Convert.ToString(0.1);

            YScaleValue = Convert.ToDouble(scaleYCtrl.Text);
            TriggerLevelValue = 0.0;
            UseExternTrigger = false;
        }

        public void ResetSingleShot()
        {
           singleShotBt.Checked = false;
        }

        public TriggerType ChTriggerType
        {
            set 
            { 
                _chTriggerType = value;
                triggerCtrl.SelectedIndex = (int)_chTriggerType;
                triggerCtrl_SelectedIndexChanged(null, null);
            }

            get { return _chTriggerType; }
        }

        public double TriggerSampleRate
        {
            set
            {
                _triggerRate = value;
                UseExternTrigger = _triggerRate != 0;
            }
            get { return _triggerRate; }
        }
        
        public double RawHorPosValue
        {
            get { return horizontalPosCtrl.Value; }
        }

        public string ScaleY
        {
            set 
            { 
                scaleYCtrl.Text = value;
                scaleYCtrl_Validated(null, null);
            }

            get { return scaleYCtrl.Text; }
        }

        public double RawVerPos
        {
            get { return verticalPosCtrl.Value; }
        }

        public int Index
        {
            set 
            { 
                _index = value;
                channelPanelCtrl.Text = StringResource.Channel + " " + (_index + 1).ToString() + ":";
            }

            get { return _index; }
        }

        private bool UseExternTrigger
        {
            set
            {
                if (value)
                {
                    triggerCtrl.Items.Clear();
                    triggerCtrl.Items.Add(StringResource.RisingEdge);
                    triggerCtrl.Items.Add(StringResource.FallingEdge);
                    triggerCtrl.Items.Add(StringResource.ExternOnOff);
                    triggerCtrl.Items.Add(StringResource.ExternRrisingEdge);
                    triggerCtrl.Items.Add(StringResource.ExternFallingEdge);
                }
                else
                {
                    triggerCtrl.Items.Clear();
                    triggerCtrl.Items.Add(StringResource.RisingEdge);
                    triggerCtrl.Items.Add(StringResource.FallingEdge);
                }
                triggerCtrl.SelectedIndex = 0;
            }
        }

        public void SetXYMode()
        {
            visibleCtrl.Checked = false;
            visibleCtrl.Enabled = false;
            triggerCtrl.SelectedIndex = 0;
            triggerCtrl.Enabled = false;
            triggerLevelGroup.Enabled = false;
            verPosGroup.Enabled = false;
            horPosGroup.Enabled = false;
            singleShotBt.Enabled = false;
        }

        public void ResetXYMode()
        {
            visibleCtrl.Checked = true;
            visibleCtrl.Enabled = true;
            invertCtrl.Enabled = true;
            triggerCtrl.Enabled = true;
            couplingCtrl.Enabled = true;
            triggerLevelGroup.Enabled = true;
            verPosGroup.Enabled = true;
            horPosGroup.Enabled = true;
            singleShotBt.Enabled = true;
        }

        public bool ChnVisible
        {
            get { return visibleCtrl.Checked; }
            set { visibleCtrl.Checked = value; }
        }

        public Rectangle DrawRect
        {
            set { _drawRect = value; }
            get { return _drawRect; }
        }

        public int AxisDivision
        {
            get { return _axisDivision; }
            set { _axisDivision = value; }

        }

        public Mutex SyncMutex
        {
            set { _mutex = value; }
            get { return _mutex; }
        }

        public double RawTriggerLevelValue
        {
            get { return triggerLevelCtrl.Value; }
        }

        public void InitSizes()
        {
            triggerLevelCtrl.MinValue = 0;
            triggerLevelCtrl.MaxValue = _drawRect.Height;
            triggerLevelCtrl.Value = _drawRect.Height / 2;

            verticalPosCtrl.MinValue = 0;
            verticalPosCtrl.MaxValue = _drawRect.Height;
            verticalPosCtrl.Value = _drawRect.Height / 2;

            horizontalPosCtrl.MinValue = 0;
            horizontalPosCtrl.MaxValue = _drawRect.Width;
            horizontalPosCtrl.Value = _drawRect.Width / 2;
            scaleYCtrl_Validated(null, null);
        }

        private void invertCtrl_CheckedChanged(object sender, EventArgs e)
        {
            InvertChn = invertCtrl.Checked;
        }
     
        public double PixelYToValue(int y, double resolution)
        {
            double ymin = -(_axisDivision * resolution * 3) / 2.0;
            double ymax = (_axisDivision * resolution * 3) / 2.0;

            double factor = (ymin - ymax) / (double)(BackBuffer.Height);

            double offset = (ymax * BackBuffer.Height) / (double)BackBuffer.Height;

            return (double)(y * factor + offset);
        }

        private void scaleYCtrl_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            Control ctrl = (Control)sender;
            try
            {
                double scale = Convert.ToDouble(ctrl.Text);
                if (scale == 0.0)
                {
                    errorProvider.SetError(ctrl, StringResource.ZeroValueErr);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                errorProvider.SetError(ctrl, ex.Message);
                e.Cancel = true;
            }

        }

        private void scaleYCtrl_Validated(object sender, EventArgs e)
        {
            YScaleValue = Convert.ToDouble(scaleYCtrl.Text);
            TriggerLevelValue = PixelYToValue(2 * _drawRect.Height - (int)triggerLevelCtrl.Value, YScaleValue);

            //Calculate the new point for the signal.
            double difference = 0.000000000001;
            _ymin = -(_axisDivision / 2.0 * YScaleValue) * 3;
            double ymax = (_axisDivision / 2.0 * YScaleValue) * 3;

            //Calculate the new point for the signal.
            if (Math.Abs(ymax - _ymin) > difference)
                _verticalFactor = (double)BackBuffer.Height / (ymax - _ymin);
            else
                _verticalFactor = 0;
        }

        private void triggerCtrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            PeriodeCounter = 0;

            _chTriggerType = (TriggerType)triggerCtrl.SelectedIndex;            

            if (OnTriggerChanged != null)
                OnTriggerChanged();
        }

        private void couplingCtrl_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Coupling = couplingCtrl.SelectedIndex;
        }

        private void triggerLevelCtrl_KnobChangeValue(object sender, LBKnobEventArgs e)
        {
            _mutex.WaitOne();
            TriggerLevelValue = PixelYToValue(2 * _drawRect.Height - (int)triggerLevelCtrl.Value, YScaleValue);
            TriggerPos = -1;
            _mutex.ReleaseMutex();
        }

        private void resetTriggerCtrl_Click(object sender, EventArgs e)
        {
            triggerLevelCtrl.Value = _drawRect.Height / 2;
            TriggerLevelValue = 0;
        }

        private void verticalPosCtrl_KnobChangeValue(object sender, LBKnobEventArgs e)
        {
            VerticalPosValue = -(_drawRect.Height / 2 - (int)verticalPosCtrl.Value);
        }

        private void resetVerPosCtrl_Click(object sender, EventArgs e)
        {
            verticalPosCtrl.Value = _drawRect.Height / 2;
            verticalPosCtrl_KnobChangeValue(null, null);
        }

        private void horizontalPosCtrl_KnobChangeValue(object sender, LBKnobEventArgs e)
        {
            HorizontalPosValue = _drawRect.Width / 2 - (int)horizontalPosCtrl.Value;
        }

        public int ValueYToPixel(double y)
        {
            return (int)(BackBuffer.Height - ((y - _ymin) * _verticalFactor));
        }

        private void resetHorPosCtrl_Click(object sender, EventArgs e)
        {
            horizontalPosCtrl.Value = _drawRect.Width / 2;
            horizontalPosCtrl_KnobChangeValue(null, null);
        }

        private void singleShotBt_CheckedChanged(object sender, EventArgs e)
        {
            SingleShot = singleShotBt.Checked;
        }

    }
}

