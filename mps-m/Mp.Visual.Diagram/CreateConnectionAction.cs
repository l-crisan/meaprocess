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

namespace Mp.Visual.Diagram
{
    internal class CreateConnectionAction : IAction
    {
        private Connection _connection;
        private Connector _fromConnector;
        private Connector _toConnector;

        public CreateConnectionAction(Connection con)
        {
            _connection = con;
            _fromConnector = con.From.AttachedTo;
        }

        public void Undo()
        {
            _toConnector = _connection.To.AttachedTo;
            DiagramCtrl diagramCtrl = _connection.Site;
            Shape shape = _toConnector.Shape;

            diagramCtrl.Connections.Remove(_connection);
            shape.OnPostDisconnectedConnector(_connection.From.AttachedTo, _connection.To.AttachedTo);

            _connection.From.AttachedTo.DetachConnector(_connection.From);
            _connection.To.AttachedTo.DetachConnector(_connection.To);

            _connection.Invalidate();
        }

        public void Redo()
        {
            DiagramCtrl diagramCtrl = _connection.Site;
            diagramCtrl.Connections.Add(_connection);

            _fromConnector.AttachConnector(_connection.From);
            _toConnector.AttachConnector(_connection.To);

            _toConnector.Shape.OnPostConnectedConnector(_fromConnector, _toConnector);
            _connection.Invalidate();
        }
    }
}
