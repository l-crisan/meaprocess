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
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using Mp.Utils;
using Mp.Visual.Docking;

namespace Mp.Scheme.Designer
{
    internal delegate void ObjectSelectionChanged(CustomTypeDescriptor obj);

    internal partial class PanelContainer : DockContent
    {
        private ISelectionService   _selection;
        private IDesignerHost       _host;
        private DesignSurface       _surface;
        private Form _rootForm;
        private Hashtable           _customTypeDescriptors = new Hashtable();
        internal ObjectSelectionChanged SelectionChanged;
        private Form                _fromPanel;
        private int                 _id = -1;
        
        public delegate void ModifyDelegate(object sender, EventArgs e);
        public event ModifyDelegate OnModify;

        public PanelContainer(Form fromForm)
        {
            _fromPanel = fromForm;
            InitializeComponent();

            _surface = new DesignSurface(typeof(Form));

            //Setup the view
            Control view = _surface.View as Control;
            view.Dock = DockStyle.Fill;

            this.Controls.Add(view);

            // Get the IDesignerHost for the surface
            _host = _surface.GetService(typeof(IDesignerHost)) as IDesignerHost;

            // Get the ISelectionService and hook the SelectionChanged event
            _selection = _surface.GetService(typeof(ISelectionService)) as ISelectionService;
            _selection.SelectionChanged += new EventHandler(OnSelectionChanged);

            // Get the rootForm from the IDesignerHost
            _rootForm = _host.RootComponent as Form;
        }


        public void UpdateOptions()
        {
            /*
            Size size = new Size(Properties.Settings.Default.GridSize, Properties.Settings.Default.GridSize);

            DesignerOptionService dos = (DesignerOptionService)_surface.GetService(typeof(DesignerOptionService));

            if (dos == null)
            {
                dos = new WindowsFormsDesignerOptionService();
                _host.AddService(typeof(DesignerOptionService), dos);
            }

            dos.Options.Properties["ShowGrid"].SetValue(dos, Properties.Settings.Default.SnapToGrid);
            dos.Options.Properties["UseSmartTags"].SetValue(dos, Properties.Settings.Default.SnapToGrid);
            dos.Options.Properties["SnapToGrid"].SetValue(dos, Properties.Settings.Default.SnapToGrid);
            dos.Options.Properties["GridSize"].SetValue(dos, size);
            //dos.Options.Properties["UseSnapLines"].SetValue(dos, Properties.Settings.Default.SnapToLine);
            dos.Options.Properties["EnableInSituEditing"].SetValue(dos, true);
            dos.Options.Properties["ObjectBoundSmartTagAutoShow"].SetValue(dos, true);
            dos.Options.Properties["UseOptimizedCodeGeneration"].SetValue(dos, true);*/
        }


        public void Init()
        {
            this.TabText = _fromPanel.Text;
            
            _rootForm.ControlAdded += new ControlEventHandler(OnRootFormControlAdded);
            _rootForm.ControlRemoved += new ControlEventHandler(OnRootFormControlRemoved);
            _rootForm.Dock = DockStyle.Fill;
            _rootForm.FormBorderStyle = FormBorderStyle.None;
            _rootForm.Controls.Clear();
            
            //Copy the form properties
            _rootForm.Text = _fromPanel.Text;
            _rootForm.AutoScroll = true;
            _rootForm.Tag = _fromPanel.Tag;
            _rootForm.BackColor = _fromPanel.BackColor;
            _rootForm.Icon = _fromPanel.Icon;
            _rootForm.BackgroundImage = _fromPanel.BackgroundImage;
            _rootForm.BackgroundImageLayout = _fromPanel.BackgroundImageLayout;
            
            _customTypeDescriptors.Clear();
            Control view = _surface.View as Control;
            view.KeyUp += new KeyEventHandler(OnViewKeyUp);
            
            PropertyDescriptorCollection props = CreateCustomTypeDescriptor(_fromPanel);

            CustomTypeDescriptor formDesc = new CustomTypeDescriptor(_rootForm, props);
            _customTypeDescriptors[_rootForm] = formDesc;
            
            if(SelectionChanged != null)
                SelectionChanged(formDesc);
            
            //Reparent the controls.
            for (int i = 0; i < _fromPanel.Controls.Count; ++i)
            {                
                _rootForm.Controls.Add(_fromPanel.Controls[i]);
                _host.Container.Add(_fromPanel.Controls[i]);
                --i;
            }

            UpdateOptions();
        }
        

