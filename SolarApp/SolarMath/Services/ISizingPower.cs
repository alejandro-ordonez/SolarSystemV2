using System;
using System.Collections.Generic;
using System.Text;
using SolarMath.Models;
namespace SolarMath.Services
{
    public interface ISizingPower
    {
        double TotalPowerConsumption(List<Element> Elements, double Eficency=0.9);
        double PowerPeakGenerator(double TotalPower, double HSS);
        double EnergyToProvide(double TotalEnergy,double FS = 1.2);
        int NumberOfModulesInSerie(double Vmodule, double VSystem);
        int NumberOfModulesInParallel(double Vmodule, double Ipmodule, double PowerPeak);
        SolarSystem GetSystem(List<Element> Elements, double Vmodule, double Ipmodule, double VSystem, double HSS, double WidthModule, double HeightModule);


    }
}
