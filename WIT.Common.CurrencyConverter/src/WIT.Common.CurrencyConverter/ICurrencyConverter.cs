using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WIT.Common.CurrencyConverter
{
    [ServiceContract]
    public interface ICurrencyConverter
    {
        [OperationContract]
        [WebGet(UriTemplate = "/GetCurrencyConversionRate?currencyCode={currencyCode}")]
        float GetCurrencyConversionRate(string currencyCode);
    }
}
