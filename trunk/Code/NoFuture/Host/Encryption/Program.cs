using System;
using System.Collections;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NoFuture.Host.Encryption.Sjcl;

namespace NoFuture.Host.Encryption
{
    #region Program's Parameters

    internal struct Parameters
    {
        public int SjclPublicKeyToPlainTextPort;
        public int SjclPublicKeyToCipherTextPort;
        public int SjclBulkKeyToPlainTextPort;
        public int SjclBulkKeyToCipherTextPort;
        public int SjclSha256HashPort;
        public bool Valid;
    }
    public static class SWITCHES
    {
        public const string SJCL_PK_PT_PORT = "sjclPkPtPort";
        public const string SJCL_PK_CT_PORT = "sjclPkCtPort";
        public const string SJCL_BK_PT_PORT = "sjclBkPtPort";
        public const string SJCL_BK_CT_PORT = "sjclBkCtPort";
        public const string SJCL_HP_PORT = "sjclHpPort";
        public const string HASH_SALT = "hashSalt";
        public const string BULK_CIPHER_KEY = "bulkCipherKey";
    }
    public static class APPCONFIG
    {
        public const string SALT = "Sha256HashSalt";
        public const string PUBLIC_KEY_PUBLIC = "PublicKeyPublicKey";
        public const string PUBLIC_KEY_PRIVATE = "PublicKeyPrivateKey";
        public const string BULK_KEY = "BulkCipherKey";
    }
    public static class STDIN_COMMANDS
    {
        public const string EXIT = "exit";
        public const string STOP = "stop";
        public const string QUIT = "quit";
        public const string ALIVE = "alive";
    }

    #endregion

    /// <summary>
    /// Creates a running console app which opens and listens on 
    /// specified sockets to perform the encryption \ decryption
    /// of 'sjcl.js' cipher text.
    /// <remarks>
    /// The idea is that loading 
    /// the 'sjcl.js' into the runtime for every call is too much 
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
        private static string _privKey;
        private static string _pubKey;
        private static string _bulkKey;
        private static string _hashSalt;
        private static bool _alive;

        #endregion

        #region Program's Main

