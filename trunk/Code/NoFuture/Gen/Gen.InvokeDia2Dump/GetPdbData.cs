using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Shared.Core.Cfg;
using NoFuture.Shared.DiaSdk;
using NoFuture.Shared.DiaSdk.LinesSwitch;

namespace NoFuture.Gen.InvokeDia2Dump
{
    /// <summary>
    /// The type used to broadcast progress events from an instance of <see cref="GetPdbData"/>.
    /// </summary>
    /// <param name="msg"></param>
    [Serializable]
    public delegate void InvokeDia2DumpProgress(ProgressMessage msg);

    /// <summary>
    /// Represents structured calls to the the Dia2Dump.exe in which 
    /// the standard output is written to the respective file of whichever implementation of 
    /// <see cref="NoFuture.Shared.DiaSdk.IPdbJsonDataFile"/> and then likewise that data is
    /// read and deserialized from the file.  The object that is returned from each structured 
    /// call contains both its File's name and direct access to the derserialized object therein.
    /// </summary>
    public class GetPdbData
    {
        #region fields
        private const string PDB_EXTENSION = ".pdb";
        private string _assemblyFilePath;

        private Process _dia2DumpExe;
        private readonly StringBuilder _stdOut = new StringBuilder();
        private readonly StringBuilder _stdErr = new StringBuilder();
        #endregion

        #region ctor
        /// <summary>
        /// In order to invoke Dia2Dump.exe access to an assemblies .pdb data is required.
        /// </summary>
        /// <param name="assemblyPath">
        /// The path to either an assembly (.exe, .dll, etc.) which has its .pdb directly next 
        /// to it or just the path to the .pdb file itself.
        /// </param>
        public GetPdbData(string assemblyPath)
        {
            assemblyPath = Path.GetExtension(assemblyPath) != PDB_EXTENSION
                ? Path.ChangeExtension(assemblyPath, PDB_EXTENSION)
                : assemblyPath;

            if (!File.Exists(assemblyPath))
                throw new ItsDeadJim(string.Format("There is no such assembly at '{0}'", assemblyPath));

            _assemblyFilePath = assemblyPath;

        }
        #endregion

        #region Message out to ps
        public event InvokeDia2DumpProgress CommLinkToPs;

        public void BroadcastProgress(ProgressMessage msg)
        {
            if (CommLinkToPs == null)
                return;
            if (msg == null)
                return;

            var subscribers = CommLinkToPs.GetInvocationList();
            var enumerable = subscribers.GetEnumerator();
            while (enumerable.MoveNext())
            {
                var handler = enumerable.Current as InvokeDia2DumpProgress;
                if (handler == null)
                    continue;
                handler.BeginInvoke(msg, Callback, msg.ProcName);
            }            
        }
        internal void Callback(IAsyncResult ar)
        {
            var arr = ar as AsyncResult;
            if (arr == null)
                return;
            var handler = arr.AsyncDelegate as InvokeDia2DumpProgress;
            if (handler == null)
                return;
            handler.EndInvoke(ar);
        }

        #endregion

        #region Invoke API
        public const int DEFAULT_WAIT_FOR_EXIT_SECONDS = 60;

        /// <summary>
        /// The amount of time the invocation to Dia2Dump.exe will wait 
        /// before sending the <see cref="System.Diagnostics.Process.Kill"/> 
        /// command to the running <see cref="System.Diagnostics.Process"/>.
        /// </summary>
        /// <remarks>
        /// Any value assigned to 0 or less will be set to the <see cref="DEFAULT_WAIT_FOR_EXIT_SECONDS"/>.
        /// </remarks>
        public static int WaitForExitInSeconds { get; set; }

        /// <summary>
        /// This is intended to get the line data of one symbol at 
        /// a time very quickly, and, as such, does not have all 
        /// the dependecies on data being written to disk.
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public PdbCompiland SingleTypeNamed(string typeFullName)
        {
            _stdOut.Clear();
            _stdErr.Clear();

            var switchValue = string.Format("{0} {1}", Dia2DumpSingleSwitches.TypeFullName, typeFullName);
            var invocationResult = InvokeProcessExe(switchValue);
            if (!invocationResult)
                throw new ItsDeadJim(
                    string.Format(
                        "The Dia2Dump.exe suffered Kill command - this may indicate that the Wait Time " +
                        "in Seconds of '{0}' is too low.",
                        WaitForExitInSeconds));

            var singleSlash = new string(new[] { (char)0x5C });
            var dblSlash = new string(new[] { (char)0x5C, (char)0x5C });

            var jsonResult = _stdOut.ToString().Replace(singleSlash, dblSlash);
            return CompilandJsonData.Parse(jsonResult, PdbJsonDataFormat.BackslashDoubled);
        }

