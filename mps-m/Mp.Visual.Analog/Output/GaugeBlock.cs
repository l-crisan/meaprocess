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
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Design;

namespace Mp.Visual.Analog
{
    public partial class GaugeBlock : Mp.Visual.Base.VisualBlock
    {

        private RangeList _ranges;
        private CaptionList _caption;
        private Point _center;
        private Single _minValue = -10;
        private Single _maxValue = 10;
        private Color _baseArcColor;
        private int _baseArcRadius;
        private int _baseArcStart;
        private int _baseArcSweep;
        private int _baseArcWidth;
        private Color _scaleLinesInterColor;
        private int _scaleLinesInterInnerRadius;
        private int _scaleLinesInterOuterRadius;
        private int _scaleLinesInterWidth;
        private int _scaleLinesMinorNumOf;
        private Color _scaleLinesMinorColor;
        private int _scaleLinesMinorInnerRadius;
        private int _scaleLinesMinorOuterRadius;
        private int _scaleLinesMinorWidth;
        private float _scaleLinesMajorStepValue;
        private Color _scaleLinesMajorColor;
        private int _scaleLinesMajorInnerRadius;
        private int _scaleLinesMajorOuterRadius;
        private int _scaleLinesMajorWidth;
        private int _scaleNumbersRadius;
        private Color _scaleNumbersColor;
        private string _scaleNumbersFormat;
        private int _scaleNumbersStartScaleLine;
        private int _scaleNumbersStepScaleLines;
        private int _scaleNumbersRotation;
        private  Gauge.NeedleKind _needleKind;
        private int _needleRadius;
        private Gauge.NeedleColorEnum _needleColor1;
        private Color _needleColor2;
        private int _needleWidth;

        public GaugeBlock()
        {
            InitializeComponent();
            Gauge ctrl = new Gauge();
            
            _ranges = ctrl.RangeDefinition;
            _caption = ctrl.CaptionDefinition;
            _center = ctrl.Center;
            _baseArcColor = ctrl.BaseArcColor;
            _baseArcRadius = ctrl.BaseArcRadius;
            _baseArcStart = ctrl.BaseArcStart;
            _baseArcSweep = ctrl.BaseArcSweep;
            _baseArcWidth = ctrl.BaseArcWidth;
            _scaleLinesInterColor = ctrl.ScaleLinesInterColor;
            _scaleLinesInterInnerRadius = ctrl.ScaleLinesInterInnerRadius;
            _scaleLinesInterOuterRadius = ctrl.ScaleLinesInterOuterRadius;
            _scaleLinesInterWidth = ctrl.ScaleLinesInterWidth;
            _scaleLinesMinorNumOf = ctrl.ScaleLinesMinorNumOf;
            _scaleLinesMinorColor = ctrl.ScaleLinesMinorColor;
            _scaleLinesMinorInnerRadius = ctrl.ScaleLinesMinorInnerRadius;
            _scaleLinesMinorOuterRadius = ctrl.ScaleLinesMinorOuterRadius;
            _scaleLinesMinorWidth = ctrl.ScaleLinesMinorWidth;
            _scaleLinesMajorStepValue = ctrl.ScaleLinesMajorStepValue;
            _scaleLinesMajorColor = ctrl.ScaleLinesMajorColor;
            _scaleLinesMajorInnerRadius = ctrl.ScaleLinesMajorInnerRadius;
            _scaleLinesMajorOuterRadius = ctrl.ScaleLinesMajorOuterRadius;
            _scaleLinesMajorWidth = ctrl.ScaleLinesMajorWidth;
            _scaleNumbersRadius = ctrl.ScaleNumbersRadius;
            _scaleNumbersColor = ctrl.ScaleNumbersColor;
            _scaleNumbersFormat = ctrl.ScaleNumbersFormat;
            _scaleNumbersStartScaleLine = ctrl.ScaleNumbersStartScaleLine;
            _scaleNumbersStepScaleLines = ctrl.ScaleNumbersStepScaleLines;
            _scaleNumbersRotation = ctrl.ScaleNumbersRotation;
            _needleKind = ctrl.NeedleType;
            _needleRadius = ctrl.NeedleRadius;
            _needleColor1 = ctrl.NeedleColor1;
            _needleColor2 = ctrl.NeedleColor2;
            _needleWidth = ctrl.NeedleWidth;            
        }

