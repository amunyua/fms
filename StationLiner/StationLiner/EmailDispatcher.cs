using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace StationLinerMVC
{
    public class EmailDispacher
    {
        public EmailDispacher()
        {
        }
        public string from { get; set; }
        public string to { get; set; }
        public string bcc { get; set; }
        public string password { get; set; }
        public string subject { get; set; }
        public string strbody { get; set; }

        private const string InfoEmail = "test@go4software.com";
        private const string InfoEmailPassword = "Destiny@123";
        private const string ClientMailServer = "mail.go4software.com";

        public bool SendEmail()
        {
            MailMessage message = new MailMessage();
            message.To.Add(to);
            if (!string.IsNullOrEmpty(bcc))
                message.Bcc.Add(bcc);
            message.From = new MailAddress(from, System.Web.Configuration.WebConfigurationManager.AppSettings["MailFromAlias"]);
            message.Subject = subject;
            message.Body = strbody;
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.ASCII;
            //message.Priority = System.Net.Mail.MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            smtp.EnableSsl = true;
            //smtp.Port = 25;
            smtp.Port = 587;
            //smtp.Port = 465;
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Host = System.Web.Configuration.WebConfigurationManager.AppSettings["ClientMailServer"];
            smtp.Credentials = new System.Net.NetworkCredential(from, password);
            ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            //message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            try
            {
                smtp.Send(message);
                message = null;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private static void SendCompletedCallback(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            String token = (string)e.UserState;
            if (e.Cancelled)
            {
                //cancelled
            }
            else
            {
                //sent
            }
        }
    }
}