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
    public partial class CollectData : ContentPage
    {
        public CollectData(Panel panel)
        {
            BindingContext = new CollectDataViewModel(panel, Startup.ServiceProvider.GetService<IRepository>(), Startup.ServiceProvider.GetService<IESPData>());
            InitializeComponent();
        }
    }
}