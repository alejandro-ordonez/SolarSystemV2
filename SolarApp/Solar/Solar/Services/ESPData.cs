using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solar.Helpers;
using Solar.Models;
using Xamarin.Essentials;

namespace Solar.Services
{
    //TODO:Update Final Service
    public class ESPData : IESPData
    {
        public Uri URL = new Uri("http://192.168.4.1");
        public HttpClient httpClient = new HttpClient();

        public Task<Location> GetLocation()
        {
            throw new NotImplementedException();
        }

        public Task<Reading> GetMaxPower(List<Reading> readings)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetTimeESP()
        {
            throw new NotImplementedException();
        }

        public async Task<DataPanel> GetDataAsync(double Height, double Width)
        {
            var response = await httpClient.GetAsync(new Uri(URL, "/Data"));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ESPRoot>(content);
                var dataPanel = new DataPanel();
                for (int i = 0; i < data.V.Length; i++)
                {
                    dataPanel.IV.Add(new Reading { I = data.I[i], V = data.V[i] });
                }
                dataPanel.IV.Add(new Reading { I = 0, V = data.V[data.V.Length - 1] });
                dataPanel.Pmax = dataPanel.IV.Max(iv => iv.Power);
                var reading = dataPanel.IV.Where(iv => (iv.I * iv.V) == dataPanel.Pmax).FirstOrDefault();
                dataPanel.Im = reading.I;
                dataPanel.Vm = reading.V;
                dataPanel.Radiation = data.ESPData.Ir;
                dataPanel.PowerIn = SolarMath.CalculatePowerPanel(Width, Height, dataPanel.Radiation);
                dataPanel.Efficency = SolarMath.CalculateEficiency(dataPanel.Im, dataPanel.Vm, dataPanel.PowerIn);
                dataPanel.FF = SolarMath.CalculateFF(dataPanel.Im, dataPanel.Vm, dataPanel.IV[0].I, dataPanel.IV[dataPanel.IV.Count - 1].V);
                dataPanel.Temp = data.ESPData.T;
                dataPanel.Date = DateTime.Now;
                return dataPanel;
            }
            return null;
        }
        public async Task<bool> StartMeasuring(int Voc)
        {
            var response = await httpClient.GetAsync(new Uri(URL, $"/Start/?VoC={Voc}"));
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
