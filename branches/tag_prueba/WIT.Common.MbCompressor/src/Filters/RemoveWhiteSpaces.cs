///////////////////////////////////////////////////////////////////////
//                  RemoveWhiteSpacesFilterStream                    //
//             Written by: Miron Abramson. Date: 5-5-08              //
//         Remove uneeded white spaces from the response html        //
//                    Last updated: 05-05-2008                       //
///////////////////////////////////////////////////////////////////////

#region Using
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
#endregion

namespace Miron.Web.MbCompression.Filters
{
    internal class RemoveWhiteSpacesFilterStream : Stream
    {
        private static readonly Regex REGEX_BETWEEN_TAGS = new Regex(@">\s+<", RegexOptions.Compiled);
        private static readonly Regex REGEX_LINE_BREAKS = new Regex(@"\n\s+", RegexOptions.Compiled);

        #region Stream filter

        public RemoveWhiteSpacesFilterStream(Stream originalStream)
        {
            _originalStream = originalStream;
        }

        private Stream _originalStream;

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
            _originalStream.Flush();
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

        #region Override Methods

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _originalStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _originalStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _originalStream.SetLength(value);
        }

        public override void Close()
        {
            _originalStream.Close();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            string html = System.Text.Encoding.Default.GetString(buffer, offset, count);
            html = OptimizeHtml(html);
            byte[] outdata = System.Text.Encoding.Default.GetBytes(html);
            _originalStream.Write(outdata, 0, outdata.Length);
        }

        #endregion

        #region Private static methods
        private static string OptimizeHtml(string html)
        {
            html = REGEX_BETWEEN_TAGS.Replace(html, "> <");
            html = REGEX_LINE_BREAKS.Replace(html, string.Empty);
            return html.Trim();
        }
        #endregion

        #endregion
    }
}
