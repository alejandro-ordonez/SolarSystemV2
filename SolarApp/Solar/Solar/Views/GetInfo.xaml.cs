using Solar.Models;
using Solar.Repositories;
using Solar.Services;
using Solar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Solar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GetInfo : ContentPage
    {
        public GetInfo(Panel panel)
        {
            BindingContext = new GetInfoViewModel(panel, Startup.ServiceProvider.GetService<IESPData>(), Startup.ServiceProvider.GetService<IRepository>());
            InitializeComponent();
        }
    }
}