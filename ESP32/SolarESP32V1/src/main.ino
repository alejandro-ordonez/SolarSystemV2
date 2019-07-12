#include <Arduino.h>
#include <WiFi.h>
#include <WebServer.h>
#include <ArduinoJson.h>
#include <RTClib.h>
/////////////////////////////ESP32 Variable/////////////////////////////////
#pragma region
//Led Indicator
#define Indicator 2
//Switch between measure and server
#define SW 34
//Resolution: 11 Bit linear zone
#define Resolution 4096
//Voltage Source
#define VSourve 3.3
// Max possible PWM value
#define MaxValuePWM 65535

#define PWM 14
// Declare a new timer
//hw_timer_t * timer = NULL;
//Variable to stablish if timer should be executed or not
volatile byte State = LOW;
//Variables to connect to wifi network
//TODO: It should be an acces point
const char *ssid = "MPS";               //"Invitados_UTADEO";
const char *password = "Siemenss71500"; //"ylch0286";

//Webserver on port 80
WebServer server(80);
#pragma endregion
////////////////////////////////////////////////////////////////////////////

//////////////////////////Thermistor Constants//////////////////////////////
#pragma region "Thermistor varibles"
// R2 in voltage divider
#define Resbase 10000 //10Kohm R2
// Resistance obtained at 16.7 °C
#define RTNOM 12226
// Temperature in test conditions
#define NOMINAL_TEMPERATURE 16.7
// Beta value between 3000-4000
#define B 3950
// ADC Pin to read Thermistor
#define Thermistor 35
#pragma endregion
////////////////////////////////////////////////////////////////////////////

////////////////Voltage and Current on Panel constants//////////////////////
// Scaled factor determined by voltage divider
#define Scale 7.27
#define ISensor 32
#define VS1 26
#define VS2 25
////////////////////////////////////////////////////////////////////////////

////////////////////////RTC Clock //////////////////////////////////////////
//Object to manipulate time
//The RTC is connected to 21 y 22 pins (SDA, SCL)
RTC_DS3231 Clock;
////////////////////////////////////////////////////////////////////////////

//////////////////////JSON Document/////////////////////////////////////////
#pragma region
//Capacity required to save all the data
const int capacity = 2 * JSON_ARRAY_SIZE(1311) + JSON_OBJECT_SIZE(5);
//Jason Document created to save data
StaticJsonDocument<capacity> doc;
//Variable used to be exposed on rest service
String send = "";
//JasonArray to add values obtained by sensors

#pragma endregion
////////////////////////////////////////////////////////////////////////////
float temp = 0;
float temp2 = 0;
float temp3 = 0;

void setup()
{
  // put your setup code here, to run once:

  Serial.begin(9600);
////////////////////////////////////INPUTS////////////////////////////////
#pragma region //ADC Thermistor
  adcAttachPin(35);
  adcStart(35);

  //CurrentSensor
  adcAttachPin(ISensor);
  adcStart(ISensor);
  //pinMode(ISensor, INPUT);

  //ADC Voltage
  //V1
  adcAttachPin(VS1);
  adcStart(VS1);
  //pinMode(VS1, INPUT);
  //V2
  adcAttachPin(VS2);
  adcStart(VS2);
  //pinMode(VS2, INPUT);

  //Other pins...
  pinMode(Indicator, OUTPUT);
  pinMode(SW, INPUT);
  //Set ADC properties
  analogReadResolution(12);       // Default of 12 is not very linear. Recommended to use 10 or 11 depending on needed resolution.
  analogSetAttenuation(ADC_11db); // Default is 11db which is very noisy. Recommended to use 2.5 or 6.
#pragma endregion
  //////////////////////////////////////////////////////////////////////////
  ledcSetup(0, 5000, 16);
  ledcAttachPin(PWM, 0);

//////////////////////////////////Connection to Wifi//////////////////////
#pragma region
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED)
  {
    Serial.print(".");
    delay(500);
  }
  // Print local IP address and start web server
  Serial.println("");
  Serial.println("WiFi connected.");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
