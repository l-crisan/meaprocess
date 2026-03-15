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

namespace Mp.Visual.Diagram
{
    public delegate string[] GetToolTipText();

    /// <summary>
    /// The base class for eatch diagram entity.
    /// </summary>
    public abstract class Entity
    {
        private bool        _hovered = false;
        private DiagramCtrl _site;
        private bool        _isSelected = false;
        private int         _XOffset = 0;
        private int         _YOffset = 0;
        private bool        _canDelete = true;

        public event GetToolTipText OnGetToolTipText;

        /// <summary>
        /// default constructor
        /// </summary>
        public Entity()
        { }

        /// <summary>
        /// Construct an entity in to the given site.
        /// </summary>
        /// <param name="site">The entity site.</param>
        public Entity( DiagramCtrl site )
        { _site = site;}


        public bool CanDelete
        {
            get { return _canDelete; }
            set { _canDelete = value; }
        }
        /// <summary>
        /// Sets or gest the selection flag.
        /// </summary>
        public bool IsSelected
        {
            get{ return _isSelected; }
            set
            {
                _isSelected = value;
                
                if (!_isSelected)
                    _master = false;

                Invalidate();
            }
        }

        private bool _master = false;
        public bool IsMaster
        {
            get { return _master; }
            set 
            {
                _master = value;
                if (_master == false)
                    _master = false;
                
                Invalidate();
            }

        }

        /// <summary>
        /// Sets or gets the entity site.
        /// </summary>
        public DiagramCtrl Site
        {
            get{ return _site; }
            set{ _site = value; }
        }

        /// <summary>
        /// Sets or gets the x offset position in the site.
        /// </summary>
        /// <remarks>
        /// Is used by scrolling to calculate the correct position of the entity.
        /// </remarks>
        public int XOffset
        {
            set { _XOffset = value; }
            get { return _XOffset; }
        }

        /// <summary>
        /// Sets or gets the y offset position in the site.
        /// </summary>
        /// <remarks>
        /// Is used by scrolling to calculate the correct position of the entity.
        /// </remarks>
        public int YOffset
        {
            set { _YOffset = value; }
            get { return _YOffset; }
        }

        /// <summary>
        /// Sets or gets hovered flag.
        /// </summary>
        public bool Hovered
        {
            set
            { 
                _hovered = value;
                if (_hovered && OnGetToolTipText != null)
                    OnShowToolTip(OnGetToolTipText());
                else
                    OnHideToolTip();
            
            }
            get { return _hovered; }
        }
        
        /// <summary>
        /// Caled be the framework to show the tool tip for this entity.
        /// </summary>
        /// <param name="text"></param>
        protected virtual void OnShowToolTip(string[] text)
        {
        }

        /// <summary>
        /// Called by the framework to hide the tool tip.
        /// </summary>
        protected virtual void OnHideToolTip()
        {
        }

        /// <summary>
        /// Called on double click of the entity
        /// </summary>        
        public virtual void OnMouseDoubleClick(Point p)
        { }

        /// <summary>
        /// Called on show the context menu.
        /// </summary>  
        public virtual void OnContextMenu(Point p)
        { }

        /// <summary>
        /// Called to paint the entity.
        /// </summary>
        /// <param name="g">The Graphics object.</param>
        /// <remarks>
        /// Overrite this to draw your entity.
        /// </remarks>
        public abstract void Paint(Graphics g);

        /// <summary>
        /// Called to check if the point hits the entity.
        /// </summary>
        /// <param name="p">The point to check.</param>
        /// <returns>True for hit.</returns>
        /// <remarks>
        /// Overrite this to check the entity selection.
        /// </remarks>
        public abstract bool Hit( Point p );

        /// <summary>
        /// Called to invalidate the entity.
        /// </summary>
        public abstract void Invalidate();

        /// <summary>
        /// Called to reposition the entity.
        /// </summary>
        /// <param name="pos">The new position</param>
        public abstract void Move( Point pos );

        public abstract bool CanMove(Point pos);        
    }
}
