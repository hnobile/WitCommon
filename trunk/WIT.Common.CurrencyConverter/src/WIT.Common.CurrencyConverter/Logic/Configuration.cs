using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace WIT.Common.CurrencyConverter.Logic
{
    public class WellKnownKeys
    {
        public static string RatesSourceFile = "RatesSourceFile";
    }

    public class CurrencyConverterConfiguration
    {
        public static string RatesSourceFile
        {
            get {
                string value = ConfigurationManager.AppSettings[WellKnownKeys.RatesSourceFile];
                if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("RateSourceFile has no value");
                }
                return value;
            }
        }
    }
}