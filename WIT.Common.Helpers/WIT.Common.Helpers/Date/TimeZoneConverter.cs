using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WIT.Common.Helpers.Date
{
    public class TimeZoneConverter
    {
        public static DateTime ConvertDateTimeToTimeZone(DateTime date, string idOriginTimeZone, string idDestinationTimeZone)
        {
            try
            {
                return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, idOriginTimeZone, idDestinationTimeZone);
            }
            catch (TimeZoneNotFoundException)
            {
                return DateTime.MinValue;
            }
        }
        
        public static DateTime ConvertDateTimeToStandardTimeZone(DateTime date, string idOriginTimeZone, string idStandardTimeZone)
        {

            return ConvertDateTimeToTimeZone(date, idOriginTimeZone, idStandardTimeZone);
        }
        
        public static DateTime ConvertDateTimeFromStandardTimeZone
            (DateTime date, string idStandardTimeZone, string idDestinationTimeZone)
        {
            return ConvertDateTimeToTimeZone(date, idStandardTimeZone, idDestinationTimeZone);
        }
    }
}
