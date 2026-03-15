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
using System.Windows.Forms;
using System.Drawing;

namespace Mp.Utils
{
    // GListBoxItem class 
    public class IconListBoxItem
    {
        private string _myText;
        private int _myImageIndex;
        private object _tag;


        // properties 
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public string Text
        {
            get { return _myText; }
            set { _myText = value; }
        }
        public int ImageIndex
        {
            get { return _myImageIndex; }
            set { _myImageIndex = value; }
        }
        //constructor
        public IconListBoxItem(string text, int index)
        {
            _myText = text;
            _myImageIndex = index;
        }
        public IconListBoxItem(string text) : this(text, -1) { }
        public IconListBoxItem() : this("") { }
        public override string ToString()
        {
            return _myText;
        }
    }//End of GListBoxItem class

    // GListBox class 
    public class IconListBox : ListBox
    {
        private ImageList _myImageList;
        public ImageList ImageList
        {
            get { return _myImageList; }
            set { _myImageList = value; }
        }
        public IconListBox()
        {
            // Set owner draw mode
            this.DrawMode = DrawMode.OwnerDrawFixed;
        }

        protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            IconListBoxItem item;
            Rectangle bounds = e.Bounds;
            Size imageSize = _myImageList.ImageSize;

            string strTextToDraw;
            item = (IconListBoxItem)Items[e.Index] as IconListBoxItem;
            if ((item != null) && (item.ImageIndex != -1))
            {
                ImageList.Draw(e.Graphics, bounds.Left, bounds.Top, item.ImageIndex);
                strTextToDraw = item.Text;
            }
            else
            {
                strTextToDraw = Items[e.Index].ToString();
            }

            e.Graphics.DrawString(strTextToDraw, e.Font, new SolidBrush(e.ForeColor),
                        bounds.Left + imageSize.Width, bounds.Top);

            base.OnDrawItem(e);
        }
    }
}
