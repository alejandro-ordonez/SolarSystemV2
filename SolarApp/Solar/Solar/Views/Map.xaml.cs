using Solar.ViewModels;
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
    public partial class Map : ContentPage
    {
        public Map()
        {
            BindingContext = Startup.ServiceProvider.GetService<ResultViewModel>();
            InitializeComponent();
            PanelMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(4.7111562, -74.142132), Distance.FromMiles(10)));
        }
    }
}