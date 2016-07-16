using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using NoFuture.Shared;


namespace NoFuture.Util.NfConsole
{
    public abstract class Program
    {
        private string _logName;
        private DateTime _startTime;
        private static readonly object _printLock = new object();
        protected readonly string[] _args;
        protected readonly bool _isVisable;

        protected Program(string[] args, bool isVisable)
        {
            _args = args;
            _isVisable = isVisable;
        }

        protected abstract string MyName { get; }

        /// <summary>
        /// Directory location where logs are saved to the file system.
        /// </summary>
        public string LogDirectory
        {
            get
            {
                var logDir = ConfigurationManager.AppSettings["NoFuture.TempDirectories.Debug"];
                if (string.IsNullOrWhiteSpace(logDir))
                    logDir = TempDirectories.AppData;
                var myName = MyName;
                myName = NfPath.SafeFilename(myName);
                logDir = string.IsNullOrWhiteSpace(myName) ? logDir : Path.Combine(logDir, myName);
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
                if (!string.IsNullOrWhiteSpace(_logName))
                    return _logName;
                _logName = Path.Combine(LogDirectory,
                    $"Program{_startTime:yyyy-MM-dd_hhmmss}.log");
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
            if (!string.IsNullOrWhiteSpace(cval) && int.TryParse(cval, out valOut))
                return valOut;
            return null;
        }

        protected internal bool? ResolveBool(string cval)
        {
            bool valOut;
            if (!string.IsNullOrWhiteSpace(cval) && bool.TryParse(cval, out valOut))
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
            try
            {
                if (!_isVisable)
                    return;
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }
        /// <summary>
        /// Prints to console and writes to <see cref="LogFile"/>
        /// </summary>
        /// <param name="someString"></param>
        /// <param name="trunc"></param>
        public void PrintToConsole(string someString, bool trunc = true)
        {
            try
            {
                File.AppendAllText(LogFile, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {someString}\n");
                if (trunc && someString.Length >= 74)
                    someString = $"{someString.Substring(0, 68)}[...]";

                if (_isVisable)
                    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {someString}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }
        /// <summary>
        /// Prints to console and writes to <see cref="LogFile"/>
        /// </summary>
        /// <param name="ex"></param>
        public void PrintToConsole(Exception ex)
        {
            lock (_printLock)
            {
                try
                {
                    Console.WriteLine();
                    var msg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {ex.Message}";
                    File.AppendAllText(LogFile, $"{msg}\n");

                    if (_isVisable)
                        Console.WriteLine(msg);

                    msg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {ex.StackTrace}";
                    File.AppendAllText(LogFile, $"{msg}\n");
                    if (_isVisable)
                        Console.WriteLine(msg);

                    if (ex.InnerException != null)
                        PrintToConsole(ex.InnerException);
                }
                catch (Exception exx)
                {
                    Debug.WriteLine(exx.Message);
                    Debug.WriteLine(exx.StackTrace);
                }
            }
        }

        protected internal void StartConsole()
        {
            //set app domain cfg
            _startTime = DateTime.Now;
            if (_isVisable)
            {
                try
                {
                    Console.WindowWidth = 100;
                    ConsoleCmd.SetConsoleAsTransparent();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Title = Assembly.GetExecutingAssembly().GetName().Name;
                    PrintToConsole($"{Console.Title} started");

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                }
            }
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
            NfConfig.UseReflectionOnlyLoad = useReflectionOnly != null && useReflectionOnly.Value;
        }
        /// <summary>
        /// Standard help for a console app - does NOT write to <see cref="LogFile"/>
        /// </summary>
        /// <returns></returns>
        protected abstract string Help();

        protected abstract void ParseProgramArgs();
    }
}
