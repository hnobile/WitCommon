using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.Configuration;

namespace WIT.Common.CurrencyConverter.Client
{
    public class CurrencyConverterClient
    {
        private static ICurrencyConverter client = null;

        public static ICurrencyConverter GetClient()
        {
            if (CurrencyConverterClient.client == null)
            {
                WebChannelFactory<ICurrencyConverter> factory = new WebChannelFactory<ICurrencyConverter>(new WebHttpBinding(), new Uri(ConfigurationManager.AppSettings["ServiceURL"]));
                CurrencyConverterClient.client = factory.CreateChannel();
            }
            return CurrencyConverterClient.client;
        }
    }
}
