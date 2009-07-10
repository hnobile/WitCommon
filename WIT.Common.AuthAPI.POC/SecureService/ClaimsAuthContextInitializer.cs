using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Web;
using System.Collections.Generic;
using Common;
using System.Security.Cryptography;
using System.Text;

namespace SecureService
{
    public class ClaimsAuthContextInitializer : ICallContextInitializer
    {


        private static void DetectCurrentUser(System.ServiceModel.Channels.Message message)
        {
            if (WebOperationContext.Current == null)
                throw new InvalidOperationException("Only HTTP web requests are supported for this version of the API");

            string authToken = WebOperationContext.Current
               .IncomingRequest
               .Headers["Authorization"];

            List<KeyValuePair<string, string>> queryParams = 
                UriUtility.GetQueryParameters(message.Headers.To.Query);
            
            string sig = UriUtility.NormalizeRequestParameters(queryParams);

            var hashAlgorithm = new HMACSHA1 { Key = Encoding.ASCII.GetBytes("SUPERSECRET") };

            byte[] dataBuffer = Encoding.ASCII.GetBytes(sig);
            byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

            string signature = Convert.ToBase64String(hashBytes);

            string safeSignature = UriUtility.UrlEncode(signature);


            if (authToken != signature)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                  WebOperationContext.Current.OutgoingResponse.StatusDescription = "No authentication token was found in the headers of the current request";
                WebOperationContext.Current.OutgoingResponse.SuppressEntityBody = true;
                throw new UnauthorizedAccessException(
                    "No authentication token was found in the headers of the current request.");
            }
        }


        #region ICallContextInitializer Members

        public void AfterInvoke(object correlationState)
        {
           
        }

        public object BeforeInvoke(InstanceContext instanceContext, IClientChannel channel, System.ServiceModel.Channels.Message message)
        {
            DetectCurrentUser(message);
            return null;
        }

        #endregion
    }

}
