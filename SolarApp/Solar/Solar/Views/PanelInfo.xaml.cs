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
        //public Position Pos { get; set; }
        public PanelInfo()
        {
            InitializeComponent();
            //var vm = (Panel)BindingContext;
            //var x = Solar.Helpers.LocationHelper.GetLocation();(x.Latitude, x.Longitude
            PanelMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(4.6071698, -74.0687585), Distance.FromMiles(2)));
        }
    }
}