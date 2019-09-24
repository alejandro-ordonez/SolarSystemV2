#include <Arduino.h>
#include <WiFi.h>
#include <ESPAsyncWebServer.h>
#include <ArduinoJson.h>
#include <RTClib.h>

/////////////////////////////ESP32 Variable/////////////////////////////////
#pragma region ESP32_Variables
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
//Pin where PWM is produced
#define PWM 5
//Pin where Pyranometer is connected
#define Pyra 33
// Current where PID should start
double setpoint =0;
// PWM value determined by PID Control
double PWMValue = 0;
//Variable to stablish if timer should be executed or not
volatile byte State = LOW;
// PID Constants
double KP=100, KD=0, KI=50;
// Error constants
double lastError=0, error=0, sumError=0;
#pragma endregion
////////////////////////////////////////////////////////////////////////////

#pragma region WIFI Variables
//Variables to connect to wifi network
const char *ssid = "ESP32-Access-Point";               //"Invitados_UTADEO";
const char *password = "123456789"; //"ylch0286";

//Webserver on port 80
AsyncWebServer server(80);
#pragma endregion

//////////////////////////////Timer Variables///////////////////////////////
// Declare a new timer
hw_timer_t *timer = NULL;
portMUX_TYPE timerMux = portMUX_INITIALIZER_UNLOCKED;
int totalInterruptCounter= 0;
volatile int interruptCounter = 0;
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
#define VS1 39

#define DEBUG 1
////////////////////////////////////////////////////////////////////////////

////////////////////////RTC Clock //////////////////////////////////////////
//The RTC is connected to 21 y 22 pins (SDA, SCL)
RTC_DS3231 Clock;
////////////////////////////////////////////////////////////////////////////

//////////////////////JSON Document/////////////////////////////////////////
#pragma region
//Capacity required to save all the data
const int capacity = 2 * JSON_ARRAY_SIZE(100) + JSON_OBJECT_SIZE(5);
//Jason Document created to save data
StaticJsonDocument<capacity> doc;

JsonArray arr;
JsonArray arr2;
//Variable used to be exposed on rest service
String send = "";
//JasonArray to add values obtained by sensors

#pragma endregion
////////////////////////////////////////////////////////////////////////////

void setup()
{
  // put your setup code here, to run once:
  Serial.begin(115200);
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

//////////////////////////////////Connection to Wifi////////////////////////
#pragma region
 /* Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED)
  {
    Serial.print(".");
    delay(500);
  }*/
 Serial.print("Setting AP…");
  // Remove the password parameter, if you want the AP (Access Point) to be open
  WiFi.softAP(ssid, password);
  IPAddress IP = WiFi.softAPIP();
  Serial.print("AP IP address: ");
  Serial.println(IP);
#pragma endregion
  //////////////////////////////////////////////////////////////////////////

  /////////////////////////////////Server EndPoints/////////////////////////
  //Get Method to obtain measured data
  server.on("/Start", HTTP_GET, [](AsyncWebServerRequest *request) {
    // Handling function
    Start(request);
  });
  server.on("/Stop", HTTP_GET, [](AsyncWebServerRequest *request) {
    // Handling function
    Stop(request);
  });
  server.on("/Data", HTTP_GET, [](AsyncWebServerRequest *request) {
    // Handling function
    Data(request);
  });
  server.begin();
  //////////////////////////////////////////////////////////////////////////
}

void loop()
{
    DetectLoop();
    //TODO: Revisar debounce
    // Probar calibracion de corriente - Listo
    // Probar el contro de corriente on/off - No funciona
    // Probar control de corriente on/off con histeresis - No Funcina
    // Si ninguno de los anteriores, PID - Parcialmente estable
    // Probar diferentes tiempos de muestreo.
    // Multiplos del muestreo.
    // Probar AP -> Request de inicio, preveer fallas de desconexion.
    // Interfaz del led, cuando esta conectado, disponible, no esta funcionando.
    //doxygen 
    // Revisar Pragmas.

  }
  //////////////////////////////////////////////////////////////////////////

