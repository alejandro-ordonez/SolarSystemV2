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
        private readonly IRepository repository;

        public ICommand GetCommand { get; set; }

        public CollectDataViewModel(Panel p, IRepository repository)
        {
            Panel = p;
            this.repository = repository;
            //this.data = data;
            GetCommand = new Command(async () => await GetData());
        }

        private async Task GetData()
        {
            IsBusy = true;
            Panel.Data.Add(
                new DataPanel()
            {
                IV = new List<Reading>(){
                    new Reading{I = 5,V = 0},
                    new Reading{I = 4.70,V = 1},
                    new Reading{I = 4.65,V = 2},
                    new Reading{I = 4.55,V = 3},
                    new Reading{I = 4.50,V = 4},
                    new Reading{I = 4.48,V = 5},
                    new Reading{I = 4.40,V = 6},
                    new Reading{I = 4.38,V = 7},
                    new Reading{I = 4.2,V = 8},
                    new Reading{I = 4.0,V = 9},
                    new Reading{I = 3.6,V = 10},
                    new Reading{I = 3.2,V = 11},
                    new Reading{I = 2.5,V = 12},
                    new Reading{I = 2.0,V = 13},
                    new Reading{I = 1.2,V = 14},
                    new Reading{I = 0,V = 15}
                }
            } );
            await Task.Delay(1000);
            await repository.InsertReadingsToExisting(Panel);
            IsBusy = false;
        }
    }
}
