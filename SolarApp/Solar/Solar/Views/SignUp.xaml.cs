using ImageCircle.Forms.Plugin.Abstractions;
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
    public partial class SignUp : ContentPage
    {
        public SignUp()
        {
            BindingContext = Startup.ServiceProvider.GetService<LogInViewModel>();
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var img = sender as CircleImage;
            await img.FadeTo(0.5, 100, Easing.Linear);
            await img.FadeTo(1, 100, Easing.CubicIn);
        }
    }
}