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
        private readonly IESPData data;
        public MeasureViewModel(IESPData data)
        {
            GetDataCommand = new Command(async () => await GetData());
            this.data = data;
        }

     
        #region Properties
        private string name;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        private string description;

        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
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


        #endregion

        public ICommand GetDataCommand;
        

        #region Tasks
        private async Task UpdateProgress()
        {
            //TODO: Implement increase and decrease actions
        }
        private async Task EnableButton()
        {
            if (String.IsNullOrEmpty(Name) || String.IsNullOrEmpty(Description)||IsBusy)
            {
                ButtonState = false;
            }
            else
            {
                ButtonState = true;
            }
        }
        private async Task GetData()
        {
            IsBusy = true;
            Thread.Sleep(600);
            var panel = await data.GetData();
            
            //TODO: Add save to Database.
            IsBusy = false;
        }

        #endregion
    }
}
