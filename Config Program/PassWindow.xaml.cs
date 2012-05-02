using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace userControlConfig
{
    /// <summary>
    /// Interaction logic for PassWindow.xaml
    /// </summary>
    public partial class PassWindow : Window
    {
        private bool _isLogIn = false;
        private int _logInTrying = 3;
        public PassWindow()
        {
            InitializeComponent();
        }

        private void LogButtonClick(object sender, RoutedEventArgs e)
        {
            var passStream = new FileInfo("C:\\Users\\Public\\pass.config");
            var window1 = new Window1();
            var passReader = new BinaryReader(passStream.OpenRead(), Encoding.Default);
            var pass = "";
            pass = passReader.ReadString();
            if (pass == passwordBox1.Password)
            {
                _isLogIn = true;
                window1.Show();
                Close();
                return;
            }
            else
            {
                _logInTrying--;
                if (_logInTrying == 0)
                {
                    this.Close();
                }
                MessageBox.Show("Неверный пароль. Попыток осталось: " + _logInTrying);
                passwordBox1.Password = "";
            }
        }

        private void CheckKey(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                LogButtonClick(0, null);
            }
        }
    }
}
