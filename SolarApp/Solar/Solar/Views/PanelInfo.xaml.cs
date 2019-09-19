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
            var x =(Panel)BindingContext;
            //var x = Solar.Helpers.LocationHelper.GetLocation();
            PanelMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(x.Latitude, x.Longitude), Distance.FromMiles(2)));
        }
    }
}