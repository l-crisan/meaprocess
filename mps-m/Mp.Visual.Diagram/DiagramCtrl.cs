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
using System.Drawing;
using System.Drawing.Printing;
using System.Collections.Generic;

namespace Mp.Visual.Diagram
{
    public delegate void ModifiedDelegate(DiagramCtrl me);
    public delegate void CopyDelegate(List<Shape> shapes);
    public delegate void PasteDelegate(DiagramCtrl diagram, Point pos);

    public class DiagramCtrl : ScrollableControl
    {
        private PrintDocument _printDoc = new PrintDocument();
        protected Point _refPos;
        protected bool _createConnection = false;
        protected bool _createSplitPoint = false;
        protected int _splitPointIndex = -1;
        protected bool _showGrid = true;
        protected bool _mouseDown = false;
        protected bool _doDragDrop = false;
        protected Rectangle _selectionRect = new Rectangle();
        private bool _dragSelectionRect = false;
        protected int _xOffset = 0;
        protected int _yOffset = 0;
        protected int _deltaX = 0;
        protected ShapeCollection _shapes;
        protected Entity _hoveredEntity;
        protected Entity _selectedEntity;
        protected bool _moving = false;
        protected bool tracking = false;
        protected Point _lastMousePos;            
        protected Size _gridSize = new Size(8, 8);
        protected List<Entity> _selected = new List<Entity>();
        protected bool _modified = false;
        private bool _selectionMode = false;
        private bool _draging = false;
        private UndoManager _undoManager = new UndoManager(500);
        private MoveAction _moveAction;
        private SplittPointAction _splittPointAction;
        private CreateConnectionAction _createConnectionAction;
        private bool _snapToGrid = true;
        private uint _id = 0;
        public event CopyDelegate OnCopy;
        public event PasteDelegate OnPaste;

        public event ModifiedDelegate ModifiedEvent;

        public DiagramCtrl()
        {                
            _printDoc.DocumentName = "Scheme";                
            _printDoc.PrintPage += new PrintPageEventHandler(OnPrintPage);
            //double-buffering
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                             
            //init the collections
            _shapes = new ShapeCollection();
            Connections = new ConnectionCollection();
        }

        public uint ID
        {
            set { _id = value; }
            get { return _id; }

        }
        public void ClearUndoStack()
        {
            _undoManager.Clear();
        }

        public bool CanUndo
        {
            get { return _undoManager.CanUndo; }
        }

        public bool CanRedo
        {
            get { return _undoManager.CanRedo; }
        }

        private void OnPrintPage(object sender, PrintPageEventArgs e)
        {
            PaintContent(e.Graphics);
        }

        public bool IsObjSelected
        {
            get
            {

                foreach (Entity entity in _selected)
                {
                    if (entity is Shape)
                        return true;
                }

                return false;
            }
        }

        public void Clear()
        {
            List<Entity> connections = new List<Entity>();

            for (int i = 0; i < Connections.Count; ++i)
            {
                connections.Add(Connections[i]);
                DeleteObject(Connections[i]);
                --i;
            }

            List<Entity> entities = new List<Entity>();
            for (int i = 0; i < _shapes.Count; ++i)
            {
                _shapes[i].IsSelected = false;

                if (_shapes[i].CanDelete)
                {
                    entities.Add(_shapes[i]);
                    DeleteObject(_shapes[i]);
                    --i;
                }
            }

            if (connections.Count > 0)
            {
                RemoveAction action = new RemoveAction(connections);
                _undoManager.Add(action);
            }

            if(entities.Count > 0)
            {
                RemoveAction action = new RemoveAction(entities);
                _undoManager.Add(action);
            }


            _selected.Clear();                
            Invalidate();
        }

        public void DeleteObject()
        {
            List<Entity> entities = new List<Entity>();

            foreach (Entity entity in _selected)
            {
                if (entity.CanDelete)
                {
                    entities.Add(entity);
                    DeleteObject(entity);
                }
            }

            if (entities.Count != 0)
            {
                RemoveAction action = new RemoveAction(entities);
                _undoManager.Add(action);
                Modified = true;

                foreach (Entity entity in _selected)
                    entity.IsSelected = false;

                _selected.Clear();
                this.Invalidate();
            }                
        }

