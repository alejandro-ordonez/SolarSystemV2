using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace SolarApp.Models
{
    public class SolarPanel
    {

        public Position  Location { get; set; }
        public double IRadainceNASA { get; set; }
        /// <summary>
        /// Boltsman Constant 
        /// </summary>
        private double K { get => 5; }
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
