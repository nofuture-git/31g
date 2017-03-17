using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Claims;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Security.Tokens;
using System.Xml;

namespace NoFuture.Shared.WcfClient
{
    public class Bindings
    {
        public static List<BasicHttpBinding> GetBasicHttpBindings(string exeConfigPath)
        {
            if (string.IsNullOrWhiteSpace(exeConfigPath))
                return null;
            var svcSection = Read.Config.ExeConfig.GetServiceModelSection(exeConfigPath);

            var configs = new List<BasicHttpBinding>();
            foreach (
                var section in
                    svcSection.Bindings.BasicHttpBinding.ConfiguredBindings
                        .Cast<BasicHttpBindingElement>())
            {
                var df = new BasicHttpBinding();
                var binding = new BasicHttpBinding
                {
                    Name = section.Name,

                    MaxBufferPoolSize = section.MaxBufferPoolSize > 0 ? section.MaxBufferPoolSize : df.MaxBufferPoolSize,
                    MaxReceivedMessageSize =
                        section.MaxReceivedMessageSize > 0 ? section.MaxReceivedMessageSize : df.MaxReceivedMessageSize,
                    CloseTimeout = section.CloseTimeout != TimeSpan.Zero ? section.CloseTimeout : df.CloseTimeout,
                    OpenTimeout = section.OpenTimeout != TimeSpan.Zero ? section.OpenTimeout : df.OpenTimeout,
                    SendTimeout = section.SendTimeout != TimeSpan.Zero ? section.SendTimeout : df.SendTimeout,
                    ReceiveTimeout =
                        section.ReceiveTimeout != TimeSpan.Zero ? section.ReceiveTimeout : df.ReceiveTimeout,

                    TextEncoding = section.TextEncoding ?? df.TextEncoding,

                    MessageEncoding = section.MessageEncoding,
                    AllowCookies = section.AllowCookies,
                    BypassProxyOnLocal = section.BypassProxyOnLocal,
                    HostNameComparisonMode = section.HostNameComparisonMode,
                    UseDefaultWebProxy = section.UseDefaultWebProxy,
                };

                var readerQuotasSection = section.ReaderQuotas;
                var readerQuotas = new System.Xml.XmlDictionaryReaderQuotas();
                if (readerQuotasSection != null && readerQuotasSection.MaxDepth > 0)
                {
                    readerQuotas.MaxDepth = readerQuotasSection.MaxDepth;
                    readerQuotas.MaxStringContentLength = readerQuotasSection.MaxStringContentLength;
                    readerQuotas.MaxArrayLength = readerQuotasSection.MaxArrayLength;
                    readerQuotas.MaxBytesPerRead = readerQuotasSection.MaxBytesPerRead;
                    readerQuotas.MaxNameTableCharCount = readerQuotasSection.MaxNameTableCharCount;
                }
                else
                {
                    readerQuotas = null;
                }

                var messageSection = section.Security.Message;
                var message = new BasicHttpMessageSecurity
                {
                    ClientCredentialType = messageSection.ClientCredentialType,
                    AlgorithmSuite = messageSection.AlgorithmSuite,
                };
                var transportSection = section.Security.Transport;
                var transport = new HttpTransportSecurity
                {
                    ClientCredentialType = transportSection.ClientCredentialType,
                    ProxyCredentialType = transportSection.ProxyCredentialType
                };
                var basicHttpSecurity = new BasicHttpSecurity()
                {
                    Message = message,
                    Mode = section.Security.Mode,
                    Transport = transport
                };

                binding.Security = basicHttpSecurity;
                if (readerQuotas != null)
                {
                    binding.ReaderQuotas = readerQuotas;
                }

                configs.Add(binding);
            }
            return configs;

        }

