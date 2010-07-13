using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WIT.Common.ServiceRunner.Common;
using WIT.Common.Logger;
using WIT.Common.AutomaticMailer.DAO;
using WIT.Common.AutomaticMailer.Common;
using WIT.Common.Mailer;
using System.Threading;

namespace WIT.Common.AutomaticMailer
{
    public class AutomaticMailSender: SchedulableServiceBase
    {
        private Dictionary<Guid, AutoResetEvent> mailsSucceded = new Dictionary<Guid, AutoResetEvent>();

        public override void Execute()
        {
            Logger.Logger.LogInfo("Starting the automatic mailer...");
            //Levanto todos los mailers configurados en el provider correspondiente
            List<SchedulableMailerInfo> mailers = SchedulableMailerDAOFactory.GetDAO().GetAll();
            Logger.Logger.LogInfo("Schedulable mailers loaded: " + mailers.Count);
            
            //Esta lista es para notificar cuando se envió un mail. El waithandle se queda esperando que todos los items de la lista estén finalizados
            
            
            //Recorro todos los mailers filtrando por los que se deben ejecutar
            mailers = mailers.Where(m => !m.LastExecution.HasValue || !m.ExecutionInterval.HasValue || ( m.LastExecution.Value.AddMinutes(m.ExecutionInterval.Value) < DateTime.Now )).ToList();
            foreach (SchedulableMailerInfo mailer in mailers)
            {
                Logger.Logger.LogInfo("Running the " + mailer.Name + " mailer");
                AppDomain d = null;
                mailer.LastExecution = DateTime.Now;
                AppDomainSetup ads = new AppDomainSetup();
                ads.ApplicationBase = mailer.BaseFolder;
                ads.ConfigurationFile = mailer.ConfigFileName;
                d = AppDomain.CreateDomain("WIT.AutomaticMailer", null, ads);
                
                ISchedulableMailer instance = (ISchedulableMailer)d.CreateInstanceAndUnwrap(mailer.AssemblyName, mailer.TypeName);
                List<MailInfo> mailList = instance.GetMailingList();
                Logger.Logger.LogInfo("Trying to send " + mailList.Count + " mails");
                
                foreach(var mailInfo in mailList){
                    var id = Guid.NewGuid();
                    mailsSucceded.Add(id, new AutoResetEvent(false));
                    QueueMail(mailInfo, id);
                }
                SchedulableMailerDAOFactory.GetDAO().SaveMailerState(mailer);
                AppDomain.Unload(d);
            }
            if (mailsSucceded.Count > 0)
            {
                WaitHandle.WaitAll(mailsSucceded.Values.ToArray());
                Thread.Sleep(5000);
            }
        }

        private void QueueMail(MailInfo mailInfo, Guid id)
        {
            ThreadPool.QueueUserWorkItem(delegate
                {
                    SMTPConnectionInfo smtpInfo = new SMTPConnectionInfo();
                    smtpInfo.SMTPHost = mailInfo.SMTPHost;
                    smtpInfo.SMTPPassword = mailInfo.SMTPPassword;
                    smtpInfo.SMTPPort = mailInfo.SMTPPort;
                    smtpInfo.SMTPUser = mailInfo.SMTPUser;
                    smtpInfo.SMTPUseSSL = mailInfo.SMTPUseSSL;
                    Logger.Logger.LogInfo("Trying to send mail \n"
                        + "To: " + mailInfo.To +
                        " From: " + mailInfo.FromAddress +
                        " Host: " + mailInfo.SMTPHost);
                    try
                    {
                        MailServiceProvider.NewInstance.Send(mailInfo.Subbject, mailInfo.Body, mailInfo.To, mailInfo.FromAddress, mailInfo.FromName,
                            mailInfo.Attachments, smtpInfo);
                    }
                    catch (Exception ex)
                    {
                        Logger.Logger.LogError("Error sending mail: " + ex.Message, ex);
                    }
                    mailsSucceded[id].Set();
                });
        }

        private void SendMail(object info)
        {
            MailInfo mailInfo = (MailInfo)info;
            SMTPConnectionInfo smtpInfo = new SMTPConnectionInfo();
            smtpInfo.SMTPHost = mailInfo.SMTPHost;
            smtpInfo.SMTPPassword = mailInfo.SMTPPassword;
            smtpInfo.SMTPPort = mailInfo.SMTPPort;
            smtpInfo.SMTPUser = mailInfo.SMTPUser;
            smtpInfo.SMTPUseSSL = mailInfo.SMTPUseSSL;
            Logger.Logger.LogInfo("Trying to send mail \n"
                + "To: " + mailInfo.To +
                " From: " + mailInfo.FromAddress +
                " Host: " + mailInfo.SMTPHost);
            try
            {
                MailServiceProvider.NewInstance.Send(mailInfo.Subbject, mailInfo.Body, mailInfo.To, mailInfo.FromAddress, mailInfo.FromName,
                    mailInfo.Attachments, smtpInfo);
            }
            catch (Exception ex)
            {
                Logger.Logger.LogError("Error sending mail: " + ex.Message, ex);
            }
        }
    }
}
