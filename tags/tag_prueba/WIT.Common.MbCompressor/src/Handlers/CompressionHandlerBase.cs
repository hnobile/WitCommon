///////////////////////////////////////////////////////////////////////
//                     CompressionHandlerBase                         //
//             Written by: Miron Abramson. Date: 04-10-07            //
//                 Base class for all HttpHandlers.                  //
//            All httpHandlers will inherit from this class.         //
//         The class holds common methods for all httpHandlers       //
//                    Last updated: 05-05-2008                       //
///////////////////////////////////////////////////////////////////////

#region Using

using System;
using System.Web;
using System.IO;
using System.Web.Caching;
using System.Net;

#endregion

namespace Miron.Web.MbCompression
{
    public abstract class CompressionHandlerBase : IHttpHandler
    {

        #region // IHttpHandler Members
        /// <summary>
        /// Must be in the class because it implement the interface IHttpHandler.
        /// </summary>
        /// <param name="context"></param>
        virtual public void ProcessRequest(HttpContext context)
        {
            // Need to be override in the inhiritate class...
        }

        /// <summary>
        /// (Must be implement by any class that implement the interface IHttpHandler)
        /// </summary>
        virtual public bool IsReusable
        {
            get { return false; }
        }
        #endregion


        #region // Protected static methods


        /// <summary>
        /// Check if the ETag that sent from the client is match to the current ETag.
        /// If so, set the status code to 'Not Modified' and stop the response.
        /// </summary>
        protected static void CheckETag(HttpContext context, string cacheKey)
        {
            if (context == null)
            {
                return;
            }
            string dateCacheKey = string.Concat(cacheKey, Constants.DATE);
            DateTime lastModified = context.Cache[dateCacheKey] != null ? (DateTime)context.Cache[dateCacheKey] : DateTime.Now;
            string etag = string.Concat("\"", lastModified.GetHashCode(), "\"");
            string incomingEtag = context.Request.Headers["If-None-Match"];

            if (String.Equals(incomingEtag, etag, StringComparison.Ordinal))
            {
                context.Response.Cache.SetETag(etag);
                context.Response.AppendHeader("Content-Length", "0");
                context.Response.StatusCode = (int)HttpStatusCode.NotModified;
                context.Response.End();
            }
        }

        #endregion

    }
}
