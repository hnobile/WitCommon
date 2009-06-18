using System;
using System.Configuration;

namespace WIT.Common.ServiceRunner.DAO
{
    public class SchedulableServiceDAOFactory
    {
        private static ISchedulableServiceDAO instance = null;
        public static ISchedulableServiceDAO GetDAO() {
            if (instance == null) {
                Type type = Type.GetType(ConfigurationManager.AppSettings["ISchedulableServiceDAO"]);
                instance = (ISchedulableServiceDAO)Activator.CreateInstance(type);
            }
            return instance;
        }
    }
}
