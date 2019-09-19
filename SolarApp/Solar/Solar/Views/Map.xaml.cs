using Solar.Helpers;
using Solar.Models;
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
            PanelMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(4.6071211, -74.0687079), Distance.FromMiles(10)));
        }

        private void Pin_Clicked(object sender, EventArgs e)
        {
            var pin = sender as CustomControls.CustomPin;
            var vm =(ResultViewModel)BindingContext;
            if(vm.ResultCommand.CanExecute(pin.IdPanel))
                vm.ResultCommand.Execute(pin.IdPanel);
            
        }
    }
}