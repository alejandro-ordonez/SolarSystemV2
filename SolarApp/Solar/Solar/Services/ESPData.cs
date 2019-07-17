using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Solar.Models;
using Xamarin.Essentials;

namespace Solar.Services
{
    //TODO:Update Final Service
    public class ESPData : IESPData
    {
        public Uri URL = new Uri("http://192.168.1:80");
        public Task<Panel> GetData()
        {
            throw new NotImplementedException();
        }

        public Task<Location> GetLocation()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetMaxPower(List<double> I, List<double> V)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetTimeESP()
        {
            throw new NotImplementedException();
        }
    }
}
