using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WIT.Common.AutomaticMailer.Common;
using System.Xml.Linq;
using System.Configuration;

namespace WIT.Common.AutomaticMailer.Sender.DAO
{
    public class LocalXMLSchedulableMailerDAO: ISchedulableMailerDAO
    {
        #region ISchedulableMailerDAO Members
        private string configPath = ConfigurationManager.AppSettings[WellKnownKeys.LocalXMLScheduledMailersConfigurationPath];
        public List<SchedulableMailerInfo> GetAll()
        {
            List<SchedulableMailerInfo> resp = new List<SchedulableMailerInfo>();

            XDocument config = XDocument.Load(configPath);

            var configElements = from serviceInfo in config.Descendants("SchedulableMailerInfo")
                                 select new
                                 {
                                     Name = serviceInfo.Attribute("name").Value,
                                     BaseFolder = serviceInfo.Attribute("baseFolder").Value,
                                     ConfigFileName = serviceInfo.Attribute("configFileName").Value,
                                     AssemblyName = serviceInfo.Attribute("assemblyName").Value,
                                     TypeName = serviceInfo.Attribute("typeName").Value,
                                     LastExecutionDate = serviceInfo.Attribute("lastExecution").Value,
                                     ExecutionInterval = serviceInfo.Attribute("executionIntervalMinutes").Value
                                 };

            foreach (var si in configElements)
            {
                SchedulableMailerInfo smi = new SchedulableMailerInfo();
                smi.Name = si.Name;
                smi.BaseFolder = si.BaseFolder;
                smi.ConfigFileName = si.ConfigFileName;
                smi.AssemblyName = si.AssemblyName;
                smi.TypeName = si.TypeName;
                smi.ExecutionInterval = Int64.Parse(si.ExecutionInterval);
                if (String.IsNullOrEmpty(si.LastExecutionDate))
                {
                    smi.LastExecution = null;
                }
                else
                {
                    smi.LastExecution = DateTime.Parse(si.LastExecutionDate);
                }
                resp.Add(smi);
            }
            return resp;
        }

        public void SaveMailerState(SchedulableMailerInfo mailerInfo)
        {
            XDocument config = XDocument.Load(configPath);
            IEnumerable<XElement> ssi = from b in config.Descendants("SchedulableMailerInfo")
                                        where b.Attribute("name").Value.Equals(mailerInfo.Name)
                                        select b;

            foreach (XElement xe in ssi)
            {
                xe.SetAttributeValue("lastExecution", mailerInfo.LastExecution.ToString());
            }
            config.Save(configPath);
        }

        #endregion
    }
}
