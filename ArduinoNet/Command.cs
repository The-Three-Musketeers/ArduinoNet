using System;
using System.Collections.Generic;
using System.Text;

namespace ArduinoNet
{
    public enum Command : int
    {
        Null = 0,
        Button = 1,
        Slide = 2,
        Knob = 3,
        LED = 4
    }

    public enum ArduinoCommandType : int
    {
        /// <summary>
        /// This command will reset the state of the Arduino
        /// </summary>
        Reset = 0,
        LED_1 = 1
    }

    public class ArduinoCommand
    {
        /// <summary>
        /// Specify the what component the command sent to
        /// </summary>
        public ArduinoCommandType ArduinoCommandType { get; set; }

        /// <summary>
        /// The value of the command
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public ArduinoCommand(ArduinoCommandType type, int value)
        {
            ArduinoCommandType = type;
            Value = value;
        }
    }
}
