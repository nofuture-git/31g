using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NoFuture.Hbm.SortingContainers;
using NoFuture.Util;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Shared.Core.Cfg;
using NoFuture.Util.Core;
using NoFuture.Util.NfConsole;

namespace NoFuture.Hbm
{
    [Serializable]
    public delegate void MessageFromStoredProcManager(ProgressMessage msg);

    [Serializable]
    public enum InvokeStoredProcExeState
    {
        Default,
        TimedOut,
        BadSyntax,
        MultiTableDs,
        NoDsReturned,
        OtherError,
        Complete
    }

    /// <summary>
    /// This is the means of exchange between the <see cref="InvokeStoredProcManager"/> 
    /// and the NoFuture.Hbm.InvokeStoredProc.exe.  This object, as a UTF8 string, is what
    /// is sent across the socket.
    /// </summary>
    [Serializable]
    public class InvokeStoredProcExeMessage
    {
        public InvokeStoredProcExeState State;
        public string StoredProcName;

        public override string ToString()
        {
            return string.Join(",", new[] {Enum.GetName(typeof(InvokeStoredProcExeState),State), StoredProcName});
        }

        public static bool TryParse(string val, out InvokeStoredProcExeMessage msgOut)
        {
            msgOut = null;
            if (string.IsNullOrWhiteSpace(val))
                return false;
            try
            {
                if (val.Split(',').Length < 2)
                    return false;
                msgOut = new InvokeStoredProcExeMessage();
                Enum.TryParse(val.Split(',')[0], true, out msgOut.State);
                msgOut.StoredProcName = val.Split(',')[1];
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    [Serializable]
    public class StoredProxSearchCriteria
    {
        private List<string> _sqlInjParamNames;
        public string ExactName { get; set; }
        public string AnyStartingWith { get; set; }
        public string AnyNameEndingWith { get; set; }
        public string AnyNameContaining { get; set; }

        /// <summary>
        /// Use to set <see cref="SortingContainers.StoredProcParamItem.IsOpenToSqlInj"/>
        /// as true whenever the <see cref="SortingContainers.StoredProcParamItem.ParamName"/> 
        /// contains any one of these strings.
        /// </summary>
        public List<string> SqlInjOnParamNamesLike
        {
            get { return _sqlInjParamNames ?? (_sqlInjParamNames = new List<string>()); }
        }
    }

    /// <summary>
    /// A manager class used to sequentially invoke stored prox.
    /// As single-process per using the NoFuture.Hbm.InvokeStoredProc.exe.
    /// </summary>
    public class InvokeStoredProcManager
    {
        #region private fields
        private Process _currentProcess;
        private StoredProcMetadata _currentSp;
        private const string Status = "Metadata Dump";
        private readonly TaskFactory _taskFactory = new TaskFactory();

        private readonly string _nfHbmInvokeProxExePath = Path.Combine(NfConfig.BinDirectories.Root,
            "NoFuture.Hbm.InvokeStoredProc.exe");

        private int _pInvokePaddingSeconds = 10;
        #endregion

        #region cmd line switches
        /// <summary>
        /// The cmd line switch used to communicate the connection string to the 
        /// autonomous NoFuture.Hbm.InvokeStoredProc.exe.  That assembly has a ref
        /// to this assembly and directly accesses this value so its value is only
        /// defined once-for-all.
        /// </summary>
        public const string CONNECTION_STR_SWITCH = "connStr";

        /// <summary>
        /// The cmd line switch used to communicate the file-path to a serialized 
        /// copy of <see cref="NoFuture.Hbm.SortingContainers.StoredProcMetadata"/> 
        /// to the autonomous NoFuture.Hbm.InvokeStoredProc.exe (who will, in turn, 
        /// deserialize it).  That assembly has a ref to this assembly and directly 
        /// accesses this value so its value is only defined once-for-all.
        /// </summary>
        public const string FILE_PATH_SWITCH = "spmBinFile";

        /// <summary>
        /// The cmd line switch used to communicate both the directory where to write its 
        /// resultant .xsd and the directory where the <see cref="Globals.LOG_NAME"/> 
        /// is located, both being required by the autonomous NoFuture.Hbm.InvokeStoredProc.exe.  
        /// That assembly has a ref to this assembly and directly accesses this value 
        /// so its value is only defined once-for-all.
        /// </summary>
        public const string HBM_STORED_PROX_DIR_SWITCH = "hbmSpDir";

        /// <summary>
        /// The cmd line swtich used to alert the invoked NoFuture.Hbm.InvokeStoredProc.exe
        /// to send status messages <see cref="InvokeStoredProcExeMessage"/> back using a socket
        /// connected to the loopback on port <see cref="SOCKET_COMM_PORT"/>.
        /// </summary>
        public const string SEND_MESSAGES_BACK_ON_SOCKET = "socketMsgBack";

        /// <summary>
        /// This value is not passed to the autonomous process but rather used as it 
        /// is - a constant.  The process will send UTF8 string versions of <see cref="InvokeStoredProcExeMessage"/>
        /// across the socket and the manager, listening on the same port, will receive and record them.
        /// </summary>
        public static int DefaultPort = NfConfig.NfDefaultPorts.HbmInvokeStoredProcMgr;
        #endregion

        #region message communication

        /// <summary>
        /// The last <see cref="ProgressMessage"/> to be broadcast will have this 
        /// as its <see cref="ProgressMessage.Status"/>.
        /// </summary>
        public const string COMPLETED_STATUS_STRING = "DONE";

        /// <summary>
        /// This event operates as a 'way' to communicate progress 
        /// to some assembly (typically, as the name implies, PowerShell).
        /// </summary>
        public event MessageFromStoredProcManager CommLinkToPs;

        /// <summary>
        /// Exactly as the name implies, this function will 'broadcast' the 
        /// <see cref="msg"/> to all the subscribers of the <see cref="CommLinkToPs"/>
        /// </summary>
        /// <param name="msg"></param>
        /// <remarks>
        /// The <see cref="msg"/> Status is inspected for the value of <see cref="COMPLETED_STATUS_STRING"/>
        /// and having this status this message handler will close the async thread task and
        /// dump all reporting data to files.
        /// </remarks>
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
                var handler = enumerable.Current as MessageFromStoredProcManager;
                if (handler == null)
                    continue;
                handler.BeginInvoke(msg, Callback, msg.ProcName);
            }

            //use this Status as que to dispose the async task if its present and log reporting data
            if (msg.Status != COMPLETED_STATUS_STRING) return;

            if (GetSpResultSetXsdTask != null)
                GetSpResultSetXsdTask.Dispose();

            //dump the reporting data to file
            File.AppendAllLines(Path.Combine(Settings.HbmStoredProcsDirectory, "__KilledProx.txt"),
                Sorting.KilledProx);
            File.AppendAllLines(Path.Combine(Settings.HbmStoredProcsDirectory, "__TimedOutProx.txt"),
                Sorting.TimedOutProx);
            File.AppendAllLines(Path.Combine(Settings.HbmStoredProcsDirectory, "__BadSyntaxProx.txt"),
                Sorting.BadSyntaxProx);
            File.AppendAllLines(Path.Combine(Settings.HbmStoredProcsDirectory, "__BadSyntaxProx.txt"),
                Sorting.BadSyntaxProx);
            File.AppendAllLines(Path.Combine(Settings.HbmStoredProcsDirectory, "__UnknownErrorProx.txt"),
                Sorting.UnknownErrorProx);
            File.AppendAllLines(Path.Combine(Settings.HbmStoredProcsDirectory, "__EmptyDatasetProx.txt"),
                Sorting.EmptyDatasetProx);
            File.AppendAllLines(Path.Combine(Settings.HbmStoredProcsDirectory, "__MultiTableDsProx.txt"),
                Sorting.MultiTableDsProx);
            File.AppendAllLines(Path.Combine(Settings.HbmStoredProcsDirectory, "__NoDatasetReturnedProx.txt"),
                Sorting.NoDatasetReturnedProx);
        }

        internal void Callback(IAsyncResult ar)
        {
            var arr = ar as AsyncResult;
            if (arr == null)
                return;
            var handler = arr.AsyncDelegate as MessageFromStoredProcManager;
            if (handler == null)
                return;
            handler.EndInvoke(ar);
        }

        /// <summary>
        /// This function is intended to be invoked async in that the invoking thread
        /// will not return but continue to listen on a socket.  
        /// This communication channel is the means of sending info from the autonomous
        /// NoFuture.Hbm.InvokeStoredProc.exe to this manager.
        /// </summary>
        internal void BeginReceiveMsgFromProcess()
        {
            using (
                var processListeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP))
            {
                processListeningSocket.Bind(new IPEndPoint(IPAddress.Loopback, DefaultPort));
                processListeningSocket.Listen(5);

                for (; ; )
                {
                    try
                    {
                        var received = new List<byte>();
                        var client = processListeningSocket.Accept();

                        var bytesReceived = 0;
                        while (client.Available > 0)
                        {
                            byte[] bytes;
                            if (client.Available < NfConfig.DefaultBlockSize)
                            {
                                bytes = new byte[client.Available];
                                bytesReceived += client.Receive(bytes, 0, client.Available, 0);
                            }
                            else
                            {
                                bytes = new byte[NfConfig.DefaultBlockSize];
                                bytesReceived += client.Receive(bytes, 0, (int)NfConfig.DefaultBlockSize, 0);
                            }

                            received.AddRange(bytes);
                        }
                        InvokeStoredProcExeMessage processMsg;
                        var messageStr = Encoding.UTF8.GetString(received.ToArray());
                        Settings.WriteToStoredProcLog(string.Format("Received from process '{0}'",messageStr));
                        if (InvokeStoredProcExeMessage.TryParse(messageStr, out processMsg))
                        {
                            switch (processMsg.State)
                            {
                                case InvokeStoredProcExeState.BadSyntax:
                                    if (!Sorting.BadSyntaxProx.Contains(processMsg.StoredProcName))
                                        Sorting.BadSyntaxProx.Add(processMsg.StoredProcName);
                                    break;
                                case InvokeStoredProcExeState.MultiTableDs:
                                    if (!Sorting.MultiTableDsProx.Contains(processMsg.StoredProcName))
                                        Sorting.MultiTableDsProx.Add(processMsg.StoredProcName);
                                    break;
                                case InvokeStoredProcExeState.TimedOut:
                                    if (!Sorting.TimedOutProx.Contains(processMsg.StoredProcName))
                                        Sorting.TimedOutProx.Add(processMsg.StoredProcName);
                                    break;
                                case InvokeStoredProcExeState.NoDsReturned:
                                    if (!Sorting.NoDatasetReturnedProx.Contains(processMsg.StoredProcName))
                                        Sorting.NoDatasetReturnedProx.Add(processMsg.StoredProcName);
                                    break;
                                case InvokeStoredProcExeState.OtherError:
                                    if (!Sorting.UnknownErrorProx.Contains(processMsg.StoredProcName))
                                        Sorting.UnknownErrorProx.Add(processMsg.StoredProcName);
                                    break;
                            }
                        }

                        client.Close();
                    }
                    catch (Exception ex)
                    {
                        Settings.WriteToStoredProcLog(ex, string.Format("Socket error while listening for external NoFuture.Hbm.InvokeStoredProc.exe on port '{0}'.", DefaultPort));
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// This is an instance level field to allow for it to be disposed on a key <see cref="ProgressMessage.Status"/>
        /// </summary>
        internal Task GetSpResultSetXsdTask;

        /// <summary>
        /// This is the extra seconds added to <see cref="Settings.HbmStoredProcXsdTimeOut"/>
        /// the sum of both represents the max number of seconds the NoFuture.Hbm.InvokeStoredProc.exe
        /// is allowed to run before being shutdown.
        /// </summary>
        public int PInvokePaddingInSeconds 
        {
            get { return _pInvokePaddingSeconds; }
            set { _pInvokePaddingSeconds = value; }
        }

        /// <summary>
        /// The asynchronous version of the function which gets 
        /// the returned datasets from stored prox at the location specified 
        /// </summary>
        /// <param name="filterOnNamesLike"></param>
        /// <param name="connectionString"></param>
        /// <remarks>
        /// See the details on <see cref="GetSpResultSetXsd"/> for 
        /// more information on the manner an purpose of this function.
        /// </remarks>
        public void BeginGetSpResultSetXsd(StoredProxSearchCriteria filterOnNamesLike, string connectionString)
        {
            GetSpResultSetXsdTask = _taskFactory.StartNew(() => GetSpResultSetXsd(filterOnNamesLike, connectionString));
        }

        /// <summary>
        /// The synchronous version of the function which gets 
        /// the returned datasets from stored prox at the location specified 
        /// at <see cref="connectionString"/>.
        /// </summary>
        /// <param name="filterOnNamesLike"> A proc(s) search criteria. </param>
        /// <param name="connectionString">
        /// The calling assembly is forced to specify what the connection string is.
        /// This value should bare a semblance to the current connection values at 
        /// <see cref="NfConfig.SqlServer"/> and 
        /// <see cref="NfConfig.SqlCatalog"/>
        /// but its value is not inspected verifying this.  
        /// </param>
        /// <remarks>
        /// The excessive level of indirection here is an attempt to gain control 
        /// over the fact that any stored proc which itself calls 'sp_executesql' does
        /// not honor the timeout value assigned on the <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// Since every proc is invoked with random values, a stored proc
        /// may run endlessly and comsume the entire machines resources. 
        /// Placing all this under the control of an autonomous process allows for the process, as a whole,
        /// to be shutdown, and SqlServer has to clean up the mess.
        /// </remarks>
        public void GetSpResultSetXsd(StoredProxSearchCriteria filterOnNamesLike, string connectionString)
        {
            //blow up for this
            if(!File.Exists(_nfHbmInvokeProxExePath))
                throw new ItsDeadJim(string.Format("The required console app is missing at '{0}'", _nfHbmInvokeProxExePath));

            BroadcastProgress(new ProgressMessage
            {
                Activity = "Loading current connection",
                Status = Status
            });

            Settings.LoadOutputPathCurrentSettings();

            BroadcastProgress(new ProgressMessage
            {
                Activity = "Getting list of stored proc targets",
                Status = Status
            });

            //copy into local copy the global sql inj param names 
            if (Settings.SqlInjParamNames.Any())
            {
                filterOnNamesLike = filterOnNamesLike ?? new StoredProxSearchCriteria();
                filterOnNamesLike.SqlInjOnParamNamesLike.AddRange(Settings.SqlInjParamNames);
            }

            //check the current connection has any procs present 
            var allStoredProx = GetFilteredStoredProcNames(filterOnNamesLike);
            if (allStoredProx == null || allStoredProx.Count <= 0)
            {
                BroadcastProgress(new ProgressMessage
                {
                    Activity = "There are no prox at NoFuture.Hbm.Sorting.AllStoredProx",
                    Status = Status
                });
                return;
            }

            //begin async listening for messages from autonomous processes
            var socketListeningTask = _taskFactory.StartNew(BeginReceiveMsgFromProcess);

            var counter = 0;

            foreach (var spItemKey in allStoredProx.Keys)
            {
                //check for the name being on the black list
                if (Settings.DoNotReference.Contains(spItemKey))
                {
                    counter += 1;
                    continue;
                }
                
                //assign to instance level
                _currentSp = allStoredProx[spItemKey];

                //serialize to disk - invoked process will read from this location
                _currentSp.SerializeToDisk();

                //start a P\Invoke to NoFuture.Hbm.InvokeStoredProc.exe
                using (_currentProcess = new Process
                {
                    StartInfo =
                        new ProcessStartInfo(_nfHbmInvokeProxExePath, ConstructCmdLineArgs(_currentSp, connectionString))
                        {
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true
                        }
                })
                {
                    BroadcastProgress(new ProgressMessage
                    {
                        Activity = string.Format("At sp '{0}'", _currentSp.ProcName),
                        Status = Status,
                        ProgressCounter = Etc.CalcProgressCounter(counter, allStoredProx.Count),
                        ProcName = _currentSp.ProcName
                    });

                    try
                    {
                        Settings.WriteToStoredProcLog(string.Format("working '{0}'", _currentSp.ProcName));

                        //fire off the exe
                        _currentProcess.Start();

                        //wait for it to finish or timeout
                        _currentProcess.WaitForExit((Settings.HbmStoredProcXsdTimeOut + PInvokePaddingInSeconds) * 1000);

                        //presumes it timed out
                        if (!_currentProcess.HasExited)
                        {
                            //the only sure way to end some p.o.s. proc
                            _currentProcess.Kill();
                            Settings.WriteToStoredProcLog(string.Format("Stored proc '{0}' is not responding is will be shutdown", _currentSp.ProcName));
                            Sorting.KilledProx.Add(_currentSp.ProcName);
                        }
                    }
                    catch (Exception ex)
                    {
                        Settings.WriteToStoredProcLog(ex, string.Format("unknown error at '{0}'", _currentSp.ProcName));
                    }

                    counter += 1;
                }
            }

            //communicate that the has completed
            BroadcastProgress(new ProgressMessage
            {
                Activity = "GetSpResultSetXsd has completed",
                Status = COMPLETED_STATUS_STRING,
                ProgressCounter = 100,
            });

            if(socketListeningTask.IsCompleted)
                socketListeningTask.Dispose();
        }

        /// <summary>
        /// Simple helper function to calc the expected amount of 
        /// time calling <see cref="GetSpResultSetXsd"/> is going to
        /// take given an average of 3 seconds per proc.
        /// </summary>
        /// <returns></returns>
        public TimeSpan CalcExpectedProcessingTime()
        {
            return new TimeSpan(0, 0, 0, (3*Sorting.AllStoredProcNames.Count));//avg 3 seconds per proc
        }

        /// <summary>
        /// A helper function used to draft the entire command line switch (s)
        /// which are passed to the NoFuture.Hbm.InvokeStoredProc.exe process.
        /// 
        /// The function handles wrapping values in double quotes when said value
        /// contains a space.  When the args are assigned to a <see cref="System.Diagnostics.ProcessStartInfo"/>
        /// as a single string and, in turn, received by a console's Main statement as a 
        /// string array the values are being split on the space char (0x20) and the double-quotes
        /// are removed.
        /// </summary>
        /// <param name="spItem"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string ConstructCmdLineArgs(StoredProcMetadata spItem, string connectionString)
        {
            var cmdArg = new StringBuilder();
            cmdArg.Append(ConsoleCmd.ConstructCmdLineArgs(CONNECTION_STR_SWITCH, connectionString));

            cmdArg.Append(" ");
            cmdArg.Append(ConsoleCmd.ConstructCmdLineArgs(FILE_PATH_SWITCH, spItem.BinFilePath));

            cmdArg.Append(" ");
            cmdArg.Append(ConsoleCmd.ConstructCmdLineArgs(HBM_STORED_PROX_DIR_SWITCH, Settings.HbmStoredProcsDirectory));

            cmdArg.Append(" ");
            cmdArg.Append(ConsoleCmd.ConstructCmdLineArgs(SEND_MESSAGES_BACK_ON_SOCKET, Boolean.TrueString));

            return cmdArg.ToString();
        }

        protected internal Dictionary<string, StoredProcMetadata> GetFilteredStoredProcNames(StoredProxSearchCriteria filterOnNamesLike)
        {
            var allStoredProx = Sorting.AllStoredProx;
            if (allStoredProx == null || allStoredProx.Count <= 0)
                return null;
            //apply filter if any
            if (filterOnNamesLike == null)
            {
                return allStoredProx;
            }

            var matchedProx = new Dictionary<string, StoredProcMetadata>();
            List<string> matchedKeys = null;

            if (!string.IsNullOrWhiteSpace(filterOnNamesLike.ExactName))
            {
                matchedKeys =
                    allStoredProx.Keys.Where(
                        x => string.Equals(x, filterOnNamesLike.ExactName, StringComparison.OrdinalIgnoreCase)).ToList();
                var exactNameEntries = allStoredProx.Keys.Where(matchedKeys.Contains)
                    .ToDictionary(pName => pName, pName => allStoredProx[pName]);
                foreach (var k in exactNameEntries.Keys.Where(k => !matchedProx.ContainsKey(k)))
                    matchedProx.Add(k, exactNameEntries[k]);
            }
            if (!string.IsNullOrWhiteSpace(filterOnNamesLike.AnyStartingWith))
            {
                matchedKeys =
                    allStoredProx.Keys.Where(
                        x =>
                            Regex.IsMatch(x, string.Format("^{0}.*", filterOnNamesLike.AnyStartingWith),
                                RegexOptions.IgnoreCase)).ToList();
                var nameEntries = allStoredProx.Keys.Where(matchedKeys.Contains)
                    .ToDictionary(pName => pName, pName => allStoredProx[pName]);
                foreach (var k in nameEntries.Keys.Where(k => !matchedProx.ContainsKey(k)))
                    matchedProx.Add(k, nameEntries[k]);
            }
            if (!string.IsNullOrWhiteSpace(filterOnNamesLike.AnyNameEndingWith))
            {
                matchedKeys =
                    allStoredProx.Keys.Where(
                        x =>
                            Regex.IsMatch(x, string.Format(".*{0}$", filterOnNamesLike.AnyStartingWith),
                                RegexOptions.IgnoreCase)).ToList();
                var nameEntries = allStoredProx.Keys.Where(matchedKeys.Contains)
                    .ToDictionary(pName => pName, pName => allStoredProx[pName]);
                foreach (var k in nameEntries.Keys.Where(k => !matchedProx.ContainsKey(k)))
                    matchedProx.Add(k, nameEntries[k]);
            }
            if (!string.IsNullOrWhiteSpace(filterOnNamesLike.AnyNameContaining))
            {
                matchedKeys =
                    allStoredProx.Keys.Where(
                        x =>
                            Regex.IsMatch(x, string.Format(".*{0}.*", filterOnNamesLike.AnyStartingWith),
                                RegexOptions.IgnoreCase)).ToList();
                var nameEntries = allStoredProx.Keys.Where(matchedKeys.Contains)
                    .ToDictionary(pName => pName, pName => allStoredProx[pName]);
                foreach (var k in nameEntries.Keys.Where(k => !matchedProx.ContainsKey(k)))
                    matchedProx.Add(k, nameEntries[k]);
            }

            allStoredProx = matchedProx;

            if (filterOnNamesLike.SqlInjOnParamNamesLike == null || filterOnNamesLike.SqlInjOnParamNamesLike.Count <= 0)
            {
                return allStoredProx;
            }

            foreach (var param in allStoredProx.SelectMany(x => x.Value.Parameters))
            {
                foreach (var namedLike in filterOnNamesLike.SqlInjOnParamNamesLike)
                {
                    if (!param.ParamName.Contains(namedLike))
                    {
                        continue;
                    }

                    param.IsOpenToSqlInj = true;
                    break;
                }
            }

            return allStoredProx;
        }
    }
}
