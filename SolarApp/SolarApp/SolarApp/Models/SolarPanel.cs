using System;
using System.Collections.Generic;
using System.Text;

namespace SolarApp.Models
{
    public class SolarPanel
    {
        public double IRadainceNASA { get; set; }
        /// <summary>
        /// Boltsman Constant 
        /// </summary>
        public double K { get; set; }
        /// <summary>
        /// Description asociated to the panel
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Data collected from the panel
        /// </summary>
        public List<Sensor> Readings { get; set; }
        /// <summary>
        /// Width of the panel
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Height of the panel
        /// </summary>
        public double  Height { get; set; }
    }
}
