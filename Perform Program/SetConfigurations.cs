using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace userControlProgram
{
    class SetConfigurations
    {
        public static bool SetConfig(ref string mailTo, ref string logPath, ref int mailInterval)
        {
            try
            {
                FileInfo streamConfig = new FileInfo("C:\\Users\\Public\\config.config");
                BinaryReader config = new BinaryReader(streamConfig.OpenRead(), Encoding.Default);
                mailTo = config.ReadString();
                logPath = config.ReadString();
                mailInterval = config.ReadInt32();
                config.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
