using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using Solar.Services;
using System.IO;

namespace Solar.ViewModels
{
    public class ShellViewModel:BaseViewModel
    {
        private IResizeImageService ResizeImage = DependencyService.Get<IResizeImageService>();
        public ShellViewModel()
        {
            OpenBrowserCommand = new Command<string>(async (URL) => await OpenBrowser(URL));
            Name = $"{App.UserLogged.Name} {App.UserLogged.LastName.Substring(0, App.UserLogged.LastName.IndexOf(" "))}";
            Email = App.UserLogged.Email;
            ImageProfile = ImageSource.FromStream(()=> new MemoryStream(ResizeImage.ResizeImage(File.ReadAllBytes(App.UserLogged.PhotoPath), 200, 200)));
        }

        private async Task OpenBrowser(string URL)
        {
            await Browser.OpenAsync(URL, new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.Black,
                PreferredControlColor = Color.White
            });
        }

        public ICommand OpenBrowserCommand { get; set; }


        private ImageSource imageProfile;

        public ImageSource ImageProfile
        {
            get => imageProfile;
            set => SetProperty(ref (imageProfile), value);
        }

        private string email;

        public string Email
        {
            get => email;
            set => SetProperty(ref (email), value);
        }


        private string name;

        public string Name
        {
            get => name;
            set =>SetProperty(ref (name), value);
        }

    }
}
