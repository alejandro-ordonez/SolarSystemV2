using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SolarApp.ViewModels
{
    public class LogInViewModel:BaseViewModel
    {
        public LogInViewModel()
        {
            BgImage = ImageSource.FromFile("login_bgimage.png");
        }
        public ImageSource BgImage { get; private set; }
    }
}
