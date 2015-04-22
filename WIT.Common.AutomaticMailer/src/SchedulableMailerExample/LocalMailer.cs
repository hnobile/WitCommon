using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WIT.Common.AutomaticMailer.Common;
using System.Configuration;

namespace SchedulableMailerExample
{
    [Serializable]
    public class LocalMailer : SchadulableMailerBase
    {

        public override List<MailInfo> GetMailingList(DateTime? lastExecutionTime)
        {

            List<MailInfo> mails = new List<MailInfo>();

            for (int i = 0; i < 400; i++)
            {
                MailInfo mail1 = new MailInfo();
                mail1.Body = "El test " + i;
                mail1.FromAddress = "soporte@wormholeit.com";
                mail1.FromName = "Alerta " + i;
                mail1.SMTPHost = "192.168.1.8";
                mail1.SMTPPort = 25;
                mail1.SMTPPassword = "test123";
                mail1.SMTPUser = "soporte@wormholeit.com";
                mail1.Subbject = "Schedulable Mailer Test " + i;
                mail1.SMTPUseSSL = false;
                mail1.To = "hnobile+alert"+i+"@wormholeit.com";

                mails.Add(mail1);
            }

            //MailInfo mail1 = new MailInfo();
            //mail1.Body = "Esto es un test";
            //mail1.FromAddress = "soporte@wormholeit.com";
            //mail1.FromName = "Alerta 1";
            //mail1.SMTPHost = "192.168.1.8";
            //mail1.SMTPPort = 25;
            //mail1.SMTPPassword = "test123";
            //mail1.SMTPUser = "soporte@wormholeit.com";
            //mail1.Subbject = "Schedulable Mailer Example";
            //mail1.SMTPUseSSL = false;
            //mail1.BCC = "hnobile@wormholeit.com";
            //mail1.CC = "hnobile@wormholeit.com";
            //mail1.To = "hnobile@wormholeit.com";

            //mails.Add(mail1);

            //MailInfo mail2 = new MailInfo();
            //mail2.Body = "Esto es el segundo test";
            //mail2.FromAddress = "mongo@minguitosrc.com.uk";
            //mail2.FromName = "Alerta 2";
            //mail2.SMTPHost = "192.168.1.8";
            //mail2.SMTPPort = 25;
            //mail2.SMTPUser = "test123";
            //mail2.Subbject = "Schedulable Mailer Example";
            //mail2.SMTPUseSSL = false;
            //mail2.To = ConfigurationManager.AppSettings["mailTo"];

            //mails.Add(mail2);

            //MailInfo mail3 = new MailInfo();
            //mail3.Body = "Esto es el tercer test";
            //mail3.FromAddress = "mic@sa.net";
            //mail3.FromName = "Alerta 3";
            //mail3.SMTPHost = "192.168.1.8";
            //mail3.SMTPPort = 25;
            //mail3.SMTPUser = "test123";
            //mail3.Subbject = "Schedulable Mailer Example";
            //mail3.SMTPUseSSL = false;
            //mail3.To = ConfigurationManager.AppSettings["mailTo"];

            //mails.Add(mail3);

            return mails;

        }
    }
}
