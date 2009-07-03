///////////////////////////////////////////////////////////////////////
//                             RequestUtil                           //
//             Written by: Miron Abramson. Date: 04-10-07            //
//                    Last updated: 05-05-2008                       //
///////////////////////////////////////////////////////////////////////

using System;
using System.Web;

namespace Miron.Web.MbCompression
{
    internal sealed class RequestUtil
    {
        private RequestUtil() { }

        /// <summary>
        /// Check if the browser support compression
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static bool IsCompressionSupported(HttpContext context)
        {
            return IsCompressionSupported(context, false);
        }

        /// <summary>
        /// Check if the browser support compression
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static bool IsCompressionSupported(HttpContext context, bool isPage)
        {
            if (context == null || context.Request == null || context.Request.Browser == null)
                return false;

            if (context.Request.Headers["Accept-encoding"] == null || !(context.Request.Headers["Accept-encoding"].Contains(Constants.GZIP) || context.Request.Headers["Accept-encoding"].Contains(Constants.DEFLATE)))
                return false;

            if (!context.Request.Browser.IsBrowser(Constants.InternetExplorer))
                return true;

            if (context.Request.Browser.MajorVersion < 7 && !isPage)
                return false;

            if (context.Request.Params["SERVER_PROTOCOL"] != null && context.Request.Params["SERVER_PROTOCOL"].Contains("1.1"))
                return true;

            return false;
        }



        /// <summary>
        /// Check if specific encoding is supported
        /// </summary>
        /// <param name="context"></param>
        /// <param name="encodingType"></param>
        /// <returns></returns>
        internal static bool IsSpecificEncodingSupported(HttpContext context, string encodingType)
        {
            return context.Request.Headers["Accept-encoding"] != null
                && context.Request.Headers["Accept-encoding"].Contains(encodingType);
        }



        /// <summary>
        /// Check if the current request is an AsyncCall
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static bool IsAjaxPostBackRequest(HttpContext context)
        {
            return context.Request.Headers["X-MicrosoftAjax"] != null;
        }
    }
}
