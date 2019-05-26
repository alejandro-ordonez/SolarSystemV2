using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms.Maps;

namespace SolarApp.Models
{
    public class SolarPanel : INotifyPropertyChanged
    {
        private List<Sensor> readings;

        /// <summary>
        /// Data collected from the panel
        /// </summary>
        public List<Sensor> Readings
        {
            get { return readings; }
            set
            {
                readings = value;
                OnPropertyChanged("Readings");
            }
        }

        public Position Location { get; set; }
        public double IRadainceNASA { get; set; }
        /// <summary>
        /// Boltsman Constant 
        /// </summary>
        private double K { get => 5; }
        /// <summary>
        /// Description asociated to the panel
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Width of the panel
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Height of the panel
        /// </summary>
        public double Height { get; set; }


        //PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
