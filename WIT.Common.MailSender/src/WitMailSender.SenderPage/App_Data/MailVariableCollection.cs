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
    public class MailVariableCollection : ConfigurationElementCollection
    {
        public MailVariable this[int index]
        {
            get
            {
                return (MailVariable)base.BaseGet(index);
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new MailVariable();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MailVariable)element).Key;
        }
    }
    //public class ASPNET2ConfigurationStateCollection : ConfigurationElementCollection
    //{
    //    public ASPNET2ConfigurationState this[int index]
    //    {
    //        get
    //        {
    //            return base.BaseGet(index) as ASPNET2ConfigurationState;
    //        }
    //        set
    //        {
    //            if (base.BaseGet(index) != null)
    //            {
    //                base.BaseRemoveAt(index);
    //            }
    //            this.BaseAdd(index, value);
    //        }
    //    }

    //    protected override ConfigurationElement CreateNewElement()
    //    {
    //        return new ASPNET2ConfigurationState();
    //    }

    //    protected override object GetElementKey(ConfigurationElement element)
    //    {
    //        return ((ASPNET2ConfigurationState)element).Name;
    //    }
    //} 
}
