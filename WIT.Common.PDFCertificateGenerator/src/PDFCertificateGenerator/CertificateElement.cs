using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace WIT.Common.PDFCertificateGenerator
{
    public class CertificateElement
    {
        public string Text { get; set; }

        public BaseFont Font { get; set; }
        
        public Color FillColor { get; set; }

        public int Size { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public CertificateElement(string text, float x, float y)
        {
            Font = BaseFont.CreateFont(BaseFont.HELVETICA_BOLDOBLIQUE, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Size = 30;
            FillColor = Color.BLACK;
            Text = text;
            X = x;
            Y = y;
        }
    }
}
