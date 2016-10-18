using System;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// Uses the R package 'sna' to run the Page Rank algo on 
        /// the adjacency graph <see cref="adjGraph"/>
        /// </summary>
        /// <param name="adjGraph">a square matrix of only 1's and 0's</param>
        /// <param name="keepTempFiles">
        /// Set to true if the temp files created to perform this should be perserved.
        /// </param>
        /// <returns></returns>
        public static double[] GetPageRank(int[,] adjGraph, bool keepTempFiles = false)
        {
            //validate input
            if (adjGraph == null)
                return null;
            var numOfRow = adjGraph.GetLongLength(0);
            var numOfCols = adjGraph.GetLongLength(1);
            if (adjGraph.GetLongLength(0) != adjGraph.GetLongLength(1))
                throw new ArgumentException("An adjacency graph will have the same " +
                                            "number or rows as columns, but this matrix " +
                                            $"is {numOfRow} by {numOfCols}");

            //write to a temp file to import into R Engine
            var tempFile = Path.Combine(RTempDir, Path.GetRandomFileName());
            for (var i = 0; i < numOfRow; i++)
            {
                var ln = new List<int>();
                for (var j = 0; j < numOfCols; j++)
                {
                    var item = adjGraph[i, j];
                    if(item != 0 && item != 1)
                        throw new ArgumentException("An adjacency graph is made up of " +
                                                    "only '1's and '0's, but the item at " +
                                                    $"index ({i},{j}) has a value of {adjGraph[i,j]}");
                    ln.Add(item);
                }
                File.AppendAllText(tempFile, string.Join("\t", ln) + Environment.NewLine);
            }
            var rTempFile = tempFile.Replace(@"\", "/");
            var rPath = GetRBinPath();
            var rHome = GetRInstallPath();
            if(!IsRLibInstalled("sna"))
                throw new DirectoryNotFoundException("The R package named 'sna' is not installed, " +
                                                     "locate this package on r-project.org and install it.");
            if (!IsRLibInstalled("igraph"))
                throw new DirectoryNotFoundException("The R package named 'igraph' is not installed, " +
                                                     "locate this package on r-project.org and install it.");

            REngine.SetEnvironmentVariables(rPath, rHome);
            var pageRank = new List<double>();
            using (var engine = REngine.GetInstance(initialize: false))
            {
                engine.Initialize();

                engine.Evaluate("library(sna)");
                engine.Evaluate("library(igraph)");
                engine.Evaluate($"myData <- read.table(\"{rTempFile}\",header=FALSE)");
                engine.Evaluate("myMatrix <- as.matrix(myData)");
                engine.Evaluate("myAdjGraph <- graph.adjacency(myMatrix, mode=\"directed\")");
                engine.Evaluate("myPageRank <- page.rank(myAdjGraph)");
                var rSymbol = engine.GetSymbol("myPageRank").AsList()[0].AsNumeric();
                for (var i = 0; i < rSymbol.Length; i++)
                {
                    pageRank.Add(rSymbol[i]);
                }
            }
            if (!keepTempFiles)
            {
                File.Delete(tempFile);
            }

            return pageRank.ToArray();
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

        /// <summary>
        /// Determine if the directory named <see cref="libName"/> is present 
        /// within the library folder of the R install dir.
        /// </summary>
        /// <param name="libName"></param>
        /// <returns></returns>
        public static bool IsRLibInstalled(string libName)
        {
            var rHome = GetRInstallPath();
            if (string.IsNullOrWhiteSpace(rHome) || !Directory.Exists(rHome))
                return false;

            var rLib = Path.Combine(rHome, $"library\\{libName}");
            return Directory.Exists(rLib);
        }
    }
}
