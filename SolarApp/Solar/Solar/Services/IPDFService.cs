using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Solar.Services
{
    interface IPDFService
    {
        Task CreatePDFAndSend(List<SfChart> charts, string email);
    }
}
