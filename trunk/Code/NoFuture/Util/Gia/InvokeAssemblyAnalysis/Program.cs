using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
        private static bool? _resolveGacAsms;
        private static string[] _gacAsmNames;
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
                if (String.IsNullOrWhiteSpace(logDir))
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
                if (!String.IsNullOrWhiteSpace(_logName))
                    return _logName;
                var name = Assembly.GetExecutingAssembly().GetName().Name;
                _logName = Path.Combine(LogDirectory, String.Format("{0}.log", name));
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
            if (!String.IsNullOrWhiteSpace(cval) && Int32.TryParse(cval, out valOut))
                return valOut;
            return null;
        }

        internal static bool? ResolveBool(string cval)
        {
            bool valOut;
            if (!String.IsNullOrWhiteSpace(cval) && Boolean.TryParse(cval, out valOut))
                return valOut;
            return null;
        }

        internal static void SetReflectionOnly()
        {
            var useReflectionOnly =
                ResolveBool(ConfigurationManager.AppSettings["NoFuture.Shared.Constants.UseReflectionOnlyLoad"]);
            Constants.UseReflectionOnlyLoad = useReflectionOnly != null && useReflectionOnly.Value;
        }

        internal static bool ResolveGacAssemblies
        {
            get
            {
                if (_resolveGacAsms != null)
                    return _resolveGacAsms.Value;

                _resolveGacAsms = ResolveBool(ConfigurationManager.AppSettings["ResolveGacAssemblies"]);

                return _resolveGacAsms != null && _resolveGacAsms.Value;
            }
        }

        public static void Main(string[] args)
        {
            var ut = String.Empty;
            try
            {
                //test if there are any args, if not just leave with no message
                if (args.Length > 0 && (args[0] == "-h" || args[0] == "-help" || args[0] == "/?" || args[0] == "--help"))
                {
                    Console.WriteLine(Help());
                    return;
                }
                PrintToConsole("New console started!");

                //set app domain cfg
                ConsoleCmd.SetConsoleAsTransparent();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Title = Assembly.GetExecutingAssembly().GetName().Name;
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
                if (argHash.ContainsKey(AssemblyAnalysis.RESOLVE_GAC_ASM_SWITCH))
                {
                    _resolveGacAsms = ResolveBool(argHash[AssemblyAnalysis.RESOLVE_GAC_ASM_SWITCH].ToString());
                }

                //print settings
                PrintToConsole(String.Format("GetAsmIndicesCmdPort listening on port [{0}]", GetAsmIndicesCmdPort));
                PrintToConsole(String.Format("GetTokenIdsCmdPort listening on port [{0}]", GetTokenIdsCmdPort));
                PrintToConsole(String.Format("GetTokenNamesCmdPort listening on port [{0}]", GetTokenNamesCmdPort));
                PrintToConsole(String.Format("Resolve GAC Assembly names is [{0}]", ResolveGacAssemblies));
                PrintToConsole("type 'exit' to quit", false);

                //open ports
                LaunchListeners();

                //park main
                for (;;) //ever
                {
                    ut = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(ut))
                        continue;
                    if (String.Equals(ut.Trim(), "exit", StringComparison.OrdinalIgnoreCase))
                        break;
                    if (string.Equals(ut.Trim(), "help", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine(Help());
                        continue;
                    }
                    
                    if (!AsmInited)
                        continue;

                    //allow user to get token id by type name
                    if (System.Text.RegularExpressions.Regex.IsMatch(ut, TypeName.NAMESPACE_CLASS_NAME_REGEX))
                    {
                        var aType = ManifestModule.GetType(ut.Trim(), true);
                        if (aType == null)
                            continue;
                        PrintToConsole(string.Format("{0,-20}0x{1}","MetadataToken Id", aType.MetadataToken.ToString("X4")));
                    }

                    //allow user to get type name by token id
                    int utTokenId = 0;
                    if (ut.StartsWith("0x") &&
                        !Int32.TryParse(ut.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture,
                            out utTokenId))
                        continue;
                    if (utTokenId == 0 && !Int32.TryParse(ut, out utTokenId)) 
                        continue;

                    if (utTokenId > 0)
                    {
                        MetadataTokenName tokenNameOut;
                        if (!ResolveSingleToken(utTokenId, out tokenNameOut))
                            continue;
                        var asmName = AsmIndicies.Asms.FirstOrDefault(x => x.IndexId == tokenNameOut.AsmIndexId);

                        PrintToConsole(string.Format("{0,-20}{1}", "Name", tokenNameOut.Name));
                        PrintToConsole(string.Format("{0,-20}{1}", "Assembly",
                            asmName == null ? string.Empty : asmName.AssemblyName));
                        PrintToConsole(string.Format("{0,-20}{1}", "Label", tokenNameOut.Label));
                    }
                }
            }
            catch (Exception ex)
            {
                PrintToConsole(ex);
            }

            if (String.IsNullOrWhiteSpace(ut) || !ut.StartsWith("exit"))
            {
                Console.ReadKey();
            }
        }

        internal static bool PrintTypeNameByTokenId(string ut)
        {
            throw new NotImplementedException();
        }

        internal static bool PrintTokenIdByTypeName(string ut)
        {
            throw new NotImplementedException();
        }

        internal static bool ResolveSingleToken(int tokenId, out MetadataTokenName metadataToken)
        {
            metadataToken = new MetadataTokenName { Id = tokenId };
            if (tokenId == 0)
            {
                return false;
            }
            var cid = metadataToken.Id;

            MemberInfo mi;
            try
            {
                mi = ManifestModule.ResolveMember(cid);
            }
            catch (ArgumentException)//does not resolve the token
            {
                return false;
            }

            if (mi == null)
            {
                return false;
            }

            metadataToken.Id = metadataToken.Id;
            metadataToken.Name = mi.Name;
            metadataToken.Label = mi.GetType().Name;

            string asmName;

            var type = mi as Type;

            if (type != null)
            {
                //do not sent back GAC assemblies - too big
                asmName = type.Assembly.GetName().FullName;
                if (!ResolveGacAssemblies && IsIgnore(asmName))
                    return false;

                var t =
                    AsmIndicies.Asms.FirstOrDefault(
                        x =>
                            String.Equals(x.AssemblyName, type.Assembly.GetName().FullName,
                                StringComparison.OrdinalIgnoreCase));

                metadataToken.Label = type.Name;
                metadataToken.AsmIndexId = t != null ? t.IndexId : 0;
                return true;
            }
            if (mi.DeclaringType == null) return true;

            asmName = mi.DeclaringType.Assembly.GetName().FullName;
            if (!ResolveGacAssemblies && IsIgnore(asmName))
                return false;

            var f =
                AsmIndicies.Asms.FirstOrDefault(
                    x =>
                        String.Equals(x.AssemblyName, mi.DeclaringType.Assembly.GetName().FullName,
                            StringComparison.OrdinalIgnoreCase));

            metadataToken.Label = mi.DeclaringType.Name;
            metadataToken.AsmIndexId = f != null ? f.IndexId : 0;

            return true;
        }

        internal static string[] GacAssemblyNames
        {
            get
            {
                if (_gacAsmNames != null)
                    return _gacAsmNames;

                _gacAsmNames = Constants.UseReflectionOnlyLoad
                    ? AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies()
                        .Where(x => x.GlobalAssemblyCache)
                        .Select(x => x.FullName)
                        .ToArray()
                    : AppDomain.CurrentDomain.GetAssemblies()
                        .Where(x => x.GlobalAssemblyCache)
                        .Select(x => x.FullName)
                        .ToArray();

                return _gacAsmNames;
            }
        }
        internal static bool IsIgnore(string asmQualName)
        {
            if (string.IsNullOrWhiteSpace(asmQualName))
                return true;
            var gacAsms = GacAssemblyNames;
            return gacAsms != null && gacAsms.Any(asmQualName.EndsWith);
        }
        internal static void LaunchListeners()
        {
            if (!Net.IsValidPortNumber(GetTokenIdsCmdPort) || !Net.IsValidPortNumber(GetTokenNamesCmdPort) ||
                !Net.IsValidPortNumber(GetAsmIndicesCmdPort))
                throw new RahRowRagee("the command's ports are either null or invalid " +
                                      String.Format(" GetAsmIndicesCmdPort is port [{0}]", GetAsmIndicesCmdPort) +
                                      String.Format(" GetTokenIdsCmdPort is port [{0}]", GetTokenIdsCmdPort) +
                                      String.Format(" GetTokenNamesCmdPort is port [{0}]", GetTokenNamesCmdPort));

            _taskFactory = new TaskFactory();
            _taskFactory.StartNew(() => HostCmd(new GetAsmIndices(), GetAsmIndicesCmdPort.Value));
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
                        PrintToConsole(String.Format("Connect from port {0}", cmdPort));
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
                File.AppendAllText(LogFile, String.Format("{0:yyyy-MM-dd HH:mm:ss.fff} {1}\n", DateTime.Now, someString));
                if (trunc && someString.Length >= 54)
                    someString = String.Format("{0}[...]", someString.Substring(0, 48));
                
                Console.WriteLine(String.Format("{0:yyyy-MM-dd HH:mm:ss.fff} {1}", DateTime.Now, someString));
            }
        }

        internal static void PrintToConsole(Exception ex)
        {
            lock (_printLock)
            {
                var msg = String.Format("{0:yyyy-MM-dd HH:mm:ss.fff} {1}", DateTime.Now, ex.Message);
                File.AppendAllText(LogFile, String.Format("{0}\n", msg));
                Console.WriteLine(msg);

                msg = String.Format("{0:yyyy-MM-dd HH:mm:ss.fff} {1}", DateTime.Now, ex.StackTrace);
                File.AppendAllText(LogFile, String.Format("{0}\n", msg));
                Console.WriteLine(msg);
            }
        }

        internal static String Help()
        {
            var help = new StringBuilder();
            help.AppendLine(" ----");
            help.AppendLine(String.Format(" [{0}] ", Assembly.GetExecutingAssembly().GetName().Name));
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
            help.AppendLine(String.Format(" {0}{1}{2}[INT]      Optional, cmd line port for the ",
                Constants.CMD_LINE_ARG_SWITCH, AssemblyAnalysis.GET_ASM_INDICES_PORT_CMD_SWITCH,
                Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine("                                 the GetAsmIndices, defaults to app.config.");
            help.AppendLine("");
            help.AppendLine(String.Format(" {0}{1}{2}[INT]      Optional, cmd line port for the ",
                Constants.CMD_LINE_ARG_SWITCH, AssemblyAnalysis.GET_TOKEN_IDS_PORT_CMD_SWITCH,
                Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine("                                 the GetTokenIds, defaults to app.config.");
            help.AppendLine("");
            help.AppendLine(String.Format(" {0}{1}{2}[INT]      Optional, cmd line port for the ",
                Constants.CMD_LINE_ARG_SWITCH, AssemblyAnalysis.GET_TOKEN_NAMES_PORT_CMD_SWITCH,
                Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine("                                 the GetTokenNames, defaults to app.config.");
            help.AppendLine("");
            help.AppendLine(String.Format(" {0}{1}{2}[BOOL]     Optional, cmd line switch for the ",
                Constants.CMD_LINE_ARG_SWITCH, AssemblyAnalysis.RESOLVE_GAC_ASM_SWITCH,
                Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine("                                 the GetTokenNames return names from ");
            help.AppendLine("                                 assemblies in GAC, defaults to app.config.");
            help.AppendLine("");

            return help.ToString();
        }

        #endregion
    }
}
