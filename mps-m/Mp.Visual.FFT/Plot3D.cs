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
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Mp.Visual.FFT
{
    internal delegate double GetZValueDelagate(double x, double y);

    internal class Plot3D
    {
        #region Filds
        //Transformations coeficients
        private double _screenDistance;
        private double _sinFi;
        private double _cosFi;
        private double _sinTeta;
        private double _cosTeta;
        private double _distanceObsToCenter; //distance from observator to center
        private double _propScreenWidthToScreenWidthPhys; //screenWidth / screenWidthPhys;
        private double _b;
        private double _propScreenHeightToScreenHeightPhys;
        private double _d;

        private double _densityX = 0.5f;
        private double _densityY = 0.5f;
        private Color _penColor = Color.Green;
        private PointF _startPoint = new PointF(-20, -20);
        private PointF _endPoint = new PointF(20, 20);
        private GetZValueDelagate GetZValue = null;
        private bool _showGrid = true;
        private SolidBrush[] _brushes = new SolidBrush[100];
        private double _obsX;
        private double _obsY;
        private double _obsZ;
        private Color[] _colorSchema = new Color[100];
        #endregion

        #region Properties

        public bool ShowGrid
        {
            get { return _showGrid; }
            set { _showGrid = value; }
        }

        /// <summary>
        /// Surface spanning net density
        /// </summary>
        public double DensityX
        {
            get { return _densityX; }
            set 
            { 
                _densityX = value; 
            }
        }

        /// <summary>
        /// Surface spanning net density
        /// </summary>
        public double DensityY
        {
            get { return _densityY; }
            set 
            { _densityY = value; 
            }
        }

        /// <summary>
        /// Quadrilateral pen color
        /// </summary>
        public Color PenColor
        {
            get { return _penColor; }
            set { _penColor = value; }
        }

        public PointF StartPoint
        {
            get { return _startPoint; }
            set { _startPoint = value; }
        }

        public PointF EndPoint
        {
            get { return _endPoint; }
            set 
            { 
                _endPoint = value; 
            }
        }

        public GetZValueDelagate OnGetZValue
        {
            get { return GetZValue; }
            set { GetZValue = value; }
        }
        
        private void CreateColorSchema(Color baseColor)
        {
            double hue = baseColor.GetHue();

            hue %= 360.0;
            for (int i = 0; i < _colorSchema.Length; i++)
                _colorSchema[i] = FromColor(1.0, i / (_colorSchema.Length - 1.0), 1.0, hue);
        }

        public Color ColorSchema
        {
            set 
            {
                CreateColorSchema(value);

                _brushes = new SolidBrush[_colorSchema.Length];

                for (int i = 0; i < _brushes.Length; i++)
                    _brushes[i] = new SolidBrush(_colorSchema[i]);
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Surface3DRenderer"/> class. Calculates transformations coeficients.
        /// </summary>
        /// <param name="obsX">Observator's X position</param>
        /// <param name="obsY">Observator's Y position</param>
        /// <param name="obsZ">Observator's Z position</param>
        /// <param name="xs0">X coordinate of screen</param>
        /// <param name="ys0">Y coordinate of screen</param>
        /// <param name="screenWidth">Drawing area width in pixels.</param>
        /// <param name="screenHeight">Drawing area height in pixels.</param>
        /// <param name="screenDistance">The screen distance.</param>
        /// <param name="screenWidthPhys">Width of the screen in meters.</param>
        /// <param name="screenHeightPhys">Height of the screen in meters.</param>
        public Plot3D(double obsX, double obsY, double obsZ, int xs0, int ys0, int screenWidth, 
                      int screenHeight, double screenDistance, double screenWidthPhys, double screenHeightPhys)
        {
            SetupTransCoeff(obsX, obsY, obsZ, xs0, ys0, screenWidth, screenHeight, screenDistance, screenWidthPhys, screenHeightPhys);
        }

        public void SetupTransCoeff(double obsX, double obsY, double obsZ, int xs0, int ys0, 
                                    int screenWidth, int screenHeight, double screenDistance, double screenWidthPhys, double screenHeightPhys)
        {
            double r1, a;

            _obsX = obsX;
            _obsY = obsY;
            _obsZ = obsZ;

            if (screenWidthPhys <= 0)//when screen dimensions are not specified
                screenWidthPhys = screenWidth * 0.0257 / 72.0;        //0.0257 m = 1 inch. Screen has 72 px/inch

            if (screenHeightPhys <= 0)
                screenHeightPhys = screenHeight * 0.0257 / 72.0;

            r1 = obsX * obsX + obsY * obsY;
            a = Math.Sqrt(r1);//distance in XY plane
            _distanceObsToCenter = Math.Sqrt(r1 + obsZ * obsZ);//distance from observator to center

            if (a != 0) //rotation matrix coeficients calculation
            {
                _sinFi = obsY / a;//sin( fi)
                _cosFi = obsX / a;//cos( fi)
            }
            else
            {
                _sinFi = 0;
                _cosFi = 1;
            }
            _sinTeta = a / _distanceObsToCenter;//sin( teta)
            _cosTeta = obsZ / _distanceObsToCenter;//cos( teta)

            //linear tranfrormation coeficients
            _propScreenWidthToScreenWidthPhys = screenWidth / screenWidthPhys;
            _b = xs0 + _propScreenWidthToScreenWidthPhys * screenWidthPhys / 2.0;
            _propScreenHeightToScreenHeightPhys = -(double)screenHeight / screenHeightPhys;
            _d = ys0 - _propScreenHeightToScreenHeightPhys * screenHeightPhys / 2.0;

            this._screenDistance = screenDistance;
        }

        /// <summary>
        /// Performs projection. Calculates screen coordinates for 3D point.
        /// </summary>
        /// <param name="x">Point's x coordinate.</param>
        /// <param name="y">Point's y coordinate.</param>
        /// <param name="z">Point's z coordinate.</param>
        /// <returns>Point in 2D space of the screen.</returns>
        private PointF Project(double x, double y, double z)
        {
            double xn;
            double yn;
            double zn;

            //Transformations
            xn = -_sinFi * x + _cosFi * y;
            yn = -_cosFi * _cosTeta * x - _sinFi * _cosTeta * y + _sinTeta * z;
            zn = -_cosFi * _sinTeta * x - _sinFi * _sinTeta * y - _cosTeta * z + _distanceObsToCenter;

            zn = (zn == 0) ? 0.01 : zn; //Avoid division by null.

            //Tales' theorem
            return  new PointF((float)(_propScreenWidthToScreenWidthPhys * xn * _screenDistance / zn + _b), 
                               (float)(_propScreenHeightToScreenHeightPhys * yn * _screenDistance / zn + _d));
        }

        private static double HueToRgb(double value1, double value2, double hue)
        {
            if (hue < 0.0)
            {
                hue += 360.0;
            }
            if (hue > 360.0)
            {
                hue -= 360.0;
            }
            if (hue < 60.0)
            {
                return value1 + (value2 - value1) * hue / 60.0;
            }
            if (hue < 180.0)
            {
                return value2;
            }
            if (hue < 240.0)
            {
                return value1 + (value2 - value1) * (240.0 - hue) / 60.0;
            }
            return value1;
        }

        private static Color FromColor(double alpha, double lightness, double saturation, double hue)
        {
            if (saturation == 0.0)
            {
                return Color.FromArgb((int)(255.0 * alpha + .5), (int)(255.0 * lightness + .5), (int)(255.0 * lightness + .5), (int)(255.0 * lightness + .5));
            }

            double value2 = lightness * saturation;
            
            if (lightness <= .5)
            {
                value2 += lightness;
            }
            else
            {
                value2 = lightness + saturation - value2;
            }
            
            double value1 = 2f * lightness - value2;
            
            return Color.FromArgb((int)(255.0 * alpha + .5), (int)(255.0 * HueToRgb(value1, value2, hue + 120.0) + .5), 
                                  (int)(255.0 * HueToRgb(value1, value2, hue) + .5), 
                                  (int)(255.0 * HueToRgb(value1, value2, hue - 120.0 + .5)));
        }

        public void RenderSurface(Graphics graphics)
        {
            if (GetZValue == null)
                return;

            double z1, z2;
            PointF[] polygon = new PointF[4];
            double xi = _startPoint.X;
            double yi;
            double minZ = double.PositiveInfinity, maxZ = double.NegativeInfinity;
            double[,] mesh = new double[(int)((_endPoint.X - _startPoint.X) / _densityX + 1), (int)((_endPoint.Y - _startPoint.Y) / _densityY + 1)];
            PointF[,] meshF = new PointF[mesh.GetLength(0), mesh.GetLength(1)];

            for (int x = 0; x < mesh.GetLength(0); x++)
            {
                yi = _startPoint.Y;
                for (int y = 0; y < mesh.GetLength(1); y++)
                {
                    double zz = GetZValue(xi, yi);
                    mesh[x, y] = zz;
                    meshF[x, y] = Project(xi, yi, zz);
                    yi += _densityY;
                    
                    if (minZ > zz) minZ = zz;
                    if (maxZ < zz) maxZ = zz;
                }
                xi += _densityX;
            }

            double cc = (maxZ - minZ) / (_brushes.Length - 1.0);

            if (cc == 0)
                cc = 0.0001;//Avoid null division.

            using (Pen pen = new Pen(_penColor))
            {
                for (int y = (mesh.GetLength(1)-1); y > 0;  --y)
                {
                    for (int x = (mesh.GetLength(0) - 1); x > 0; --x)
                    {
                        z1 = mesh[x, y];
                        z2 = mesh[x, y-1];

                        polygon[0] = meshF[x, y];
                        polygon[1] = meshF[x, y - 1];
                        polygon[2] = meshF[x - 1, y - 1];
                        polygon[3] = meshF[x - 1, y];

                        graphics.SmoothingMode = SmoothingMode.None;
                        graphics.FillPolygon(_brushes[Math.Min((int)(((z1 + z2) / 2.0 - minZ) / cc), _brushes.Length - 1)], polygon);

                        if (_showGrid)
                        {
                            graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            graphics.DrawPolygon(pen, polygon);
                        }
                    }
                }
            }
        }
    }
}