using Solar.Models;
using Solar.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Solar.ViewModels
{
    public class ResultViewModel:BaseViewModel
    {
        public ResultViewModel()
        {
            RefreshListCommand = new Command(async()=> await Refresh());
        }

        //TODO: Link Database service to update command
        private Task Refresh()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Panel> Panels { get; set; }
        private Panel panelSelected;

        public Panel PanelSelected
        {
            get { return panelSelected; }
            set
            {
                SetProperty(ref panelSelected, value);
                Application.Current.MainPage.Navigation.PushModalAsync(new Graph(PanelSelected));
            }
        }


        public ICommand RefreshListCommand;
    }
}
