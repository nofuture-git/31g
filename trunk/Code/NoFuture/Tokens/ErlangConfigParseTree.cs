using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using NoFuture.Antlr.Grammers;
using NoFuture.Tokens.ParseResults;

namespace NoFuture.Tokens
{


    public class ErlangConfigParseTree : ErlangConfigBaseListener
    {
        #region consts
        private const string RabbitmqTracing = "rabbitmq_tracing";
        private const string Directory = "directory";
        private const string Ssl = "ssl";
        private const string ErlName = "versions";
        private const string Rabbit = "rabbit";
        private const string RabbitmqManagement = "rabbitmq_management";
        private const string Listener = "listener";
        private const string Port = "port";
        private const string Ip = "ip";
        private const string SslOpts = "ssl_opts";
        private const string Cacertfile = "cacertfile";
        private const string Certfile = "certfile";
        private const string Keyfile = "keyfile";
        private const string Global = "global";
        private const string Basic = "basic";
        private const string Detailed = "detailed";
        private const string LoopbackUsers = "loopback_users";
        private const string SslListeners = "ssl_listeners";
        private const string SslOptions = "ssl_options";
        private const string Verify = "verify";
        private const string FailIfNoPeerCert = "fail_if_no_peer_cert";
        private const string AuthMechanisms = "auth_mechanisms";
        private const string SslCertLoginFrom = "ssl_cert_login_from";
        #endregion

        #region fields
        private static ErlangSslConfig _app = new ErlangSslConfig();
        private static RabbitConfig _rabbit = new RabbitConfig();
        private static RabbitMqMgmtConfig _mgmtConfig = new RabbitMqMgmtConfig();
        private static RabbitMqTracingConfig _tracingConfig = new RabbitMqTracingConfig();
        private string _activeAppName = string.Empty;
        private ParseTreeProperty<string[]> _arrayOfAtomValue = new ParseTreeProperty<string[]>();
        private ParseTreeProperty<Tuple<string,string>> _erlNvAtomValue = new ParseTreeProperty<Tuple<string, string>>();
        private ParseTreeProperty<List<Tuple<string, string>>> _arrayOfNameValues = new ParseTreeProperty<List<Tuple<string, string>>>();
        #endregion

        #region BaseListener overrides
        public override void EnterErlApplication(ErlangConfigParser.ErlApplicationContext context)
        {
            _activeAppName = context.erlName().GetText();
        }

        public override void ExitNameValue2Value(ErlangConfigParser.NameValue2ValueContext context)
        {
            if (context.IsEmpty)
                return;
            var n = context.erlName().GetText();
            var v = context.erlAtomValue().GetText();

            var nv = new Tuple<string, string>(n, v);
            _erlNvAtomValue.Put(context, nv);

        }

        public override void ExitErlArray(ErlangConfigParser.ErlArrayContext context)
        {
            if (context.IsEmpty)
                return;
            if (context.erlAtomValue() != null && context.erlAtomValue().Any())
            {
                var k = context.erlAtomValue().Select(f => f.GetText()).ToList();

                _arrayOfAtomValue.Put(context, k.ToArray());

            }
            if (context.erlNameValuePair() != null && context.erlNameValuePair().Any())
            {
                var nvs = context.erlNameValuePair().Select(fc => _erlNvAtomValue.Get(fc)).Where(ff => ff != null).ToList();
                _arrayOfNameValues.Put(context, nvs);
            }
        }
        public override void ExitErlApplication(ErlangConfigParser.ErlApplicationContext context)
        {
            if (_activeAppName == RabbitmqTracing)
            {
                var fdkf = _arrayOfNameValues.Get(context.erlArray());
                if (fdkf != null && fdkf.Any(x => x.Item1 == Directory))
                {
                    _tracingConfig.Directory = fdkf.First(x => x.Item1 == Directory).Item2;
                }
            }
            if (_activeAppName == Rabbit)
            {
                var simpleNv = _arrayOfNameValues.Get(context.erlArray());
                if (simpleNv != null && simpleNv.Any(x => x.Item1 == SslCertLoginFrom))
                    _rabbit.SslCertLoginFrom = simpleNv.First(x => x.Item1 == SslCertLoginFrom).Item2;
            }
        }

