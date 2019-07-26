#include <Arduino.h>
#define ISensor 32
double temp =0;
#define Resolution 4096
//Voltage Source
#define VSourve 3.3
// Max possible PWM value
double pwm =0;
double error=0, KP=200, KD=5, KI=1, lastError=0, sumError=0;
void setup() {
  Serial.begin(9600);
  // put your setup code here, to run once:
  adcAttachPin(ISensor);
  adcStart(ISensor);
  analogReadResolution(12);       // Default of 12 is not very linear. Recommended to use 10 or 11 depending on needed resolution.
  analogSetAttenuation(ADC_11db);
  ledcSetup(0, 5000, 16);
  ledcAttachPin(5, 0);
}

void loop() {
  // put your main code here, to run repeatedly:
  for (size_t i = 0; i < 65000; i+=300)
  { 
    Serial.println(readISensor(),5);
    ledcWrite(0, i);
    delay(1000);
  }
  
  /*for (double i = 0; i < 1; i+=0.01)
  { 
    temp=readISensor();
    print(temp, i, pwm);
    while (temp<i)
    {
      temp=readISensor();
      print(temp, i,pwm);
      error = i-temp;
      pwm+=KP*error + KD * (error - lastError)+ KI*sumError;
      if(pwm<0)
        pwm=0;
      sumError+=error;
      lastError=error;
      ledcWrite(0, pwm);
      delay(1);  
    }
      
  }*/
}

float readISensor()
{
  float I =(averageAnalogReading(400.0, ISensor)) * (VSourve / Resolution);

  I*=15.336;
  I-=23.26;
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

void print(double t, double i, double p){
    Serial.print("PWM: ");
    Serial.print(p);
    Serial.print("  Sensor: ");
    Serial.print(temp,5);
    Serial.print("  Corriente: ");
    Serial.println(i,4);
}