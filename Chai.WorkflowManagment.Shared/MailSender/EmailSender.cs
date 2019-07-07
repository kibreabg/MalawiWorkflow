using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Net.Configuration;
using System.Configuration;
namespace Chai.WorkflowManagment.Shared.MailSender
{

    public class EmailSender
    {

        public static bool Send(string to, string subject, string body)
        {
            string from = string.Empty;
            string localIP = "http://zimops/ZWFM/UserLogin";
            //string publicIp = "http://197.211.216.65/ZWFM/UserLogin.aspx";
        

            SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            try
            {
                using (SmtpClient client = new SmtpClient(section.Network.Host, section.Network.Port))
                {
                    client.EnableSsl = section.Network.EnableSsl;
                    client.Timeout = 2000000;
                    client.Credentials = new System.Net.NetworkCredential(section.Network.UserName, section.Network.Password);
                    client.Send(section.From, to, subject, body + " Click here: " + localIP );
                    client.Dispose();
                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public static bool SendEmails(string from, string to, string subject, string body)
        {

            SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            try
            {
                using (SmtpClient client = new SmtpClient(section.Network.Host, section.Network.Port))
                {
                    client.EnableSsl = section.Network.EnableSsl;
                    client.Timeout = 2000000;
                    client.Credentials = new System.Net.NetworkCredential(section.Network.UserName, section.Network.Password);
                    client.Send(section.From, to, subject, body);
                    client.Dispose();
                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public static bool SendException(string to, string subject, string body)
        {
            string from = string.Empty;

            SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            //create the mail message
            MailMessage mail = new MailMessage();

            //set the addresses
            mail.From = new MailAddress(section.From);
            mail.To.Add(to);

            //set the content
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            
            try
            {
                using (SmtpClient client = new SmtpClient(section.Network.Host, section.Network.Port))
                {
                    client.EnableSsl = section.Network.EnableSsl;
                    client.Timeout = 2000000;
                    client.Credentials = new System.Net.NetworkCredential(section.Network.UserName, section.Network.Password);
                    client.Send(mail);
                    client.Dispose();
                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

    }
}

