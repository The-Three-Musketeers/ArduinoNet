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
        Knob = 3
    }

    public enum ArduinoCommandType : int
    {
        Reset = 0
    }

    public class ArduinoCommand
    {
        public ArduinoCommandType ArduinoCommandType { get; set; }
        public int Value { get; set; }

        public ArduinoCommand(ArduinoCommandType type, int value)
        {
            ArduinoCommandType = type;
            Value = value;
        }
    }
}