        public override void UpdateProperties()
        {
            base.UpdateProperties();

            foreach (Gauge ctrl in base.ControlsInContainer)
            {      
            
                for(int i = 0; i < _ranges.Count; ++i)
                    ctrl.RangeDefinition[i].Copy(_ranges[i]);

                for(int i = 0; i < _caption.Count; ++i)
                {
                    Caption c = _caption[i];
                    ctrl.CaptionDefinition[i].Position = c.Position;
                    ctrl.CaptionDefinition[i].CapColor = c.CapColor;
                }
                
                ctrl.Center = _center;
                ctrl.BaseArcColor = _baseArcColor;
                ctrl.BaseArcRadius = _baseArcRadius;
                ctrl.BaseArcStart = _baseArcStart;
                ctrl.BaseArcSweep = _baseArcSweep;
                ctrl.BaseArcWidth = _baseArcWidth;
                ctrl.ScaleLinesInterColor = _scaleLinesInterColor;
                ctrl.ScaleLinesInterInnerRadius = _scaleLinesInterInnerRadius;
                ctrl.ScaleLinesInterOuterRadius = _scaleLinesInterOuterRadius;
                ctrl.ScaleLinesInterWidth = _scaleLinesInterWidth;
                ctrl.ScaleLinesMinorNumOf = _scaleLinesMinorNumOf;
                ctrl.ScaleLinesMinorColor = _scaleLinesMinorColor;
                ctrl.ScaleLinesMinorInnerRadius = _scaleLinesMinorInnerRadius;
                ctrl.ScaleLinesMinorOuterRadius = _scaleLinesMinorOuterRadius;
                ctrl.ScaleLinesMinorWidth = _scaleLinesMinorWidth;
                ctrl.ScaleLinesMajorStepValue = _scaleLinesMajorStepValue;
                ctrl.ScaleLinesMajorColor = _scaleLinesMajorColor;
                ctrl.ScaleLinesMajorInnerRadius = _scaleLinesMajorInnerRadius;
                ctrl.ScaleLinesMajorOuterRadius =_scaleLinesMajorOuterRadius;
                ctrl.ScaleLinesMajorWidth = _scaleLinesMajorWidth;
                ctrl.ScaleNumbersRadius = _scaleNumbersRadius;
                ctrl.ScaleNumbersColor = _scaleNumbersColor;
                ctrl.ScaleNumbersFormat = _scaleNumbersFormat;
                ctrl.ScaleNumbersStartScaleLine = _scaleNumbersStartScaleLine;
                ctrl.ScaleNumbersStepScaleLines = _scaleNumbersStepScaleLines;
                ctrl.ScaleNumbersRotation = _scaleNumbersRotation;
                ctrl.NeedleType = _needleKind;
                ctrl.MinValue  = _minValue;
                ctrl.MaxValue = _maxValue;
                ctrl.NeedleRadius = _needleRadius;
                ctrl.NeedleColor1 = _needleColor1;
                ctrl.NeedleColor2 = _needleColor2;
                ctrl.NeedleWidth = _needleWidth;
                ctrl.BackColor = this.BackColor;        
                ctrl.Invalidate();
            }            
        }

        [System.ComponentModel.Browsable(true), SRCategory("Range"), SRDescription("RangeDefinition"), System.ComponentModel.Editor(typeof(GaugeUIEditor), typeof(UITypeEditor))]
        public RangeList RangeDefinition
        {
            set
            {
                _ranges = value;
            }

            get { return _ranges; }
        }

        [System.ComponentModel.Browsable(true),
         SRCategory("Caption"),
         SRDescription("CaptionDefinition"),
         System.ComponentModel.Editor(typeof(GaugeUIEditor), typeof(UITypeEditor))]
        public CaptionList CaptionDefinition
        {
            set
            {
                _caption = value;
            }

            get { return _caption; }
        }

