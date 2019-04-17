using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SolarApp.Models;
using Xamarin.Essentials;
namespace SolarApp.Services
{
    public class DataProcess : IDataProcess
    {
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
