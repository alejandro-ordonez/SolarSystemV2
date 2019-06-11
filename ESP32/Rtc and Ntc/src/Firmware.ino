
// Include Libraries
#include "Arduino.h"
#include "Thermistor.h"


// Pin Definitions
#define THERMISTOR_PIN_CON1	25



// Global variables and defines

// object initialization
Thermistor thermistor(THERMISTOR_PIN_CON1);


// define vars for testing menu
const int timeout = 1000;       //define timeout of 10 sec
char menuOption = 0;
long time0;

// Setup the essentials for your circuit to work. It runs first every time your circuit is powered with electricity.
void setup() 
{
    
    // Setup Serial which is useful for debugging
    // Use the Serial Monitor to view printed messages
    Serial.begin(9600);
    while (!Serial) ; // wait for serial port to connect. Needed for native USB
    Serial.println("start");
    
    
    menuOption = menu();
    
}

// Main logic of your circuit. It defines the interaction between the components you selected. After setup, it runs over and over again, in an eternal loop.
void loop() 
{
    
    
    if(menuOption == '1') {
    // NTC Thermistor 10k - Test Code
    //Get Measurment from Thermistor temperature sensor.
    float thermistorTempC = thermistor.getTempC();
    Serial.print(F("Temp: ")); Serial.print(thermistorTempC); Serial.println(F("[°C]"));
    }
    
    if (millis() - time0 > timeout)
    {
        menuOption = menu();
    }
    
}



// Menu function for selecting the components to be tested
// Follow serial monitor for instrcutions
char menu()
{

    Serial.println(F("\nWhich component would you like to test?"));
    Serial.println(F("(1) NTC Thermistor 10k"));
    Serial.println(F("(menu) send anything else or press on board reset button\n"));
    while (!Serial.available());
11
    // Read data from serial monitor if received
    while (Serial.available()) 
    {
        char c = Serial.read();
        if (isAlphaNumeric(c)) 
        {   
            
            if(c == '1') 
    			Serial.println(F("Now Testing NTC Thermistor 10k"));
            else
            {
                Serial.println(F("illegal input!"));
                return 0;
            }
            time0 = millis();
            return c;
        }
    }
}

