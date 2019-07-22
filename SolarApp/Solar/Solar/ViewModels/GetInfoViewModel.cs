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
    public class GetInfoViewModel:BaseViewModel
    {
        private readonly IESPData data;
        private readonly IRepository repository;

        public GetInfoViewModel(Panel panel, IESPData data, IRepository repository)
        {
            Panel = panel;
            this.data = data;
            this.repository = repository;
            GetCommand = new Command(async () => await GetAndSave());
        }

        private async Task GetAndSave()
        {
            var p =await data.GetData();
            await repository.InsertReadingsToExisting(Panel.Id, p);

        }

        public ICommand GetCommand { get; set; }
        public Panel Panel { get; }
    }
}
