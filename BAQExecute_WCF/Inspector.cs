using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace BAQExecute_WCF
{
    public class CustomMessageInspector : IClientMessageInspector
    {
        private string company;
        private string plant;
        public CustomMessageInspector(String Company, string Plant)
        {
            company = Company;
            plant = Plant;

        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {

        }

        public object BeforeSendRequest(ref Message request, System.ServiceModel.IClientChannel channel)
        {
            if (company != null && plant != null)
            {
                var vallCallSettings = new CallSettingsHeader() { Company = company, Plant = plant };

                request.Headers.Add(vallCallSettings);
            }
            return request;
        }
    }


    class CallSettingsHeader : MessageHeader
    {

        public string Company;
        public string Plant = "";
        protected override void OnWriteHeaderContents(System.Xml.XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteElementString("Company", @"http://schemas.datacontract.org/2004/07/Epicor.Hosting", Company);
            writer.WriteElementString("FormatCulture", @"http://schemas.datacontract.org/2004/07/Epicor.Hosting", "");
            writer.WriteElementString("Language", @"http://schemas.datacontract.org/2004/07/Epicor.Hosting", "");
            writer.WriteElementString("Plant", @"http://schemas.datacontract.org/2004/07/Epicor.Hosting", Plant);
            writer.WriteElementString("TimezoneOffset", @"http://schemas.datacontract.org/2004/07/Epicor.Hosting", 0.ToString());
        }

        public override string Name
        {
            get { return "CallSettings"; }
        }

        public override string Namespace
        {
            get { return "urn:epic:headers:CallSettings"; }
        }



    }

    class HookServiceBehavior : System.ServiceModel.Description.IEndpointBehavior
    {
        private string Company;
        private string Plant;
        public HookServiceBehavior(String company, string plant)
        {
            this.Company = company;
            this.Plant = plant;
        }
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new CustomMessageInspector(Company, Plant));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }
    }
    
    public static class EpicorClient
    {
        private static BasicHttpBinding GetWsHttpBinding()
        {
            var binding = new BasicHttpBinding();
            const int maxBindingSize = Int32.MaxValue;
            binding.MaxReceivedMessageSize = maxBindingSize;
            binding.ReaderQuotas.MaxDepth = maxBindingSize;
            binding.ReaderQuotas.MaxStringContentLength = maxBindingSize;
            binding.ReaderQuotas.MaxArrayLength = maxBindingSize;
            binding.ReaderQuotas.MaxBytesPerRead = maxBindingSize;
            binding.ReaderQuotas.MaxNameTableCharCount = maxBindingSize;
            binding.Security.Mode = BasicHttpSecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

            return binding;
        }

        public static TClient GetClient<TClient, TIterface>(string url, string username, string password)
            where TClient : ClientBase<TIterface>
            where TIterface : class
        {
            System.ServiceModel.Channels.Binding binding = null;
            TClient client;

            var endpointAddress = new EndpointAddress(url);
            binding = GetWsHttpBinding();
            client = (TClient)Activator.CreateInstance(typeof(TClient), binding, endpointAddress);

            if (!string.IsNullOrEmpty(username) && client.ClientCredentials != null)
            {
                client.ClientCredentials.UserName.UserName = username;
                client.ClientCredentials.UserName.Password = password;
            }
            return client;

        }
    }
}
