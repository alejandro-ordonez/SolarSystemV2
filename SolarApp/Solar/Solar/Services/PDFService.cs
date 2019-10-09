using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Solar.Helpers;
using Xamarin.Essentials;

using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Syncfusion.SfChart.XForms;
using Xamarin.Forms;

namespace Solar.Services
{
    public class PDFService : IPDFService
    {

        public async Task CreatePDFAndSend(List<SfChart> charts, string email)
        {
            int count = 20;
            PdfDocument Pdf = new PdfDocument();
            //Add a page to the current PDF document
            PdfPage page = Pdf.Pages.Add();
            //Create PDF graphics for the page
            PdfGraphics graphics = page.Graphics;
            //Set the standard fonts
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            //Draw the text
            graphics.DrawString("Hello World!!!", font, PdfBrushes.Black, new PointF(10, 10));
            foreach (var chart in charts)
            {
                graphics.DrawImage(PdfImage.FromStream(await chart.GetStreamAsync()), 10, count, page.GetClientSize().Width, page.GetClientSize().Height);
                count += 100;
            }
            //Save the document to the stream
            MemoryStream stream = new MemoryStream();
            Pdf.Save(stream);
            //Close the document
            Pdf.Close(true);
            var filename = "report" + DateTime.Now.Hour;
            var path =CreatePDF(filename, stream);
            await SendEmail.Send("Prueba", "Mensaje enviado", new List<string> { "m.alejandro1898@gmail.com" }, await path);

        }

        /// <summary>
        /// Returns the full path where the pdf was saved
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="PDF"></param>
        /// <returns></returns>
        private async Task<string> CreatePDF(string fileName, MemoryStream PDF)
        {
            try
            {
                var directory = Path.Combine(FileSystem.AppDataDirectory, $"{fileName}.pdf");
                var fs = new FileStream(directory, FileMode.Create, FileAccess.ReadWrite);
                PDF.WriteTo(fs);
                fs.Close();
                return directory;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", $"El PDF {fileName}, no pudo ser generado {e}", "Ok");
                return "";
            }
        }
    }
}