        public void LoadResources()
        {
            ResourceLoader.LoadResources(this);
            List<Control> ctrls = new List<Control>();
            
            foreach (Control ctrl in _rootForm.Controls)
                ctrls.Add(ctrl);

            _rootForm.Controls.Clear();

            foreach (Control ctrl in ctrls)
                _rootForm.Controls.Add(ctrl);
        }


        public void PropertyChanged(Control ctrl, PropertyValueChangedEventArgs e)
        {
            PropertyDescriptorCollection propertiesFrom = TypeDescriptor.GetProperties(ctrl);
            PropertyDescriptor propDescrFrom = propertiesFrom[e.ChangedItem.Label];
            if(propDescrFrom == null)
                return;

            foreach(Control control in _selection.GetSelectedComponents())
            {
                if( control != ctrl)
                    continue;

                if( control.GetType().FullName != ctrl.GetType().FullName)
                    continue;

                PropertyDescriptorCollection propertiesTo = TypeDescriptor.GetProperties(control);
                PropertyDescriptor propDescrTo = propertiesTo[e.ChangedItem.Label];
                
                if (propDescrTo == null)
                    continue;

                propDescrTo.SetValue(control, propDescrFrom.GetValue(ctrl));                
            }            
        }


        public void AlignLefts()
        {
            Control primarySelCtrl = _selection.PrimarySelection as Control;
            if (primarySelCtrl == null)
                return;

            ArrayList ctrls = new ArrayList();
            ctrls.Add(primarySelCtrl);

            foreach (Control control in _selection.GetSelectedComponents())
            {
                if (control != primarySelCtrl)
                {
                    control.Left = primarySelCtrl.Left;
                    ctrls.Add(control);
                }
            }

            _selection.SetSelectedComponents(null);
            Invalidate();
            _selection.SetSelectedComponents(ctrls, SelectionTypes.Primary);
            Invalidate();            
        }


        public void SameWidth()
        {
            Control primarySelCtrl = _selection.PrimarySelection as Control;
            if (primarySelCtrl == null)
                return;

            ArrayList ctrls = new ArrayList();
            ctrls.Add(primarySelCtrl);

            foreach (Control control in _selection.GetSelectedComponents())
            {
                if (control != primarySelCtrl)
                {
                    control.Width = primarySelCtrl.Width;
                    ctrls.Add(control);
                }
            }

            _selection.SetSelectedComponents(null);
            Invalidate();
            _selection.SetSelectedComponents(ctrls, SelectionTypes.Primary);
            Invalidate();   
        }


        private class LeftComparer : IComparer<Control>
        {
            public LeftComparer()
            {
            }

            public int Compare(Control a, Control b)
            {
                if (a.Left < b.Left)
                    return -1;

                if (a.Left == b.Left)
                    return 0;

                return 1;
            }
        }


        private class TopComparer : IComparer<Control>
        {
            public TopComparer()
            {
            }

            public int Compare(Control a, Control b)
            {
                if (a.Top < b.Top)
                    return -1;

                if (a.Top == b.Top)
                    return 0;

                return 1;
            }
        }


        public void MakeHSpacingEgual()
        {
            Control primarySelCtrl = _selection.PrimarySelection as Control;
            if (primarySelCtrl == null)
                return;

            if (_selection.SelectionCount < 3)
                return;

            ArrayList primSelCtrl = new ArrayList();
            primSelCtrl.Add(primarySelCtrl);
            
            List<Control> selControls = new List<Control>();

            foreach (Control control in _selection.GetSelectedComponents())
                selControls.Add(control);

            selControls.Sort(new LeftComparer());
            int totalDistance = selControls[selControls.Count - 1].Left - selControls[0].Left;


            double spacing = totalDistance / ((double)selControls.Count -1);

            Control ctrl = selControls[0];
            double pos = 0;

            for(int i = 1; i < selControls.Count; ++i)
            {
                pos = (ctrl.Left + spacing);
                ctrl = selControls[i];
                ctrl.Left = (int) pos;
            }

            _selection.SetSelectedComponents(null);
            Invalidate();
            _selection.SetSelectedComponents(selControls, SelectionTypes.Normal);
            _selection.SetSelectedComponents(primSelCtrl, SelectionTypes.Primary);
            Invalidate();   
        }


