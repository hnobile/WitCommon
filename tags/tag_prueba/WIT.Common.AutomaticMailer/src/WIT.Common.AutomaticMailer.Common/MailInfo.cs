using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WIT.Common.AutomaticMailer.Common
{
    [Serializable]
    public class MailInfo
    {
        public string Subbject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string[] Attachments = new string[] { };

        public int SMTPPort { get; set; }
        public string SMTPHost { get; set; }
        public bool SMTPUseSSL { get; set; }
        public string SMTPUser { get; set; }
        public string SMTPPassword { get; set; }
    }
}
