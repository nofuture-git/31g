using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cfg = System.Configuration.ConfigurationManager;
using NoFuture.Host.Encryption.Sjcl;
using NoFuture.Tools;
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
            return Util.Net.IsValidPortNumber(SjclBulkKeyToCipherTextPort) 
                && Util.Net.IsValidPortNumber(SjclBulkKeyToPlainTextPort) 
                && Util.Net.IsValidPortNumber(SjclSha256HashPort);
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
    public class Program : SocketConsole
    {
        #region constants
        public const int BRING_ONLINE_DEFAULT_WAIT_MS = 1000;
        #endregion

        #region fields
        private TaskFactory _taskFactory;
        private List<Task> _tasks;
        private readonly Object _lock = new object();
        private Hashtable _sendReceiveBytes;
        private string _bulkKey;
        private string _hashSalt;
        private bool _alive;
        private FileParameters MyFileParameters { get; set; }
        private HostParameters MyHostParameters { get; set; }
        #endregion

        #region ctors
        public Program(string[] args) : base(args, true)
        {
        }
        #endregion

        #region properties
        protected override string MyName => "NoFuture.Host.Encryption";
        #endregion

        public static void Main(string[] args)
        {
            var p = new Program(args);
            try
            {
                p.StartConsole();

                if (p.PrintHelp())
                    return;

                p.ParseProgramArgs();

                //either perform single file op
                if (p.MyFileParameters.FileCommand != InvokeKind.HostSjcl)
                {
                    p.PerformFileCipher();
                    return;
                }

                //or host SJCL.js
                p.HostSjcl();
            }
            catch (Exception ex)
            {
                p.PrintToConsole("CRITICAL ERROR");
                p.PrintToConsole(ex);
            }
            p.PrintToConsole("press any key to exit...");
            var noop = Console.ReadKey();
        }

        #region methods

        internal void HostSjcl()
        {
            //at least one port value must be present
            if (!MyHostParameters.IsValid())
            {
                PrintToConsole("Arg list is invalid.");
                return;
            }

            //all keys will have value regardless of what is listening
            if (AssignKeys())
            {
                PrintToConsole("the app.config is invalid.");
                return;
            }

            //start up js interpreter
            WarmUp();

            //print to user that something is happening
            PrintSettingsToConsole();

            _sendReceiveBytes = new Hashtable();

            Thread.CurrentThread.Name = "Main";

            //launch a socket listening on any ports specified
            LaunchListeners();

            //park main thread to handle user commands
            for (;;) //ever
            {
                _alive = true;
                var userText = Console.ReadLine(); //main thread parks here
                try
                {
                    HandleStdInText(userText);
                }
                catch (Exception ex)
                {
                    PrintToConsole(ex);
                }
            }
        }

        internal void PerformFileCipher()
        {
            if (!MyFileParameters.IsValid())
            {
                PrintToConsole("Arg list is invalid.");
                return;
            }

            if (MyFileParameters.FileCommand == InvokeKind.DecryptFile)
            {
                NoFuture.Encryption.NfX509.DecryptFile(MyFileParameters.InputFile, MyFileParameters.CertPath,
                    MyFileParameters.Pwd);
                return;
            }
            NoFuture.Encryption.NfX509.EncryptFile(MyFileParameters.InputFile, MyFileParameters.CertPath);

        }

        //launch a socket listener on each specified port
        protected override void LaunchListeners()
        {

            _taskFactory = new TaskFactory();
            _tasks = new List<Task>();

            if (Util.Net.IsValidPortNumber(MyHostParameters.SjclBulkKeyToPlainTextPort))
            {
                _tasks.Add(_taskFactory.StartNew(
                    () =>
                        HostCmd(new BkToPlainTextCommand(_bulkKey),
                            MyHostParameters.SjclBulkKeyToPlainTextPort)));
                Thread.Sleep(BRING_ONLINE_DEFAULT_WAIT_MS);
                PrintToConsole("Sjcl Bulk Key To Plain Text is online.");

            }

            if (Util.Net.IsValidPortNumber(MyHostParameters.SjclBulkKeyToCipherTextPort))
            {
                _tasks.Add(_taskFactory.StartNew(
                    () =>
                        HostCmd(new BkToCipherTextCommand(_bulkKey),
                            MyHostParameters.SjclBulkKeyToCipherTextPort)));
                Thread.Sleep(BRING_ONLINE_DEFAULT_WAIT_MS);
                PrintToConsole("Sjcl Bulk Key To Cipher Text is online.");

            }

            if (Util.Net.IsValidPortNumber(MyHostParameters.SjclSha256HashPort))
            {
                _tasks.Add(_taskFactory.StartNew(
                    () =>
                        HostCmd(new Sha256HashCommand(_hashSalt),
                            MyHostParameters.SjclSha256HashPort)));
                Thread.Sleep(BRING_ONLINE_DEFAULT_WAIT_MS);
                PrintToConsole("Sjcl Sha256 Hash is online.");

            }
        }

        //handle user entered text from the console app
        internal void HandleStdInText(string text)
        {
            switch (text)
            {
                case STDIN_COMMANDS.ALIVE:
                    if (_alive)
                    {
                        foreach (string threadname in _sendReceiveBytes.Keys)
                        {
                            var totals = (int[]) _sendReceiveBytes[threadname];
                            PrintToConsole($"{threadname} received {totals[0]} bytes, sent {totals[1]} bytes");
                        }
                    }
                    break;
                case STDIN_COMMANDS.EXIT:
                case STDIN_COMMANDS.QUIT:
                case STDIN_COMMANDS.STOP:
                    Environment.Exit(0);
                    break;
                default: //default echo
                    PrintToConsole(text);
                    break;
            }
        }

        //assign instance keys to cmd arg or app.config value
        internal bool AssignKeys()
        {
            if (String.IsNullOrWhiteSpace(_bulkKey))
                _bulkKey = ConfigurationManager.AppSettings[APPCONFIG.BULK_KEY];

            if (String.IsNullOrWhiteSpace(_hashSalt))
                _hashSalt = ConfigurationManager.AppSettings[APPCONFIG.SALT];

            return String.IsNullOrWhiteSpace(_bulkKey) || String.IsNullOrWhiteSpace(_hashSalt);
        }

        //at startup print resolved settings
        internal void PrintSettingsToConsole()
        {
            if (MyHostParameters.SjclBulkKeyToPlainTextPort != 0)
                PrintToConsole($"Bulk Key To Plain Text on port '{MyHostParameters.SjclBulkKeyToPlainTextPort}'");
            if (MyHostParameters.SjclBulkKeyToCipherTextPort != 0)
                PrintToConsole($"Bulk Key To Cipher Text on port '{MyHostParameters.SjclBulkKeyToCipherTextPort}'");
            if (MyHostParameters.SjclSha256HashPort != 0)
                PrintToConsole($"Hash on port '{MyHostParameters.SjclSha256HashPort}'");

            PrintToConsole("----");

            PrintToConsole($"Bulk Key '{_bulkKey}'");
            PrintToConsole($"Hash Salt '{_hashSalt}'");
            PrintToConsole("----");
        }

        protected override string GetHelpText()
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
            help.AppendLine(string.Format("                        will default to App.Config '{0}' value.",
                APPCONFIG.BULK_KEY));
            help.AppendLine("");

            help.AppendLine(string.Format(" -{0}=[STRING]", SWITCHES.HASH_PORT));
            help.AppendLine("                        Optional, starts Sha256 Hash with value as the salt.");
            help.AppendLine(string.Format("                        will default to App.Config '{0}' value.",
                APPCONFIG.SALT));
            help.AppendLine("");

            return help.ToString();
        }

        protected override void ParseProgramArgs()
        {
            var argHash = ConsoleCmd.ArgHash(_args);
            GetFileCmdArgs(argHash);

            CustomTools.InvokeNfTypeName = ConfigurationManager.AppSettings["NoFuture.ToolsCustomTools.InvokeNfTypeName"];

            if (MyFileParameters.FileCommand == InvokeKind.HostSjcl)
                return;

            GetHostSjclCmdArgs(argHash);
        }

        internal void GetFileCmdArgs(Hashtable argHash)
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
            MyFileParameters = p;
        }

        internal void GetHostSjclCmdArgs(Hashtable argHash)
        {
            var sjclBkPt = 0;
            var sjclBkCt = 0;
            var sjclHp = 0;
            var sjclHashSalt = string.Empty;
            var sjclBulkCipherKey = string.Empty;

            var ptp = argHash.ContainsKey(SWITCHES.TO_PLAIN_TXT_PORT)
                ? argHash[SWITCHES.TO_PLAIN_TXT_PORT].ToString()
                : Cfg.AppSettings[SWITCHES.TO_PLAIN_TXT_PORT];
            var ctp = argHash.ContainsKey(SWITCHES.TO_CIPHER_TEXT_PORT)
                ? argHash[SWITCHES.TO_CIPHER_TEXT_PORT].ToString()
                : Cfg.AppSettings[SWITCHES.TO_CIPHER_TEXT_PORT];
            var hp = argHash.ContainsKey(SWITCHES.HASH_PORT)
                ? argHash[SWITCHES.HASH_PORT].ToString()
                : Cfg.AppSettings[SWITCHES.HASH_PORT];

            sjclBkPt = ResolvePort(ptp).GetValueOrDefault(Tools.NfDefaultPorts.SJCL_TO_PLAIN_TXT);
            sjclBkCt = ResolvePort(ctp).GetValueOrDefault(Tools.NfDefaultPorts.SJCL_TO_CIPHER_TXT);
            sjclHp = ResolvePort(hp).GetValueOrDefault(Tools.NfDefaultPorts.SJCL_HASH_PORT);

            if (argHash.ContainsKey(SWITCHES.HASH_SALT))
                sjclHashSalt = argHash[SWITCHES.HASH_SALT].ToString();

            if (argHash.ContainsKey(SWITCHES.BULK_CIPHER_KEY))
                sjclBulkCipherKey = argHash[SWITCHES.BULK_CIPHER_KEY].ToString();

            MyHostParameters = new HostParameters
            {
                SjclBulkKeyToPlainTextPort = sjclBkPt,
                SjclBulkKeyToCipherTextPort = sjclBkCt,
                SjclSha256HashPort = sjclHp,
            };

            _hashSalt = CleanupDblSnglQuotes(sjclHashSalt);
            _bulkKey = CleanupDblSnglQuotes(sjclBulkCipherKey);
        }

        internal string CleanupDblSnglQuotes(string s)
        {
            if (String.IsNullOrWhiteSpace(s)) return s;

            if (s.StartsWith("\"") && s.EndsWith("\""))
                s = s.Substring(1, (s.Length - 2));

            if (s.StartsWith("'") && s.EndsWith("'"))
                s = s.Substring(1, (s.Length - 2));
            return s;
        }

        //starts slow & expensive js interpreter
        internal void WarmUp()
        {
            var noop00 = NoFuture.Encryption.Sjcl.Resources.ScriptEngine;
            var noop01 = NoFuture.Encryption.Sjcl.Resources.CipherTextSerializer;
            var noop02 = NoFuture.Encryption.Sjcl.Resources.SjclJs;
        }

        #endregion

    }
}
