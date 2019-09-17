#include <Arduino.h>
int Pira = 33;
void setup()
{
  adcAttachPin(Pira);
  analogReadResolution(12);
  analogSetAttenuation(ADC_11db);
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

  cal = ReadVoltage(Pira);
  cal /= 27.5;
  Serial.println(cal, 5);
  cal *= 1000000;
  cal /= 61.5;
  //cal -=353.055132114;
  return cal;
}

double ReadVoltage(byte pin)
{
  double reading = averageAnalogReading(100.0, pin);//analogRead(pin); // Reference voltage is 3v3 so maximum reading is 3v3 = 4095 in range 0 to 4095
  if(reading < 1 || reading > 4095) return 0;
  //return -0.000000000009824 * pow(reading,3) + 0.000000016557283 * pow(reading,2) + 0.000854596860691 * reading + 0.065440348345433;
  return -0.000000000000016 * pow(reading,4) + 0.000000000118171 * pow(reading,3)- 0.000000301211691 * pow(reading,2)+ 0.001109019271794 * reading + 0.034143524634089;
} // Adde