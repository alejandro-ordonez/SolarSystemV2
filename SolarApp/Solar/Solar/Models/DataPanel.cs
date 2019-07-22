using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Solar.Models
{
    [Table("DataPanels")]
    public class DataPanel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [OneToMany(CascadeOperations =CascadeOperation.All)]
        public List<Reading> IV { get; set; }
        public DateTime Date { get; set; }
        public double Radiation { get; set; }
        public double Pmax { get; set; }
        public double Vm { get; set; }
        public double Im { get; set; }
        public double Temp { get; set; }

        [ForeignKey(typeof(Panel))]
        public int PanelId { get; set; }


    }
}
