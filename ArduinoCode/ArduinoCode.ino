#include <SoftwareSerial.h>

// constants won't change. They're used here to
// set pin numbers:
const int buttonPin = 2;     // the number of the pushbutton pin
const int ledPin =  3;      // the number of the LED pin

const char led_1 = '1';

// variables will change:
int buttonState = 0;         // variable for reading the pushbutton status
int preButtonState = LOW;

void setup() {
  // initialize the LED pin as an output:
  pinMode(ledPin, OUTPUT);
  // initialize the pushbutton pin as an input:
  pinMode(buttonPin, INPUT);
  Serial.begin(9600);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }
}

void loop() {
  // read the state of the pushbutton value:
  buttonState = digitalRead(buttonPin);
  
  // check if the pushbutton is pressed.
  // if it is, the buttonState is HIGH:
  if (buttonState == HIGH) {
    if (preButtonState == LOW) { // this is a button press
      preButtonState = HIGH;
      Serial.write(11);
    }
  } else {

    preButtonState = LOW;
  }

  if (Serial.available() == 2) {
    byte command = Serial.read();
    byte value = Serial.read();
    if (command == led_1) {
      bool ledValue = value == '1';
      digitalWrite(ledPin, ledValue);
    }
  }

}
