class Solar{
  private:
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
   float getIR(){
       return(Radiaton);
   }
   float getT(){
       return(Temp);
   }
};