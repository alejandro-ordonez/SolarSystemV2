using Solar.Models;
using Solar.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Solar.ViewModels
{
    public class InfoViewModel:BaseViewModel
    {
        private Panel panel;

        public Panel Panel
        {
            get { return panel; }
            set { SetProperty(ref (panel), value); }
        }

        private DataPanel dataSelected;

        public DataPanel DataSelected
        {
            get { return dataSelected; }
            set
            {
                SetProperty(ref (dataSelected), value);
                ShowGraphCommand.Execute(null);
            }
        }

        public ICommand ShowGraphCommand { get; set; }

        public InfoViewModel()
        {
            ShowGraphCommand = new Command(async () => await ShowGraph());
        }

        private async Task ShowGraph()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new Graph(DataSelected));
        }
    }
}
