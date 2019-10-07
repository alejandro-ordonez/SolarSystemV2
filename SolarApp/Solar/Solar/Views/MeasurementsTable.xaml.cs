using Solar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Solar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeasurementsTable : ContentView
    {
        public ICommand GraphCommand { get; set; }
        public MeasurementsTable()
        {
            GraphCommand = new Command<DataPanel>(async(data) => await GraphView(data));
            InitializeComponent();
        }

        private async Task GraphView(DataPanel item)
        {
            await Navigation.PushModalAsync(new Graph(item));
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var f = sender as Frame;
            await f.ScaleTo(1.1, 100, Easing.Linear);
            await f.ScaleTo(1, 100, Easing.Linear);
        }
    }
}