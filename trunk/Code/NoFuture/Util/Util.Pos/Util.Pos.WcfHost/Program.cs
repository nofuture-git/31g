using System;
using System.IO;
using System.ServiceModel;
using System.Text;

namespace NoFuture.Util.Pos.WcfHost
{
    public static class Program
    {
        internal static ServiceHost _svcHost = null;
        internal static int exceptionCount = 0;
        private static int _maxExceptionCount;
        private static object _printLock = new object();
        private const string HOST_URI = "net.tcp://localhost/PosParser";
        private const string EN_SVC = "English";
        private const string MEX = "mex";

        public static int MaxExceptionsCount
        {
            get
            {
                if (_maxExceptionCount > 0) return _maxExceptionCount < 0 ? 10 : _maxExceptionCount;
                int configVal;
                _maxExceptionCount =
                    !int.TryParse(System.Configuration.ConfigurationManager.AppSettings["MaxExceptionsCount"],
                        out configVal)
                        ? 10
                        : configVal;
                return _maxExceptionCount < 0 ? 10 : _maxExceptionCount;
            }
        }

        public static string NfRootBinFolder
        {
            get
            {
                var cval = System.Configuration.ConfigurationManager.AppSettings["NoFuture.BinDirectories.Root"];
                return string.IsNullOrWhiteSpace(cval) || !Directory.Exists(cval) ? string.Empty : cval;
            }
        }

        public static string NfSvcUtilDir
        {
            get
            { 
                var cval = System.Configuration.ConfigurationManager.AppSettings["NoFuture.TempDirectories.SvcUtil"];
                return string.IsNullOrWhiteSpace(cval) || !Directory.Exists(cval) ? string.Empty : cval;
            }
        }

        /// <summary>
        /// Uses default port of 808
        /// </summary>
        public static Uri HostUri
        {
            get
            {
                var compName = Environment.GetEnvironmentVariable("COMPUTERNAME");
                var compDomain = Environment.GetEnvironmentVariable("USERDNSDOMAIN");
                if (string.IsNullOrWhiteSpace(compName) || string.IsNullOrWhiteSpace(compDomain))
                    return new Uri(HOST_URI);
                var uriBldr = new UriBuilder("net:tcp", string.Format("{0}.{1}", compName, compDomain))
                {
                    Path = "PosParser"
                };
                return uriBldr.Uri;
            }
        }

        public static void Main(string[] args)
        {
            var ut = string.Empty;
            try
            {
                ConsoleCmd.SetConsoleAsTransparent(true);
                Console.ForegroundColor = ConsoleColor.DarkRed;

                JavaTools.StanfordPostTaggerModels =
                    System.Configuration.ConfigurationManager.AppSettings["NoFuture.JavaTools.StanfordPostTaggerModels"];

                using (
                    _svcHost =
                        new ServiceHost(typeof (EnPosParser),
                            new[] {HostUri}))
                {
                    var tcpBind = new NetTcpBinding();
                    _svcHost.AddServiceEndpoint(typeof(IPosParser), tcpBind, EN_SVC);

                    var behavior = new System.ServiceModel.Description.ServiceMetadataBehavior();
                    _svcHost.Description.Behaviors.Add(behavior);

                    var mex = System.ServiceModel.Description.MetadataExchangeBindings.CreateMexTcpBinding();
                    _svcHost.AddServiceEndpoint(typeof (System.ServiceModel.Description.IMetadataExchange), mex,
                        MEX);

                    PrintToConsole("Opening this host.");
                    _svcHost.Open();

                    for (;;) //ever
                    {
                        ut = Console.ReadLine();//thread parks here
                        ut = string.IsNullOrWhiteSpace(ut) ? string.Empty : ut.Trim().ToLower();
                        if (ut.StartsWith("exit") || exceptionCount >= MaxExceptionsCount)
                            break;

                        var svcUtilCmd = CtorMySvcUtilCmd();
                        PrintToConsole("Run the following commnad to generate a proxy -OR- enter 'exit' to quit.", false);
                        PrintToConsole(string.Format("\n{0}", svcUtilCmd), false);
                    }

                    PrintToConsole("Closing this host.");
                    _svcHost.Close();
                }
            }
            catch (Exception ex)
            {
                exceptionCount += 1;
                PrintToConsole(ex);
            }

            PrintToConsole("press any key to exit or close the console window.", false);

            if (string.IsNullOrWhiteSpace(ut) || !ut.StartsWith("exit"))
            {
                var k = Console.ReadLine();
            }
        }

        public static void PrintToConsole(string someString, bool trunc = true)
        {
            lock (_printLock)
            {
                if (trunc && someString.Length > 55)
                    someString = string.Format("{0}[...]", someString.Substring(0, 48));

                Console.WriteLine(string.Format("{0:yyyy-MM-dd HH:mm:ss.fffff} {1}", DateTime.Now, someString));
            }
        }

        public static void PrintToConsole(System.Exception ex)
        {
            lock (_printLock)
            {
                Console.WriteLine(string.Format("{0:yyyy-MM-dd HH:mm:ss.fffff} {1}", DateTime.Now, ex.Message));
                Console.WriteLine(string.Format("{0:yyyy-MM-dd HH:mm:ss.fffff} {1}", DateTime.Now, ex.StackTrace));
            }
        }

        public static string CtorMySvcUtilCmd()
        {
            var svcUtilCmd = new StringBuilder();
            svcUtilCmd.Append("svcutil ");
            svcUtilCmd.Append(HOST_URI);
            svcUtilCmd.Append("/");
            svcUtilCmd.Append(MEX);
            svcUtilCmd.Append(" /out:");
            svcUtilCmd.AppendLine(Path.Combine(NfSvcUtilDir, "PosParserClient.cs"));
            svcUtilCmd.Append(" /config:");
            svcUtilCmd.AppendLine(Path.Combine(NfSvcUtilDir, "PosParserClient.config"));

            var nfAsms = new[] { "NoFuture.Shared.dll", "NoFuture.Util.dll", "NoFuture.Util.Pos.dll" };
            foreach (var nfAsm in nfAsms)
            {
                svcUtilCmd.Append(" /reference:");
                svcUtilCmd.AppendLine(Path.Combine(NfRootBinFolder, nfAsm));
            }
            svcUtilCmd.AppendLine(" /namespace:\"*,NoFuture\"");

            return svcUtilCmd.ToString();
        }
    }
}
