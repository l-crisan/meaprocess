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
using System.Drawing;
using System.Windows.Forms;

namespace Mp.Scheme.Sdk
{
    /// <summary>
    /// The base class for each tool.
    /// </summary>
    /// <remarks>
    /// Each runtime engine can have a set of tools to help the user to manipulate data.
    /// </remarks>
    /// <example>
    /// The Windows runtime engine have a panel designer tool.
    /// </example>
    public class Tool
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Tool()
        { }

        public virtual void OnLoadDocument(Mp.Scheme.Sdk.Document doc, Mp.Visual.Docking.DockPanel dockPanel, System.Windows.Forms.Form mainFrame)
        { }

        /// <summary>
        /// Called by the framework when the user wish to execute the tool.
        /// </summary>
        public virtual void OnExecute()
        { }

        /// <summary>
        /// Called by the framework when the document is saved.
        /// </summary>
        public virtual void OnSaveDocument()
        { }      

        /// <summary>
        /// Called be the framework when the document is closed.
        /// </summary>
        public virtual void OnCloseDocument()
        { }

        public virtual void OnClose()
        { }

        public virtual void OnCreate()
        { }
   


        public virtual void LoadResources()
        {  }

        /// <summary>
        /// Gets or sets the tool name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the tool symbol.
        /// </summary>
        public Icon Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        /// <summary>
        /// Gets or sets the tool key shortcut. 
        /// </summary>
        public Keys Shortcut
        {
            get { return _shortcut; }
            set { _shortcut = value; }
        }
        /// <summary>
        /// Gets or sets the tool ToolTip.
        /// </summary>
        public string   ToolTip
        {
            get{ return _toolTip;}
            set { _toolTip = value; }
        }

        /// <summary>
        /// Override this to indicate that the document should be saved before call this tool.
        /// </summary>
        public virtual bool NeedToSaveDocument
        {
            get { return false; }
        }

        /// <summary>
        /// Override this to indicate that the document should be reloaded after the tool execution.
        /// </summary>
        public virtual bool NeedToReloadDocument
        {
            get { return false; }
        }

        /// <summary>
        /// Override this to indicate that the document should be validated before call the tool.
        /// </summary>
        public virtual bool NeedToValidateDocument
        {
            get { return false; }
        }

        private string _name;
        private Icon _icon;
        private Keys _shortcut = Keys.None;
        private string _toolTip;
    }
}
