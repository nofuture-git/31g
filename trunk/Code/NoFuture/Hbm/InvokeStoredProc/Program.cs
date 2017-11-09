using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Configuration;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Util;
using NoFuture.Util.NfConsole;

namespace NoFuture.Hbm.InvokeStoredProc
{
    public class Program : Util.NfConsole.Program
    {
        private bool _sendSocketMessages;
        public bool SendSocketMessages => _sendSocketMessages;
        public string ConnectionString { get; private set; }
        public string SpmFilePath { get; private set; }
        private SortingContainers.StoredProcMetadata _myProcMetadata;
        public SortingContainers.StoredProcMetadata MyProcMetadata => _myProcMetadata;

        public static class AppSettingKeyNames
        {
            public const string HBM_STORED_PROC_XSD_TIME_OUT = "Hbm.Settings.HbmStoredProcXsdTimeOut";
            public const string NF_TYPE_NAME_EXE = "NoFuture.ToolsCustomTools.InvokeNfTypeName";
        }

        public static void Main(string[] args)
        {
            var p = new Program(args);
            try
            {
                //get and test the cmd line arg key\values
                p.StartConsole();

                if (p.PrintHelp()) return;

                p.ParseProgramArgs();

                using (var conn = new SqlConnection(p.ConnectionString))
                {
                    Console.WriteLine($@"{p.MyProcMetadata.ProcName}");
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            var ds = new DataSet(p.MyProcMetadata.ProcName);    
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();

                            //can be controlled from app.config
                            cmd.CommandTimeout = ConfigTimeout;

                            //assigns random values to (hopefully) avoid timeouts
                            p.MyProcMetadata.AssignSpParams(cmd.Parameters);
                            cmd.CommandText = p.MyProcMetadata.ProcName;

                            var processMsg = new InvokeStoredProcExeMessage { StoredProcName = p.MyProcMetadata.ProcName };
                            try
                            {
                                da.Fill(ds);
                                if (ds.Tables.Count <= 0)
                                    processMsg.State = InvokeStoredProcExeState.NoDsReturned;
                                else if (ds.Tables.Count > 1)
                                    processMsg.State = InvokeStoredProcExeState.MultiTableDs;
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.StartsWith("Timeout expired."))
                                    processMsg.State = InvokeStoredProcExeState.TimedOut;
                                else if (ex.Message.StartsWith("Invalid ") || ex.Message.StartsWith("Incorrect syntax"))
                                {
                                    processMsg.State = InvokeStoredProcExeState.BadSyntax;
                                    Settings.WriteToStoredProcLog(ex, string.Format("bad syntax"));
                                }
                                else
                                {
                                    processMsg.State = InvokeStoredProcExeState.OtherError;
                                    Settings.WriteToStoredProcLog(ex, string.Format("invoke of da.Fill got this unexpected error."));
                                }
                            }

                            var xsdOutput = p.MyProcMetadata.XsdFilePath;
                            if (processMsg.State != InvokeStoredProcExeState.BadSyntax &&
                                processMsg.State != InvokeStoredProcExeState.TimedOut &&
                                processMsg.State != InvokeStoredProcExeState.OtherError &&
                                processMsg.State != InvokeStoredProcExeState.NoDsReturned)
                            {
                                //dump the returned dataset schema to file
                                ds.WriteXmlSchema(xsdOutput);
                            }
                            if(conn.State == ConnectionState.Open)
                                conn.Close();

                            if (!p.SendSocketMessages) return;

                            //once its on the disk then send a message to the manager
                            using (var com = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP))
                            {
                                Settings.WriteToStoredProcLog($"Sent from process '{processMsg}'");
                                com.Connect(new IPEndPoint(IPAddress.Loopback, InvokeStoredProcManager.DefaultPort));
                                com.Send(Encoding.UTF8.GetBytes(processMsg.ToString()));
                                com.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Settings.WriteToStoredProcLog(ex, "Error while executing NoFuture.Hbm.InvokeStoredProc.exe");
            }
            Thread.Sleep(NfConfig.ThreadSleepTime);//slight pause
        }

