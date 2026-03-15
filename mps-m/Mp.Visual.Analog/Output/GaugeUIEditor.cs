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
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.ComponentModel;

namespace Mp.Visual.Analog
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")] 
    public class GaugeUIEditor : UITypeEditor
    {
        public GaugeUIEditor()
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }


        private void SetRangeList(Control ctrl, RangeList list)
        {
            Gauge gauge = ctrl as Gauge;
            GaugeBlock gaugeBlk = ctrl as GaugeBlock;
            
            if(gauge != null)
                gauge.RangeDefinition = list;

            if (gaugeBlk != null)
                gaugeBlk.RangeDefinition = list;
        }

        private void SetCaptionList(Control ctrl, CaptionList list)
        {
            Gauge gauge = ctrl as Gauge;
            GaugeBlock gaugeBlk = ctrl as GaugeBlock;

            if (gauge != null)
                gauge.CaptionDefinition = list;

            if (gaugeBlk != null)
                gaugeBlk.CaptionDefinition = list;
        }

        private void UpdateCenter(Control ctrl)
        {
            Gauge gauge = ctrl as Gauge;
            GaugeBlock gaugeBlk = ctrl as GaugeBlock;
            System.Drawing.Point c;


            if (gauge != null)
            {
                c = gauge.Center;
                gauge.Center = c;
            }

            if (gaugeBlk != null)
            {
                c = gaugeBlk.Center;
                gaugeBlk.Center = c;
            }
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            ICustomTypeDescriptor desc = (ICustomTypeDescriptor)context.Instance;
            
            Control ctrl = desc.GetPropertyOwner(null) as Control;            
               
            if (value.GetType() == typeof(RangeList))
            {
                RangeList list = (RangeList)value;

                RangeList newList = new RangeList();
                
                foreach (Range range in list)
                    newList.Add(range);

                GaugeRangeDlg dlg = new GaugeRangeDlg(newList);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    value = newList;
                    SetRangeList(ctrl, newList);
                }
            }

            if (value.GetType() == typeof(CaptionList))
            {
                CaptionList newList = new CaptionList();

                CaptionList list = (CaptionList)value;
                foreach (Caption caption in list)
                    newList.Add(caption);

                GaugeCaptionDlg dlg = new GaugeCaptionDlg(newList);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    SetCaptionList(ctrl, newList);
                    value = newList;
                }
            }

            //UpdateCenter(ctrl);
            return value;
        }
    }
}
