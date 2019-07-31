using Solar.Models;
using Solar.Repositories;
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
        #region Properties
        public ObservableCollection<Panel> Panels { get; set; }
        private Panel panelSelected;
        #endregion

        public ICommand RefreshListCommand { get; set; }
        private readonly SolarDbContext repository;

        public ResultViewModel(SolarDbContext repository)
        {
            this.repository = repository;
            RefreshListCommand = new Command(async()=> await Refresh());
            Panels = new ObservableCollection<Panel>();
            RefreshListCommand.Execute(null);           
        }


        #region Methods
        //TODO: Link Database service to update command
        private async Task Refresh()
        {
            var x = await repository.GetPanels();
            //var x = await repository.GetPanels();
            foreach (var item in x)
            {
                if (!Panels.Contains(item))
                {
                    Panels.Add(item);
                }
            }
        }

        

        public Panel PanelSelected
        {
            get { return panelSelected; }
            set
            {
                SetProperty(ref panelSelected, value);
                Application.Current.MainPage.Navigation.PushModalAsync(new Graph(PanelSelected));
            }
        }
        #endregion

        
    }
}
