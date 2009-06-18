using System;
using System.IO;
using WIT.Common.ServiceRunner;

namespace SchedulableServiceExample
{
    class FileWriterExample : ISchedulableService
    {
        #region ISchedulableService Members

        public void Execute()
        {
            TextWriter tw = new StreamWriter("C:\\Test.txt");

            // write a line of text to the file
            tw.WriteLine(DateTime.Now);

            // close the stream
            tw.Close();
            
        }

        #endregion
    }
}
