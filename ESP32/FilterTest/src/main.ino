#include <Arduino.h>
#include <digcomp.h>

//5Hz
//float b[]={0.072820507087338, 0.072820507087338};
//float a[]={1.000000000000000, -0.854358985825324};

//10Hz
//float b[]={0.072820507087338, 0.072820507087338};
//float a[]={1.000000000000000, -0.854358985825324};

//15Hz
//float b[]={0.940148300306698  , 0.940148300306698};
//float a[]={1.000000000000000, 0.880296600613396};

//10Hz
float b[]={0.0304590279514212,0.0304590279514212};
float a[]={1.000000000000000,-0.939081944097158};
float lp_in[2];
float lp_out[2];
float temp = 0;
long int ta=0;
long int tb=0;
dig_comp filter(b,a, lp_in, lp_out, 2,2);

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  adcAttachPin(39);
  adcStart(39);
  ledcSetup(0, 50000, 16);
  ledcAttachPin(5, 0);
  //ledcWrite(0,65536);
}

void loop() {
  for (size_t i = 0; i < 65536; i+=1)
    { 
      ta=micros();
      ledcWrite(0,i);
      // put your main code here, to run repeatedly:
      temp = analogRead(39);
    //Serial.print(ReadVoltage(filter.calc_out(temp)));
    Serial.print(filter.calc_out(temp),5);
    Serial.print("    ");
    Serial.println(temp,5);
     tb=micros();
     if((tb-ta)<100){
       delayMicroseconds(100-(tb-ta));
     }
  }
  }

double ReadVoltage(double reading)
{
  if(reading < 1 || reading > 4095) return 0;
  //return -0.000000000009824 * pow(reading,3) + 0.000000016557283 * pow(reading,2) + 0.000854596860691 * reading + 0.065440348345433;
  return -0.000000000000016 * pow(reading,4) + 0.000000000118171 * pow(reading,3)- 0.000000301211691 * pow(reading,2)+ 0.001109019271794 * reading + 0.034143524634089;
} // Added an improved polynomial, use either, comment out as requi