using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WIT.Common.Mailer
{
    public class SMTPConnectionInfo
    {
        public int SMTPPort { get; set; }
        public string SMTPHost { get; set; }
        public bool SMTPUseSSL { get; set; }
        public string SMTPUser { get; set; }
        public string SMTPPassword { get; set; }
    }
}
