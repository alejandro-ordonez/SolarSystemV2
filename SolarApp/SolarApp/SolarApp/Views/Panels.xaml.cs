using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SolarApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Panels : ContentPage
    {
        public Panels()
        {
            InitializeComponent();
        }

        private async void SetDate_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Map());
        }
    }
}