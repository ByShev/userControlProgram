using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace userControlConfig
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private string _logPath = "";
        private string _email = "";
        private int _timerInterval;
        public Window1()
        {
            this.Hide();
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            logTextBox.Text = openFileDialog.FileName;
            _logPath = openFileDialog.FileName;
        }

        private void saveConfig_Click(object sender, RoutedEventArgs e)
        {
            var unstocked = "";
            if (_email == "" && emailTextBox.Text == "")
            {
                unstocked += "Email";
            }
            else if (emailTextBox.Text != "")
                _email = emailTextBox.Text;

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
                }
                catch (Exception)
                {
                    if (unstocked != "")
                        unstocked += ", интервал таймера";
                    else unstocked = "Интервал таймера";
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
                configWriter.Write(_timerInterval);
                configWriter.Flush();
                configWriter.Close();
            }
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            logTextBox.Text = folderBrowserDialog.SelectedPath;
            _logPath = folderBrowserDialog.SelectedPath;
        }

        private void changePassButton_Click(object sender, RoutedEventArgs e)
        {
            FileInfo passStream = new FileInfo("C:\\Users\\Public\\pass.config");
            BinaryReader passReader = new BinaryReader(passStream.OpenRead(), Encoding.Default);
            string realPass = passReader.ReadString();
            
            passReader.Close();
            if (oldPassBox.Password == realPass)
            {
                if (newPassBox.Password.Length >= 5 && (newPassBox.Password == newRPassBox.Password))
                {
                    realPass = newPassBox.Password;
                    BinaryWriter passWriter = new BinaryWriter(passStream.OpenWrite(), Encoding.Default);
                    passWriter.Write(realPass);
                }
                else if (newPassBox.Password.Length < 5)
                {
                    MessageBox.Show("Длина пароля меньше 5 символов. Для надежности задайте более длинный пароль");
                }
                else MessageBox.Show("Введенные пароли не совпадают, попробуйте снова");
            }
            else MessageBox.Show("Старый пароль введен неверно");

        }
    }
}
