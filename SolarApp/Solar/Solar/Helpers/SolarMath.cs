using System;
using System.Collections.Generic;
using System.Text;

namespace Solar.Helpers
{
    public static class SolarMath
    {
        /// <summary>
        /// This method calculates the power of the panel
        /// </summary>
        /// <param name="Width">The width of the panel in meters</param>
        /// <param name="Height">The height of the panel in meters</param>
        /// <param name="IR">The current irradition in W/m^2</param>
        /// <returns>Returns the value in W</returns>
        public static double CalculatePowerPanel(double Width, double Height, double IR) => Width * Height * IR;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Im"></param>
        /// <param name="Vm"></param>
        /// <param name="Isc"></param>
        /// <param name="Voc"></param>
        /// <returns></returns>
        public static double CalculateFF(double Im, double Vm, double Isc, double Voc) => (Im * Vm) / (Isc * Voc);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Im"></param>
        /// <param name="Vm"></param>
        /// <param name="Pin"></param>
        /// <returns></returns>
        public static double CalculateEficiency(double Im, double Vm, double Pin) => ((Im * Vm) / Pin)*100;
    }
}
