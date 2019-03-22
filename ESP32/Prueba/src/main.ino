#include <Arduino.h>
int dacMos = 25;
int sensorReading = 35;
double current[500];

int count = 0;
void setup()
{
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(2, OUTPUT);
}

void loop()
{
  // put your main code here, to run repeatedly:
  //digitalWrite(2, LOW);
  //delay(1000);
  for (int i = 0; i <= 255; i++)
  {
    Serial.println(readSensor());
    dacWrite(dacMos, i);
    delay(150);
  }
  for (int i = 255; i >=0; i--)
  {
    Serial.println(readSensor());
    dacWrite(dacMos, i);
    delay(150);
  }
  //digitalWrite(2, HIGH);
  //delay(1000);
}
float readSensor()
{
  float reading =analogRead(35)*(4095 / 3300); //lectura del sensor   
  float I=((reading - 2500) / 100);
  return I;
}