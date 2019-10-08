using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Solar.Models;
using System.Linq;
using Xamarin.Essentials;

namespace Solar.Services
{
    public class ESPMockData : IESPData
    {
        //TODO: Update ESP32 Ip
        public string URL = "http://192.168.1:80";
        public async Task<DataPanel> GetDataAsync(double Width, double Height)
        {
            var dataP = new DataPanel
            {
                Date = DateTime.Now,
                IV = new List<Reading>() {
                    new Reading{ I=2.5, V=0},
                    new Reading{ I=2.5, V=1.0},
                    new Reading{ I=2.5, V=1.5},
                    new Reading{ I=2.5, V=2.0},
                    new Reading{ I=2.5, V=2.5},
                    new Reading{ I=2.5, V=3.0},
                    new Reading{ I=2.5, V=3.5},
                    new Reading{ I=2.5, V=4.5},
                    new Reading{ I=2.0, V=5.0},
                    new Reading{ I=1.8, V=5.2},
                    new Reading{ I=1.5, V=5.3},
                    new Reading{ I=1, V=5.35},
                    new Reading{ I=0.5, V=5.45},
                    new Reading{ I=0.1, V=5.50} },
                Radiation = 4.5,
                Temp = 21.8,
            };
            var r = await GetMaxPower(dataP.IV);
            dataP.Im = r.I;
            dataP.Vm = r.V;
            return dataP;
        }

        public async Task<Reading> GetMaxPower(List<Reading> Readings)
        {
            Reading pos=null;
            double temp = 0;
            foreach (var item in Readings)
            {
                if (item.V * item.I > temp)
                {
                    pos = item;
                    temp = item.V * item.I;
                }
            }
            return pos;
        }
        public async Task<Location> GetLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.High);
                var location = await Geolocation.GetLocationAsync(request);
                return location;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                return null;
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                return null;
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                return null;
            }
            catch (Exception ex)
            {
                // Unable to get location
                return null;
            }
        }
        public async Task<bool> SetTimeESP()
        {
            return true;
        }

        public Task<bool> StartMeasuring(int Voc)
        {
            throw new NotImplementedException();
        }
    }
}
