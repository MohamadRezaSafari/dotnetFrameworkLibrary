using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Providers
{
    public class Email
    {
        private const string fromEmail = "mail@1000eonline.ir";
        private const string webEmail = "webmail.1000eonline.ir";
        private const string password = "Y2_8v2vo";

        //  Email.Send("ali272897@gmail.com", "Congrast", "HIIIIIIIIIIIIIIIIIIII");
        public static void Send(string to, string subject, string body)
        {
            try
            {
                var message = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    To = { to },
                    Subject = subject,
                    Body = body,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                };
                using (SmtpClient smtpClient = new SmtpClient(webEmail))
                {
                    smtpClient.Credentials = new NetworkCredential(fromEmail, password);
                    smtpClient.Port = 25;
                    smtpClient.EnableSsl = false;
                    smtpClient.Send(message);
                }
            }
            catch (Exception excep)
            {
                throw new Exception(excep.Message);
            }
        }
    }
}