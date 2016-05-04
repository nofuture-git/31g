using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NoFuture.Exceptions;
using NoFuture.Host.Encryption.Sjcl;
using NoFuture.Util.NfConsole;

namespace NoFuture.Host.Encryption
{
    #region Program's Parameters

    internal struct HostParameters
    {
        internal int SjclBulkKeyToPlainTextPort;
        internal int SjclBulkKeyToCipherTextPort;
        internal int SjclSha256HashPort;

        internal bool IsValid()
        {
            return SjclBulkKeyToCipherTextPort > 0 && SjclBulkKeyToPlainTextPort > 0 && SjclSha256HashPort > 0;
        }
    }
    internal struct FileParameters
    {
        internal string InputFile;
        internal InvokeKind FileCommand;
        internal string CertPath;
        internal string Pwd;

        internal bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(InputFile) || !File.Exists(InputFile) || string.IsNullOrWhiteSpace(CertPath) ||
                !File.Exists(CertPath))
                return false;
            if (FileCommand == InvokeKind.DecryptFile && string.IsNullOrWhiteSpace(Pwd))
                return false;
            return true;
        }
    }
    public static class SWITCHES
    {
        public const string ENCRYPT_FILE = "encrypt";
        public const string DECRYPT_FILE = "decrypt";
        public const string CERT = "cert";
        public const string PWD = "pwd";
        public const string TO_PLAIN_TXT_PORT = "toPlainTextPort";
        public const string TO_CIPHER_TEXT_PORT = "toCipherTextPort";
        public const string HASH_PORT = "hashPort";
        public const string HASH_SALT = "hashSalt";
        public const string BULK_CIPHER_KEY = "bulkCipherKey";
    }
    public static class APPCONFIG
    {
        public const string SALT = "Sha256HashSalt";
        public const string BULK_KEY = "BulkCipherKey";
    }
    public static class STDIN_COMMANDS
    {
        public const string EXIT = "exit";
        public const string STOP = "stop";
        public const string QUIT = "quit";
        public const string ALIVE = "alive";
    }

    internal enum InvokeKind
    {
        HostSjcl,
        EncryptFile,
        DecryptFile
    }

    #endregion

    /// <summary>
    /// Performs a variety of encryption functions in an isolated exe
    /// <remarks>
    /// Regarding hosting <see cref="NoFuture.Encryption.Sjcl"/>, the idea 
    /// is that loading  the 'sjcl.js' into the runtime for every call is too much 
    /// of a performance hit.  This console app loads it once 
    /// and then opens sockets to perform the actual work - sending the
    /// data across the wire and receiveing it back again.
    /// </remarks>
    /// </summary>
    public class Program
    {
        #region Constants

        public const int BRING_ONLINE_DEFAULT_WAIT_MS = 1000;
        public const int LISTEN_NUM_REQUEST = 5;

        #endregion

        #region Fields

        private static TaskFactory _taskFactory;

        private static List<Task> _tasks;
        private static readonly Object LOCK = new object();
        private static readonly Object SECOND_LOCK = new object();
        private static Hashtable _sendReceiveBytes;
        private static string _bulkKey;
        private static string _hashSalt;
        private static bool _alive;

        #endregion

        #region Program's Main

        public static void Main(string[] args)
        {
            try
            {
                //check for some args passed in
                if (args == null || args.Length == 0 || args[0] == "-h" || args[0] == "-help")
                {
                    //no args, print help, exit
                    Console.Out.WriteLine(Help());
                    return;
                }

                //parse command line args
                var argHash = ConsoleCmd.ArgHash(args);

                //either perform single file op
                var cmlF = GetFileCmdArgs(argHash);
                if (cmlF.FileCommand != InvokeKind.HostSjcl)
                {
                    PerformFileCipher(cmlF);
                    return;
                }

                //or host SJCL.js
                HostSjcl(argHash);
            }
            catch (Exception ex)
            {
                Print("CRITICAL ERROR");
                Print(ex);
                Print("press any key to exit...");
                var noop = Console.ReadKey();
            }
        }

        #endregion

        #region Program's Internals
        internal static void HostSjcl(Hashtable argHash)
        {
            ConsoleCmd.SetConsoleAsTransparent(true);

            var cmdL = GetHostSjclCmdArgs(argHash);

            //at least one port value must be present
            if (!cmdL.IsValid())
            {
                Console.Out.WriteLine("Arg list is invalid.");
                return;
            }

            //all keys will have value regardless of what is listening
            if (AssignKeys())
            {
                Console.Out.WriteLine("the app.config is invalid.");
                return;
            }

            //start up js interpreter
            WarmUp();

            //print to user that something is happening
            PrintSettingsToConsole(cmdL);

            _sendReceiveBytes = new Hashtable();

            Thread.CurrentThread.Name = "Main";

            //launch a socket listening on any ports specified
            LaunchListeners(cmdL);

            //park main thread to handle user commands
            for (; ; ) //ever
            {
                _alive = true;
                var userText = Console.ReadLine(); //main thread parks here
                try
                {
                    HandleStdInText(userText);
                }
                catch (Exception ex)
                {
                    Print(ex);
                }
            }
        }

        internal static void PerformFileCipher(FileParameters p)
        {
            if (!p.IsValid())
            {
                Console.Out.WriteLine("Arg list is invalid.");
                return;
            }

            if (p.FileCommand == InvokeKind.DecryptFile)
            {
                NoFuture.Encryption.NfX509.DecryptFile(p.InputFile, p.CertPath, p.Pwd);
                return;
            }
            NoFuture.Encryption.NfX509.EncryptFile(p.InputFile, p.CertPath);

        }

        //launch a socket listener on each specified port
        internal static void LaunchListeners(HostParameters cmdL)
        {
            _taskFactory = new TaskFactory();
            _tasks = new List<Task>();


            if (cmdL.SjclBulkKeyToPlainTextPort != 0)
            {
                _tasks.Add(_taskFactory.StartNew(
                                                   () =>
                                                   HostSjcl(new BkToPlainTextCommand(_bulkKey),
                                                            cmdL.SjclBulkKeyToPlainTextPort,
                                                            "BkToPlainTextCommand")));
                Thread.Sleep(BRING_ONLINE_DEFAULT_WAIT_MS);
                Print("Sjcl Bulk Key To Plain Text is online.");

            }

            if (cmdL.SjclBulkKeyToCipherTextPort != 0)
            {
                _tasks.Add(_taskFactory.StartNew(
                                                   () =>
                                                   HostSjcl(new BkToCipherTextCommand(_bulkKey),
                                                            cmdL.SjclBulkKeyToCipherTextPort,
                                                            "BkToCipherTextCommand")));
                Thread.Sleep(BRING_ONLINE_DEFAULT_WAIT_MS);
                Print("Sjcl Bulk Key To Cipher Text is online.");

            }

            if (cmdL.SjclSha256HashPort != 0)
            {
                _tasks.Add(_taskFactory.StartNew(
                                                   () =>
                                                   HostSjcl(new Sha256HashCommand(_hashSalt),
                                                            cmdL.SjclSha256HashPort,
                                                            "Sha256HashCommand")));
                Thread.Sleep(BRING_ONLINE_DEFAULT_WAIT_MS);
                Print("Sjcl Sha256 Hash is online.");

            }
        }

        //handle user entered text from the console app
        internal static void HandleStdInText(string text)
        {
            switch (text)
            {
                case STDIN_COMMANDS.ALIVE:
                    if(_alive)
                    {
                        foreach(string threadname in _sendReceiveBytes.Keys)
                        {
                            var totals = (int[])_sendReceiveBytes[threadname];
                            Print(string.Format("{0} received {1} bytes, sent {2} bytes", threadname,totals[0],totals[1]));
                        }
                    }
                    break;
                case STDIN_COMMANDS.EXIT:
                case STDIN_COMMANDS.QUIT:
                case STDIN_COMMANDS.STOP:
                    Environment.Exit(0);
                    break;
                default: //default echo
                    Console.WriteLine(text);
                    break;
            }
        }

        //assign instance keys to cmd arg or app.config value
        internal static bool AssignKeys()
        {
            if(String.IsNullOrWhiteSpace(_bulkKey))
                _bulkKey = ConfigurationManager.AppSettings[APPCONFIG.BULK_KEY];

            if(String.IsNullOrWhiteSpace(_hashSalt))
                _hashSalt = ConfigurationManager.AppSettings[APPCONFIG.SALT];

            return String.IsNullOrWhiteSpace(_bulkKey) || String.IsNullOrWhiteSpace(_hashSalt);
        }

        //at startup print resolved settings
        internal static void PrintSettingsToConsole(HostParameters cmdL)
        {
            if (cmdL.SjclBulkKeyToPlainTextPort != 0)
                Print(string.Format("Bulk Key To Plain Text on port '{0}'", cmdL.SjclBulkKeyToPlainTextPort));
            if (cmdL.SjclBulkKeyToCipherTextPort != 0)
                Print(string.Format("Bulk Key To Cipher Text on port '{0}'", cmdL.SjclBulkKeyToCipherTextPort));
            if (cmdL.SjclSha256HashPort != 0)
                Print(string.Format("Hash on port '{0}'", cmdL.SjclSha256HashPort));

            Print("----");

            Print(string.Format("Bulk Key '{0}'", _bulkKey));
            Print(string.Format("Hash Salt '{0}'", _hashSalt));
            Print("----");
        }

        internal static string Help()
        {
            var help = new StringBuilder();
            help.AppendLine("Usage:  [File Operations | sjcl.js Host Operations ]");

            help.AppendLine("");
            help.AppendLine(" -h | -help             Will print this help.");
            help.AppendLine("");

            help.AppendLine("File Operations:");
            help.AppendLine("----------------");
            help.AppendLine("Description : performs encryption or decryption on a single file using ");
            help.AppendLine("              the X509 cert.");

            help.AppendLine(string.Format(" -{0}=[PATH]", SWITCHES.ENCRYPT_FILE));
            help.AppendLine("                        Path to file to be encrypted.");
            help.AppendLine("");

            help.AppendLine(string.Format(" -{0}=[PATH]", SWITCHES.DECRYPT_FILE));
            help.AppendLine("                        Path to file to be decrypted.");
            help.AppendLine("");

            help.AppendLine(string.Format(" -{0}=[PATH]", SWITCHES.CERT));
            help.AppendLine("                        Path to X509 cert for encrypt or decrypt.");
            help.AppendLine("");

            help.AppendLine(string.Format(" -{0}=[STRING]", SWITCHES.PWD));
            help.AppendLine("                        Password used for decryption only");
            help.AppendLine("");
            help.AppendLine("");

            help.AppendLine("sjcl.js Host Operations:");
            help.AppendLine("------------------------");
            help.AppendLine("Description : hosts NoFuture.Encryption.Sjcl on TCP\\IP sockets, one for ");
            help.AppendLine("              each function.  All three ports must be specified.");


            help.AppendLine(string.Format(" -{0}=[INT32]", SWITCHES.TO_PLAIN_TXT_PORT));
            help.AppendLine("                        Port for sjcl.js encryption");
            help.AppendLine("");

            help.AppendLine(string.Format(" -{0}=[INT32]", SWITCHES.TO_CIPHER_TEXT_PORT));
            help.AppendLine("                        Port for sjcl.js decryption");
            help.AppendLine("");

            help.AppendLine(string.Format(" -{0}=[INT32]", SWITCHES.HASH_PORT));
            help.AppendLine("                        Port for sjcl.js Sha256 Hash.");
            help.AppendLine("");

            help.AppendLine(string.Format(" -{0}=[STRING]", SWITCHES.BULK_CIPHER_KEY));
            help.AppendLine("                        Optional, starts listener using specified key");
            help.AppendLine(string.Format("                        will default to App.Config '{0}' value.", APPCONFIG.BULK_KEY));
            help.AppendLine("");

            help.AppendLine(string.Format(" -{0}=[STRING]", SWITCHES.HASH_PORT));
            help.AppendLine("                        Optional, starts Sha256 Hash with value as the salt.");
            help.AppendLine(string.Format("                        will default to App.Config '{0}' value.", APPCONFIG.SALT));
            help.AppendLine("");

            return help.ToString();
        }

        internal static FileParameters GetFileCmdArgs(Hashtable argHash)
        {
            var p = new FileParameters();
            if (argHash.ContainsKey(SWITCHES.CERT))
                p.CertPath = argHash[SWITCHES.CERT].ToString();

            if (argHash.ContainsKey(SWITCHES.ENCRYPT_FILE))
            {
                p.InputFile = argHash[SWITCHES.ENCRYPT_FILE].ToString();
                p.FileCommand = InvokeKind.EncryptFile;
            }
            if (argHash.ContainsKey(SWITCHES.DECRYPT_FILE) && p.FileCommand != InvokeKind.EncryptFile)
            {
                p.InputFile = argHash[SWITCHES.DECRYPT_FILE].ToString();
                p.FileCommand = InvokeKind.DecryptFile;
            }
            if (argHash.ContainsKey(SWITCHES.PWD))
                p.Pwd = argHash[SWITCHES.PWD].ToString();

            return p;
        }

        internal static HostParameters GetHostSjclCmdArgs(Hashtable argHash)
        {
            var sjclBkPt = 0;
            var sjclBkCt = 0;
            var sjclHp = 0;
            var sjclHashSalt = string.Empty;
            var sjclBulkCipherKey = string.Empty;

            if (argHash.ContainsKey(SWITCHES.TO_PLAIN_TXT_PORT))
                Int32.TryParse(argHash[SWITCHES.TO_PLAIN_TXT_PORT].ToString(), out sjclBkPt);

            if (argHash.ContainsKey(SWITCHES.TO_CIPHER_TEXT_PORT))
                Int32.TryParse(argHash[SWITCHES.TO_CIPHER_TEXT_PORT].ToString(), out sjclBkCt);

            if (argHash.ContainsKey(SWITCHES.HASH_PORT))
                Int32.TryParse(argHash[SWITCHES.HASH_PORT].ToString(), out sjclHp);

            if (argHash.ContainsKey(SWITCHES.HASH_SALT))
                sjclHashSalt = argHash[SWITCHES.HASH_SALT].ToString();

            if (argHash.ContainsKey(SWITCHES.BULK_CIPHER_KEY))
                sjclBulkCipherKey = argHash[SWITCHES.BULK_CIPHER_KEY].ToString();

            var rtrn = new HostParameters
                           {
                               SjclBulkKeyToPlainTextPort = sjclBkPt,
                               SjclBulkKeyToCipherTextPort = sjclBkCt,
                               SjclSha256HashPort = sjclHp,
                           };

            _hashSalt = CleanupDblSnglQuotes(sjclHashSalt);
            _bulkKey = CleanupDblSnglQuotes(sjclBulkCipherKey);

            return rtrn;
        }

        internal static string CleanupDblSnglQuotes(string s)
        {
            if (String.IsNullOrWhiteSpace(s)) return s;

            if (s.StartsWith("\"") && s.EndsWith("\""))
                s = s.Substring(1, (s.Length - 2));

            if (s.StartsWith("'") && s.EndsWith("'"))
                s = s.Substring(1, (s.Length - 2));
            return s;
        }

        //starts slow & expensive js interpreter
        internal static void WarmUp()
        {
            var noop00 = NoFuture.Encryption.Sjcl.Resources.ScriptEngine;
            var noop01 = NoFuture.Encryption.Sjcl.Resources.CipherTextSerializer;
            var noop02 = NoFuture.Encryption.Sjcl.Resources.SjclJs;
        }

        //sync locked console print
        internal static void Print(string msg)
        {
            lock(LOCK)
            {
                try
                {
                    var printMsg = string.Format("{0:yyyyMMdd HH:mm:ss.fffff} - {1}\n", DateTime.Now, msg);
                    Console.Write(printMsg);
                }
                catch (Exception ex)
                {

                    var printEx = string.Format("Logging exception :: {0}\n{1}\n", ex.Message, ex.StackTrace);
                    Console.Write(printEx);
                    if (ex.InnerException != null)
                    {
                        var printInnerEx = string.Format("Inner exception :: {0}\n{1}\n",
                                                         ex.InnerException.Message,
                                                         ex.InnerException.StackTrace);

                        Console.Write(printInnerEx);
                    }
                }                
            }
        }

        internal static void Print(Exception ex)
        {
            if (ex == null)
                return;
            Print(String.Format("Logging exception :: {0}\n{1}", ex.Message, ex.StackTrace));
            if (ex.InnerException != null)
            {
                Print(String.Format("Inner exception :: {0}\n{1}", ex.InnerException.Message, ex.InnerException.StackTrace));
            }
            
        }

        //sync locked totals counters
        internal static void AddToTotals(string threadName, int bytesSent, int bytesReceived)
        {
            if (string.IsNullOrWhiteSpace(threadName))
                return;

            lock (SECOND_LOCK)
            {
                if (!_sendReceiveBytes.ContainsKey(threadName))
                    return;
                ((int[]) _sendReceiveBytes[threadName])[0] += bytesSent;
                ((int[]) _sendReceiveBytes[threadName])[1] += bytesReceived;
            }
        }

        internal static void HostSjcl(ICommand cmd, int cmdPort, string name)
        {
            Thread.CurrentThread.Name = name;
            if(!_sendReceiveBytes.ContainsKey(name))
                _sendReceiveBytes.Add(name,new[]{0,0});

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //this should NOT be reachable from any other machine
            var endPt = new IPEndPoint(IPAddress.Loopback, cmdPort);
            socket.Bind(endPt);
            socket.Listen(LISTEN_NUM_REQUEST);

            for(;;)//ever
            {
                try
                {
                    var buffer = new List<byte>();

                    var client = socket.Accept();

                    //park for first data received
                    var data = new byte[Shared.Constants.DEFAULT_BLOCK_SIZE];
                    client.Receive(data, 0, data.Length, SocketFlags.None);
                    buffer.AddRange(data.Where(b => b != (byte)'\0'));

                    while(client.Available > 0)
                    {
                        data = new byte[client.Available];
                        client.Receive(data, 0, data.Length, SocketFlags.None);
                        buffer.AddRange(data.ToArray());
                    }

                    var output = cmd.Execute(buffer.ToArray());
                    client.Send(output);
                    client.Close();

                    Print(string.Format("THREAD: '{0}', receive: {1} bytes, send: {2} bytes", Thread.CurrentThread.Name, buffer.Count, output.Length));

                    //contains lock
                    AddToTotals(Thread.CurrentThread.Name, buffer.Count, output.Length);

                }
                catch (Exception ex)
                {
                    Print(String.Format("THREAD: '{0}'", Thread.CurrentThread.Name));
                    Print(ex);
                }
            }
        }

        #endregion
    }
}
