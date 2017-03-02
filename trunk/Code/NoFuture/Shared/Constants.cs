using System;
using System.ComponentModel;

namespace NoFuture.Shared
{
    /// <summary>
    /// For encoding strings
    /// </summary>
    public enum EscapeStringType
    {
        DECIMAL,
        DECIMAL_LONG,
        HEXDECIMAL,
        UNICODE,
        REGEX,
        HTML,
        XML,
        BLANK
    }

    /// <summary>
    /// Common versions of the .NET framework
    /// </summary>
    public enum DotNetVersion
    {
        NET11,
        NET20,
        NET35,
        NET4Plus
    }

    public enum ChronoCompare
    {
        Before = -1,
        SameTime = 0,
        After = 1
    }
    /// <summary>
    /// Misc. constant values used throughout.
    /// </summary>
    public class Constants
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public const string ENUM = "System.Enum";

        public const string NF_CRYPTO_EXT = ".nfk"; //nofuture kruptos
        public const char LF = (char)0xA;
        public const char CR = (char)0xD;
        public const string OUTLOOK_APPT_PREFIX = "[shell]";
        public const char STRING_TERMINATOR = '\0';
        public const int SQL_SERVER_FALSE = 0;
        public const int SQL_SERVER_TRUE = 1;

        public const string TYPE_METHOD_NAME_SPLIT_ON = "::";

        public const int SOCKET_LISTEN_NUM = 5;
        /// <summary>
        /// The max size allowed by PowerShell 3.0's ConvertTo-Json cmdlet.
        /// </summary>
        public const int MAX_JSON_LEN = 2097152;

        /// <summary>
        /// Common name give to constructors in runtime type defs
        /// </summary>
        public const string CTOR_NAME = ".ctor";



        public const double DBL_TROPICAL_YEAR = 365.24255;

        public static TimeSpan TropicalYear = new TimeSpan(365, 5, 49, 16, 320);

        /// <summary>
        /// The location within the Registry where one may set 
        /// domains to a specific Zone.
        /// </summary>
        public const string REGISTRY_ZONE_PATH =
            @"HKCU:\Software\Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\Domains";

        /// <summary>
        /// Useful link to a very extensive list of domains used 
        /// by advertisers.  Its original intention was for use in 
        /// 'Hosts' file.
        /// </summary>
        public const string HOST_TXT = "http://winhelp2002.mvps.org/hosts.txt";

        #region string arrays

        /// <summary>
        /// see http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.connectionstring(v=vs.90).aspx
        /// </summary>
        public static string[] MsSqlConnStrKeywords =
        {
            "Application Name", "Async", "AttachDBFilename", "extended properties", "Initial File Name",
            "Connect Timeout",
            "Connection Timeout", "Context Connection", "Current Language", "Data Source", "Server", "Address", "Addr",
            "Network Address",
            "Encrypt", "Enlist", "Failover Partner", "Initial Catalog", "Database", "Integrated Security",
            "Trusted_Connection", "MultipleActiveResultSets",
            "Network Library", "Net", "Packet Size", "Password", "Pwd", "Persist Security Info", "Replication",
            "Transaction Binding", "TrustServerCertificate",
            "Type System Version", "User ID", "User Instance", "Workstation ID", "Connection Lifetime", "Enlist",
            "Max Pool Size", "Min Pool Size", "Pooling"
        };

        #endregion
    }
}
