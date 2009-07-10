using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

namespace SecureService
{
   
    public class ClaimsAuthServiceBehavior : BehaviorExtensionElement, IServiceBehavior
    {
        #region IServiceBehavior Members

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
                                         Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase cdb in serviceHostBase.ChannelDispatchers)
            {
                var cd = cdb as ChannelDispatcher;

                if (cd != null)
                {
                    foreach (EndpointDispatcher ed in cd.Endpoints)
                    {

                        foreach (var dispatchOperation in
                            ed.DispatchRuntime.Operations)
                        {
                            dispatchOperation.CallContextInitializers
                                .Add(new ClaimsAuthContextInitializer());
                        }

                    }
                }
            }
        }

        #endregion

        public override Type BehaviorType
        {
            get { return this.GetType(); }
        }

        protected override object CreateBehavior()
        {
            return new ClaimsAuthServiceBehavior();
        }
    }
}
