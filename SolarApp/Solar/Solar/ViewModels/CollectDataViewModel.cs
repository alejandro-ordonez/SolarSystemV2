using Solar.Models;
using Solar.Repositories;
using Solar.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Solar.ViewModels
{
    public class CollectDataViewModel : BaseViewModel
    {
        public Panel Panel { get; set; }
        private readonly IRepository repository;
        private readonly IESPData espData;

        private bool clickable;

        public bool Clickable
        {
            get { return clickable; }
            set { SetProperty(ref(clickable), value); }
        }


        private bool isReady;

        public bool IsReady
        {
            get { return isReady; }
            set { SetProperty(ref (isReady), value); }
        }


        public ICommand GetCommand { get; set; }
        public ICommand StartCommand { get; set; }

        public CollectDataViewModel(Panel p, IRepository repository, IESPData espData)
        {
            Panel = p;
            this.repository = repository;
            this.espData = espData;
            IsReady = false;
            Clickable = true;
            //this.data = data;
            GetCommand = new Command(async () => await GetData());
            StartCommand = new Command(async () => await Start());
        }

        private async Task Start()
        {
            var state = await espData.StartMeasuring((int)Panel.Voc);
            IsBusy = true;
            if (state)
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", "La caracterización está en ejecución, por favor espere al indicador verde", "Ok");
                await Task.Delay(3000);
                IsReady = true;
            }
            else
            {
                IsReady = false;
                await Application.Current.MainPage.DisplayAlert("Aviso", "No se pudo conectar con el dispositivo, revise que el dispositivo se encuentre encendido y en funcionamiento", "Ok");
            }
        }

        private async Task GetData()
        {
            var data = await espData.GetDataAsync(Panel.Height, Panel.Width);
            await repository.InsertReadingsToExisting(Panel.Id, data);
            IsBusy = false;
            Clickable = false;
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
