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
    public class PanelsViewModel : BaseViewModel
    {

        public ObservableCollection<Panel> Panels { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand MeasureCommand { get; set; }
        public int MyProperty { get; set; }
        private Panel panel;
        private readonly SolarDbContext repository;

        public Panel Panel
        {
            get { return panel; }
            set
            {
                SetProperty(ref panel, value);
            }
        }

        public PanelsViewModel(SolarDbContext repository)
        {
            this.repository = repository;
            Panels = new ObservableCollection<Panel>
            {
                new Panel { Place = "Wherever", Reference = "1192" }
            };
            AddCommand = new Command(async () => await AddPanel());
            MeasureCommand = new Command(async () => await MeasureUI());
            RefreshCommand = new Command(async () => await RefreshList());
            RefreshCommand.Execute(null);
        }

        private async Task MeasureUI()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new GetInfo(Panel));
        }

        private async Task RefreshList()
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

        private async Task AddPanel()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new Measure());
        }
    }
}
