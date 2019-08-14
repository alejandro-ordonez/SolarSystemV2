#include <Arduino.h>
#define ISensor 32
double temp =0;
#define Resolution 4096
//Voltage Source
#define VSourve 3.3
#define LED 2
#define VN 39
// Max possible PWM value
double pwm =0;
double error=0, KP=200, KD=5, KI=1, lastError=0, sumError=0;
void setup() {
  Serial.begin(9600);
  // put your setup code here, to run once:
  adcAttachPin(ISensor);
  adcAttachPin(VN);
  adcStart(VN);
  adcStart(ISensor);

  analogReadResolution(12);       // Default of 12 is not very linear. Recommended to use 10 or 11 depending on needed resolution.
  analogSetAttenuation(ADC_11db);
  pinMode(LED, OUTPUT);
  ledcSetup(0, 10000, 16);
  ledcAttachPin(5, 0);
}

void loop() {
  // put your main code here, to run repeatedly:
  for (size_t i = 0; i < 65000; i+=300)
  { 
    Serial.print(ReadVoltage(ISensor),5);
    Serial.print(" ");
    Serial.println(ReadVoltage(VN), 5);
    ledcWrite(0, i);
    digitalWrite(LED, HIGH);
    delay(100);
    digitalWrite(LED, LOW);
    delay(100);
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
  float I =(ReadVoltage(ISensor)) * (VSourve / Resolution);

  //I*=15.336;
  //I-=23.26;
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

double ReadVoltage(byte pin)
{
  double reading = averageAnalogReading(300.0, pin); // Reference voltage is 3v3 so maximum reading is 3v3 = 4095 in range 0 to 4095
  if(reading < 1 || reading > 4095) return 0;
  //return -0.000000000009824 * pow(reading,3) + 0.000000016557283 * pow(reading,2) + 0.000854596860691 * reading + 0.065440348345433;
  return -0.000000000000016 * pow(reading,4) + 0.000000000118171 * pow(reading,3)- 0.000000301211691 * pow(reading,2)+ 0.001109019271794 * reading + 0.034143524634089;
} // Added an improved polynomial, use either, comment out as requi

void print(double t, double i, double p){
    Serial.print("PWM: ");
    Serial.print(p);
    Serial.print("  Sensor: ");
    Serial.print(temp,5);
    Serial.print("  Corriente: ");
    Serial.println(i,4);
}