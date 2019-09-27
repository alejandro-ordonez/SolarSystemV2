using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Solar.Models
{
    public class Reading
    {
        public int Id { get; set; }
        public double I { get; set; }
        public double V { get; set; }

        [NotMapped]
        public double Power { get { return I * V; } }


        public int DataPanelId { get; set; }
        public DataPanel DataPanel { get; set; }
    }
}
