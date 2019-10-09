using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using System.ComponentModel.DataAnnotations.Schema;
using Xamarin.Forms.Maps;

namespace Solar.Models
{

    public class Panel
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Voc { get; set; }
        public double Isc { get; set; }
        public double Power { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        //public Location Location { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }

        public List<DataPanel> Data { get; set; } = new List<DataPanel>();

        public int UserId { get; set; }
        public User User { get; set; }


        [NotMapped]
        public Position Location { get; set; }
    }
}
