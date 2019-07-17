using Solar.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Solar.ViewModels
{
    public class ResultViewModel
    {
        public ObservableCollection<Panel> Panels { get; set; }
    }
}
