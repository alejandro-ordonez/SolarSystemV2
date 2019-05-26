using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SolarApp.Models;
using Xamarin.Essentials;
namespace SolarApp.Services
{
    public interface IDataProcess
    {
        /// <summary>
        /// Get location provided by the phone
        /// </summary>
        /// <returns>Returns the current location</returns>
        Task<Location> GetLocation();
        /// <summary>
        /// Get readings buffered by ESP32 (Time, I, V, T, R)
        /// </summary>
        /// <returns>A list of type sensor whic maps all the information obtained by ESP32</returns>
        Task<List<Sensor>> GetReadingsESP32();
        /// <summary>
        /// Post action to publish all the data obtained.
        /// </summary>
        /// <param name="sensors">List of data obtained by a measure</param>
        /// <returns>ture if succesful</returns>
        Task<bool> PostReadingsToServer(List<Sensor> sensors);
        /// <summary>
        /// Gets readings saved in DB in server
        /// </summary>
        /// <param name="start">Start date</param>
        /// <param name="end">End date</param>
        /// <returns>List of curves I/V obtained in the past</returns>
        Task<IEnumerable<SolarPanel>> GetReadingsServer();
        Task<SolarPanel> GetPanel(double Longitude, double Latitude);
    }
}
