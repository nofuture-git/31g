using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NoFuture.Util.Binary;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.DotNetMeta.TokenRank;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.DotNetMeta
{
    /// <summary>
    /// Wrapper class to invoke the <see cref="NfConfig.CustomTools.InvokeDpx"/> exe.
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
            if (string.IsNullOrWhiteSpace(NfConfig.CustomTools.InvokeDpx) || !File.Exists(NfConfig.CustomTools.InvokeDpx))
                throw new ItsDeadJim("Don't know where to locate the NoFuture.Util.Binary.InvokeDpx, assign " +
                                     "the global variable at NoFuture.CustomTools.InvokeDpx.");
            var args = string.Join(" ", ConsoleCmd.ConstructCmdLineArgs(BIN_DIR, binDir));
            _si = new ProcessStartInfo
            {
                FileName = NfConfig.CustomTools.InvokeDpx,
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
            MyProcess.WaitForExit(maxWaitInSeconds * 1000);
            if (string.IsNullOrWhiteSpace(buffer))
            {
                throw new ItsDeadJim($"The remote process {MyProcess.ProcessName} did not " +
                                     "return anything on the StandardOutput");
            }

            //HACK - the std out will have all the banner stuff printed by the R Console w\ JSON at bottom
            buffer = buffer.Substring(buffer.IndexOf('{'));

            return JsonConvert.DeserializeObject<AsmAdjancyGraph>(buffer, JsonSerializerSettings);
        }

        public static AsmAdjancyGraph GetDpxAdjGraph(string binDir)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(binDir) || !Directory.Exists(binDir))
                {
                    var asmAdjGraph = new AsmAdjancyGraph
                    {
                        Msg = $"The bin directory {binDir} is null or does not exist.",
                        St = MetadataTokenStatus.Error
                    };
                    return asmAdjGraph;
                }

                //get all the dll files in the dir
                var di = new DirectoryInfo(binDir);
                var dllsFullNames =
                    di.EnumerateFileSystemInfos("*.dll", SearchOption.TopDirectoryOnly).Select(x => x.FullName).ToArray();

                if (!dllsFullNames.Any())
                {
                    var asmAdjGraph = new AsmAdjancyGraph
                    {
                        Msg = $"The directory {binDir} does not contain any files with a .dll extension.",
                        St = MetadataTokenStatus.Error
                    };
                    return asmAdjGraph;
                }

                //get all the assembly names of direct files and each of thier ref's
                var asmIndxBuffer = new List<Tuple<RankedMetadataTokenAsm, RankedMetadataTokenAsm[]>>();
                foreach (var dll in dllsFullNames)
                {
                    var asmName = AssemblyName.GetAssemblyName(dll);
                    if (asmName == null)
                        continue;
                    var asm = Asm.NfLoadFrom(dll);
                    var refs = asm.GetReferencedAssemblies().ToArray();
                    var mdta = new RankedMetadataTokenAsm
                    {
                        AssemblyName = asm.GetName().FullName,
                        IndexId = -1,
                        DllFullName = dll,
                        HasPdb = File.Exists(Path.ChangeExtension(dll, "pdb"))
                    };
                    asmIndxBuffer.Add(new Tuple<RankedMetadataTokenAsm, RankedMetadataTokenAsm[]>(mdta,
                        refs.Select(x => new RankedMetadataTokenAsm { AssemblyName = x.FullName, IndexId = -2}).ToArray()));
                }
                return GetDpxAdjGraph(asmIndxBuffer);

            }
            catch (Exception ex)
            {
                return new AsmAdjancyGraph {Msg = ex.Message, St = MetadataTokenStatus.Error};
            }
        }

        internal static AsmAdjancyGraph GetDpxAdjGraph(
            List<Tuple<RankedMetadataTokenAsm, RankedMetadataTokenAsm[]>> asmIndxBuffer)
        {
            //flatten all of them to a unique list
            var uqAsmIndices = asmIndxBuffer.Select(x => x.Item1).ToList();
            uqAsmIndices.AddRange(asmIndxBuffer.SelectMany(x => x.Item2));
            uqAsmIndices = uqAsmIndices.Distinct().ToList();
            for (var i = 0; i < uqAsmIndices.Count; i++)
            {
                uqAsmIndices[i].IndexId = i;
            }

            //create adj graph 
            var adjGraph = new int[uqAsmIndices.Count, uqAsmIndices.Count];
            for (var i = 0; i < uqAsmIndices.Count; i++)
            {
                var iMdta = uqAsmIndices.First(x => x.IndexId == i);
                var mdtaRefs = asmIndxBuffer.FirstOrDefault(x => x.Item1.Equals(iMdta))?.Item2 ??
                               new RankedMetadataTokenAsm[0];

                for (var j = 0; j < uqAsmIndices.Count; j++)
                {
                    if (i == j)
                        continue;
                    var jMdta = uqAsmIndices.First(x => x.IndexId == j);
                    var hadEdge = mdtaRefs.Any(x => x.Equals(jMdta));
                    adjGraph[i, j] = hadEdge ? 1 : 0;
                }
            }
            //assign and return
            var asmAdjGraph = new AsmAdjancyGraph
            {
                Asms = uqAsmIndices.ToArray(),
                Graph = adjGraph,
                St = MetadataTokenStatus.Ok
            };

            return asmAdjGraph;
        }

        #endregion

    }
}
