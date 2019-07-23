#include <Arduino.h>
#define ISensor 32
double temp =0;
#define Resolution 2048
//Voltage Source
#define VSourve 3.3
// Max possible PWM value
void setup() {
  Serial.begin(9600);
  // put your setup code here, to run once:
  adcAttachPin(ISensor);
  adcStart(ISensor);
  pinMode(5, OUTPUT);
  analogReadResolution(11);       // Default of 12 is not very linear. Recommended to use 10 or 11 depending on needed resolution.
  analogSetAttenuation(ADC_11db);
}

void loop() {
  // put your main code here, to run repeatedly:
  digitalWrite(5, LOW);
  Serial.print("Corriente: ");
  Serial.print(readISensor(), 6);
  Serial.print(" Analog: ");
  temp = averageAnalogReading(100.0, ISensor);
  Serial.println(temp, 5);
  delay(10);
}

float readISensor()
{
  float I = 0;
  //Machete
  float vesp = (averageAnalogReading(3000.0, ISensor)) * (VSourve / Resolution)*1.1017565218703;
  //vesp-=2.2639167; 1.5901880
  I= vesp;
  return I;
}

double averageAnalogReading(double samples, int analogPin)
{
  double avg = 0;
  for (size_t i = 0; i < samples; i++)
  {
    /* code */
    avg += analogRead(analogPin);
  }
  return avg / samples;
}