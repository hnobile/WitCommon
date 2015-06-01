using System.Configuration;
using WIT.Common.ReflectionHelper;

namespace WIT.Common.Mailer
{
    public class MailServiceProvider
    {
        private static IMailService _instance = null;

        public static IMailService NewInstance
        {
            get
            {
                string typeName = ConfigurationManager.AppSettings[WellKnownKeys.MailService_Implementation.ToString()];

                return ReflectionHelper.ReflectionHelper.GetInstance<IMailService>(typeName);
            }
        }
        
        public static IMailService Instance
        {
            get
            {
                if (_instance == null)
                {
                    string typeName = ConfigurationManager.AppSettings[WellKnownKeys.MailService_Implementation.ToString()];

                    _instance = ReflectionHelper.ReflectionHelper.GetInstance<IMailService>(typeName);
                }

                return _instance;
            }
        }
    }
}
