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
using System.ComponentModel;
using System.Globalization;
using System.Drawing;
using System.Threading;

namespace Mp.Utils
{
    public class ResourceLoader
    {
        public static void LoadResources(Control control)
        {
            ComponentResourceManager resManager = new ComponentResourceManager(control.GetType());
            Point old_location = control.Location;
            try
            {
                CultureInfo cInfo = Thread.CurrentThread.CurrentCulture;
                resManager.ApplyResources(control, "$this", cInfo);
                control.Location = old_location;
                ApplyRessourcesAllControls(control, resManager, cInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static  void ApplyRessourcesAllControls(Control control, ComponentResourceManager resManager, CultureInfo cInfo)
        {      

            foreach (Control ctl in ((Control)control).Controls)
            {
                if (ctl.Controls.Count > 0) 
                    ApplyRessourcesAllControls(ctl, resManager, cInfo);

                resManager.ApplyResources(ctl, ctl.Name, cInfo); // folgendes nur für .NET 2.0
                
                if (ctl is ToolStrip) 
                    ApplyRessourcesAllToolStrips((ToolStrip)ctl, resManager, cInfo);

                if (ctl.ContextMenuStrip != null)
                    ApplyRessourcesAllToolStrips(ctl.ContextMenuStrip, resManager, cInfo);
            }
        }

        private static void ApplyRessourcesAllToolStrips(ToolStrip ts, ComponentResourceManager resManager, CultureInfo cInfo)
        {
            foreach (ToolStripItem tsi in ts.Items)
            {
                ToolStripDropDownItem tdi = tsi as ToolStripDropDownItem;
                if (tdi != null) 
                    ApplyAllToolStripItems(tdi, resManager, cInfo);

                ToolStripComboBox tdc = tsi as ToolStripComboBox;
                if (tdc != null) 
                    ApplyAllToolStripItems(tdc, resManager, cInfo);

                resManager.ApplyResources(tsi, tsi.Name, cInfo);
            }
            resManager.ApplyResources(ts, ts.Name, cInfo);
        }

        private static void ApplyAllToolStripItems(ToolStripItem tsi, ComponentResourceManager resManager, CultureInfo cInfo)
        {
            ToolStripDropDownItem tdi = tsi as ToolStripDropDownItem;
            if (tdi != null)
            {
                foreach (ToolStripItem tsi2 in tdi.DropDownItems)
                {
                    ToolStripDropDownItem tdi2 = tsi2 as ToolStripDropDownItem;
                    if (tdi2 != null && tdi2.DropDownItems.Count > 0)
                        ApplyAllToolStripItems(tdi2, resManager, cInfo);
                    resManager.ApplyResources(tsi2, tsi2.Name, cInfo);
                }
            }

            ToolStripComboBox tdc = tsi as ToolStripComboBox;
            if (tdc != null)
            {
                for (int i = 0; i < tdc.Items.Count; i++)
                {
                    tdc.Items[i] = resManager.GetString(tdc.Name + ".Items" +
                      ((i == 0) ? "" : i.ToString()), cInfo);
                }
            }
            resManager.ApplyResources(tsi, tsi.Name, cInfo);
        }        
    }
}
