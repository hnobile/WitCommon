using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WIT.Common.Mailer;
using System.Web.UI.MobileControls;
using System.Collections.Generic;
using System.IO;

namespace WitMailSender.SenderPage
{
    public partial class _SendMail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SendMail();
        }

        private void SendMail()
        {
            IMailService mailer = MailServiceProvider.GetMailService();

            if (mailer != null)
            {
                string body = BuildBody();
                string sendToAddress = GetConfigurationValue("SendToAddress");
                string mailSubject = GetConfigurationValue("MailSubject");
                if (string.IsNullOrEmpty(sendToAddress))
                {
                    sendToAddress = GetQueryString(GetConfigurationValue("VariableEmail"));
                }
                if (!string.IsNullOrEmpty(sendToAddress))
                {
                    mailer.SendMail(sendToAddress, mailSubject, null, body);
                }
            }
        }

        private string BuildBody()
        {
            string body = GetTemplate();

            MailVariableSection variables = (MailVariableSection)ConfigurationManager.GetSection("MailVariableSection");

            Dictionary<string, string> varsValues = new Dictionary<string, string>();

            foreach (MailVariable mv in variables.Variables)
            {
                varsValues.Add(mv.Key, GetQueryString(mv.Value));
            }

            foreach (KeyValuePair<string, string> kv in varsValues)
            {
                body = body.Replace("{" + kv.Key + "}", kv.Value);
            }

            return body;
        }

        private string GetQueryString(string key)
        {
            return Request.Form[key];
        }

        private string GetConfigurationValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private string GetTemplate()
        {
            string templateName = ConfigurationManager.AppSettings["MailTemplate"];
            string templatePath = ConfigurationManager.AppSettings["MailTemplatesPath"] + templateName + ".htm";
            string message = String.Empty;
            if (File.Exists(templatePath))
            {
                TextReader template = new StreamReader(templatePath);
                message = template.ReadToEnd();
                template.Close();
            }
            return message;
        }
    }
}
