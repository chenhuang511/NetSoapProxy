using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceWithProxy.TestService;
using System.Net;
using System.ServiceModel;

namespace ServiceWithProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            //init
            string endpointUri = @"http://gw.softdreams.vn:6161/Services.svc";
            bool useCredentials = false;
            bool useDefaultWebProxy = true;

            //custom proxy
            CustomProxy customProxy = new CustomProxy();
            customProxy.protocol = "http"; //or https
            //pass
            customProxy.host = "123.30.238.16";
            //not pass
            //customProxy.host = "171.251.51.218";
            customProxy.port = "3128";

            //setup proxy
            string proxy = string.Format("{0}://{1}:{2}", customProxy.protocol, customProxy.host, customProxy.port);
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;

            //use default web proxy or not
            if (!useDefaultWebProxy)
            {
                binding.UseDefaultWebProxy = false;
                binding.ProxyAddress = new Uri(proxy);
            }
            else
            {
                binding.UseDefaultWebProxy = true;
            }
            EndpointAddress endpoint = new EndpointAddress(endpointUri);
            ServicesClient client = null;
            if (!useCredentials)
            {
                binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                client = new ServicesClient(binding, endpoint);
                client.ClientCredentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
            }
            else
            {
                binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.Basic;
                client = new ServicesClient(binding, endpoint);
                CustomCredential customCredential = new CustomCredential("", "", "");
                client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(customCredential.username, customCredential.password, customCredential.domain);
            }

            System.Net.ServicePointManager.Expect100Continue = false;

            //call service
            object[] responseData = null;
            client.Process("token", "sd8888h11", "e10adc3949ba59abbe56e057f20f883e", "serviceId", "C101", "", new byte[1], ref responseData);
            Console.WriteLine(responseData[0]);
            Console.ReadLine();
        }
    }

    class CustomCredential
    {
        public string username { set; get; }
        public string password { set; get; }
        public string domain { set; get; }

        public CustomCredential() { }

        public CustomCredential(string username, string password, string domain)
        {
            this.username = username;
            this.password = password;
            this.domain = domain;
        }
    }

    class CustomProxy
    {
        public string protocol { set; get; }
        public string host { set; get; }
        public string port { set; get; }

        public CustomProxy() { }

        public CustomProxy(string protocol, string host, string port)
        {
            this.protocol = protocol;
            this.host = host;
            this.port = port;
        }
    }
}
