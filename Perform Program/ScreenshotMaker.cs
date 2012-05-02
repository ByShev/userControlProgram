using System;
using System.Drawing.Imaging;
using System.Timers;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace userControlProgram
{
    static class ScreenshotMaker
    {
        private static Timer _timer;
        private static string _logDir;

        public static void ScreenshotStart(int interval, string directory)
        {
            _logDir = directory;
            _timer = new Timer(interval);
            _timer.Start();
            _timer.Elapsed += OnTimerTick;
        }

        public static void OnTimerTick(object o, ElapsedEventArgs e)
        {
            int count = 0;
            foreach ( var screen in Screen.AllScreens)
            {
                Image img = TakeScreenshot(screen);
                img.Save(_logDir + "\\screen" + count + "_" + DateTime.Now.Hour + "h." + DateTime.Now.Minute+"m." + DateTime.Now.Second + "s.jpg");
                count++;
            }
        }

        public static Bitmap TakeScreenshot(Screen screen)
        {
            Bitmap bitmap = new Bitmap(screen.Bounds.Width, screen.Bounds.Height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, 0, 0, screen.Bounds.Size, CopyPixelOperation.SourceCopy);
            return bitmap;
        }
    }
}
