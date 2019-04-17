using System;
using System.Collections.Generic;
using System.Text;

namespace SolarApp.Models
{
    /// <summary>
    /// This class maps the data related to a solar panel.
    /// </summary>
    public class Sensor
    {
        /// <summary>
        /// Voltage extracted from sensor
        /// </summary>
        public double ValueV { get; set; }
        /// <summary>
        /// Current sensed with a connected panel
        /// </summary>
        public double ValueI { get; set; }
        /// <summary>
        /// Temp obtained by a thermistor
        /// </summary>
        public double ValueTemp { get; set; }
        /// <summary>
        /// Irradiance measured by piranometer.
        /// </summary>
        public double ValueIR { get; set; }
        /// <summary>
        /// Date register by ds1307
        /// </summary>
        public DateTime Date { get; set; }
    }
}
