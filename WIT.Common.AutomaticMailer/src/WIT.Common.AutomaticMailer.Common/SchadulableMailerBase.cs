using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WIT.Common.AutomaticMailer.Common
{
    [Serializable]
    public abstract class SchadulableMailerBase: MarshalByRefObject, ISchedulableMailer
    {

        #region ISchedulableMailer Members

        public abstract List<MailInfo> GetMailingList();

        #endregion
    }
}
