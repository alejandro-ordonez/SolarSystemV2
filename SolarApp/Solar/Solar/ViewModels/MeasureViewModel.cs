using Solar.Models;
using Solar.Repositories;
using Solar.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Solar.Helpers;


namespace Solar.ViewModels
{
    public class MeasureViewModel:BaseViewModel
    {
        private readonly SolarDbContext repository;

        public MeasureViewModel(SolarDbContext repository)
        {
            GetDataCommand = new Command(async () => await GetData());
            this.repository = repository;
        }

     
        #region Properties
        private string reference;

        public string Reference
        {
            get { return reference; }
            set { SetProperty(ref reference, value); EnableButton(); }
        }
        private string description;

        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); EnableButton(); }
        }
        private string place;

        public string Place
        {
            get { return place; }
            set { SetProperty(ref place, value); }
        }
        private string power;

        public string Power
        {
            get { return power; }
            set { power = value; }
        }

        private string width;

        public string Width
        {
            get { return width; }
            set { SetProperty(ref width, value); }
        }
        private string height;

        public string Height
        {
            get { return height; }
            set { SetProperty(ref height, value); }
        }
        private bool buttonState;

        public bool ButtonState
        {
            get { return buttonState; }
            set { SetProperty(ref buttonState, value); }
        }
        private string progress;

        public string Progress
        {
            get { return progress; }
            set { SetProperty(ref progress, value); }
        }
        private string nominalV;

        public string NominalV
        {
            get { return nominalV; }
            set { SetProperty(ref nominalV, value); }
        }
        private string nominalI;
        public string NominalI
        {
            get { return nominalI; }
            set { SetProperty(ref nominalI, value); }
        }


        #endregion

        public ICommand GetDataCommand { get; set; }
        

        #region Tasks
        private async Task UpdateProgress()
        {
            //TODO: Implement increase and decrease actions
        }
        private async Task EnableButton()
        {
            if (String.IsNullOrEmpty(Reference) || String.IsNullOrEmpty(Description)||IsBusy)
            {
                ButtonState = false;
            }
            else
            {
                ButtonState = true;
            }
        }
        //TODO: Test Mockdata
        private async Task GetData()
        {
            await Application.Current.MainPage.DisplayAlert("Aviso", "Agregando", "Ok");
            IsBusy = true;
            var p = await CreatePanel();
            await repository.InsertNewPanel(p);
            await Task.Delay(5000);
            //TODO: Add save to Database.
            await Application.Current.MainPage.DisplayAlert("Aviso", "Panel Agregado exitosamente", "Ok");
            IsBusy = false;
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private async Task<Panel> CreatePanel()
        {
            var Loc = await LocationHelper.GetLocation();
            var p = new Panel {
                Reference = Reference,
                Description = Description,
                Height = await DoubleConverter(Height),
                Width = await DoubleConverter(Width),
                Power = await DoubleConverter(Power),
                NominalI = await DoubleConverter(NominalI),
                NominalV = await DoubleConverter(NominalV),
                Longitude = Loc.Longitude,
                Latitude = Loc.Latitude
            };
            return p;
        }
        private async Task<double> DoubleConverter(string x)
        {
            try
            {
                return double.Parse(x);
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", "Verifica tus entradas", "Ok");
                return 0;
            }
        }
        #endregion
    }
}
