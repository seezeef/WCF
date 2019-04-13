using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Net;
using System.Configuration;
using System.Web;

namespace YsLibrary
{
    public class SecurityTokenEndpointBehaviourExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(SecurityTokenServiceBehaviour); }
        }
        protected override object CreateBehavior()
        {
            return new SecurityTokenServiceBehaviour();
        }
    }


    public class SecurityTokenServiceBehaviour : IServiceBehavior
    {
        public void Validate(ServiceDescription serviceDescription,
                             ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription,
                              ServiceHostBase serviceHostBase,
                              Collection<ServiceEndpoint> endpoints,
                              BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription,
                              ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher epd in cd.Endpoints)
                {
                    epd.DispatchRuntime.MessageInspectors.Add(new SecurityTokenMessageInspector());
                }
            }
        }
    }



    public class SecurityTokenMessageInspector : IDispatchMessageInspector
    {
        public static string Token { get { return ConfigurationManager.AppSettings.Get("Token"); } }

        public HttpContext Context
        {
            get
            { 
                return HttpContext.Current;
            }
        }
        public HttpRequest Request
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }

        public object AfterReceiveRequest(ref Message request,
                          IClientChannel channel,
                          InstanceContext instanceContext)
        {


            if (Request == null)
            {
                throw new Exception("Unauthorized access:Error_no:10011");

              
            }

            if (Request.Headers.Count<=0)
                throw new Exception("Unauthorized access:Error_no:10011");


            if (!Request.Headers.AllKeys.Contains("AuthorizationToken"))
                throw new Exception("Unauthorized access:Error_no:10011");


            string AuthorizationToken = Request.Headers["AuthorizationToken"].ToString();

         


            if (Token == null || string.IsNullOrEmpty(Token))
            {
                throw new Exception("Unauthorized access:Error_no:10011");

            }

            if (AuthorizationToken == null || string.IsNullOrEmpty(AuthorizationToken))
            {
                throw new Exception("Unauthorized access:Error_no:10011");

            }


            if (!AuthorizationToken.Equals(Token))
                throw new Exception("Unauthorized access:Error_no:10011" );


            return null;





        }



        public void BeforeSendReply(ref Message reply, object correlationState)
        {


        }
    }


}
