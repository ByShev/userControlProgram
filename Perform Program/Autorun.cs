using System;
using Microsoft.Win32;

namespace userControlProgram
{
    static class Autorun
    {
        public static bool SetAutorunValue(bool autorun, string needPath)
        {
            const string name = "systems";
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
