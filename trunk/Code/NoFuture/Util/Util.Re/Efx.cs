using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using RDotNet;

namespace NoFuture.Util.Re
{
    public class Efx
    {
        public static string RVersion { get; } = "3.3.1";
        private static string _rTempDir;

        /// <summary>
        /// Its easier to write tsv to disk then read it into the engine.
        /// </summary>
        public static string RTempDir
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(_rTempDir) && Directory.Exists(_rTempDir))
                    return _rTempDir;

                _rTempDir =
                    Path.Combine(string.IsNullOrWhiteSpace(TempDirectories.TsvCsv)
                        ? TempDirectories.AppData
                        : TempDirectories.TsvCsv);
                return _rTempDir;
            }
        }
        /// <summary>
        /// Gets the R install path from the registry
        /// </summary>
        /// <returns></returns>
        public static string GetRInstallPath()
        {
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey($@"SOFTWARE\R-core\R\{RVersion}"))
            {
                if (registryKey == null)
                    throw new ApplicationException("No Registry entry found for the R installation.");
                return (string)registryKey.GetValue("InstallPath");
            }
        }

        public static void MyExample()
        {
            try
            {
                var rPath = GetRBinPath();
                var rHome = GetRInstallPath();
                REngine.SetEnvironmentVariables(rPath, rHome);
                using (var engine = REngine.GetInstance(initialize: false))
                {
                    engine.Initialize();

                    engine.Evaluate("library(sna)");
                    engine.Evaluate("library(igraph)");
                    engine.Evaluate("myData <- read.table(\"C:/Temp/TestIO_InR.tsv\",header=FALSE)");
                    engine.Evaluate("myMatrix <- as.matrix(myData)");
                    engine.Evaluate("myAdjGraph <- graph.adjacency(myMatrix, mode=\"directed\")");
                    engine.Evaluate("myPageRank <- page.rank(myAdjGraph)");
                    var pageRankDf = engine.GetSymbol("myPageRank").AsList()[0].AsNumeric();

                }

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Get the R bin directory off <see cref="GetRInstallPath"/>.
        /// </summary>
        /// <returns></returns>
        public static string GetRBinPath()
        {
            var rInstall = GetRInstallPath();
            if (string.IsNullOrEmpty(rInstall))
                throw new ArgumentException("Could not locate registry entry for the R enviornment.");
            if (!Directory.Exists(rInstall))
                throw new DirectoryNotFoundException($"R not installed at {rInstall}");
            var rPath = Environment.Is64BitProcess
                ? Path.Combine(rInstall, @"bin\x64")
                : Path.Combine(rInstall, @"bin\i386");
            if (!Directory.Exists(rPath))
                throw new DirectoryNotFoundException($"R not bin not found at {rPath}");
            return rPath;
        }
    }
}
