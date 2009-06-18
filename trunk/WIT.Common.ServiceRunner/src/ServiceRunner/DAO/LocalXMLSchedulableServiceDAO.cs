using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Configuration;

namespace WIT.Common.ServiceRunner.DAO
{
    public class LocalXMLSchedulableServiceDAO : ISchedulableServiceDAO
    {
        #region ISchedulableServiceDAO Members
        private string configPath = ConfigurationManager.AppSettings[WellKnownKeys.LocalXMLScheduledServicesConfigurationPath];
        public List<SchedulableServiceInfo> GetAll()
        {
            List<SchedulableServiceInfo> resp = new List<SchedulableServiceInfo>();
           
            XDocument config = XDocument.Load(configPath);
            
            var configElements = from serviceInfo in config.Descendants("SchedulableServiceInfo")
                                 select new {
                                    Name = serviceInfo.Attribute("name").Value,
                                    Assembly = serviceInfo.Attribute("assembly").Value,
                                    LastExecutionDate = serviceInfo.Attribute("lastExecution").Value,
                                    ExecutionInterval = serviceInfo.Attribute("executionIntervalMinutes").Value
                                 };

            foreach (var si in configElements)
            {
                SchedulableServiceInfo ssi = new SchedulableServiceInfo();
                ssi.Name = si.Name;
                ssi.Assembly = si.Assembly;
                ssi.ExecutionInterval = Int64.Parse(si.ExecutionInterval);
                if (String.IsNullOrEmpty(si.LastExecutionDate)){
                     ssi.LastExecution = null;
                }else{
                    ssi.LastExecution = DateTime.Parse(si.LastExecutionDate);
                }
                resp.Add(ssi);
            }
            return resp;
        }

        public void SaveServiceState(SchedulableServiceInfo serviceInfo)
        {
            XDocument config = XDocument.Load(configPath);
            IEnumerable<XElement> ssi = from b in config.Descendants("SchedulableServiceInfo")
                                        where b.Attribute("name").Value.Equals(serviceInfo.Name)
                                        select b;

            foreach (XElement xe in ssi) {
                xe.SetAttributeValue("lastExecution", serviceInfo.LastExecution.ToString());
            }
            config.Save(configPath);
        }

        #endregion
    }
}
