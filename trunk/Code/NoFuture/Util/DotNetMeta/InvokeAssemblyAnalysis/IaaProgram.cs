using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NoFuture.Util.DotNetMeta.InvokeAssemblyAnalysis.Cmds;
using NoFuture.Util.DotNetMeta.TokenAsm;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.DotNetMeta.TokenName;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.DotNetMeta.InvokeAssemblyAnalysis
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
        private int? _getTokenPageRankCmdPort;
        private int? _getTokenTypesCmdPort;
        private bool? _resolveGacAsms;
        private TaskFactory _taskFactory;
        private int _maxRecursionDepth;
        private readonly UtilityMethods _utilityMethods;
        #endregion

        public IaaProgram(string[] args)
            : base(args, true)
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
                p.PrintToConsole($"{nameof(GetAsmIndicesCmdPort)} listening on port [{p.GetAsmIndicesCmdPort}]");
                p.PrintToConsole($"{nameof(GetTokenIdsCmdPort)} listening on port [{p.GetTokenIdsCmdPort}]");
                p.PrintToConsole($"{nameof(GetTokenNamesCmdPort)} listening on port [{p.GetTokenNamesCmdPort}]");
                p.PrintToConsole($"{nameof(GetTokenTypesCmdPort)} listening on port [{p.GetTokenTypesCmdPort}]");
                p.PrintToConsole($"{nameof(GetTokenPageRankCmdPort)} listening on port [{p.GetTokenPageRankCmdPort}]");
                p.PrintToConsole($"Resolve GAC Assembly names is [{p.AreGacAssembliesResolved}]");
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
                    if (p.PrintHelp(new [] {ut}))
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

            if (string.IsNullOrWhiteSpace(ut) || !ut.StartsWith("exit"))
            {
                Console.ReadKey();
            }
        }

        #region properties

        /// <summary>
        /// Proposition that the required assemblies are ready and loaded
        /// for subsequent calls by any <see cref="ICmd"/>
        /// </summary>
        protected internal bool AsmInited => RootAssembly != null &&
                                             AsmIndicies.Asms != null &&
                                             AsmIndicies.Asms.Length > 0;

        /// <summary>
        /// Single instance of <see cref="UtilityMethods"/> shared by all commands.
        /// </summary>
        protected internal UtilityMethods UtilityMethods => _utilityMethods;

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
        protected internal Dictionary<MetadataTokenId, MetadataTokenName> TokenId2NameCache { get; } =
            new Dictionary<MetadataTokenId, MetadataTokenName>();

        /// <summary>
        /// Internal memory hashset for tokens which could not or will
        /// not be resolved.
        /// </summary>
        protected internal HashSet<MetadataTokenId> DisolutionCache { get; } = new HashSet<MetadataTokenId>();

        /// <summary>
        /// The dictionary of internal ids to assembly names
        /// </summary>
        protected internal AsmIndexResponse AsmIndicies { get; } = new AsmIndexResponse();

        /// <summary>
        /// Resolves the socket port used for <see cref="GetTokenNames"/>
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
        /// Resolves the socket port used for <see cref="GetTokenIds"/>
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
        /// Resolves the socket port used for <see cref="GetAsmIndices"/>
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
        /// Resolves the socket port used for <see cref="GetTokenPageRank"/>
        /// </summary>
        protected internal int? GetTokenPageRankCmdPort
        {
            get
            {
                if (Net.IsValidPortNumber(_getTokenPageRankCmdPort))
                    return _getTokenPageRankCmdPort;

                _getTokenPageRankCmdPort = ResolvePort("GetTokenPageRankCmdPort");
                return _getTokenPageRankCmdPort;
            }
        }
        /// <summary>
        /// Resolves the socket port used for <see cref="GetTokenTypes"/>
        /// </summary>
        protected internal int? GetTokenTypesCmdPort
        {
            get
            {
                if (Net.IsValidPortNumber(_getTokenTypesCmdPort))
                    return _getTokenTypesCmdPort;

                _getTokenTypesCmdPort = ResolvePort("GetTokenTypesCmdPort");
                return _getTokenTypesCmdPort;
            }
        }

        /// <summary>
        /// Assignable from the config file, a value to keep <see cref="GetTokenIds.ResolveCallOfCall"/>
        /// from blowing out the stack.
        /// </summary>
        protected internal int MaxRecursionDepth
        {
            get
            {
                if (_maxRecursionDepth > 0)
                    return _maxRecursionDepth;
                var t = ResolveInt(SysCfg.GetAppCfgSetting("MaxRecursionDepth"));
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

                _resolveGacAsms = ResolveBool(SysCfg.GetAppCfgSetting("AreGacAssembliesResolved"));

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
        }

        protected override void ParseProgramArgs()
        {
            //get and test the cmd line arg key\values
            var argHash = ConsoleCmd.ArgHash(_args);

            NfConfig.CustomTools.InvokeNfTypeName = SysCfg.GetAppCfgSetting("NoFuture.ToolsCustomTools.InvokeNfTypeName");
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
            if (argHash.ContainsKey(AssemblyAnalysis.GET_TOKEN_TYPES_PORT_CMD_SWITCH))
            {
                _getTokenTypesCmdPort = ResolveInt(argHash[AssemblyAnalysis.GET_TOKEN_TYPES_PORT_CMD_SWITCH].ToString());
            }
            if (argHash.ContainsKey(AssemblyAnalysis.GET_TOKEN_PAGE_RANK_PORT_CMD_SWITCH))
            {
                _getTokenPageRankCmdPort = ResolveInt(argHash[AssemblyAnalysis.GET_TOKEN_PAGE_RANK_PORT_CMD_SWITCH].ToString());
            }
            if (argHash.ContainsKey(AssemblyAnalysis.RESOLVE_GAC_ASM_SWITCH))
            {
                _resolveGacAsms = ResolveBool(argHash[AssemblyAnalysis.RESOLVE_GAC_ASM_SWITCH].ToString());
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
                if (!ut.StartsWith(NfConfig.CmdLineArgSwitch))
                    return;

                var cmds = ut.Split(' ');
                var rtArgHash = ConsoleCmd.ArgHash(cmds);

                if (rtArgHash.ContainsKey(ASSIGN_REGEX_PATTERN_RT_CMD))
                {
                    var regexPattern = rtArgHash[ASSIGN_REGEX_PATTERN_RT_CMD];
                    if (string.IsNullOrWhiteSpace(regexPattern?.ToString()))
                        return;
                    AssemblyNameRegexPattern = regexPattern.ToString().Trim();
                    PrintToConsole($"AssemblyNameRegexPattern = {AssemblyNameRegexPattern}");
                }
                if (rtArgHash.ContainsKey(RESOLVE_TOKEN_ID_CMD))
                {
                    var tokenCmd = rtArgHash[RESOLVE_TOKEN_ID_CMD];
                    if (string.IsNullOrWhiteSpace(tokenCmd?.ToString()))
                        return;
                    var tokenCmdStr = tokenCmd.ToString().Trim();
                    if (!Regex.IsMatch(tokenCmdStr, @"^[0-9]\.([0-9]+|0x[0-9a-fA-F]+)$"))
                        return;
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
                        PrintToConsole($"could not resolve name {ut}");
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
            if (!Net.IsValidPortNumber(GetTokenIdsCmdPort) 
                || !Net.IsValidPortNumber(GetTokenNamesCmdPort) 
                || !Net.IsValidPortNumber(GetAsmIndicesCmdPort) 
                || !Net.IsValidPortNumber(GetTokenPageRankCmdPort)
                || !Net.IsValidPortNumber(GetTokenTypesCmdPort))
                throw new RahRowRagee("the command's ports are either null or invalid " +
                                      $" {nameof(GetAsmIndicesCmdPort)} is port [{GetAsmIndicesCmdPort}]" +
                                      $" {nameof(GetTokenIdsCmdPort)} is port [{GetTokenIdsCmdPort}]" +
                                      $" {nameof(GetTokenNamesCmdPort)} is port [{GetTokenNamesCmdPort}]" +
                                      $" {nameof(GetTokenPageRankCmdPort)} is port [{GetTokenPageRankCmdPort}]" +
                                      $" {nameof(GetTokenTypesCmdPort)} is port [{GetTokenPageRankCmdPort}]");

            _taskFactory = new TaskFactory();
            _taskFactory.StartNew(() => HostCmd(new GetAsmIndices(this), GetAsmIndicesCmdPort.Value));
            _taskFactory.StartNew(() => HostCmd(new GetTokenIds(this), GetTokenIdsCmdPort.Value));
            _taskFactory.StartNew(() => HostCmd(new GetTokenNames(this), GetTokenNamesCmdPort.Value));
            _taskFactory.StartNew(() => HostCmd(new GetTokenPageRank(this), GetTokenPageRankCmdPort.Value));
            _taskFactory.StartNew(() => HostCmd(new GetTokenTypes(this), GetTokenTypesCmdPort.Value));

        }

        protected override string MyName => "InvokeAssemblyAnalysis";

        protected override String GetHelpText()
        {
            var help = new StringBuilder();
            help.AppendLine(" ----");
            help.AppendLine($" [{Assembly.GetExecutingAssembly().GetName().Name}] ");
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
                NfConfig.CmdLineArgSwitch, AssemblyAnalysis.GET_ASM_INDICES_PORT_CMD_SWITCH,
                NfConfig.CmdLineArgAssign));
            help.AppendLine("                                 the GetAsmIndices, defaults to app.config.");
            help.AppendLine("");
            help.AppendLine(String.Format(" {0}{1}{2}[INT]      Optional, cmd line port for the ",
                NfConfig.CmdLineArgSwitch, AssemblyAnalysis.GET_TOKEN_IDS_PORT_CMD_SWITCH,
                NfConfig.CmdLineArgAssign));
            help.AppendLine("                                 the GetTokenIds, defaults to app.config.");
            help.AppendLine("");
            help.AppendLine(String.Format(" {0}{1}{2}[INT]      Optional, cmd line port for the ",
                NfConfig.CmdLineArgSwitch, AssemblyAnalysis.GET_TOKEN_NAMES_PORT_CMD_SWITCH,
                NfConfig.CmdLineArgAssign));
            help.AppendLine("                                 the GetTokenNames, defaults to app.config.");
            help.AppendLine("");
            help.AppendLine(String.Format(" {0}{1}{2}[INT]      Optional, cmd line port for the ",
                NfConfig.CmdLineArgSwitch, AssemblyAnalysis.GET_TOKEN_TYPES_PORT_CMD_SWITCH,
                NfConfig.CmdLineArgAssign));
            help.AppendLine("                                 the GetTokenTypes, defaults to app.config.");
            help.AppendLine("");
            help.AppendLine(String.Format(" {0}{1}{2}[INT]      Optional, cmd line port for the ",
                NfConfig.CmdLineArgSwitch, AssemblyAnalysis.GET_TOKEN_PAGE_RANK_PORT_CMD_SWITCH,
                NfConfig.CmdLineArgAssign));
            help.AppendLine("                                 the GetTokenPageRank, defaults to app.config.");
            help.AppendLine("");
            help.AppendLine(String.Format(" {0}{1}{2}[BOOL]     Optional, cmd line switch for the ",
                NfConfig.CmdLineArgSwitch, AssemblyAnalysis.RESOLVE_GAC_ASM_SWITCH,
                NfConfig.CmdLineArgAssign));
            help.AppendLine("                                 the GetTokenNames return names from ");
            help.AppendLine("                                 assemblies in GAC, defaults to app.config.");
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
                NfConfig.CmdLineArgSwitch, ASSIGN_REGEX_PATTERN_RT_CMD,
                NfConfig.CmdLineArgAssign));
            help.AppendLine("                           This will override any value passed through a socket.");
            help.AppendLine("");
            help.AppendLine(String.Format(" {0}{1}{2}[INT].[INT]  Resolve a token name by its id.",
                NfConfig.CmdLineArgSwitch, RESOLVE_TOKEN_ID_CMD,
                NfConfig.CmdLineArgAssign));
            help.AppendLine("");
            help.AppendLine(" ----");

            return help.ToString();            
        }

        #endregion
    }
}
