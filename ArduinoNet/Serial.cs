using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Management;
using System.Threading;

namespace ArduinoNet
{
    public class Serial
    {
        private string serialName;
        private SerialPort stream;
        private Queue<ArduinoCommand> commandQueue;

        public delegate void ArduinoEventHandler(object sender, ArduinoEventArg arg);

        public event ArduinoEventHandler OnButtonPressed;

        public event ArduinoEventHandler OnSlideChanged;

        public event ArduinoEventHandler OnKnobChanged;

        public Serial()
        {
            commandQueue = new Queue<ArduinoCommand>();
        }
        
        internal static string GetArduinoSerialName()
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            foreach (ManagementObject item in searcher.Get())
            {
                string desc = item["Description"].ToString();
                string deviceId = item["DeviceID"].ToString();

                if (desc.Contains("Arduino"))
                {
                    return deviceId;
                }
            }
            return null;
        }

        public void SendCommand(ArduinoCommand command)
        {
            commandQueue.Enqueue(command);
        }

        public bool Connect()
        {
            serialName = GetArduinoSerialName();
            if (string.IsNullOrEmpty(serialName))
                return false;
            stream = new SerialPort(serialName, 9600);

            //stream.ReadTimeout = 50;
            stream.Open();
            // use another thread to read the incoming message
            ThreadStart backgroundRef = new ThreadStart(Loop);
            Thread backgroundThread =  new Thread(backgroundRef);

            // start the background thread
            backgroundThread.Start();

            return true;
        }

        private void HandleCommand(string command)
        {
            // this is the entire protocol
            // the command is in three components
            // [0] command type:
            //      0: null
            //      1: button
            //      2: slide
            //      3: knob
            //      4: led
            // [1-] value in int
            int strlen = command.Length;
            string commandType = command.Substring(strlen - 1, 1); // get the command type
            Command cmd = (Command)(int.Parse(commandType));
            int value =  strlen > 1? int.Parse(command.Substring(strlen - 2)) : 0;  // deal with null

            // construct an event arg
            // TODO:  added this string value once we have more buttons
            ArduinoEventArg arg = new ArduinoEventArg(cmd, value, "");
            switch (cmd)
            {
                case Command.Button:
                    OnButtonPressed(this, arg);
                    break;
                case Command.Knob:
                    OnKnobChanged(this, arg);
                    break;
                case Command.Slide:
                    OnSlideChanged(this, arg);
                    break;
                default:
                    break;
            }
        }

        private string ConvertCommandToString(ArduinoCommand command)
        {
            return string.Format("{0}{1}", (int)command.ArduinoCommandType, command.Value);
        }

        private void Loop()
        {
            while (true)
            {
                // see if there is anything to be written
                if(commandQueue.Count > 0)
                {
                    var command = commandQueue.Dequeue();
                    var rawCommand = ConvertCommandToString(command);
                    Write(rawCommand);
                }
                Thread.Sleep(100); // 10 Hz refresh rate

                // see if there is anything to be read
                var from = Read();
                if (!string.IsNullOrEmpty(from))
                {
                    // decode the command
                    HandleCommand(from);
                }
            }
        }

        private void Write(string value)
        {
            stream.Write(value);
            stream.BaseStream.Flush();
        }

        private string Read(int timeout = 1000)
        {
            stream.ReadTimeout = timeout;
            try
            {
                var result = stream.ReadByte().ToString();
                return result;
            }
            catch (TimeoutException)
            {
                return null;
            }
        }

        
    }
}
