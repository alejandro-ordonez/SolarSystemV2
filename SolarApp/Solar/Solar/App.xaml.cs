using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Solar.Services;
using Solar.Views;
using Solar.Repositories;
namespace Solar
{
    public partial class App : Application
    {
        //public static SQLiteRepository DB { get; set; }
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTIxNTM4QDMxMzcyZTMyMmUzMEMyNW03U2FrbHBUNi9xYjQ3b1dIQm1GemJpZzdzcG00NFc2WHNWRjZFRWc9");
            //DB = new SQLiteRepository();
            Startup.Init();
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
