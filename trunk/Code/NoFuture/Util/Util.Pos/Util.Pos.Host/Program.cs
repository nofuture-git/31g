﻿using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Exceptions;
using NoFuture.Tools;
using NoFuture.Util.NfConsole;
using NoFuture.Util.Pos.Host.Cmds;
using Cfg = System.Configuration.ConfigurationManager;

namespace NoFuture.Util.Pos.Host
{
    public class Program : SocketConsole
    {
        private int? _utilPosHostCmdPort;
        private readonly TaskFactory _taskFactory;
        public Program(string[] args) : base(args, true)
        {
            _taskFactory = new TaskFactory();
        }

        protected internal int? CmdPort
        {
            get
            {
                if (Net.IsValidPortNumber(_utilPosHostCmdPort))
                    return _utilPosHostCmdPort;

                _utilPosHostCmdPort = ResolvePort("UtilPosHostDefaultPort");
                return _utilPosHostCmdPort;
            }
        }
        public static void Main(string[] args)
        {
            var ut = string.Empty;
            var p = new Program(args);
            try
            {
                p.StartConsole();

                if (p.PrintHelp()) return;
                p.PrintToConsole("New console started!");

                p.ParseProgramArgs();

                JavaTools.StanfordPostTaggerModels = Cfg.AppSettings["NoFuture.JavaTools.StanfordPostTaggerModels"];
                if (string.IsNullOrWhiteSpace(JavaTools.StanfordPostTaggerModels) ||
                    !File.Exists(JavaTools.StanfordPostTaggerModels))
                    throw new ItsDeadJim("The Stanford Post Tagger Models are not assigned in the config file");

                p.PrintToConsole($"models @ {JavaTools.StanfordPostTaggerModels}");

                //open ports
                p.LaunchListeners();

                //park main
                for (;;) //ever
                {
                    ut = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(ut))
                        continue;
                    ut = ut.Trim();
                    if (String.Equals(ut, "exit", StringComparison.OrdinalIgnoreCase))
                        break;
                    if (string.Equals(ut, "-help", StringComparison.OrdinalIgnoreCase) || string.Equals(ut, "-h", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine(p.Help());
                    }
                }

            }
            catch (Exception ex)
            {
                p.PrintToConsole(ex);
            }
        }

        protected override string MyName => "Util.Pos.Host";

        protected override string Help()
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
            help.AppendLine(" The exe will tag any unstructured text." );
            help.AppendLine(" ");
            help.AppendLine(" Usage: options ");
            help.AppendLine(" Options:");
            help.AppendLine(" -h | -help             Will print this help.");
            help.AppendLine("");

            return help.ToString();
        }

        protected override void ParseProgramArgs()
        {
            //noop
        }

        protected override void LaunchListeners()
        {
            if (!Net.IsValidPortNumber(CmdPort))
                throw new RahRowRagee("the command's ports are either null or invalid " +
                                      $"[{CmdPort}].");

            _taskFactory.StartNew(() => HostCmd(new PosParserCmd(this), CmdPort.Value));
            //print settings
            PrintToConsole($"InvokeFlatten listening on port [{CmdPort}]");

        }
    }
}
