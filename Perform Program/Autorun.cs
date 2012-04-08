using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace userControlProgram
{
    class Autorun
    {
        public static bool SetAutorunValue(bool autorun, string needPath)
        {
            const string name = "userControlProgram";
            string exePath = needPath;
            var key = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            try
            {
                if(autorun)
                    key.SetValue(name, exePath);
                else key.DeleteValue(name);
                key.Flush();
                key.Close();
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }
    }
}
