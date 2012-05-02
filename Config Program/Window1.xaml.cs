using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using System.Net.Mail;

namespace userControlConfig
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1
    {
        private string _logPath = "";
        private string _email;
        private int _timerInterval;
        private int _mailInterval;
        public Window1()
        {
            Hide();
            InitializeComponent();
        }

        private void SaveConfigClick(object sender, RoutedEventArgs e)
        {
            var unstocked = "";
            var autorun = AddToAutorun.IsChecked;
            if (_email == "" && emailTextBox.Text == "")
            {
                unstocked += "Email";
            }
            else if (emailTextBox.Text != "")
            {
                try
                {
                    var adr = new MailAddress(emailTextBox.Text);
                    _email = adr.Address;
                }
                catch
                {
                    unstocked += "Email";
                }
            }

            if (_logPath == "" && logTextBox.Text == "")
                if (unstocked != "")
                    unstocked += ", путь к логу";
                else unstocked += "Путь к логу";
            else if (logTextBox.Text != "")
                _logPath = logTextBox.Text;

            if (_timerInterval == 0 && timerTextBox.Text == "")
                if (unstocked != "")
                    unstocked += ", интервал таймера";
                else unstocked = "Интервал таймера";
            else if (timerTextBox.Text != "")
            {
                try
                {
                    _timerInterval = Int32.Parse(timerTextBox.Text);
                    _timerInterval *= 60000;
                }
                catch (Exception)
                {
                    if (unstocked != "")
                        unstocked += ", интервал таймера";
                    else unstocked = "Интервал таймера";
                }
            }

            if (_mailInterval == 0 && mailIntervalTextBox.Text == "")
                if (unstocked != "")
                    unstocked += ", интервал таймера";
                else unstocked = "Интервал таймера";
            else if (mailIntervalTextBox.Text != "")
            {
                try
                {
                    _mailInterval = Int32.Parse(mailIntervalTextBox.Text);
                    _mailInterval *= 60000;
                }
                catch (Exception)
                {
                    if (unstocked != "")
                        unstocked += ", интервал почты";
                    else unstocked = "Интервал почты";
                }
            }

            if (unstocked != "")
                MessageBox.Show("Некоторые поля не заполнены или заполнены неверно: " + unstocked);
            else
            {
                FileInfo streamConfig = new FileInfo("C:\\Users\\Public\\config.config");
                BinaryWriter configWriter = new BinaryWriter(streamConfig.OpenWrite(), Encoding.Default);
                configWriter.Write(_email);
                configWriter.Write(_logPath);
                configWriter.Write(_mailInterval);
                configWriter.Write(_timerInterval);
                configWriter.Write(autorun.ToString());
                configWriter.Flush();
                configWriter.Close();
                File.SetAttributes("C:\\Users\\Public\\config.config", FileAttributes.Hidden);
            }
        }

        private void Button1Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            logTextBox.Text = folderBrowserDialog.SelectedPath;
            _logPath = folderBrowserDialog.SelectedPath;
        }

        private void ChangePassButtonClick(object sender, RoutedEventArgs e)
        {
            var passStream = new FileInfo("C:\\Users\\Public\\pass.config");
            var passReader = new BinaryReader(passStream.OpenRead(), Encoding.Default);
            string realPass = passReader.ReadString();
            
            passReader.Close();
            if (oldPassBox.Password == realPass)
            {
                if (newPassBox.Password.Length >= 5 && (newPassBox.Password == newRPassBox.Password))
                {
                    realPass = newPassBox.Password;
                    var passWriter = new BinaryWriter(passStream.OpenWrite(), Encoding.Default);
                    passWriter.Write(realPass);
                }
                else if (newPassBox.Password.Length < 5)
                {
                    MessageBox.Show("Длина пароля меньше 5 символов. Для надежности задайте более длинный пароль");
                }
                else MessageBox.Show("Введенные пароли не совпадают, попробуйте снова");
            }
            else MessageBox.Show("Старый пароль введен неверно");

            File.SetAttributes("C:\\Users\\Public\\pass.config", FileAttributes.Hidden);
        }

        private void RunNowButtonClick(object o, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("system.exe");
            }
            catch
            {
                MessageBox.Show("Невозможно запустить файл");
            }
        }

        private void ViewLog(object o, EventArgs e)
        {
            try
            {
                var log = new StreamReader(_logPath + "\\report");
                logList.Text = log.ReadToEnd();
            }
            catch{}
        }
    }
}
