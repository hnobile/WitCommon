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
    public class MailVariable : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key{
            get
            {
                return (string)this["key"];
            }
        }
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
                return (string)this["value"];
            }
        }
    }
}
