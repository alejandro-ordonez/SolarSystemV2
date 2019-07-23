#include <Arduino.h>
int Pira = 33;
void setup()
{
  adcAttachPin(Pira);
  analogReadResolution(11);
  analogSetAttenuation(ADC_6db);
  analogSetWidth(11);
  adcStart(Pira);

  Serial.begin(9600);
}

void loop()
{

  double VoltajePira = radiation();
  Serial.print("Radiation: ");
  Serial.println(VoltajePira, 9);
  delay(1000);
}
double averageAnalogReading(double samples, int analogPin)
{
  float temp = 0;
  for (size_t i = 0; i < samples; i++)
  {
    /* code */
    temp += analogRead(analogPin);
  }
  return temp / samples;
}
double radiation()
{
  double cal = 0;

  cal = (averageAnalogReading(200.0, Pira) * (2.0 / 2048.0));
  cal /= 27.5;
  Serial.println(cal, 5);
  cal *= 1000000;
  cal /= 61.5;
  return cal;
}
