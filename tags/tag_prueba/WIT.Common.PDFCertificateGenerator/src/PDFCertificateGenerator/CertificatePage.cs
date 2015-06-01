using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WIT.Common.PDFCertificateGenerator
{
    public class CertificatePage
    {
        public CertificatePage()
        {
            CertificateElements = new List<CertificateElement>();
        }

        public CertificatePage(IList<CertificateElement> elements)
        {
            CertificateElements = elements;
        }

        public void AddCertificateElement(CertificateElement element)
        {
            CertificateElements.Add(element);
        }

        public IList<CertificateElement> CertificateElements { get; set; }
    }
}
