using Solar.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Solar.ViewModels
{
    public class GraphViewModel
    {
        public GraphViewModel(Panel P)
        {
            Panel = P;
        }
        public Panel Panel { get; set; }
    }
}
