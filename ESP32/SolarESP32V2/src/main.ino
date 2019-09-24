//////////////////////////////////////// Libraries used /////////////////////////////////////////
#pragma region Libraries

#include <Arduino.h>
#include <WiFi.h>
#include <ESPAsyncWebServer.h>
#include <ArduinoJson.h>
#include <AsyncJson.h>
#include <RTClib.h>
#include <digcomp.h>
//#include <AsyncJson.h>

#pragma endregion
/////////////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////// Global Variables ///////////////////////////////////////
#pragma region Variables

////////////////////////////// ESP32 Variables //////////////////////////////
#pragma region ESP32 Variales

/////////////// RC Filters /////////////
// This constants should be fixed to an specific dt and fc

float b[] = {0.0304590279514212, 0.0304590279514212};
float a[] = {1.000000000000000, -0.939081944097158};
float lp_in[2];
float lp_out[2];
float lp_in1[2];
float lp_out1[2];

dig_comp filter(b, a, lp_in, lp_out, 2, 2);
dig_comp filter2(b, a, lp_in1, lp_out1, 2, 2);
////////////////////////////////////////

////////////////INPUTS//////////////////
#pragma region

// Pin where Pyranometer is connected
#define Pyra 33
// ADC Pin to read Thermistor
#define Thermistor 35
// ADC Pin where I Sensor is connected
#define ISensor 32
// ADC Pin where Voltage over the panel is measured
#define VS1 39

#pragma endregion
///////////////////////////////////////

///////////////OUTPUTS/////////////////
//Led Indicator
#define Indicator 2
//Pin where PWM is produced
#define PWMPin 5

///////////////////////////////////////

//////////////Constants////////////////
//Resolution: 11 Bit linear zone
#define Resolution 4096
//Voltage Source
#define VSourve 3.3
// Max possible PWM value
#define MaxValuePWM 65535
// It indicates which is the state of the microcontroller if false it means is not
// doing anything, if true it means microcontroller is taking measures.
bool isBusy = false;
// Number Of Samples
int numberOfSamples = 1000;
///////////////////////////////////////

/////////////PID///////////////////////
#pragma region
/**
 * @brief PID control is based on a voltage control
 * It should produce a linear output, that increase according
 * to VOC
 */
// Voltage in Open Circuit
float VoC = 0;
// Current where PID should start
double setpoint = 0;
// PWM value determined by PID Control
double PWMValue = 0;
//Variable to stablish if timer should be executed or not
volatile byte State = LOW;
// PID Constants
double KP = 100, KD = 0, KI = 50;
// Error constants
double lastError = 0, error = 0, sumError = 0;
// Increase rate linear function.
float increase = 0;
// Time

#pragma endregion
///////////////////////////////////////

#pragma endregion
/////////////////////////////////////////////////////////////////////////////

/////////////////////////////// Wfif Variables /////////////////////////////
#pragma region WIFI Variables

//Variables to connect to wifi network
const char *ssid = "ESP32-Access-Point";
const char *password = "123456789";
//Webserver on port 80
AsyncWebServer server(80);

#pragma endregion
////////////////////////////////////////////////////////////////////////////

////////////////////////////// Timer Variables ////////////////////////////
#pragma region Timer Variables

// Declare a new timer
hw_timer_t *timer = NULL;
portMUX_TYPE timerMux = portMUX_INITIALIZER_UNLOCKED;
int totalInterruptCounter = 0;
volatile int interruptCounter = 0;

#pragma endregion
///////////////////////////////////////////////////////////////////////////

///////////////////////////// Thermistor Constants ////////////////////////
#pragma region Variables

// R2 in voltage divider
#define Resbase 10000 //10Kohm R2
// Resistance obtained at 16.7 °C
#define RTNOM 12226
// Temperature in test conditions
#define NOMINAL_TEMPERATURE 16.7
// Beta value between 3000-4000
#define B 3950

#pragma endregion
///////////////////////////////////////////////////////////////////////////

/////////////////////////////////// RTC  //////////////////////////////////

//The RTC is connected to 21 y 22 pins (SDA, SCL)
RTC_DS3231 Clock;

///////////////////////////////////////////////////////////////////////////

////////////////////////////// JSON Document //////////////////////////////
#pragma region

//Capacity required to save all the data
const int capacity = 2 * JSON_ARRAY_SIZE(100) + JSON_OBJECT_SIZE(5);
//Jason Document created to save data
DynamicJsonDocument doc(2000);
// Array to save all currents measured
JsonArray IArray;
// Array to save all voltages measured
JsonArray VArray;
//Variable used to be exposed on rest service
String send = "";

#pragma endregion
///////////////////////////////////////////////////////////////////////////

#pragma endregion
/////////////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////// Arduino Methods ////////////////////////////////////////
#pragma region Arduino Methods

