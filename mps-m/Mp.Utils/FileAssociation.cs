using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace Atesion.Utils
{
    public class FileAssociation
    {
        /// <summary>
        /// Register a file extension to a given application.
        /// </summary>
        /// <param name="extension">The extension to register.</param>
        /// <param name="progId">The unique program id.</param>
        /// <param name="description">The short program description.</param>
        /// <param name="executeable">The path to the executable.</param>
        public static void Register(string extension, string progId, string description, string executeable, bool forced)
        {
            string extPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\";
            string progPath = @"Software\Classes\";

            //Register the extension, if necessary.
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(extPath + extension,true))
            {
                if (key == null)
                {
                    forced = true;
                    using (RegistryKey extKey = Registry.CurrentUser.CreateSubKey(extPath + extension))
                    {
                        extKey.SetValue(string.Empty, progId);
                    }
                }            
                else
                {
                    if( forced)
                        key.SetValue(string.Empty, progId);
                }
            }

            //Associate the extension with the progId.
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(progPath + extension,true))
            {
                if (key == null)
                {
                    forced = true;
                    using (RegistryKey progIdKey = Registry.CurrentUser.CreateSubKey(progPath + extension))
                    {
                        progIdKey.SetValue(string.Empty, progId);
                    }
                }
                else
                {
                    if (forced)
                        key.SetValue(string.Empty, progId);
                }
            }

            //Register the progId, if necessary.
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(progPath + progId,true))
            {
                if (key == null)
                {
                    forced = true;
                    using (RegistryKey progIdKey = Registry.CurrentUser.CreateSubKey(progPath + progId))
                    {
                        progIdKey.SetValue(string.Empty, description);
                        using (RegistryKey command = progIdKey.CreateSubKey("shell\\open\\command"))
                        {
                            command.SetValue(string.Empty, String.Format("\"{0}\" \"%1\"", executeable));
                        }
                    }
                }
                else
                {
                    if (forced)
                    {
                        key.SetValue(string.Empty, description);
                        using (RegistryKey command = key.CreateSubKey("shell\\open\\command"))
                        {
                            command.SetValue(string.Empty, String.Format("\"{0}\" \"%1\"", executeable));
                        }
                    }
                }
            }

            //Send change notification to the shell.
            if (forced)
                ShellNotification.NotifyOfChange();
        }

    }
}