        public override void ExitNameValue2Array(ErlangConfigParser.NameValue2ArrayContext context)
        {
            if (_activeAppName == Ssl)
            {
                string[] values = null;
                if (TryGetNvArrayValues(ErlName, context, out values))
                    _app.Versions = values;
            }
            if (_activeAppName == Rabbit)
            {
                GetRabbitConfigSection(context);
            }
            if (_activeAppName == RabbitmqManagement)
            {
                GetRabbitMqMgmtSection(context);
            }
        }
        #endregion

        #region internal helpers
        private void GetRabbitMqMgmtSection(ErlangConfigParser.NameValue2ArrayContext context)
        {
            if (context.erlName().GetText() == Listener)
            {
                var fkj = _arrayOfNameValues.Get(context.erlArray());
                if (fkj != null)
                {
                    var kj = fkj.FirstOrDefault(x => x.Item1 == Port);
                    if (kj != null)
                    {
                        int p;
                        if (int.TryParse(kj.Item2, out p))
                            _mgmtConfig.Port = p;
                    }
                    kj = fkj.FirstOrDefault(x => x.Item1 == Ip);
                    if (kj != null)
                        _mgmtConfig.Ip = kj.Item2;
                    kj = fkj.FirstOrDefault(x => x.Item1 == Ssl);
                    if (kj != null)
                    {
                        bool s;
                        if (bool.TryParse(kj.Item2, out s))
                        {
                            _mgmtConfig.EnableSsl = s;
                        }
                    }
                }
            }
            if (context.erlName().GetText() == SslOpts)
            {
                var fkj = _arrayOfNameValues.Get(context.erlArray());
                if (fkj != null)
                {
                    if (_mgmtConfig.SslOptionsConfig == null)
                    {
                        _mgmtConfig.SslOptionsConfig = new ErlangSslOptionsConfig();
                    }
                    var kj = fkj.FirstOrDefault(x => x.Item1 == Cacertfile);
                    if (kj != null)
                        _mgmtConfig.SslOptionsConfig.CaCertFile = kj.Item2;
                    kj = fkj.FirstOrDefault(x => x.Item1 == Certfile);
                    if (kj != null)
                        _mgmtConfig.SslOptionsConfig.CertFile = kj.Item2;
                    kj = fkj.FirstOrDefault(x => x.Item1 == Keyfile);
                    if (kj != null)
                        _mgmtConfig.SslOptionsConfig.KeyFile = kj.Item2;
                }
            }
            if (context.erlName().GetText() == Global)
            {
                Tuple<int, int>[] pvs = null;
                if (TryGetNvIntArrayValues(context, out pvs))
                {
                    _mgmtConfig.Global = new RabbitMqMgmtRenPolicyConfig { PolicyValues = pvs };
                }
            }
            if (context.erlName().GetText() == Basic)
            {
                Tuple<int, int>[] pvs = null;
                if (TryGetNvIntArrayValues(context, out pvs))
                {
                    _mgmtConfig.Basic = new RabbitMqMgmtRenPolicyConfig { PolicyValues = pvs };
                }
            }
            if (context.erlName().GetText() == Detailed)
            {
                Tuple<int, int>[] pvs = null;
                if (TryGetNvIntArrayValues(context, out pvs))
                {
                    _mgmtConfig.Detailed = new RabbitMqMgmtRenPolicyConfig { PolicyValues = pvs };
                }
            }
        }

