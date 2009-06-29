using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel;
using Common;
using System.Security.Cryptography;

namespace SecureServiceConsumer
{
    class ClientMessageInspector : IClientMessageInspector
    {
        #region IClientMessageInspector Members

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            Console.WriteLine(reply.ToString());
        }

        public object BeforeSendRequest(ref Message request,IClientChannel channel)
        {
            //Get the HttpRequestMessage property from the Message
            var httpRequest =
                request.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;

            //Make sure we have a valid property, or create one
            if (httpRequest == null)
            {
                httpRequest = new HttpRequestMessageProperty();
                request.Properties.Add(HttpRequestMessageProperty.Name, httpRequest);
            }

            List<KeyValuePair<string,string>> queryParams = UriUtility.GetQueryParameters(request.Headers.To.Query);
            string sig = UriUtility.NormalizeRequestParameters(queryParams);

            var hashAlgorithm = new HMACSHA1{ Key = Encoding.ASCII.GetBytes("SUPERSECRET") };

            byte[] dataBuffer = Encoding.ASCII.GetBytes(sig);
            byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

            string signature =  Convert.ToBase64String(hashBytes);

            string safeSignature = UriUtility.UrlEncode(signature);
            
            //Add your token to the header. Simple as that.
            //In real life, we injected an ITokenProvider using an IoC,
            //instead of using static methods directly, but that was
            //overkill for a sample.
            httpRequest.Headers.Add("Authorization",signature);
            return null;
        }

        #endregion
    }

    public class SampleCustomBehavior : IEndpointBehavior
    {

        public void AddBindingParameters(ServiceEndpoint serviceEndpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        { }

        public void ApplyClientBehavior(ServiceEndpoint serviceEndpoint, System.ServiceModel.Dispatcher.ClientRuntime behavior)
        {
            //Add the inspector
            behavior.MessageInspectors.Add(new ClientMessageInspector());
        }

        public void ApplyDispatchBehavior(ServiceEndpoint serviceEndpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        { }

        public void Validate(ServiceEndpoint serviceEndpoint)
        { }
    }
}
