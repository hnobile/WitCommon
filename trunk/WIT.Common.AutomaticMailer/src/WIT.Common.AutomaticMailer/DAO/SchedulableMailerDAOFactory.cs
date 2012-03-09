using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WIT.Common.AutomaticMailer.Common;
using System.Configuration;

namespace WIT.Common.AutomaticMailer.Sender.DAO
{
    public class SchedulableMailerDAOFactory
    {
        private static ISchedulableMailerDAO instance = null;
        public static ISchedulableMailerDAO GetDAO()
        {
            if (instance == null)
            {
                Type type = Type.GetType(ConfigurationManager.AppSettings[WellKnownKeys.ISchedulableMailerDAO]);
                instance = (ISchedulableMailerDAO)Activator.CreateInstance(type);
            }
            return instance;
        }
    }
}
