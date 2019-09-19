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
        public ResultViewModel viewModel { get; set; }
        public Map()
        {
            BindingContext = viewModel=Startup.ServiceProvider.GetService<ResultViewModel>();
            InitializeComponent();
            PanelMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(4.6071211, -74.0687079), Distance.FromMiles(10)));
        }

        private void Pin_Clicked(object sender, EventArgs e)
        {
            var pin = (CustomControls.CustomPin)sender;
            //DisplayAlert("Whatever", pin.IdPanel.ToString(), "Ok");
            if(viewModel.ResultCommand.CanExecute(pin.IdPanel))
                viewModel.ResultCommand.Execute(pin.IdPanel);
            
        }
    }
}