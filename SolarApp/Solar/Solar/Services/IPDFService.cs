using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Solar.Services
{
    interface IPDFService
    {
        Task<bool> SendPDFAsync();
        Task<Stream> CreatePDF();
        Task<bool> CreateSfcChart();
    }
}
