#include <Arduino.h>
#include <driver/adc.h>
#include <ArduinoJson.h>
#include <Solar.h>
#include <WiFi.h>
#include <aREST.h>
#include <RTClib.h>
const char *ssid = "MPS";//"Invitados_UTADEO";
const char *password = "Siemenss71500";//"ylch0286";
const int capacity = 2*JSON_ARRAY_SIZE(1311) + JSON_OBJECT_SIZE(2); 
WiFiServer server(80);
aREST rest = aREST();
int PWM = 14;
int maxValuePWM = 65535;
int currentSensor = 32;
int dacMos = 25;
int sw = 34;
double readingI = 0;
double voltageConverter = (3.3 / 2048.0);
String year;
String month;
String day;
String hours;
String minutes;
String seconds;
StaticJsonDocument<capacity> doc;
String send = "";
Solar panel = Solar();

RTC_DS3231 rtc;
char daysOfTheWeek[7][12] = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"};
 
void setup()
{

  adcAttachPin(currentSensor);
  adcStart(currentSensor);
  analogReadResolution(11);       // Default of 12 is not very linear. Recommended to use 10 or 11 depending on needed resolution.
  analogSetAttenuation(ADC_11db); // Default is 11db which is very noisy. Recommended to use 2.5 or 6.
  pinMode(currentSensor, INPUT);
  pinMode(2, OUTPUT);
  pinMode(sw, INPUT);
  rest.function("test", Get);
  rest.variable("json", &send);
  // put your setup code here, to run once:
  //Probando git hub
  Serial.begin(9600);
  Serial.print("JSON Capacity");
  Serial.println(capacity);
  ledcSetup(0, 5000, 16);
  ledcAttachPin(PWM, 0);
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
    Serial.print(".");
  }
  // Print local IP address and start web server
  Serial.println("");
  Serial.println("WiFi connected.");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
  server.begin();
  delay(2000);

   if (! rtc.begin()) {
    Serial.println("Couldn't find RTC");
    while (1);
  }
   if (rtc.lostPower()) {
    Serial.println("RTC lost power, lets set the time!");
    
    rtc.adjust(DateTime(F(__DATE__), F(__TIME__)));
}

}
void loop()
{
  if (digitalRead(sw))
  {
    doc.clear();
    JsonObject Panel = doc.createNestedObject("Panel");
    JsonArray Readings = doc.createNestedArray("Readings");
    Panel["IR"]= panel.getIR();
    Panel["T"] = panel.getT();
    
    Serial.println("***********************");
    Serial.println("****Caracterizando*****");
    Serial.println("***********************");
    send = "";
    for (int i = 0; i < maxValuePWM; i += 50)
    {
      digitalWrite(2, HIGH);
      ledcWrite(0, i);
      Serial.print("PWM: ");
      Serial.print(i);
      Serial.print("   Corriente: ");
      readingI = readSensor(1300.0);
      Serial.println(readingI, 9);
      //panel.addCurrentAndVoltage(readingI, 5);
      JsonObject feed1 = Readings.createNestedObject();
      feed1["I"]=readingI;
      feed1["V"]=readingI;
      delay(50);
      digitalWrite(2, LOW);
      delay(50);
    }
    delay(8000);
    digitalWrite(2, HIGH);
    //panel.resetList();
    serializeJson(doc, send);
    
  }
  else
  {
    Serial.println("***********************");
    Serial.println("****Server iniciado****");
    Serial.println("***********************");
    Serial.println();
    Serial.println();
    //Serial.println(send);
    WiFiClient client = server.available();
    if (client)
    {
      while (!client.available())
      {
        delay(5);
      }
      rest.handle(client);
    }
  }
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
}
float readSensor(int n)
{
  float reading = 0;
  double I = 0;
  for (int i = 0; i < n; i++)
  {
    //reading+=analogRead(currentSensor);
    reading += (analogRead(currentSensor) - 1422.520075) * (voltageConverter); //lectura del sensor
    //I+=(reading/ 0.1);
  }
  reading = (reading / n) + 0.00753246 + 0.000166091;
  //return (reading/n);
  I = reading / 0.09747542864;
  return analogRead(currentSensor);

   DateTime now = rtc.now();
     
    Serial.print(now.year(), DEC);
    Serial.print('/');
    Serial.print(now.month(), DEC);
    Serial.print('/');
    Serial.print(now.day(), DEC);
    Serial.print(" (");
    Serial.print(daysOfTheWeek[now.dayOfTheWeek()]);
    Serial.print(") ");
    Serial.print(now.hour(), DEC);
    Serial.print(':');
    Serial.print(now.minute(), DEC);
    Serial.print(':');
    Serial.print(now.second(), DEC);
    Serial.println();
    delay(3000);
}
int Get(String command)
{
  Serial.println("Received rest request");
}//192.168.8.188