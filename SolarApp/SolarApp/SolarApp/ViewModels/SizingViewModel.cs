using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SolarMath.Models;
using SolarMath.Services;
using Xamarin.Forms;

namespace SolarApp.ViewModels
{
    public class SizingViewModel:BaseViewModel
    {
        private SizingSystem Sizing;
        public SolarSystem SolarData { get; set; }
        public SizingViewModel()
        {
            Sizing = new SizingSystem();
            System = new SolarSystem();
            CalculateSystemCommand = new Command<SolarSystem>(async (p) => await CalculateSystem(p));
        }
        public SolarSystem System { get; set; }
        public ICommand CalculateSystemCommand { private set; get; }
        public async Task CalculateSystem(SolarSystem s)
        {
            await Task.Run(() => SolarData = Sizing.GetSystem(s));
        }
    }
}
