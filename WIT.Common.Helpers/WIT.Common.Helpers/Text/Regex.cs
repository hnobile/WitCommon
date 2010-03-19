using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WIT.Common.Helpers.Text
{
    public class RegexHelper
    {
        /// <summary>
        /// Just a Regex validation of an e-mail address. Maybe in the future it will make more sense to incorporate
        /// more advanced validations like the ones described in http://www.codeproject.com/KB/validation/Valid_Email_Addresses.aspx
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <returns></returns>
        public static bool isValidEmail(string inputEmail)
        {
            if (String.IsNullOrEmpty(inputEmail))
            {
                return false;
            }

            string strRegex = @"^([a-zA-Z0-9_\-\.\+]+)@((\[[0-9]{1,3}" +
                              @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                              @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";


            Regex re = new Regex(strRegex);
            return re.IsMatch(inputEmail);
        }
    }
}
