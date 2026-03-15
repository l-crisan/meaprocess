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
using System.Text;
using System.Windows.Forms;
using System.IO;
using Mp.Scheme.Sdk;
using Mp.Visual.Docking;

namespace Mp.Scheme.App
{
    public partial class DiagramContainer : DockContent
    {
        private StationPalletWindow _pallete = new StationPalletWindow();
        private Document _document;

        public DiagramContainer()
        {
            InitializeComponent();

            _pallete.Show(dockPanel);
            _pallete.DockState = DockState.DockLeft;
        }


        public void LoadResources()
        {
            Mp.Utils.ResourceLoader.LoadResources(this);
            _pallete.LoadResources();

            this.TabText = this.Text;
            
            if(_document != null)
                foreach (DiagramWindow dw in _document.Diagrams)
                    dw.LoadResources();            

            if (_document != null)
                _document.MainDiagram.TabText = _document.MainDiagram.Text;
        }


        public Document Document
        {
            set { _document = value; }
            get { return _document; }
        }


        public DockPanel Reinit()
        {
            this.Controls.Remove(dockPanel);
            dockPanel = new DockPanel();
            dockPanel.DocumentStyle = DocumentStyle.DockingWindow;
            dockPanel.Dock = DockStyle.Fill;
            this.Controls.Add(dockPanel);
            return dockPanel; 
        }


        private DockContent GetSchemeDockContent(string padTypeName)
        {
            try
            {
                string[] array = padTypeName.Split('\n');

                switch (array[0])
                {
                    case "Mp.Scheme.App.StationPalletWindow":
                        return _pallete;

                    case "Mp.Scheme.Sdk.DiagramWindow":

                        if (array.Length < 2)
                            return null;

                        return GetDiagramByID(Convert.ToInt32(array[1]));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }


        private DiagramWindow GetDiagramByID(int id)
        {
            if (_document == null)
                return null;
 
            foreach (DiagramWindow w in _document.Diagrams)
            {
                if (w.Diagram.ID == id)
                    return w;
            }
            return null;
        }


        public void LoadState(string state)
        {
            if (state == "")
                return;

            using (MemoryStream mm = new MemoryStream())
            {
                StreamWriter sw = new StreamWriter(mm);
                sw.Write(state.ToCharArray());
                sw.Write(0);

                sw.Flush();
                mm.Flush();

                mm.Seek(0, SeekOrigin.Begin);
                dockPanel.LoadFromXml(mm, new DeserializeDockContent(GetSchemeDockContent), true);
            }
            dockPanel.ShowDocumentIcon = true;
        }    


        public void LoadPalette(Mp.Scheme.Sdk.Module engine)
        {
            Tag = engine;
            _pallete.LoadPalette(engine);
        }


        public void RemovePalette()
        {
            _pallete.RemovePalette();
        }


        public string SaveState()
        {
            //Save the docking state            
            using (MemoryStream mm = new MemoryStream())
            {
                dockPanel.SaveAsXml(mm, Encoding.UTF8, true);

                mm.Flush();
                mm.Seek(0, SeekOrigin.Begin);

                StreamReader sr = new StreamReader(mm);
                return sr.ReadToEnd();
            }            
        }


        public DockPanel InnerPanel
        {
            get 
            { 
                return dockPanel; 
            }
        }   
    }
}
