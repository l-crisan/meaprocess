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
using System.Collections.Generic;

namespace Mp.Visual.Diagram
{
    /// <summary>
    /// Base class for eatch shape.
    /// </summary>
    public class Shape : Entity
    {
        private class ConnectorState
        {
            public ConnectorState()
            {
            }

            public Connection Connection;
            public Connector FromConnector;
            public Connector ToConnector;
            public bool IsInput;
        }

        private List<ConnectorState> _shapeState = new List<ConnectorState>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Shape()
        { Init(); }

        /// <summary>
        /// Constructor with the site of the shape.
        /// </summary>
        /// <param name="site">The site instance to which the shape is attached.</param>
        public Shape(DiagramCtrl site)
        : base(site)
        { Init(); }

        public virtual void OnHelpRequested()
        {
        }

        public bool ContainsConnector(Connector con)
        {
            foreach (Connector connector in Connectors)
            {
                if (connector == con)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Called before remove the entity.
        /// </summary>        
        public virtual void OnRemove()
        {
            Connection connection;
            Connector connector;
            _shapeState.Clear();

            for (int k = 0; k < _connectors.Count; k++)
            {
                connector = _connectors[k];

                for (int i = 0; i < connector.AttachedConnectors.Count; i++)
                {
                    if (connector.AttachedConnectors[i] == null)
                        continue;

                    connection = connector.AttachedConnectors[i].Connection;

                    ConnectorState connectorState = new ConnectorState();
                    connectorState.FromConnector = connector;
                    connectorState.Connection = connection;
                    connectorState.IsInput = connector.IsInput;

                    connectorState.ToConnector = connection.To.AttachedTo;
                    connectorState.FromConnector = connection.From.AttachedTo;
                    connectorState.IsInput = connector.IsInput;

                    //deattache the connection
                    if (!connector.IsInput && connection.To.AttachedTo != null)
                    {
                        connection.To.AttachedTo.DetachConnector(connection.To);
                        connection.To.AttachedTo.Shape.OnPostDisconnectedConnector(connection.From.AttachedTo, connection.To.AttachedTo);
                    }

                    if (connector.IsInput && connection.From.AttachedTo != null)
                    {
                        connection.From.AttachedTo.DetachConnector(connection.From);
                    }

                    //remove the connection
                    Site.Connections.Remove(connection);
                    _shapeState.Add(connectorState);
                }
            }        
        }

        public virtual void OnRestore()
        {
            IsSelected = false;
            foreach (ConnectorState connectorState in _shapeState)
            {
                Connection connection = connectorState.Connection;
                DiagramCtrl diagramCtrl = connection.Site;

                if (!connectorState.FromConnector.Shape.ContainsConnector(connectorState.FromConnector))
                {
                    connectorState.FromConnector.DetachConnector(connection.From);
                    connectorState.ToConnector.DetachConnector(connection.To);
                    continue;
                }

                if (!connectorState.ToConnector.Shape.ContainsConnector(connectorState.ToConnector))
                {
                    connectorState.FromConnector.DetachConnector(connection.From);
                    connectorState.ToConnector.DetachConnector(connection.To);
                    continue;
                }

                diagramCtrl.Connections.Add(connection);
                
                connectorState.FromConnector.AttachConnector(connection.From);
                connectorState.ToConnector.AttachConnector(connection.To);

                this.OnPostConnectedConnector(connectorState.FromConnector, connectorState.ToConnector);

                connection.IsSelected = false;
                connection.Invalidate();
            }        
        }

        /// <summary>
        /// Gets or sets the connectors of this shape
        /// </summary>
        public ConnectorCollection Connectors
        {
            get { return _connectors; }
            set { _connectors = value; }
        }

        /// <summary>
        /// Gets or sets the width of the shape.
        /// </summary>
        public int Width
        {
            get { return this._rectangle.Width; }
            set { _rectangle.Width = value; }
        }

        /// <summary>
        /// Gets or sets the height of the shape.
        /// </summary>		
        public int Height
        {
            get { return this._rectangle.Height; }
            set { _rectangle.Height = value; }
        }

        public Rectangle Rect
        {
            get { return _rectangle; }
        }

        /// <summary>
        /// Gets or sets the text of the shape
        /// </summary>
        public string Text
        {
            get {  return _text;  }
            set 
            {
                _text = value;
                OnTextChanged(_text);
            }
        }


        protected virtual void OnTextChanged(string text)
        {
        }

        /// <summary>
        /// The x-coordinate of the upper-left corner
        /// </summary>
        public int X
        {
            get { return _rectangle.X; }
            set
            {
                Point p = new Point(value - _rectangle.X, 0);
                this.Move(p);
                Site.Invalidate();
            }
        }

        /// <summary>
        /// The y-coordinate of the upper-left corner
        /// </summary>
        public int Y
        {
            get { return _rectangle.Y; }
            set
            {
                this.Move( new Point( 0, value - _rectangle.Y ) );
                Site.Invalidate();
            }
        }
        /// <summary>
        /// The backcolor of the shape.
        /// </summary>
        public Color ShapeColor
        {
            get { return _shapeColor; }
            set
            {
                _shapeColor = value; 
                SetBrush(); 
                Invalidate(); 
            }
        }

        /// <summary>
        /// Gets or sets the location of the shape.
        /// </summary>
        public Point Location
        {
            get { return new Point(this._rectangle.X, this._rectangle.Y); }
            set
            {
                this.Move( new Point( value.X - _rectangle.X, value.Y - _rectangle.Y ) );
            }
        }

        /// <summary>
        /// Summarizes the initialization used by the constructors.
        /// </summary>
        private void Init()
        {
            _rectangle = new Rectangle(0, 0, 100, 70);
            _connectors = new ConnectorCollection();
            SetBrush();
        }

        /// <summary>
        /// Returns the connector hit by the mouse, if any.
        /// </summary>
        /// <param name="p">The mouse coordinates.</param>
        /// <returns>The connector hit by the mouse.</returns>
        public Connector HitConnector(Point p)
        {
            for (int i = 0; i < _connectors.Count; i++)
            {
                if( _connectors[i].Hit(p) )
                {
                    _connectors[i].Hovered = true;
                    _connectors[i].Invalidate();
                    return _connectors[i];
                }
                else
                {
                    _connectors[i].Hovered = false;
                    _connectors[i].Invalidate();
                }
            }

            return null;
        }

        /// <summary>
        /// Sets the brush corresponding to the backcolor
        /// </summary>
        private void SetBrush()
        { 
           _shapeBrush = new SolidBrush( _shapeColor ); 
        }

        /// <summary>
        /// Overrides the abstract paint method
        /// </summary>
        /// <param name="g">a graphics object onto which to paint</param>
        public override void Paint(System.Drawing.Graphics g)
        {  return; }

        /// <summary>
        /// Override the abstract Hit method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Hit(System.Drawing.Point p)
        { return false; }

        /// <summary>
        /// Overrides the abstract Invalidate method
        /// </summary>
        public override void Invalidate()
        { Site.Invalidate(_rectangle); }


        public override bool CanMove(Point pos)
        {
            if ((this._rectangle.X + pos.X) < 0)
                return false;

            if ((this._rectangle.Y + pos.Y) < 0)
                return false;
            
            return true;            
        }

        /// <summary>
        /// Moves the shape with the given shift
        /// </summary>
        /// <param name="p">represent a shift-vector, not the absolute position!</param>
        public override void Move(Point p)
        {
            if ((this._rectangle.X + p.X) < 0)
                return;

            if ((this._rectangle.Y + p.Y) < 0)
                return;

            this._rectangle.X += p.X;
            this._rectangle.Y += p.Y;

            for (int i = 0; i < this._connectors.Count; i++)
                _connectors[i].Move(p);

            this.Invalidate();
        }

        /// <summary>
        /// Resizes the shape and moves the connectors
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public virtual void Resize(int width, int height)
        {
            this._rectangle.Height = height;
            this._rectangle.Width = width;
        }

        /// <summary>
        /// Called to ask if two connectors can be connected.
        /// </summary>
        /// <param name="from">The from connector</param>
        /// <param name="to">The to connector</param>
        /// <returns></returns>
        public virtual bool CanConnectToPort(Connector from, Connector to)
        {  return true; }

        /// <summary>
        /// Called after a successfull conection of two ports.
        /// </summary>
        /// <param name="from">The from Port.</param>
        /// <param name="to">The to port.</param>
        public virtual void OnPostConnectedConnector(Connector from, Connector to)
        { }

        /// <summary>
        /// Called after two ports are disconnected.
        /// </summary>
        /// <param name="from">The from port.</param>
        /// <param name="to">The to port.</param>
        public virtual void OnPostDisconnectedConnector(Connector from, Connector to)
        { }

        /// <summary>
        /// Called to open the context menu for the connector
        /// </summary>
        /// <param name="point"></param>
        /// <param name="connector"></param>
        public virtual void OnContextMenuConnector(Point point, Connector connector)
        { }


        /// <summary>
        /// The rectangle on which any shape lives.
        /// </summary>
        protected Rectangle _rectangle;

        /// <summary>
        /// the backcolor of the shapes
        /// </summary>
        protected Color _shapeColor = Color.SteelBlue;

        /// <summary>
        /// the brush corresponding to the backcolor
        /// </summary>
        protected Brush _shapeBrush;

        /// <summary>
        /// the collection of connectors onto which you can attach a connection
        /// </summary>
        protected ConnectorCollection _connectors;

        /// <summary>
        /// The text on the shape.
        /// </summary>
        protected string _text = string.Empty;
    }
}
