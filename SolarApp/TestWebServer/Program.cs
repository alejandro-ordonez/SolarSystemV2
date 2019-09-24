using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;

namespace TestWebServer
{
    public class Program
    {
        public static RootObject ESP32= new RootObject();
        public static Readings r = new Readings();
        public static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Connecting....");
                Test();
                
            }
            
        }
        public static async void Test()
        {
            var response = await client.GetAsync("http://192.168.4.1/Start/?VoC=35");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine("No Response");
            }
            Thread.Sleep(8000);
            response = await client.GetAsync("http://192.168.4.1/Data");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine("No Response");
            }
        }
    }
    public class Variables
    {
        public string json { get; set; }
    }

    public class RootObject
    {
        public Variables variables { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string hardware { get; set; }
        public bool connected { get; set; }
    }
    public class Readings
    {
        public int carID { get; set; }
        public int placeID { get; set; }
        public int floorID { get; set; }
        public int owner { get; set; }
        public int licensePlate { get; set; }
    }
}
