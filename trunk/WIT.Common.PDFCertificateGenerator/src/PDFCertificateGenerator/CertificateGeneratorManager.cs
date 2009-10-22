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
        private string fileName;
        public CertificateGeneratorManager(string outputPath, string templateFile)
        {
            SetParameters(outputPath, templateFile, Guid.NewGuid().ToString());
        }

        public CertificateGeneratorManager(string outputPath, string templateFile, string fileName)
        {
            SetParameters(outputPath, templateFile, fileName);
        }

        private void SetParameters(string outputPath, string templateFile, string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                this.fileName = fileName;
            }
            else
            {
                throw new ArgumentException();
            }
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
            string file = this.outputPath + @"\" + fileName + ".pdf";

            Image i = Image.GetInstance(this.templateFile);
            float height = i.Height;
            float width = i.Width;
            
            Document d = new Document(new Rectangle(width, height));
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
            
            i.SetAbsolutePosition(0, 0);
            d.Add(i);

            d.Close();

            return file;
        }
    }
}
