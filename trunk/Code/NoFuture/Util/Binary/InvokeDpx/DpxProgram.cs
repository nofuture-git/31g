using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Binary.InvokeDpx
{
    public class DpxProgram : Program
    {
        public const string APP_SET_KEY_KEEP_TEMP = "KeepRTempFiles";
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
                var rspn = Dpx.GetDpxAdjGraph(p.BinDir);
                p.AssignPageRank(rspn);
                var json = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(rspn));
                File.WriteAllBytes(Path.Combine(p.LogDirectory, $"{rspn.GetType().Name}.json"), json);
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
                NfConfig.CmdLineArgSwitch, Dpx.BIN_DIR,
                NfConfig.CmdLineArgAssign));
            help.AppendLine("");

            return help.ToString();
        }

        protected override void ParseProgramArgs()
        {
            var argHash = ConsoleCmd.ArgHash(_args);

            NfConfig.CustomTools.InvokeNfTypeName = NoFuture.Util.NfPath.GetAppCfgSetting("NoFuture.ToolsCustomTools.InvokeNfTypeName");

            if (!argHash.ContainsKey(Dpx.BIN_DIR) ||
                argHash[Dpx.BIN_DIR] == null)
            {
                throw new RahRowRagee(
                    $"the switch '{Dpx.BIN_DIR}' could not be " +
                    $"parsed from cmd line arg \n{string.Join(" ", _args)}");
            }
            BinDir = argHash[Dpx.BIN_DIR].ToString();
        }

        public void AssignPageRank(AsmAdjancyGraph g)
        {
            if (g.St == MetadataTokenStatus.Error)
                return;
            if (g.Asms == null || !g.Asms.Any())
                return;
            if (g.Graph == null)
                return;
            var keepTemp = ResolveBool(NoFuture.Util.NfPath.GetAppCfgSetting(APP_SET_KEY_KEEP_TEMP)) ?? false;
            Re.Efx.RTempDir = LogDirectory;
            var pageRank = Re.Efx.GetPageRank(g.Graph, keepTemp);
            if (pageRank == null || !pageRank.Any())
                return;
            for (var i = 0; i < g.Asms.Length; i++)
            {
                var asm = g.Asms.FirstOrDefault(x => x.IndexId == i);
                if (asm == null)
                    continue;
                asm.PageRank = pageRank[i];
            }
        }
    }
}
