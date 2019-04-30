#include <Arduino.h>
#include <driver/adc.h>
#include <Solar.h>
#include <WiFi.h>
#include <aREST.h>
const char* ssid     = "MPS";
const char* password = "Siemenss71500";
WiFiServer server(80);
aREST rest = aREST();
String header;
int PWM = 14;
int maxValuePWM = 65535;
int currentSensor = 32;
int dacMos = 25;
int sensorReading = 35;
int count = 0;
double readingI=0;
double voltageConverter = (3.3/2048.0);
Solar panel;
void setup()
{
  adcAttachPin(currentSensor);
	adcStart(currentSensor);
	analogReadResolution(11); // Default of 12 is not very linear. Recommended to use 10 or 11 depending on needed resolution.
	analogSetAttenuation(ADC_11db); // Default is 11db which is very noisy. Recommended to use 2.5 or 6.

	pinMode(currentSensor, INPUT);
  // put your setup code here, to run once:
  //Probando git hub
  Serial.begin(9600);
  ledcSetup(0,5000, 16);
  ledcAttachPin(PWM,0);

  pinMode(2, OUTPUT);

Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  // Print local IP address and start web server
  Serial.println("");
  Serial.println("WiFi connected.");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
  server.begin();

}

void loop()
{
  for(int i=0; i< maxValuePWM; i+=15)
  {
    ledcWrite(0, i);
    Serial.print("PWM: ");
    Serial.print(i);
    Serial.print("   Corriente: ");
    readingI =readSensor(1300.0);
    Serial.println(readingI, 9);
    panel.addCurrentAndVoltage(readingI, 5);
    delay(2);
  }
  delay(8000); 
  // put your main code here, to run repeatedly:
  //digitalWrite(2, LOW);
  //delay(1000);
  /*readP = analogRead(35);  PWM: 65535   Corriente: 0.787616
  PWM: 35   Corriente: 0.046351
  dac= map(readP,0,4095,0,255);
  dacWrite(dacMos,dac);
  Serial.println(dac);
  PWM: 65350   Corriente: 0.000260
  PWM: 65500   Corriente: 0.000250 - 0.70
  PWM: 35   Corriente: 0.000035
  PWM: 35   Corriente: -0.000019 0.02

  PWM: 35   Corriente: -0.009401 0.02
  PWM: 65500   Corriente: 0.060164 0.70
  PWM: 65500   Corriente: 0.059932 0.70


PWM: 35   Corriente: 0.012153
  PWM: 65500   Corriente: 0.786558 0.74


PWM: 65500   Corriente: 0.613481
PWM: 35   Corriente: -0.173910

PWM: 65525   Corriente: 0.614585338
PWM: 65525   Corriente: 0.061553974 0.60
PWM: 10   Corriente: -0.009998139
-0.000736253
*/
  //digitalWrite(2, HIGH);
  //delay(1000);
    WiFiClient client = server.available();
  if (client) {
 
    while(!client.available()){
      delay(5);
    }
    rest.handle(client);
  }
}
double readSensor(double n)
{
  double reading=0; 
  double I=0;
  for(double i = 0; i < n; i++)
  {
    //reading+=analogRead(currentSensor);
    reading +=(analogRead(currentSensor)-1422.520075)*(voltageConverter); //lectura del sensor
    //I+=(reading/ 0.1);
  }
  reading =(reading/n)+0.00753246+0.000166091;
  //return (reading/n);
  I=reading/0.09747542864;
  return I;
}