        public void DeleteObject(Entity entity)
        {
            Shape aShape = (entity as Shape);
            Connection aConnection = (entity as Connection);

            if (aShape != null)
            {
                if (aShape.CanDelete)
                {
                    aShape.OnRemove();
                    _shapes.Remove(aShape);
                    Modified = true;
                }
                //continue;
            }

            if (aConnection != null)
            {
                Connector connectorTo = aConnection.To.AttachedTo;
                Connector connectorFrom = aConnection.From.AttachedTo;

                Shape shape = connectorTo.Shape;

                connectorTo.DetachConnector(aConnection.To);
                connectorFrom.DetachConnector(aConnection.From);

                Connections.Remove(aConnection);
                Modified = true;
                shape.OnPostDisconnectedConnector(aConnection.From.AttachedTo, aConnection.To.AttachedTo);
            }                
        }

        public bool CanDeleteObject
        {
            get
            {
                bool canDelete = false;
                foreach (Entity entity in _selected)
                {
                    if (entity.CanDelete)
                    {
                        canDelete = true;
                        break;
                    }
                }

                return canDelete;
            }
        }


        public Shape AddShape(Shape shape)
        {
            InsertShapeAction action = new InsertShapeAction(shape);
            _undoManager.Add(action);

            _shapes.Add(shape);
            shape.Site = this;
            SnapShapeToGrid(shape);

            RecalcAutoScroll(new Point(shape.X, shape.Y));
            shape.X += _xOffset;
            shape.Y += _yOffset;
            this.Invalidate();

            Modified = true;
            return shape;
        }

        private void SnapShapeToGrid(Shape shape)
        {
            if (!_snapToGrid)
                return;
                
            shape.X = shape.X - (shape.X % _gridSize.Width);
            shape.Y = shape.Y - (shape.Y % _gridSize.Height);
        }

        public Connection AddConnection(Connection con)
        {
            Connections.Add(con);
            con.Site = this;
            con.From.Site = this;
            con.To.Site = this;
            this.Invalidate();
            Modified = true;
            return con;
        }
        public Connection AddConnection(Point startPoint)
        {
            Point rndPoint = new Point(startPoint.X + 5, startPoint.Y + 5);
            Connection con = new Connection(startPoint, rndPoint);
            AddConnection(con);
            _selectedEntity = con.To;
            tracking = true;
            this.Invalidate();
            Modified = true;
            return con;

        }

        public Connection AddConnection(Connector from, Connector to)
        {
            Connection con = AddConnection(from.Position, to.Position);
            from.AttachConnector(con.From);
            to.AttachConnector(con.To);
            Modified = true;
            return con;
        }

        public Connection AddConnection(Point from, Point to)
        {
            Connection con = new Connection(from, to);
            this.AddConnection(con);
            Modified = true;
            return con;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            PaintContent(e.Graphics);
        }

        public bool ContainsShape(Shape shape)
        {
            foreach (Shape sshape in Shapes)
            {
                if (sshape == shape)
                    return true;
            }

            return false;
        }

        private void PaintContent(Graphics g)
        {                
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //Draw the shapes
            for (int k = 0; k < _shapes.Count; k++)
            {
                _shapes[k].XOffset = _xOffset;
                _shapes[k].YOffset = _yOffset;

                if (_shapes[k].IsSelected)
                    continue;          

                _shapes[k].Paint(g);
            }

            //Draw the connections
            for (int k = 0; k < Connections.Count; k++)
            {
                if (Connections[k].IsSelected)
                    continue;

                Connections[k].XOffset = _xOffset;
                Connections[k].YOffset = _yOffset;
                Connections[k].Paint(g);
                Connections[k].From.Position = new Point(Connections[k].From.Position.X - _deltaX, Connections[k].From.Position.Y);
                Connections[k].From.Paint(g);
                Connections[k].To.Paint(g);
            }
             
            //Draw the selected connections
            foreach (Entity entity in _selected)
            {
                if (entity is Connection)
                    entity.Paint(g);
            }
                          

            //Draw the selected shapes
            foreach (Entity entity in _selected)
            {
                if (!(entity is Connection))
                    entity.Paint(g);
            }

            if (_dragSelectionRect)
            {
                Point[] pol = new Point[4];
                pol[0] = new Point(_selectionRect.X, _selectionRect.Y);
                pol[1] = new Point(_selectionRect.X + _selectionRect.Width, _selectionRect.Y);
                pol[2] = new Point(_selectionRect.X + _selectionRect.Width, _selectionRect.Y + _selectionRect.Height);
                pol[3] = new Point(_selectionRect.X, _selectionRect.Y + _selectionRect.Height);
                Pen pen = new Pen(Color.Gray, 1);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                g.DrawPolygon(pen, pol);
            }
        }

