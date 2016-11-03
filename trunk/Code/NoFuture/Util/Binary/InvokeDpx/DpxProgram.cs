using System;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using NoFuture.Shared;

namespace NoFuture.Util.Binary.InvokeDpx
{
    public class DpxProgram : NfConsole.Program
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
                var dpxAdj = Dpx.GetDpxDirectedAdjacencyGraph(p.BinDir);
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
            throw new System.NotImplementedException();
        }

        protected override void ParseProgramArgs()
        {
            throw new System.NotImplementedException();
        }

    }
}
