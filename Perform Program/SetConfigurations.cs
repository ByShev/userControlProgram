using System.Text;
using System.IO;

namespace userControlProgram
{
    class SetConfigurations
    {
        public static bool SetConfig(ref string mailTo, ref string logPath, ref int mailInterval, ref int screenshotInterval, ref bool isAutorun)
        {
            try
            {
                FileInfo streamConfig = new FileInfo("C:\\Users\\Public\\config.config");
                BinaryReader config = new BinaryReader(streamConfig.OpenRead(), Encoding.Default);
                mailTo = config.ReadString();
                logPath = config.ReadString();
                mailInterval = config.ReadInt32();
                screenshotInterval = config.ReadInt32();
                isAutorun = config.ReadBoolean();
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
