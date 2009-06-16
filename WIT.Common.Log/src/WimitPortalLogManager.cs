using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Collections;

namespace WIT.Common.Log
{
    public class WITLogManager
    {
        private static ILog logger = null;

        private static WITLogManager instance = null;

        private WITLogManager()
        {



        }

        public static WITLogManager GetInstance(string loggerName)
        {
            if (instance == null)
            {
                log4net.Config.XmlConfigurator.Configure();
                logger = LogManager.GetLogger(loggerName);
                instance = new WITLogManager();
            }
            return instance;

        }
        private string GetNumberError()
        {
            string numberError = DateTime.Now.Ticks.ToString();
            numberError = numberError.Substring(numberError.Length - 5, 5);
            return numberError;
        }

        public void LogInfo(string message)
        {
            logger.Info(message);

        }
        public string LogError(string message, Exception ex)
        {
            string error = GetNumberError();
            log4net.ThreadContext.Properties["EventID"] = error;
            ArrayList logParams = new ArrayList();
            logParams.Add(error);
            logParams.Add(message);
            logParams.Add(ex.StackTrace);
            logger.ErrorFormat("\n\nCode: {0}\n\nUser: {1}\n\nWebPage: {2}\n\nMessage: {3}\n\nStackTrace: {4}", logParams.ToArray());
            return error;
        }
        public string LogError(string message, Exception ex, string userName, string webpageName)
        {
            string error = GetNumberError();
            log4net.ThreadContext.Properties["EventID"] = error;
            ArrayList logParams = new ArrayList();
            logParams.Add(error);
            logParams.Add(userName);
            logParams.Add(webpageName);
            logParams.Add(message);
            logParams.Add(ex.StackTrace);
            logger.ErrorFormat("\n\nCode: {0}\n\nUser: {1}\n\nWebPage: {2}\n\nMessage: {3}\n\nStackTrace: {4}", logParams.ToArray());
            return error;
        }

        public void LogWarning(string message)
        {
            logger.Warn(message);
        }
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }
    }
}
