using Solar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Solar.Services
{
    public interface IESPData
    {
        Task<DataPanel> GetData();
        Task<bool> SetTimeESP();
        Task<Reading> GetMaxPower(List<Reading> readings);
        Task<Location> GetLocation();
    }
}
