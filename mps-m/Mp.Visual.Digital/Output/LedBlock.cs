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

namespace Mp.Visual.Digital
{
    [Serializable]
    public partial class LedBlock : Base.VisualBlock
    {
        private Color _colorOff;
        private Color _colorOn;
        private bool _on;
        private bool _showLabel;

        public LedBlock()
        {
            _colorOff = Color.Red;
            _colorOn = Color.Gray;
            _on = true;
            _showLabel = true;

            InitializeComponent();
            Width = 400;
            Height = 400;
        }

        public override void UpdateProperties()
        {
            foreach (LabelLed ctrl in base.ControlsInContainer)
            {
                ctrl.ColorOff = _colorOff;
                ctrl.ColorOn = _colorOn;
                ctrl.On = _on;
                ctrl.ShowLabel = _showLabel;
                ctrl.Invalidate();
            }

            base.UpdateProperties();
        }

        [Category("Appearance")]
        public Color ColorOff
        {
            get { return _colorOff; }
            set
            {
                _colorOff = value;
            }
        }

        [Category("Appearance")]
        public Color ColorOn
        {
            get { return _colorOn; }
            set
            {
                _colorOn = value;
                UpdateProperties();
            }
        }


        public bool On
        {
            get { return _on; }
            set
            {
                _on = value;
                UpdateProperties();
            }
        }

        public bool ShowLabel
        {
            get { return _showLabel; }

            set
            {
                _showLabel = value;
            }
        }
    }
}
