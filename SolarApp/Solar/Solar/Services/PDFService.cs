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
            PdfFont fontTitle = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            PdfFont fontSubtTitle = new PdfStandardFont(PdfFontFamily.Helvetica, 15);
            PdfFont fontNormal = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
            PdfFont fontNormalBold = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
            PdfPen blackPen = new PdfPen(PdfBrushes.Black);
            //Draw the text
            var size = page.GetClientSize();
            float col1 = 15;
            float col2 = (size.Width / 2)+5;
            float startRectangle = 60;
            float startText = startRectangle + 5;
            //Header
            graphics.DrawString($"Reporte generado: {DateTime.Now}", fontTitle, PdfBrushes.Black, new PointF(10, 10));
            graphics.DrawString($"Caracterización realizada por: {App.UserLogged.Name} {App.UserLogged.LastName}", fontSubtTitle, PdfBrushes.Black, new PointF(10, 30));
            //DrawRectangle with two columns
            graphics.DrawRectangle(blackPen, new RectangleF(10, startRectangle, size.Width - 10, 200));
            //Vertical line divider
            graphics.DrawLine(blackPen, new PointF(size.Width / 2, startRectangle), new PointF(size.Width / 2, startRectangle+200));
            // Generate all the needed rows
            for (int i = (int)startRectangle+20; i < startRectangle+200; i+=20)
            {
                graphics.DrawLine(blackPen, new PointF(10, i), new PointF(size.Width - 10, i));
            }

            //Values
            graphics.DrawString("Atributo", fontNormalBold, PdfBrushes.Black, new PointF(size.Width/4, startText));
            graphics.DrawString("Valor", fontNormalBold, PdfBrushes.Black, new PointF((size.Width / 4) * 3, startText));
            //Date
            graphics.DrawString("Fecha de caracterización", fontNormalBold, PdfBrushes.Black, new PointF(col1, startText+=20));
            graphics.DrawString(data.Date.ToString(), fontNormal, PdfBrushes.Black, new PointF(col2, startText));
            // Radiación
            graphics.DrawString("Radiación", fontNormalBold, PdfBrushes.Black, new PointF(col1, startText+=20));
            graphics.DrawString($"{data.Radiation} W/m^2", fontNormal, PdfBrushes.Black, new PointF(col2, startText));
            // Temperatura
            graphics.DrawString("Temperatura", fontNormalBold, PdfBrushes.Black, new PointF(col1, startText+=20));
            graphics.DrawString($"{data.Temp}", fontNormal, PdfBrushes.Black, new PointF(col2, startText));
            // Potencia de entrada
            graphics.DrawString("Potencia de entrada", fontNormalBold, PdfBrushes.Black, new PointF(col1, startText += 20));
            graphics.DrawString($"{data.PowerIn}", fontNormal, PdfBrushes.Black, new PointF(col2, startText));
            // Potencia de Salida
            graphics.DrawString("Potencia de salida", fontNormalBold, PdfBrushes.Black, new PointF(col1, startText += 20));
            graphics.DrawString($"{data.Pmax}", fontNormal, PdfBrushes.Black, new PointF(col2, startText));
            // Factor de llenado
            graphics.DrawString("Factor de llenado", fontNormalBold, PdfBrushes.Black, new PointF(col1, startText += 20));
            graphics.DrawString($"{data.FF}", fontNormal, PdfBrushes.Black, new PointF(col2, startText));
            // Eficiencia
            graphics.DrawString("Eficiencia", fontNormalBold, PdfBrushes.Black, new PointF(col1, startText += 20));
            graphics.DrawString($"{data.Efficency}", fontNormal, PdfBrushes.Black, new PointF(col2, startText));
            // Voltaje Voc
            graphics.DrawString("Voltaje Voc", fontNormalBold, PdfBrushes.Black, new PointF(col1, startText += 20));
            graphics.DrawString($"{data.VoC}", fontNormal, PdfBrushes.Black, new PointF(col2, startText));
            // Temperatura
            graphics.DrawString("Corriente Isc", fontNormalBold, PdfBrushes.Black, new PointF(col1, startText += 20));
            graphics.DrawString($"{data.Isc}", fontNormal, PdfBrushes.Black, new PointF(col2, startText));
            // Charts
            foreach (var chart in charts)
            {
                graphics.DrawImage(PdfImage.FromStream(await chart.GetStreamAsync()), count, 270, (size.Width/2)-10, 150 );
                count += size.Width/2+10;
            }
            //Save the document to the stream
            MemoryStream stream = new MemoryStream();
            Pdf.Save(stream);
            //Close the document
            Pdf.Close(true);
            Pdf.Dispose();
            var filename = "report" + DateTime.Now.Hour;
            var path =CreatePDF(filename, stream);
            await SendEmail.Send("Prueba", "Mensaje enviado", new List<string> { email }, await path);

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
