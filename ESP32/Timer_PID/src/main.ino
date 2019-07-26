#include <Arduino.h>
#include <WiFi.h>
#include <ESPAsyncWebServer.h>

const char *ssid = "ESP32-Access-Point";
const char *password = "123456789";
hw_timer_t *timer = NULL;
portMUX_TYPE timerMux = portMUX_INITIALIZER_UNLOCKED;
int totalInterruptCounter;
volatile int interruptCounter = 0;
AsyncWebServer server(80);
#define ISensor 32
double temp = 0;
#define Resolution 4096
//Voltage Source
#define VSourve 3.3

double setpoint = 0;
double pwm = 0;
double error = 0, KP = 200, KD = 10, KI = 10, lastError = 0, sumError = 0;

void setup()
{
  // put your setup code here, to run once:
  Serial.begin(115200);
  adcAttachPin(ISensor);
  adcStart(ISensor);
  ledcSetup(0, 5000, 16);
  ledcAttachPin(5, 0);

  Serial.print("Setting AP (Master Puto)…");
  // Remove the password parameter, if you want the AP (Access Point) to be open
  WiFi.softAP(ssid, password);
  IPAddress IP = WiFi.softAPIP();
  Serial.print("AP IP address: ");
  Serial.println(IP);

  server.on("/", HTTP_GET, [](AsyncWebServerRequest *request) {
    // Handling function
    AsyncWebParameter *p = request->getParam(0);
    setpoint = p->value().toDouble();
    request->send(200, "text/plain", "Holi");
    Serial.println("Timer Iniciado");
    StartTimer();
  });
  server.on("/Stop", HTTP_GET, [](AsyncWebServerRequest *request) {
    // Handling function
    StopTimer();
    request->send(200, "text/plain", "Timer Detenido");
  });
  server.begin();
}

void loop()
{
  // put your main code here, to run repeatedly:
/*
  if (interruptCounter > 0)
  {
    portENTER_CRITICAL(&timerMux);
    interruptCounter--;
    portEXIT_CRITICAL(&timerMux);
    PID();
    //Serial.println("Interrupt");
  }*/


  for (size_t i = 0; i < 65000; i+=300)
  { 
    temp=readISensor();
    print(temp, i, 0);
    ledcWrite(0, i);
    delay(1000);
  }
}

void StartTimer()
{
  timer = timerBegin(0, 240, true);
  timerAttachInterrupt(timer, &OnTimer, true);
  timerAlarmWrite(timer, 10000, true);
  timerAlarmEnable(timer);
}

void IRAM_ATTR OnTimer()
{
  portENTER_CRITICAL_ISR(&timerMux);
  interruptCounter++;
  portEXIT_CRITICAL_ISR(&timerMux);
}

void StopTimer()
{
  if (timer)
  {
    // Stop and free timer
    totalInterruptCounter = 0;
    timerEnd(timer);
  }
}
float readISensor()
{
  float I = ReadVoltage(ISensor);//(averageAnalogReading(400.0, ISensor)) * (VSourve / Resolution);
  //I*=15.336;
  //I-=23.26;
  return I;
}

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

void print(double t, double i, double p)
{
  Serial.print("PWM: ");
  Serial.print(p);
  Serial.print("  Sensor: ");
  Serial.print(temp, 5);
  Serial.print("  Corriente: ");
  Serial.println(i, 4);
}

void PID()
{
  temp = readISensor();
  print(temp, setpoint, pwm);
  error = setpoint - temp;
  pwm += KP * error + KD * (error - lastError) + KI * sumError;
  if (pwm < 0)
    pwm = 0;
  sumError += error;
  lastError = error;
  ledcWrite(0, pwm);
}

double ReadVoltage(byte pin){
  double reading = averageAnalogReading(100.0, pin);//analogRead(pin); // Reference voltage is 3v3 so maximum reading is 3v3 = 4095 in range 0 to 4095
  if(reading < 1 || reading > 4095) return 0;
  //return -0.000000000009824 * pow(reading,3) + 0.000000016557283 * pow(reading,2) + 0.000854596860691 * reading + 0.065440348345433;
  return -0.000000000000016 * pow(reading,4) + 0.000000000118171 * pow(reading,3)- 0.000000301211691 * pow(reading,2)+ 0.001109019271794 * reading + 0.034143524634089;
} // Added an improved polynomial, use either, comment out as requi