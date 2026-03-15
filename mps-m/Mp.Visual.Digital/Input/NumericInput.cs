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
using System.Windows.Forms;

namespace Mp.Visual.Digital
{
    public class NumericInput : TextBox
    {
        public enum NumberT
        {
            SignedInteger,
            UnsignedInteger,
            Real,
        }

        private NumberT _ntype = NumberT.Real;
        private double _value = 0.0;
        private double _min = 0;
        private double _max = 100;

        public NumericInput()
        {
            this.Text = _value.ToString();
        }

        [SRDescription("NumberType")]
        [SRCategory("View")]
        public NumberT NumberType 
        {
            set { _ntype = value; }
            get { return _ntype; }
        }

        [SRDescription("Maximum")]
        [SRCategory("View")]
        public double Maximum
        {
            get { return _max; }
            set
            {
                try
                {
                    double max = _max;
                    max = CheckValue(value);

                    if (max > _min)
                        _max = max;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        [SRDescription("Minimum")]
        [SRCategory("View")]
        public double Minimum
        {
            get{ return _min;}
            set
            {
                try
                {
                    double min = _min;
                    min = CheckValue(value);
                    if (min < _max)
                        _min = min;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private double CheckValue(double value)
        {
            double newValue = 0.0;
            switch (_ntype)
            {
                case NumberT.SignedInteger:
                    newValue = (double)((int)(value));
                    break;

                case NumberT.UnsignedInteger:
                    if (value >= 0)
                        newValue = ((uint)value);
                    break;

                case NumberT.Real:
                    newValue = value;
                    break;
            }
            return newValue;
        }

        [SRDescription("Value")]
        [SRCategory("View")]
        public double Value 
        {
            get
            {
                return _value;
            }

            set
            {
                try
                {
                    double val = CheckValue(value);

                    if (val >= _min && val <= _max)
                    {
                        this.Text = val.ToString();
                        _value = val;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            
            if (e.KeyCode == Keys.Enter)
                UpdateValue();
        }
        
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            UpdateValue();
        }

        private void UpdateValue()
        {
            try
            {
                double value = _value;
                switch (_ntype)
                {
                    case NumberT.SignedInteger:
                        value = Convert.ToInt32(this.Text);
                        break;

                    case NumberT.UnsignedInteger:
                        value = Convert.ToUInt32(this.Text);
                        break;

                    case NumberT.Real:
                        value = Convert.ToDouble(this.Text);
                        break;
                }

                if (value >= _min && value <= _max)
                    _value = value;
                else
                    this.Text = _value.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                this.Text = _value.ToString();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NumericInputBox
            // 
            this.AcceptsReturn = true;
            this.ResumeLayout(false);
        }
    }
}
