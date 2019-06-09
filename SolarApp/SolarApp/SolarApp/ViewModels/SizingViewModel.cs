using System;
using System.Collections.Generic;
using System.Text;
using SolarMath.Models;
using SolarMath.Services;

namespace SolarApp.ViewModels
{
    public class SizingViewModel:BaseViewModel
    {
        private SizingSystem Sizing;
        public SizingViewModel()
        {
            Sizing = new SizingSystem();
            System = new SolarSystem();
        }
        public SolarSystem System { get; set; }

    }
}
