using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

        public async Task<DataPanel> GetDataAsync()
        {
            var response = await httpClient.GetAsync(new Uri(URL, "/Data"));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ESPDataModel>(content);
                var dataPanel = new DataPanel();
                for (int i = 0; i < data.V.Length; i++)
                {
                    dataPanel.IV.Add(new Reading { I = data.I[i], V = data.V[i] });
                }
                dataPanel.Radiation = data.Radiation;
                dataPanel.Temp = data.Temp;
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
