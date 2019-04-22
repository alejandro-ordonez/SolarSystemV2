using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using SolarApp.Models;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace SolarApp.ViewModels
{
    public class MapViewModel:BaseViewModel
    {
        public List<SolarPanel> Panels  { get; set; }
        public ICommand SetCurrentPanel { get; set; }
        public MapViewModel()
        {
            Panels = new List<SolarPanel>();
            LoadPositions();
            SetCurrentPanel = new Command<Position>(async(panel) => await SetPanel(panel) );
        }
        private async Task SetPanel(Position panel)
        {
           await DataProcess.SetCurrentPanel(await DataProcess.GetPanel(panel.Longitude, panel.Latitude));
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
