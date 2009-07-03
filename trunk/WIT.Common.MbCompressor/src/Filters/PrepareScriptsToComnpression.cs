///////////////////////////////////////////////////////////////////////
//                PrepareScriptsToComnpressionStream                 //
//             Written by: Miron Abramson. Date: 5-5-08              //
//   Parse the response, and convert any injected javascript include //
//   to format that can be parsed and compress by the js compressor  //
//                    Last updated: 05-05-2008                       //
///////////////////////////////////////////////////////////////////////

#region Using
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Globalization;
#endregion

namespace Miron.Web.MbCompression.Filters
{
    internal class PrepareScriptsToComnpressionStream : Stream
    {
        private static readonly Regex REGEX_SCRIPT = new Regex(@"<script\s*[^<]*(src=""/)\s*[^<]*\.js(?!\.axd)\s*[^<]*""\s*[^<]*(</script>)", RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        private static readonly Regex REGEX_SCRIPT_SRC = new Regex(@"(src=""/)\s*[^<]*""", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private const string SCRIPT_TEMPLATE = "src=\"{0}\"";


        public PrepareScriptsToComnpressionStream(Stream sink)
        {
            _sink = sink;
        }

        private Stream _sink;

        #region Properites

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            _sink.Flush();
        }

        public override long Length
        {
            get { return 0; }
        }

        private long _position;
        public override long Position
        {
            get { return _position; }
            set { _position = value; }
        }

        #endregion

        #region Methods

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _sink.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _sink.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _sink.SetLength(value);
        }

        public override void Close()
        {
            _sink.Close();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {

            string html = System.Text.Encoding.Default.GetString(buffer);

            html = FixScriptsUrl(html);

            byte[] outdata = System.Text.Encoding.Default.GetBytes(html);

            _sink.Write(outdata, 0, outdata.Length);
        }

        /// <summary>
        /// Fix the scripts url that it can be compressed
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static string FixScriptsUrl(string html)
        {
            return REGEX_SCRIPT.Replace(html, new System.Text.RegularExpressions.MatchEvaluator(Found));
        }

        /// <summary>
        /// Find the source
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static string Found(Match m)
        {
            return REGEX_SCRIPT_SRC.Replace(m.Value, new MatchEvaluator(Replace));
        }

        /// <summary>
        /// Replace the source url
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static string Replace(Match m)
        {
            // This condition should never be true because the REGEX_SCRIPT, but I put it here anyway, to make sure.
            if (m.Value.Contains(".axd"))
            {
                return m.Value;
            }
            string path = VirtualPathUtility.ToAppRelative(m.Value.Substring(5, m.Value.Length - 6));
            if (path[0].Equals('~'))
            {
                path = "jslib.axd?d=" + HttpContext.Current.Server.UrlEncode(path);
            }
            else
            {
                path = "jslib.axd?d=" + HttpContext.Current.Server.UrlEncode("~" + path);
            }
            return string.Format(CultureInfo.InvariantCulture, SCRIPT_TEMPLATE, path);
        }

        #endregion
    }
}
