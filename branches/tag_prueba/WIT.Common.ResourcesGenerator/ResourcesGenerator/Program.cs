using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Linq;
using System.IO;
using System.Resources;

namespace ResourcesGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string importerConnection = ConfigurationManager.AppSettings["importResourcesExcelConnectionString"];
            ResourcesImporter importer = new ResourcesImporter(importerConnection);
            Resource newResource = importer.Import();

            //Build XML File
            BuildXml(newResource);
        }

        private static void BuildXml(Resource resource)
        {
            XDocument xmlFile = new XDocument();
            xmlFile = BuildXmlResourceHeader(xmlFile);
            
            foreach (KeyValuePair<string,string> res in resource.Resources)
            {
                xmlFile.Root.Add(new XElement("data",
                                    new XAttribute("name",res.Key),
                                    new XAttribute(XNamespace.Xml + "space","preserve"),
                                    new XElement("value",res.Value))
                           );
            }
            
            string destination = ConfigurationManager.AppSettings["importResourceExcelDestination"];
            string resourcesType = ConfigurationManager.AppSettings["resourcesType"];
            
            if (!String.IsNullOrEmpty(destination) && !String.IsNullOrEmpty(resourcesType))
            {
                destination = destination.Replace("{ResourcesType}", resourcesType)
                                         .Replace("{Culture}", resource.Type);
            }

            xmlFile.Save(destination);
        }

        private static XDocument BuildXmlResourceHeader(XDocument xmlFile)
        {
            xmlFile.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            xmlFile.Add(new XElement("root"));

            xmlFile.Root.Add(new XElement("resheader",
                                          new XAttribute("name","resmimetype"),
                                          new XElement("value","text/microsoft-resx")
                                         )
                            );

            xmlFile.Root.Add(new XElement("resheader",
                                          new XAttribute("name", "version"),
                                          new XElement("value", "2.0")
                                         )
                            );

            xmlFile.Root.Add(new XElement("resheader",
                                          new XAttribute("name", "reader"),
                                          new XElement("value", "System.Resources.ResXResourceReader, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
                                         )
                            );

            xmlFile.Root.Add(new XElement("resheader",
                                          new XAttribute("name", "writer"),
                                          new XElement("value", "System.Resources.ResXResourceWriter, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
                                         )
                            );

            return xmlFile;
        }
    }
}