        public void MakeVSpacingEgual()
        {
            Control primarySelCtrl = _selection.PrimarySelection as Control;
            if (primarySelCtrl == null)
                return;

            if (_selection.SelectionCount < 3)
                return;

            ArrayList primSelCtrl = new ArrayList();
            primSelCtrl.Add(primarySelCtrl);

            List<Control> selControls = new List<Control>();

            foreach (Control control in _selection.GetSelectedComponents())
                selControls.Add(control);

            selControls.Sort(new TopComparer());
            int totalDistance = selControls[selControls.Count - 1].Top - selControls[0].Top;


            double spacing = totalDistance / ((double)selControls.Count - 1);

            Control ctrl = selControls[0];
            double pos = 0;

            for (int i = 1; i < selControls.Count; ++i)
            {
                pos = (ctrl.Top + spacing);
                ctrl = selControls[i];
                ctrl.Top = (int)pos;
            }

            _selection.SetSelectedComponents(null);
            Invalidate();
            _selection.SetSelectedComponents(selControls, SelectionTypes.Normal);
            _selection.SetSelectedComponents(primSelCtrl, SelectionTypes.Primary);
            Invalidate();
        }


        public void SameHeight()
        {
            Control primarySelCtrl = _selection.PrimarySelection as Control;
            if (primarySelCtrl == null)
                return;

            ArrayList ctrls = new ArrayList();
            ctrls.Add(primarySelCtrl);

            foreach (Control control in _selection.GetSelectedComponents())
            {
                if (control != primarySelCtrl)
                {
                    control.Height = primarySelCtrl.Height;
                    ctrls.Add(control);
                }
            }

            _selection.SetSelectedComponents(null);
            Invalidate();
            _selection.SetSelectedComponents(ctrls, SelectionTypes.Primary);
            Invalidate();
        }


        public void BringSelectionToFront()
        {
            Control ctrl = _selection.PrimarySelection as Control;
            if (ctrl == null)
                return;

            ctrl.BringToFront();
        }


        public void SendSelectionToBack()
        {
            Control ctrl = _selection.PrimarySelection as Control;
            if (ctrl == null)
                return;

            ctrl.SendToBack();
        }


        public bool HasControlsSelected()
        {
            if (_selection.SelectionCount == 0)
                return false;

            if (_selection.PrimarySelection is Form)
                return false;

            return true;
        }


        public void AlignTops()
        {
            Control primarySelCtrl = _selection.PrimarySelection as Control;
            ArrayList ctrls = new ArrayList();
            ctrls.Add(primarySelCtrl);

            foreach (Control control in _selection.GetSelectedComponents())
            {
                if (control != primarySelCtrl)
                {
                    ctrls.Add(control);
                    control.Top = primarySelCtrl.Top;
                }
            }

            _selection.SetSelectedComponents(null);
            Invalidate();
            _selection.SetSelectedComponents(ctrls,SelectionTypes.Primary);
            Invalidate();            
        }


        public void AlignRights()
        {
            Control primarySelCtrl = _selection.PrimarySelection as Control;
            if (primarySelCtrl == null)
                return;

            ArrayList ctrls = new ArrayList();
            ctrls.Add(primarySelCtrl);

            foreach (Control control in _selection.GetSelectedComponents())
            {
                if (control == primarySelCtrl)
                    continue;

                ctrls.Add(control);
                int delta = (primarySelCtrl.Left + primarySelCtrl.Width) - (control.Left + control.Width);
                control.Left += delta;
            }

            _selection.SetSelectedComponents(null);
            Invalidate();
            _selection.SetSelectedComponents(ctrls,SelectionTypes.Primary);
            Invalidate();            
        }


