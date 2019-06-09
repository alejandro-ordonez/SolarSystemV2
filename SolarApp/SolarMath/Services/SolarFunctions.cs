using System;
using System.Collections.Generic;
using System.Text;

namespace SolarMath.Services
{

    public class SolarFunctions : ISolar
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

        public double CalculateN(double PMax, double Pin)
        {
            throw new NotImplementedException();
        }

        public double CalculatePIn(double A, double IR)
        {
            throw new NotImplementedException();
        }

        public double CalculatePMax()
        {
            throw new NotImplementedException();
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

        public void SetPoints(double V, double I)
        {
            throw new NotImplementedException();
        }
    }
}
