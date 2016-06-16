using System.Collections.Generic;

namespace NoFuture
{
    /// <summary>
    /// A reference class used to store the names of NoFuture powershell functions.
    /// </summary>
    public class MyFunctions
    {
        /// <summary>
        /// At the top of most NoFuture powershell scripts a try\catch used to map the 
        /// powershell function to the file in which its defined.  All of the NoFuture powershell 
        /// script files are expected to be loaded into a console from a single call to the start.ps1
        /// </summary>
        public static Dictionary<string, string> FunctionFiles = new Dictionary<string, string>();
    }


    /// <summary>
    /// Resuable temp file paths used by various NoFuture powershell scripts.
    /// </summary>
    public class TempFiles
    {
        public static string JavaScript { get; set; }
        public static string Html { get; set; }
        public static string Csv { get; set; }
        public static string NetStat { get; set; }
        public static string T4Template { get; set; }
        public static string StdOut { get; set; }
        public static string Wmi { get; set; }
    }
    /// <summary>
    /// Paths to specific directories used by powershell scripts, 
    /// <see cref="Root"/> is expected to be in a 'bin' folder directly 
    /// below the location of the powershell scripts themselves.
    /// </summary>
    public class BinDirectories
    {
        public static string Root { get; set; }
        public static string X64Root { get; set; }
        public static string X86Root { get; set; }
        public static string JavaRoot { get; set; }
        public static string T4Templates { get; set; }
        public static string PhpRoot { get; set; }
    }
}