        public static List<WSHttpBinding> GetWsHttpBindings(string exeConfigPath)
        {
            if (string.IsNullOrWhiteSpace(exeConfigPath))
                return null;

            var svcSection = Read.Config.ExeConfig.GetServiceModelSection(exeConfigPath);

            var configs = new List<WSHttpBinding>();
            foreach (
                var section in
                    svcSection.Bindings.WSHttpBinding.ConfiguredBindings
                        .Cast<WSHttpBindingElement>())
            {
                var df = new WSHttpBinding();
                var binding = new WSHttpBinding
                {
                    Name = section.Name,

                    MaxBufferPoolSize = section.MaxBufferPoolSize > 0 ? section.MaxBufferPoolSize : df.MaxBufferPoolSize,
                    MaxReceivedMessageSize = section.MaxReceivedMessageSize > 0 ? section.MaxReceivedMessageSize : df.MaxReceivedMessageSize,
                    CloseTimeout = section.CloseTimeout != TimeSpan.Zero ? section.CloseTimeout : df.CloseTimeout,
                    OpenTimeout = section.OpenTimeout != TimeSpan.Zero ? section.OpenTimeout : df.OpenTimeout,
                    SendTimeout = section.SendTimeout != TimeSpan.Zero ? section.SendTimeout : df.SendTimeout,
                    ReceiveTimeout =
                        section.ReceiveTimeout != TimeSpan.Zero ? section.ReceiveTimeout : df.ReceiveTimeout,
                    
                    TextEncoding = section.TextEncoding ?? df.TextEncoding,

                    MessageEncoding = section.MessageEncoding,
                    AllowCookies = section.AllowCookies,
                    BypassProxyOnLocal = section.BypassProxyOnLocal,
                    TransactionFlow = section.TransactionFlow,
                    HostNameComparisonMode = section.HostNameComparisonMode,
                    UseDefaultWebProxy = section.UseDefaultWebProxy,
                };

                var readerQuotasSection = section.ReaderQuotas;
                var readerQuotas = new System.Xml.XmlDictionaryReaderQuotas();
                if (readerQuotasSection != null && readerQuotasSection.MaxDepth > 0)
                {
                    readerQuotas.MaxDepth = readerQuotasSection.MaxDepth;
                    readerQuotas.MaxStringContentLength = readerQuotasSection.MaxStringContentLength;
                    readerQuotas.MaxArrayLength = readerQuotasSection.MaxArrayLength;
                    readerQuotas.MaxBytesPerRead = readerQuotasSection.MaxBytesPerRead;
                    readerQuotas.MaxNameTableCharCount = readerQuotasSection.MaxNameTableCharCount;
                }
                else
                {
                    readerQuotas = null;
                }

                var reliableSessionSection = section.ReliableSession;
                var dfRss = new OptionalReliableSession();
                var reliableSession = new OptionalReliableSession
                {
                    Enabled = reliableSessionSection.Enabled,
                    Ordered = reliableSessionSection.Ordered,
                    InactivityTimeout =
                        reliableSessionSection.InactivityTimeout != TimeSpan.Zero
                            ? reliableSessionSection.InactivityTimeout
                            : dfRss.InactivityTimeout,
                };

                var messageSection = section.Security.Message;
                var message = new NonDualMessageSecurityOverHttp
                {
                    EstablishSecurityContext = messageSection.EstablishSecurityContext,
                    ClientCredentialType = messageSection.ClientCredentialType,
                    NegotiateServiceCredential = messageSection.NegotiateServiceCredential,
                    AlgorithmSuite = messageSection.AlgorithmSuite
                };

                var transportSection = section.Security.Transport;
                var transport = new HttpTransportSecurity
                {
                    ClientCredentialType = transportSection.ClientCredentialType,
                    ProxyCredentialType = transportSection.ProxyCredentialType
                };

                var wsHttpSecuritySection = section.Security;
                var wsHttpSecurity = new WSHttpSecurity
                {
                    Mode = wsHttpSecuritySection.Mode,
                    Transport = transport,
                    Message = message
                };
                ;
                binding.Security = wsHttpSecurity;
                if (readerQuotas != null)
                {
                    binding.ReaderQuotas = readerQuotas;
                }
                binding.ReliableSession = reliableSession;

                configs.Add(binding);
            }
            return configs;
        }