#pragma region Methods
///////////////////////////Measure and store data///////////////////////////
void DetectLoop(){
  if (interruptCounter > 0)
  {
    portENTER_CRITICAL(&timerMux);
    interruptCounter--;
    portEXIT_CRITICAL(&timerMux);
    totalInterruptCounter++;
    PID();
  }
}
void Init(){
  double temp;
  lastError=0;
  sumError=0;
  doc.clear();
  //Serialize Basic Data
  JsonObject Panel= doc.createNestedObject("Panel");
  Panel["IR"] = radiation();
  Serial.print("Temp: ");
  temp = CalculateTemp();
  Serial.println(temp);
  Panel["T"] = temp;
  Panel["Time"] = ClockToString(); 
  arr = doc.createNestedArray("V");
  arr2 = doc.createNestedArray("I");
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
  return VoltageF;
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
void print(double t, double i, double p)
{
  Serial.print("PWM: ");
  Serial.print(p);
  Serial.print("  Sensor: ");
  Serial.print(t, 5);
  Serial.print("  Corriente: ");
  Serial.println(i, 4);
}
void imprimir(String g){
  #ifdef DEBUG
  Serial.println(g);
  #endif
}
double ReadVoltage(byte pin)
{
  double reading = averageAnalogReading(100.0, pin);//analogRead(pin); // Reference voltage is 3v3 so maximum reading is 3v3 = 4095 in range 0 to 4095
  if(reading < 1 || reading > 4095) return 0;
  //return -0.000000000009824 * pow(reading,3) + 0.000000016557283 * pow(reading,2) + 0.000854596860691 * reading + 0.065440348345433;
  return -0.000000000000016 * pow(reading,4) + 0.000000000118171 * pow(reading,3)- 0.000000301211691 * pow(reading,2)+ 0.001109019271794 * reading + 0.034143524634089;
} // Added an improved polynomial, use either, comment out as requi
#pragma endregion
////////////////////////////////////////////////////////////////////////////

///////////////////////////Radiation////////////////////////////////////////
double radiation()
{
  double cal = 0;

  cal = (averageAnalogReading(200.0, Pyra) * (2.0 / 2048.0));
  cal /= 27.5;
  Serial.println(cal, 5);
  cal *= 1000000;
  cal /= 61.5;
  return cal;
}
////////////////////Methods to handle http request//////////////////////////

void Test(AsyncWebServerRequest *request)
{
  request->send(200, "text/plain", "Timer Detenido");
}
void Start(AsyncWebServerRequest *request)
{
  AsyncWebParameter *p = request->getParam(0);
  setpoint = p->value().toDouble();
  request->send(200, "text/plain", "Holi");
  Serial.println("Timer Iniciado");
  Init();
  StartTimer(); 
}
void Stop(AsyncWebServerRequest *request){
    StopTimer(); 
}
void Data(AsyncWebServerRequest *request){
  request -> send(200, "application/json", send);
  doc.clear();
  send="";
}
////////////////////////////////////////////////////////////////////////////

////////////////////////////Timer Methods///////////////////////////////////
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
    interruptCounter=0;
    serializeJson(doc, send);
    timerEnd(timer);
    timer = NULL;
  }
}
/////////////////////////////////////////////////////////////////////////////

//////////////////////////////PID////////////////////////////////////////////
void PID()
{
  double temp = readISensor();
  print(temp, setpoint, PWMValue);
  error = setpoint - temp;
  PWMValue += KP * error + KD * (error - lastError) + KI * sumError;
  if (PWMValue < 0)
    PWMValue = 0;
  sumError += error;
  lastError = error;
  ledcWrite(0, PWMValue); 
  setpoint-=0.01;
  if(setpoint<=0){
    StopTimer();
  }
  arr.add(temp);
  arr2.add(readVSensor());
}
////////////////////////////////////////////////////////////////////////////


#pragma endregion