        [System.ComponentModel.Browsable(false),
        SRCategory("Gauge"),
        SRDescription("Center")]
        public Point Center
        {
            get
            {
                return _center;
            }
            set
            {
                _center = value;
            }                
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Range"),
        SRDescription("MinValue")]
        public Single MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                _minValue = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Range"),
        SRDescription("MaxValue")]
        public Single MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
               _maxValue = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Arc"),
        SRDescription("BaseArcColor")]
        public Color BaseArcColor
        {
            get
            {
                return _baseArcColor;
            }
            set
            {
                _baseArcColor = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Arc"),
        SRDescription("BaseArcRadius")]
        public int BaseArcRadius
        {
            get
            {
                return _baseArcRadius;
            }
            set
            {
                _baseArcRadius = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Arc"),
        SRDescription("BaseArcStart")]
        public Int32 BaseArcStart
        {
            get
            {
                return _baseArcStart;
            }
            set
            {
      
                _baseArcStart =  value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Arc"),
        SRDescription("BaseArcSweep")]
        public Int32 BaseArcSweep
        {
            get
            {
                return _baseArcSweep;
            }
            set
            {
                _baseArcSweep = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Arc"),
        SRDescription("BaseArcWidth")]
        public Int32 BaseArcWidth
        {
            get
            {
                return _baseArcWidth;
            }
            set
            {
                _baseArcWidth = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesInterColor")]
        public Color ScaleLinesInterColor
        {
            get
            {
                return _scaleLinesInterColor;
            }
            set
            {
                _scaleLinesInterColor = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesInterInnerRadius")]
        public Int32 ScaleLinesInterInnerRadius
        {
            get
            {
                return _scaleLinesInterInnerRadius;
            }
            set
            {
                _scaleLinesInterInnerRadius = value;
            }
        }

        [System.ComponentModel.Browsable(true),
         SRCategory("ScaleLines"),
         SRDescription("ScaleLinesInterOuterRadius")]
        public Int32 ScaleLinesInterOuterRadius
        {
            get
            {
                return _scaleLinesInterOuterRadius;
            }
            set
            {
                _scaleLinesInterOuterRadius = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesInterWidth")]
        public Int32 ScaleLinesInterWidth
        {
            get
            {
                return _scaleLinesInterWidth;
            }
            set
            {
                _scaleLinesInterWidth = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMinorNumOf")]
        public Int32 ScaleLinesMinorNumOf
        {
            get
            {
                return _scaleLinesMinorNumOf;
            }
            set
            {
                _scaleLinesMinorNumOf = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMinorColor")]
        public Color ScaleLinesMinorColor
        {
            get
            {
                return _scaleLinesMinorColor;
            }
            set
            {
                _scaleLinesMinorColor = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMinorInnerRadius")]
        public Int32 ScaleLinesMinorInnerRadius
        {
            get
            {
                return _scaleLinesMinorInnerRadius;
            }
            set
            {
                _scaleLinesMinorInnerRadius = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMinorOuterRadius")]
        public Int32 ScaleLinesMinorOuterRadius
        {
            get
            {
                return _scaleLinesMinorOuterRadius;
            }
            set
            {
                _scaleLinesMinorOuterRadius = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMinorWidth")]
        public Int32 ScaleLinesMinorWidth
        {
            get
            {
                return _scaleLinesMinorWidth;
            }
            set
            {
                _scaleLinesMinorWidth = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMajorStepValue")]
        public Single ScaleLinesMajorStepValue
        {
            get
            {
                return _scaleLinesMajorStepValue;
            }
            set
            {
                _scaleLinesMajorStepValue = value;
            }
        }
        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMajorColor")]
        public Color ScaleLinesMajorColor
        {
            get
            {
                return _scaleLinesMajorColor;
            }
            set
            {
                _scaleLinesMajorColor = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMajorInnerRadius")]
        public Int32 ScaleLinesMajorInnerRadius
        {
            get
            {
                return _scaleLinesMajorInnerRadius;
            }
            set
            {
                _scaleLinesMajorInnerRadius = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMajorOuterRadius")]
        public Int32 ScaleLinesMajorOuterRadius
        {
            get
            {
                return _scaleLinesMajorOuterRadius;
            }
            set
            {
                _scaleLinesMajorOuterRadius = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMajorWidth")]
        public Int32 ScaleLinesMajorWidth
        {
            get
            {
                return _scaleLinesMajorWidth;
            }
            set
            {
                _scaleLinesMajorWidth = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleNumbers"),
        SRDescription("ScaleNumbersRadius")]
        public Int32 ScaleNumbersRadius
        {
            get
            {
                return _scaleNumbersRadius;
            }
            set
            {
                _scaleNumbersRadius = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleNumbers"),
        SRDescription("ScaleNumbersColor")]
        public Color ScaleNumbersColor
        {
            get
            {
                return _scaleNumbersColor;
            }
            set
            {
                _scaleNumbersColor = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleNumbers"),
        SRDescription("ScaleNumbersFormat")]
        public String ScaleNumbersFormat
        {
            get
            {
                return _scaleNumbersFormat;
            }
            set
            {
                _scaleNumbersFormat = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleNumbers"),
        SRDescription("ScaleNumbersStartScaleLine")]
        public Int32 ScaleNumbersStartScaleLine
        {
            get
            {
                return _scaleNumbersStartScaleLine;
            }
            set
            {
                _scaleNumbersStartScaleLine = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleNumbers"),
        SRDescription("ScaleNumbersStepScaleLines")]
        public Int32 ScaleNumbersStepScaleLines
        {
            get
            {
                return _scaleNumbersStepScaleLines;
            }
            set
            {
                _scaleNumbersStepScaleLines = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleNumbers"),
        SRDescription("ScaleNumbersRotation")]
        public Int32 ScaleNumbersRotation
        {
            get
            {
                return _scaleNumbersRotation;
            }
            set
            {
                _scaleNumbersRotation = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Needle"),
        SRDescription("NeedleType")]
        public Gauge.NeedleKind NeedleType
        {
            get
            {
                return _needleKind;
            }
            set
            {
                _needleKind = value;                
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Needle"),
        SRDescription("NeedleRadius")]
        public Int32 NeedleRadius
        {
            get
            {
                return _needleRadius;
            }
            set
            {
                _needleRadius = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Needle"),
        SRDescription("NeedleColor1")]
        public Gauge.NeedleColorEnum NeedleColor1
        {
            get
            {
                return _needleColor1;
            }
            set
            {
                _needleColor1 = value;
            }
        }

        [System.ComponentModel.Browsable(true),
       SRCategory("Needle"),
        SRDescription("NeedleColor2")]
        public Color NeedleColor2
        {
            get
            {
                return _needleColor2;
            }
            set
            {
                _needleColor2 = value;
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Needle"),
        SRDescription("NeedleWidth")]
        public Int32 NeedleWidth
        {
            get
            {
                return _needleWidth;
            }
            set
            {
                _needleWidth = value;
            }
        }             
    }
}
