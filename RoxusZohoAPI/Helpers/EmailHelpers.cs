using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Helpers
{
    public class EmailHelpers
    {
        public static async Task SendEmail(EmailContent emailContent)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    SmtpClient SmtpServer = new SmtpClient(emailContent.SmtpServer);

                    if (!string.IsNullOrEmpty(emailContent.FromName))
                    {
                        mail.From = new MailAddress(emailContent.Email, emailContent.FromName);
                    }
                    else
                    {
                        mail.From = new MailAddress(emailContent.Email);
                    }
                    mail.IsBodyHtml = true;

                    string[] clients = emailContent.Clients.Split(";");

                    foreach (var client in clients)
                    {
                        mail.To.Add(client);
                    }

                    mail.Subject = emailContent.Subject;
                    mail.Body = emailContent.Body;

                    SmtpServer.Port = emailContent.SmtpPort;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(emailContent.Email, emailContent.Password);
                    SmtpServer.EnableSsl = true;

                    await SmtpServer.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task SendEmailWithAttachment(EmailContent emailContent, List<Attachment> attachments)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    var SmtpServer = new SmtpClient(emailContent.SmtpServer);

                    if (!string.IsNullOrEmpty(emailContent.FromName))
                    {
                        mail.From = new MailAddress(emailContent.Email, emailContent.FromName);
                    }
                    else
                    {
                        mail.From = new MailAddress(emailContent.Email);
                    }

                    mail.IsBodyHtml = true;

                    string[] clients = emailContent.Clients.Split(";");

                    foreach (var client in clients)
                    {
                        mail.To.Add(client);
                    }

                    mail.Subject = emailContent.Subject;
                    mail.Body = emailContent.Body;

                    foreach (var attachment in attachments)
                    {
                        mail.Attachments.Add(attachment);
                    }

                    SmtpServer.Port = CommonConstants.SmtpPort;
                    SmtpServer.UseDefaultCredentials = false;
                    SmtpServer.EnableSsl = true;
                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                    SmtpServer.Timeout = 20000;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(emailContent.Email, emailContent.Password);

                    await SmtpServer.SendMailAsync(mail);
                }
            }
            catch (Exception)
            {
                throw new Exception("Send email failed");
            }
        }
    }
}
