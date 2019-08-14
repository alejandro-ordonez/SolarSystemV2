#include <Arduino.h>


void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  //pinMode(D2, OUTPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
  for (int i = 0; i < 1023; i++)
  {
    analogWrite(D2, i);
    Serial.println(i);
    delay(100);
}
  /*digitalWrite(D2, HIGH);
  delay(5000);
  digitalWrite(D2, LOW);
  delay(5000);*/
}