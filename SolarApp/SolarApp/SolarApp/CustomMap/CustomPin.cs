using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace SolarApp.CustomMap
{
    public class CustomPin:Pin
    {
        public string Url { get; set; }
        public string Description { get; set; }
        public ImageSource Photo { get; set; }
    }
}