void setup()
{
  // put your setup code here, to run once:
  // Start Serial comunicatino at 115200 bauds
  Serial.begin(115200);

  ///////////// Set adc Inputs...
  adcAttachPin(Thermistor);
  adcStart(Thermistor);

  //CurrentSensor
  adcAttachPin(ISensor);
  adcStart(ISensor);

  //ADC Voltage
  adcAttachPin(VS1);
  adcStart(VS1);

  //Set ADC properties
  analogReadResolution(12);       // Default of 12 is not very linear. Recommended to use 10 or 11 depending on needed resolution.
  analogSetAttenuation(ADC_11db); // Default is 11db which is very noisy. Recommended to use 2.5 or 6.

  ////////////

  //////////// Digital Inputs and Outputs
  //Set led indicator as an output
  pinMode(Indicator, OUTPUT);
  ///////////

  ////////// Set PWM properties
  ledcSetup(0, 5000, 16);
  ledcAttachPin(PWMPin, 0);
/////////

//////////// Wifi Set Up //////////////////
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
  //////////////////////////////////////////

  if (!Clock.begin())
  {
    Serial.println(F("Couldn't find RTC"));
    while (1)
      ;
  }

//////////// Server Set up //////////////
#pragma region

  //Get Method to obtain measured data
  server.on("/Start", HTTP_GET, [](AsyncWebServerRequest *request) {
    // Handling function
    Start(request);
  });
  server.on("/TestServer", HTTP_GET, [](AsyncWebServerRequest *request) {
    // Handling function
    Test(request);
  });

  server.on("/Data", HTTP_GET, [](AsyncWebServerRequest *request) {
    // Handling function
    Data(request);
  });
  server.on("/SetTime", HTTP_GET, [](AsyncWebServerRequest *request) {
    SetTime(request);
  });

  server.begin();

#pragma endregion
  ///////////////////////////////////////////
}

void loop()
{
  // put your main code here, to run repeatedly:
  if (interruptCounter > 0)
  {
    portENTER_CRITICAL(&timerMux);
    interruptCounter--;
    portEXIT_CRITICAL(&timerMux);
    totalInterruptCounter++;
    PID();
  }
}

#pragma endregion
/////////////////////////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////// Methods ///////////////////////////////////////////

#pragma region

//////////// Methods to handle http request ////////////////////
#pragma region Http request

/**
 * @brief This method is useful for testing web server, it just return
 * a simple plain text
 * @param request this is a param that helps to manage the request
 */
void Test(AsyncWebServerRequest *request)
{
  request->send(200, "text/plain", "Timer Detenido");
}

/**
 * @brief This method starts the service passing some params through the URL
 * It needs the VoC to determinate the step of the PID, after it send back a
 * response indicating that PID has started and it's measuring data.
 * 
 * https://IP/Start/?Voc=30
 * 
 * @param request 
 */
void Start(AsyncWebServerRequest *request)
{
  doc.clear();
  JsonObject Panel= doc.createNestedObject("Panel");
  Panel["IR"] =radiation();
  Panel["T"] = CalculateTemp();
  Panel["Time"] = Clock.now().timestamp();
  VArray = doc.createNestedArray("V");
  IArray = doc.createNestedArray("I");
  AsyncWebParameter *p = request->getParam(0);
  VoC = p->value().toDouble();
  increase = CalculateStep();
  StartTimer();
  Serial.println("Timer Iniciado");
  request->send(200, "text/plain", "Service Started");
}

/**
 * @brief This method let us get the data that device has measured.
 * If it hasn't finished, it would response with and advice. It can be queried
 * anytime to check if the data is available, once Start endpoint has been
 * requested, this data would be automatically deleted.
 * 
 * https://IP/Data
 * 
 * @param request param to handle the request. 
 */
void Data(AsyncWebServerRequest *request)
{
  if (!isBusy)
  {

    AsyncResponseStream *response = request->beginResponseStream("application/json");
    
    serializeJson(doc, *response);
    request->send(response);
  }
  else
  {
    request->send(102, "text/plain", "The device still busy");
  }
}

/**
 * @brief Set the Time object through an endpoint
 * That serie of params are extracted and passed to
 * an DateTime Object which would be used to update
 * current Time on RTC module
 * 
 * http://192.168.4.1/SetTime?year=2019&month=9&day=16&hour=19&minutes=12&seconds=40
 * 
 * @param request this param handle and extract all
 * params over URL.
 */
void SetTime(AsyncWebServerRequest *request)
{
  AsyncWebParameter *Params;
  int dateInfo[6];
  int temp = 0;
  for (size_t i = 0; i < 6; i++)
  {
    /* code */
    Params = request->getParam(i);
    temp = Params->value().toInt();
    dateInfo[i] = temp;
  }
  Clock.adjust(DateTime(dateInfo[0], dateInfo[1], dateInfo[2], dateInfo[3], dateInfo[4], dateInfo[5]));
  request->send(200, "text/plain", GetCurrentTime());
}

#pragma endregion
////////////////////////////////////////////////////////////////

////////////////////// PID  Methods ///////////////////////////
#pragma region

/**
 * @brief 
 * 
 */
