namespace SecretSafe.Common.Functions
{
    using Microsoft.AspNet.Identity;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Threading.Tasks;

    public class Email 
    {
        public static Boolean Send(string reciever, string subject, string body)
        {
            try
            {
                SmtpClient SmtpServer = new SmtpClient();
                MailMessage mail = new MailMessage();
                mail.To.Add(reciever);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static async Task SendAsync(IdentityMessage message)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(message.Destination);
            mail.Subject = message.Subject;
            mail.Body = message.Body;
            mail.IsBodyHtml = true;
            
            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.SendMailAsync(mail);
            }
        }
    }
}
