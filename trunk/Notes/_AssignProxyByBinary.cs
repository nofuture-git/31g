using System;
using System.Net;
 
namespace NoFuture.AuthProxy
{
    public class AuthProxyModule : IWebProxy
    {
 
        ICredentials crendential = new NetworkCredential("proxy.user", "password");
 
        public ICredentials Credentials
        {
            get
            {
                return crendential;
            }
            set
            {
                crendential = value;
            }
        }
 
        public Uri GetProxy(Uri destination)
        {
            return new Uri("http://proxy:8080", UriKind.Absolute);
        }
 
        public bool IsBypassed(Uri host)
        {
            return host.IsLoopback;
        }
 
    }
}

//add the following to the config file
//lifted from http://blogs.msdn.com/b/rido/archive/2010/05/06/how-to-connect-to-tfs-through-authenticated-web-proxy.aspx
/*
<system.net>
    <defaultProxy>
      <module type="NoFuture.AuthProxy.AuthProxyModule, NoFuture.AuthProxy"/>
    </defaultProxy>
  </system.net>
*/