        /// <summary>
        /// Invocation of the Dia2Dump.exe using the <see cref="Dia2DumpAllSwitches.Modules"/> switch.
        /// </summary>
        /// <param name="outFile">
        /// Optional, if null, empty or invalid the dump file 
        /// reverts to its default directory at <see cref="NfConfig.TempDirectories.Code"/>
        /// </param>
        public AllModulesJsonDataFile DumpAllModulesToFile(string outFile)
        {
            BroadcastProgress(new ProgressMessage
            {
                Activity = string.Format("Getting Pdb Modules for '{0}'", _assemblyFilePath),
                Status = "OK",
                ProgressCounter = 16,
                ProcName = NfConfig.CustomTools.Dia2Dump
            });
            var jsonDataOut = ValidateAssignedOutputFile(outFile)
                ? new AllModulesJsonDataFile(outFile)
                : new AllModulesJsonDataFile();
            UsingSwitchInvocation(Dia2DumpAllSwitches.Modules, jsonDataOut.FullFileName);
            return jsonDataOut;
        }

        /// <summary>
        /// Invocation of the Dia2Dump.exe using the <see cref="Dia2DumpAllSwitches.Globals"/> switch.
        /// </summary>
        /// <param name="outFile">
        /// Optional, if null, empty or invalid the dump file 
        /// reverts to its default directory at <see cref="NfConfig.TempDirectories.Code"/>
        /// </param>
        /// <returns></returns>
        public AllGlobalsJsonDataFile DumpAllGlobalsToFile(string outFile)
        {
            BroadcastProgress(new ProgressMessage
            {
                Activity = string.Format("Getting Pdb Globals for '{0}'", _assemblyFilePath),
                Status = "OK",
                ProgressCounter = 32,
                ProcName = NfConfig.CustomTools.Dia2Dump
            });
            var jsonDataOut = ValidateAssignedOutputFile(outFile)
                ? new AllGlobalsJsonDataFile(outFile)
                : new AllGlobalsJsonDataFile();
            UsingSwitchInvocation(Dia2DumpAllSwitches.Globals, jsonDataOut.FullFileName);
            return jsonDataOut;
        }

        /// <summary>
        /// Invocation of the Dia2Dump.exe using the <see cref="Dia2DumpAllSwitches.Files"/> switch.
        /// </summary>
        /// <param name="outFile">
        /// Optional, if null, empty or invalid the dump file 
        /// reverts to its default directory at <see cref="NfConfig.TempDirectories.Code"/>
        /// </param>
        /// <returns></returns>
        public AllFilesJsonDataFile DumpAllFilesToFile(string outFile)
        {
            BroadcastProgress(new ProgressMessage
            {
                Activity = string.Format("Getting Pdb Files for '{0}'", _assemblyFilePath),
                Status = "OK",
                ProgressCounter = 48,
                ProcName = NfConfig.CustomTools.Dia2Dump
            });
            var jsonDataOut = ValidateAssignedOutputFile(outFile)
                ? new AllFilesJsonDataFile(outFile)
                : new AllFilesJsonDataFile();
            UsingSwitchInvocation(Dia2DumpAllSwitches.Files, jsonDataOut.FullFileName);
            return jsonDataOut;
        }

        /// <summary>
        /// Invocation of the Dia2Dump.exe using the <see cref="Dia2DumpAllSwitches.Lines"/> switch.
        /// </summary>
        /// <param name="outFile">
        /// Optional, if null, empty or invalid the dump file 
        /// reverts to its default directory at <see cref="NfConfig.TempDirectories.Code"/>
        /// </param>
        /// <returns></returns>
        public AllLinesJsonDataFile DumpAllLinesToFile(string outFile)
        {
            BroadcastProgress(new ProgressMessage
            {
                Activity = string.Format("Getting Pdb Lines for '{0}'", _assemblyFilePath),
                Status = "OK",
                ProgressCounter = 64,
                ProcName = NfConfig.CustomTools.Dia2Dump
            });
            var jsonDataOut = ValidateAssignedOutputFile(outFile)
                ? new AllLinesJsonDataFile(outFile)
                : new AllLinesJsonDataFile();
            UsingSwitchInvocation(Dia2DumpAllSwitches.Lines, jsonDataOut.FullFileName);
            return jsonDataOut;
        }

