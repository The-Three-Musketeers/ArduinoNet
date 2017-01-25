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
        private static Dictionary<string, Serial> currentConnection = new Dictionary<string, Serial>();

        /// <summary>
        /// Used to signal any Arduino event, such as button press
        /// </summary>
        /// <param name="sender">Object that fired this event</param>
        /// <param name="arg">Argument that specifies all the information</param>
        public delegate void ArduinoEventHandler(object sender, ArduinoEventArg arg);

        private event ArduinoEventHandler _onButtonPressed;

        /// <summary>
        /// Event called when any button is pressed
        /// </summary>
        public event ArduinoEventHandler OnButtonPressed
        {
            add
            {
                _onButtonPressed -= value;
                _onButtonPressed += value;
            }
            remove
            {
                _onButtonPressed -= value;
            }
        }

        /// <summary>
        /// Event called when the value of the slider is changed
        /// </summary>
        public event ArduinoEventHandler OnSlideChanged;

        /// <summary>
        /// Event called when the value of the knob is changed
        /// </summary>
        public event ArduinoEventHandler OnKnobChanged;

        private Serial(string serialName)
        {
            commandQueue = new Queue<ArduinoCommand>();
            this.serialName = serialName;

            stream = new SerialPort(serialName, 9600);

            //stream.ReadTimeout = 50;
            stream.Open();
            // use another thread to read the incoming message
            ThreadStart backgroundRef = new ThreadStart(Loop);
            Thread backgroundThread = new Thread(backgroundRef);

            currentConnection.Add(serialName, this);

            // start the background thread
            backgroundThread.Start();

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
            var serial = currentConnection[serialName];
            serial.commandQueue.Enqueue(command);
        }

        /// <summary>
        /// This is a factory method to setup serial connection to the Arduino.
        /// Use this method if you don't know the device ID.
        /// This will prevent multiple instantiation, that is, if called multiple times, only one instance will be returned. 
        /// </summary>
        /// <returns>An Arduino serial instance connected to the serial port</returns>
        public static Serial Connect()
        {
            var serialName = GetArduinoSerialName();
            return Connect(serialName);
        }

        /// <summary>
        /// This is a factory method to setup serial connection to the Arduino.
        /// This will prevent multiple instantiation, that is, if called multiple times, only one instance will be returned. 
        /// </summary>
        /// <param name="serialName">Name of the device ID, e.g., COM1 or /tty/ACM0 on Linux</param>
        /// <returns>An Arduino serial instance connected to the serial port</returns>
        public static Serial Connect(string serialName)
        {
            if (string.IsNullOrEmpty(serialName))
                return null;
            if (currentConnection.ContainsKey(serialName))
            {
                // already connected;
                return currentConnection[serialName];
            }
            else
            {
                return new Serial(serialName);
            }
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
            string commandType = command.Substring(0, 1); // get the command type
            Command cmd = (Command)(int.Parse(commandType));
            int value =  strlen > 1? int.Parse(command.Substring(strlen - 1)) : 0;  // deal with null

            // construct an event arg
            // TODO:  added this string value once we have more buttons
            ArduinoEventArg arg = new ArduinoEventArg(cmd, value, "");
            switch (cmd)
            {
                case Command.Button:
                    _onButtonPressed?.Invoke(this, arg);
                    break;
                case Command.Knob:
                    OnKnobChanged?.Invoke(this, arg);
                    break;
                case Command.Slide:
                    OnSlideChanged?.Invoke(this, arg);
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
