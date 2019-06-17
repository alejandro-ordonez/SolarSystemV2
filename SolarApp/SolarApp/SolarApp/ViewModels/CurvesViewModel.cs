using SolarApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SolarApp.ViewModels
{
    public class CurvesViewModel:ResultViewModel
    {
        public CurvesViewModel()
        {
            SetPanelOnMapCommand = new Command(async () => await SetPanelOnMap());
        }
        private async Task SetPanelOnMap()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new Map());
        }
        public Command SetPanelOnMapCommand { get;}
        //TODO: Complete functions on view side
    }
}
