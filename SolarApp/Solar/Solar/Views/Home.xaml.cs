using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Solar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        //public ObservableCollection<ItemCarousel> Items = new ObservableCollection<ItemCarousel>() { new ItemCarousel {Animation="Home1.json", Description="Test" } };
        public Home()
        {
            InitializeComponent();
            //Carousel.ItemsSource = Items;
        }
    }
    /*public class ItemCarousel
    {
        public string Animation { get; set; }
        public string Description { get; set; }
    }*/
}