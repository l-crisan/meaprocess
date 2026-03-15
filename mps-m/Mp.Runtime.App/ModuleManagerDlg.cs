using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

using Mp.Runtime.Sdk;
using Mp.Components;
using CrisanSoft.Utils;

namespace Mp.Runtime.Win
{
    public partial class ModuleManagerDlg : Form
    {
        List<Mp.Runtime.Sdk.Module> _modules;
        public ModuleManagerDlg(List<Mp.Runtime.Sdk.Module> m)
        {
            _modules = m;

            InitializeComponent();

            string str = Properties.Settings.Default.Modules;

            string[] modules = str.Split(';');

            foreach(string file in modules)
            {
                if (file == "")
                    continue;

                string path = ExpandFile(file);

                if (!File.Exists(path))
                    continue;

                Mp.Runtime.Sdk.Module module = TryLoadAssembly(path);

                if( module == null && !RuntimeEngine.LoadModule(path))
                    continue;
                   
                InsertModuleInGrid(path, module);
            }
        }

        private static string ExpandFile(string file)
        {
            if (file.Contains(".\\"))
            {
                string str = file.TrimStart('.');
                str = str.TrimStart('\\');
                return AppDomain.CurrentDomain.BaseDirectory + str;
            }

            return file;
        }
        private void OK_Click(object sender, EventArgs e)
        {
            string modulesStr = "";
            foreach (DataGridViewRow row in modules.Rows)
            {
                modulesStr += row.Cells[4].Value;
                modulesStr += ";";
            }

            modulesStr.TrimEnd(';');
            Properties.Settings.Default.Modules = modulesStr;

            Close();
        }

        private void add_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "*.dll|*.dll|*.*|*.*";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            if( !LoadModule(dlg.FileName) )
                MessageBox.Show("The module couldn't be loaded. Unsupported module type.", "Load module", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        
        private bool LoadModule(string file)
        {
            if( file ==null || file == "")
                return false;

            //Load managed modules.
            Mp.Runtime.Sdk.Module module = TryLoadAssembly(file);
            if (module != null)
            {
                InsertModuleInGrid(file, module);
                _modules.Add(module);
            }

            //Load native modules.
            if (!RuntimeEngine.LoadModule(file))
                return false;
                
            InsertModuleInGrid(file, null);
            return true;
        }

        private void InsertModuleInGrid(string file, Mp.Runtime.Sdk.Module  module)
        {
            int index = modules.Rows.Add();
            DataGridViewRow row = modules.Rows[index];
            
            if (module != null)
            {
                row.Tag = module;
                row.Cells[0].Value = AssemblyVersionInfo.GetModuleName(file);
                row.Cells[1].Value = AssemblyVersionInfo.GetModuleVersion(file);
                row.Cells[2].Value = AssemblyVersionInfo.GetModuleManufacturer(file);
                row.Cells[3].Value = AssemblyVersionInfo.GetModuleDescription(file);
                row.Cells[4].Value = file;
                return;
            }

            row.Cells[0].Value = Path.GetFileName(file);
            row.Cells[1].Value = "Unknown";
            row.Cells[2].Value = "Unknown";
            row.Cells[3].Value = "Unknown";
            row.Cells[4].Value = file;
        }

        public static Mp.Runtime.Sdk.Module TryLoadAssembly(string file)
        {
            Assembly assembly;
            Type[] types;

            if (file[0] == '.')
            {
                file = file.Substring(2);
                file = AppDomain.CurrentDomain.BaseDirectory + file;
            }

            try
            {
                assembly = Assembly.LoadFrom(file);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            types = assembly.GetTypes();

            foreach (Type type in types)
            {
                if (type.BaseType == null)
                    continue;

                if (type.BaseType.FullName == "Mp.Runtime.Sdk.Module")
                {
                    return (Activator.CreateInstance(type) as Mp.Runtime.Sdk.Module);                
                }

                if (type.BaseType.BaseType == null)
                    continue;

                if (type.BaseType.BaseType.FullName == "Mp.Schema.Sdk.Module")
                {
                    return  (Activator.CreateInstance(type) as Mp.Runtime.Sdk.Module);
                }
            }

            return null;
        }

        private void remove_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < modules.SelectedRows.Count; i++)
            {
                DataGridViewRow  row  = modules.SelectedRows[i];

                if (row.Tag != null)
                    _modules.Remove((Mp.Runtime.Sdk.Module)row.Tag);

                modules.Rows.Remove(row);
            }
        }
    }
}