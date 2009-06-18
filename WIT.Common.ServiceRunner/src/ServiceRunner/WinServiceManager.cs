using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using WIT.Common.Log;

namespace WIT.Common.ServiceRunner
{
    partial class WinServiceManager : ServiceBase
    {
        public WinServiceManager()
        {
            InitializeComponent();
        }

        private void ServiceMain()
        {
            while (true)
            {
                WITLogManager.GetInstance(WellKnownKeys.DefaultLoggerName).LogInfo("Started main WIT Service Runner Loop");
                ServiceProvider sProvider = new ServiceProvider();
                sProvider.ProcessServices();
                System.Threading.Thread.Sleep(Int32.Parse(ConfigurationManager.AppSettings["ExcecutionGapInMillisecs"]));
            }
        }

        protected override void OnStart(string[] args)
        {
            WITLogManager.GetInstance(WellKnownKeys.DefaultLoggerName).LogInfo("WIT Service Runner Started");
            ThreadStart ts = new ThreadStart(ServiceMain);
            System.Threading.Thread t = new System.Threading.Thread(ts);
            t.Start();
        }

        protected override void OnStop()
        {
            WITLogManager.GetInstance(WellKnownKeys.DefaultLoggerName).LogWarning("WIT Service Runner Stopped");
        }
    }
}
