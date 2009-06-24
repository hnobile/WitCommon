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
        private string outputPath;
        private string templateFile;

        public CertificateGeneratorManager(string outputPath, string templateFile)
        {
            if (Directory.Exists(outputPath))
            {
                this.outputPath = outputPath;
            }
            else
            {
                throw new DirectoryNotFoundException();
            }

            if (File.Exists(templateFile))
            {
                this.templateFile = templateFile;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public string Generate(List<CertificateElement> elements)
        {
            string file = this.outputPath + @"\" + Guid.NewGuid().ToString() + ".pdf";
            Document d = new Document(new Rectangle(1661, 1183));
            PdfWriter w = PdfWriter.GetInstance(d, new FileStream(file, FileMode.Create));
            d.Open();
            w.Open();

            PdfContentByte cb = w.DirectContent;

            cb.BeginText();

            foreach (CertificateElement ce in elements)
            {
                cb.SetFontAndSize(ce.Font, ce.Size);
                cb.SetColorFill(ce.FillColor);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, ce.Text, ce.X, ce.Y, 0);
            }

            cb.EndText();

            Image i = Image.GetInstance(this.templateFile);
            i.SetAbsolutePosition(0, 0);
            d.Add(i);

            d.Close();

            return file;
        }
    }
}
