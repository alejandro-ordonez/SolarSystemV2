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
        private SolarPanel currentPanel;
        public SolarPanel CurrentPanel
        {
            get { return currentPanel; }
            set
            {
                currentPanel = value;
                OnPropertyChanged("CurrentPanel");
            }
        }
        public ObservableCollection<SolarPanel> Panels { get; set; }
        public ResultViewModel()
        {
            Panels = new ObservableCollection<SolarPanel>();
            LoadPositions();
        }
        private async void LoadPositions()
        {
            var items = await DataProcess.GetReadingsServer();
            foreach (var item in items)
            {
                Panels.Add(item);
            }
        }
    }
}
