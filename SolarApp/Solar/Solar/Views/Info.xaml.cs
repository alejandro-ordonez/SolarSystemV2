using Solar.Models;
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
    public partial class Info : TabbedPage
    {
        public Info(Panel p)
        { 
            BindingContext = p;
            InitializeComponent();
        }

    }
}