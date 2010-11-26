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
        /// Private cache for base URL.
        /// </summary>
        private string _BaseURL = null;

        /// <summary>
        /// Returns the base URL of the current web application.
        /// </summary>
        public string BaseURL
        {
            get
            {
                if (_BaseURL == null)
                {
                    StringBuilder baseURLBuider = new StringBuilder();
                    HttpRequest request = HttpContext.Current.Request;

                    baseURLBuider.Append(request.Url.Scheme);
                    baseURLBuider.Append("://");
                    baseURLBuider.Append(request.Url.DnsSafeHost);

                    int port = request.Url.Port;
                    if (port != 80)
                    {
                        baseURLBuider.Append(":");
                        baseURLBuider.Append(port);
                    }

                    string applicationPath = request.ApplicationPath;
                    baseURLBuider.Append(applicationPath);
                    if (!applicationPath.Equals("/"))
                    {
                        baseURLBuider.Append("/");
                    }

                    _BaseURL = baseURLBuider.ToString();
                }

                return _BaseURL;
            }
        }

        public string GetFullURL(string appRelativeURL)
        {
            return BaseURL + appRelativeURL;
        }
    }
}