        public static List<NetMsmqBinding> GetNetMsmqBindings(string exeConfigPath)
        {
            if (string.IsNullOrWhiteSpace(exeConfigPath))
                return null;

            var svcSection = Read.Config.ExeConfig.GetServiceModelSection(exeConfigPath);

            var configs = new List<NetMsmqBinding>();
            foreach (
                var section in
                    svcSection.Bindings.NetMsmqBinding.ConfiguredBindings
                        .Cast<NetMsmqBindingElement>())
            {
                var df = new NetMsmqBinding();
                var binding = new NetMsmqBinding
                {
                    Name = section.Name,
                    MaxBufferPoolSize = section.MaxBufferPoolSize > 0 ? section.MaxBufferPoolSize : df.MaxBufferPoolSize,
                    MaxReceivedMessageSize = section.MaxReceivedMessageSize > 0 ? section.MaxReceivedMessageSize : df.MaxReceivedMessageSize,
                    CloseTimeout = section.CloseTimeout != TimeSpan.Zero ? section.CloseTimeout : df.CloseTimeout,
                    OpenTimeout = section.OpenTimeout != TimeSpan.Zero ? section.OpenTimeout : df.OpenTimeout,
                    SendTimeout = section.SendTimeout != TimeSpan.Zero ? section.SendTimeout : df.SendTimeout,
                    ReceiveTimeout =
                        section.ReceiveTimeout != TimeSpan.Zero ? section.ReceiveTimeout : df.ReceiveTimeout,

                    MaxRetryCycles = section.MaxRetryCycles > 0 ? section.MaxRetryCycles : df.MaxRetryCycles,
                    ReceiveRetryCount = section.ReceiveRetryCount > 0 ? section.ReceiveRetryCount : df.ReceiveRetryCount,
                    RetryCycleDelay = section.RetryCycleDelay != TimeSpan.Zero ? section.RetryCycleDelay : df.RetryCycleDelay,
                    TimeToLive = section.TimeToLive != TimeSpan.Zero ? section.TimeToLive : df.TimeToLive,

                        
                    DeadLetterQueue = section.DeadLetterQueue,
                    Durable = section.Durable,
                    ExactlyOnce = section.ExactlyOnce,
                    ReceiveErrorHandling = section.ReceiveErrorHandling,
                    UseSourceJournal = section.UseSourceJournal,
                    UseMsmqTracing = section.UseMsmqTracing,
                    QueueTransferProtocol = section.QueueTransferProtocol,
                    UseActiveDirectory = section.UseActiveDirectory
                };

                var readerQuotasSection = section.ReaderQuotas;
                var readerQuotas = new System.Xml.XmlDictionaryReaderQuotas();
                if (readerQuotasSection != null && readerQuotasSection.MaxDepth > 0)
                {
                    readerQuotas.MaxDepth = readerQuotasSection.MaxDepth;
                    readerQuotas.MaxStringContentLength = readerQuotasSection.MaxStringContentLength;
                    readerQuotas.MaxArrayLength = readerQuotasSection.MaxArrayLength;
                    readerQuotas.MaxBytesPerRead = readerQuotasSection.MaxBytesPerRead;
                    readerQuotas.MaxNameTableCharCount = readerQuotasSection.MaxNameTableCharCount;
                }
                else
                {
                    readerQuotas = null;
                }
                var msmqSecurity = new NetMsmqSecurity {Mode = section.Security.Mode};
                var securityTransportSection = section.Security.Transport;
                var msmqSecurityTransport = new MsmqTransportSecurity
                {
                    MsmqAuthenticationMode = securityTransportSection.MsmqAuthenticationMode,
                    MsmqEncryptionAlgorithm = securityTransportSection.MsmqEncryptionAlgorithm,
                    MsmqProtectionLevel = securityTransportSection.MsmqProtectionLevel,
                    MsmqSecureHashAlgorithm = securityTransportSection.MsmqSecureHashAlgorithm
                };
                var msmqSecurityMessage = new MessageSecurityOverMsmq
                {
                    ClientCredentialType = section.Security.Message.ClientCredentialType
                };
                msmqSecurity.Message = msmqSecurityMessage;
                msmqSecurity.Transport = msmqSecurityTransport;
                binding.Security = msmqSecurity;
                if (readerQuotas != null)
                {
                    binding.ReaderQuotas = readerQuotas;
                }
                configs.Add(binding);

            }
            return configs;
        }

