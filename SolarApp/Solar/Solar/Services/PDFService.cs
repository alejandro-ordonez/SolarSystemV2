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
using Solar.Models;

namespace Solar.Services
{
    public class PDFService : IPDFService
    {

        public async Task CreatePDFAndSend(List<SfChart> charts, string email, DataPanel data)
        {
            float count = 10;
            PdfDocument Pdf = new PdfDocument();
            //Add a page to the current PDF document
            PdfPage page = Pdf.Pages.Add();
            //Create PDF graphics for the page
            PdfGraphics graphics = page.Graphics;
            //Set the standard fonts
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            PdfPen pen = new PdfPen(PdfBrushes.Black);
            //Draw the text
            var size = page.GetClientSize();
            graphics.DrawString($"Reporte generado: {DateTime.Now}", font, PdfBrushes.Black, new PointF(10, 10));
            graphics.DrawString($"Caracterización realizada por: {App.UserLogged.Name} {App.UserLogged.LastName}", font, PdfBrushes.Black, new PointF(10, 25));

            /// Table
            graphics.DrawRectangle(new PdfPen(PdfBrushes.Black, 2), 10, 40, size.Width-10, 200);
            graphics.DrawLine(new PdfPen(PdfBrushes.Black), new PointF(size.Width / 2, 30), new PointF(size.Width/2, 130));

            //Values
            graphics.DrawString("Atributo", new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold), PdfBrushes.Black, new PointF(size.Width/4, 42));
            graphics.DrawString("Valor", new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold), PdfBrushes.Black, new PointF((size.Width / 4)*3, 42));
            graphics.DrawLine(new PdfPen(PdfBrushes.Black), new PointF(10, 60), new PointF(size.Width-10, 45));

            //Date
            graphics.DrawString("Fecha de caracterización", new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold), PdfBrushes.Black, new PointF(15, 62));
            graphics.DrawString(data.Date.ToString(), new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold), PdfBrushes.Black, new PointF(size.Width+5, 62));
            graphics.DrawLine(new PdfPen(PdfBrushes.Black), new PointF(10, 80), new PointF(size.Width-10, 60));
            
            graphics.DrawString("Radiación", new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold), PdfBrushes.Black, new PointF(15, 82));
            graphics.DrawString($"{data.Radiation} W/m^2", new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold), PdfBrushes.Black, new PointF(size.Width+5, 82));
            graphics.DrawLine(new PdfPen(PdfBrushes.Black), new PointF(10, 80), new PointF(size.Width-10, 80));


            foreach (var chart in charts)
            {
                graphics.DrawImage(PdfImage.FromStream(await chart.GetStreamAsync()), count, 200, (size.Width/2)-10, 150 );
                count += size.Width/2+10;
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
