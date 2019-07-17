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
        public async Task<Panel> GetData()
        {
            var P = new Panel
            {
                Date = DateTime.Now,
                IV = new List<Reading>() {
                    new Reading{ I=2.5, V=0},
                    new Reading{ I=2.5, V=0},
                    new Reading{ I=2.5, V=0},
                    new Reading{ I=2.5, V=0},
                    new Reading{ I=2.5, V=0},
                    new Reading{ I=2.5, V=0},
                    new Reading{ I=2.5, V=0},
                    new Reading{ I=2.5, V=0},
                    new Reading{ I=2.5, V=0},
                    new Reading{ I=2.5, V=0},
                    new Reading{ I=2.5, V=0},
                    new Reading{ I=2.5, V=0},
                    new Reading{ I=2.5, V=0},
                    new Reading{ I=2.5, V=0} },
                Radiation = 4.5,
                Temp = 21.8,
                Location = await GetLocation(),
                Description = "Test1",
                Name = "Panel"
            };
            var r = await GetMaxPower(P.IV);
            P.Im = r.I;
            P.Vm = r.V;
            return P;
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
    }
}