        /// <summary>
        /// Invocation of the Dia2Dump.exe using the <see cref="Dia2DumpAllSwitches.Symbols"/> switch.
        /// </summary>
        /// <param name="outFile">
        /// Optional, if null, empty or invalid the dump file 
        /// reverts to its default directory at <see cref="NfConfig.TempDirectories.Code"/>
        /// </param>
        /// <returns></returns>
        public AllSymbolsJsonDataFile DumpAllSymbolsToFile(string outFile)
        {
            BroadcastProgress(new ProgressMessage
            {
                Activity = string.Format("Getting Pdb Symbols for '{0}'", _assemblyFilePath),
                Status = "OK",
                ProgressCounter = 64,
                ProcName = NfConfig.CustomTools.Dia2Dump
            });
            var jsonDataOut = ValidateAssignedOutputFile(outFile)
                ? new AllSymbolsJsonDataFile(outFile)
                : new AllSymbolsJsonDataFile();
            UsingSwitchInvocation(Dia2DumpAllSwitches.Symbols, jsonDataOut.FullFileName);
            return jsonDataOut;
        }

        /// <summary>
        /// Invocation of the Dia2Dump.exe using the <see cref="Dia2DumpAllSwitches.Sections"/> switch.
        /// </summary>
        /// <param name="outFile">
        /// Optional, if null, empty or invalid the dump file 
        /// reverts to its default directory at <see cref="NfConfig.TempDirectories.Code"/>
        /// </param>
        /// <returns></returns>
        public AllSectionsJsonDataFile DumpAllSectionsToFile(string outFile)
        {
            BroadcastProgress(new ProgressMessage
            {
                Activity = string.Format("Getting Pdb Symbols for '{0}'", _assemblyFilePath),
                Status = "OK",
                ProgressCounter = 64,
                ProcName = NfConfig.CustomTools.Dia2Dump
            });
            var jsonDataOut = ValidateAssignedOutputFile(outFile)
                ? new AllSectionsJsonDataFile(outFile)
                : new AllSectionsJsonDataFile();
            UsingSwitchInvocation(Dia2DumpAllSwitches.Sections, jsonDataOut.FullFileName);
            return jsonDataOut;
        }
        #endregion

        #region internal helpers
        internal void UsingSwitchInvocation(string switchValue, string outputFileName)
        {
            _stdOut.Clear();
            _stdErr.Clear();

            var invocationResult = InvokeProcessExe(switchValue);
            if (!invocationResult)
                throw new ItsDeadJim(
                    string.Format(
                        "The Dia2Dump.exe suffered Kill command - this may indicate that the Wait " +
                        "Time in Seconds of '{0}' is too low.",
                        WaitForExitInSeconds));

            var singleSlash = new string(new[] {(char) 0x5C});
            var dblSlash = new string(new[] { (char)0x5C, (char)0x5C });
            var jsonResult = "{" + (_stdOut.ToString().Replace(singleSlash, dblSlash)) + "}";
            File.WriteAllBytes(outputFileName, Encoding.UTF8.GetBytes(jsonResult));
        }

        internal bool InvokeProcessExe(string switchValue)
        {
            var dia2Dump = NfConfig.CustomTools.Dia2Dump;

            if(string.IsNullOrWhiteSpace(dia2Dump))
                throw new RahRowRagee("The constants value at 'NoFuture.CustomTools.Dia2Dump' is not assigned.");
            if(!File.Exists(dia2Dump))
                throw new ItsDeadJim(string.Format("The binary 'Dia2Dump.exe' is not at '{0}'.",dia2Dump));

            using (_dia2DumpExe = new Process
            {
                StartInfo = new ProcessStartInfo(dia2Dump)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    Arguments = string.Format("{0} {1}", switchValue, _assemblyFilePath)
                }
            })
            {
                var waitTime = WaitForExitInSeconds <= 0 ? DEFAULT_WAIT_FOR_EXIT_SECONDS : WaitForExitInSeconds;
                _dia2DumpExe.Start();
                _stdOut.Append(_dia2DumpExe.StandardOutput.ReadToEnd());
                _dia2DumpExe.WaitForExit(waitTime * 1000);

                if (_dia2DumpExe.HasExited)
                    return true;

                _dia2DumpExe.Kill();
                return false;
            }
        }

        internal bool ValidateAssignedOutputFile(string outFile)
        {
            return !string.IsNullOrWhiteSpace(outFile) &&
                   !string.IsNullOrWhiteSpace(Path.GetDirectoryName(outFile)) &&
                   Directory.Exists(Path.GetDirectoryName(outFile));
        }
        #endregion
    }
}
