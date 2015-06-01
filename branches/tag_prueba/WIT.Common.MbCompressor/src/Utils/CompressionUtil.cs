///////////////////////////////////////////////////////////////////////
//                           CompressionUtil                         //
//             Written by: Miron Abramson. Date: 04-10-07            //
//                    Last updated: 05-05-2008                       //
///////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Collections.Specialized;

namespace Miron.Web.MbCompression
{
    public sealed class CompressionUtil
    {
        private CompressionUtil() { }

        private static readonly Object _getMethodLock = new Object();
        private static StringDictionary __compressibleTypes;

        private static StringDictionary CompressibleTypes
        {
            get
            {
                if (__compressibleTypes == null)
                {
                    lock (_getMethodLock)
                    {
                        if (__compressibleTypes == null)
                        {
                            __compressibleTypes = new StringDictionary();
                            __compressibleTypes.Add("text/css", null);
                            __compressibleTypes.Add("application/x-javascript", null);
                            __compressibleTypes.Add("text/javascript", null);
                            __compressibleTypes.Add("text/html", null);
                            __compressibleTypes.Add("text/plain", null);
                        }
                    }
                }
                return __compressibleTypes;
            }
        }


        /// <summary>
        /// Compress a given string using a given algorithm
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encodingType"></param>
        /// <returns></returns>
        public static byte[] Compressor(string input, string encodingType)
        {
            if (input == null)
                return null;
            return Compressor(Encoding.ASCII.GetBytes(input), encodingType);
        }

        /// <summary>
        /// Compress a given byte[] using a given algorithm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] Compressor(byte[] buffer, string encodingType)
        {
            if (buffer != null)
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    Stream compress = null;

                    // Choose the compression type and make the compression
                    if (String.Equals(encodingType, Constants.GZIP, StringComparison.Ordinal))
                    {
                        compress = new GZipStream(memStream, CompressionMode.Compress);
                    }
                    else if (String.Equals(encodingType, Constants.DEFLATE, StringComparison.Ordinal))
                    {
                        compress = new DeflateStream(memStream, CompressionMode.Compress);
                    }
                    else
                    {
                        // No compression
                        return buffer;
                    }

                    compress.Write(buffer, 0, buffer.Length);
                    compress.Dispose();

                    return memStream.ToArray();
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Check if a specific content type is compressible
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static bool IsContentTypeCompressible(string contentType)
        {
            return CompressibleTypes.ContainsKey(contentType);
        }
    }
}