        public static List<WS2007FederationHttpBinding> Get07FederationHttpBindings(string exeConfigPath)
        {
            if (string.IsNullOrWhiteSpace(exeConfigPath))
                return null;

            var svcSection = Read.Config.ExeConfig.GetServiceModelSection(exeConfigPath);
            var defaultTimeout = new TimeSpan(0, 0, 60);
            var configs = new List<WS2007FederationHttpBinding>();
            var dfltb = new WS2007FederationHttpBinding();
            foreach (
                var section in
                    svcSection.Bindings.WS2007FederationHttpBinding.ConfiguredBindings
                        .Cast<WS2007FederationHttpBindingElement>())
            {
                var binding = new WS2007FederationHttpBinding
                {
                    Name = section.Name,
                    CloseTimeout = section.CloseTimeout != TimeSpan.Zero ? section.CloseTimeout : defaultTimeout,
                    OpenTimeout = section.OpenTimeout != TimeSpan.Zero ? section.OpenTimeout : defaultTimeout,
                    SendTimeout = section.SendTimeout != TimeSpan.Zero ? section.SendTimeout : defaultTimeout,
                    ReceiveTimeout =
                        section.ReceiveTimeout != TimeSpan.Zero ? section.ReceiveTimeout : defaultTimeout,
                    BypassProxyOnLocal = section.BypassProxyOnLocal,
                    TransactionFlow = section.TransactionFlow,
                    HostNameComparisonMode = section.HostNameComparisonMode,

                    MaxReceivedMessageSize =
                        section.MaxReceivedMessageSize > 0
                            ? section.MaxReceivedMessageSize
                            : dfltb.MaxReceivedMessageSize,
                    MaxBufferPoolSize =
                        section.MaxBufferPoolSize > 0 ? section.MaxBufferPoolSize : dfltb.MaxBufferPoolSize,
                    MessageEncoding = section.MessageEncoding,
                    TextEncoding = section.TextEncoding ?? dfltb.TextEncoding,
                    UseDefaultWebProxy = section.UseDefaultWebProxy,
                };
                var readerQuotasSection = section.ReaderQuotas;
                var readerQuotas = new System.Xml.XmlDictionaryReaderQuotas();
                if (readerQuotasSection != null && readerQuotasSection.MaxDepth > 0)
                {
                    readerQuotas.MaxDepth = readerQuotasSection.MaxDepth;
                    readerQuotas.MaxStringContentLength = readerQuotasSection.MaxStringContentLength;
                    readerQuotas.MaxArrayLength = readerQuotasSection.MaxArrayLength;
                    readerQuotas.MaxBytesPerRead = readerQuotasSection.MaxBytesPerRead;
                    readerQuotas.MaxNameTableCharCount = readerQuotasSection.MaxNameTableCharCount;
                }
                else
                {
                    readerQuotas = null;
                }
                if (readerQuotas != null)
                {
                    binding.ReaderQuotas = readerQuotas;
                }

                if (section.Security == null)
                {
                    configs.Add(binding);
                    continue;
                }

                var cfgSecurityMsg = section.Security.Message;
                if (cfgSecurityMsg == null)
                {
                    configs.Add(binding);
                    continue;
                }

                var security = new WSFederationHttpSecurity
                {
                    Mode = section.Security.Mode,
                    Message = new FederatedMessageSecurityOverHttp
                    {
                        EstablishSecurityContext = cfgSecurityMsg.EstablishSecurityContext,
                    }
                };
                var wsSecurityMsg = security.Message;



                if (cfgSecurityMsg.IssuerMetadata?.Address != null)
                    wsSecurityMsg.IssuerMetadataAddress = new EndpointAddress(cfgSecurityMsg.IssuerMetadata.Address);

                if (!string.IsNullOrWhiteSpace(cfgSecurityMsg.IssuedTokenType))
                    wsSecurityMsg.IssuedTokenType = cfgSecurityMsg.IssuedTokenType;

                if(cfgSecurityMsg.Issuer?.Address != null)
                    wsSecurityMsg.IssuerAddress = new EndpointAddress(cfgSecurityMsg.Issuer.Address);

                if (cfgSecurityMsg.ClaimTypeRequirements != null && cfgSecurityMsg.ClaimTypeRequirements.Count > 0)
                {
                    foreach (var ctr in cfgSecurityMsg.ClaimTypeRequirements.Cast<ClaimTypeElement>())
                    {
                        wsSecurityMsg.ClaimTypeRequirements.Add(new ClaimTypeRequirement(ctr.ClaimType, ctr.IsOptional));   
                    }
                }

                binding.Security = security;

                configs.Add(binding);
            }

            return configs;
        }