void PID()
{
  double temp = readVSensor();
  print(temp, setpoint, PWMValue);
  error = setpoint - temp;
  PWMValue += KP * error + KD * (error - lastError) + KI * sumError;
  if (PWMValue < 0)
    PWMValue = 0;
  sumError += error;
  lastError = error;
  ledcWrite(0, PWMValue);
  if (totalInterruptCounter >= 20)
  {
    IArray.add(readISensor());
    VArray.add(temp);
    setpoint += increase;
    if (setpoint >= VoC)
    {
      setpoint = 0;
      digitalWrite(Indicator, HIGH);
    }
  }
}

/**
 * @brief 
 * 
 */
double CalculateStep()
{
  return VoC / numberOfSamples;
}

#pragma endregion
///////////////////////////////////////////////////////////////

///////////////////////// TIMER ///////////////////////////////
#pragma region

/**
 * @brief 
 * 
 */
void StartTimer()
{
  timer = timerBegin(0, 240, true);
  timerAttachInterrupt(timer, &OnTimer, true);
  //TODO: Update scale
  timerAlarmWrite(timer, 10, true);
  timerAlarmEnable(timer);
}

/**
 * @brief 
 * 
 */
void IRAM_ATTR OnTimer()
{
  portENTER_CRITICAL_ISR(&timerMux);
  interruptCounter++;
  portEXIT_CRITICAL_ISR(&timerMux);
}

/**
 * @brief 
 * 
 */
void StopTimer()
{
  if (timer)
  {
    // Stop and free timer
    totalInterruptCounter = 0;
    interruptCounter = 0;
    serializeJson(doc, send);
    timerEnd(timer);
    timer = NULL;
  }
}

#pragma endregion
///////////////////////////////////////////////////////////////

//////////////////////// Current //////////////////////////////

/**
 * @brief This method should be fixed
 * 
 * @return float returns the value in Amps
 */
float readISensor()
{
  float I = 0;
  I = (averageAnalogReading(1000, ISensor) - 1422.520075) * (VSourve / Resolution);
  return I;
}
///////////////////////////////////////////////////////////////

////////////////////////// Voltage ////////////////////////////

/**
 * @brief 
 * 
 * @return float 
 */
float readVSensor()
{
  float VoltageF = (averageAnalogReading(600.0, VS1) / Resolution) * 13 * (340206.186 / 320000);
  return VoltageF;
}

///////////////////////////////////////////////////////////////


////////////////////////Temperature///////////////////////////
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

///////////////////////////////////////////////////////////////

//////////////////////////Radiation////////////////////////////
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
///////////////////////////////////////////////////////////////

//////////////////////////Clock////////////////////////////////
String GetCurrentTime(){
  DateTime TimeNow = Clock.now();
  String res = "";
  res += TimeNow.year();
  res += "/";
  res += TimeNow.month();
  res += "/";
  res += TimeNow.day();
  res += "    ";
  res += TimeNow.hour();
  res += "-";
  res += TimeNow.minute();
  res += "-";
  res += TimeNow.second();
  return res;
}
/////////////////// Generic Methods ///////////////////////////
#pragma region

/**
 * @brief This methods obtain an average of a given number of samples
 * 
 * @param samples Number of samples that code should take and divide
 * @param analogPin The pin number in which 
 * @return double 
 */
double averageAnalogReading(double samples, int analogPin)
{
  double avg = 0;
  for (size_t i = 0; i < samples; i++)
  {
    avg += analogRead(analogPin);
  }
  return avg / samples;
}

/**
 * @brief Method to print some data.
 * The next could be any other parameter but in this code is used with the next ones
 * @param t Analog value given by Current Sensor
 * @param i Current value obtained by the formula
 * @param p PWM Value defined by PID
 */
void print(double t, double i, double p)
{
  Serial.print("PWM: ");
  Serial.print(p);
  Serial.print("  Sensor: ");
  Serial.print(t, 5);
  Serial.print("  Corriente: ");
  Serial.println(i, 4);
}
void imprimir(String g)
{
#ifdef DEBUG
  Serial.println(g);
#endif
}

/**
 * @brief Method to convert analog value to voltage
 * 
 * This method was taken from a repository it suggest a good approach to the real value
 * due to the fact that ESP32 is not very linear in some sections
 * 
 * @param pin Pin number to read analog value
 * @return double real voltage value
 */
double ReadVoltage(byte pin)
{
  double reading = averageAnalogReading(100.0, pin); //analogRead(pin); // Reference voltage is 3v3 so maximum reading is 3v3 = 4095 in range 0 to 4095
  if (reading < 1 || reading > 4095)
    return 0;
  //return -0.000000000009824 * pow(reading,3) + 0.000000016557283 * pow(reading,2) + 0.000854596860691 * reading + 0.065440348345433;
  return -0.000000000000016 * pow(reading, 4) + 0.000000000118171 * pow(reading, 3) - 0.000000301211691 * pow(reading, 2) + 0.001109019271794 * reading + 0.034143524634089;
} // Added an improved polynomial, use either, comment out as requi

#pragma endregion
///////////////////////////////////////////////////////////////

#pragma endregion
////////////////////////////////////////////////////////////////////////////////////////////////