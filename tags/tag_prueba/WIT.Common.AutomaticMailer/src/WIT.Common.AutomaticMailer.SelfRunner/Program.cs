using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WIT.Common.AutomaticMailer.Sender;

namespace WIT.Common.AutomaticMailer.SelfRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            AutomaticMailSender s = new AutomaticMailSender();
            s.Execute(null);
        }
    }
}
