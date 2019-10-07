using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Solar.ViewModels
{
    public class ShellViewModel:BaseViewModel
    {
        public ShellViewModel()
        {
            OpenBrowserCommand = new Command<string>(async (URL) => await OpenBrowser(URL));
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

        private string name;

        public string Name
        {
            get => name;
            set =>SetProperty(ref (name), value);
        }

    }
}
