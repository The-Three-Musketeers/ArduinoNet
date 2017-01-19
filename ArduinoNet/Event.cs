using System;
using System.Collections.Generic;
using System.Text;

namespace ArduinoNet
{
    public class ArduinoEventArg : EventArgs
    {
        public Command EventType { get; private set; }
        public int Value { get; private set; }
        public string Name { get; private set; }

        internal ArduinoEventArg(Command eventType, int value, string name): base()
        {
            EventType = eventType;
            Value = value;
            Name = name;
        }
    }
}
