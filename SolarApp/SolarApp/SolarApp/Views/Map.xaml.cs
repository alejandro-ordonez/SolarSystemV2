using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolarApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace SolarApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Map : ContentPage
    {
        public Map()
        {
            BindingContext = new MapViewModel();
            InitializeComponent();
            PanelMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(4.7111562, -74.142132), Distance.FromMiles(10)));
        }

        private async void Pin_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Aviso", "Pin Clicked", "ok");
            var pin = (Pin)sender;
            var x = BindingContext as MapViewModel;
            x.SetCurrentPanel.Execute(pin.Position);
            await Navigation.PopModalAsync();
        }
    }
}