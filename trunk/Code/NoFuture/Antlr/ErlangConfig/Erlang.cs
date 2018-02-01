using System;

namespace NoFuture.Tokens.ParseResults
{
    public class RabbitConfig
    {
        public int[] SslListeners { get; set; }
        public ErlangSslOptionsConfig SslOptionsConfig { get; set; }
        public string[] AuthMechanisms { get; set; }
        public string[] LoopBackUsers { get; set; }
        public string SslCertLoginFrom { get; set; }
    }

    public class ErlangSslOptionsConfig
    {
        public ErlangSslConfig SslVersions { get; set; }
        public string CaCertFile { get; set; }
        public string CertFile { get; set; }
        public string KeyFile { get; set; }
        public string Verify { get; set; }
        public bool FailIfNoPeerCert { get; set; }
    }

    public class ErlangSslConfig
    {
        public string[] Versions { get; set; }
    }

    public class RabbitMqTracingConfig
    {
        public string Directory { get; set; }
    }

    public class RabbitMqMgmtConfig
    {
        public int Port { get; set; }
        public string Ip { get; set; }
        public bool EnableSsl { get; set; }
        public ErlangSslOptionsConfig SslOptionsConfig { get; set; }
        public RabbitMqMgmtRenPolicyConfig Global { get; set; }
        public RabbitMqMgmtRenPolicyConfig Basic { get; set; }
        public RabbitMqMgmtRenPolicyConfig Detailed { get; set; }
    }

    public class RabbitMqMgmtRenPolicyConfig
    {
        public Tuple<int, int>[] PolicyValues { get; set; }
    }

    public class ErlangConfig
    {
        public RabbitConfig Rabbit { get; set; }
        public ErlangSslConfig Ssl { get; set; }
        public RabbitMqMgmtConfig MgmtRabbit { get; set; }
        public RabbitMqTracingConfig Tracing { get; set; }
    }
}
