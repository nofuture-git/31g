using System;
using System.IO;

namespace NoFuture
{

    /// <summary>
    /// Paths to directories used for storing temp results of NoFuture powershell scripts.
    /// </summary>
    public class TempDirectories
    {
        /// <summary>
        /// The NoFuture's AppData folder contained within 
        /// this environment's <see cref="Environment.SpecialFolder.ApplicationData"/>
        /// </summary>
        public static string AppData
        {
            get
            {
                var nfAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                if (string.IsNullOrWhiteSpace(nfAppData) || !Directory.Exists(nfAppData))
                    throw new DirectoryNotFoundException("The Environment.GetFolderPath for " +
                                                         "SpecialFolder.ApplicationData returned a bad path.");
                nfAppData = Path.Combine(nfAppData, "NoFuture");
                if (!Directory.Exists(nfAppData))
                {
                    Directory.CreateDirectory(nfAppData);
                }
                return nfAppData;
            }
        }

        public static string Root { get; set; }
        public static string Sql { get; set; }
        public static string StoredProcedures { get; set; }
        public static string Binary { get; set; }
        public static string Code { get; set; }
        public static string Graph { get; set; }
        public static string Text { get; set; }
        public static string Debug { get; set; }
        public static string SvcUtil { get; set; }
        public static string Wsdl { get; set; }
        public static string Hbm { get; set; }
        public static string JavaSrc { get; set; }
        public static string JavaBuild { get; set; }
        public static string JavaDist { get; set; }
        public static string JavaArchive { get; set; }
        public static string Calendar { get; set; }
        public static string HttpAppDomain { get; set; }
        public static string Audio { get; set; }
    }
}
