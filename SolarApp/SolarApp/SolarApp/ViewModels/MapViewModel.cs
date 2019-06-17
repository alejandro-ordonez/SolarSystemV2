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
    public class MapViewModel: ResultViewModel
    {
        public ICommand SetCurrentPanel { get; set; }
        public MapViewModel()
        {
            SetCurrentPanel = new Command<Position>(async(panel) => await SetPanel(panel) );
        }
        private async Task SetPanel(Position panel)
        {
           CurrentPanel=await DataProcess.GetPanel(panel.Longitude, panel.Latitude);
        }

    }
}
