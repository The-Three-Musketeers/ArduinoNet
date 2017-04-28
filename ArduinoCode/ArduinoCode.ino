#include <SoftwareSerial.h>

#define NUM_BUTTON 6
#define KNOB_PIN 0
#define SLIDE_PIN 1

// constants won't change. They're used here to
// set pin numbers:
const int buttonPins[NUM_BUTTON] = {2, 3, 4, 5, 6, 7};     // the numbers of the pushbutton pins
int preButtonState[NUM_BUTTON] = {LOW, LOW, LOW, LOW, LOW, LOW};
unsigned long lastDebounceTime[NUM_BUTTON] = {0};  // the last time the output pin was toggled
unsigned long debounceDelay = 1;    // the debounce time; increase if the output flickers

int preKnobValue = 0;
int preSlideValue = 0;
int knobCount = 0;
const int ledPin =  3;      // the number of the LED pin

const char led_1 = '1';

// variables will change:
int buttonState = 0;         // variable for reading the pushbutton status


int count = 0;
int btnCount = 0;
const int KNOB_MAX = 20000;
const int BUTTON_MAX = 500;

void setup() {
  // initialize the LED pin as an output:
  pinMode(ledPin, OUTPUT);
  // initialize the pushbutton pin as an input:
  int i;
  for (i = 0; i < NUM_BUTTON; i++) {
    int buttonPin = buttonPins[i];
    pinMode(buttonPin, INPUT_PULLUP);
    //digitalWrite(buttonPin, HIGH);
  }
  Serial.begin(9600);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }
}

void loop() {
  // check if the pushbutton is pressed.
  // if it is, the buttonState is HIGH:
  // hard debounce
  if (btnCount++ == BUTTON_MAX) {
    for (int i = 0; i < NUM_BUTTON; i++) {
      int buttonPin = buttonPins[i];
      // read the state of the pushbutton value:
      buttonState = digitalRead(buttonPin);
      if (buttonState == HIGH) {
        if (preButtonState[i] == LOW) { // this is a button press
          preButtonState[i] = HIGH;
          Serial.write(1);
          Serial.write(i);
        }
      } else {

        preButtonState[i] = LOW;
      }
    }
    btnCount = 0;
  }


  // knob
  if (count++ == KNOB_MAX) {
    int knob_val = analogRead(KNOB_PIN) / 4;
    if (abs(knob_val - preKnobValue) > 1) {
      knobCount = 0;
      preKnobValue = knob_val;
      Serial.write(3);
      Serial.write(knob_val); // fit into one byte
    }

    int slide_val = analogRead(SLIDE_PIN) / 4;
    if (abs(slide_val - preSlideValue) > 1) {
      preSlideValue = slide_val;
      Serial.write(2);
      Serial.write(slide_val); // fit into one byte
    }

    count = 0;
  }
  //  if (Serial.available() == 2) {
  //    byte command = Serial.read();
  //    byte value = Serial.read();
  //    if (command == led_1) {
  //      bool ledValue = value == '1';
  //      digitalWrite(ledPin, ledValue);
  //    }
  //  }

}
