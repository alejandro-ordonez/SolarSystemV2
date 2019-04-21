using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SolarApp.Services;
using SolarApp.Views;
using System.Threading.Tasks;

namespace SolarApp
{
    public partial class App : Application
    {
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        public static string AzureBackendUrl = "http://localhost:5000";
        public static bool UseMockDataStore = true;
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("OTI2NzhAMzEzNzJlMzEyZTMwQWVVRHFlYnlMMnQrbHdvTXpZTEhtVUMwWUFXWkVPNHlxbHJmcEZVS3Njaz0=");
            InitializeComponent();
            DependencyService.Register <DataProcess>();
            if (UseMockDataStore)
                DependencyService.Register<MockDataStore>();
            else
                DependencyService.Register<AzureDataStore>();
            MainPage = new AppShell();
        }

        internal static ShellSection GetShellSection(Element element)
        {
            if (element == null)
            {
                return null;
            }

            var parent = element;
            var parentSection = parent as ShellSection;

            while (parentSection == null && parent != null)
            {
                parent = parent.Parent;
                parentSection = parent as ShellSection;
            }

            return parentSection;
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
