using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;

namespace WIT.Common.PDFCertificateGenerator
{
    public class CertificateElementAlign
    {
        public enum AlignType
        {
            ALIGN_RIGHT = 0,
            ALIGN_LEFT = 1,
            ALIGN_CENTER = 2,
            ALIGN_CENTER_DOCUMENT = 3
        }

        public AlignType Type { get; set; }

        public CertificateElementAlign(AlignType t)
        {
            this.Type = t;
        }

        public int GetPdfContentTypeAlign()
        {
            switch (Type)
            {
                case AlignType.ALIGN_RIGHT:
                    return PdfContentByte.ALIGN_RIGHT;
                case AlignType.ALIGN_LEFT:
                    return PdfContentByte.ALIGN_LEFT;
                case AlignType.ALIGN_CENTER:
                    return PdfContentByte.ALIGN_CENTER;
                case AlignType.ALIGN_CENTER_DOCUMENT:
                    return PdfContentByte.ALIGN_CENTER;
                default:
                    return PdfContentByte.ALIGN_LEFT;
            }
        }

        public float GetX(float x, float documentWidth)
        {
            if (Type == AlignType.ALIGN_CENTER_DOCUMENT)
            {
                return (documentWidth / 2) + x;
            }
            return x;
        }
    }
}
