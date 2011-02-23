using System;
using System.Text;
using System.Web;

namespace WIT.Common.WebHelper
{
    /// <summary>
    /// Helper for easier HttpContext and Session usage.
    /// </summary>
    public class WebHelper
    {
        /// <summary>
        /// Private instance for singleton pattern implementation.
        /// </summary>
        private static readonly WebHelper _instance = new WebHelper();

        /// <summary>
        /// Returns an instance of WebHelper class.
        /// </summary>
        public static WebHelper Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Private constructor for singleton pattern implementation.
        /// </summary>
        private WebHelper() { }

        /// <summary>
        /// Returns the base URL of the current web application.
        /// </summary>
        public string BaseURL
        {
            get
            {
                StringBuilder baseURLBuider = new StringBuilder();
                HttpRequest request = HttpContext.Current.Request;

                string scheme = request.Url.Scheme;
                int port = request.Url.Port;
                string applicationPath = request.ApplicationPath;

                baseURLBuider.Append(scheme);
                baseURLBuider.Append("://");
                baseURLBuider.Append(request.Url.DnsSafeHost);

                if ((scheme.Equals("http") && port != 80) || (scheme.Equals("https") && port != 443))
                {
                    baseURLBuider.Append(":");
                    baseURLBuider.Append(port);
                }

                baseURLBuider.Append(applicationPath);
                if (!applicationPath.Equals("/"))
                {
                    baseURLBuider.Append("/");
                }
                
                return baseURLBuider.ToString();
            }
        }

        public string GetFullURL(string appRelativeURL)
        {
            if (appRelativeURL.StartsWith("~"))
            {
                appRelativeURL = appRelativeURL.Remove(0, 1);
            }
            if (appRelativeURL.StartsWith("/"))
            {
                appRelativeURL = appRelativeURL.Remove(0, 1);
            }

            return BaseURL + appRelativeURL;
        }
    }
}
