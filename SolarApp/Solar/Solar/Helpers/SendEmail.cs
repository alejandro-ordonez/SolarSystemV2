using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Solar.Helpers
{
    public static class SendEmail
    {
        public static async Task Send(string subject, string body, List<string> recipients, string PDFPath)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                message.Attachments.Add(new EmailAttachment(PDFPath));
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                await Application.Current.MainPage.DisplayAlert("¡Aviso!", "El dispositivo no soporta esta característica" + fbsEx, "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Lo sentimos", "Algo ha salido mal :( revisa que tengas acceso a internet :) " + ex, "Ok");
            }
        }
    }

}
