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

        private void logButton_Click(object sender, RoutedEventArgs e)
        {
            FileInfo passStream = new FileInfo("C:\\Users\\Public\\pass.config");
            Window1 window1 = new Window1();
            BinaryReader passReader = new BinaryReader(passStream.OpenRead(), Encoding.Default);
            string pass = "";
            pass = passReader.ReadString();
            if (pass == passwordBox1.Password)
            {
                _isLogIn = true;
                window1.Show();
                this.Close();
                return;
            }
            else if (_logInTrying == 0)
                this.Close();
            else
            {
                _logInTrying--;
                MessageBox.Show("Попыток осталось: " + _logInTrying);
            }
        }
    }
}
