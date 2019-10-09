using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Solar.Services;
using Solar.Views;
using Solar.Repositories;
using Solar.Helpers;
using System.Threading.Tasks;

namespace Solar
{
    public partial class App : Application
    {
        public static int UserLogged { get; set; }
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTU0MDcxQDMxMzcyZTMzMmUzMFZPcGswa0ljV2tvYi9ZTURpeWZOclk2dFZDV3dWcDZSYjY4TjR6MnlCRlk9");
            //DB = new SQLiteRepository();
            Startup.Init();
            //DB = new SolarDbContext();
            InitializeComponent();
            MainPage = new Login();
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
