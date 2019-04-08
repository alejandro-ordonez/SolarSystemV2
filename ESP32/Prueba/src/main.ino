#include <Arduino.h>
int dacMos = 25;
int sensorReading = 35;
double current[500];
int readP;
int count = 0;
int dac=0;
void setup()
{
  // put your setup code here, to run once:
  //Probando git hub
  Serial.begin(9600);
  pinMode(2, OUTPUT);
}

void loop()
{
  // put your main code here, to run repeatedly:
  //digitalWrite(2, LOW);
  //delay(1000);
  readP = analogRead(35);
  dac= map(readP,0,4095,0,255);
  dacWrite(dacMos,dac);
  Serial.println(dac);

  //digitalWrite(2, HIGH);
  //delay(1000);
}
/*float readSensor()
{
  float reading =analogRead(35)*(4095 / 3300); //lectura del sensor   
  float I=((reading - 2500) / 100);
  return I;
}*/