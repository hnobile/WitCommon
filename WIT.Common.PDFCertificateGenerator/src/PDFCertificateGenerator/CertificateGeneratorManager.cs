using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WIT.Common.PDFCertificateGenerator
{
    public class CertificateGeneratorManager
    {
        private byte[] templateBytes;
        public CertificateGeneratorManager(byte[] templateBytes)
        {
            if (templateBytes.Length == 0)
                throw new ArgumentNullException();
            
            this.templateBytes = templateBytes;
        }

        public byte[] Generate(IList<CertificatePage> pages)
        {
            Image i = Image.GetInstance(templateBytes);
            float height = i.Height;
            float width = i.Width;

            Document d = new Document(new Rectangle(width, height));
            MemoryStream mStream = new MemoryStream();

            PdfWriter w = PdfWriter.GetInstance(d, mStream);
            d.Open();
            w.Open();

            PdfContentByte cb = w.DirectContent;

            for (int x = 0; x < pages.Count;x++)
            {
                CertificatePage cp = pages[x];
                cb.BeginText();
                foreach (CertificateElement ce in cp.CertificateElements)
                {
                    cb.SetFontAndSize(ce.Font, ce.Size);
                    cb.SetColorFill(ce.FillColor);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, ce.Text, ce.X, ce.Y, 0);
                }

                cb.EndText();

                i.SetAbsolutePosition(0, 0);
                d.Add(i);
                if (x + 1  < pages.Count)
                    d.NewPage();
            }


            d.Close();
            w.Flush();
            w.Close();

            return mStream.GetBuffer();
        }

        public byte[] Generate(List<CertificateElement> elements)
        {
            CertificatePage cp = new CertificatePage(elements);
            IList<CertificatePage> pages = new List<CertificatePage>();
            pages.Add(cp);

            return Generate(pages);
        }
    }
}
