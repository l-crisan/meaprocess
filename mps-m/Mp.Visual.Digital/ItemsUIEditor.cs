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
using System.Collections;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.ComponentModel;

namespace Mp.Visual.Digital
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")] 
    public class ItemsUIEditor : UITypeEditor
    {
        public ItemsUIEditor()
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            ArrayList list = new ArrayList();

            if (value.GetType() == typeof(ArrayList))
            {
                ArrayList oldList = (ArrayList)value;
                
                foreach (ControlItem item in oldList)
                    list.Add(item);

                ItemsUIDlg dlg = new ItemsUIDlg((ArrayList)list);

                if (dlg.ShowDialog() != DialogResult.OK)
                    list = oldList;                
            }

            ICustomTypeDescriptor desc = (ICustomTypeDescriptor)context.Instance;
            Control ctrl = (Control) desc.GetPropertyOwner(null);

            if (ctrl is RadioButton)
            {
                RadioButton control = (RadioButton)ctrl;
                control.Items = list;
                control.UpdateView();
            }

            if (ctrl is ComboBox)
            {
                ComboBox control = (ComboBox)ctrl;
                control.Items = list;
                control.UpdateView();
            }

            return list;
        }
    }
}
