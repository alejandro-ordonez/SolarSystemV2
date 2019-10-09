using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solar.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Solar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            BindingContext = Startup.ServiceProvider.GetService<LogInViewModel>();
            InitializeComponent();
        }
    }
}