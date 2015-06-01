using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace WitMailSender.SenderPage
{
    public class MailVariableSection : ConfigurationSection
    {
        [ConfigurationProperty("variables", IsRequired = true)]
        public MailVariableCollection Variables
        {
            get
            {
                return (MailVariableCollection)this["variables"];
            }
        }
    }
}
