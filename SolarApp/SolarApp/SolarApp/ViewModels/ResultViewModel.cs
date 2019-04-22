using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SolarApp.Models;
using Syncfusion.SfChart.XForms;

namespace SolarApp.ViewModels
{
    public class ResultViewModel:BaseViewModel
    {
        public List<SolarPanel> Panels { get; set; }
        public List<Sensor> Readings { get; set; }
        public SolarPanel CurrentPanel { get; set; }
        public ResultViewModel()
        {
            Panels = new List<SolarPanel>();
            CurrentPanel = new SolarPanel();
            Readings = new List<Sensor>();
            LoadData();
            //Readings = new ObservableCollection<Sensor>();

        }
        private async void LoadData()
        {
            var res =await DataProcess.GetReadingsServer();
            foreach (var item in res)
            {
                Panels.Add(item);
            }
            CurrentPanel = await DataProcess.GetPanel(4.632154, -74.142143);
            Readings = CurrentPanel.Readings;
        }
    }
}