        public void RecalcScrollSize()
        {
            foreach (Shape shape in _shapes)
                RecalcAutoScroll(new Point(shape.X, shape.Y));

            foreach (Connection connetion in Connections)
            {
                foreach (Point p in connetion.Points)
                {
                    RecalcAutoScroll(p);
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (!_showGrid)
                return;
               
            //Draw the grid into a bitmap 
            Rectangle r = this.ClientRectangle;
            r.Width += _xOffset;
            r.Height += _yOffset;               
            Bitmap bitMap = new Bitmap(r.Width + _xOffset, r.Height + _yOffset, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics gr = Graphics.FromImage(bitMap);
            ControlPaint.DrawGrid(gr, r, _gridSize, this.BackColor);

            //Shift and draw the grid in control graphics
            Point p = new Point(0 - _xOffset, 0 - _yOffset);
            e.Graphics.DrawImage(bitMap, p);
            bitMap.Dispose();
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            _mouseDown = false;

            Point p = this.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));

            //Test for connnectors from shapes.
            for (int k = _shapes.Count - 1; k > -1; --k)
            {
                for (int i = _shapes[k].Connectors.Count - 1; i > -1; --i)
                {
                    if (_shapes[k].Connectors[i].Hit(p))
                    {
                        _shapes[k].Connectors[i].OnMouseDoubleClick(p);
                        return;
                    }
                }
            }

            //Test for shape.
            for (int k = _shapes.Count - 1; k > -1; --k)
            {
                if (_shapes[k].Hit(p))
                {
                    _shapes[k].OnMouseDoubleClick(p);
                    return;
                }
            }

            //Test for connection
            for (int k = Connections.Count - 1; k > -1; --k)
            {
                if (Connections[k].Hit(p))
                {
                    Connections[k].OnMouseDoubleClick(p);
                    return;
                }
            }

            base.OnDoubleClick(e);
        }

        protected bool CheckContextMenu(Point point)
        {
            Entity entity;
            for (int k = _shapes.Count - 1; k > -1; --k)
            {
                for (int i = _shapes[k].Connectors.Count - 1; i > -1; --i)
                {
                    if (_shapes[k].Connectors[i].Hit(point))
                    {
                        _shapes[k].OnContextMenuConnector(point, _shapes[k].Connectors[i]);
                        return true;
                    }
                }
            }


            for (int i = _shapes.Count - 1; i > -1; --i)
            {
                entity = (Entity)_shapes[i];

                if (entity.Hit(point))
                {
                    entity.OnContextMenu(point);
                    return true;
                }
            }

            for (int i = Connections.Count - 1; i > -1; --i)
            {
                if(Connections[i].Hit(point))
                    return true;
            }

            return false;
        }

        public PrintDocument PrintDoc
        {
            get { return _printDoc; }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.Focus();
            base.OnMouseDown(e);

            Point mousePos = new Point(e.X, e.Y);
            Cursor = Cursors.Default;

            _lastMousePos = mousePos;

            _mouseDown = true;
            _doDragDrop = false;

            if (!_selectionMode)
                UnselectAll();

            if (HandleShapeSelection(mousePos))
                _doDragDrop = true;

            if (HandleShapeConnectorSelection(mousePos))
                _doDragDrop = true;

            if (HandleConnectionSelection(mousePos))
                _doDragDrop = true;

            if (HandleSelection(mousePos))
                _doDragDrop = true;

            if (!_doDragDrop)
            {
                _selectionMode = false;
                UnselectAll();
            }

            if (e.Button != MouseButtons.Left)
                _doDragDrop = false;

            Invalidate();
        }

        private bool HandleSelection(Point mousePos)
        {
            if (_selected.Count != 0)
            {
                _dragSelectionRect = false;
                return false;
            }

            _selectionRect.X = mousePos.X;
            _selectionRect.Y = mousePos.Y;

            _dragSelectionRect = true;
            return true;

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            Point mousePos = new Point(e.X, e.Y);

            _mouseDown = false;
            if (_draging)
            {
                Cursor = Cursors.SizeAll;
                _draging = false;
                DragDrop(mousePos);
            }


            if (e.Button == MouseButtons.Right)
                if (CheckContextMenu(mousePos))
                    return;

            base.OnMouseUp(e);

        }

        private bool HandleConnectionSelection(Point mousePos)
        {
            _createSplitPoint = false;
            _splitPointIndex = -1;
            _refPos = mousePos;
            _splittPointAction = null;

            for (int k = 0; k < this.Connections.Count; k++)
            {
                _splitPointIndex = Connections[k].HitSplitPoint(mousePos);

                if (_splitPointIndex != -1)
                {
                    _splittPointAction = new SplittPointAction(Connections[k]);
                    _selected.Add(Connections[k]);
                    return true;
                }

                if (Connections[k].Hit(mousePos))
                {
                    Connections[k].IsSelected = true;
                    _selected.Add(Connections[k]);

                    _createSplitPoint = true;
                    _splittPointAction = new SplittPointAction(Connections[k]);
                    return true;
                }
            }

            return false;
        }

        private bool HandleShapeConnectorSelection(Point mousePos)
        {
            _createConnection = false;
            _refPos = mousePos;
            //Test for connnectors from shapes.
            bool dragDrop = false;
            for (int k = 0; k < _shapes.Count; k++)
            {
                for (int i = 0; i < _shapes[k].Connectors.Count; i++)
                {
                    if (_shapes[k].Connectors[i].Hit(mousePos))
                    {

                        if (_selectedEntity != null)
                            _selectedEntity.IsSelected = false;

                        Connector connector = _shapes[k].Connectors[i];
                        connector.IsSelected = true;

                        if (!_shapes[k].Connectors[i].IsInput)
                        {
                            _refPos = mousePos;
                            UnselectAll();
                            _selected.Add(_shapes[k].Connectors[i]);
                            _createConnection = true;
                        }
                        dragDrop = true;
                    }
                }
            }

            return dragDrop;
        }

        private bool HandleShapeSelection(Point mousePos)
        {
            Shape shape;

            _refPos = mousePos;

            bool hit = false;

            for (int i = _shapes.Count - 1; i != -1; i--)
            {
                shape = _shapes[i];
                shape.IsMaster = false;
                    
                if( !hit )
                {
                    hit = shape.Hit(mousePos);

                    if (hit)
                    {
                        if (!shape.IsSelected)
                            _selected.Add(shape);
                            
                        shape.IsSelected = true;
                        shape.IsMaster = true;
                    }
                }
            }

            return hit;
        }

        private bool StartDrag(Point pos)
        {
            base.Capture = true;
            _moveAction = null;

            if (_createSplitPoint)
            {
                _createSplitPoint = false;
                Connection connection = _selected[0] as Connection;
                    
                if( connection == null)
                    return false;

                _splitPointIndex = connection.AddSplitPoint(_refPos);
                Invalidate();
                return true;
            }

            if (_createConnection)
            {
                _splitPointIndex = -1;
                _createConnection = false;
                Connector connector = _selected[0] as Connector;

                if (connector == null)
                    return false;

                Connection connection = AddConnection(connector.Position);
                Point mousePos = this.PointToClient(new Point(pos.X, pos.Y));

                connector.AttachConnector(connection.From);
                //                    connection.To.Point = new Point(connection.To.Point.X + 5 + _xOffset, connection.To.Point.Y + _yOffset);
                connection.To.Position = new Point(this._lastMousePos.X + _xOffset, _lastMousePos.Y + _yOffset);

                _createConnectionAction = new CreateConnectionAction(connection);
                UnselectAll();
                _selected.Add(connection.To);

                Invalidate();
                return true;
            }
            _moveAction = new MoveAction(_selected);
            _moveAction.StartPos = _refPos;
            return true;
        }

        private new void DragOver(Point mousePos)
        {
            //Move splitt point 
            if (_splitPointIndex != -1)
            {
                Connection connection = _selected[0] as Connection;

                if (connection == null)
                {
                    _splitPointIndex = -1;
                    return;
                }
                   
                _splitPointIndex = connection.MoveSplitPoint(mousePos, _splitPointIndex);

                Invalidate();
                Modified = true;
                return;
            }

            //Move
            bool canMove = true;
            Point movePoint = new Point(mousePos.X - _refPos.X, mousePos.Y - _refPos.Y);

            foreach (Entity entity in _selected)
            {
                if (!entity.CanMove(movePoint))
                {
                    canMove = false;
                    break;
                }
            }

            if( canMove)
            {
                foreach (Entity entity in _selected)
                {
                    Modified = true;                        
                    entity.Move(movePoint);
                }
            }

            if (_dragSelectionRect)
            {
                _selectionRect.Width = mousePos.X - _selectionRect.X;
                _selectionRect.Height = mousePos.Y - _selectionRect.Y;
            }

            //Remember the last point 
            _refPos = mousePos;
            Invalidate();
        }

        protected new void DragDrop(Point mousePos)
        {
            base.Capture = false;

            _refPos = mousePos;
            SnapConnectionToGrid();


            if (_selected.Count == 1)
            {
                Connector dragConnector = (_selected[0] as Connector);

                if (dragConnector != null)
                {//Is a connector what we drag?
                    Connection connection;
                    Connector shapeConnector;
                    bool connected = false;

                    for (int k = 0; k < _shapes.Count; k++)
                    {
                        if ((shapeConnector = _shapes[k].HitConnector(mousePos)) != null)
                        {
                            connection = dragConnector.Connection;
                            if (connection != null)
                            {
                                if (_shapes[k].CanConnectToPort(connection.From.AttachedTo, shapeConnector))
                                { //Connect the connector to the connection                                    
                                    shapeConnector.AttachConnector(dragConnector);

                                    dragConnector.Position = shapeConnector.Position;

                                    _shapes[k].OnPostConnectedConnector(connection.From.AttachedTo, shapeConnector);
                                    Modified = true;
                                    connected = true;
                                    UnselectAll();
                                    Invalidate();
                                    _undoManager.Add(_createConnectionAction);
                                    _createConnectionAction = null;
                                    return;
                                }
                            }
                        }
                    }

                    if (!connected)
                    {
                        Connector  con = dragConnector.Connection.From.AttachedTo;
                        if( con != null )
                            con.DetachConnector(dragConnector.Connection.From);

                        Connections.Remove(dragConnector.Connection);
                        UnselectAll();
                        Invalidate();
                    }
                }
            }
                
            _createConnectionAction = null;

            if(_splittPointAction != null)
            {
                _undoManager.Add(_splittPointAction);
                _splittPointAction = null;
                return;
            }

            if (_dragSelectionRect)
                SelectByRect();
            else
            {
                if (_moveAction != null)
                {
                    _moveAction.EndPos = mousePos;

                    _undoManager.Add(_moveAction);
                    _moveAction = null;
                }
                    
                //Snap to grid the shapes
                SnapShapesToGrid();
            }

            _splitPointIndex = -1;
            RecalcScrollSize();
                
            Invalidate();
        }

        private void SnapShapesToGrid()
        {
            if (!_snapToGrid)
                return;

            foreach (Entity entity in _selected)
            {
                Shape shape = entity as Shape;
                if (shape != null)
                    SnapShapeToGrid(shape);
            }
        }

        private void SnapConnectionToGrid()
        {
            if (!_snapToGrid)
                return;

            foreach (Entity entity in _selected)
            {
                Connection con = entity as Connection;

                if (con != null)
                {
                    for (int i = 0; i < con.Points.Count; ++i)
                    {
                        Point p = con.Points[i];
                        p.X = p.X - (p.X % _gridSize.Width);
                        p.Y = p.Y - (p.Y % _gridSize.Height);
                        con.Points[i] = p;
                    }
                }
            }

            Invalidate();
        }

        private bool IsPointInRect(Rectangle rect, Point point)
        {
            if (rect.Width > 0 && rect.Height > 0)
            {
                return rect.Contains(point);
            }
            else if (rect.Width < 0 && rect.Height > 0)
            {
                return ((point.X > (rect.X + rect.Width)) && point.X < (rect.X)) &&
                        ((point.Y < (rect.X + rect.Height)) && point.Y > (rect.Y));
            }
            else if (rect.Width > 0 && rect.Height < 0)
            {
                return ((point.X < (rect.X + rect.Width)) && point.X > (rect.X)) &&
                        ((point.Y > (rect.X + rect.Height)) && point.Y < (rect.Y));
            }
            else if (rect.Width < 0 && rect.Height < 0)
            {
                return (point.X < rect.X) && (point.X > (rect.X + rect.Width)) &&
                        (point.Y < rect.Y) && (point.Y > (rect.Y + rect.Height));
            }
            return false;

        }

        private void SelectByRect()
        {
            _dragSelectionRect = false;
            _selectionMode = false;
            _selectionRect.X += _xOffset;
            _selectionRect.Y += _yOffset;

            Shape lastShape = null;
            foreach (Shape shape in _shapes)
            {
                if (IsPointInRect(_selectionRect, new Point(shape.X, shape.Y)))
                {
                    shape.IsSelected = true;
                    _selected.Add(shape);
                    lastShape = shape;
                }
            }

            if (lastShape != null)
                lastShape.IsMaster = true;

            foreach (Connection connection in Connections)
            {
                if (IsPointInRect(_selectionRect, (Point)connection.Points[0]))
                {
                    connection.IsSelected = true;
                    _selected.Add(connection);
                }
            }
            _selectionRect.X = 0;
            _selectionRect.Y = 0;
            _selectionRect.Width = 0;
            _selectionRect.Height = 0;
            _selectionMode = _selected.Count != 0;

            Invalidate();
        }

        public void SelectAll()
        {
            _selected.Clear();
            Shape lastShape = null;

            foreach (Shape shape in _shapes)
            {
                _selected.Add(shape);
                shape.IsSelected = true;
                lastShape = shape;
            }

            if (lastShape != null)
                lastShape.IsMaster = true;

            foreach (Connection connection in Connections)
            {
                _selected.Add(connection);
                connection.IsSelected = true;
            }
                
            if (_selected.Count != 0)
                _selectionMode = true;

            Invalidate();
        }

        private void Unselect(Entity entity)
        {
            for (int i = 0; i < _selected.Count; i++)
            {
                if (entity == _selected[i])
                {
                    ((Entity)_selected[i]).IsSelected = false;
                    _selected.RemoveAt(i);
                    return;
                }
            }
        }

        private void UnselectAll()
        {
            foreach (Entity entity in _selected)
                entity.IsSelected = false;

            _selected.Clear();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point mousePos = new Point(e.X, e.Y);
                
            //Draging?
            if (_draging)
            {
                DragOver(mousePos);
                return;
            }
                
            if (_mouseDown && (Math.Abs(_lastMousePos.X - e.X) > 3))
            {
                _mouseDown = false;

                if (_doDragDrop)
                {
                    _doDragDrop = false;
                        _draging = StartDrag(new Point(e.X, e.Y));
                }
            }
            else
            {
                if (!Focused)
                {
                    HoverNone();
                    return;
                }

                //Hovering
                for (int k = _shapes.Count - 1; k > -1; --k)
                {
                    if (_shapes[k].Hit(mousePos))
                    {
                        if (_shapes[k] != _hoveredEntity)
                        {
                            if (_hoveredEntity != null)
                                _hoveredEntity.Hovered = false;
                            Cursor = Cursors.SizeAll;
                            _shapes[k].Hovered = true;
                            _hoveredEntity = _shapes[k];
                            Invalidate();
                        }
                        return;
                    }
                }

                for (int k = Connections.Count - 1; k > -1; --k)
                {
                    if (Connections[k].Hit(mousePos))
                    {
                        if (Connections[k] != _hoveredEntity)
                        {                                
                                
                            if (_hoveredEntity != null)
                                _hoveredEntity.Hovered = false;

                            Cursor = Cursors.SizeAll;
                            Connections[k].Hovered = true;
                            _hoveredEntity = Connections[k];
                            Invalidate();
                        }
                        return;
                    }
                }
                HoverNone();
            }
        }

        private Connection GetConnection(Connector con)
        {
            for (int n = 0; n < Connections.Count; n++)
            {
                if (Connections[n].To == con)
                {
                    return Connections[n];
                }
            }
            return null;
        }

        private void HoverNone()
        {
            Cursor = Cursors.Default;
            if (_hoveredEntity != null)
            {
                _hoveredEntity.Hovered = false;
                Invalidate();
            }
            _hoveredEntity = null;
        }


        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DiagramCtrl
            // 
            this.AllowDrop = true;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(3000, 3000);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.OnHelpRequested);
            this.ResumeLayout(false);

        }
            
