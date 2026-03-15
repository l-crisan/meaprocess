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
using System.Drawing;
using System.Resources;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Windows.Forms;

using Mp.Scheme.Sdk;
using Mp.Visual.Diagram;

namespace Mp
{
    namespace Mod.FGen
    {
        /// <summary>
        /// Implements a function generator Process-Station.
        /// </summary>
        public class FunctionGenPS : ProcessStation
        {
            internal enum FunctionType
            {
                Sine = 0,
                RampUp,
                RectanglePlus,
                Noise,
                Constant,
                Sinc,
                SinePlus,
                SineMinus,
                RampDown,
                HalfRoundPlus,
                HalfRoundMinus,
                RectangleMinus,
                ExpPlus,
                ExpMinus,
                SincMinus,
                Random
            }

            public override string RuntimeModule
            {
                get { return "mps-fgen"; }
            }

            public FunctionGenPS()
            {
                base.Type   = "Mp.FGen.PS";
                base.Text   = StringResource.Generator;
                base.Group = StringResource.General;
                base.Symbol = Images.FunctionGen;
                base.Icon = Images.FuncGenIcon;
            }

            public override void OnLoadResources()
            {
                base.Text = StringResource.Generator;
                base.Group = StringResource.General;
            }

            public override string Description
            {
                get
                {
                    return StringResource.GeneratorPsDescription;
                }
            }

            public override void OnDefaultInit()
            {
                base.OnDefaultInit();

                //Create the data out port.
                Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.Out",false, true);

                port.SignalList = Document.CreateSignalList();

                InitMenuForPort(port);

                AddPort(port);

            }

            public override void OnHelpRequested()
            {
                Document.ShowHelp(this.Site, 320);    
            }

            public override void OnLoadXml()
            {
                base.OnLoadXml();

                foreach (Port port in InputPorts)
                    InitMenuForPort(port);

                foreach (Port port in OutputPorts)
                    InitMenuForPort(port);
            }
            
            public override void OnPortDoubleClick(Port port)
            {
                base.OnPortDoubleClick(port);
                
                if(port.ContextMenuStrip != null)
                    OnPropertyDataPort(port.ContextMenuStrip.Items[0], null);
            }
                
            protected void OnPropertyDataPort(object sender, EventArgs e)
            {
                ToolStripMenuItem item = (sender as ToolStripMenuItem);

                Port port = (item.Owner.Tag as Port);

                if (port.Type != "Mp.Port.Out")
                    return;

                FuncGenPortDlg Dlg = new FuncGenPortDlg(Document, port.SignalList);
                Dlg.ShowDialog();
            }

            protected override void OnValidate(List<ValidationInfo> valInfoList)
            {
                base.OnValidate(valInfoList);
                Port dataOutPort = OutputPorts[0];

                if (!dataOutPort.Connected)
                {
                    string msg = String.Format(StringResource.DataOutPortConError, this.Text);
                    valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                }

                if( dataOutPort.SignalList.InnerText == "" )
                {
                        string msg = String.Format(StringResource.DataOutPortSigError, this.Text);
                    valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                }
            }

            private void InitMenuForPort(Port port)
            {
                //Create the context menu.
                port.ContextMenuStrip = new ContextMenuStrip();
                port.ContextMenuStrip.Tag = port;

                ToolStripMenuItem menuItem = new ToolStripMenuItem(StringResource.MenuProperties);

                menuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
                menuItem.Click += new System.EventHandler(this.OnPropertyDataPort);
                port.ContextMenuStrip.Items.Add(menuItem);
            }
        }
    }
}
