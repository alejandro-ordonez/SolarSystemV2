using Solar.Models;
using Solar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Solar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Panels : ContentPage
    {
        public Panels()
        {
            BindingContext = Startup.ServiceProvider.GetService<PanelsViewModel>();
            InitializeComponent();
            
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var f = sender as Frame;
            await f.ScaleTo(1.1, 100, Easing.Linear);
            await f.ScaleTo(1, 100, Easing.Linear);
        }
    }
}