#pragma endregion
  //////////////////////////////////////////////////////////////////////////

  /////////////////////////////////Server EndPoints/////////////////////////
  //Get Method to obtain measured data
  server.on("/Data", handle_Data);
  //Server initialization
  server.begin();
  //////////////////////////////////////////////////////////////////////////
}

void loop()
{
  // put your main code here, to run repeatedly:

  ///////////////////////Measure Data //////////////////////////////////////
  if (digitalRead(SW))
  {
    doc.clear();
    //Serialize Basic Data
    JsonObject Panel = doc.createNestedObject("Panel");
    Panel["IR"] = 5;
    Serial.print("Temp: ");
    temp3 = CalculateTemp();
    Serial.println(temp3);
    Panel["T"] = temp3;
    Panel["Time"] = ClockToString();

    Serial.println("***********************");
    Serial.println("****Caracterizando*****");
    Serial.println("***********************");
    send = "";
    JsonArray arr = doc.createNestedArray("V");
    JsonArray arr2 = doc.createNestedArray("I");
    for (int i = 0; i < MaxValuePWM; i += 50)
    {
      digitalWrite(2, HIGH);
      ledcWrite(0, i);
      Serial.print("PWM: ");
      Serial.print(i);
      Serial.print("   Corriente: ");
      temp = readISensor();
      temp2 = readVSensor();
      Serial.print(temp);
      Serial.print(" Voltaje: ");
      Serial.println(temp2);
      arr.add(temp);
      arr2.add(temp2);
      delay(50);
      digitalWrite(2, LOW);
      delay(50);
    }
    delay(8000);
    digitalWrite(2, HIGH);
    serializeJson(doc, send);
  }
  else
  {
    Serial.println("Server Available.....");
    server.handleClient();
  }
  //////////////////////////////////////////////////////////////////////////
}

///////////////////////////Temperature Methods//////////////////////////////
#pragma region
double Average(double samples){
  double avg = 0;
  for (size_t i = 0; i < samples; i++)
  {
      /* code */
      avg+=analogRead(35);
  }
  return avg/samples;
}

double NTCRes()
{
  double res = (Resolution / Average(700.0)) - 1;
  return Resbase / res;
}
double CalculateTemp()
{
  double steinhart;
  steinhart = NTCRes() / RTNOM; // (R/Ro)
  steinhart = log(steinhart);                        // ln(R/Ro)
  steinhart /= B;                                    // 1/B * ln(R/Ro)
  steinhart += 1.0 / (NOMINAL_TEMPERATURE + 273.15); // + (1/To)
  steinhart = 1.0 / steinhart;                       // Invert
  steinhart -= 273.15;                               // convert to C
  return steinhart;
}
#pragma endregion
////////////////////////////////////////////////////////////////////////////

///////////////////////////Clock Methods//////////////////////////////////// 192.168.0.104
#pragma region
String ClockToString()
{
  DateTime now = Clock.now();
  String time = "";
  //2012-04-23T18:25:43.511Z
  //Date
  time += now.year();
  time += "-";
  time += now.month();
  time += "-";
  time += now.day();
  time += "T";

  //Time
  time += now.hour();
  time += ":";
  time += now.minute();
  time += ":";
  time += now.second();
  time += "-05";
  return time;
}
void SetTime()
{
}
#pragma endregion
////////////////////////////////////////////////////////////////////////////

////////////////////////////Current Methods ////////////////////////////////
#pragma region
float readISensor()
{
  float I = 0;
  I = (averageAnalogReading(1000, ISensor) - 1422.520075) * (VSourve / Resolution);
  return I;
}
#pragma endregion
////////////////////////////////////////////////////////////////////////////

////////////////////////////Voltage Methods ////////////////////////////////
#pragma region
float readVSensor()
{
  float VoltageF =  (averageAnalogReading(600.0, VS1) / Resolution)*13*(340206.186/320000);
  float VoltageT =  (averageAnalogReading(600.0, VS2) / Resolution)*13*(340206.186/320000);
  return VoltageF-VoltageT;
}
#pragma endregion
////////////////////////////////////////////////////////////////////////////

////////////////////////////Generic Methods ////////////////////////////////
#pragma region
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
#pragma endregion
////////////////////////////////////////////////////////////////////////////

void handle_Data()
{
  server.send(200, "application/json", send);
}