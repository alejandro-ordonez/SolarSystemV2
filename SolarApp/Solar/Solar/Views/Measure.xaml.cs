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
    public partial class Measure : ContentPage
    {
        public Measure()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<MeasureViewModel>();
        }
    }
}