        public void AlignBottoms()
        {
            Control primarySelCtrl = _selection.PrimarySelection as Control;
            if (primarySelCtrl == null)
                return;

            ArrayList ctrls = new ArrayList();
            ctrls.Add(primarySelCtrl);

            foreach (Control control in _selection.GetSelectedComponents())
            {
                if (control == primarySelCtrl)
                    continue;

                ctrls.Add(control);
                
                int delta = (primarySelCtrl.Top + primarySelCtrl.Height) - (control.Top + control.Height);
                control.Top += delta;
            }
            _selection.SetSelectedComponents(null);
            Invalidate();
            _selection.SetSelectedComponents(ctrls, SelectionTypes.Click);
            Invalidate();
        }


        private void OnViewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete)
                return;

            if (_selection.PrimarySelection == null)
                return;

            Control ctrl = _selection.PrimarySelection as Control;

            ControlData ctrlData = ctrl.Tag as ControlData;
            
            if (ctrlData == null)
                return;

            if( ctrlData.StationId == 0)
                RemoveControl(ctrl);
        }

        
        private void OnRootFormControlRemoved(object sender, ControlEventArgs e)
        {
            _customTypeDescriptors.Remove(e.Control);
        }


        public void OnSelectionChanged(object sender, EventArgs e)
        {
            if (SelectionChanged == null)
                return;

            if (_selection.PrimarySelection == null)
            {
                SelectionChanged(null);
                return;
            }

            CustomTypeDescriptor controlDesc = _customTypeDescriptors[_selection.PrimarySelection] as CustomTypeDescriptor;
            SelectionChanged(controlDesc);
        }


        private void OnRootFormControlAdded(object sender, ControlEventArgs e)
        {
            Control ctrl = e.Control;

            if (_customTypeDescriptors.ContainsKey(ctrl))
                return;

            PropertyDescriptorCollection propsForCustomDescriptor = CreateCustomTypeDescriptor(ctrl);
            _customTypeDescriptors.Add(ctrl, new CustomTypeDescriptor(ctrl, propsForCustomDescriptor));
        }


        public void AddControl(Control control)
        {
            int top = control.Top;
            int left = control.Left;

            _host.Container.Add(control);
            _rootForm.Controls.Add(control);

            control.Resize += new EventHandler(OnControlStateChanged);
            control.Move += new EventHandler(OnControlStateChanged);
            control.Left = left;
            control.Top = top;
        }


        private void OnControlStateChanged(object sender, EventArgs e)
        {
            if (OnModify != null)
                OnModify(sender, e);
        }


        public void RemoveControl(Control control)
        {
            _rootForm.Controls.Remove(control);
            _host.Container.Remove(control);
        }


        private static PropertyDescriptorCollection CreateCustomTypeDescriptor(Control ctrl)
        {
            ControlData ctrlData = ctrl.Tag as ControlData;

            PropertyDescriptorCollection ctrlProps = TypeDescriptor.GetProperties(ctrl);
            PropertyDescriptorCollection propsForCustomDescriptor = new PropertyDescriptorCollection(null);

            foreach (PropertyDescriptor prop in ctrlProps)
            {
                string propName = prop.Name;

                // Add the property to the collection for the ICustomTypeDescriptor
                if (ctrlData != null && ctrlData.PropertyFilter != null)
                {
                    //Filter
                    foreach (string propView in ctrlData.PropertyFilter)
                    {
                        if (propView == propName && propName != "Tag")
                        {
                            propsForCustomDescriptor.Add(prop);
                            break;
                        }
                    }
                }
                else
                {
                    propsForCustomDescriptor.Add(prop);
                }
            }
            return propsForCustomDescriptor;
        }


        public Form Panel
        {
            get { return _rootForm; }
        }        
        

        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }


        protected override string GetPersistString()
        {
            return base.GetPersistString() + "\n" + _id.ToString();
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);        
            _rootForm.Close();
        }
    }
       
}