        /// <summary>
        /// Creates a custom binding based on an example generated from svcutil.exe
        /// </summary>
        /// <param name="exeConfigPath"></param>
        /// <returns></returns>
        /// <remarks>
        /// https://msdn.microsoft.com/en-us/library/ms731690(v=vs.110).aspx
        /// </remarks>
        public static List<CustomBinding> GetCustomBindings(string exeConfigPath)
        {
            if (string.IsNullOrWhiteSpace(exeConfigPath))
                return null;

            var svcSection = Read.Config.ExeConfig.GetServiceModelSection(exeConfigPath);
            var configs = new List<CustomBinding>();
            foreach (var section in svcSection.Bindings.CustomBinding.ConfiguredBindings.Cast<CustomBindingElement>())
            {
                var binding = new CustomBinding
                {
                    Name = section.Name,
                };
                var cfgSecurity = section[0] as SecurityElement;

                if (cfgSecurity == null)
                {
                    configs.Add(binding);
                    continue;
                }

                var mode = cfgSecurity.AuthenticationMode;
                var msgSecurityVersion = cfgSecurity.MessageSecurityVersion;
                var issuedTokenParameter = new IssuedSecurityTokenParameters();
                if (cfgSecurity.IssuedTokenParameters.AdditionalRequestParameters != null)
                {
                    foreach (var arp in cfgSecurity.IssuedTokenParameters.AdditionalRequestParameters.Cast<XmlElementElement>())
                    {
                        issuedTokenParameter.AdditionalRequestParameters.Add(arp.XmlElement);
                    }
                }
                if (cfgSecurity.IssuedTokenParameters.Issuer?.Address != null)
                {
                    var address = cfgSecurity.IssuedTokenParameters.Issuer.Address;
                    var idElem = cfgSecurity.IssuedTokenParameters.Issuer.Identity;

                    issuedTokenParameter.IssuerAddress = GetEnpointAddressWithIdentity(address, idElem);
                }

                if (cfgSecurity.IssuedTokenParameters.Issuer?.Binding != null)
                    issuedTokenParameter.IssuerBinding = GetBindingByName(cfgSecurity.IssuedTokenParameters.Issuer.Binding);

                if (cfgSecurity.IssuedTokenParameters.IssuerMetadata?.Address != null)
                {
                    var address = cfgSecurity.IssuedTokenParameters.IssuerMetadata.Address;
                    var idElem = cfgSecurity.IssuedTokenParameters.IssuerMetadata.Identity;
                    issuedTokenParameter.IssuerMetadataAddress = GetEnpointAddressWithIdentity(address, idElem);
                }

                SecurityBindingElement securityElemnt;
                switch (mode)
                {
                    case AuthenticationMode.IssuedTokenOverTransport:
                        securityElemnt =
                            SecurityBindingElement.CreateIssuedTokenOverTransportBindingElement(issuedTokenParameter);
                        break;
                    case AuthenticationMode.AnonymousForCertificate:
                        securityElemnt = SecurityBindingElement.CreateAnonymousForCertificateBindingElement();
                        break;
                    case AuthenticationMode.AnonymousForSslNegotiated:
                        securityElemnt = SecurityBindingElement.CreateSslNegotiationBindingElement(false);
                        break;
                    case AuthenticationMode.CertificateOverTransport:
                        securityElemnt =
                            SecurityBindingElement.CreateCertificateOverTransportBindingElement(msgSecurityVersion);
                        break;
                    case AuthenticationMode.IssuedToken:
                        securityElemnt = SecurityBindingElement.CreateIssuedTokenBindingElement(issuedTokenParameter);
                        break;
                    case AuthenticationMode.IssuedTokenForCertificate:
                        securityElemnt = SecurityBindingElement.CreateIssuedTokenForCertificateBindingElement(issuedTokenParameter);
                        break;
                    case AuthenticationMode.IssuedTokenForSslNegotiated:
                        securityElemnt = SecurityBindingElement.CreateIssuedTokenForSslBindingElement(issuedTokenParameter);
                        break;
                    case AuthenticationMode.Kerberos:
                        securityElemnt = SecurityBindingElement.CreateKerberosBindingElement();
                        break;
                    case AuthenticationMode.KerberosOverTransport:
                        securityElemnt = SecurityBindingElement.CreateKerberosOverTransportBindingElement();
                        break;
                    case AuthenticationMode.MutualCertificate:
                        securityElemnt = SecurityBindingElement.CreateMutualCertificateBindingElement(msgSecurityVersion);
                        break;
                    case AuthenticationMode.MutualCertificateDuplex:
                        securityElemnt = SecurityBindingElement.CreateMutualCertificateDuplexBindingElement(msgSecurityVersion);
                        break;
                    case AuthenticationMode.MutualSslNegotiated:
                        securityElemnt = SecurityBindingElement.CreateSslNegotiationBindingElement(false);
                        break;
                    case AuthenticationMode.SspiNegotiated:
                        securityElemnt = SecurityBindingElement.CreateSspiNegotiationBindingElement();
                        break;
                    case AuthenticationMode.SspiNegotiatedOverTransport:
                        securityElemnt = SecurityBindingElement.CreateSspiNegotiationOverTransportBindingElement();
                        break;
                    case AuthenticationMode.UserNameForCertificate:
                        securityElemnt = SecurityBindingElement.CreateUserNameForCertificateBindingElement();
                        break;
                    case AuthenticationMode.UserNameForSslNegotiated:
                        securityElemnt = SecurityBindingElement.CreateUserNameForSslBindingElement();
                        break;
                    case AuthenticationMode.UserNameOverTransport:
                        securityElemnt = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
                        break;
                    default:
                        throw new NotImplementedException();
                }

                securityElemnt.AllowInsecureTransport = cfgSecurity.AllowInsecureTransport;
                securityElemnt.DefaultAlgorithmSuite = cfgSecurity.DefaultAlgorithmSuite;
                securityElemnt.EnableUnsecuredResponse = cfgSecurity.EnableUnsecuredResponse;
                securityElemnt.IncludeTimestamp = cfgSecurity.IncludeTimestamp;
                securityElemnt.MessageSecurityVersion = cfgSecurity.MessageSecurityVersion;
                securityElemnt.KeyEntropyMode = cfgSecurity.KeyEntropyMode;
                securityElemnt.ProtectTokens = cfgSecurity.ProtectTokens;
                securityElemnt.SecurityHeaderLayout = cfgSecurity.SecurityHeaderLayout;
                securityElemnt.SetKeyDerivation(cfgSecurity.RequireDerivedKeys);

                binding.Elements.Add(securityElemnt);
                configs.Add(binding);
            }

            return configs;
        }

