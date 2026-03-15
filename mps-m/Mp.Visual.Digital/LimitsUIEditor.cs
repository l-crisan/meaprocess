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
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.ComponentModel;

namespace Mp.Visual.Digital
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")] 
    public class LimitsUIEditor : UITypeEditor
    {
        public LimitsUIEditor()
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value.GetType() == typeof(ControlLimits))
            {
                LimitsEditDlg dlg = new LimitsEditDlg((ControlLimits)value);
                dlg.ShowDialog();
            }

            //Update the control => Stupide but it's working!!!
            ICustomTypeDescriptor desc = (ICustomTypeDescriptor)context.Instance;
            Control ctrl = (Control)desc.GetPropertyOwner(null);            
            int width = ctrl.Width;
            ctrl.Width = width + 1;
            return value;
        }
    }
}
