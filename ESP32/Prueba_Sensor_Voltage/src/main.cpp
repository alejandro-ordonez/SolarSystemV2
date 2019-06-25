#include <Arduino.h>

double R1 =1200000.0;
double R2 =1000000.0;
int VF =11.3;
int VT =0.265;
int VTrans=25;
int VFuent=26;
double Req=0 ;
double VoltajeDif=0 ;
double Voltaje=0 ;
void setup() {
  adcAttachPin(VTrans);
  adcStart(VTrans);
  analogReadResolution(11);       
  analogSetAttenuation(ADC_11db);   

  adcAttachPin(VFuent);
  adcStart(VFuent);
  analogReadResolution(11);       
  analogSetAttenuation(ADC_11db);  

 Serial.begin (9600);

 Req = (R2/(R1+R2));

}

void loop() {
    
double VoltajeSensorFuent =averageAnalogReading(200, VFuent)*(3.3/ 2048.0);
double VoltajeSensorTrans =averageAnalogReading(200, VTrans)*(3.3/ 2048.0);
VoltajeDif =VoltajeSensorFuent-VoltajeSensorTrans;
Voltaje=VoltajeDif*3.6363;
Serial.print("V1: ");
Serial.println(VoltajeSensorFuent);
Serial.print("V2: ");
Serial.println(VoltajeSensorTrans);
Serial.print("Voltaje: ");
Serial.println(Voltaje);
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

}double averageAnalogReading(double samples, int analogPin){
  float temp = 0;
  for (size_t i = 0; i < samples; i++)
  {
    /* code */
    temp+=analogRead(analogPin);
  }
  return temp/samples;  
}



