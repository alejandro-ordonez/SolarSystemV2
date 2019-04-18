using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolarApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SolarApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Curves : ContentPage
    {
        public Curves()
        {
            BindingContext = new CurvesViewModel();
            InitializeComponent();
        }
    }
}