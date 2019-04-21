using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SolarApp.Models;
using Syncfusion.SfChart.XForms;

namespace SolarApp.ViewModels
{
    public class CurvesViewModel:BaseViewModel
    {
        public ObservableCollection<Sensor> Readings { get; set; }
        public CurvesViewModel()
        {
            //Readings = new ObservableCollection<Sensor>();
            Readings = new ObservableCollection<Sensor>{
                new Sensor { Date= DateTime.Now, ValueV=0.1, ValueI=10, ValueIR=4.5, ValueTemp=25},
                new Sensor { Date= DateTime.Now, ValueV=0.5, ValueI=9.98, ValueIR=4.5, ValueTemp=25},
                new Sensor { Date= DateTime.Now, ValueV=2.0, ValueI=9.95, ValueIR=4.5, ValueTemp=25},
                new Sensor { Date= DateTime.Now, ValueV=3.0, ValueI=9.93, ValueIR=4.5, ValueTemp=25},
                new Sensor { Date= DateTime.Now, ValueV=4.0, ValueI=9.91, ValueIR=4.5, ValueTemp=25},
                new Sensor { Date= DateTime.Now, ValueV=5.0, ValueI=8.8, ValueIR=4.5, ValueTemp=25},
                new Sensor { Date= DateTime.Now, ValueV=6.0, ValueI=7.8, ValueIR=4.5, ValueTemp=25},
                new Sensor { Date= DateTime.Now, ValueV=7.0, ValueI=6, ValueIR=4.5, ValueTemp=25},
                new Sensor { Date= DateTime.Now, ValueV=8.0, ValueI=0.1, ValueIR=4.5, ValueTemp=25},
            };
        }
    }
}
