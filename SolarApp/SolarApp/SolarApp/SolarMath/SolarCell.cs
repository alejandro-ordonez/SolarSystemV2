using System;
using System.Collections.Generic;
using System.Text;

namespace SolarApp.SolarMath
{

    public class SolarCell : ISolar
    {
        private readonly double Kb = 0;
        private readonly double q = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Im"></param>
        /// <param name="Vm"></param>
        /// <param name="Isc"></param>
        /// <param name="Voc"></param>
        /// <returns></returns>
        public double CalculateFF(double Im, double Vm, double Isc, double Voc)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Im"></param>
        /// <param name="Vm"></param>
        /// <param name="Pin"></param>
        /// <returns></returns>
        public double CalculateN(double Im, double Vm, double Pin)
        {
            return (Im*Vm)/Pin;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="T"></param>
        /// <param name="Iph"></param>
        /// <param name="Is"></param>
        /// <returns></returns>
        public double CalculateVoc( double T, double Iph, double Is)
        {
            return ((Kb * T) / q) * Math.Log(1 + (Iph / Is));
        }
    }
}
