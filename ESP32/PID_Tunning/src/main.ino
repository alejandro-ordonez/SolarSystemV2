#include <Arduino.h>

#define VN 39
#define ISensor 32

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  analogReadResolution(11);
  ledcSetup(0, 50000, 16);
  ledcAttachPin(5, 0);
  ledcWrite(0, 65536);
}

void loop() {
  // put your main code here, to run repeatedly:
  Serial.print(getVoltage());
  Serial.print(",");
  delayMicroseconds(1);
}


double averageAnalogReading(double samples, int analogPin)
{
  double avg = 0;

  for (size_t i = 0; i < samples; i++)
  {
    /* code */
    avg+=analogRead(analogPin);
  }
  return avg/samples;
  //return lowpassFilter.output();
}
double getVoltage(){
  return 0.0264*averageAnalogReading(300,VN)+2.4251;//  *19.66720169 * (3.3/4096);
}
