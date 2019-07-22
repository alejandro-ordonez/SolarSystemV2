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

namespace Solar.ViewModels
{
    public class MeasureViewModel:BaseViewModel
    {
        private IRepository repository;

        public MeasureViewModel(IRepository repository)
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
        private double width;

        public double Width
        {
            get { return width; }
            set { SetProperty(ref width, value); }
        }
        private double height;

        public double Height
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
        private double progress;

        public double Progress
        {
            get { return progress; }
            set { SetProperty(ref progress, value); }
        }
        private double nominalV;

        public double NominalV
        {
            get { return nominalV; }
            set { SetProperty(ref nominalV, value); }
        }
        private double nominalI;
        public double NominalI
        {
            get { return nominalI; }
            set { SetProperty(ref nominalI, value); }
        }


        #endregion

        public ICommand GetDataCommand;
        

        #region Tasks
        private async Task UpdateProgress()
        {
            //TODO: Implement increase and decrease actions
        }
        private async Task EnableButton()
        {
            if (String.IsNullOrEmpty(reference) || String.IsNullOrEmpty(Description)||IsBusy)
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
            IsBusy = true;
            await repository.InsertNewPanel(CreatePanel());
            await Task.Delay(5000);
            //TODO: Add save to Database.
            IsBusy = false;
        }
        private Panel CreatePanel()
        {
            var p = new Panel {
                Reference = Reference,
                Description = Description,
                Height = Height,
                Width = Width,
                NominalI = NominalI,
                NominalV = NominalV
            };
            return p;
        }
        #endregion
    }
}
