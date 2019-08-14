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
        private readonly IESPData data;

        public ICommand GetCommand { get; set; }

        public CollectDataViewModel(Panel p)
        {
            Panel = p;
            //this.data = data;
            GetCommand = new Command(async () => await GetData());
        }

        private async Task GetData()
        {
            IsBusy = true;
            //var x =await data.GetData();
            await Task.Delay(1000);
            //await repository.InsertReadingsToExisting(Panel.Id, x);
            IsBusy = false;
        }
    }
}
