using System;
using System.Collections.Generic;
using System.Text;

namespace Solar.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhotoPath { get; set; }
        public List<Panel> Panels { get; set; } = new List<Panel>();
    }
}
