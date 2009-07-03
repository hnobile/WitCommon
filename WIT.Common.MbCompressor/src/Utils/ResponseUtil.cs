///////////////////////////////////////////////////////////////////////
//                             ResponseUtil                          //
//             Written by: Miron Abramson. Date: 04-10-07            //
//                    Last updated: 05-05-2008                       //
///////////////////////////////////////////////////////////////////////

using System;
using System.Web;
using System.IO;

namespace Miron.Web.MbCompression
{
    internal sealed class ResponseUtil
    {
        private ResponseUtil() { }

        /// <summary>
        /// Adds the specified encoding to the response header.
        /// </summary>
        /// <param name="encoding"></param>
        internal static void SetEncodingType(HttpResponse response, string encoding)
        {
            if (response != null)
                response.AppendHeader("Content-encoding", encoding);
        }


        /// <summary>
        /// Set the response cache headers for WebResource
        /// </summary>
        /// <param name="cache"></param>
        internal static void SetCachingHeadersForWebResource(HttpCachePolicy cache, int etag)
        {
            cache.SetCacheability(HttpCacheability.Public);
            cache.VaryByParams["d"] = true;
            cache.SetOmitVaryStar(true);
            cache.SetExpires(DateTime.Now + TimeSpan.FromDays(Settings.Instance.DaysInCache));
            cache.SetValidUntilExpires(true);
            cache.VaryByHeaders["Accept-Encoding"] = true;
            cache.SetETag(string.Concat("\"", etag, "\""));
        }

        /// <summary>
        /// Set the response headers to cache the content for file (JS or CSS)
        /// </summary>
        internal static void SetHeadersForFile(string file, HttpContext context, string contentType, string cacheKey)
        {
            if (context == null || context.Response == null)
                return;

            HttpCachePolicy cachePolicy = context.Response.Cache;

            if (cachePolicy != null && !string.IsNullOrEmpty(file) && !string.IsNullOrEmpty(contentType))
            {

                context.Response.ContentType = contentType;
                context.Response.AddFileDependency(file);

                cachePolicy.VaryByHeaders["Accept-Encoding"] = true;
                cachePolicy.SetOmitVaryStar(true);

                string dateCacheKey = string.Concat(cacheKey, Constants.DATE);
                DateTime lastModified;

                try
                {
                    lastModified = context.Cache[dateCacheKey] != null ? (DateTime)context.Cache[dateCacheKey] : File.GetLastWriteTime(file);
                }
                catch (ArgumentException) { lastModified = DateTime.Now; }
                catch (FormatException) { lastModified = DateTime.Now; }

                cachePolicy.SetLastModified(lastModified);

                cachePolicy.SetExpires(DateTime.Now + TimeSpan.FromDays(Settings.Instance.DaysInCache));
                cachePolicy.SetValidUntilExpires(true);

                cachePolicy.SetCacheability(HttpCacheability.Public);
                cachePolicy.SetRevalidation(HttpCacheRevalidation.AllCaches);

                string etag = string.Concat("\"", lastModified.GetHashCode(), "\"");
                cachePolicy.SetETag(etag);
            }
        }
    }
}
