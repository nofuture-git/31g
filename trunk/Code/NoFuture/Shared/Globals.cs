using System;
using System.Collections.Generic;
using System.Reflection;

namespace NoFuture.Shared
{
    /// <summary>
    /// Booleans intended as global to the namespace
    /// </summary>
    public class Switches
    {
        public static bool PrintWebHeaders { get; set; }
        public static bool SqlCmdHeadersOff { get; set; }
        public static bool SqlFiltersOff { get; set; }
        public static bool SupressNpp { get; set; }
    }
    /// <summary>
    /// Various keys, those which are not assigned a value would be real keys externally defined -
    /// the rest is just for flippin' bits.
    /// </summary>
    public class SecurityKeys
    {
        public static string AesEncryptionKey = "gb0352wHVco94Gr260BpJzH+N1yrwmt5/BaVhXmPm6s=";
        public static string AesIV = "az9HzsMj6pygMvZyTpRo6g==";
        public static string HMACSHA1 = "eTcmPilTLmtbalRpKjFFJjpMNns=";
        public static Uri ProxyServer;
        public static string GoogleCodeApiKey { get; set; }
        public static string BeaDataApiKey { get; set; }
        public static string CensusDataApiKey { get; set; }
        public static string NoFutureX509Cert { get; set; }
        public static string BlsApiRegistrationKey { get; set; }
    }

    public static class NfConfig
    {
        private static int _threadSleepTime;

        public static string NfLoggerName { get; set; } = "NfConsoleLogger";

        /// <summary>
        /// Default flags used to get a type's members.
        /// </summary>
        public static BindingFlags DefaultFlags { get; set; } = BindingFlags.DeclaredOnly | BindingFlags.Instance |
                                                                BindingFlags.NonPublic |
                                                                BindingFlags.Public | BindingFlags.Static;
        /// <summary>
        /// Used when invoking Assembly.Load
        /// </summary>
        public static bool UseReflectionOnlyLoad { get; set; } = true;

        /// <summary>
        /// Shared location to assign thread sleep in milliseconds
        /// </summary>
        public static int ThreadSleepTime
        {
            get
            {
                if (_threadSleepTime == 0)
                    _threadSleepTime = 500;
                return _threadSleepTime;
            }
            set { _threadSleepTime = value; }
        }

        /// <summary>
        /// This is expected, in PowerShell, to match the $Global variable of the same name
        /// </summary>
        public static string SqlServer { get; set; }

        /// <summary>
        /// This is expected, in PowerShell, to match the $Global variable of the same name
        /// </summary>
        public static string SqlCatalog { get; set; }

        /// <summary>
        /// Drafts a trusted connection .NET SqlClient Connection string using the 
        /// assigned values from <see cref="SqlServer"/> and <see cref="SqlCatalog"/>
        /// </summary>
        public static string SqlServerDotNetConnString
            => $"Server={SqlServer};Database={SqlCatalog};Trusted_Connection=True;";

        /// <summary>
        /// Global paths used by NoFuture.Util.FxPointers.ResolveAssemblyEventHandler
        /// </summary>
        public static List<string> AssemblySearchPaths { get; } = new List<string>();

        /// <summary>
        /// Path used as local directory within the environment variable '_NT_SYMBOL_PATH'
        /// </summary>
        public static string SymbolsPath { get; set; } = @"C:\Symbols";
    }
}