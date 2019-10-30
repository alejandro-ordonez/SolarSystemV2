using Solar.Models;
using Solar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Solar.Services;

namespace Solar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Graph : ContentPage
    {
        public ICommand GenerateReportCommand { get; set; }
        public DataPanel VM { get; set; }
        private readonly IPDFService service;
        public Graph(DataPanel data)
        {
            VM = data;
            VM.IV = data.IV.OrderBy(reading => reading.V).ToList();
            BindingContext = VM;
            service = Startup.ServiceProvider.GetService<IPDFService>();
            InitializeComponent();
        }


        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.DisplayAlert("Aviso", "El reporte será generado y enviado a m.alejandro1898@gmail.com", "Ok");
            await service.CreatePDFAndSend(new List<Syncfusion.SfChart.XForms.SfChart> { IVCurve, PVCurve }, App.UserLogged.Email, VM);
        }
    }
}