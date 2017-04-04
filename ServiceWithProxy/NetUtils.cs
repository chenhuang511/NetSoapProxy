using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Net.Utils
{
    public class MyWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.KeepAlive = false;
            //request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            return request;
        }
    }

    public class ProxyConfig
    {
        public const string PROXY_CONFIG = "proxy.conf";

        public bool NoneProxy { get; set; }
        public bool UseIEProxy { get; set; }
        public bool UseCustomProxy { get; set; }
        public string ProxyHost { get; set; }
        public int ProxyPort { get; set; }
        public bool ProxyAuthen { get; set; }
        public string ProxyUser { get; set; }
        public string ProxyPass { get; set; }

        //public static ProxyConfig GetConfig()
        //{
        //    string configPath = Path.Combine(Application.StartupPath, PROXY_CONFIG);
        //    if (string.IsNullOrEmpty(configPath) || !File.Exists(configPath))
        //        return null;

        //    try
        //    {
        //        System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(ProxyConfig));
        //        FileInfo fi = new FileInfo(configPath);
        //        using (System.IO.StreamReader file = new System.IO.StreamReader(configPath))
        //        {
        //            ProxyConfig config = (ProxyConfig)reader.Deserialize(file);
        //            return config;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log2File.LogExceptionToFile(ex);
        //        return null;
        //    }
        //}
        public static IWebProxy SetProxy(ProxyConfig config)
        {
            if (config == null)
                return null;

            IWebProxy proxy = null;
            if (config.NoneProxy)
            {
                proxy = null;
            }
            else if (config.UseCustomProxy)
            {
                Uri newUri = new Uri(string.Format("http://{0}:{1}", config.ProxyHost, config.ProxyPort));
                proxy = new WebProxy(newUri);
                if (config.ProxyAuthen)
                {
                    proxy.Credentials = new NetworkCredential(config.ProxyUser, config.ProxyPass);
                }
            }
            else if (config.UseIEProxy)
            {
                // use system proxy
                proxy = WebRequest.GetSystemWebProxy();

                // var request = (HttpWebRequest)WebRequest.Create(link);
                //if (request.Proxy != null) // check system proxy
                //{
                //    var uri = request.Proxy.GetProxy(request.RequestUri);
                //    proxy = new WebProxy(uri, false);
                //    proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;                    
                //}               
            }

            // set global http request proxy
            WebRequest.DefaultWebProxy = proxy;
            return proxy;
        }
        //public static void SaveConfig(ProxyConfig config)
        //{
        //    string configPath = Path.Combine(Application.StartupPath, PROXY_CONFIG);
        //    //if (!File.Exists(configPath))
        //    //    File.Create(configPath).Close();

        //    var writer = new System.Xml.Serialization.XmlSerializer(typeof(ProxyConfig));
        //    using (var wfile = new System.IO.StreamWriter(configPath, false))
        //    {
        //        writer.Serialize(wfile, config);
        //        wfile.Close();
        //    }
        //}


    }
}
