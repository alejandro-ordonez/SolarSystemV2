using System;
using System.Collections.Generic;
using System.Text;
using SolarMath.Models;

namespace SolarMath.Services
{
    public class SizingSystem : ISizingPower
    {
        private SolarSystem Solar { get; set; }
        public double EnergyToProvide(double TotalEnergy,double FS = 1.2)
        {
            return TotalEnergy*FS;
        }
        public SolarSystem GetSystem(List<Element> Elements, double Vmodule, double Ipmodule, double VSystem, double HSS, double WidthModule, double HeightModule)
        {
            Solar = new SolarSystem();
            Solar.ModuleDimensions = new Area(WidthModule, HeightModule);
            Solar.VModule = Vmodule;
            Solar.IPModule = Ipmodule;
            Solar.VSystem = VSystem;
            Solar.TotalPowerComsumption= EnergyToProvide(TotalPowerConsumption(Elements));
            Solar.NSerieModules = NumberOfModulesInSerie(Vmodule, VSystem);
            Solar.NParallelModules = NumberOfModulesInParallel(Vmodule, Ipmodule, PowerPeakGenerator(Solar.TotalPowerComsumption, HSS));
            Solar.SolarSystemDimensions = new Area(Solar.NSerieModules * Solar.ModuleDimensions.H, Solar.NParallelModules * Solar.ModuleDimensions.W);
            return Solar;
        }

        public int NumberOfModulesInParallel(double Vmodule, double Ipmodule, double PowerPeak)
        {  
            return (int)Math.Round((PowerPeak)/(Vmodule*Ipmodule));
        }
        public int NumberOfModulesInSerie(double Vmodule, double VSystem)
        {
            return (int)Math.Round(VSystem/Vmodule);   
        }
        public double PowerPeakGenerator(double TotalPower, double HSS)
        {
            return TotalPower/HSS;
        }

        public double TotalPowerConsumption(List<Element> Elements, double Eficency=0.9)
        {
            double res = 0;
            foreach (var item in Elements)
            {
                res += item.Power * item.HoursADay;
            }
            return res/Eficency;
        }
    }
}
