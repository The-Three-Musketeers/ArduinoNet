using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ArduinoNet;

namespace ArduinoNetDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var serial = new Serial();
            serial.Connect();

            serial.OnButtonPressed += Serial_OnButtonPressed;
        }

        private void Serial_OnButtonPressed(object sender, ArduinoEventArg arg)
        {
            MessageBox.Show("Button Pressed");
        }
    }
}
