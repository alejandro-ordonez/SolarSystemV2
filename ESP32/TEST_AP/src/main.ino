#include <Arduino.h>
#include <WiFi.h>
#include <WebServer.h>

const char* ssid     = "ESP32-Access-Point";
const char* password = "123456789";
hw_timer_t * timer = NULL;
portMUX_TYPE timerMux = portMUX_INITIALIZER_UNLOCKED;
int totalInterruptCounter;
volatile int interruptCounter=0;
WebServer server(80);

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  Serial.print("Setting AP (Master Puto)…");
  // Remove the password parameter, if you want the AP (Access Point) to be open
  WiFi.softAP(ssid, password);
  IPAddress IP = WiFi.softAPIP();
  Serial.print("AP IP address: ");
  Serial.println(IP);

  server.on("/", handle_Data);
  server.begin();
}

void loop() {
  // put your main code here, to run repeatedly:
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
  }
server.handleClient();
}

void handle_Data()
{
  server.send(200, "text/plain", "Puto");
  StartTimer();
}

void StartTimer(){
  timer = timerBegin(0, 80, true);
  timerAttachInterrupt(timer, &OnTimer, true);
  timerAlarmWrite(timer, 100000, true);
  timerAlarmEnable(timer);
}

void IRAM_ATTR OnTimer() {
  portENTER_CRITICAL_ISR(&timerMux);
  interruptCounter++;
  portEXIT_CRITICAL_ISR(&timerMux);

}

void StopTimer(){
  if (timer) {
      // Stop and free timer
      totalInterruptCounter =0;
      timerEnd(timer);
}