/***************************************************************************
 *   Copyright (C) 2006-2008 by Laurentiu-Gheorghe Crisan                  *
 *                                                                         *
 *   All rights reserved.												   *
 **************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;

using Mp.Scheme.Sdk;
using Mp.Thermometer;
using Mp.Components;
using Atesion.Utils;

namespace Mp.Scheme.Win
{
    internal class ThermometerPS : SignalBasedControlPS
    {
        private Assembly _myControlsAssembly;

        public ThermometerPS()
        {
            base.Type = "PS_OUTPUT";
            base.Text = "Thermometer";
            base.Group = StringResource.Display;
            base.Symbol = Mp.Scheme.Win.Images.ThermometerImage;
            base.Icon = Mp.Scheme.Win.Images.ThermometerIcon;
            base.SubType = "Mp.Runtime.Sdk.ThermometerPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Thermometer.dll");
        }

        public override string Description
        {
            get
            {
                return StringResource.ThermometerPsDescription;
            }
        }

        protected override Control OnCreateNewControl(XmlElement xmlSignal)
        {
            ThermometerCtrl therm = new ThermometerCtrl();
            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");            
            ctrlData.PropertyFilter.Add("SmallTicFreq");
            ctrlData.PropertyFilter.Add("LargeTicFreq");
            ctrlData.PropertyFilter.Add("RangeMin");
            ctrlData.PropertyFilter.Add("RangeMax");
            ctrlData.PropertyFilter.Add("Value");
            ctrlData.PropertyFilter.Add("BackColor");
            ctrlData.PropertyFilter.Add("ForeColor");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("Display");
            ctrlData.PropertyFilter.Add("Dock");

            therm.Tag = ctrlData;
            therm.Text = XmlHelper.GetParam(xmlSignal, "name");
            therm.RangeMin = XmlHelper.GetParamDouble(xmlSignal, "physMin");
            therm.RangeMax = XmlHelper.GetParamDouble(xmlSignal, "physMax");
            therm.Name = this.Text;
            return therm;
        }

        protected override void OnUpdateControl(Control control, XmlElement xmlSignal)
        {
            ThermometerCtrl therm = (ThermometerCtrl)control;
            therm.Text = XmlHelper.GetParam(xmlSignal, "name");
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "PORT_INPUT", true, false);
            AddPort(port);
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
        }
    }
}