        public static List<NetTcpBinding> GetNetTcpBindings(string exeConfigPath)
        {
            if (string.IsNullOrWhiteSpace(exeConfigPath))
                return null;

            var svcSection = Read.Config.ExeConfig.GetServiceModelSection(exeConfigPath);
            var defaultTimeout = new TimeSpan(0, 0, 60);
            var configs = new List<NetTcpBinding>();
            foreach (
                var section in
                    svcSection.Bindings.NetTcpBinding.ConfiguredBindings
                        .Cast<NetTcpBindingElement>())
            {
                var dfltb = new NetTcpBinding();
                var binding = new NetTcpBinding
                {
                    Name = section.Name,
                    CloseTimeout = section.CloseTimeout != TimeSpan.Zero ? section.CloseTimeout : defaultTimeout,
                    OpenTimeout = section.OpenTimeout != TimeSpan.Zero ? section.OpenTimeout : defaultTimeout,
                    SendTimeout = section.SendTimeout != TimeSpan.Zero ? section.SendTimeout : defaultTimeout,
                    ReceiveTimeout =
                        section.ReceiveTimeout != TimeSpan.Zero ? section.ReceiveTimeout : defaultTimeout,
                    MaxReceivedMessageSize =
                        section.MaxReceivedMessageSize > 0
                            ? section.MaxReceivedMessageSize
                            : dfltb.MaxReceivedMessageSize,
                    MaxBufferPoolSize =
                        section.MaxBufferPoolSize > 0 ? section.MaxBufferPoolSize : dfltb.MaxBufferPoolSize,
                    MaxConnections = section.MaxConnections > 0 ? section.MaxConnections : dfltb.MaxConnections,

                    ListenBacklog = section.ListenBacklog > 0 ? section.ListenBacklog : dfltb.ListenBacklog,
                    PortSharingEnabled = section.PortSharingEnabled,
                    TransactionFlow = section.TransactionFlow,
                    TransferMode = section.TransferMode,
                    HostNameComparisonMode = section.HostNameComparisonMode
                };
                var readerQuotasSection = section.ReaderQuotas;
                var readerQuotas = new System.Xml.XmlDictionaryReaderQuotas();
                if (readerQuotasSection != null && readerQuotasSection.MaxDepth > 0)
                {
                    readerQuotas.MaxDepth = readerQuotasSection.MaxDepth;
                    readerQuotas.MaxStringContentLength = readerQuotasSection.MaxStringContentLength;
                    readerQuotas.MaxArrayLength = readerQuotasSection.MaxArrayLength;
                    readerQuotas.MaxBytesPerRead = readerQuotasSection.MaxBytesPerRead;
                    readerQuotas.MaxNameTableCharCount = readerQuotasSection.MaxNameTableCharCount;
                }
                else
                {
                    readerQuotas = null;
                }
                var netTcpSecurity = new NetTcpSecurity { Mode = section.Security.Mode };
                var tcpTransportSecurity = new TcpTransportSecurity();

                var msgSecurityOverTcp = new MessageSecurityOverTcp
                {
                    ClientCredentialType = section.Security.Message.ClientCredentialType,
                    AlgorithmSuite = section.Security.Message.AlgorithmSuite
                };
                netTcpSecurity.Message = msgSecurityOverTcp;
                netTcpSecurity.Transport = tcpTransportSecurity;
                binding.Security = netTcpSecurity;
                if (readerQuotas != null)
                {
                    binding.ReaderQuotas = readerQuotas;
                }
                binding.ReliableSession = new OptionalReliableSession
                {
                    Enabled = section.ReliableSession.Enabled,
                    InactivityTimeout = section.ReliableSession.InactivityTimeout,
                    Ordered = section.ReliableSession.Ordered
                };


                configs.Add(binding);

            }
            return configs;
        }

