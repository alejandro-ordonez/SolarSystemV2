using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Solar.Models
{
    public class DataPanel
    {
        public int Id { get; set; }
        public List<Reading> IV { get; set; }
        public DateTime Date { get; set; }
        public double Radiation { get; set; }
        public double Pmax { get; set; }
        public double Vm { get; set; }
        public double Im { get; set; }
        public double Temp { get; set; }

        public int PanelId { get; set; }
        public Panel Panel { get; set; }
    }
}
