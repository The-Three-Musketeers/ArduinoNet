#include <SoftwareSerial.h>

#define NUM_BUBTTON 0
#define KNOB_PIN 0

#define KNOB_THRESHOLD 10
#define KNOB_JUMP 50

// constants won't change. They're used here to
// set pin numbers:
const int buttonPins[NUM_BUBTTON]; //{2, 3, 6, 5};     // the numbers of the pushbutton pins
int preButtonState[NUM_BUBTTON]; // = {LOW, LOW, LOW, LOW};
int preKnobValue = 0;
int knobCount = 0;
const int ledPin =  3;      // the number of the LED pin

const char led_1 = '1';

// variables will change:
int buttonState = 0;         // variable for reading the pushbutton status


void setup() {
  // initialize the LED pin as an output:
  pinMode(ledPin, OUTPUT);
  // initialize the pushbutton pin as an input:
  int i;
  for (i = 0; i < NUM_BUBTTON; i++) {
    int buttonPin = buttonPins[i];
    pinMode(buttonPin, INPUT);
  }
  Serial.begin(9600);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }
}

void loop() {
  // check if the pushbutton is pressed.
  // if it is, the buttonState is HIGH:
  int i;
  for (i = 0; i < NUM_BUBTTON; i++) {
    int buttonPin = buttonPins[i];
    // read the state of the pushbutton value:
    buttonState = digitalRead(buttonPin);
    if (buttonState == HIGH) {
      if (preButtonState[buttonPin] == LOW) { // this is a button press
        preButtonState[buttonPin] = HIGH;
        Serial.write(1);
        Serial.write(i);
      }
    } else {

      preButtonState[buttonPin] = LOW;
    }
  }

  // knob
  int knob_val = analogRead(KNOB_PIN) / 4;
  if(abs(knob_val - preKnobValue) > KNOB_THRESHOLD){
    // handle jump
    //if(abs(knob_val - preKnobValue) > KNOB_JUMP && knobCount < 100){
    //  knobCount++;
    //}
    knobCount = 0;
    preKnobValue = knob_val;
    Serial.write(3);
    Serial.write(knob_val); // fit into one byte
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
