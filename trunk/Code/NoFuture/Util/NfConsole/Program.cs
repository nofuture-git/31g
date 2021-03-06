﻿using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Linq;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NoFuture.Util.Binary;
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
        protected internal const string DEFAULT_FLAGS = "NoFuture.Shared.Core.NfSettings.DefaultFlags";
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
        public virtual string LogDirectory
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
        public virtual string LogFile
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_logName))
                    return _logName;
                _logName = Path.Combine(LogDirectory,
                    $"{GetType().Name}{_startTime:yyyy-MM-dd_HHmmss}.log");
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
        /// Helper method to get a possible int from some string the the config.
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
        /// Helper method to get a possible bool from some string in the config.
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
        /// Prints to the console
        /// </summary>
        /// <param name="pMsg"></param>
        public virtual void PrintToConsole(ProgressMessage pMsg)
        {
            try
            {
                if (!_isVisable)
                    return;
                var currentState = new Tuple<int, string>(pMsg.ProgressCounter, pMsg.Status);

                if (ProgressMessageState == null)
                {
                    PrintToConsole(currentState.Item2);
                    ProgressMessageState = currentState;
                    return;
                }

                if (currentState.Item2 != ProgressMessageState.Item2)
                {
                    PrintToConsole();
                    PrintToConsole(currentState.Item2);
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

        public virtual void PrintToConsole()
        {
            Console.WriteLine();
        }

        /// <summary>
        /// Prints to the console
        /// </summary>
        /// <param name="someString"></param>
        /// <param name="trunc"></param>
        public virtual void PrintToConsole(string someString, bool trunc = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(someString))
                {
                    Console.WriteLine();
                    return;
                }

                PrintToLog(someString);
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

        public virtual void PrintToLog(string someString)
        {
            var v = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {someString}";
            if (!v.EndsWith("\n"))
                v += "\n";

            File.AppendAllText(LogFile, v);
        }

        /// <summary>
        /// Prints to console and writes to <see cref="LogFile"/>
        /// </summary>
        /// <param name="ex"></param>
        public virtual void PrintToConsole(Exception ex)
        {
            lock (_printLock)
            {
                try
                {
                    Console.WriteLine();
                    PrintToLog(ex.GetType().FullName);

                    if (_isVisable)
                        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {ex.GetType().FullName}");

                    Console.WriteLine();
                    PrintToLog(ex.Message);

                    if (_isVisable)
                        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {ex.Message}");

                    Console.WriteLine();
                    PrintToLog(ex.StackTrace);
                    if (_isVisable)
                        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {ex.StackTrace}");

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
        protected internal void StartConsole(string title = null)
        {
            //set app domain cfg
            _startTime = DateTime.Now;
            if (_isVisable)
            {
                try
                {
                    title = title ?? Assembly.GetEntryAssembly().GetName().Name;
                    Console.WindowWidth = 100;
                    ConsoleCmd.SetConsoleAsTransparent();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Title = title;
                    PrintToConsole($"{Console.Title} started");

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                }
            }
            SetReflectionOnly();
            SetDefaultReflectionFlags();
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
        /// Assignable from config file.  Allows for control over the default <see cref="BindingFlags"/>
        /// used in .NET reflection
        /// </summary>
        protected internal void SetDefaultReflectionFlags()
        {
            var strDfFlagsRaw = SysCfg.GetAppCfgSetting(DEFAULT_FLAGS);
            if (string.IsNullOrWhiteSpace(strDfFlagsRaw))
                return;
            var hasFlags = false;
            var dfFlag = BindingFlags.Public;
            var splitOn = strDfFlagsRaw.Contains(",") ? ',' : '|';
            foreach (var strDfFlag in strDfFlagsRaw.Split(splitOn))
            {
                if(string.IsNullOrWhiteSpace(strDfFlag))
                    continue;

                if (Enum.TryParse(strDfFlag.Trim(), true, out BindingFlags flag))
                {
                    dfFlag |= flag;
                    hasFlags = true;
                }
            }

            if (hasFlags)
                NfSettings.DefaultFlags = dfFlag;
        }

        internal static BindingFlags? GetBindingFlags(string strDfFlagsRaw)
        {
            var dfFlag = BindingFlags.Public;
            if (string.IsNullOrWhiteSpace(strDfFlagsRaw))
                return null;
            var hasFlags = false;
            var splitOn = strDfFlagsRaw.Contains(",") ? ',' : '|';
            foreach (var strDfFlag in strDfFlagsRaw.Split(splitOn))
            {
                if (string.IsNullOrWhiteSpace(strDfFlag))
                    continue;

                if (Enum.TryParse(strDfFlag.Trim(), true, out BindingFlags flag))
                {
                    dfFlag |= flag;
                    hasFlags = true;
                }
            }

            
            return hasFlags ? new BindingFlags?(dfFlag) : null;
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