        private void GetRabbitConfigSection(ErlangConfigParser.NameValue2ArrayContext context)
        {
            string[] values;
            if (TryGetNvArrayValues(LoopbackUsers, context, out values))
            {
                _rabbit.LoopBackUsers = values;
            }
            if (TryGetNvArrayValues(SslListeners, context, out values))
            {
                var ports = new List<int>();
                foreach (var v in values)
                {
                    int p;
                    if (!int.TryParse(v, out p))
                        continue;
                    ports.Add(p);
                }
                _rabbit.SslListeners = ports.ToArray();
            }

            if (context.erlName().GetText() == SslOptions)
            {
                var fkj = _arrayOfNameValues.Get(context.erlArray());
                if (fkj != null)
                {
                    if (_rabbit.SslOptionsConfig == null)
                    {
                        _rabbit.SslOptionsConfig = new ErlangSslOptionsConfig();
                    }
                    var kj = fkj.FirstOrDefault(x => x.Item1 == Cacertfile);
                    if (kj != null)
                        _rabbit.SslOptionsConfig.CaCertFile = kj.Item2;
                    kj = fkj.FirstOrDefault(x => x.Item1 == Certfile);
                    if (kj != null)
                        _rabbit.SslOptionsConfig.CertFile = kj.Item2;
                    kj = fkj.FirstOrDefault(x => x.Item1 == Keyfile);
                    if (kj != null)
                        _rabbit.SslOptionsConfig.KeyFile = kj.Item2;
                    kj = fkj.FirstOrDefault(x => x.Item1 == Verify);
                    if (kj != null)
                        _rabbit.SslOptionsConfig.Verify = kj.Item2;
                    kj = fkj.FirstOrDefault(x => x.Item1 == FailIfNoPeerCert);
                    if (kj != null)
                    {
                        bool finpc;
                        if (bool.TryParse(kj.Item2, out finpc))
                            _rabbit.SslOptionsConfig.FailIfNoPeerCert = finpc;
                    }
                }
            }

            if (context.erlName().GetText() == ErlName)
            {
                if (TryGetNvArrayValues(ErlName, context, out values))
                {
                    if (_rabbit.SslOptionsConfig == null)
                    {
                        _rabbit.SslOptionsConfig = new ErlangSslOptionsConfig();
                    }
                    _rabbit.SslOptionsConfig.SslVersions = new ErlangSslConfig {Versions = values};
                }
            }
            if (context.erlName().GetText() == AuthMechanisms)
            {
                if (TryGetNvArrayValues(AuthMechanisms, context, out values))
                    _rabbit.AuthMechanisms = values;
            }
        }

        private bool TryGetNvArrayValues(string erlName, ErlangConfigParser.NameValue2ArrayContext context, out string[] values)
        {
            values = null;
            if (context.erlName().GetText() != erlName || context.erlArray().erlEmptyArray() != null ||
                _arrayOfAtomValue.Get(context.erlArray()) == null) return false;
            values = _arrayOfAtomValue.Get(context.erlArray());
            return true;
        }

        private bool TryGetNvIntArrayValues(ErlangConfigParser.NameValue2ArrayContext context, out Tuple<int, int>[] values )
        {
            values = null;
            var policyCtx = _arrayOfNameValues.Get(context.erlArray());
            if (policyCtx == null) return false;
            var policyVals = new List<Tuple<int, int>>();
            foreach (var pv in policyCtx)
            {
                int i0;
                int j0;
                if (int.TryParse(pv.Item1, out i0) && int.TryParse(pv.Item2, out j0))
                {
                    policyVals.Add(new Tuple<int, int>(i0, j0));
                }
            }

            values = policyVals.ToArray();
            return true;
        }
        #endregion

        #region api
        public static ErlangConfig InvokeParse(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;
            if (!System.IO.File.Exists(fileName))
                return null;

            var tr = System.IO.File.OpenRead(fileName);
            var input = new AntlrInputStream(tr);
            var lexer = new ErlangConfigLexer(input);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ErlangConfigParser(tokens);

            var tree = parser.erlConfigFile();

            var walker = new ParseTreeWalker();
            var loader = new ErlangConfigParseTree();

            walker.Walk(loader, tree);

            var results = new ErlangConfig
            {
                MgmtRabbit = _mgmtConfig,
                Rabbit = _rabbit,
                Ssl = _app,
                Tracing = _tracingConfig
            };

            return results;
        }
        #endregion
    }
}
