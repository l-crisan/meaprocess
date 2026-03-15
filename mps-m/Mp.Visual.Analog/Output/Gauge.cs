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
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Drawing.Design;

namespace Mp.Visual.Analog
{ 
    [ToolboxBitmapAttribute(typeof(Gauge), "AGauge.bmp"), 
    DefaultEvent("ValueInRangeChanged"), 
    Description("Displays a value on an analog gauge. Raises an event if the value enters one of the definable ranges.")]
    [Serializable]
    public partial class Gauge : UserControl
    {
#region enum, var, delegate, event
        public enum NeedleColorEnum
        {
            Gray = 0,
            Red = 1,
            Green = 2,
            Blue = 3,
            Yellow = 4,
            Violet = 5,
            Magenta = 6
        };

        public enum NeedleKind
        {
            Needle3D = 0,
            Needle2D = 1,
        };

        private const Byte ZERO = 0;
        private const Byte NUMOFCAPS = 5;
        private const Byte NUMOFRANGES = 5;

        private Single fontBoundY1;
        private Single fontBoundY2;
        private Bitmap gaugeBitmap;
        private Boolean drawGaugeBackground = true;

        private Single m_value;
        private Boolean[] m_valueIsInRange = { false, false, false, false, false };

        private CaptionList _caption = new CaptionList();
        private Point m_Center = new Point(100, 100);
        private Single m_MinValue = -100;
        private Single m_MaxValue = 400;

        private Color m_BaseArcColor = Color.Gray;
        private Int32 m_BaseArcRadius = 80;
        private Int32 m_BaseArcStart = 135;
        private Int32 m_BaseArcSweep = 270;
        private Int32 m_BaseArcWidth = 2;

        private Color m_ScaleLinesInterColor = Color.Black;
        private Int32 m_ScaleLinesInterInnerRadius = 73;
        private Int32 m_ScaleLinesInterOuterRadius = 80;
        private Int32 m_ScaleLinesInterWidth = 1;

        private Int32 m_ScaleLinesMinorNumOf = 5;
        private Color m_ScaleLinesMinorColor = Color.Gray;
        private Int32 m_ScaleLinesMinorInnerRadius = 75;
        private Int32 m_ScaleLinesMinorOuterRadius = 80;
        private Int32 m_ScaleLinesMinorWidth = 1;

        private Single m_ScaleLinesMajorStepValue = 2.0f;
        private Color m_ScaleLinesMajorColor = Color.Black;
        private Int32 m_ScaleLinesMajorInnerRadius = 70;
        private Int32 m_ScaleLinesMajorOuterRadius = 80;
        private Int32 m_ScaleLinesMajorWidth = 2;

        private RangeList _ranges = new RangeList();

        private Int32 m_ScaleNumbersRadius = 95;
        private Color m_ScaleNumbersColor = Color.Black;
        private String m_ScaleNumbersFormat = "";
        private Int32 m_ScaleNumbersStartScaleLine;
        private Int32 m_ScaleNumbersStepScaleLines = 1;
        private Int32 m_ScaleNumbersRotation = 0;

        private NeedleKind m_NeedleType = 0;
        private Int32 m_NeedleRadius = 80;
        private NeedleColorEnum m_NeedleColor1 = NeedleColorEnum.Gray;
        private Color m_NeedleColor2 = Color.DimGray;
        private Int32 m_NeedleWidth = 2;
        private int _width;
        private int _height;
        private int _xerror;
        private int _yerror;
        private bool _first = true;


        public class ValueInRangeChangedEventArgs : EventArgs
        {
            public Int32 valueInRange;

            public ValueInRangeChangedEventArgs(Int32 valueInRange)
            {
                this.valueInRange = valueInRange;
            }
        }

