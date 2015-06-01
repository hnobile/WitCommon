using System;
using log4net;
using System.Configuration;

namespace WIT.Common.Logger
{
    /// <summary>
    /// Wrapper for log4net service.
    /// </summary>
    public class Logger
    {
        private static ILog logger = null;

        /// <summary>
        /// Private default constructor.
        /// </summary>
        private Logger()
        {
        }

        /// <summary>
        /// Logs a debug level message.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        public static void LogDebug(string message)
        {
            GetLogger().Debug(message);
        }

        /// <summary>
        /// Logs an error level message with a default message format.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="ex">The exception to be logged.</param>
        /// <param name="parameters">The parameters to be applied to the default message.</param>
        /// <returns>The generated error number.</returns>
        public static string LogError(string message, Exception ex, params object[] parameters)
        {
            return LogError(message, ex, ConfigurationManager.AppSettings["WIT.Common.Logger_LogErrorFormat"], parameters);
        }

        /// <summary>
        /// Logs an error level message with a given message format.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="ex">The exception to be logged.</param>
        /// <param name="format">The format for the message.</param>
        /// <param name="parameters">The parameters to be applied to the default message.</param>
        /// <returns>The generated error number.</returns>
        public static string LogError(string message, Exception ex, string format, params object[] parameters)
        {
            string errorNumber = GetErrorNumber();

            log4net.ThreadContext.Properties["EventID"] = errorNumber;

            // avoid .net configurationmanager extra escape characters
            format = format.Replace("\\r", "\r"); 
            format = format.Replace("\\n", "\n");
            format = format.Replace("\\t", "\t");

            format = format.Replace("[message]", message);
            format = format.Replace("[errorNumber]", errorNumber);
            if (ex != null)
            {
                if (ex.Message != null)
                    format = format.Replace("[exceptionMessage]", ex.Message);

                if (ex.StackTrace != null)
                    format = format.Replace("[exceptionStackTrace]", ex.StackTrace);
            }
            GetLogger().ErrorFormat(format, parameters);
            
            return errorNumber;
        }

        /// <summary>
        /// Logs an info level message.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        public static void LogInfo(string message)
        {
            GetLogger().Info(message);
        }

        /// <summary>
        /// Logs a warning level message.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        public static void LogWarn(string message)
        {
            GetLogger().Warn(message);
        }

        /// <summary>
        /// Gets an instance of the log4net service.
        /// </summary>
        /// <returns>The instance of the log4net service.</returns>
        private static ILog GetLogger()
        {
            if(logger == null)
            {
                log4net.Config.XmlConfigurator.Configure();

                logger = LogManager.GetLogger(ConfigurationManager.AppSettings["WIT.Common.Logger_LoggerName"]);
            }

            return logger;
        }

        /// <summary>
        /// Generates an error number.
        /// </summary>
        /// <returns>The generated error number.</returns>
        private static string GetErrorNumber()
        {
            string errorNumber = DateTime.Now.Ticks.ToString();

            errorNumber = errorNumber.Substring(errorNumber.Length - 5, 5);
            
            return errorNumber;
        }
    }
}
