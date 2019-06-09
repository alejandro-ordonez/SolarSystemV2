using System;
using System.Collections.Generic;
using System.Text;

namespace SolarMath.Models
{
    public class Element
    {
        /// <summary>
        /// Name of the device
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Nominal Power that the device consumes
        /// </summary>
        public float Power { get; set; }
        /// <summary>
        /// Hours a day where it is used
        /// </summary>
        public int HoursADay { get; set; }
    }
}
