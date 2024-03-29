﻿using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NoFuture.Tokens.Gia.InvokeFlatten.Cmds;
using NoFuture.Util.Core;
using NoFuture.Util.NfConsole;

namespace NoFuture.Tokens.Gia.InvokeFlatten
{
    public class FlattenProgram  : SocketConsole
    {
        private int? _getFlattenAssemblyCmdPort;
        private TaskFactory _taskFactory;

        protected internal int GetMaxDepth
        {
            get
            {
                var dk = SysCfg.GetAppCfgSetting("MaxDepth");
                var dfk = ResolveInt(dk);
                return dfk ?? FlattenLineArgs.MAX_DEPTH;
            }
        }

        protected internal int? GetFlattenAssemblyCmdPort
        {
            get
            {
                if(NfNet.IsValidPortNumber(_getFlattenAssemblyCmdPort))
                    return _getFlattenAssemblyCmdPort;

                _getFlattenAssemblyCmdPort = ResolvePort("GetFlattenAssemblyCmdPort");
                return _getFlattenAssemblyCmdPort;
            }
        }

        public static void Main(string[] args)
        {
            var ut = String.Empty;
            var p = new FlattenProgram(args);
            try
            {
                p.StartConsole();

                if (p.PrintHelp()) return;
                p.PrintToConsole("New console started!");

                p.ParseProgramArgs();

                //print settings
                p.PrintToConsole($"InvokeFlatten listening on port [{p.GetFlattenAssemblyCmdPort}]");
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
                        Console.WriteLine(p.GetHelpText());
                    }
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

        public FlattenProgram(string[] args)
            : base(args, true)
        {

        }

        protected override string MyName
        {
            get { return "InvokeFlatten"; }
        }

        protected override string GetHelpText()
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
                NfConfig.CmdLineArgSwitch, Flatten.GET_FLAT_ASM_PORT_CMD_SWITCH,
                NfConfig.CmdLineArgAssign));
            help.AppendLine("                                 the GetFlattenAssembly, defaults to app.config.");
            help.AppendLine("");
            return help.ToString();
        }

        protected override void ParseProgramArgs()
        {
            var argHash = ConsoleCmd.ArgHash(_args);
            NfConfig.CustomTools.InvokeNfTypeName = SysCfg.GetAppCfgSetting("NoFuture.ToolsCustomTools.InvokeNfTypeName");
            if (argHash.ContainsKey(Flatten.GET_FLAT_ASM_PORT_CMD_SWITCH))
            {
                _getFlattenAssemblyCmdPort = ResolveInt(argHash[Flatten.GET_FLAT_ASM_PORT_CMD_SWITCH].ToString());
            }
        }

        protected override void LaunchListeners()
        {

            if(!NfNet.IsValidPortNumber(GetFlattenAssemblyCmdPort))
                throw new RahRowRagee("the command's ports are either null or invalid " +
                                      String.Format(" GetFlattenAssemblyCmdPort is port [{0}]", GetFlattenAssemblyCmdPort));
            _taskFactory = new TaskFactory();

            _taskFactory.StartNew(() => HostCmd(new GetFlattenAssembly(this, GetMaxDepth), GetFlattenAssemblyCmdPort.Value));
        }
    }
}
