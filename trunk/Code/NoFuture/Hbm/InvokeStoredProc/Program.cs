using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Configuration;

namespace NoFuture.Hbm.InvokeStoredProc
{
    public class Program
    {
        public static class AppSettingKeyNames
        {
            public const string HbmStoredProcXsdTimeOut = "Hbm.Settings.HbmStoredProcXsdTimeOut";
        }
        public static void Main(string[] args)
        {
            try
            {
                //test if there are any args, if not just leave with no message
                if (args.Length == 0 || args[0] == "-h" || args[0] == "-help" || args[0] == "/?")
                {
                    Console.WriteLine(Help());
                    return;
                }

                //get and test the cmd line arg key\values
                var argHash = Util.ConsoleCmd.ArgHash(args);

                if (argHash == null || argHash.Keys.Count <= 0)
                {
                    var msg = string.Format("could not parse cmd line arg \n{0}", string.Join(" ", args));
                    Console.WriteLine(msg);
                    return;
                }
                if (!argHash.ContainsKey(InvokeStoredProcManager.HBM_STORED_PROX_DIR_SWITCH) ||
                    argHash[InvokeStoredProcManager.HBM_STORED_PROX_DIR_SWITCH] == null)
                {
                    var msg = string.Format("the switch '{0}' could be parsed from cmd line arg \n{1}",
                        InvokeStoredProcManager.HBM_STORED_PROX_DIR_SWITCH, string.Join(" ", args));
                    Console.WriteLine(msg);
                    return;
                }

                Settings.HbmStoredProcsDirectory = argHash[InvokeStoredProcManager.HBM_STORED_PROX_DIR_SWITCH].ToString();
                
                if (!argHash.ContainsKey(InvokeStoredProcManager.CONNECTION_STR_SWITCH) ||
                    argHash[InvokeStoredProcManager.CONNECTION_STR_SWITCH] == null)
                {
                    var msg = string.Format("the switch '{0}' could be parsed from cmd line arg \n{1}",
                        InvokeStoredProcManager.CONNECTION_STR_SWITCH, string.Join(" ", args));
                    Settings.WriteToStoredProcLog(msg);
                    Console.WriteLine(msg);
                    return;
                }

                if (!argHash.ContainsKey(InvokeStoredProcManager.FILE_PATH_SWITCH) ||
                    argHash[InvokeStoredProcManager.FILE_PATH_SWITCH] == null)
                {
                    var msg = string.Format("the switch '{0}' could be parsed from cmd line arg \n{1}",
                        InvokeStoredProcManager.FILE_PATH_SWITCH, string.Join(" ", args));
                    Settings.WriteToStoredProcLog(msg);
                    Console.WriteLine(msg);
                    return;
                }

                if (!argHash.ContainsKey(InvokeStoredProcManager.SEND_MESSAGES_BACK_ON_SOCKET) ||
                    argHash[InvokeStoredProcManager.SEND_MESSAGES_BACK_ON_SOCKET] == null)
                {
                    var msg = string.Format("the switch '{0}' could be parsed from cmd line arg \n{1}",
                        InvokeStoredProcManager.SEND_MESSAGES_BACK_ON_SOCKET, string.Join(" ", args));
                    Settings.WriteToStoredProcLog(msg);
                    Console.WriteLine(msg);
                    return;
                }

                bool sendSocketMessages;
                if (
                    !bool.TryParse(argHash[InvokeStoredProcManager.SEND_MESSAGES_BACK_ON_SOCKET].ToString(),
                        out sendSocketMessages))
                {
                    var msg = string.Format("could not parse the value 'True' nor 'False' from the '{0}' switch " +
                                            "using the cmd line arg \n{1}",
                        InvokeStoredProcManager.SEND_MESSAGES_BACK_ON_SOCKET, string.Join(" ", args));
                    Settings.WriteToStoredProcLog(msg);
                    Console.WriteLine(msg);
                    return;
                }

                var connectionString = argHash[InvokeStoredProcManager.CONNECTION_STR_SWITCH].ToString();
                var spmFilePath = argHash[InvokeStoredProcManager.FILE_PATH_SWITCH].ToString();
                SortingContainers.StoredProcMetadata mySpm;

                if (!File.Exists(spmFilePath))
                {
                    var msg = string.Format("bad path or file name '{0}'", spmFilePath);
                    Settings.WriteToStoredProcLog(msg);
                    Console.WriteLine(msg);
                    return;
                    
                }

                if (!SortingContainers.StoredProcMetadata.TryDeserializeFromDisk(spmFilePath, out mySpm))
                {
                    var msg = string.Format("could not deserialize the file \n'{0}' into the type \n'{1}'", spmFilePath,
                        typeof (SortingContainers.StoredProcMetadata).FullName);
                    Settings.WriteToStoredProcLog(msg);
                    Console.WriteLine(msg);
                    return;
                }

                if (string.IsNullOrWhiteSpace(mySpm.ProcName))
                {
                    var msg = string.Format("there isn't a stored proc name in the deserialized file at \n'{0}'",
                        spmFilePath);
                    Settings.WriteToStoredProcLog(msg);
                    Console.WriteLine(msg);
                    return;
                }
                
                using (var conn = new SqlConnection(connectionString))
                {
                    Console.WriteLine("{0}", mySpm.ProcName);
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            var ds = new DataSet(mySpm.ProcName);    
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();

                            //can be controlled from app.config
                            cmd.CommandTimeout = ConfigTimeout;

                            //assigns random values to (hopefully) avoid timeouts
                            mySpm.AssignSpParams(cmd.Parameters);
                            cmd.CommandText = mySpm.ProcName;

                            var processMsg = new InvokeStoredProcExeMessage { StoredProcName = mySpm.ProcName };
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

                            var xsdOutput = mySpm.XsdFilePath;
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

                            if (!sendSocketMessages) return;

                            //once its on the disk then send a message to the manager
                            using (var com = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP))
                            {
                                Settings.WriteToStoredProcLog(string.Format("Sent from process '{0}'", processMsg));
                                com.Connect(new IPEndPoint(IPAddress.Loopback, InvokeStoredProcManager.SOCKET_COMM_PORT));
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
                Settings.WriteToStoredProcLog(ex, string.Format("Error while executing NoFuture.Hbm.InvokeStoredProc.exe"));
            }
            Thread.Sleep(20);//slight pause
        }

        public static int ConfigTimeout
        {
            get
            {
                var appSet = ConfigurationManager.AppSettings[AppSettingKeyNames.HbmStoredProcXsdTimeOut];
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

        public static String Help()
        {
            var help = new StringBuilder();
            help.AppendLine(" ----");
            help.AppendLine(string.Format(" [{0}] ", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name));
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
                Shared.Constants.CMD_LINE_ARG_SWITCH, InvokeStoredProcManager.HBM_STORED_PROX_DIR_SWITCH,
                Shared.Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine("                        where the resulting .xsd files are to be ");
            help.AppendLine("                        placed.");
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[STRING]      Must be a valid connection ",
                Shared.Constants.CMD_LINE_ARG_SWITCH, InvokeStoredProcManager.CONNECTION_STR_SWITCH,
                Shared.Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine("                        string to an accessable SqlServer");
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[STRING]   The path to a binary ",
                Shared.Constants.CMD_LINE_ARG_SWITCH, InvokeStoredProcManager.FILE_PATH_SWITCH,
                Shared.Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine(string.Format("                        serialization of \n     '{0}' ",
                typeof (SortingContainers.StoredProcMetadata).FullName));
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[BOOL]      Tells exe to send messages back  ",
                Shared.Constants.CMD_LINE_ARG_SWITCH, InvokeStoredProcManager.SEND_MESSAGES_BACK_ON_SOCKET,
                Shared.Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine(string.Format("                        on the loopback:{0}",InvokeStoredProcManager.SOCKET_COMM_PORT));
            help.AppendLine("");

            return help.ToString();
        }
    }
}
