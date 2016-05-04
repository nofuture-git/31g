using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using NoFuture.Shared;


namespace NoFuture.Util.NfConsole
{
    public abstract class Program
    {
        public const int LISTEN_NUM_REQUEST = 5;

        private string _logName;
        private DateTime _startTime;
        private static readonly object _printLock = new object();
        protected readonly string[] _args;

        protected Program(string[] args)
        {
            _args = args;
        }

        /// <summary>
        /// Directory location where logs are saved to the file system.
        /// </summary>
        public string LogDirectory
        {
            get
            {
                var logDir = ConfigurationManager.AppSettings["NoFuture.TempDirectories.Debug"];
                if (String.IsNullOrWhiteSpace(logDir))
                    logDir = TempDirectories.AppData;
                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);
                return logDir;
            }
        }

        /// <summary>
        /// The full name of the processes log file
        /// </summary>
        public string LogFile
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(_logName))
                    return _logName;
                _logName = Path.Combine(LogDirectory,
                    String.Format("{0}{1:yyyy-MM-dd_hhmmss}.log", "InvokeAssemblyAnalysis", _startTime));
                return _logName;
            }
        }

        /// <summary>
        /// Message state specific to printing progress with <see cref="ProgressMessage"/>
        /// </summary>
        public Tuple<int, string> ProgressMessageState { get; set; }

        protected internal int? ResolveInt(string cval)
        {
            int valOut;
            if (!String.IsNullOrWhiteSpace(cval) && Int32.TryParse(cval, out valOut))
                return valOut;
            return null;
        }

        protected internal bool? ResolveBool(string cval)
        {
            bool valOut;
            if (!String.IsNullOrWhiteSpace(cval) && Boolean.TryParse(cval, out valOut))
                return valOut;
            return null;
        }
        /// <summary>
        /// Prints a header then a kind of console progress bar.
        /// Does NOT write to <see cref="LogFile"/>.
        /// </summary>
        /// <param name="pMsg"></param>
        public void PrintToConsole(ProgressMessage pMsg)
        {
            var currentState = new Tuple<int, string>(pMsg.ProgressCounter, pMsg.Status);

            if (ProgressMessageState == null)
            {
                Console.WriteLine(currentState.Item2);
                ProgressMessageState = currentState;
                return;
            }
            if (currentState.Item2 != ProgressMessageState.Item2)
            {
                Console.WriteLine();
                Console.WriteLine(currentState.Item2);
                ProgressMessageState = currentState;
                return;
            }
            if (currentState.Item1 > ProgressMessageState.Item1)
            {
                Console.Write('.');
                ProgressMessageState = currentState;
            }
        }
        /// <summary>
        /// Prints to console and writes to <see cref="LogFile"/>
        /// </summary>
        /// <param name="someString"></param>
        /// <param name="trunc"></param>
        public void PrintToConsole(string someString, bool trunc = true)
        {
            File.AppendAllText(LogFile, String.Format("{0:yyyy-MM-dd HH:mm:ss.fff} {1}\n", DateTime.Now, someString));
            if (trunc && someString.Length >= 74)
                someString = String.Format("{0}[...]", someString.Substring(0, 68));

            Console.WriteLine(String.Format("{0:yyyy-MM-dd HH:mm:ss.fff} {1}", DateTime.Now, someString));
        }
        /// <summary>
        /// Prints to console and writes to <see cref="LogFile"/>
        /// </summary>
        /// <param name="ex"></param>
        public void PrintToConsole(Exception ex)
        {
            lock (_printLock)
            {
                Console.WriteLine();
                var msg = String.Format("{0:yyyy-MM-dd HH:mm:ss.fff} {1}", DateTime.Now, ex.Message);
                File.AppendAllText(LogFile, String.Format("{0}\n", msg));
                Console.WriteLine(msg);

                msg = String.Format("{0:yyyy-MM-dd HH:mm:ss.fff} {1}", DateTime.Now, ex.StackTrace);
                File.AppendAllText(LogFile, String.Format("{0}\n", msg));
                Console.WriteLine(msg);
            }
        }

        protected internal void StartConsole()
        {
            //set app domain cfg
            _startTime = DateTime.Now;
            Console.WindowWidth = 100;
            ConsoleCmd.SetConsoleAsTransparent();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = Assembly.GetExecutingAssembly().GetName().Name;
            SetReflectionOnly();
            FxPointers.AddResolveAsmEventHandlerToDomain();            
        }

        protected internal bool PrintHelp()
        {
            //test if there are any args, if not just leave with no message
            if (_args.Length > 0 && (_args[0] == "-h" || _args[0] == "-help" || _args[0] == "/?" || _args[0] == "--help"))
            {
                Console.WriteLine(Help());
                return true;
            }
            return false;
        }

        /// <summary>
        /// Assignable from the config file. Determines if assemblies are loaded using ReflectionOnly
        /// </summary>
        protected internal void SetReflectionOnly()
        {
            var useReflectionOnly =
                ResolveBool(ConfigurationManager.AppSettings["NoFuture.Shared.Constants.UseReflectionOnlyLoad"]);
            Constants.UseReflectionOnlyLoad = useReflectionOnly != null && useReflectionOnly.Value;
        }
        /// <summary>
        /// Standard help for a console app - does NOT write to <see cref="LogFile"/>
        /// </summary>
        /// <returns></returns>
        protected abstract string Help();

        protected abstract void ParseProgramArgs();
    }
}
