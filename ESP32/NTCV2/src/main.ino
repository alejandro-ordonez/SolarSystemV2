#include <Arduino.h>
#define Resolution 2048
#define Resbase 10000 //10Kohm R2
#define RTNOM 12226
#define NOMINAL_TEMPERATURE 16.7
#define B 3950

int RES =0;
void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  adcAttachPin(27);
  adcStart(27);
  analogReadResolution(11);       // Default of 12 is not very linear. Recommended to use 10 or 11 depending on needed resolution.
  analogSetAttenuation(ADC_11db); // Default is 11db which is very noisy. Recommended to use 2.5 or 6.
}

void loop() {
  // put your main code here, to run repeatedly:
  Serial.print("Res: ");
  RES = NTCREs(Average(700.0));
  Serial.print(RES);
  Serial.print(" Temp: ");
  Serial.println(Temp(RES));
  delay(10);
}
double NTCREs(double adc){
  double res = (Resolution/adc) -1;
  return Resbase/res;
}

double Average(double samples){
  double avg = 0;
  double r =0;
  for (size_t i = 0; i < samples; i++)
  {
      /* code */
      r=analogRead(35);
      //Serial.println(r);
      delay(1);
      avg+=r;
  }
  return avg/samples;
}
double Temp(double average){
  double steinhart;
  steinhart = average / RTNOM;     // (R/Ro)
  steinhart = log(steinhart);                  // ln(R/Ro)
  steinhart /= B;                   // 1/B * ln(R/Ro)
  steinhart += 1.0 / (NOMINAL_TEMPERATURE + 273.15); // + (1/To)
  steinhart = 1.0 / steinhart;                 // Invert
  steinhart -= 273.15;                         // convert to C
  return steinhart;
}