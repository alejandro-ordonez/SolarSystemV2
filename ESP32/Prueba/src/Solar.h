#include "ListLib.h"

class Solar{
  private:
   List<double> Current;
   List<double> Voltage;
   double Radiaton;
   double Temp;
   public:
   Solar(){
       Temp =0;
       Radiaton =0;
   }
   void setRadiation(double r){
       Radiaton = r;
   }
   void setTemp(double t){
       Temp=t;
   }
   List<double> getCurrent(){       
       return(Current);
   }
   List<double> getVoltage(){
       return(Voltage);
   }
   int getSizeCurrent(){
       return(Current.Capacity());
   }
   double getIR(){
       return(Radiaton);
   }
   double getT(){
       return(Temp);
   }
   void addCurrentAndVoltage(double I, double V){
       Current.Add(I);
       Voltage.Add(V);
   }
};