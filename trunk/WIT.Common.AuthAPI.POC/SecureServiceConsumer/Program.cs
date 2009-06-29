using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;
using System.ServiceModel;

namespace SecureServiceConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            WebChannelFactory<IService1> factory = null;
            IService1 client = null;
            factory = new WebChannelFactory<IService1>(new WebHttpBinding(),
                  new Uri("http://localhost/WIT.API.AUTH/Service1.svc/"));
            factory.Endpoint.Behaviors.Add(new SampleCustomBehavior());
            client = factory.CreateChannel();
            
            string result = client.GetData("nacho es","un lopez");
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
