#include "ListLib.h"

class Solar{
  private:
   List<float> Current;
   List<float> Voltage;
   float Radiaton;
   float Temp;
   public:
   Solar(){
       Temp =0;
       Radiaton =0;
   }
   void setRadiation(float r){
       Radiaton = r;
   }
   void setTemp(float t){
       Temp=t;
   }
   void resetList(){
       Voltage= List<float>();
       Current= List<float>();
   }
   List<float> getCurrent(){       
       return(Current);
   }
   List<float> getVoltage(){
       return(Voltage);
   }
   float getVoltageinPos(int i){
       return(Voltage[i]);
   }
   float getCurrentinPos(int i){
       return(Current[i]);
   }
   int getSizeCurrent(){
       return(Current.Capacity());
   }
   float getIR(){
       return(Radiaton);
   }
   float getT(){
       return(Temp);
   }
   void addCurrentAndVoltage(float I, float V){
       Current.Add(I);
       Voltage.Add(V);
   }
};