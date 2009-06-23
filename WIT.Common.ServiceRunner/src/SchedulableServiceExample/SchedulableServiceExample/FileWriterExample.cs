using System;
using System.Configuration;
using System.IO;
using WIT.Common.ServiceRunner.Common;

namespace SchedulableServiceExample
{
    [Serializable]
    public class FileWriterExample : SchedulableServiceBase
    {
        #region ISchedulableService Members

        public override void Execute()
        {
            TextWriter tw = new StreamWriter("C:\\Test.txt");

            // write a line of text to the file
            tw.WriteLine(ConfigurationManager.AppSettings["test"]);

            // close the stream
            tw.Close();
            
        }

        #endregion
    }
}
