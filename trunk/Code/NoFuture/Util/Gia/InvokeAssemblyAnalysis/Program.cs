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
    /// A console program to host assembly-token resolutions as a separate process.
    /// As a console app, the calling <see cref="AppDomain"/>
    /// is kept from becoming clustered with loaded assemblies.  
    /// It will continue running and listening on the given sockets.
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
        private static int? _processProgressCmdPort;
        private static bool? _resolveGacAsms;
        private static readonly Dictionary<MetadataTokenId, MetadataTokenName> _tokenId2NameCache =
            new Dictionary<MetadataTokenId, MetadataTokenName>();
        private static readonly HashSet<MetadataTokenId> _disolutionCache = new HashSet<MetadataTokenId>();
        private static readonly List<MetadataTokenId> _resolutionCache = new List<MetadataTokenId>();
        private static TaskFactory _taskFactory;
        private static string _logName;
        private static readonly AsmIndicies _asmIndices = new AsmIndicies();
        private static int _maxRecursionDepth;
        private static ProcessProgress _processProgress;
        #endregion

        #region properties

        /// <summary>
        /// Proposition that the required assemblies are ready and loaded
        /// for subsequent calls by any <see cref="Cmds.ICmd"/>
        /// </summary>
        internal static bool AsmInited
        {
            get
            {
                return RootAssembly != null &&
                       ManifestModule != null &&
                       AsmIndicies.Asms != null &&
                       AsmIndicies.Asms.Length > 0;
            }
        }

        /// <summary>
        /// The original assembly that the <see cref="AssemblyAnalysis"/> was spawned from.
        /// </summary>
        internal static Assembly RootAssembly { get; set; }

        /// <summary>
        /// A module for used in resolving metadata token id's to <see cref="MemberInfo"/>
        /// </summary>
        internal static Module ManifestModule { get; set; }

        /// <summary>
        /// A memory store for the regex pattern passed into various <see cref="Cmds.ICmd"/>
        /// </summary>
        internal static string AssemblyNameRegexPattern { get; set; }

        /// <summary>
        /// Internal memory for token-ids to token-names so that the expense of resolution
        /// occurs only once.
        /// </summary>
        internal static Dictionary<MetadataTokenId, MetadataTokenName> TokenId2NameCache
        {
            get { return _tokenId2NameCache; }
        }

        /// <summary>
        /// Internal memory for tokens who have had thier entire 
        /// call-stack resolved.  
        /// </summary>
        internal static List<MetadataTokenId> ResolutionCache
        {
            get { return _resolutionCache; }
        }

        /// <summary>
        /// Internal memory hashset for tokens which could not or will
        /// not be resolved.
        /// </summary>
        internal static HashSet<MetadataTokenId> DisolutionCache
        {
            get { return _disolutionCache; }
        }

        /// <summary>
        /// The dictionary of internal ids to assembly names
        /// </summary>
        internal static AsmIndicies AsmIndicies
        {
            get { return _asmIndices; }
        }

        /// <summary>
        /// Resolves the socket port used for <see cref="Cmds.GetTokenNames"/>
        /// </summary>
        internal static int? GetTokenNamesCmdPort
        {
            get
            {
                if (Net.IsValidPortNumber(_getTokenNamesCmdPort))
                    return _getTokenNamesCmdPort;

                _getTokenNamesCmdPort = UtilityMethods.ResolvePort("GetTokenNamesCmdPort");
                return _getTokenNamesCmdPort;
            }
        }

        /// <summary>
        /// Resolves the socket port used for <see cref="Cmds.GetTokenIds"/>
        /// </summary>
        internal static int? GetTokenIdsCmdPort
        {
            get
            {
                if (Net.IsValidPortNumber(_getTokenIdsCmdPort))
                    return _getTokenIdsCmdPort;

                _getTokenIdsCmdPort = UtilityMethods.ResolvePort("GetTokenIdsCmdPort");
                return _getTokenIdsCmdPort;
            }
        }

        /// <summary>
        /// Resolves the socket port used for <see cref="Cmds.GetAsmIndices"/>
        /// </summary>
        internal static int? GetAsmIndicesCmdPort
        {
            get
            {
                if (Net.IsValidPortNumber(_getasmIndicesCmdPort))
                    return _getasmIndicesCmdPort;

                _getasmIndicesCmdPort = UtilityMethods.ResolvePort("GetAsmIndicesCmdPort");
                return _getasmIndicesCmdPort;
            }
        }

        /// <summary>
        /// Resolves the socket port used for <see cref="Cmds.ProcessProgress"/>
        /// </summary>
        internal static int? ProcessProgressCmdPort
        {
            get
            {
                if (Net.IsValidPortNumber(_processProgressCmdPort))
                    return _processProgressCmdPort;
                _processProgressCmdPort = UtilityMethods.ResolvePort("ProcessProgressCmdPort");
                return _processProgressCmdPort;
            }
        }

        /// <summary>
        /// Directory location where logs are saved to the file system.
        /// </summary>
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

        /// <summary>
        /// The full name of the processes log file
        /// </summary>
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

        /// <summary>
        /// Assignable from the config file, a value to keep <see cref="Cmds.GetTokenIds.ResolveCallOfCall"/>
        /// from blowing out the stack.
        /// </summary>
        internal static int MaxRecursionDepth
        {
            get
            {
                if (_maxRecursionDepth > 0)
                    return _maxRecursionDepth;
                var t = UtilityMethods.ResolveInt(ConfigurationManager.AppSettings["MaxRecursionDepth"]);
                if (t == null)
                    return 0;
                _maxRecursionDepth = t.Value;
                return _maxRecursionDepth;
            }
        }

        /// <summary>
        /// Assignable from config file.  Set to 'true' to have tokens resolved for members defined 
        /// in GAC assemblies
        /// </summary>
        internal static bool AreGacAssembliesResolved
        {
            get
            {
                if (_resolveGacAsms != null)
                    return _resolveGacAsms.Value;

                _resolveGacAsms = UtilityMethods.ResolveBool(ConfigurationManager.AppSettings["AreGacAssembliesResolved"]);

                return _resolveGacAsms != null && _resolveGacAsms.Value;
            }
        }
        #endregion

        #region methods

        /// <summary>
        /// Assignable from the config file. Determines if assemblies are loaded using ReflectionOnly
        /// </summary>
        internal static void SetReflectionOnly()
        {
            var useReflectionOnly =
                UtilityMethods.ResolveBool(ConfigurationManager.AppSettings["NoFuture.Shared.Constants.UseReflectionOnlyLoad"]);
            Constants.UseReflectionOnlyLoad = useReflectionOnly != null && useReflectionOnly.Value;
        }

        /// <summary>
        /// Sends a the <see cref="msg"/> back on the <see cref="ProcessProgressCmdPort"/>
        /// to any listeners.
        /// </summary>
        /// <param name="msg"></param>
        internal static void ReportProgress(Shared.ProgressMessage msg)
        {
            if (!Net.IsValidPortNumber(ProcessProgressCmdPort) || _processProgress == null)
                return;
            _processProgress.ReportIn(msg);
        }

        /// <summary>
        /// Main from which the console is launched.
        /// </summary>
        /// <param name="args"></param>
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
                    _getTokenIdsCmdPort = UtilityMethods.ResolveInt(argHash[AssemblyAnalysis.GET_TOKEN_IDS_PORT_CMD_SWITCH].ToString());
                }
                if (argHash.ContainsKey(AssemblyAnalysis.GET_TOKEN_NAMES_PORT_CMD_SWITCH))
                {
                    _getTokenNamesCmdPort = UtilityMethods.ResolveInt(argHash[AssemblyAnalysis.GET_TOKEN_NAMES_PORT_CMD_SWITCH].ToString());
                }
                if (argHash.ContainsKey(AssemblyAnalysis.GET_ASM_INDICES_PORT_CMD_SWITCH))
                {
                    _getasmIndicesCmdPort = UtilityMethods.ResolveInt(argHash[AssemblyAnalysis.GET_ASM_INDICES_PORT_CMD_SWITCH].ToString());
                }
                if (argHash.ContainsKey(AssemblyAnalysis.RESOLVE_GAC_ASM_SWITCH))
                {
                    _resolveGacAsms = UtilityMethods.ResolveBool(argHash[AssemblyAnalysis.RESOLVE_GAC_ASM_SWITCH].ToString());
                }
                if (argHash.ContainsKey(AssemblyAnalysis.PROCESS_PROGRESS_PORT_CMD_SWITCH))
                {
                    _processProgressCmdPort =
                        UtilityMethods.ResolveInt(argHash[AssemblyAnalysis.PROCESS_PROGRESS_PORT_CMD_SWITCH].ToString());
                }

                //print settings
                PrintToConsole(String.Format("GetAsmIndicesCmdPort listening on port [{0}]", GetAsmIndicesCmdPort));
                PrintToConsole(String.Format("GetTokenIdsCmdPort listening on port [{0}]", GetTokenIdsCmdPort));
                PrintToConsole(String.Format("GetTokenNamesCmdPort listening on port [{0}]", GetTokenNamesCmdPort));
                PrintToConsole(String.Format("Resolve GAC Assembly names is [{0}]", AreGacAssembliesResolved));
                PrintToConsole(String.Format("Progress reported on port [{0}]", ProcessProgressCmdPort));
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
                        if (!UtilityMethods.ResolveSingleTokenName(new MetadataTokenId() {Id = utTokenId}, out tokenNameOut))
                            continue;
                        var asmName = AsmIndicies.Asms.FirstOrDefault(x => x.IndexId == tokenNameOut.OwnAsmIdx);

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

        /// <summary>
        /// Opens all the sockets for all <see cref="Cmds.ICmd"/>
        /// </summary>
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

            if (Net.IsValidPortNumber(ProcessProgressCmdPort))
            {
                _processProgress = new ProcessProgress(ProcessProgressCmdPort.Value);
            }
        }

        /// <summary>
        /// The residence of the listening-socket thread, one for each of the <see cref="Cmds.ICmd"/>
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cmdPort"></param>
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

        /// <summary>
        /// Prints to console and writes to <see cref="LogFile"/>
        /// </summary>
        /// <param name="someString"></param>
        /// <param name="trunc"></param>
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

        /// <summary>
        /// Prints to console and writes to <see cref="LogFile"/>
        /// </summary>
        /// <param name="ex"></param>
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

        /// <summary>
        /// Standard help for a console app - does NOT get send to <see cref="LogFile"/>
        /// </summary>
        /// <returns></returns>
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
            help.AppendLine(String.Format(" {0}{1}{2}[INT]     Optional, cmd line port on which ",
                Constants.CMD_LINE_ARG_SWITCH, AssemblyAnalysis.PROCESS_PROGRESS_PORT_CMD_SWITCH,
                Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine("                                 the progress of long running calls ");
            help.AppendLine("                                 reported.");
            help.AppendLine("");

            return help.ToString();
        }

        #endregion
    }
}
