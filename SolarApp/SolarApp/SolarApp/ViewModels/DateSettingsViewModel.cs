using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SolarApp.ViewModels
{
    public class DateSettingsViewModel:BaseViewModel
    {
        private ICommand SetDate { get; set; }
        public DateSettingsViewModel()
        {
            SetDate = new Command<DateTime>((a) => SaveDate(a));
        }
        private void SaveDate(DateTime x)
        {

        }
    }
}