        internal static EndpointAddress GetEnpointAddressWithIdentity(Uri address, IdentityElement idElem)
        {
            if (address == null)
                return null;

            if (idElem == null)
                return new EndpointAddress(address);

            if (!string.IsNullOrWhiteSpace(idElem.Certificate?.EncodedValue))
            {
                return new EndpointAddress(address,
                    new X509CertificateEndpointIdentity(
                        new X509Certificate2(Convert.FromBase64String(idElem.Certificate.EncodedValue))));
            }

            if (!string.IsNullOrWhiteSpace(idElem.Dns ?.Value))
            {
                return new EndpointAddress(address, new DnsEndpointIdentity(idElem.Dns.Value));

            }

            if (!string.IsNullOrWhiteSpace(idElem.Rsa?.Value))
            {
                return new EndpointAddress(address, new RsaEndpointIdentity(idElem.Rsa.Value));

            }

            if (!string.IsNullOrWhiteSpace(idElem.UserPrincipalName?.Value))
            {
                return new EndpointAddress(address, new UpnEndpointIdentity(idElem.UserPrincipalName.Value));
            }

            if (!string.IsNullOrWhiteSpace(idElem.ServicePrincipalName?.Value))
            {
                return new EndpointAddress(address, new SpnEndpointIdentity(idElem.ServicePrincipalName.Value));
            }

            return new EndpointAddress(address);
        }

        public static Binding GetBindingByName(string bindingName)
        {
            if(string.IsNullOrWhiteSpace(bindingName))
                return new CustomBinding();
            var bname = bindingName.ToLower().Replace("binding","");
            
            switch (bname)
            {
                case "custom":
                    return new CustomBinding();
                case "netnamedpipe":
                    return new NetNamedPipeBinding();
                case "nettcp":
                    return new NetTcpBinding();
                case "wsdualhttp":
                    return new WSDualHttpBinding();
                case "basichttp":
                    return new BasicHttpBinding();
                case "basichttps":
                    return new BasicHttpsBinding();
                case "nethttp":
                    return new NetHttpBinding();
                case "nethttps":
                    return new NetHttpsBinding();
                case "netmsmq":
                    return new NetMsmqBinding();
                case "wsfederationhttp":
                    return new WSFederationHttpBinding();
                case "wshttp":
                    return new WSHttpBinding();
                case "ws2007federationhttp":
                    return new WS2007FederationHttpBinding();
                case "ws2007http":
                    return new WS2007HttpBinding();
                case "wshttpcontext":
                    return new WSHttpContextBinding();
            }

            throw new NotImplementedException($"Don't know which binding '{bindingName}' is.");
        }
    }
}
