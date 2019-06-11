using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SolarMath.Models;
using SolarApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SolarApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SizingSystem : ContentPage
    {
        public SizingViewModel ViewModel { get; set; }
        public SizingSystem()
        {
            ViewModel = new SizingViewModel();
            InitializeComponent();
        }

        private async void Btn_Clicked(object sender, EventArgs e)
        {
            SolarSystem system = new SolarSystem();
            system.IPModule = double.Parse(Ip.Text);
            system.VModule = double.Parse(Vm.Text);
            system.VSystem = double.Parse(Vsystem.Text);
            system.HSS = 5; //Update with a service
            system.TotalPowerComsumption = double.Parse(ACPower.Text) + double.Parse(DCPower.Text);
            var x = DimensionsText.Value.ToString();
            system.ModuleDimensions = new Area(double.Parse( x.Substring(0, x.IndexOf('x')).Trim()), double.Parse(x.Substring(x.IndexOf('x')+1).Trim()));
            await DisplayAlert("Hola", system.ModuleDimensions.H.ToString(), "Ok");

            if (ViewModel.CalculateSystemCommand.CanExecute(system))
            {
                ViewModel.CalculateSystemCommand.Execute(system);
            }
        }
    }
}