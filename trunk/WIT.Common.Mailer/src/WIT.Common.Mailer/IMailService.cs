namespace WIT.Common.Mailer
{
    /// <summary>
    /// Interface to be implemented by all mailing services
    /// </summary>
    public interface IMailService
    {
        void Send(string subject, string body, string toAddress);
        void Send(string subject, string body, string toAddress, string[] attachments);
        void Send(string subject, string body, string toAddress, string fromAddress, string fromName);
        void Send(string subject, string body, string toAddress, string fromAddress, string fromName, string[] attachments);
        void Send(string subject, string body, string toAddress, string cc, string bcc, string fromAddress, string fromName, string[] attachments, SMTPConnectionInfo smtpInfo);
    }
}
