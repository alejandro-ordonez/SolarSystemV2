#include <Arduino.h>
int PWM = 14;
int maxValuePWM = 65535;
int currentSensor = 27;
int dacMos = 25;
int sensorReading = 35;
double current[500];
int readP;
int count = 0;
int dac=0;
double voltageConverter = 0.805860805;
void setup()
{
  // put your setup code here, to run once:
  //Probando git hub
  Serial.begin(9600);
  ledcSetup(0,5000, 16);
  ledcAttachPin(PWM,0);

  pinMode(2, OUTPUT);
}

void loop()
{
  for(int i=0; i< maxValuePWM; i+=10)
  {
    ledcWrite(0, i);
    Serial.print("PWM: ");
    Serial.print(i);
    Serial.print("   Corriente: ");
    Serial.println(readSensor(),6);
    delay(1);
  }
  for(int i = maxValuePWM; i > 0; i-=10)
  {
    ledcWrite(0, i);
    Serial.print("PWM: ");
    Serial.print(i);
    Serial.print("   Corriente: ");
    Serial.println(readSensor(),6);
    delay(1);
    
  }
 
  // put your main code here, to run repeatedly:
  //digitalWrite(2, LOW);
  //delay(1000);
  /*readP = analogRead(35);
  dac= map(readP,0,4095,0,255);
  dacWrite(dacMos,dac);
  Serial.println(dac);
*/
  //digitalWrite(2, HIGH);
  //delay(1000);
}
double readSensor()
{
  double reading=0; 
  double I=0;
  for(int i = 0; i < 100; i++)
  {
    reading +=analogRead(currentSensor)*(voltageConverter); //lectura del sensor   
  }
  reading=reading/100.0;
  I=((reading-500.0)/ 100.0);
  
  return I;
}