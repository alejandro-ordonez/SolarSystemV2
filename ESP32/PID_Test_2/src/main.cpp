#include <Arduino.h>

int data[1000];

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  ledcSetup(0, 5000, 16);
  ledcAttachPin(5, 0);
  ledcWrite(0,65535);
  for (size_t i = 0; i < 1000; i++)
  {
    /* code */
    data[i]=analogRead(39);
    delayMicroseconds(10);
  }
  for (size_t i = 0; i < 1000; i++)
  {
    /* code */
    Serial.println(data[i]);
  }
  
  
}

void loop() {
  // put your main code here, to run repeatedly:
}