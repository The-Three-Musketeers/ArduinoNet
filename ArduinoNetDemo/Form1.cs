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
        Serial serial;
        bool isLED_ON = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            serial = Serial.Connect();

            serial.OnButtonPressed += Serial_OnButtonPressed;

            serial.OnKnobChanged += Serial_OnKnobChanged;
        }

        private void Serial_OnKnobChanged(object sender, ArduinoEventArg arg)
        {
            lblKnob.Invoke(new MethodInvoker(delegate
            {
                lblKnob.Text = string.Format("Knob value is {0}", arg.Value);
            }));
        }

        private void Serial_OnButtonPressed(object sender, ArduinoEventArg arg)
        {
            MessageBox.Show("Button " + arg.Value.ToString() + " Pressed");
        }

        private void btnFlash_Click(object sender, EventArgs e)
        {
            int value = isLED_ON ? 0 : 1;
            var command = new ArduinoCommand(ArduinoCommandType.LED_1, value);
            serial.SendCommand(command);
            this.btnFlash.Text = "Turn " + (isLED_ON ? "On" : "Off") + " LED";
            isLED_ON = !isLED_ON;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}
