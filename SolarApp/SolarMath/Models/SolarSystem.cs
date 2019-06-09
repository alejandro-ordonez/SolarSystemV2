using System;
using System.Collections.Generic;
using System.Text;

namespace SolarMath.Models
{
    public class SolarSystem
    {
        public double VModule { get; set; }
        public double IPModule { get; set; }
        public int NSerieModules { get; set; }
        public int NParallelModules { get; set; }
        public Area ModuleDimensions { get; set; }
        public Area SolarSystemDimensions { get; set; }
        public double VSystem { get; set; }
        public double IPSystem { get; set; }
        public double TotalPowerComsumption { get; set; }
    }
    public class Area
    {
        public Area(double Width, double Height)
        {
            W = Width;
            H = Height;
        }
        public double W { get; private set; }
        public double H { get; private set; }
    }
}
