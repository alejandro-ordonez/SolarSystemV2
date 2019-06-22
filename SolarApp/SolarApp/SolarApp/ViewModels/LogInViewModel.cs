using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SolarApp.ViewModels
{
    public class LogInViewModel:BaseViewModel
    {
        public LogInViewModel()
        {
            BgImage = ImageSource.FromFile("login_bgimage.png");
            LogInCommand = new Command(async () => await LogIn(), ()=> !IsBusy);
        }
        public ImageSource BgImage { get; private set; }

        public Command LogInCommand { get;}

        private string user;

        public string User
        {
            get { return user; }
            set
            {
                SetProperty(ref user, value);
            }
        }
        private string pass;

        public string Pass
        {
            get { return pass; }
            set
            {
                SetProperty(ref pass, value);
            }
        }
        private async Task<bool> LogIn()
        {
            //TODO: Add feature to process LogIn in Service.
            IsBusy = true;
            LogInCommand.ChangeCanExecute();
            await Task.Delay(5000);
            IsBusy = false;
            LogInCommand.ChangeCanExecute();
            await Application.Current.MainPage.Navigation.PopModalAsync();
            return true;
        }
    }
}
