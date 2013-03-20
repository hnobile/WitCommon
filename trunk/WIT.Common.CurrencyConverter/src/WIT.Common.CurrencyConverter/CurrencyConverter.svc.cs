using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel.Activation;
//using System.Xml;
using System.Xml.Linq;
using WIT.Common.CurrencyConverter.Logic;

namespace WIT.Common.CurrencyConverter
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class CurrencyConverter : ICurrencyConverter
    {
        public float GetCurrencyConversionRate(string currencyCode)
        {
            XElement currencies = XElement.Load(CurrencyConverterConfiguration.RatesSourceFile);
            
            float rate = 0;
            if (!string.IsNullOrEmpty(currencyCode))
            {
                rate = float.Parse((from c in currencies.Elements("currency")
                                    where c.Attribute("code").Value == currencyCode
                                    select c.Element("rate").Value).First());
            }

            return rate;
        }
    }
}
