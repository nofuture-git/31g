using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Exceptions;
using NoFuture.Shared;
using NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis
{
    /// <summary>
    /// A console program to host assembly-token resolutions.
    /// As a separate process this keeps the calling <see cref="AppDomain"/>
    /// from becoming clustered with loaded assemblies.  It will
    /// continue running and listening on the given sockets.
    /// </summary>
    public class Program
    {
        #region constants
        public const int LISTEN_NUM_REQUEST = 5;
        #endregion

        #region fields
        private static readonly object _printLock = new object();
        private static int? _getTokenNamesCmdPort;
        private static int? _getTokenIdsCmdPort;
        private static int? _getasmIndicesCmdPort;

        private static readonly Dictionary<int, MetadataTokenName> _resolutionCache =
            new Dictionary<int, MetadataTokenName>();
        private static readonly List<int> _disolutionCache = new List<int>();
        private static TaskFactory _taskFactory;
        private static string _logName;
        private static readonly AsmIndicies _asmIndices = new AsmIndicies();
        #endregion

        #region properties

        internal static bool AsmInited
        {
            get
            {
                return Assembly != null &&
                       ManifestModule != null &&
                       AsmIndicies.Asms != null &&
                       AsmIndicies.Asms.Length > 0;
            }
        }

        internal static Assembly Assembly { get; set; }
        internal static Module ManifestModule { get; set; }

        internal static Dictionary<int, MetadataTokenName> ResolutionCache
        {
            get { return _resolutionCache; }
        }

        internal static List<int> DisolutionCache
        {
            get { return _disolutionCache; }
        }

        internal static AsmIndicies AsmIndicies
        {
            get { return _asmIndices; }
        }

        internal static int? GetTokenNamesCmdPort
        {
            get
            {
                if (Net.IsValidPortNumber(_getTokenNamesCmdPort))
                    return _getTokenNamesCmdPort;

                _getTokenNamesCmdPort = ResolvePort("GetTokenNamesCmdPort");
                return _getTokenNamesCmdPort;
            }
        }

        internal static int? GetTokenIdsCmdPort
        {
            get
            {
                if (Net.IsValidPortNumber(_getTokenIdsCmdPort))
                    return _getTokenIdsCmdPort;

                _getTokenIdsCmdPort = ResolvePort("GetTokenIdsCmdPort");
                return _getTokenIdsCmdPort;
            }
        }

        internal static int? GetAsmIndicesCmdPort
        {
            get
            {
                if (Net.IsValidPortNumber(_getasmIndicesCmdPort))
                    return _getasmIndicesCmdPort;

                _getasmIndicesCmdPort = ResolvePort("GetAsmIndicesCmdPort");
                return _getasmIndicesCmdPort;
            }
        }

        internal static string LogDirectory
        {
            get
            {
                var logDir = ConfigurationManager.AppSettings["NoFuture.TempDirectories.Debug"];
                if (string.IsNullOrWhiteSpace(logDir))
                    logDir = TempDirectories.AppData;
                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);
                return logDir;
            }
        }

        internal static string LogFile
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_logName))
                    return _logName;
                var name = Assembly.GetExecutingAssembly().GetName().Name;
                _logName = Path.Combine(LogDirectory, string.Format("{0}.log", name));
                return _logName;
            }
        }
        #endregion

        #region methods
        internal static int? ResolvePort(string appKey)
        {
            var cval = ConfigurationManager.AppSettings[appKey];
            return ResolveInt(cval);
        }

        internal static int? ResolveInt(string cval)
        {
            int valOut;
            if (!string.IsNullOrWhiteSpace(cval) && int.TryParse(cval, out valOut))
                return valOut;
            return null;
        }

        internal static void SetReflectionOnly()
        {
            bool useReflectionOnly;
            var p = ConfigurationManager.AppSettings["NoFuture.Shared.Constants.UseReflectionOnlyLoad"];
            if (!string.IsNullOrWhiteSpace(p) && bool.TryParse(p, out useReflectionOnly))
            {
                Constants.UseReflectionOnlyLoad = useReflectionOnly;
            }
            else
            {
                Constants.UseReflectionOnlyLoad = false;
            }
        }

        public static void Main(string[] args)
        {
            var ut = string.Empty;
            try
            {
                //test if there are any args, if not just leave with no message
                if (args.Length > 0 && (args[0] == "-h" || args[0] == "-help" || args[0] == "/?" || args[0] == "--help"))
                {
                    Console.WriteLine(Help());
                    return;
                }

                ConsoleCmd.SetConsoleAsTransparent();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Title = Assembly.GetExecutingAssembly().GetName().Name;

                PrintToConsole("New console started!");

                SetReflectionOnly();
                FxPointers.AddResolveAsmEventHandlerToDomain();

                //get and test the cmd line arg key\values
                var argHash = ConsoleCmd.ArgHash(args);
                if (argHash.ContainsKey(AssemblyAnalysis.GET_TOKEN_IDS_PORT_CMD_SWITCH))
                {
                    _getTokenIdsCmdPort = ResolveInt(argHash[AssemblyAnalysis.GET_TOKEN_IDS_PORT_CMD_SWITCH].ToString());
                }
                if (argHash.ContainsKey(AssemblyAnalysis.GET_TOKEN_NAMES_PORT_CMD_SWITCH))
                {
                    _getTokenNamesCmdPort = ResolveInt(argHash[AssemblyAnalysis.GET_TOKEN_NAMES_PORT_CMD_SWITCH].ToString());
                }
                if (argHash.ContainsKey(AssemblyAnalysis.GET_ASM_INDICES_PORT_CMD_SWITCH))
                {
                    _getasmIndicesCmdPort = ResolveInt(argHash[AssemblyAnalysis.GET_ASM_INDICES_PORT_CMD_SWITCH].ToString());
                }

                PrintToConsole(string.Format("GetAsmIndicesCmdPort listening on port [{0}]", GetAsmIndicesCmdPort));
                PrintToConsole(string.Format("GetTokenIdsCmdPort listening on port [{0}]", GetTokenIdsCmdPort));
                PrintToConsole(string.Format("GetTokenNamesCmdPort listening on port [{0}]", GetTokenNamesCmdPort));
                PrintToConsole("type 'exit' to quit", false);
                LaunchListeners();

                for (;;) //ever
                {
                    ut = Console.ReadLine();//console app thread parks
                    if (!string.IsNullOrWhiteSpace(ut) &&
                        string.Equals(ut.Trim(), "exit", StringComparison.OrdinalIgnoreCase))
                        break;
                }

            }
            catch (Exception ex)
            {
                PrintToConsole(ex);
            }

            if (string.IsNullOrWhiteSpace(ut) || !ut.StartsWith("exit"))
            {
                Console.ReadKey();
            }
        }

        internal static void LaunchListeners()
        {
            if (!Net.IsValidPortNumber(GetTokenIdsCmdPort) || !Net.IsValidPortNumber(GetTokenNamesCmdPort) ||
                !Net.IsValidPortNumber(GetAsmIndicesCmdPort))
                throw new RahRowRagee("the command's ports are either null or invalid " +
                                      string.Format(" GetAsmIndicesCmdPort is port [{0}]", GetAsmIndicesCmdPort) +
                                      string.Format(" GetTokenIdsCmdPort is port [{0}]", GetTokenIdsCmdPort) +
                                      string.Format(" GetTokenNamesCmdPort is port [{0}]", GetTokenNamesCmdPort));

            _taskFactory = new TaskFactory();
            _taskFactory.StartNew(() => HostCmd(new GetTokenIds(), GetAsmIndicesCmdPort.Value));
            _taskFactory.StartNew(() => HostCmd(new GetTokenIds(), GetTokenIdsCmdPort.Value));
            _taskFactory.StartNew(() => HostCmd(new GetTokenNames(), GetTokenNamesCmdPort.Value));
        }

        internal static void HostCmd(ICmd cmd, int cmdPort)
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                //this should NOT be reachable from any other machine
                var endPt = new IPEndPoint(IPAddress.Loopback, cmdPort);
                socket.Bind(endPt);
                socket.Listen(LISTEN_NUM_REQUEST);

                for (; ; )//ever
                {
                    try
                    {
                        var buffer = new List<byte>();

                        var client = socket.Accept();
                        PrintToConsole(string.Format("Connect from port {0}", cmdPort));
                        var data = new byte[Constants.DEFAULT_BLOCK_SIZE];

                        //park for first data received
                        client.Receive(data, 0, data.Length, SocketFlags.None);
                        buffer.AddRange(data.Where(b => b != (byte)'\0'));
                        while (client.Available > 0)
                        {
                            if (client.Available < Constants.DEFAULT_BLOCK_SIZE)
                            {
                                data = new byte[client.Available];
                                client.Receive(data, 0, client.Available, SocketFlags.None);
                            }
                            else
                            {
                                data = new byte[Constants.DEFAULT_BLOCK_SIZE];
                                client.Receive(data, 0, (int) Constants.DEFAULT_BLOCK_SIZE, SocketFlags.None);
                            }
                            buffer.AddRange(data.Where(b => b != (byte)'\0'));
                        }

                        var output = cmd.Execute(buffer.ToArray());
                        client.Send(output);
                        client.Close();

                    }
                    catch (Exception ex)
                    {
                        PrintToConsole(ex);
                    }
                }
            }
        }

        internal static void PrintToConsole(string someString, bool trunc = true)
        {
            lock (_printLock)
            {
                File.AppendAllText(LogFile, string.Format("{0:yyyy-MM-dd HH:mm:ss.fffff} {1}\n", DateTime.Now, someString));
                if (trunc && someString.Length >= 54)
                    someString = string.Format("{0}[...]", someString.Substring(0, 48));
                
                Console.WriteLine(string.Format("{0:yyyy-MM-dd HH:mm:ss.fffff} {1}", DateTime.Now, someString));
            }
        }

        internal static void PrintToConsole(Exception ex)
        {
            lock (_printLock)
            {
                var msg = string.Format("{0:yyyy-MM-dd HH:mm:ss.fffff} {1}", DateTime.Now, ex.Message);
                File.AppendAllText(LogFile, string.Format("{0}\n", msg));
                Console.WriteLine(msg);

                msg = string.Format("{0:yyyy-MM-dd HH:mm:ss.fffff} {1}", DateTime.Now, ex.StackTrace);
                File.AppendAllText(LogFile, string.Format("{0}\n", msg));
                Console.WriteLine(msg);
            }
        }

        internal static String Help()
        {
            var help = new StringBuilder();
            help.AppendLine(" ----");
            help.AppendLine(string.Format(" [{0}] ", Assembly.GetExecutingAssembly().GetName().Name));
            help.AppendLine("");
            help.AppendLine(" ");
            help.AppendLine(" This exe will open a socket listeners on the ");
            help.AppendLine(" localhost for the ports specified in the  ");
            help.AppendLine(" config file.");
            help.AppendLine(" ");
            help.AppendLine(" Usage: options ");
            help.AppendLine(" Options:");
            help.AppendLine(" -h | -help             Will print this help.");
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[INT]      Optional, cmd line port for the ",
                Constants.CMD_LINE_ARG_SWITCH, AssemblyAnalysis.GET_TOKEN_IDS_PORT_CMD_SWITCH,
                Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine("                                 the DumpAllTokensCmd, defaults to app.config.");
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[INT]      Optional, cmd line port for the ",
                Constants.CMD_LINE_ARG_SWITCH, AssemblyAnalysis.GET_TOKEN_NAMES_PORT_CMD_SWITCH,
                Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine("                                 the ResolveTokensCmd, defaults to app.config.");
            help.AppendLine("");

            return help.ToString();
        }

        #endregion
    }
}
