using System;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Configuration;

namespace WIT.Common.Mailer
{
    public class SMTPMailService : IMailService
    {
        private string FromAddress { get; set; }
        private string FromName { get; set; }
        private string ServerHost { get; set; }
        private int? ServerPort { get; set; }
        private string ServerUsername { get; set; }
        private string ServerPassword { get; set; }
        private bool? UseSSL { get; set; }

        #region IMailService Members

        public void Send(string subject, string body, string toAddress)
        {
            Send(subject, body, toAddress, new string[0]);
        }

        public void Send(string subject, string body, string toAddress, string[] attachments)
        {
            if (FromAddress == null)
            {
                FromAddress = ConfigurationManager.AppSettings[WellKnownKeys.MailService_FromAddress.ToString()];
            }

            if (FromName == null)
            {
                FromName = ConfigurationManager.AppSettings[WellKnownKeys.MailService_FromName.ToString()];
            }

            Send(subject, body, toAddress, FromAddress, FromName, attachments);
        }

        public void Send(string subject, string body, string toAddress, string fromAddress, string fromName)
        {
            Send(subject, body, toAddress, fromAddress, fromName, new string[0]);
        }

        public void Send(string subject, string body, string toAddress, string fromAddress, string fromName, string[] attachments)
        {
            SMTPConnectionInfo info = new SMTPConnectionInfo();
            if (!ServerPort.HasValue)
            {
                int serverPort = 25;
                int.TryParse(ConfigurationManager.AppSettings[WellKnownKeys.MailService_ServerPort.ToString()], out serverPort);
                info.SMTPPort = serverPort;
                ServerPort = serverPort;
            }

            if (ServerHost == null)
            {
                info.SMTPHost = ServerHost = ConfigurationManager.AppSettings[WellKnownKeys.MailService_ServerHost.ToString()];
            }

            if (!UseSSL.HasValue)
            {
                bool useSSL = false;
                bool.TryParse(ConfigurationManager.AppSettings[WellKnownKeys.MailService_UseSSL.ToString()], out useSSL);

                UseSSL = useSSL;
                info.SMTPUseSSL = useSSL;
            }

            if (ServerUsername == null)
            {
                info.SMTPUser = ServerUsername = ConfigurationManager.AppSettings[WellKnownKeys.MailService_ServerUsername.ToString()];
            }

            if (ServerPassword == null)
            {
                info.SMTPPassword = ServerPassword = ConfigurationManager.AppSettings[WellKnownKeys.MailService_ServerPassword.ToString()];
            }

            Send(subject, body, toAddress, fromAddress, fromName, attachments, info);

        }

        public void Send(string subject, string body, string toAddress, string fromAddress, string fromName, string[] attachments, SMTPConnectionInfo info)
        {
            MailMessage message = null;

            try
            {
                SmtpClient client = new SmtpClient(info.SMTPHost, info.SMTPPort);

                client.EnableSsl = info.SMTPUseSSL;

                MailAddress from = new MailAddress(fromAddress, fromName);
                MailAddress to = new MailAddress(toAddress);
                message = new MailMessage(from, to);

                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Body = body;
                message.IsBodyHtml = true;

                message.SubjectEncoding = System.Text.Encoding.UTF8;
                message.Subject = subject;

                foreach (string attachFilePath in attachments)
                {
                    if (File.Exists(attachFilePath))
                    {
                        Attachment attach = new Attachment(attachFilePath);
                        message.Attachments.Add(attach);
                    }
                }

                NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(info.SMTPUser, info.SMTPPassword);
                client.UseDefaultCredentials = false;
                client.Credentials = SMTPUserInfo;

                client.Send(message);
            }
            catch (Exception ex)
            {
                throw new MailerException("Could not send SMTP message", ex);
            }
            finally
            {
                if (message != null)
                {
                    message.Dispose();
                }
            }
        }

        #endregion
    }
}

