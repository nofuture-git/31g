using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Exceptions;
using NoFuture.Shared;
using NoFuture.Tokens.InvokeNfTypeName.Cmds;
using NoFuture.Util;
using NoFuture.Util.Gia;
using NoFuture.Util.NfConsole;
using NoFuture.Util.NfType;

namespace NoFuture.Tokens.InvokeNfTypeName
{
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
                if (Net.IsValidPortNumber(_getNfTypeNamePort))
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
                Constants.CMD_LINE_ARG_SWITCH, NfTypeNameProcess.GET_NF_TYPE_NAME_CMD_SWITCH,
                Constants.CMD_LINE_ARG_ASSIGN));
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
            if(!Net.IsValidPortNumber(GetNfTypeNamePort))
                throw new RahRowRagee("the command's ports are either null or invalid " +
                                      $" GetFlattenAssemblyCmdPort is port [{GetNfTypeNamePort}]");
            _taskFactory = new TaskFactory();
            _taskFactory.StartNew(() => HostCmd(new GetNfTypeName(this), GetNfTypeNamePort.Value));
        }
    }
}
