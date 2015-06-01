using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace SecureServiceConsumer
{
    // NOTE: If you change the interface name "IService1" here, you must also update the reference to "IService1" in Web.config.
    [ServiceContract]
    public interface IService1
    {
        [WebGet(UriTemplate = "/GetData?param1={p1}&param2={p2}")]
        [OperationContract]
        string GetData(string p1,string p2);
    }


}
