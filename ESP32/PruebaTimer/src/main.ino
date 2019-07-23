#include <Arduino.h>
#include <ArduinoJson.h>
#include <WiFi.h>
#include <WebServer.h>
volatile int interruptCounter;
int totalInterruptCounter;
bool state = false;
hw_timer_t * timer = NULL;
portMUX_TYPE timerMux = portMUX_INITIALIZER_UNLOCKED;
const char *ssid = "MPS";               //"Invitados_UTADEO";
const char *password = "Siemenss71500"; //"ylch0286";
StaticJsonDocument<50000> doc;
String send = "";
JsonArray arr;
JsonArray arr2; 
WebServer server(80);
void setup() {
 
  Serial.begin(9600);
  pinMode(34, INPUT);
  attachInterrupt(digitalPinToInterrupt(34), OnPushed, RISING);
  arr = doc.createNestedArray("V");
  arr2 = doc.createNestedArray("I");
  pinMode(2, OUTPUT);
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
  server.on("/Start",StartTimer);
  server.begin();
 
}
 
void loop() {
 
  if (interruptCounter > 0) {
 
    portENTER_CRITICAL(&timerMux);
    interruptCounter--;
    portEXIT_CRITICAL(&timerMux);
 
    totalInterruptCounter++;
    Serial.print("An interrupt has occurred. Total number: ");
    Serial.println(totalInterruptCounter);
    if(totalInterruptCounter>10){
      StopTimer();
      Serial.println("Terminado");
      totalInterruptCounter=0;
    }

  server.handleClient(); 
  }
}
int randomNumber(){
  return random(1,100);
}

////////////////////////// On Pushed Button /////////////////////
void IRAM_ATTR OnPushed() {
  Serial.println("Button Interrupt");
  state = !state;
  if(state){
    digitalWrite(2, HIGH);
    StartTimer();
  }
  else{
    digitalWrite(2, LOW);
    StopTimer();
  }
}
///////////////////////// Timer Triggered ///////////////////////
void IRAM_ATTR OnTimer() {
  portENTER_CRITICAL_ISR(&timerMux);
  interruptCounter++;
  arr.add(randomNumber());
  arr2.add(randomNumber());
  portEXIT_CRITICAL_ISR(&timerMux);

}
void StartTimer(){
  timer = timerBegin(0, 80, true);
  timerAttachInterrupt(timer, &OnTimer, true);
  timerAlarmWrite(timer, 1000000, true);
  timerAlarmEnable(timer);
}
void StopTimer(){
  if (timer) {
      // Stop and free timer
      totalInterruptCounter =0;
      timerEnd(timer);
      timer = NULL;
    }
}
void handle_Data(){
  serializeJson(doc, send);
  server.send(200, "application/json", send);
  doc.clear();
  send="";
}
