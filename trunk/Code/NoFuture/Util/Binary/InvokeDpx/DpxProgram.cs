using System;
using System.Reflection;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using NoFuture.Exceptions;
using NoFuture.Shared;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Binary.InvokeDpx
{
    public class DpxProgram : Program
    {
        public string BinDir { get; set; }

        public static void Main(string[] args)
        {
            var p = new DpxProgram(args);
            try
            {
                p.StartConsole();
                if (p.PrintHelp())
                    return;
                p.ParseProgramArgs();
                var dpxAdj = Dpx.GetDpxAdjGraph(p.BinDir);
                var rspn = new AsmAdjancyGraph { Asms = dpxAdj.Item1, Graph = dpxAdj.Item2, St = MetadataTokenStatus.Ok };
                var json = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(rspn));
                using (var std = Console.OpenStandardOutput())
                {
                    std.Write(json, 0, json.Length);
                    std.Flush();
                    std.Close();
                }
            }
            catch (Exception ex)
            {
                p.PrintToConsole(ex);
            }
            Thread.Sleep(NfConfig.ThreadSleepTime);
        }

        public DpxProgram(string[] args) : base(args, false)
        {
        }

        protected override string MyName => nameof(DpxProgram);

        protected override string GetHelpText()
        {
            var help = new StringBuilder();
            help.AppendLine(" ----");
            help.AppendLine($" [{Assembly.GetExecutingAssembly().GetName().Name}] ");
            help.AppendLine("");
            help.AppendLine(" ");
            help.AppendLine(" This exe gets an adjacency graph of ");
            help.AppendLine(" all assemblies present in a directory  ");
            help.AppendLine(" ");
            help.AppendLine(" Usage: options ");
            help.AppendLine(" Options:");
            help.AppendLine(" -h | -help             Will print this help.");
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[STRING]   Path to bin directory ",
                Constants.CMD_LINE_ARG_SWITCH, Dpx.BIN_DIR,
                Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine("");

            return help.ToString();
        }

        protected override void ParseProgramArgs()
        {
            var argHash = ConsoleCmd.ArgHash(_args);
            if (!argHash.ContainsKey(Dpx.BIN_DIR) ||
                argHash[Dpx.BIN_DIR] == null)
            {
                throw new RahRowRagee(
                    $"the switch '{Dpx.BIN_DIR}' could not be " +
                    $"parsed from cmd line arg \n{string.Join(" ", _args)}");
            }
            BinDir = argHash[Dpx.BIN_DIR].ToString();
        }

    }
}
