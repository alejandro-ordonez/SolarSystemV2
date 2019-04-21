using System;
using System.Collections.Generic;
using SolarApp.Views;
using Xamarin.Forms;

namespace SolarApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            LoadLogin();
           
        }
        public async void LoadLogin()
        {
            await Navigation.PushModalAsync(new LogIn());
        }
    }
}
