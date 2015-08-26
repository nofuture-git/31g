using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;

namespace NoFuture.Shared.WcfClient
{
    public class Bindings
    {
        public static List<BasicHttpBinding> GetBasicHttpBindings(string exeConfigPath)
        {
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

        public static List<NetTcpBinding> GetNetTcpBindings(string exeConfigPath)
        {
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
                var netTcpSecurity = new NetTcpSecurity() { Mode = section.Security.Mode };
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
    }
}
