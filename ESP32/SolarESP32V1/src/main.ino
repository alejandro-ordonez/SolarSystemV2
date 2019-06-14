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
#define Resolution 2048
//Variables to connect to wifi network
//TODO: It should be an acces point
const char *ssid = "MPS";//"Invitados_UTADEO";
const char *password = "Siemenss71500";//"ylch0286";
//Webserver on port 80
WebServer server(80);
#pragma endregion
////////////////////////////////////////////////////////////////////////////


//////////////////////////Thermistor Constants//////////////////////////////
#pragma region 
// R2 in voltage divider
#define Resbase 10000 //10Kohm R2
// Resistance obtained at 16.7 °C
#define RTNOM 12226
// Temperature in test conditions
#define NOMINAL_TEMPERATURE 16.7
// Beta value between 3000-4000
#define B 3950
// ADC Pin to read Thermistor
#define Thermistor 27
#pragma endregion
////////////////////////////////////////////////////////////////////////////


/////////////////////////Voltage and Current on Panel constants/////////////////////////
// Scaled factor determined by voltage divider
#define Scale 7.27
#define ISensor 32
#define VS1 26
#define VS2 33
////////////////////////////////////////////////////////////////////////////


////////////////////////RTC Clock //////////////////////////////////////////
//Object to manipulate time
//The RTC is connected to 21 y 22 pins (SDA, SCL)
RTC_DS3231 Clock;
////////////////////////////////////////////////////////////////////////////


//////////////////////JSON Document/////////////////////////////////////////
#pragma region 
//Capacity required to save all the data
const int capacity = 2*JSON_ARRAY_SIZE(1311) + JSON_OBJECT_SIZE(2); 
//Jason Document created to save data
StaticJsonDocument<capacity> doc;
//Variable used to be exposed on rest service
String send = "";
//JasonArray to add values obtained by sensors
JsonArray arr = doc.createNestedArray("V");
JsonArray arr2 = doc.createNestedArray("I");
#pragma endregion
////////////////////////////////////////////////////////////////////////////




void setup() {
  // put your setup code here, to run once:


  ////////////////////////////////////INPUTS///////////////////////////////
  #pragma region //ADC Thermistor
  adcAttachPin(Thermistor);
  adcStart(Thermistor);
  #pragma endregion
  #pragma region //CurrentSensor
  adcAttachPin(ISensor);
  adcStart(ISensor);
  pinMode(ISensor, INPUT);
  #pragma endregion
  #pragma region //ADC Voltage
  //V1
  adcAttachPin(VS1);
  adcStart(VS1);
  pinMode(VS1, INPUT); 
  //V2
  adcAttachPin(VS2);
  adcStart(VS2);
  pinMode(VS2, INPUT);
  #pragma endregion
  //Other pins...
  pinMode(Indicator, OUTPUT);
  pinMode(SW, INPUT);
  //Set ADC properties
  analogReadResolution(11);       // Default of 12 is not very linear. Recommended to use 10 or 11 depending on needed resolution.
  analogSetAttenuation(ADC_11db); // Default is 11db which is very noisy. Recommended to use 2.5 or 6.
///////////////////////////////////////////////////////////////////////////

  /////////////////////////////////Connection to Wifi//////////////////////
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
  ////////////////////////////////////////////////////////////////////////

  ////////////////////////////////Server EndPoints////////////////////////
  //Get Method to obtain measured data
  server.on("/Data", handle_Data);
  //Server initialization
  server.begin();
  ///////////////////////////////////////////////////////////////////////
}

void loop() {
  // put your main code here, to run repeatedly:
}

//Temperature:

double NTCRes(double adc){
  double res = (Resolution/adc) -1;
  return Resbase/res;
}

double Average(double samples){
  double avg = 0;
  for (size_t i = 0; i < samples; i++)
  {
      /* code */
    avg+=analogRead(27);
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

void handle_Data(){
  server.send(200, "application/json", send);
}