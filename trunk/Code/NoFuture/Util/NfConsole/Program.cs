using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Linq;
using NoFuture.Shared;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;


namespace NoFuture.Util.NfConsole
{
    /// <summary>
    /// A Nf type to extend for basic console apps
    /// </summary>
    public abstract class Program
    {
        #region constants
        protected internal static string[] HelpCmdLnArgs = {"-h", "-help", "/?", "--help" };
        protected internal const string TEMP_DIR_DEBUG = "NoFuture.TempDirectories.Debug";
        protected internal const string ROOT_BIN_DIR = "NoFuture.BinDirectories.Root";
        protected internal const string USE_REFLX_LOAD = "NoFuture.Shared.Constants.UseReflectionOnlyLoad";
        #endregion

        #region fields
        private string _logName;
        private DateTime _startTime;
        private static readonly object _printLock = new object();
        protected readonly string[] _args;
        protected readonly bool _isVisable;
        #endregion

        #region ctors
        protected Program(string[] args, bool isVisable)
        {
            _args = args;
            _isVisable = isVisable;
        }
        #endregion

        #region properties
        /// <summary>
        /// A name used for creating a log folder in the <see cref="TEMP_DIR_DEBUG"/>
        /// </summary>
        protected abstract string MyName { get; }

        /// <summary>
        /// Directory location where logs are saved to the file system.
        /// </summary>
        public string LogDirectory
        {
            get
            {
                var logDir = SysCfg.GetAppCfgSetting(TEMP_DIR_DEBUG);
                if (string.IsNullOrWhiteSpace(logDir))
                    logDir = NfSettings.AppData;
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

        /// <summary>
        /// The original cmd line args passed in at ctor time.
        /// </summary>
        public string[] MyArgs => _args;

        #endregion

        #region methods
        /// <summary>
        /// Helper method to get a possiable int from some string the the config.
        /// </summary>
        /// <param name="cval"></param>
        /// <returns></returns>
        protected internal int? ResolveInt(string cval)
        {
            int valOut;
            if (!string.IsNullOrWhiteSpace(cval) && int.TryParse(cval, out valOut))
                return valOut;
            return null;
        }

        /// <summary>
        /// Helper method to get a possiable bool from some string in the config.
        /// </summary>
        /// <param name="cval"></param>
        /// <returns></returns>
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

        public void PrintToConsole()
        {
            Console.WriteLine();
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
                if (string.IsNullOrWhiteSpace(someString))
                {
                    Console.WriteLine();
                    return;
                }

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
                    var msg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {ex.GetType().FullName}";
                    File.AppendAllText(LogFile, $"{msg}\n");

                    if (_isVisable)
                        Console.WriteLine(msg);

                    Console.WriteLine();
                    msg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {ex.Message}";
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

        /// <summary>
        /// Performs boilerplate code settings
        /// </summary>
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
                    Console.Title = Assembly.GetEntryAssembly().GetName().Name;
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

        /// <summary>
        /// Prints the contents of <see cref="GetHelpText"/> to the console.
        /// </summary>
        /// <returns>
        /// Returns true if there is at least one cmd line arg and its value is 
        /// found in <see cref="HelpCmdLnArgs"/>.
        /// </returns>
        protected internal bool PrintHelp(string[] cmdLnArgs = null)
        {
            if (cmdLnArgs == null)
                cmdLnArgs = _args;
            //test if there are any args, if not just leave with no message
            if (cmdLnArgs.Length <= 0 || HelpCmdLnArgs.All(x => x != cmdLnArgs[0]?.ToLower()))
                return false;
            Console.WriteLine(GetHelpText());
            return true;
        }

        /// <summary>
        /// Assignable from the config file. Determines if assemblies are loaded using ReflectionOnly
        /// </summary>
        protected internal void SetReflectionOnly()
        {
            var useReflectionOnly =
                ResolveBool(SysCfg.GetAppCfgSetting(USE_REFLX_LOAD));
            NfConfig.UseReflectionOnlyLoad = useReflectionOnly != null && useReflectionOnly.Value;
        }

        /// <summary>
        /// Standard help for a console app - does NOT write to <see cref="LogFile"/>
        /// </summary>
        /// <returns></returns>
        protected abstract string GetHelpText();

        /// <summary>
        /// Parse the console cmd line args into instance variables of the given type.
        /// </summary>
        protected abstract void ParseProgramArgs();
        #endregion 
    }
}
