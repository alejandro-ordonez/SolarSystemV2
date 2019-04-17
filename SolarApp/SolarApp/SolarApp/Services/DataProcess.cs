using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SolarApp.Models;

namespace SolarApp.Services
{
    public class DataProcess : IDataProcess
    {
        public Task<List<Sensor>> GetReadingsESP32()
        {
            throw new NotImplementedException();
        }

        public Task<List<SolarPanel>> GetReadingsServer(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PostReadingsToServer(List<Sensor> sensors)
        {
            throw new NotImplementedException();
        }
    }
}
