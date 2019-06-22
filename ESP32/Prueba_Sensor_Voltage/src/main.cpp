#include <Arduino.h>
double R1 =1200000.0;
double R2 =1000000.0;
int VF =11.3;
int VT =0.265;
const double VTrans=25;
const double VFuent=26;
double Req=0 ;
double VoltajeDif=0 ;
double Voltaje=0 ;
void setup() {

 Serial.begin (9600);

 Req = (R2/(R1+R2));

}

void loop() {

double ValueFuent = analogRead(VFuent)* (VF/ 4096.0); 
double ValueTrans = analogRead(VTrans)* (VT/ 4096.0); 
float VoltajeSensorFuent =(ValueFuent/Req);
float VoltajeSensorTrans = (ValueTrans/Req);
VoltajeDif =VoltajeSensorFuent-VoltajeSensorTrans;
Voltaje=VoltajeDif;
delay(1000);
Serial.print("Voltaje: ");
Serial.println(Voltaje);
Serial.print("V1: ");
Serial.println(analogRead(VFuent));
Serial.println(VoltajeSensorFuent);
Serial.print("V2: ");
Serial.println(analogRead(VTrans));
Serial.println(VoltajeSensorTrans);


}