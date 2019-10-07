using System;
using System.Collections.Generic;
using System.Text;

namespace Solar.Models
{
    public class ESPDataModel
    {
        public double IR { get; set; }
        public double T { get; set; }
        //public DateTime Date { get; set; }
        public double[] V { get; set; }
        public double[] I { get; set; }
    }
}
