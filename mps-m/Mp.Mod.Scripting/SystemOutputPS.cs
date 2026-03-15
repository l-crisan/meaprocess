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
using System.Text;
using Mp.Scheme.Sdk;
using System.Drawing;

namespace Mp.Mod.Scripting
{
    internal class SystemOutputPS : ProcessStation
    {
        public SystemOutputPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.SystemOutput;
            base.Group = "I/O";
            base.Symbol = Images.SystemOutputImg;
            base.Icon = Images.SystemOutput;
            base.SubType = "Mp.Runtime.Win.SystemOutputPS";
            base.IsSingleton = true;
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.SystemOutput;
            base.Group ="I/O";
        }

        public override string Description
        {
            get
            {
                return StringResource.SystemOutputPsDescription;
            }
        }

        public override void OnDefaultInit()
        {

            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);

        }

     
        public override void OnHelpRequested()
        {
            //Document.ShowHelp(this.Site, 660);
        }
    }
}
