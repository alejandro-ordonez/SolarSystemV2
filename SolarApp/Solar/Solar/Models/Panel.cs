using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Solar.Models
{
    public class Panel
    {
        public List<Reading> IV { get; set; }
        public DateTime Date { get; set; }
        public double Radiation { get; set; }
        public double NominalV { get; set; }
        public double Power { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Vm { get; set; }
        public double Im { get; set; }
        public double Temp { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }
}
