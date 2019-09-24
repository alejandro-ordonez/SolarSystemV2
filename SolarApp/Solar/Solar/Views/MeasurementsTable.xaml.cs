using Solar.Models;
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
    public partial class MeasurementsTable : ContentView
    {
        public MeasurementsTable()
        {
            InitializeComponent();
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Application.Current.MainPage.DisplayAlert("Test", "ok", "ok");
            var item = (DataPanel)e.SelectedItem;
            await Navigation.PushModalAsync(new Graph(item));
        }
    }
}