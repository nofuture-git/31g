using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NoFuture.Exceptions;
using NoFuture.Shared;
using NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis
{
    /// <summary>
    /// A console program to host assembly-token resolutions as a separate process.
    /// As a console app, the calling <see cref="AppDomain"/>
    /// is kept from becoming clustered with loaded assemblies.  
    /// It will continue running and listening on the given sockets.
    /// </summary>
    public class IaaProgram : SocketConsole
    {
        #region constants
        public const string ASSIGN_REGEX_PATTERN_RT_CMD = "AssignRegex";
        public const string RESOLVE_TOKEN_ID_CMD = "ResolveToken";
        #endregion

        #region fields
        private int? _getTokenNamesCmdPort;
        private int? _getTokenIdsCmdPort;
        private int? _getasmIndicesCmdPort;
        private int? _processProgressCmdPort;
        private bool? _resolveGacAsms;
        private readonly Dictionary<MetadataTokenId, MetadataTokenName> _tokenId2NameCache =
            new Dictionary<MetadataTokenId, MetadataTokenName>();
        private readonly HashSet<MetadataTokenId> _disolutionCache = new HashSet<MetadataTokenId>();
        private TaskFactory _taskFactory;
        private readonly AsmIndicies _asmIndices = new AsmIndicies();
        private int _maxRecursionDepth;
        private ProcessProgress _processProgress;
        private readonly UtilityMethods _utilityMethods;
        #endregion

        public IaaProgram(string[] args) : base(args)
        {
            _utilityMethods = new UtilityMethods(this);
        }

        /// <summary>
        /// Main from which the console is launched.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var ut = String.Empty;
            var p = new IaaProgram(args);
            try
            {
                p.StartConsole();

                if (p.PrintHelp()) return;
                p.PrintToConsole("New console started!");

                p.ParseProgramArgs();

                //print settings
                p.PrintToConsole(String.Format("GetAsmIndicesCmdPort listening on port [{0}]", p.GetAsmIndicesCmdPort));
                p.PrintToConsole(String.Format("GetTokenIdsCmdPort listening on port [{0}]", p.GetTokenIdsCmdPort));
                p.PrintToConsole(String.Format("GetTokenNamesCmdPort listening on port [{0}]", p.GetTokenNamesCmdPort));
                p.PrintToConsole(String.Format("Resolve GAC Assembly names is [{0}]", p.AreGacAssembliesResolved));
                p.PrintToConsole(String.Format("Progress reported on port [{0}]", p.ProcessProgressCmdPort));
                p.PrintToConsole("type 'exit' to quit", false);

                //open ports
                p.LaunchListeners();

                //park main
                for (; ; ) //ever
                {
                    ut = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(ut))
                        continue;
                    ut = ut.Trim();
                    if (String.Equals(ut, "exit", StringComparison.OrdinalIgnoreCase))
                        break;
                    if (string.Equals(ut, "-help", StringComparison.OrdinalIgnoreCase) || string.Equals(ut, "-h", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine(p.RuntimeHelp());
                    }

                    //handle command from the console.
                    p.RtCommands(ut);
                }
            }
            catch (Exception ex)
            {
                p.PrintToConsole(ex);
            }

            if (String.IsNullOrWhiteSpace(ut) || !ut.StartsWith("exit"))
            {
                Console.ReadKey();
            }
        }

        #region properties

        /// <summary>
        /// Proposition that the required assemblies are ready and loaded
        /// for subsequent calls by any <see cref="ICmd"/>
        /// </summary>
        protected internal bool AsmInited
        {
            get
            {
                return RootAssembly != null &&
                       AsmIndicies.Asms != null &&
                       AsmIndicies.Asms.Length > 0;
            }
        }

        /// <summary>
        /// Single instance of <see cref="UtilityMethods"/> shared by all commands.
        /// </summary>
        protected internal UtilityMethods UtilityMethods { get { return _utilityMethods;} }

        /// <summary>
        /// The original assembly that the <see cref="AssemblyAnalysis"/> was spawned from.
        /// </summary>
        protected internal Assembly RootAssembly { get; set; }

        /// <summary>
        /// A memory store for the regex pattern passed into various <see cref="ICmd"/>
        /// </summary>
        protected internal string AssemblyNameRegexPattern { get; set; }

        /// <summary>
        /// Internal memory for token-ids to token-names so that the expense of resolution
        /// occurs only once.
        /// </summary>
        protected internal Dictionary<MetadataTokenId, MetadataTokenName> TokenId2NameCache
        {
            get { return _tokenId2NameCache; }
        }

        /// <summary>
        /// Internal memory hashset for tokens which could not or will
        /// not be resolved.
        /// </summary>
        protected internal HashSet<MetadataTokenId> DisolutionCache
        {
            get { return _disolutionCache; }
        }

        /// <summary>
        /// The dictionary of internal ids to assembly names
        /// </summary>
        protected internal AsmIndicies AsmIndicies
        {
            get { return _asmIndices; }
        }

        /// <summary>
        /// Resolves the socket port used for <see cref="Cmds.GetTokenNames"/>
        /// </summary>
        protected internal int? GetTokenNamesCmdPort
        {
            get
            {
                if (Net.IsValidPortNumber(_getTokenNamesCmdPort))
                    return _getTokenNamesCmdPort;

                _getTokenNamesCmdPort = ResolvePort("GetTokenNamesCmdPort");
                return _getTokenNamesCmdPort;
            }
        }

        /// <summary>
        /// Resolves the socket port used for <see cref="Cmds.GetTokenIds"/>
        /// </summary>
        protected internal int? GetTokenIdsCmdPort
        {
            get
            {
                if (Net.IsValidPortNumber(_getTokenIdsCmdPort))
                    return _getTokenIdsCmdPort;

                _getTokenIdsCmdPort = ResolvePort("GetTokenIdsCmdPort");
                return _getTokenIdsCmdPort;
            }
        }

        /// <summary>
        /// Resolves the socket port used for <see cref="Cmds.GetAsmIndices"/>
        /// </summary>
        protected internal int? GetAsmIndicesCmdPort
        {
            get
            {
                if (Net.IsValidPortNumber(_getasmIndicesCmdPort))
                    return _getasmIndicesCmdPort;

                _getasmIndicesCmdPort = ResolvePort("GetAsmIndicesCmdPort");
                return _getasmIndicesCmdPort;
            }
        }

        /// <summary>
        /// Resolves the socket port used for <see cref="Cmds.ProcessProgress"/>
        /// </summary>
        protected internal int? ProcessProgressCmdPort
        {
            get
            {
                if (Net.IsValidPortNumber(_processProgressCmdPort))
                    return _processProgressCmdPort;
                _processProgressCmdPort = ResolvePort("ProcessProgressCmdPort");
                return _processProgressCmdPort;
            }
        }

        /// <summary>
        /// Assignable from the config file, a value to keep <see cref="Cmds.GetTokenIds.ResolveCallOfCall"/>
        /// from blowing out the stack.
        /// </summary>
        protected internal int MaxRecursionDepth
        {
            get
            {
                if (_maxRecursionDepth > 0)
                    return _maxRecursionDepth;
                var t = ResolveInt(ConfigurationManager.AppSettings["MaxRecursionDepth"]);
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
        protected internal bool AreGacAssembliesResolved
        {
            get
            {
                if (_resolveGacAsms != null)
                    return _resolveGacAsms.Value;

                _resolveGacAsms = ResolveBool(ConfigurationManager.AppSettings["AreGacAssembliesResolved"]);

                return _resolveGacAsms != null && _resolveGacAsms.Value;
            }
        }
        #endregion

        #region methods
        protected internal void Init(Assembly asm)
        {
            RootAssembly = asm;
            TokenId2NameCache.Clear();
            DisolutionCache.Clear();
            DisolutionCache.Add(new MetadataTokenId());//empty token Id
        }

        protected internal void AssignAsmIndicies(Assembly asm)
        {
            var tempList = new List<MetadataTokenAsm>();

            var refedAsms = asm.GetReferencedAssemblies();
            tempList.Add(new MetadataTokenAsm { AssemblyName = asm.GetName().FullName, IndexId = 0 });
            for (var i = 0; i < refedAsms.Length; i++)
            {
                tempList.Add(new MetadataTokenAsm { AssemblyName = refedAsms[i].FullName, IndexId = i + 1 });
            }

            AsmIndicies.Asms = tempList.ToArray();
            AsmIndicies.St = MetadataTokenStatus.Ok;
        }

        /// <summary>
        /// Reports progress to the console and any calling assembly (if so configured).
        /// </summary>
        /// <param name="msg"></param>
        protected internal void ReportProgress(ProgressMessage msg)
        {
            PrintToConsole(msg);
            if (!Net.IsValidPortNumber(ProcessProgressCmdPort) || _processProgress == null)
                return;
            _processProgress.ReportIn(msg);
        }

        protected override void ParseProgramArgs()
        {
            //get and test the cmd line arg key\values
            var argHash = ConsoleCmd.ArgHash(_args);
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
            if (argHash.ContainsKey(AssemblyAnalysis.PROCESS_PROGRESS_PORT_CMD_SWITCH))
            {
                _processProgressCmdPort =
                    ResolveInt(argHash[AssemblyAnalysis.PROCESS_PROGRESS_PORT_CMD_SWITCH].ToString());
            }            
        }

        /// <summary>
        /// Handles a specific set of formatted runtime commands as printed in <see cref="RuntimeHelp"/>
        /// </summary>
        /// <param name="ut">User entered text</param>
        protected internal void RtCommands(string ut)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ut))
                    return;
                if (!ut.StartsWith(Constants.CMD_LINE_ARG_SWITCH))
                    return;

                var cmds = ut.Split(' ');
                var rtArgHash = ConsoleCmd.ArgHash(cmds);

                if (rtArgHash.ContainsKey(ASSIGN_REGEX_PATTERN_RT_CMD))
                {
                    var regexPattern = rtArgHash[ASSIGN_REGEX_PATTERN_RT_CMD];
                    if (regexPattern == null)
                        return;
                    if (string.IsNullOrWhiteSpace(regexPattern.ToString()))
                        return;
                    AssemblyNameRegexPattern = regexPattern.ToString().Trim();
                    PrintToConsole(string.Format("AssemblyNameRegexPattern = {0}",AssemblyNameRegexPattern));
                }
                if (rtArgHash.ContainsKey(RESOLVE_TOKEN_ID_CMD))
                {
                    var tokenCmd = rtArgHash[RESOLVE_TOKEN_ID_CMD];
                    if (tokenCmd == null)
                        return;
                    if (string.IsNullOrWhiteSpace(tokenCmd.ToString()))
                        return;
                    var tokenCmdStr = tokenCmd.ToString().Trim();
                    if (!Regex.IsMatch(tokenCmdStr, @"^[0-9]\.([0-9]+|0x[0-9a-fA-F]+)$")) return;
                    var asmIdxStr = tokenCmdStr.Split('.')[0];
                    var tokenIdStr = tokenCmdStr.Split('.')[1];

                    int tokenId;
                    int asmIdx;
                    if (!int.TryParse(asmIdxStr, out asmIdx))
                        return;
                    if (Regex.IsMatch(tokenIdStr, "0x[0-9a-fA-F]+"))
                    {
                        tokenIdStr = tokenIdStr.Substring(2);
                        if (!int.TryParse(tokenIdStr, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out tokenId))
                            return;
                    }
                    else
                    {
                        if (!int.TryParse(tokenIdStr, out tokenId))
                            return;
                    }

                    var mdt = new MetadataTokenId { Id = tokenId, RslvAsmIdx = asmIdx };
                    MetadataTokenName tokenName;
                    if (!_utilityMethods.ResolveSingleTokenName(mdt, out tokenName))
                    {
                        PrintToConsole(string.Format("could not resolve name {0}", ut));
                        return;
                    }
                    PrintToConsole(tokenName.Name);

                    if (!tokenName.IsMethodName())
                        return;
                    var resolveDepth = 0;
                    var st = new Stack<MetadataTokenId>();
                    var msg = new StringBuilder();
                    var tTokensIds = new GetTokenIds(this);
                    tTokensIds.ResolveCallOfCall(mdt, ref resolveDepth, st, msg);
                    if (mdt.Items == null || mdt.Items.Length <= 0)
                    {
                        PrintToConsole("ResolveCallOfCall returned nothing");
                        PrintToConsole(msg.ToString(), false);
                        return;
                    }
                    Console.WriteLine();
                    PrintToConsole(MetadataTokenId.Print(mdt), false);
                }

            }
            catch (Exception ex)
            {
                PrintToConsole("console entry error");
                PrintToConsole(ex);
            }            
        }

        protected override void LaunchListeners()
        {
            if (!Net.IsValidPortNumber(GetTokenIdsCmdPort) || !Net.IsValidPortNumber(GetTokenNamesCmdPort) ||
                !Net.IsValidPortNumber(GetAsmIndicesCmdPort))
                throw new RahRowRagee("the command's ports are either null or invalid " +
                                      String.Format(" GetAsmIndicesCmdPort is port [{0}]", GetAsmIndicesCmdPort) +
                                      String.Format(" GetTokenIdsCmdPort is port [{0}]", GetTokenIdsCmdPort) +
                                      String.Format(" GetTokenNamesCmdPort is port [{0}]", GetTokenNamesCmdPort));

            _taskFactory = new TaskFactory();
            _taskFactory.StartNew(() => HostCmd(new GetAsmIndices(this), GetAsmIndicesCmdPort.Value));
            _taskFactory.StartNew(() => HostCmd(new GetTokenIds(this), GetTokenIdsCmdPort.Value));
            _taskFactory.StartNew(() => HostCmd(new GetTokenNames(this), GetTokenNamesCmdPort.Value));

            if (Net.IsValidPortNumber(ProcessProgressCmdPort))
            {
                _processProgress = new ProcessProgress(ProcessProgressCmdPort.Value);
            }
        }

        protected override String Help()
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

        protected internal string RuntimeHelp()
        {
            var help = new StringBuilder();
            help.AppendLine(" ----");
            help.AppendLine(String.Format(" [{0}] ", Assembly.GetExecutingAssembly().GetName().Name));
            help.AppendLine(" ");
            help.AppendLine(" This console offers a small set of commands at runtime.");
            help.AppendLine(" These commands must be entered in the format shown.");
            help.AppendLine(" Multiple commands in a single line are space-separated");
            help.AppendLine(" (same as a command line invocation).");
            help.AppendLine(" ");
            help.AppendLine(" -h | -help                Will print this help.");
            help.AppendLine("");
            help.AppendLine(String.Format(" {0}{1}{2}[STRING]     Assign a regex pattern for recursive calls.",
                Constants.CMD_LINE_ARG_SWITCH, ASSIGN_REGEX_PATTERN_RT_CMD,
                Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine("                           This will override any value passed through a socket.");
            help.AppendLine("");
            help.AppendLine(String.Format(" {0}{1}{2}[INT].[INT]  Resolve a token name by its id.",
                Constants.CMD_LINE_ARG_SWITCH, RESOLVE_TOKEN_ID_CMD,
                Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine("");
            help.AppendLine(" ----");

            return help.ToString();            
        }

        #endregion
    }
}