        public delegate void ValueInRangeChangedDelegate(Object sender, ValueInRangeChangedEventArgs e);
        [Description("This event is raised if the value falls into a defined range.")]
        public event ValueInRangeChangedDelegate ValueInRangeChanged;
#endregion

#region hidden , overridden inherited properties
        public new Boolean AllowDrop
        {
            get
            {
                return false;
            }
            set
            {

            }
        }
        public new Boolean AutoSize
        {
            get
            {
                return false;
            }
            set
            {

            }
        }
        public new Boolean ForeColor
        {
            get
            {
                return false;
            }
            set
            {
            }
        }
        public new Boolean ImeMode
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public override System.Drawing.Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                drawGaugeBackground = true;
                Refresh();
            }
        }
        public override System.Drawing.Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                drawGaugeBackground = true;
                Refresh();
            }
        }
        public override System.Windows.Forms.ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                base.BackgroundImageLayout = value;
                drawGaugeBackground = true;
                Refresh();
            }
        }
        #endregion


        public Gauge()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            Width = 200;
            Height = 200;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;

            for (int i = 0; i < NUMOFRANGES; ++i)
            {               
                //Caption
                Caption cap = new Caption();
                cap.CapColor = Color.Black;
                cap.Position = new PointF(this.Width/2, this.Height/2);
                cap.Text = "";

                _caption.Add(cap);
                
                //Range
                Range range = new Range();
                range.Enabled = false;
                range.RangeColor = Color.FromKnownColor(KnownColor.Control);
                range.StartValue = 0.0f;
                range.EndValue = 0.0f;
                range.InnerRadius = 70;
                range.OuterRadius = 80;

                _ranges.Add(range);
            }
            _ranges[0].Enabled = true;
            _ranges[1].Enabled = true;

            _ranges[0].RangeColor = Color.LightGreen;
            _ranges[1].RangeColor = Color.Red;

            _ranges[0].StartValue = -10.0f;
            _ranges[0].EndValue = 8.0f;

            _ranges[1].StartValue = 8.0f;
            _ranges[1].EndValue = 10.0f;   
        }

        #region properties

        [System.ComponentModel.Browsable(true),
        SRCategory("Range"),
        SRDescription("RangeDefinition"),
        System.ComponentModel.Editor(typeof(GaugeUIEditor), typeof(UITypeEditor))]
        public RangeList RangeDefinition
        {
            set 
            { 
                _ranges = value;
                drawGaugeBackground = true;
                Refresh();
                Invalidate();
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
                drawGaugeBackground = true;
                Refresh();
                Invalidate();
            }

            get { return _caption; }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Gauge"),
        SRDescription("GaugeValue")]
        public Single Value
        {
            get
            {
                return m_value;
            }
            set
            {
                if (m_value != value)
                {
                    m_value = Math.Min(Math.Max(value, m_MinValue), m_MaxValue);
                    
                    if (this.DesignMode)
                    {
                        drawGaugeBackground = true;
                    }

                    for (Int32 counter = 0; counter < NUMOFRANGES - 1; counter++)
                    {
                        if ((_ranges[counter].StartValue <= m_value)
                        && (m_value <= _ranges[counter].EndValue)
                        && (_ranges[counter].Enabled))
                        {
                            if (!m_valueIsInRange[counter])
                            {
                                if (ValueInRangeChanged!=null)
                                {
                                    ValueInRangeChanged(this, new ValueInRangeChangedEventArgs(counter));
                                }
                            }
                        }
                        else
                        {
                            m_valueIsInRange[counter] = false;
                        }
                    }          
                    Invalidate();
                }
            }
        }


        [System.ComponentModel.Browsable(false),
        SRCategory("Gauge"),
        SRDescription("Center")]
        public Point Center
        {
            get
            {
                return m_Center;
            }
            set
            {
                m_Center = value;
                drawGaugeBackground = true;
                Refresh();
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Range"),
        SRDescription("MinValue")]
        public Single MinValue
        {
            get
            {
                return m_MinValue;
            }
            set
            {
                if ((m_MinValue != value)
                && (value < m_MaxValue))
                {
                    m_MinValue = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Range"),
        SRDescription("MaxValue")]
        public Single MaxValue
        {
            get
            {
                return m_MaxValue;
            }
            set
            {
                if ((m_MaxValue != value)
                && (value > m_MinValue))
                {
                    m_MaxValue = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Arc"),
        SRDescription("BaseArcColor")]
        public Color BaseArcColor
        {
            get
            {
                return m_BaseArcColor;
            }
            set
            {
                if (m_BaseArcColor != value)
                {
                    m_BaseArcColor = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Arc"),
        SRDescription("BaseArcRadius")]
        public Int32 BaseArcRadius
        {
            get
            {
                return m_BaseArcRadius;
            }
            set
            {
                if (m_BaseArcRadius != value)
                {
                    m_BaseArcRadius = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Arc"),
        SRDescription("BaseArcStart")]
        public Int32 BaseArcStart
        {
            get
            {
                return m_BaseArcStart;
            }
            set
            {
                if (m_BaseArcStart != value)
                {
                    m_BaseArcStart = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Arc"),
        SRDescription("BaseArcSweep")]
        public Int32 BaseArcSweep
        {
            get
            {
                return m_BaseArcSweep;
            }
            set
            {
                if (m_BaseArcSweep != value)
                {
                    m_BaseArcSweep = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Arc"),
        SRDescription("BaseArcWidth")]
        public Int32 BaseArcWidth
        {
            get
            {
                return m_BaseArcWidth;
            }
            set
            {
                if (m_BaseArcWidth != value)
                {
                    m_BaseArcWidth = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesInterColor")]
        public Color ScaleLinesInterColor
        {
            get
            {
                return m_ScaleLinesInterColor;
            }
            set
            {
                if (m_ScaleLinesInterColor != value)
                {
                    m_ScaleLinesInterColor = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesInterInnerRadius")]
        public Int32 ScaleLinesInterInnerRadius
        {
            get
            {
                return m_ScaleLinesInterInnerRadius;
            }
            set
            {
                if (m_ScaleLinesInterInnerRadius != value)
                {
                    m_ScaleLinesInterInnerRadius = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
         SRCategory("ScaleLines"),
         SRDescription("ScaleLinesInterOuterRadius")]
        public Int32 ScaleLinesInterOuterRadius
        {
            get
            {
                return m_ScaleLinesInterOuterRadius;
            }
            set
            {
                if (m_ScaleLinesInterOuterRadius != value)
                {
                    m_ScaleLinesInterOuterRadius = value;

                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesInterWidth")]
        public Int32 ScaleLinesInterWidth
        {
            get
            {
                return m_ScaleLinesInterWidth;
            }
            set
            {
                if (m_ScaleLinesInterWidth != value)
                {
                    m_ScaleLinesInterWidth = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMinorNumOf")]
        public Int32 ScaleLinesMinorNumOf
        {
            get
            {
                return m_ScaleLinesMinorNumOf;
            }
            set
            {
                if (m_ScaleLinesMinorNumOf != value)
                {
                    m_ScaleLinesMinorNumOf = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMinorColor")]
        public Color ScaleLinesMinorColor
        {
            get
            {
                return m_ScaleLinesMinorColor;
            }
            set
            {
                if (m_ScaleLinesMinorColor != value)
                {
                    m_ScaleLinesMinorColor = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMinorInnerRadius")]
        public Int32 ScaleLinesMinorInnerRadius
        {
            get
            {
                return m_ScaleLinesMinorInnerRadius;
            }
            set
            {
                if (m_ScaleLinesMinorInnerRadius != value)
                {
                    m_ScaleLinesMinorInnerRadius = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMinorOuterRadius")]
        public Int32 ScaleLinesMinorOuterRadius
        {
            get
            {
                return m_ScaleLinesMinorOuterRadius;
            }
            set
            {
                if (m_ScaleLinesMinorOuterRadius != value)
                {
                    m_ScaleLinesMinorOuterRadius = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMinorWidth")]
        public Int32 ScaleLinesMinorWidth
        {
            get
            {
                return m_ScaleLinesMinorWidth;
            }
            set
            {
                if (m_ScaleLinesMinorWidth != value)
                {
                    m_ScaleLinesMinorWidth = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMajorStepValue")]
        public Single ScaleLinesMajorStepValue
        {
            get
            {
                return m_ScaleLinesMajorStepValue;
            }
            set
            {
                if ((m_ScaleLinesMajorStepValue != value) && (value > 0))
                {
                    m_ScaleLinesMajorStepValue = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }
        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMajorColor")]
        public Color ScaleLinesMajorColor
        {
            get
            {
                return m_ScaleLinesMajorColor;
            }
            set
            {
                if (m_ScaleLinesMajorColor != value)
                {
                    m_ScaleLinesMajorColor = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMajorInnerRadius")]
        public Int32 ScaleLinesMajorInnerRadius
        {
            get
            {
                return m_ScaleLinesMajorInnerRadius;
            }
            set
            {
                if (m_ScaleLinesMajorInnerRadius != value)
                {
                    m_ScaleLinesMajorInnerRadius = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMajorOuterRadius")]
        public Int32 ScaleLinesMajorOuterRadius
        {
            get
            {
                return m_ScaleLinesMajorOuterRadius;
            }
            set
            {
                if (m_ScaleLinesMajorOuterRadius != value)
                {
                    m_ScaleLinesMajorOuterRadius = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleLines"),
        SRDescription("ScaleLinesMajorWidth")]
        public Int32 ScaleLinesMajorWidth
        {
            get
            {
                return m_ScaleLinesMajorWidth;
            }
            set
            {
                if (m_ScaleLinesMajorWidth != value)
                {
                    m_ScaleLinesMajorWidth = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleNumbers"),
        SRDescription("ScaleNumbersRadius")]
        public Int32 ScaleNumbersRadius
        {
            get
            {
                return m_ScaleNumbersRadius;
            }
            set
            {
                if (m_ScaleNumbersRadius != value)
                {
                    m_ScaleNumbersRadius = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleNumbers"),
        SRDescription("ScaleNumbersColor")]
        public Color ScaleNumbersColor
        {
            get
            {
                return m_ScaleNumbersColor;
            }
            set
            {
                if (m_ScaleNumbersColor != value)
                {
                    m_ScaleNumbersColor = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleNumbers"),
        SRDescription("ScaleNumbersFormat")]
        public String ScaleNumbersFormat
        {
            get
            {
                return m_ScaleNumbersFormat;
            }
            set
            {
                if (m_ScaleNumbersFormat != value)
                {
                    m_ScaleNumbersFormat = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleNumbers"),
        SRDescription("ScaleNumbersStartScaleLine")]
        public Int32 ScaleNumbersStartScaleLine
        {
            get
            {
                return m_ScaleNumbersStartScaleLine;
            }
            set
            {
                if (m_ScaleNumbersStartScaleLine != value)
                {
                    m_ScaleNumbersStartScaleLine = Math.Max(value, 1);
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleNumbers"),
        SRDescription("ScaleNumbersStepScaleLines")]
        public Int32 ScaleNumbersStepScaleLines
        {
            get
            {
                return m_ScaleNumbersStepScaleLines;
            }
            set
            {
                if (m_ScaleNumbersStepScaleLines != value)
                {
                    m_ScaleNumbersStepScaleLines = Math.Max(value, 1);
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("ScaleNumbers"),
        SRDescription("ScaleNumbersRotation")]
        public Int32 ScaleNumbersRotation
        {
            get
            {
                return m_ScaleNumbersRotation;
            }
            set
            {
                if (m_ScaleNumbersRotation != value)
                {
                    m_ScaleNumbersRotation = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Needle"),
        SRDescription("NeedleType")]
        public NeedleKind NeedleType
        {
            get
            {
                return m_NeedleType;
            }
            set
            {
                if (m_NeedleType != value)
                {
                    m_NeedleType = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Needle"),
        SRDescription("NeedleRadius")]
        public Int32 NeedleRadius
        {
            get
            {
                return m_NeedleRadius;
            }
            set
            {
                if (m_NeedleRadius != value)
                {
                    m_NeedleRadius = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Needle"),
        SRDescription("NeedleColor1")]
        public NeedleColorEnum NeedleColor1
        {
            get
            {
                return m_NeedleColor1;
            }
            set
            {
                if (m_NeedleColor1 != value)
                {
                    m_NeedleColor1 = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
       SRCategory("Needle"),
        SRDescription("NeedleColor2")]
        public Color NeedleColor2
        {
            get
            {
                return m_NeedleColor2;
            }
            set
            {
                if (m_NeedleColor2 != value)
                {
                    m_NeedleColor2 = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        [System.ComponentModel.Browsable(true),
        SRCategory("Needle"),
        SRDescription("NeedleWidth")]
        public Int32 NeedleWidth
        {
            get
            {
                return m_NeedleWidth;
            }
            set
            {
                if (m_NeedleWidth != value)
                {
                    m_NeedleWidth = value;
                    drawGaugeBackground = true;
                    Refresh();
                }
            }
        }
        #endregion

#region helper
        private void FindFontBounds()
        {
            //find upper and lower bounds for numeric characters
            Int32 c1;
            Int32 c2;
            Boolean boundfound;
            Bitmap b;
            Graphics g;
            SolidBrush backBrush = new SolidBrush(Color.White);
            SolidBrush foreBrush = new SolidBrush(Color.Black);
            SizeF boundingBox;

            b = new Bitmap(5, 5);
            g = Graphics.FromImage(b);
            boundingBox = g.MeasureString("0123456789", Font, -1, StringFormat.GenericTypographic);
            b = new Bitmap((Int32)(boundingBox.Width), (Int32)(boundingBox.Height));
            g = Graphics.FromImage(b);
            g.FillRectangle(backBrush, 0.0F, 0.0F, boundingBox.Width, boundingBox.Height);
            g.DrawString("0123456789", Font, foreBrush, 0.0F, 0.0F, StringFormat.GenericTypographic);

            fontBoundY1 = 0;
            fontBoundY2 = 0;
            c1 = 0;
            boundfound = false;
            while ((c1 < b.Height) && (!boundfound))
            {
                c2 = 0;
                while ((c2 < b.Width) && (!boundfound))
                {
                    if (b.GetPixel(c2, c1) != backBrush.Color)
                    {
                        fontBoundY1 = c1;
                        boundfound = true;
                    }
                    c2++;
                }
                c1++;
            }

            c1 = b.Height - 1;
            boundfound = false;
            while ((0 < c1) && (!boundfound))
            {
                c2 = 0;
                while ((c2 < b.Width) && (!boundfound))
                {
                    if (b.GetPixel(c2, c1) != backBrush.Color)
                    {
                        fontBoundY2 = c1;
                        boundfound = true;
                    }
                    c2++;
                }
                c1--;
            }
        }
#endregion

#region base member overrides
        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;

            UpdateSizes();
            _first = false;

            try
            {
                m_Center = new Point(this.Width / 2, this.Height / 2);
                

                if (drawGaugeBackground)
                {
                    drawGaugeBackground = false;

                    FindFontBounds();

                    Color back = BackColor;

                    gaugeBitmap = new Bitmap(Width, Height);
                    Graphics ggr = Graphics.FromImage(gaugeBitmap);

                    ggr.FillRectangle(new SolidBrush(back), ClientRectangle);

                    if (BackgroundImage != null)
                    {
                        switch (BackgroundImageLayout)
                        {
                            case ImageLayout.Center:
                                ggr.DrawImageUnscaled(BackgroundImage, Width / 2 - BackgroundImage.Width / 2, Height / 2 - BackgroundImage.Height / 2);
                                break;
                            case ImageLayout.None:
                                ggr.DrawImageUnscaled(BackgroundImage, 0, 0);
                                break;
                            case ImageLayout.Stretch:
                                ggr.DrawImage(BackgroundImage, 0, 0, Width, Height);
                                break;
                            case ImageLayout.Tile:
                                Int32 pixelOffsetX = 0;
                                Int32 pixelOffsetY = 0;
                                while (pixelOffsetX < Width)
                                {
                                    pixelOffsetY = 0;
                                    while (pixelOffsetY < Height)
                                    {
                                        ggr.DrawImageUnscaled(BackgroundImage, pixelOffsetX, pixelOffsetY);
                                        pixelOffsetY += BackgroundImage.Height;
                                    }
                                    pixelOffsetX += BackgroundImage.Width;
                                }
                                break;
                            case ImageLayout.Zoom:
                                if ((Single)(BackgroundImage.Width / Width) < (Single)(BackgroundImage.Height / Height))
                                {
                                    ggr.DrawImage(BackgroundImage, 0, 0, Height, Height);
                                }
                                else
                                {
                                    ggr.DrawImage(BackgroundImage, 0, 0, Width, Width);
                                }
                                break;
                        }
                    }


                    ggr.SmoothingMode = SmoothingMode.HighQuality;
                    //ggr.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    GraphicsPath gp = new GraphicsPath();
                    Single rangeStartAngle;
                    Single rangeSweepAngle;
                    for (Int32 counter = 0; counter < NUMOFRANGES; counter++)
                    {
                        if (_ranges[counter].EndValue > _ranges[counter].StartValue
                        && _ranges[counter].Enabled)
                        {
                            rangeStartAngle = m_BaseArcStart + (_ranges[counter].StartValue - m_MinValue) * m_BaseArcSweep / (m_MaxValue - m_MinValue);
                            rangeSweepAngle = (_ranges[counter].EndValue - _ranges[counter].StartValue) * m_BaseArcSweep / (m_MaxValue - m_MinValue);
                            gp.Reset();
                            gp.AddPie(new Rectangle(m_Center.X - _ranges[counter].OuterRadius, m_Center.Y - _ranges[counter].OuterRadius, 2 * _ranges[counter].OuterRadius, 2 * _ranges[counter].OuterRadius), rangeStartAngle, rangeSweepAngle);
                            gp.Reverse();
                            gp.AddPie(new Rectangle(m_Center.X - _ranges[counter].InnerRadius, m_Center.Y - _ranges[counter].InnerRadius, 2 * _ranges[counter].InnerRadius, 2 * _ranges[counter].InnerRadius), rangeStartAngle, rangeSweepAngle);
                            gp.Reverse();
                            ggr.SetClip(gp);
                            ggr.FillPie(new SolidBrush(_ranges[counter].RangeColor), new Rectangle(m_Center.X - _ranges[counter].OuterRadius, m_Center.Y - _ranges[counter].OuterRadius, 2 * _ranges[counter].OuterRadius, 2 * _ranges[counter].OuterRadius), rangeStartAngle, rangeSweepAngle);
                        }
                    }

                    ggr.SetClip(ClientRectangle);
                    if (m_BaseArcRadius > 0)
                    {
                        ggr.DrawArc(new Pen(m_BaseArcColor, m_BaseArcWidth), new Rectangle(m_Center.X - m_BaseArcRadius, m_Center.Y - m_BaseArcRadius, 2 * m_BaseArcRadius, 2 * m_BaseArcRadius), m_BaseArcStart, m_BaseArcSweep);
                    }

                    String valueText = "";
                    SizeF boundingBox;
                    Single countValue = 0;
                    Int32 counter1 = 0;
                    while (countValue <= (m_MaxValue - m_MinValue))
                    {
                        valueText = (m_MinValue + countValue).ToString(m_ScaleNumbersFormat);
                        ggr.ResetTransform();
                        boundingBox = ggr.MeasureString(valueText, Font, -1, StringFormat.GenericTypographic);

                        gp.Reset();
                        gp.AddEllipse(new Rectangle(m_Center.X - m_ScaleLinesMajorOuterRadius, m_Center.Y - m_ScaleLinesMajorOuterRadius, 2 * m_ScaleLinesMajorOuterRadius, 2 * m_ScaleLinesMajorOuterRadius));
                        gp.Reverse();
                        gp.AddEllipse(new Rectangle(m_Center.X - m_ScaleLinesMajorInnerRadius, m_Center.Y - m_ScaleLinesMajorInnerRadius, 2 * m_ScaleLinesMajorInnerRadius, 2 * m_ScaleLinesMajorInnerRadius));
                        gp.Reverse();
                        ggr.SetClip(gp);

                        ggr.DrawLine(new Pen(m_ScaleLinesMajorColor, m_ScaleLinesMajorWidth),
                        (Single)(Center.X),
                        (Single)(Center.Y),
                        (Single)(Center.X + 2 * m_ScaleLinesMajorOuterRadius * Math.Cos((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue)) * Math.PI / 180.0)),
                        (Single)(Center.Y + 2 * m_ScaleLinesMajorOuterRadius * Math.Sin((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue)) * Math.PI / 180.0)));

                        gp.Reset();
                        gp.AddEllipse(new Rectangle(m_Center.X - m_ScaleLinesMinorOuterRadius, m_Center.Y - m_ScaleLinesMinorOuterRadius, 2 * m_ScaleLinesMinorOuterRadius, 2 * m_ScaleLinesMinorOuterRadius));
                        gp.Reverse();
                        gp.AddEllipse(new Rectangle(m_Center.X - m_ScaleLinesMinorInnerRadius, m_Center.Y - m_ScaleLinesMinorInnerRadius, 2 * m_ScaleLinesMinorInnerRadius, 2 * m_ScaleLinesMinorInnerRadius));
                        gp.Reverse();
                        ggr.SetClip(gp);

                        if (countValue < (m_MaxValue - m_MinValue))
                        {
                            for (Int32 counter2 = 1; counter2 <= m_ScaleLinesMinorNumOf; counter2++)
                            {
                                if (((m_ScaleLinesMinorNumOf % 2) == 1) && ((Int32)(m_ScaleLinesMinorNumOf / 2) + 1 == counter2))
                                {
                                    gp.Reset();
                                    gp.AddEllipse(new Rectangle(m_Center.X - m_ScaleLinesInterOuterRadius, m_Center.Y - m_ScaleLinesInterOuterRadius, 2 * m_ScaleLinesInterOuterRadius, 2 * m_ScaleLinesInterOuterRadius));
                                    gp.Reverse();
                                    gp.AddEllipse(new Rectangle(m_Center.X - m_ScaleLinesInterInnerRadius, m_Center.Y - m_ScaleLinesInterInnerRadius, 2 * m_ScaleLinesInterInnerRadius, 2 * m_ScaleLinesInterInnerRadius));
                                    gp.Reverse();
                                    ggr.SetClip(gp);

                                    ggr.DrawLine(new Pen(m_ScaleLinesInterColor, m_ScaleLinesInterWidth),
                                    (Single)(Center.X),
                                    (Single)(Center.Y),
                                    (Single)(Center.X + 2 * m_ScaleLinesInterOuterRadius * Math.Cos((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue) + counter2 * m_BaseArcSweep / (((Single)((m_MaxValue - m_MinValue) / m_ScaleLinesMajorStepValue)) * (m_ScaleLinesMinorNumOf + 1))) * Math.PI / 180.0)),
                                    (Single)(Center.Y + 2 * m_ScaleLinesInterOuterRadius * Math.Sin((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue) + counter2 * m_BaseArcSweep / (((Single)((m_MaxValue - m_MinValue) / m_ScaleLinesMajorStepValue)) * (m_ScaleLinesMinorNumOf + 1))) * Math.PI / 180.0)));

                                    gp.Reset();
                                    gp.AddEllipse(new Rectangle(m_Center.X - m_ScaleLinesMinorOuterRadius, m_Center.Y - m_ScaleLinesMinorOuterRadius, 2 * m_ScaleLinesMinorOuterRadius, 2 * m_ScaleLinesMinorOuterRadius));
                                    gp.Reverse();
                                    gp.AddEllipse(new Rectangle(m_Center.X - m_ScaleLinesMinorInnerRadius, m_Center.Y - m_ScaleLinesMinorInnerRadius, 2 * m_ScaleLinesMinorInnerRadius, 2 * m_ScaleLinesMinorInnerRadius));
                                    gp.Reverse();
                                    ggr.SetClip(gp);
                                }
                                else
                                {
                                    ggr.DrawLine(new Pen(m_ScaleLinesMinorColor, m_ScaleLinesMinorWidth),
                                    (Single)(Center.X),
                                    (Single)(Center.Y),
                                    (Single)(Center.X + 2 * m_ScaleLinesMinorOuterRadius * Math.Cos((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue) + counter2 * m_BaseArcSweep / (((Single)((m_MaxValue - m_MinValue) / m_ScaleLinesMajorStepValue)) * (m_ScaleLinesMinorNumOf + 1))) * Math.PI / 180.0)),
                                    (Single)(Center.Y + 2 * m_ScaleLinesMinorOuterRadius * Math.Sin((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue) + counter2 * m_BaseArcSweep / (((Single)((m_MaxValue - m_MinValue) / m_ScaleLinesMajorStepValue)) * (m_ScaleLinesMinorNumOf + 1))) * Math.PI / 180.0)));
                                }
                            }
                        }

                        ggr.SetClip(ClientRectangle);

                        if (m_ScaleNumbersRotation != 0)
                        {
                            ggr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                            ggr.RotateTransform(90.0F + m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue));
                        }

                        ggr.TranslateTransform((Single)(Center.X + m_ScaleNumbersRadius * Math.Cos((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue)) * Math.PI / 180.0f)),
                                               (Single)(Center.Y + m_ScaleNumbersRadius * Math.Sin((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue)) * Math.PI / 180.0f)),
                                               System.Drawing.Drawing2D.MatrixOrder.Append);


                        if (counter1 >= ScaleNumbersStartScaleLine - 1)
                        {
                            ggr.DrawString(valueText, Font, new SolidBrush(m_ScaleNumbersColor), -boundingBox.Width / 2, -fontBoundY1 - (fontBoundY2 - fontBoundY1 + 1) / 2, StringFormat.GenericTypographic);
                        }

                        countValue += m_ScaleLinesMajorStepValue;
                        counter1++;
                    }

                    ggr.ResetTransform();
                    ggr.SetClip(ClientRectangle);

                    if (m_ScaleNumbersRotation != 0)
                    {
                        ggr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
                    }

                    for (Int32 counter = 0; counter < NUMOFCAPS; counter++)
                    {
                        if (_caption[counter].Text != "")
                        {
                            ggr.DrawString(_caption[counter].Text, Font, new SolidBrush(_caption[counter].CapColor), _caption[counter].Position.X, _caption[counter].Position.Y, StringFormat.GenericTypographic);
                        }
                    }
                }

                g.DrawImageUnscaled(gaugeBitmap, 0, 0);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                //g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                Single brushAngle = (Int32)(m_BaseArcStart + (m_value - m_MinValue) * m_BaseArcSweep / (m_MaxValue - m_MinValue)) % 360;
                Double needleAngle = brushAngle * Math.PI / 180;

                switch (m_NeedleType)
                {
                    case NeedleKind.Needle3D:
                        PointF[] points = new PointF[3];
                        Brush brush1 = Brushes.White;
                        Brush brush2 = Brushes.White;
                        Brush brush3 = Brushes.White;
                        Brush brush4 = Brushes.White;

                        Brush brushBucket = Brushes.White;
                        Int32 subcol = (Int32)(((brushAngle + 225) % 180) * 100 / 180);
                        Int32 subcol2 = (Int32)(((brushAngle + 135) % 180) * 100 / 180);

                        g.FillEllipse(new SolidBrush(m_NeedleColor2), Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6);
                        switch (m_NeedleColor1)
                        {
                            case NeedleColorEnum.Gray:
                                brush1 = new SolidBrush(Color.FromArgb(80 + subcol, 80 + subcol, 80 + subcol));
                                brush2 = new SolidBrush(Color.FromArgb(180 - subcol, 180 - subcol, 180 - subcol));
                                brush3 = new SolidBrush(Color.FromArgb(80 + subcol2, 80 + subcol2, 80 + subcol2));
                                brush4 = new SolidBrush(Color.FromArgb(180 - subcol2, 180 - subcol2, 180 - subcol2));
                                g.DrawEllipse(Pens.Gray, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6);
                                break;
                            case NeedleColorEnum.Red:
                                brush1 = new SolidBrush(Color.FromArgb(145 + subcol, subcol, subcol));
                                brush2 = new SolidBrush(Color.FromArgb(245 - subcol, 100 - subcol, 100 - subcol));
                                brush3 = new SolidBrush(Color.FromArgb(145 + subcol2, subcol2, subcol2));
                                brush4 = new SolidBrush(Color.FromArgb(245 - subcol2, 100 - subcol2, 100 - subcol2));
                                g.DrawEllipse(Pens.Red, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6);
                                break;
                            case NeedleColorEnum.Green:
                                brush1 = new SolidBrush(Color.FromArgb(subcol, 145 + subcol, subcol));
                                brush2 = new SolidBrush(Color.FromArgb(100 - subcol, 245 - subcol, 100 - subcol));
                                brush3 = new SolidBrush(Color.FromArgb(subcol2, 145 + subcol2, subcol2));
                                brush4 = new SolidBrush(Color.FromArgb(100 - subcol2, 245 - subcol2, 100 - subcol2));
                                g.DrawEllipse(Pens.Green, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6);
                                break;
                            case NeedleColorEnum.Blue:
                                brush1 = new SolidBrush(Color.FromArgb(subcol, subcol, 145 + subcol));
                                brush2 = new SolidBrush(Color.FromArgb(100 - subcol, 100 - subcol, 245 - subcol));
                                brush3 = new SolidBrush(Color.FromArgb(subcol2, subcol2, 145 + subcol2));
                                brush4 = new SolidBrush(Color.FromArgb(100 - subcol2, 100 - subcol2, 245 - subcol2));
                                g.DrawEllipse(Pens.Blue, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6);
                                break;
                            case NeedleColorEnum.Magenta:
                                brush1 = new SolidBrush(Color.FromArgb(subcol, 145 + subcol, 145 + subcol));
                                brush2 = new SolidBrush(Color.FromArgb(100 - subcol, 245 - subcol, 245 - subcol));
                                brush3 = new SolidBrush(Color.FromArgb(subcol2, 145 + subcol2, 145 + subcol2));
                                brush4 = new SolidBrush(Color.FromArgb(100 - subcol2, 245 - subcol2, 245 - subcol2));
                                g.DrawEllipse(Pens.Magenta, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6);
                                break;
                            case NeedleColorEnum.Violet:
                                brush1 = new SolidBrush(Color.FromArgb(145 + subcol, subcol, 145 + subcol));
                                brush2 = new SolidBrush(Color.FromArgb(245 - subcol, 100 - subcol, 245 - subcol));
                                brush3 = new SolidBrush(Color.FromArgb(145 + subcol2, subcol2, 145 + subcol2));
                                brush4 = new SolidBrush(Color.FromArgb(245 - subcol2, 100 - subcol2, 245 - subcol2));
                                g.DrawEllipse(Pens.Violet, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6);
                                break;
                            case NeedleColorEnum.Yellow:
                                brush1 = new SolidBrush(Color.FromArgb(145 + subcol, 145 + subcol, subcol));
                                brush2 = new SolidBrush(Color.FromArgb(245 - subcol, 245 - subcol, 100 - subcol));
                                brush3 = new SolidBrush(Color.FromArgb(145 + subcol2, 145 + subcol2, subcol2));
                                brush4 = new SolidBrush(Color.FromArgb(245 - subcol2, 245 - subcol2, 100 - subcol2));
                                g.DrawEllipse(Pens.Violet, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6);
                                break;
                        }

                        if (Math.Floor((Single)(((brushAngle + 225) % 360) / 180.0)) == 0)
                        {
                            brushBucket = brush1;
                            brush1 = brush2;
                            brush2 = brushBucket;
                        }

                        if (Math.Floor((Single)(((brushAngle + 135) % 360) / 180.0)) == 0)
                        {
                            brush4 = brush3;
                        }

                        points[0].X = (Single)(Center.X + m_NeedleRadius * Math.Cos(needleAngle));
                        points[0].Y = (Single)(Center.Y + m_NeedleRadius * Math.Sin(needleAngle));
                        points[1].X = (Single)(Center.X - m_NeedleRadius / 20 * Math.Cos(needleAngle));
                        points[1].Y = (Single)(Center.Y - m_NeedleRadius / 20 * Math.Sin(needleAngle));
                        points[2].X = (Single)(Center.X - m_NeedleRadius / 5 * Math.Cos(needleAngle) + m_NeedleWidth * 2 * Math.Cos(needleAngle + Math.PI / 2));
                        points[2].Y = (Single)(Center.Y - m_NeedleRadius / 5 * Math.Sin(needleAngle) + m_NeedleWidth * 2 * Math.Sin(needleAngle + Math.PI / 2));
                        g.FillPolygon(brush1, points);

                        points[2].X = (Single)(Center.X - m_NeedleRadius / 5 * Math.Cos(needleAngle) + m_NeedleWidth * 2 * Math.Cos(needleAngle - Math.PI / 2));
                        points[2].Y = (Single)(Center.Y - m_NeedleRadius / 5 * Math.Sin(needleAngle) + m_NeedleWidth * 2 * Math.Sin(needleAngle - Math.PI / 2));
                        g.FillPolygon(brush2, points);

                        points[0].X = (Single)(Center.X - (m_NeedleRadius / 20 - 1) * Math.Cos(needleAngle));
                        points[0].Y = (Single)(Center.Y - (m_NeedleRadius / 20 - 1) * Math.Sin(needleAngle));
                        points[1].X = (Single)(Center.X - m_NeedleRadius / 5 * Math.Cos(needleAngle) + m_NeedleWidth * 2 * Math.Cos(needleAngle + Math.PI / 2));
                        points[1].Y = (Single)(Center.Y - m_NeedleRadius / 5 * Math.Sin(needleAngle) + m_NeedleWidth * 2 * Math.Sin(needleAngle + Math.PI / 2));
                        points[2].X = (Single)(Center.X - m_NeedleRadius / 5 * Math.Cos(needleAngle) + m_NeedleWidth * 2 * Math.Cos(needleAngle - Math.PI / 2));
                        points[2].Y = (Single)(Center.Y - m_NeedleRadius / 5 * Math.Sin(needleAngle) + m_NeedleWidth * 2 * Math.Sin(needleAngle - Math.PI / 2));
                        g.FillPolygon(brush4, points);

                        points[0].X = (Single)(Center.X - m_NeedleRadius / 20 * Math.Cos(needleAngle));
                        points[0].Y = (Single)(Center.Y - m_NeedleRadius / 20 * Math.Sin(needleAngle));
                        points[1].X = (Single)(Center.X + m_NeedleRadius * Math.Cos(needleAngle));
                        points[1].Y = (Single)(Center.Y + m_NeedleRadius * Math.Sin(needleAngle));

                        g.DrawLine(new Pen(m_NeedleColor2), Center.X, Center.Y, points[0].X, points[0].Y);
                        g.DrawLine(new Pen(m_NeedleColor2), Center.X, Center.Y, points[1].X, points[1].Y);
                        break;
                    case NeedleKind.Needle2D:
                        Point startPoint = new Point((Int32)(Center.X - m_NeedleRadius / 8 * Math.Cos(needleAngle)),
                                                   (Int32)(Center.Y - m_NeedleRadius / 8 * Math.Sin(needleAngle)));
                        Point endPoint = new Point((Int32)(Center.X + m_NeedleRadius * Math.Cos(needleAngle)),
                                                 (Int32)(Center.Y + m_NeedleRadius * Math.Sin(needleAngle)));

                        g.FillEllipse(new SolidBrush(m_NeedleColor2), Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6);

                        switch (m_NeedleColor1)
                        {
                            case NeedleColorEnum.Gray:
                                g.DrawLine(new Pen(Color.DarkGray, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y);
                                g.DrawLine(new Pen(Color.DarkGray, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y);
                                break;
                            case NeedleColorEnum.Red:
                                g.DrawLine(new Pen(Color.Red, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y);
                                g.DrawLine(new Pen(Color.Red, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y);
                                break;
                            case NeedleColorEnum.Green:
                                g.DrawLine(new Pen(Color.Green, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y);
                                g.DrawLine(new Pen(Color.Green, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y);
                                break;
                            case NeedleColorEnum.Blue:
                                g.DrawLine(new Pen(Color.Blue, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y);
                                g.DrawLine(new Pen(Color.Blue, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y);
                                break;
                            case NeedleColorEnum.Magenta:
                                g.DrawLine(new Pen(Color.Magenta, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y);
                                g.DrawLine(new Pen(Color.Magenta, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y);
                                break;
                            case NeedleColorEnum.Violet:
                                g.DrawLine(new Pen(Color.Violet, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y);
                                g.DrawLine(new Pen(Color.Violet, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y);
                                break;
                            case NeedleColorEnum.Yellow:
                                g.DrawLine(new Pen(Color.Yellow, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y);
                                g.DrawLine(new Pen(Color.Yellow, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y);
                                break;
                        }
                        break;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public override DockStyle Dock
        {
            get
            {
                return base.Dock;
            }
            set
            {
                _dockEvent = true;
                 base.Dock = value;
            }

        }

        private bool _dockEvent;
        private void UpdateSizes()
        {
            int dx = (this.Width - _width);
            int dy = (this.Height - _height);

            dx /= 2;
            dy /= 2;

            dx += _xerror;
            dy += _yerror;

            _xerror = (this.Width - _width) % 2;
            _yerror = (this.Height - _height) % 2;

            int grown = this.Width > this.Height ? dy : dx;

            if( _dockEvent)
            {
                _dockEvent = false;
                grown = Math.Abs(dy) > Math.Abs(dx) ? dx : dy;
            }

            _width = this.Width;
            _height = this.Height;

            foreach (Caption caption in _caption)
                caption.Position = new PointF(caption.Position.X + dx, caption.Position.Y + dy);

            if(!drawGaugeBackground)
                drawGaugeBackground = grown != 0;

            foreach (Range range in _ranges)
            {
                range.InnerRadius += grown;
                range.OuterRadius += grown;
            }

            m_ScaleLinesInterInnerRadius += grown;
            m_ScaleLinesInterOuterRadius += grown;
            m_ScaleLinesMinorInnerRadius += grown;
            m_ScaleLinesMinorOuterRadius += grown;
            m_ScaleLinesMajorInnerRadius += grown;
            m_ScaleLinesMajorOuterRadius += grown;
            m_ScaleNumbersRadius += grown;
            m_NeedleRadius += grown;
            m_BaseArcRadius += grown;
        }

        
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (_first && this.Width != 0 && this.Height != 0)
            {
                _width = this.Width;
                _height = this.Height;
            }

            drawGaugeBackground = true;
            Refresh();
        }

#endregion

    }
}

