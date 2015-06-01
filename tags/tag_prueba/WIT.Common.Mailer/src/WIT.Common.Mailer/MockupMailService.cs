namespace WIT.Common.Mailer
{
    public class MockupMailService : IMailService
    {
        #region IMailService Members

        public void Send(string subject, string body, string toAddress)
        {
            
        }

        public void Send(string subject, string body, string toAddress, string[] attachments)
        {
            
        }

        public void Send(string subject, string body, string toAddress, string fromAddress, string fromName)
        {
            
        }

        public void Send(string subject, string body, string toAddress, string fromAddress, string fromName, string[] attachments)
        {
            
        }

        public void Send(string subject, string body, string toAddress, string cc, string bcc, string fromAddress, string fromName, string[] attachments, SMTPConnectionInfo smtpInfo)
        {
        }

        #endregion
    }
}
