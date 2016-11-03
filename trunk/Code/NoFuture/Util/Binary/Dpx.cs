using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using NoFuture.Exceptions;
using NoFuture.Shared;
using NoFuture.Tools;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Binary
{
    /// <summary>
    /// Wrapper class to invoke the <see cref="CustomTools.InvokeDpx"/> exe.
    /// This is separated to keep the calling assemblies AppDomain clean
    /// of all the dll's which will need to be loaded to resolve the 
    /// entire directed adjacency dependency graph.
    /// </summary>
    public class Dpx : InvokeConsoleBase
    {
        #region constants
        public const string BIN_DIR = "nfBinDir";
        #endregion

        #region fields
        private readonly ProcessStartInfo _si;
        #endregion

        #region ctor
        public Dpx(string binDir)
        {
            if(string.IsNullOrWhiteSpace(binDir) || !Directory.Exists(binDir))
                throw new ItsDeadJim($"The {binDir} directory was not found");
            if (string.IsNullOrWhiteSpace(CustomTools.InvokeDpx) || !File.Exists(CustomTools.InvokeDpx))
                throw new ItsDeadJim("Don't know where to locate the NoFuture.Util.Binary.InvokeDpx, assign " +
                                     "the global variable at NoFuture.CustomTools.InvokeDpx.");
            var args = string.Join(" ", ConsoleCmd.ConstructCmdLineArgs(BIN_DIR, binDir));
            _si = new ProcessStartInfo
            {
                FileName = CustomTools.InvokeDpx,
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

        }
        #endregion

        #region methods

        public AsmAdjancyGraph RunIsolatedGetDpxAdjGraph(int maxWaitInSeconds = 60)
        {
            MyProcess = StartRemoteProcess(_si);
            var buffer = MyProcess.StandardOutput.ReadToEnd();
            MyProcess.WaitForExit(maxWaitInSeconds);

            return JsonConvert.DeserializeObject<AsmAdjancyGraph>(buffer, JsonSerializerSettings);
        }

        public static Tuple<MetadataTokenAsm[], int[,]> GetDpxAdjGraph(string binDir)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
