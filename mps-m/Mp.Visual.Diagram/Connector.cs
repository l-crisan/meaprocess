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
    public delegate void PostConnectConnectorDelegate(Connector from, Connector to);


    /// <summary>
    /// Represents an endpoint of a connection or a location of a shape to which
    /// a connection can be attached
    /// </summary>
    public class Connector : Entity
    {        
        /// <summary>
        /// Default connector
        /// </summary>
        public Connector()
        { _attachedConnectors = new ConnectorCollection(); }

        /// <summary>
        /// Constructs a connector, passing its location
        /// </summary>
        /// <param name="p"></param>
        public Connector( Point p )
        {
            _attachedConnectors = new ConnectorCollection();
            _position = p;
        }


        public event GetToolTipText OnGetConnectionToolTipText;
        public event PostConnectConnectorDelegate OnPostConnect;
        public event PostConnectConnectorDelegate OnPostDisconnect;

        /// <summary>
        /// The name of this connector
        /// </summary>
        public bool IsInput
        {
            get { return _input; }
            set { _input = value; }
        }

        /// <summary>
        /// If the connector is attached to another connector
        /// </summary>
        public Connector AttachedTo
        {
            get { return _attachedTo; }
            set { _attachedTo = value; }
        }

        /// <summary>
        /// The location of this connector
        /// </summary>
        public Point Position
        {
            get { return _position; }
            set 
            {
                _position  = value;

                for (int k = 0; k < _attachedConnectors.Count; k++)
                    _attachedConnectors[k].Position = _position;
            }
        }

        ///<summary>
        ///  Gets or sets the connector connection
        ///</summary>
        public Connection Connection
        {
            get { return _connection; }
            set 
            { 
                _connection = value;

                if (_connection != null && IsInput)
                    _connection.OnGetToolTipText += new GetToolTipText(GetToolTipText);
            }
        }

        /// <summary>
        /// Gets or sets the connector shape.
        /// </summary>
        public Shape Shape
        {
            get { return _shape; }
            set { _shape = value; }
        }

        /// <summary>
        /// Gets or sets the connector brush.
        /// </summary>
        public SolidBrush ConnectorBrush
        {
            get { return _connectorBrush; }
            set { _connectorBrush = value; }
        }

        /// <summary>
        /// Gets the attached connectors 
        /// </summary>
        public ConnectorCollection AttachedConnectors
        {
            get { return _attachedConnectors; }
        }

        /// <summary>
        /// Gets the connected flag.
        /// </summary>
        public bool Connected
        {
            get { return (_attachedConnectors.Count != 0); }
        }

        private string[] GetToolTipText()
        {
            if (!IsInput || this.AttachedTo == null)
            {
                if (OnGetConnectionToolTipText != null)
                    return OnGetConnectionToolTipText();
 
            }
            else
            {
                if( this.AttachedTo != null)
                    return this.AttachedTo.GetToolTipText();
            }

            return null;
        }

        /// <summary>
        /// Paints the connector on the canvas
        /// </summary>
        /// <param name="g"></param>
        public override void Paint(Graphics g)
        {
            Point np = _position;
            np.X -= XOffset;
            np.Y -= YOffset;

            Point[] pts = { new Point(np.X - 7, np.Y - 7), new Point(np.X + 8, np.Y), new Point(np.X - 7, np.Y +7) };

            g.FillPolygon(_connectorBrush, pts);
        }

        /// <summary>
        /// Tests if the mouse hits this connector
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Hit(Point p)
        {
            Point a =  p;
            Point b = _position;
            b.X -= XOffset;
            b.Y -= YOffset;
            
            b.Offset(-7, -7);
            
            Rectangle r = new Rectangle(a, new Size(1, 1));
            Rectangle d = new Rectangle(b, new Size(10, 10));
            return d.Contains(r);
        }

        /// <summary>
        /// Invalidates the connector
        /// </summary>
        public override void Invalidate()
        {
            Point p = _position;
            p.Offset(-5, -5);
            Site.Invalidate(new Rectangle(p, new Size(10, 10)));
        }

        /// <summary>
        /// Moves the connector with the given shift-vector
        /// </summary>
        /// <param name="p"></param>
        public override void Move(Point p)
        {
            this._position.X += p.X;
            this._position.Y += p.Y;

            for (int k = 0; k < _attachedConnectors.Count; k++)
                _attachedConnectors[k].Move(p);
            
        }

        public override bool CanMove(Point pos)
        {
            return true;
        }

        /// <summary>
        /// Attaches the given connector to this connector.
        /// </summary>
        /// <param name="c"></param>
        public void AttachConnector(Connector conector)
        {
            //remove from the previous, if any
            if (conector._attachedTo != null)
                conector._attachedTo._attachedConnectors.Remove(conector);

            _attachedConnectors.Add(conector);
            conector._attachedTo = this;

            if (OnPostConnect != null)
                OnPostConnect(conector.Connection.From.AttachedTo, this);
        }

        /// <summary>
        /// Detaches the given connector from this connector
        /// </summary>
        /// <param name="c"></param>
        public void DetachConnector(Connector c)
        {
            if (OnPostDisconnect != null)
                OnPostDisconnect(c.Connection.From.AttachedTo, this);

            _attachedConnectors.Remove(c);            
        }

        /// <summary>
        /// Releases this connector from any other
        /// </summary>
        public void Release()
        {
            if( this._attachedTo != null )
            {
                this._attachedTo._attachedConnectors.Remove(this);
                this._attachedTo = null;
            }
        }

        private SolidBrush          _connectorBrush = (SolidBrush)Brushes.Gray;
        private Point               _position;
        private ConnectorCollection _attachedConnectors;
        private Connector           _attachedTo;
        private bool                _input = true;
        private Shape               _shape;
        private Connection          _connection;
    }
}
