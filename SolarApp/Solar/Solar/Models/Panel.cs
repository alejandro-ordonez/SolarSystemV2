using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Solar.Models
{
    [Table("Panels")]
    public class Panel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Reference { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double NominalV { get; set; }
        public double NominalI { get; set; }
        public double Power { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }
        [OneToMany]
        public List<DataPanel> Data { get; set; }
    }
}
