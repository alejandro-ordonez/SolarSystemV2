using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolarApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SolarApp.Models;
namespace SolarApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Curves : ContentPage
    {
        private ResultViewModel ViewModel;
        public Curves()
        {
            BindingContext = ViewModel = new ResultViewModel();
            InitializeComponent();
        }

        private async void SetPosition_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Map());
        }
    }
}