using Solar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Solar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PanelInfo : ContentView
    {
        public PanelInfo()
        {
            InitializeComponent();
            
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var vm = (Panel)BindingContext;
            PanelMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(vm.Latitude, vm.Longitude), Distance.FromMiles(1)));
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(vm.Latitude, vm.Longitude),
                Label = "custom pin",
                Address = "custom detail info"
            };

            PanelMap.Pins.Add(pin);
        }
    }
}