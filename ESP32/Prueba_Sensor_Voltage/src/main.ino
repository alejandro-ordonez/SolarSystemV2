#include <Arduino.h>

double Vf = 11.85;
double Vd1 = 3.0;
double Vt = 0.460;
double Vd2 = 0.120;

double p1 = Vf/Vd1;
double p2 = Vt/Vd2;

double R1 = 1200000.0;
double R2 = 1000000.0;

#define MaxValuePWM 65535
#define PWM 14

int VTrans = 25;
int VFuent = 26;
double Req = 0;
double VoltajeDif = 0;
double Voltaje = 0;
double VoltajeSensorFuent;
double VoltajeSensorTrans;
void setup()
{
  adcAttachPin(VTrans);
  adcStart(VTrans);

  adcAttachPin(VFuent);
  adcStart(VFuent);
  analogReadResolution(12);
  analogSetAttenuation(ADC_11db);

  ledcSetup(0, 5000, 16);
  ledcAttachPin(PWM, 0);
  Serial.begin(9600);

  Req = (R2 / (R1 + R2));
}

void loop()
{
  ledcWrite(0, 20000);
  VoltajeSensorFuent = (averageAnalogReading(600.0, VFuent) / 4096.0)*13*(340206.186/320000);
  VoltajeSensorTrans = (averageAnalogReading(600.0, VTrans) / 4096.0)*13*(340206.186/320000);
  VoltajeDif = (VoltajeSensorFuent) - (VoltajeSensorTrans);
  Serial.print("V1: ");
  Serial.println(VoltajeSensorFuent);
  Serial.print("V2: ");
  Serial.println(VoltajeSensorTrans);
  Serial.print("Voltaje: ");
  Serial.println(VoltajeDif);
  delay(100);
  //double ValueFuent = analogRead(VFuent)* (3.3/ 4096.0);
  //double ValueTrans = analogRead(VTrans)* (3.3/ 4096.0);
  //float VoltajeSensorFuent =(ValueFuent/Req);
  //float VoltajeSensorTrans = (ValueTrans/Req);
  //VoltajeDif =VoltajeSensorFuent-VoltajeSensorTrans;
  //Voltaje=VoltajeDif*3.6363;
  //delay(1000);
  //Serial.print("Voltaje: ");
  //Serial.println(Voltaje);
  //Serial.print("V1: ");
  //Serial.println(analogRead(VFuent));
  //Serial.println(VoltajeSensorFuent);
  //Serial.print("V2: ");
  //Serial.println(analogRead(VTrans));
  //Serial.println(VoltajeSensorTrans);
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
