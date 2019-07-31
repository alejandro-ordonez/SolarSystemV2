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

        public Panel PanelSelected
        {
            get { return panel; }
            set
            {
                SetProperty(ref panel, value);
            }
        }
        private bool _opening=true;

        public bool Opening
        {
            get { return _opening; }
            set { SetProperty(ref _opening, value); }
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


        private bool isRefreshing;

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); }
        }

        private async Task MeasureUI()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new GetInfo(PanelSelected));
        }

        private async Task RefreshList()
        {
            IsRefreshing = true;
            var x = await repository.GetPanels();
            //var x = await repository.GetPanels();
            foreach (var item in x)
            {
                if (!Panels.Contains(item))
                {
                    Panels.Add(item);
                }
            }
            IsRefreshing = false;
        }

        private async Task AddPanel()
        {
            Opening = false;
            await Application.Current.MainPage.Navigation.PushModalAsync(new Measure());
            Opening = true;
        }
    }
}