        public static void Main(string[] args)
        {
            try
            {
                Util.ConsoleCmd.SetConsoleAsTransparent();
                //check for some args passed in
                if (args == null || args.Length == 0 || args[0] == "-h" || args[0] == "-help")
                {
                    //no args, print help, exit
                    Console.Out.WriteLine(Help());
                    return;
                }

                //parse command line args
                var argHash = Util.ConsoleCmd.ArgHash(args);

                var cmdL = GetCmdLineArgs(argHash);

                //at least one port value must be present
                if (!cmdL.Valid)
                {
                    Console.Out.WriteLine("Arg list is invalid.");
                    return;
                }

                //all keys will have value regardless of what is listening
                if(AssignKeys())
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
                for (; ; )//ever
                {
                    _alive = true;
                    var userText = Console.ReadLine();//main thread parks here
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

        //launch a socket listener on each specified port
        internal static void LaunchListeners(Parameters cmdL)
        {
            _taskFactory = new TaskFactory();
            _tasks = new List<Task>();

            if (cmdL.SjclPublicKeyToPlainTextPort != 0)
            {
                _tasks.Add(_taskFactory.StartNew(
                                                   () =>
                                                   HostSjcl(new PkToPlainTextCommand(_privKey),
                                                            cmdL.SjclPublicKeyToPlainTextPort,
                                                            "PkToPlainTextCommand")));
                Thread.Sleep(BRING_ONLINE_DEFAULT_WAIT_MS);
                Print("Sjcl Public Key To Plain Text is online.");

            }

            if (cmdL.SjclPublicKeyToCipherTextPort != 0)
            {
                _tasks.Add(_taskFactory.StartNew(
                                                   () =>
                                                   HostSjcl(new PkToCipherTextCommand(_pubKey),
                                                            cmdL.SjclPublicKeyToCipherTextPort,
                                                            "PkToCipherTextCommand")));
                Thread.Sleep(BRING_ONLINE_DEFAULT_WAIT_MS);
                Print("Sjcl Public Key To Cipher Text is online.");

            }

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
                    Console.WriteLine("Please close the console window from the desktop.");
                    break;
                default: //default echo
                    Console.WriteLine(text);
                    break;
            }
        }

        //assign instance keys to cmd arg or app.config value
        internal static bool AssignKeys()
        {
            _privKey = ConfigurationManager.AppSettings[APPCONFIG.PUBLIC_KEY_PUBLIC];

            _pubKey = ConfigurationManager.AppSettings[APPCONFIG.PUBLIC_KEY_PRIVATE];

            if(String.IsNullOrWhiteSpace(_bulkKey))
                _bulkKey = ConfigurationManager.AppSettings[APPCONFIG.BULK_KEY];

            if(String.IsNullOrWhiteSpace(_hashSalt))
                _hashSalt = ConfigurationManager.AppSettings[APPCONFIG.SALT];

            return String.IsNullOrWhiteSpace(_privKey) || String.IsNullOrWhiteSpace(_bulkKey) ||
                   String.IsNullOrWhiteSpace(_hashSalt) || String.IsNullOrWhiteSpace(_pubKey);
        }

        //at startup print resolved settings
        internal static void PrintSettingsToConsole(Parameters cmdL)
        {
            if (cmdL.SjclPublicKeyToPlainTextPort != 0)
                Print(string.Format("Public Key To Plain Text on port '{0}'", cmdL.SjclPublicKeyToPlainTextPort));
            if (cmdL.SjclPublicKeyToCipherTextPort != 0)
                Print(string.Format("Public Key To Cipher Text on port '{0}'", cmdL.SjclPublicKeyToCipherTextPort));
            if (cmdL.SjclBulkKeyToPlainTextPort != 0)
                Print(string.Format("Bulk Key To Plain Text on port '{0}'", cmdL.SjclBulkKeyToPlainTextPort));
            if (cmdL.SjclBulkKeyToCipherTextPort != 0)
                Print(string.Format("Bulk Key To Cipher Text on port '{0}'", cmdL.SjclBulkKeyToCipherTextPort));
            if (cmdL.SjclSha256HashPort != 0)
                Print(string.Format("Hash on port '{0}'", cmdL.SjclSha256HashPort));

            Print("----");

            Print(string.Format("Public Key '{0}'", _pubKey));
            Print(string.Format("Private Key '{0}'", _privKey));
            Print(string.Format("Bulk Key '{0}'", _bulkKey));
            Print(string.Format("Hash Salt '{0}'", _hashSalt));
            Print("----");
        }

        internal static string Help()
        {
            var help = new StringBuilder();
            help.AppendLine("Usage:  [options]");
            help.AppendLine("Description : hosts NoFuture.Encryption library with an open ports, one for ");
            help.AppendLine("              each function.  At least one port must be specified.");

            help.AppendLine("");
            help.AppendLine("Options:");
            help.AppendLine(" -h | -help             Will print this help.");
            help.AppendLine("");

            help.AppendLine(string.Format(" -{0}=[INT32]", SWITCHES.SJCL_PK_PT_PORT));
            help.AppendLine("                        Optional, open port for sjcl.js Public Key Plain Text .");
            help.AppendLine("");

            help.AppendLine(string.Format(" -{0}=[INT32]", SWITCHES.SJCL_PK_CT_PORT));
            help.AppendLine("                        Optional, open port for sjcl.js Public Key Cipher Text .");
            help.AppendLine("");

            help.AppendLine(string.Format(" -{0}=[INT32]", SWITCHES.SJCL_BK_PT_PORT));
            help.AppendLine("                        Optional, open port for sjcl.js Bulk Key Plain Text .");
            help.AppendLine("");

            help.AppendLine(string.Format(" -{0}=[INT32]", SWITCHES.SJCL_BK_CT_PORT));
            help.AppendLine("                        Optional, open port for sjcl.js Bulk Key Cipher Text .");
            help.AppendLine("");

            help.AppendLine(string.Format(" -{0}=[INT32]", SWITCHES.SJCL_HP_PORT));
            help.AppendLine("                        Optional, open port for sjcl.js Sha256 Hash.");
            help.AppendLine("");

            help.AppendLine(string.Format(" -{0}=[STRING]", SWITCHES.BULK_CIPHER_KEY));
            help.AppendLine("                        Optional, starts listener using specified key");
            help.AppendLine(string.Format("                        will default to App.Config '{0}' value.", APPCONFIG.BULK_KEY));
            help.AppendLine("");

            help.AppendLine(string.Format(" -{0}=[STRING]", SWITCHES.SJCL_HP_PORT));
            help.AppendLine("                        Optional, starts Sha256 Hash with specified value as the salt.");
            help.AppendLine(string.Format("                        will default to App.Config '{0}' value.", APPCONFIG.SALT));
            help.AppendLine("");

            return help.ToString();
        }

        //turn the command line args into strong-typed values thereof
        internal static Parameters GetCmdLineArgs(Hashtable argHash)
        {
            var sjclPkPt = 0;
            var sjclPkCt = 0;
            var sjclBkPt = 0;
            var sjclBkCt = 0;
            var sjclHp = 0;
            var sjclHashSalt = string.Empty;
            var sjclBulkCipherKey = string.Empty;

            if (argHash.ContainsKey(SWITCHES.SJCL_PK_PT_PORT))
                Int32.TryParse(argHash[SWITCHES.SJCL_PK_PT_PORT].ToString(), out sjclPkPt);

            if (argHash.ContainsKey(SWITCHES.SJCL_PK_CT_PORT))
                Int32.TryParse(argHash[SWITCHES.SJCL_PK_CT_PORT].ToString(), out sjclPkCt);

            if (argHash.ContainsKey(SWITCHES.SJCL_BK_PT_PORT))
                Int32.TryParse(argHash[SWITCHES.SJCL_BK_PT_PORT].ToString(), out sjclBkPt);

            if (argHash.ContainsKey(SWITCHES.SJCL_BK_CT_PORT))
                Int32.TryParse(argHash[SWITCHES.SJCL_BK_CT_PORT].ToString(), out sjclBkCt);

            if (argHash.ContainsKey(SWITCHES.SJCL_HP_PORT))
                Int32.TryParse(argHash[SWITCHES.SJCL_HP_PORT].ToString(), out sjclHp);

            if (argHash.ContainsKey(SWITCHES.HASH_SALT))
                sjclHashSalt = argHash[SWITCHES.HASH_SALT].ToString();

            if (argHash.ContainsKey(SWITCHES.BULK_CIPHER_KEY))
                sjclBulkCipherKey = argHash[SWITCHES.BULK_CIPHER_KEY].ToString();

            var allPortsAreZero = sjclPkPt == 0 && sjclPkCt == 0 && sjclBkPt == 0 && sjclBkCt == 0 && sjclHp == 0;

            var rtrn = new Parameters
                           {
                               SjclPublicKeyToPlainTextPort = sjclPkPt,
                               SjclPublicKeyToCipherTextPort = sjclPkCt,
                               SjclBulkKeyToPlainTextPort = sjclBkPt,
                               SjclBulkKeyToCipherTextPort = sjclBkCt,
                               SjclSha256HashPort = sjclHp,
                               Valid = !allPortsAreZero
                           };

            if (!String.IsNullOrWhiteSpace(sjclHashSalt))
            {
                _hashSalt = sjclHashSalt;

                //check if user wrapped string in single or double quotes
                if (_hashSalt.StartsWith("\"") && _hashSalt.EndsWith("\""))
                    _hashSalt = _hashSalt.Substring(1, (_hashSalt.Length - 2));

                if (_hashSalt.StartsWith("'") && _hashSalt.EndsWith("'"))
                    _hashSalt = _hashSalt.Substring(1, (_hashSalt.Length - 2));

            }
                
            if (!String.IsNullOrWhiteSpace(sjclBulkCipherKey))
            {
                _bulkKey = sjclBulkCipherKey;
                if (_bulkKey.StartsWith("\"") && _bulkKey.EndsWith("\""))
                    _bulkKey = _bulkKey.Substring(1, (_bulkKey.Length - 2));

                if (_bulkKey.StartsWith("'") && _bulkKey.EndsWith("'"))
                    _bulkKey = _bulkKey.Substring(1, (_bulkKey.Length - 2));
            }

            return rtrn;
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