        protected override void OnResize(EventArgs e)
        {
            _yOffset = base.VerticalScroll.Value;
            _xOffset = base.HorizontalScroll.Value;
            base.OnResize(e);
        }

        protected override void OnScroll(ScrollEventArgs e)
        {
            base.OnScroll(e);
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
                _xOffset = e.NewValue;
            else if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                _yOffset = e.NewValue;

            this.Focus();
            Invalidate();                
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {                
            base.OnMouseWheel(e);        
            _yOffset = base.VerticalScroll.Value;
            _xOffset = base.HorizontalScroll.Value;
            this.Focus();
            Invalidate();
        }

        private void RecalcAutoScroll(Point p)
        {
            if (_xOffset + p.X >= AutoScrollMinSize.Width)
                AutoScrollMinSize = new Size(_xOffset + p.X + 300, AutoScrollMinSize.Height);

            if (_yOffset + p.Y >= AutoScrollMinSize.Height)
                AutoScrollMinSize = new Size(AutoScrollMinSize.Width, _yOffset + p.Y + 300);

            Invalidate();
        }

        public ConnectionCollection Connections;

        public ShapeCollection Shapes
        {
            get { return _shapes; }
            set { _shapes = value; }
        }

        public bool ShowGrid
        {
            get { return _showGrid; }
            set { _showGrid = value; Invalidate(); }
        }
            
        public bool Modified
        {
            get { return _modified; }
            set                 
            { 
                _modified = value;

                if (ModifiedEvent != null && _modified)
                    ModifiedEvent(this);
            }
        }
           
        public void AlignSelectionLeft()
        {
            AlignShapes(true);
        }

        private void AlignShapes(bool left)
        {
            List<Shape> shapes = new List<Shape>();
            Shape masterShape = GetSelectedShapes(shapes);

            if (shapes.Count == 0)
                return;

            MoveXYAction moveAction;
                
            if(left)
                moveAction = new MoveXYAction(shapes, masterShape.X, left);
            else
                moveAction = new MoveXYAction(shapes, masterShape.Y, left);

            _undoManager.Add(moveAction);

            foreach (Shape shape in shapes)
            {
                if( left)
                    shape.X = masterShape.X;
                else
                    shape.Y = masterShape.Y;
            }

            Modified = shapes.Count != 0;
            Invalidate();
        }

        public void AlignSelectionTop()
        {
            AlignShapes(false);
        }

        private Shape GetSelectedShapes(List<Shape> shapes)
        {
            Shape masterShape = null;

            for (int i = 0; i < _selected.Count; ++i)
            {
                Shape shape = _selected[i] as Shape;
                    
                if (shape == null)
                    continue;

                if (shape.IsMaster)
                    masterShape = shape;

                if (shape != null)
                    shapes.Add(shape);
            }
            return masterShape;
        }

        public void KeyDownEvent(KeyEventArgs e)
        {
            _selectionMode = e.Control;
        }

        public void KeyUpEvent(KeyEventArgs e)
        {
            _selectionMode = e.Control;
        }

        public void Undo()
        {
            _undoManager.Undo();
            Modified = true;
            Invalidate();                
        }

        public void Redo()
        {
            _undoManager.Redo();
            Modified = true;
            Invalidate();                
        }

        public int GridInterval
        {
            get { return _gridSize.Height; }
            set 
            { 
                _gridSize.Height = value;
                _gridSize.Width  = value;
                Invalidate();
            }
        }

        public bool SnapToGrid
        {
            get { return _snapToGrid; }
            set 
            { 
                _snapToGrid = value;
                Invalidate();
            }
        }

        public void Cut()
        {
            Copy();
            DeleteObject();
        }


        public void Paste(Point p)
        {
            if (OnPaste != null)
            {
                OnPaste(this, p);
                Modified = true;
            }                
        }

        public void Copy()
        {
            List<Shape> shapesToCopy = new List<Shape>();
            foreach (Entity entity in _selected)
            {
                if (entity is Shape)
                    shapesToCopy.Add((Shape)entity);
            }

            if (OnCopy != null)
                OnCopy(shapesToCopy);
        }

        public void OnHelp()
        {
            foreach (Entity entity in _selected)
            {
                Shape shape = entity as Shape;
                if (shape != null)
                {
                    if (shape.IsMaster)
                    {
                        shape.OnHelpRequested();
                        return;
                    }
                }
            }
        }

        public void OnHelpRequested(object sender, HelpEventArgs hlpevent)
        {
            OnHelp();
        }        
    }
}


