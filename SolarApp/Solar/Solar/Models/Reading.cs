using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Solar.Models
{
    [Table("Readings")]
    public class Reading
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double I { get; set; }
        public double V { get; set; }

        [ForeignKey(typeof(DataPanel))]
        public int DataPanelId { get; set; }
    }
}
