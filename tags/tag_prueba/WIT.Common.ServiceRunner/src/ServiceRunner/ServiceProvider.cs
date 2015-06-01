using System;
using System.Collections.Generic;
using WIT.Common.Log;
using WIT.Common.ServiceRunner.DAO;

namespace WIT.Common.ServiceRunner
{
    public class ServiceProvider
    {
        private List<SchedulableServiceInfo> services;
        public ServiceProvider()
        {
            GetServices();
        }

        public void GetServices()
        {
            services = SchedulableServiceDAOFactory.GetDAO().GetAll();
        }

        public void ProcessServices()
        {
            foreach (SchedulableServiceInfo s in services)
            {
                if (MustExecute(s))
                {
                    WITLogManager.GetInstance(WellKnownKeys.DefaultLoggerName).LogInfo("Started to process service with name " + s.Name);
                    AppDomain d = null;
                    try
                    {
                        DateTime? lastExecution = s.LastExecution;
                        s.LastExecution = DateTime.Now;
                        AppDomainSetup ads = new AppDomainSetup();
                        ads.ApplicationBase = s.BaseFolder;
                        ads.ConfigurationFile = s.ConfigFileName;
                        d = AppDomain.CreateDomain("WIT.ServiceRunner", null, ads);
                        ISchedulableService instance = (ISchedulableService)d.CreateInstanceAndUnwrap(
                            s.AssemblyName, s.TypeName);
                        instance.Execute(lastExecution);
                        ServiceLog(s);
                       
                        WITLogManager.GetInstance(WellKnownKeys.DefaultLoggerName).LogInfo("Finished to process service with name " + s.Name);
                    }
                    catch (Exception ex)
                    {
                        WITLogManager logManager = WITLogManager.GetInstance(WellKnownKeys.DefaultLoggerName);
                        string error = logManager.LogError(ex.Message, ex);
                    }
                    finally {
                        AppDomain.Unload(d);
                    }
                }
            }
        }

        private bool MustExecute(SchedulableServiceInfo s)
        {
            if (s.LastExecution == null) {
                return true;
            }
            DateTime date = (DateTime)s.LastExecution + new TimeSpan(0, (int)s.ExecutionInterval, 0);
            return (DateTime.Now > date);
        }

        private void ServiceLog(SchedulableServiceInfo service)
        {
            service.LastExecution = DateTime.Now;
            SchedulableServiceDAOFactory.GetDAO().SaveServiceState(service);
        }
    }
}
