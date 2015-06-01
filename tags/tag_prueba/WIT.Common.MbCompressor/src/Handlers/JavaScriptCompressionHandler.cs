///////////////////////////////////////////////////////////////////////
//                    JavaScriptCompressionHandler                   //
//            Written by: Miron Abramson. Date: 04-10-07              //
//             Compress using GZIP or DEFLATE algorithm              //
//          & store in server & client cache js files                //
//                    Last updated: 05-05-2008                       //
///////////////////////////////////////////////////////////////////////

#region Using

using System;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.Caching;

#endregion

namespace Miron.Web.MbCompression
{
    /// <summary>
    /// Compress (GZIP or DEFLATE) any Javascript file (starting with '~'),
    /// and save the javascript file in the server & client cache
    /// </summary>
    public class JavaScriptCompressionHandler : CompressionHandlerBase
    {

        #region // ProcessRequest - Starting point

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
            if (file != null && file.StartsWith("~",StringComparison.Ordinal))
            {
                file = context.Server.MapPath(file);

                // Check if the file is not exist
                if (!File.Exists(file))
                {
                    context.Response.ContentType = "application/x-javascript";
                    context.Response.Write(SR.GetString(SR.File_FileNotFound, file, context.Server.UrlDecode(context.Request.QueryString["d"])));
                    return;
                }

                string cacheKey;
                string preferedEncoding;

                if (Settings.Instance.CompressJavaScript && RequestUtil.IsCompressionSupported(context))
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
                        context.Cache.Insert(cacheKey + Constants.DATE, DateTime.UtcNow, new CacheDependency(file), System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromDays(Settings.Instance.DaysInCache));
                    }
                }

                #region // Set the headers and send the data to the client

                if (preferedEncoding.Length > 0)
                    ResponseUtil.SetEncodingType(context.Response, preferedEncoding);

                ResponseUtil.SetHeadersForFile(file, context, "application/x-javascript", cacheKey);
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
            if (!file.EndsWith(".js", StringComparison.OrdinalIgnoreCase))
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
                body = OptimizeJS(body);
                return CompressionUtil.Compressor(body, preferedEncoding);  // Compress the string  
            }

            // No need to compress
            return System.Text.Encoding.ASCII.GetBytes(body);
        }


        /// <summary>
        /// Optimize Javascript file by removing any comments and unneeded spaces.
        /// The code for this method was taken from the source of the project BlogEngine.NET
        /// http://www.codeplex.com/blogengine
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        private static string OptimizeJS(string script)
        {
            string[] lines = script.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                string s = lines[i].Trim();
                if (s.Length > 0 && !s.StartsWith("//", StringComparison.Ordinal))
                {
                    sb.AppendLine(s.Trim());
                }
            }
            
            script = sb.ToString();
            script = Regex.Replace(script, @"^[\s]+|[ \f\r\t\v]+$", string.Empty);
            script = Regex.Replace(script, @"([+-])\n\1", "$1 $1");
            script = Regex.Replace(script, @"([^+-][+-])\n", "$1");
            script = Regex.Replace(script, @"([^+]) ?(\+)", "$1$2");
            script = Regex.Replace(script, @"(\+) ?([^+])", "$1$2");
            script = Regex.Replace(script, @"([^-]) ?(\-)", "$1$2");
            script = Regex.Replace(script, @"(\-) ?([^-])", "$1$2");
            script = Regex.Replace(script, @"\n([{}()[\],<>/*%&|^!~?:=.;+-])", "$1");
            script = Regex.Replace(script, @"(\W(if|while|for)\([^{]*?\))\n", "$1");
            script = Regex.Replace(script, @"(\W(if|while|for)\([^{]*?\))((if|while|for)\([^{]*?\))\n", "$1$3");
            script = Regex.Replace(script, @"([;}]else)\n", "$1 ");
            script = Regex.Replace(script, @"(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,}(?=&nbsp;)|(?<=&ndsp;)\s{2,}(?=[<])", string.Empty);

            return script;
        }

        #endregion


    }
}