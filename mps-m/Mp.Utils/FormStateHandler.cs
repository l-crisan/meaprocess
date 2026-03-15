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
using System.IO;
using System.Xml;

namespace Mp.Utils
{
    public class FormStateHandler
    {
        private static string _template = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                          "<FormState><top></top><left></left><width></width><height></height>" +
                                          "<state></state>"+
                                          "</FormState>";

        public static void Save(Form form, string key)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            
            string file = path + "\\" + key + ".mcfg";

            if (!Directory.Exists(Path.GetDirectoryName(file)))
                Directory.CreateDirectory(Path.GetDirectoryName(file));            

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(_template);

            xmlDoc.DocumentElement["top"].InnerText = form.Top.ToString();
            xmlDoc.DocumentElement["left"].InnerText = form.Left.ToString();
            xmlDoc.DocumentElement["width"].InnerText = form.Width.ToString();
            xmlDoc.DocumentElement["height"].InnerText = form.Height.ToString();
            xmlDoc.DocumentElement["state"].InnerText = ((int)form.WindowState).ToString();
            xmlDoc.Save(file);
        }

        public static void Restore(Form form, string key)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string file = path + "\\" + key + ".mcfg";

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(file);

                form.Top = Convert.ToInt32(xmlDoc.DocumentElement["top"].InnerText);
                form.Left = Convert.ToInt32(xmlDoc.DocumentElement["left"].InnerText);
                form.Width = Convert.ToInt32(xmlDoc.DocumentElement["width"].InnerText);
                form.Height = Convert.ToInt32(xmlDoc.DocumentElement["height"].InnerText);
                form.WindowState = (FormWindowState)Convert.ToInt32(xmlDoc.DocumentElement["state"].InnerText);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
           
        }
    }
}
