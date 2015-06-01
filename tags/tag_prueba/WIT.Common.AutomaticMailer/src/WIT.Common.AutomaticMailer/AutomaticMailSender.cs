using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WIT.Common.ServiceRunner.Common;
using WIT.Common.Logger;
using WIT.Common.AutomaticMailer.Sender.DAO;
using WIT.Common.AutomaticMailer.Common;
using WIT.Common.Mailer;
using System.Threading;
using System.Threading.Tasks;

namespace WIT.Common.AutomaticMailer.Sender
{
    public class AutomaticMailSender: SchedulableServiceBase
    {
        private Dictionary<Guid, AutoResetEvent> mailsSucceded = new Dictionary<Guid, AutoResetEvent>();

        public override void Execute(DateTime? lastExecutionTime)
        {
            try
            {
                Logger.Logger.LogInfo("Starting the automatic mailer...");
                //Levanto todos los mailers configurados en el provider correspondiente
                List<SchedulableMailerInfo> mailers = SchedulableMailerDAOFactory.GetDAO().GetAll();
                Logger.Logger.LogInfo("Schedulable mailers loaded: " + mailers.Count);

                //Esta lista es para notificar cuando se envió un mail. El waithandle se queda esperando que todos los items de la lista estén finalizados
                //Recorro todos los mailers filtrando por los que se deben ejecutar
                mailers = mailers.Where(m => !m.LastExecution.HasValue || !m.ExecutionInterval.HasValue || (m.LastExecution.Value.AddMinutes(m.ExecutionInterval.Value) < DateTime.Now)).ToList();
                foreach (SchedulableMailerInfo mailer in mailers)
                {
                    try
                    {
                        Logger.Logger.LogInfo("Running the " + mailer.Name + " mailer");
                        AppDomain d = null;
                        mailer.LastExecution = DateTime.Now;
                        AppDomainSetup ads = new AppDomainSetup();
                        ads.ApplicationBase = mailer.BaseFolder;
                        ads.ConfigurationFile = mailer.ConfigFileName;
                        d = AppDomain.CreateDomain("WIT.AutomaticMailer", null, ads);

                        ISchedulableMailer instance = (ISchedulableMailer)d.CreateInstanceAndUnwrap(mailer.AssemblyName, mailer.TypeName);
                        List<MailInfo> mailList = instance.GetMailingList(lastExecutionTime);
                        Logger.Logger.LogInfo("Trying to send " + mailList.Count + " mails");

                        Parallel.ForEach<MailInfo>(mailList, new ParallelOptions { MaxDegreeOfParallelism = 5 }, mailInfo => { SendMail(mailInfo); });
                        
                        //foreach (var mailInfo in mailList)
                        //{
                        //    SendMail(mailInfo);
                        //}
                        Logger.Logger.LogInfo("Trying to unload the AppDomain " + d.FriendlyName);
                        AppDomain.Unload(d);
                        Logger.Logger.LogInfo("The AppDomain was unloaded");
                    }
                    catch (Exception ex)
                    {
                        Logger.Logger.LogError("Error processing the mailer '" + mailer.Name + "'", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Logger.LogError("Error executing automatic the automatic mailer'", ex);
            }
        }

        private void SendMail(MailInfo mailInfo)
        {
            try
            {
                SMTPConnectionInfo smtpInfo = new SMTPConnectionInfo();
                smtpInfo.SMTPHost = mailInfo.SMTPHost;
                smtpInfo.SMTPPassword = mailInfo.SMTPPassword;
                smtpInfo.SMTPPort = mailInfo.SMTPPort;
                smtpInfo.SMTPUser = mailInfo.SMTPUser;
                smtpInfo.SMTPUseSSL = mailInfo.SMTPUseSSL;
                MailServiceProvider.Instance.Send(mailInfo.Subbject, mailInfo.Body, mailInfo.To, mailInfo.CC, mailInfo.BCC, mailInfo.FromAddress, mailInfo.FromName, mailInfo.Attachments, smtpInfo);
            }
            catch (Exception ex)
            {
                Logger.Logger.LogInfo("Error sending mail:\n" + ex.Message + "\nStackTrace:\n" + ex.StackTrace + "\n\n");
            }
        }
    }
}
