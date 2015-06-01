///////////////////////////////////////////////////////////////////////
//                      CssCompressionHandler                        //
//            Written by: Miron Abramson. Date: 04-10-07             //
//         Reduce & Compress using GZIP or DEFLATE algorithm         //
//          & store in server & client cache css files               //
//                    Last updated: 05-05-2008                       //
///////////////////////////////////////////////////////////////////////

#region Using

using System;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.Caching;
using System.Net;

#endregion

namespace Miron.Web.MbCompression
{
    /// <summary>
    /// Remove whitespace from any css file (starting with '~')
    /// compress it (GZIP or DEFLATE), and save the css file in the server & client cache
    /// </summary>
    public class CssCompressionHandler : CompressionHandlerBase
    {

        #region // ProcessRequest - The starting point

        /// <summary>
        /// Processing of HTTP Web requests.
        /// </summary>
        /// <param name="context"></param>
        public override void ProcessRequest(HttpContext context)
        {
            if (context == null)
            {
                return;
            }
            string file = context.Server.UrlDecode(context.Request.QueryString["d"]);
            if (file != null && file.StartsWith("~", StringComparison.Ordinal))
            {
                file = context.Server.MapPath(file);

                // Check if the file is not exist
                if (!File.Exists(file))
                {
                    context.Response.ContentType = "text/css";
                    context.Response.Write(SR.GetString(SR.File_FileNotFound, file, context.Server.UrlDecode(context.Request.QueryString["d"])));
                    return;
                }

                string cacheKey;
                string preferedEncoding;

                if (Settings.Instance.CompressCSS && RequestUtil.IsCompressionSupported(context))
                {
                    preferedEncoding = RequestUtil.IsSpecificEncodingSupported(context, Constants.GZIP) ? Constants.GZIP : Constants.DEFLATE;
                    cacheKey = string.Concat(file, preferedEncoding);
                }
                else
                {
                    preferedEncoding = string.Empty;
                    cacheKey = file;
                }

                // Check if the ETag is match. If so, we don't send nothing to the client.
                CheckETag(context, cacheKey);

                // Get the file content from the cache
                byte[] fileContent = context.Cache[cacheKey] as byte[];

                // The file content is not in the cache, so load it from the phisical file
                if (fileContent == null || fileContent.Length == 0)
                {
                    fileContent = GetFileContent(file, preferedEncoding);

                    if (fileContent != null && fileContent.Length > 0)
                    {
                        context.Cache.Insert(cacheKey, fileContent, new CacheDependency(file), System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromDays(Settings.Instance.DaysInCache));
                        context.Cache.Insert(string.Concat(cacheKey + Constants.DATE), DateTime.UtcNow, new CacheDependency(file), System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromDays(Settings.Instance.DaysInCache));
                    }
                }

                #region // Set the headers and send the data to the client

                if (preferedEncoding.Length > 0)
                    ResponseUtil.SetEncodingType(context.Response, preferedEncoding);

                ResponseUtil.SetHeadersForFile(file, context, "text/css", cacheKey);
                context.Response.OutputStream.Write(fileContent, 0, fileContent.Length);

                #endregion
            }
        }

        #endregion


        #region // Private static Members

        /// <summary>
        /// Get the file content and compress it if needed
        /// </summary>
        private static byte[] GetFileContent(string file, string preferedEncoding)
        {
            if (!file.EndsWith(".css", StringComparison.OrdinalIgnoreCase))
            {
                throw new System.IO.FileLoadException("File type error");
            }

            string body = string.Empty;

            // Load the data from the file
            using (StreamReader reader = new StreamReader(file))
            {
                body = reader.ReadToEnd();
            }

            // Optimize & compress
            if (preferedEncoding.Length > 0)
            {
                body = OptimizeCSS(body);
                return CompressionUtil.Compressor(body, preferedEncoding);  // Compress the string  
            }

            // No need to compress
            return System.Text.Encoding.ASCII.GetBytes(body);
        }

        /// <summary>
        /// Optimize css file by removing any comments and unneeded spaces
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        private static string OptimizeCSS(string style)
        {
            style = style.Replace("  ", string.Empty);
            style = style.Replace(Environment.NewLine, string.Empty);
            style = style.Replace("\t", string.Empty);
            style = style.Replace(" {", "{");
            style = style.Replace(" :", ":");
            style = style.Replace(": ", ":");
            style = style.Replace(", ", ",");
            style = style.Replace("; ", ";");
            style = style.Replace(";}", "}");
            style = Regex.Replace(style, @"(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,}(?=&nbsp;)|(?<=&ndsp;)\s{2,}(?=[<])", String.Empty);

            return style;
        }
        #endregion

    }
}