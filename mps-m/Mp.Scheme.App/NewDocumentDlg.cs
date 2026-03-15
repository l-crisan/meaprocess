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
using Mp.Scheme.Sdk;

namespace Mp.Scheme.App
{
    public partial class NewDocumentDlg : Form
    {
        private ModuleManager _moduleManager;
        private Module _runtimeEngine;


        public NewDocumentDlg(ModuleManager mng)
        {
            _moduleManager = mng;
            InitializeComponent();
            Icon = Document.AppIcon;
        }        


        public Module RuntimeEngine
        {
            get { return _runtimeEngine; }
        }


        private void OnOKClick(object sender, EventArgs e)
        {
            _runtimeEngine = _moduleManager.GetEngineByName((string)_RuntimeEngines.SelectedItem);
            DialogResult = DialogResult.OK;
            Close();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (_moduleManager.RuntimeEngines.Count == 0)
            {
                _RuntimeEngines.Enabled = false;
                OK.Enabled = false;
                return;
            }

            foreach (Module Engine in _moduleManager.RuntimeEngines)
                _RuntimeEngines.Items.Add(Engine.Identifier);

            if (_RuntimeEngines.Items.Count > 0)
                _RuntimeEngines.SelectedIndex = 0;
        }
    }
}