        public static int ConfigTimeout
        {
            get
            {
                var appSet = SysCfg.GetAppCfgSetting(AppSettingKeyNames.HBM_STORED_PROC_XSD_TIME_OUT);
                if (string.IsNullOrWhiteSpace(appSet))
                    return Settings.HbmStoredProcXsdTimeOut;

                int timeout;
                if (!int.TryParse(appSet, out timeout))
                    return Settings.HbmStoredProcXsdTimeOut;

                if (timeout <= 0)
                    return Settings.HbmStoredProcXsdTimeOut;

                Settings.HbmStoredProcXsdTimeOut = timeout;
                return Settings.HbmStoredProcXsdTimeOut;
            }
        }

        protected override string MyName => "NoFuture.Hbm.InvokeStoredProc";

        protected override String GetHelpText()
        {
            var help = new StringBuilder();
            help.AppendLine(" ----");
            help.AppendLine($" [{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}] ");
            help.AppendLine("");
            help.AppendLine(" This executable is specific to invoking a stored procedure.  ");
            help.AppendLine(" All switches (except for help) are required.  This functionality ");
            help.AppendLine(" exists only to allow for a definite way to terminate a very long");
            help.AppendLine(" running stored proc.  Typically setting the Timeout will work to this");
            help.AppendLine(" end; however, a stored proc which itself invokes 'sp_executesql' will");
            help.AppendLine(" not honor this timeout and may run indefinitely.");
            help.AppendLine("");
            help.AppendLine("Usage: options ");
            help.AppendLine("Options:");
            help.AppendLine(" -h | -help             Will print this help.");
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[STRING]      The full directory path ",
                NfConfig.CmdLineArgSwitch, InvokeStoredProcManager.HBM_STORED_PROX_DIR_SWITCH,
                NfConfig.CmdLineArgAssign));
            help.AppendLine("                        where the resulting .xsd files are to be ");
            help.AppendLine("                        placed.");
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[STRING]      Must be a valid connection ",
                NfConfig.CmdLineArgSwitch, InvokeStoredProcManager.CONNECTION_STR_SWITCH,
                NfConfig.CmdLineArgAssign));
            help.AppendLine("                        string to an accessable SqlServer");
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[STRING]   The path to a binary ",
                NfConfig.CmdLineArgSwitch, InvokeStoredProcManager.FILE_PATH_SWITCH,
                NfConfig.CmdLineArgAssign));
            help.AppendLine(string.Format("                        serialization of \n     '{0}' ",
                typeof (SortingContainers.StoredProcMetadata).FullName));
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[BOOL]      Tells exe to send messages back  ",
                NfConfig.CmdLineArgSwitch, InvokeStoredProcManager.SEND_MESSAGES_BACK_ON_SOCKET,
                NfConfig.CmdLineArgAssign));
            help.AppendLine(string.Format("                        on the loopback:{0}",InvokeStoredProcManager.DefaultPort));
            help.AppendLine("");

            return help.ToString();
        }

        protected override void ParseProgramArgs()
        {
            var argHash = ConsoleCmd.ArgHash(_args);

            if (argHash == null || argHash.Keys.Count <= 0)
            {
                var msg = $"could not parse cmd line arg \n{string.Join(" ", _args)}";
                throw new ItsDeadJim(msg);
            }

            NfConfig.CustomTools.InvokeNfTypeName = SysCfg.GetAppCfgSetting(AppSettingKeyNames.NF_TYPE_NAME_EXE);

            if (!argHash.ContainsKey(InvokeStoredProcManager.HBM_STORED_PROX_DIR_SWITCH) ||
                argHash[InvokeStoredProcManager.HBM_STORED_PROX_DIR_SWITCH] == null)
            {
                var msg =
                    $"the switch '{InvokeStoredProcManager.HBM_STORED_PROX_DIR_SWITCH}' could not " +
                    $"be parsed from cmd line arg \n{string.Join(" ", _args)}";
                throw new ItsDeadJim(msg);
            }

            Settings.HbmStoredProcsDirectory = argHash[InvokeStoredProcManager.HBM_STORED_PROX_DIR_SWITCH].ToString();

            if (!argHash.ContainsKey(InvokeStoredProcManager.CONNECTION_STR_SWITCH) ||
                argHash[InvokeStoredProcManager.CONNECTION_STR_SWITCH] == null)
            {
                var msg =
                    $"the switch '{InvokeStoredProcManager.CONNECTION_STR_SWITCH}' could not " +
                    $"be parsed from cmd line arg \n{string.Join(" ", _args)}";
                Settings.WriteToStoredProcLog(msg);
                throw new ItsDeadJim(msg);
            }

            if (!argHash.ContainsKey(InvokeStoredProcManager.FILE_PATH_SWITCH) ||
                argHash[InvokeStoredProcManager.FILE_PATH_SWITCH] == null)
            {
                var msg =
                    $"the switch '{InvokeStoredProcManager.FILE_PATH_SWITCH}' could not " +
                    $"be parsed from cmd line arg \n{string.Join(" ", _args)}";
                Settings.WriteToStoredProcLog(msg);
                throw new ItsDeadJim(msg);
            }

            if (!argHash.ContainsKey(InvokeStoredProcManager.SEND_MESSAGES_BACK_ON_SOCKET) ||
                argHash[InvokeStoredProcManager.SEND_MESSAGES_BACK_ON_SOCKET] == null)
            {
                var msg =
                    $"the switch '{InvokeStoredProcManager.SEND_MESSAGES_BACK_ON_SOCKET}' could not " +
                    $"be parsed from cmd line arg \n{string.Join(" ", _args)}";
                Settings.WriteToStoredProcLog(msg);
                throw new ItsDeadJim(msg);
            }


            if (
                !bool.TryParse(argHash[InvokeStoredProcManager.SEND_MESSAGES_BACK_ON_SOCKET].ToString(),
                    out _sendSocketMessages))
            {
                var msg =
                    "could not parse the value 'True' nor 'False' from " +
                    $"the '{InvokeStoredProcManager.SEND_MESSAGES_BACK_ON_SOCKET}' switch " +
                    $"using the cmd line arg \n{string.Join(" ", _args)}";
                Settings.WriteToStoredProcLog(msg);
                throw new ItsDeadJim(msg);
            }

            ConnectionString = argHash[InvokeStoredProcManager.CONNECTION_STR_SWITCH].ToString();
            SpmFilePath = argHash[InvokeStoredProcManager.FILE_PATH_SWITCH].ToString();

            if (!File.Exists(SpmFilePath))
            {
                var msg = $"bad path or file name '{SpmFilePath}'";
                Settings.WriteToStoredProcLog(msg);
                throw new ItsDeadJim(msg);
            }

            if (!SortingContainers.StoredProcMetadata.TryDeserializeFromDisk(SpmFilePath, out _myProcMetadata))
            {
                var msg =
                    $"could not deserialize the file \n'{SpmFilePath}' " +
                    $"into the type \n'{typeof(SortingContainers.StoredProcMetadata).FullName}'";
                Settings.WriteToStoredProcLog(msg);
                throw new ItsDeadJim(msg);
            }

            if (string.IsNullOrWhiteSpace(_myProcMetadata.ProcName))
            {
                var msg = "there isn't a stored proc name in " +
                          $"the deserialized file at \n'{SpmFilePath}'";
                Settings.WriteToStoredProcLog(msg);
                throw new ItsDeadJim(msg);
            }
        }

        public Program(string[] args) : base(args, true)
        {
        }
    }
}
