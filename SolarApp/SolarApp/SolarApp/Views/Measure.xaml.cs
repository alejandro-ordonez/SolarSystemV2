using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.XForms.MaskedEdit;

namespace SolarApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Measure : ContentPage
    {
        public decimal Progress { get; set; }
        public Measure()
        {
            Progress = 0;
            InitializeComponent();
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            var x= sender as Entry;
            if (!x.Text.Equals(null))
            {
                Completed.Progress += 0.1;
            }
        }

    }
}