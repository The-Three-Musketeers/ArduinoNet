using System;
using System.Collections.Generic;
using System.Text;

namespace ArduinoNet
{
    public class ArduinoEventArg : EventArgs
    {
        /// <summary>
        /// Specify the type of the event
        /// </summary>
        public Command EventType { get; private set; }
        /// <summary>
        /// Value of the event. 
        /// For buttons, it will be the button ID, for others it will be analog value
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Unused.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Instantiation an Arduino event argument.
        /// </summary>
        /// <param name="eventType">Specifies what type of Arduino event is</param>
        /// <param name="value">The value of the event</param>
        /// <param name="name">The name of the sensor that triggers the event</param>
        internal ArduinoEventArg(Command eventType, int value, string name): base()
        {
            EventType = eventType;
            Value = value;
            Name = name;
        }
    }
}
