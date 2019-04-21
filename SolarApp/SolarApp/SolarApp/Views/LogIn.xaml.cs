using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SolarApp.ViewModels;

namespace SolarApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogIn : ContentPage
    {
        public LogIn()
        {
            BindingContext = new LogInViewModel();
            InitializeComponent();
        }

        private async void Log_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}