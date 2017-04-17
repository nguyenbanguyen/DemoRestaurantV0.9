using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
namespace DemoRestaurant.Models
{
    public class EmailModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Destination { get; set; }
        public EmailModel() {

        }
        public EmailModel(string csubject, string cbody, string cdestination) {
            Subject = csubject;
            Body = cbody;
            Destination = cdestination;
        }
        public  bool SendEmail() {
            SmtpClient client = new SmtpClient();

            client.Port = 587;
            // port 465 587 25
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("Nguyễn Bá Nguyên", "Nguyễn Bá Nguyên");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            var MailMessage = new MailMessage("Nguyễn Bá Nguyên", Destination, Subject, Body);
            MailMessage.IsBodyHtml = true;
            try
            {
                client.Send(MailMessage);
                return true;
            }
            catch
            {
                return false;
            }
            }
    }
}