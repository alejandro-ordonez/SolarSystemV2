#include <Arduino.h>
#include <WiFi.h>
#include <WebServer.h>
#include <ArduinoJson.h>
const char *ssid = "MPS";               //"Invitados_UTADEO";
const char *password = "Siemenss71500"; //"ylch0286";


int PWM = 14;
int maxValuePWM = 65535;
int currentSensor = 32;
int dacMos = 25;
int sw = 34;
const int capacity = 2*JSON_ARRAY_SIZE(1000)+2*JSON_OBJECT_SIZE(1); 
StaticJsonDocument<capacity> doc;
JsonArray arr = doc.createNestedArray("V");
JsonArray arr2 = doc.createNestedArray("I");
WebServer server(80);
String send;
void setup()
{
  // put your setup code here, to run once:
  Serial.begin(115200);
  adcAttachPin(currentSensor);
  adcStart(currentSensor);
  analogReadResolution(11);       // Default of 12 is not very linear. Recommended to use 10 or 11 depending on needed resolution.
  analogSetAttenuation(ADC_11db); // Default is 11db which is very noisy. Recommended to use 2.5 or 6.
  pinMode(currentSensor, INPUT);
  pinMode(2, OUTPUT);
  pinMode(sw, INPUT);
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
  server.on("/Data", handle_Data);
  server.begin();
 
  for (size_t i = 0; i < 1000; i++)
  {
    arr.add(i);
    arr2.add(5000-i);
  }
  
  
  serializeJson(doc, send);


  Serial.println("Finish");

  delay(1000);
}

void loop()
{
 server.handleClient(); 
}
void handle_Data(){
  server.send(200, "application/json", send);
}