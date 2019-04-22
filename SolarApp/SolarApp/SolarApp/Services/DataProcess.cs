using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SolarApp.Models;
using Xamarin.Essentials;
using System.Linq;
using Xamarin.Forms.Maps;
namespace SolarApp.Services
{
    public class DataProcess : IDataProcess
    {
        private List<Sensor> MockReadings { get; set; }
        public DateTime StartDate { get; set; }
        public SolarPanel CurrentPanel { get; private set; }
        public DataProcess()
        {
            MockReadings = new List<Sensor>()
            {
                new Sensor { Date= DateTime.Now, ValueV=0.1, ValueI=10, ValueIR=4.5, ValueTemp=25},
                new Sensor { Date= DateTime.Now, ValueV=0.5, ValueI=9.98, ValueIR=4.5, ValueTemp=25},
                new Sensor { Date= DateTime.Now, ValueV=2.0, ValueI=9.95, ValueIR=4.5, ValueTemp=25},
                new Sensor { Date= DateTime.Now, ValueV=3.0, ValueI=9.93, ValueIR=4.5, ValueTemp=25},
                new Sensor { Date= DateTime.Now, ValueV=4.0, ValueI=9.91, ValueIR=4.5, ValueTemp=25},
                new Sensor { Date= DateTime.Now, ValueV=5.0, ValueI=8.8, ValueIR=4.5, ValueTemp=25},
                new Sensor { Date= DateTime.Now, ValueV=6.0, ValueI=7.8, ValueIR=4.5, ValueTemp=25},
                new Sensor { Date= DateTime.Now, ValueV=7.0, ValueI=6, ValueIR=4.5, ValueTemp=25},
                new Sensor { Date= DateTime.Now, ValueV=8.0, ValueI=0.1, ValueIR=4.5, ValueTemp=25},
            };
            Panels = new List<SolarPanel>()
            {
                new SolarPanel{Description="Panel 1", Height=10, Width=4, Readings=MockReadings, Location=new Position(4.632154,-74.142143 ) },
                new SolarPanel{Description="Panel 2", Height=10, Width=4, Readings=MockReadings, Location=new Position(4.656544,-74.144213 ) },
                new SolarPanel{Description="Panel 3", Height=10, Width=4, Readings=MockReadings, Location=new Position(4.623546,-74.14213 ) },
                new SolarPanel{Description="Panel 4", Height=10, Width=4, Readings=MockReadings, Location=new Position(4.631564,-74.14213 ) },
                new SolarPanel{Description="Panel 5", Height=10, Width=4, Readings=MockReadings, Location=new Position(4.656542,-74.14213 ) },
                new SolarPanel{Description="Panel 6", Height=10, Width=4, Readings=MockReadings, Location=new Position(4.642478,-74.14213 ) },
                new SolarPanel{Description="Panel 7", Height=10, Width=4, Readings=MockReadings, Location=new Position(4.547646,-74.14213 )},
            };
        }

        private List<SolarPanel> Panels { get; set; }
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

        public async Task<IEnumerable<SolarPanel>> GetReadingsServer()
        {
            return await Task.FromResult(Panels);
        }

        public Task<bool> PostReadingsToServer(List<Sensor> sensors)
        {
            throw new NotImplementedException();
        }

        public Task<SolarPanel> GetPanel(double Longitude, double Latitude)
        {
            var Panel = Panels.Where((SolarPanel arg) => arg.Location.Longitude == Longitude && arg.Location.Latitude == Latitude).FirstOrDefault();
            return Task.FromResult(Panel);
        }

        public async Task SetCurrentPanel(SolarPanel panel)
        {
            CurrentPanel = panel;
        }
    }
}
