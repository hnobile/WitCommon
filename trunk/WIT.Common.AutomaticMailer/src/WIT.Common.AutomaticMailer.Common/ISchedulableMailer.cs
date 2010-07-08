using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WIT.Common.AutomaticMailer.Common
{
    public interface ISchedulableMailer
    {
        List<MailInfo> GetMailingList();
    }
}
