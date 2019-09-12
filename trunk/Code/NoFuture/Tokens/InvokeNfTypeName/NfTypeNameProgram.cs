using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NoFuture.Tokens.InvokeNfTypeName.Cmds;
using NoFuture.Util.Core;
using NoFuture.Util.NfConsole;
using NoFuture.Util.NfType;

namespace NoFuture.Tokens.InvokeNfTypeName
{
    /// <summary>
    /// The actual remote process which is both launched and in duplex communication with 
    /// the <see cref="NfTypeNameProcess"/>
    /// </summary>
    public class NfTypeNameProgram : SocketConsole
    {
        #region fields
        private TaskFactory _taskFactory;
        private int? _getNfTypeNamePort;
        #endregion

        protected internal int? GetNfTypeNamePort
        {
            get
            {
                if (NfNet.IsValidPortNumber(_getNfTypeNamePort))
                    return _getNfTypeNamePort;
                _getNfTypeNamePort = ResolvePort("GetNfTypeName");
                return _getNfTypeNamePort;
            }
        }

        static void Main(string[] args)
        {
            var ut = string.Empty;
            var p = new NfTypeNameProgram(args);
            try
            {
                p.StartConsole();

                if (p.PrintHelp()) return;
                p.PrintToConsole("New console started.");

                p.ParseProgramArgs();

                p.PrintToConsole($"InvokeNfTypeName listening on port [{p.GetNfTypeNamePort}]");
                p.LaunchListeners();

                for (;;)
                {
                    ut = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(ut))
                        continue;
                    ut = ut.Trim();
                    if (string.Equals(ut, "exit", StringComparison.OrdinalIgnoreCase))
                        break;
                }

            }
            catch (Exception ex)
            {
                p.PrintToConsole(ex);
            }

        }

        public NfTypeNameProgram(string[] args) : base(args, true)
        {
        }

        protected override string MyName => "NfTypeNameProgram";

        protected override string GetHelpText()
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
                NfConfig.CmdLineArgSwitch, NfTypeNameProcess.GET_NF_TYPE_NAME_CMD_SWITCH,
                NfConfig.CmdLineArgAssign));
            help.AppendLine("                                 the NfTypeNameProgram, defaults to app.config.");
            help.AppendLine("");
            return help.ToString();
        }

        protected override void ParseProgramArgs()
        {
            var argHash = ConsoleCmd.ArgHash(_args);
            if (argHash.ContainsKey(NfTypeNameProcess.GET_NF_TYPE_NAME_CMD_SWITCH))
            {
                _getNfTypeNamePort = ResolveInt(argHash[NfTypeNameProcess.GET_NF_TYPE_NAME_CMD_SWITCH].ToString());
            }
        }

        protected override void LaunchListeners()
        {
            if(!NfNet.IsValidPortNumber(GetNfTypeNamePort))
                throw new RahRowRagee("the command's ports are either null or invalid " +
                                      $" GetFlattenAssemblyCmdPort is port [{GetNfTypeNamePort}]");
            _taskFactory = new TaskFactory();
            _taskFactory.StartNew(() => HostCmd(new GetNfTypeName(this), GetNfTypeNamePort.Value));
        }
    }
}
