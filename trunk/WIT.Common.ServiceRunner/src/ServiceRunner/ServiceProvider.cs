using System;
using System.Collections.Generic;
using WIT.Common.ServiceRunner.DAO;
using WIT.Common.Log;

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
                    try {
                        ISchedulableService instance = CreateServiceInstance(s.Assembly);
                        s.LastExecution = DateTime.Now;
                        instance.Execute();
                        ServiceLog(s);
                    }
                    catch (Exception ex)
                    {
                        WITLogManager logManager = WITLogManager.GetInstance(WellKnownKeys.DefaultLoggerName);
                        string error = logManager.LogError(ex.Message, ex);
                    }
                }
            }
        }

        private ISchedulableService CreateServiceInstance(string assembly)
        {
            Type type = Type.GetType(assembly);
            ISchedulableService instance = (ISchedulableService)Activator.CreateInstance(type);
            return instance;
            
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
