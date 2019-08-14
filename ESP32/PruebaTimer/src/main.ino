#include <Arduino.h>
#include <ArduinoJson.h>
#include <WiFi.h>
#include <WebServer.h>
volatile int interruptCounter;
long int totalInterruptCounter;
bool state = false;
hw_timer_t * timer = NULL;
portMUX_TYPE timerMux = portMUX_INITIALIZER_UNLOCKED;
const char *ssid = "MPS";               //"Invitados_UTADEO";
const char *password = "Siemenss71500"; //"ylch0286";
StaticJsonDocument<50000> doc;
String send = "";
int pwm=0;
#define ISensor 32
double Vf = 11.85;
double Vd1 = 3.0;
double Vt = 0.460;
double Vd2 = 0.120;

double p1 = Vf/Vd1;
double p2 = Vt/Vd2;

double R1 = 1200000.0;
double R2 = 1000000.0;
int VTrans = 36;
int VFuent = 39;
double Req = 0;
double VoltajeDif = 0;
double Voltaje = 0;
double VoltajeSensorFuent;
double VoltajeSensorTrans;


WebServer server(80);
void setup() {
 
  Serial.begin(115200);
  pinMode(34, INPUT);
  pinMode(2, OUTPUT);
  Serial.print("Setting AP…");
  // Remove the password parameter, if you want the AP (Access Point) to be open
  WiFi.softAP(ssid, password);
  IPAddress IP = WiFi.softAPIP();
  Serial.print("AP IP address: ");
  Serial.println(IP);
  digitalWrite(5,LOW);
  // Print local IP address and start web server
  server.on("/Data", handle_Data);
  server.on("/Start",StartTimer);
  server.begin();
  adcAttachPin(VTrans);
  adcStart(VTrans);

  adcAttachPin(VFuent);
  adcStart(VFuent);

  adcAttachPin(ISensor);
  adcStart(ISensor);

  analogReadResolution(12);
  analogSetAttenuation(ADC_11db);
  ledcSetup(0, 50000, 16);
  ledcAttachPin(5, 0);
  StartTimer();
}
 
void loop() {
 
  if (interruptCounter > 0) {
 
    portENTER_CRITICAL(&timerMux);
    interruptCounter--;
    portEXIT_CRITICAL(&timerMux);
    pwm+=100;
    ledcWrite(0,pwm);
    totalInterruptCounter++;
    if(totalInterruptCounter>1500){
      StopTimer();
      pwm=0;
      Serial.println("Terminado");
      totalInterruptCounter=0;
    }
    Voltage();
    Serial.print(" PWM: ");
    Serial.print(pwm);
    Serial.print(" V: ");
    Serial.println(ReadVoltage(ISensor),4);
    if(pwm>=65000){
      pwm=0;
    }
  }
}
int randomNumber(){
  return random(1,100);
}

///////////////////////// Timer Triggered ///////////////////////
void IRAM_ATTR OnTimer() {
  portENTER_CRITICAL_ISR(&timerMux);
  interruptCounter++;
  portEXIT_CRITICAL_ISR(&timerMux);
}
void StartTimer(){
  timer = timerBegin(0, 240, true);
  timerAttachInterrupt(timer, &OnTimer, true);
  timerAlarmWrite(timer, 20000, true);
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

double ReadVoltage(byte pin)
{
  double reading = averageAnalogReading(300.0, pin); // Reference voltage is 3v3 so maximum reading is 3v3 = 4095 in range 0 to 4095
  if(reading < 1 || reading > 4095) return 0;
  //return -0.000000000009824 * pow(reading,3) + 0.000000016557283 * pow(reading,2) + 0.000854596860691 * reading + 0.065440348345433;
  return -0.000000000000016 * pow(reading,4) + 0.000000000118171 * pow(reading,3)- 0.000000301211691 * pow(reading,2)+ 0.001109019271794 * reading + 0.034143524634089;
} // Added an improved polynomial, use either, comment out as requi
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
void Voltage(){
  VoltajeSensorFuent = (ReadVoltage(VFuent))* (3.78/4096.00)*10.23391; // 4096.0);//*13*(340206.186/320000);
  VoltajeSensorTrans = (ReadVoltage(VTrans))* 3.3/4096.00;//*13*(340206.186/320000);
  VoltajeDif = (VoltajeSensorFuent) - (VoltajeSensorTrans);
  Serial.print("Voltaje: ");
  Serial.print(VoltajeDif,5);
}