#include <Arduino.h>
double V1 = 0;
double I =0;
int Pin = 39;
void setup()
{
  Serial.begin(9600);
  adcAttachPin(Pin);
  adcStart(Pin);
  adcAttachPin(32);
  adcStart(32);
  pinMode(5, OUTPUT);
  analogReadResolution(11);
  analogSetAttenuation(ADC_11db);
  digitalWrite(5, LOW);
}

void loop()
{
  V1 = averageAnalogReading(300.0, Pin)*0.0263+2.4093;
  I = 0.1124*averageAnalogReading(300, 32)-16.906;
  //I = averageAnalogReading(300, 32);
  Serial.print("V1: ");
  Serial.print(V1,4);
  Serial.print("  I: ");
  Serial.println(I,4);
  delay(100);
}

double averageAnalogReading(double samples, int analogPin)
{
  double temp = 0;
  for (size_t i = 0; i < samples; i++)
  {
    /* code */
    temp += analogRead(analogPin);
  }
  return temp / samples;
}
