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
    public partial class LogIn : ContentPage
    {
        public LogIn()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        private void Log_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AppShell());
        }
    }
}