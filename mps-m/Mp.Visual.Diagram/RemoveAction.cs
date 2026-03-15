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
using System.Collections.Generic;

namespace Mp.Visual.Diagram
{
    internal class RemoveAction : IAction
    {
        private List<Connector> _fromConnector = new List<Connector>();
        private List<Connector> _toConnector = new List<Connector>();
        private List<Entity> _entities = new List<Entity>();
        private DiagramCtrl _diagram;

        public RemoveAction(List<Entity> entities)
        {
            
            foreach (Entity entity in entities)
            {
                _entities.Add(entity);

                Connection connection = entity as Connection;
                if (connection != null)
                {
                    _fromConnector.Add(connection.From.AttachedTo);
                    _toConnector.Add(connection.To.AttachedTo);
                }

                _diagram = entity.Site;
            }
        }

        public void Undo()
        {
            int i = 0;
            foreach (Entity entity in _entities)
            {
                Connection connection = entity as Connection;
                Shape shape = entity as Shape;

                if (connection != null)
                {
                    DiagramCtrl diagramCtrl = connection.Site;

                    if (!diagramCtrl.ContainsShape(_fromConnector[i].Shape))
                    {
                        ++i;
                        continue;
                    }

                    if (!_fromConnector[i].Shape.ContainsConnector(_fromConnector[i]))
                    {
                        ++i;
                        continue;
                    }

                    if (!diagramCtrl.ContainsShape(_toConnector[i].Shape))
                    {
                        ++i;
                        continue;
                    }

                    if (!_toConnector[i].Shape.ContainsConnector(_toConnector[i]))
                    {
                        ++i;
                        continue;
                    }

                    //Create the connection
                    bool conAvailable = false;

                    foreach (Connection con in diagramCtrl.Connections)
                    {
                        if (con == connection)
                        {
                            conAvailable = true;
                            break;
                        }
                    }

                    if (!conAvailable)
                    {
                        diagramCtrl.Connections.Add(connection);

                        //Connect the connection to shapes  
                        _fromConnector[i].AttachConnector(connection.From);
                        _toConnector[i].AttachConnector(connection.To);

                        //Notify the new connection
                        _toConnector[i].Shape.OnPostConnectedConnector(_fromConnector[i], _toConnector[i]);

                        //Deselect and invalidate
                        connection.IsSelected = false;
                        connection.Invalidate();
                    }
                    ++i;
                }
                else if (shape != null)
                {
                    shape.Site = _diagram;
                    _diagram.Shapes.Add(shape);                    
                    shape.OnRestore();                    
                    _diagram.Invalidate();
                }                
            }
        }

        public void Redo()
        {
            int i = 0;
            foreach (Entity entity in _entities)
            {
                Connection connection = entity as Connection;
                Shape shape = entity as Shape;
                if (connection != null)
                {
                    Connector toConnector = connection.To.AttachedTo;                    

                    DiagramCtrl diagramCtrl = connection.Site;
                    Shape toShape = toConnector.Shape;

                    diagramCtrl.Connections.Remove(connection);
                    toShape.OnPostDisconnectedConnector(connection.From.AttachedTo, connection.To.AttachedTo);

                    connection.From.AttachedTo.DetachConnector(connection.From);
                    connection.To.AttachedTo.DetachConnector(connection.To);

                    connection.IsSelected = false;
                    connection.Invalidate();
                    ++i;
                }
                else if (shape != null)
                {                                        
                    shape.OnRemove();
                    _diagram.Shapes.Remove(shape);
                    _diagram.Invalidate();
                }                
            }
        }
    }
}
