using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

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
    /// Paths to directories used for storing temp results of NoFuture powershell scripts.
    /// </summary>
    public class TempDirectories
    {
        /// <summary>
        /// The NoFuture's AppData folder contained withing this environment's <see cref="Environment.SpecialFolder.ApplicationData"/>
        /// </summary>
        public static string AppData
        {
            get
            {
                var nfAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                if(string.IsNullOrWhiteSpace(nfAppData) || !Directory.Exists(nfAppData))
                    throw new DirectoryNotFoundException("The Environment.GetFolderPath for SpecialFolder.ApplicationData returned a bad path.");
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

namespace NoFuture.Tools
{
    /// <summary>
    /// The file path to exe's referenced by NoFuture powershell scripts
    /// </summary>
    public class X64
    {
        public static string SvcUtil { get; set; }
        public static string Cdb { get; set; }
        public static string TList { get; set; }
        public static string Depends { get; set; }
        public static string Dumpbin { get; set; }
        public static string Ildasm { get; set; }
        public static string SqlCmd { get; set; }
        public static string Wsdl { get; set; }
        public static string Mdbg { get; set; }
        public static string ClrVer { get; set; }
        public static string SymChk { get; set; }
        public static string XsdExe { get; set; }
    }
    /// <summary>
    /// The file path to exe's referenced by NoFuture powershell scripts
    /// </summary>
    public class X86
    {
        public static string Cdb { get; set; }
        public static string Depends { get; set; }
        public static string Dumpbin { get; set; }
        public static string Ildasm { get; set; }
        public static string SqlMetal { get; set; }
        public static string SvcUtil { get; set; }
        public static string TextTransform { get; set; }
        public static string Wsdl { get; set; }
        public static string DotExe { get; set; }
        public static int NsLookupPort { get; set; }
    }
    /// <summary>
    /// The file path to exe's, and java JAR files referenced by NoFuture powershell scripts
    /// </summary>
    public class JavaTools
    {
        public static string Javac { get; set; }
        public static string Java { get; set; }
        public static string JavaDoc { get; set; }
        public static string JavaRtJar { get; set; }
        public static string Jar { get; set; }
        public static string Ant { get; set; }
        public static string JRunScript { get; set; }
        public static int JrePort { get; set; }
        public static string Antlr { get; set; }
        public static string StanfordPostTagger { get; set; }
        public static string StanfordPostTaggerModels { get; set; }
    }
    /// <summary>
    /// The file path to exe's and dll's produced by this solution.
    /// </summary>
    public class CustomTools
    {
        public static string HostProc { get; set; }
        public static string RunTransparent { get; set; }
        public static string Favicon { get; set; }
        public static string CodeBase { get; set; }
        public static string Dia2Dump { get; set; }
        public static string InvokeGetCgType { get; set; }
        public static string InvokeGraphViz { get; set; }
        public static string InvokeAssemblyAnalysis { get; set; }
    }

    public class BinTools
    {
        public static string Ffmpeg { get; set; }
        public static string YoutubeDl { get; set; }
        
    }
}

namespace NoFuture.Globals
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
    }
}

namespace NoFuture.Shared
{
    public class RSAPKCS1SHA512SigDesc : SignatureDescription
    {
        public const string XML_NS_DIGEST = "http://www.w3.org/2001/04/xmlenc#sha512";
        public const string XML_NS_SIG = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512";

        public const string SHA_512 = "SHA512";

        public RSAPKCS1SHA512SigDesc()
        {
            FormatterAlgorithm = typeof(RSAPKCS1SignatureFormatter).FullName;
            DeformatterAlgorithm = typeof(RSAPKCS1SignatureDeformatter).FullName;

            KeyAlgorithm = typeof(RSACryptoServiceProvider).FullName;
            DigestAlgorithm = typeof(SHA512Managed).FullName;
        }

        public override AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            var d = new RSAPKCS1SignatureDeformatter(key);
            d.SetHashAlgorithm(SHA_512);
            return d;
        }

        public override AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            var f = new RSAPKCS1SignatureFormatter(key);
            f.SetHashAlgorithm(SHA_512);
            return f;
        }
    }
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

    /// <summary>
    /// Misc. constant values used throughout.
    /// </summary>
    public class Constants
    {
        public const string NF_CRYPTO_EXT = ".nfk"; //nofuture kruptos
        public const char LF = (char)0xA;
        public const char CR = (char)0xD;
        public const string OUTLOOK_APPT_PREFIX = "[shell]";
        public const long DEFAULT_BLOCK_SIZE = 256;
        public static char StringTerminator = '\0';
        public static DateTime UnixTimeZero = new DateTime(1970, 1, 1, 0, 0, 0);
        public static DateTime MsDosTimeZero = new DateTime(1980, 1, 1, 0, 0, 0);
        public static int SqlServerFalse = 0;
        public static int SqlServerTrue = 1;
        public static string AddJavaTypeRootPackage = "MyJava";
        
        private static string _symbolsFolder = @"C:\Symbols";
        private static int _threadSleepTime;
        private static bool _useReflectionOnlyLoad = true;
        private static readonly List<string> _searchAsmDirs = new List<string>();

        public const double DBL_TROPICAL_YEAR = 365.24255;
        public static TimeSpan TropicalYear = new TimeSpan(365, 5, 49, 16, 320);

        public const char DefaultTypeSeparator = '.';
        public const string TypeMethodNameSplitOn = "::";

        [EditorBrowsable(EditorBrowsableState.Never)]
        public const string ENUM = "System.Enum";

        /// <summary>
        /// The max size allowed by PowerShell 3.0's ConvertTo-Json cmdlet.
        /// </summary>
        public const int MAX_JSON_LEN = 2097152;

        /// <summary>
        /// Common name give to constructors in runtime type defs
        /// </summary>
        public const string CTOR_NAME = ".ctor";

        /// <summary>
        /// The comma is a typical delimiter in many programming constructs.
        /// </summary>
        public const char DEFAULT_CHAR_SEPARATOR = ',';

        /// <summary>
        /// Typical char of '-' used to delimit the start of a command line switch.
        /// </summary>
        public const string CMD_LINE_ARG_SWITCH = "-";

        /// <summary>
        /// Not so typical char of '=' used to right of a command line switch 
        /// to represent that said switch's assignment
        /// </summary>
        public const char CMD_LINE_ARG_ASSIGN = '=';

        public static bool UseReflectionOnlyLoad
        {
            get { return _useReflectionOnlyLoad; }
            set { _useReflectionOnlyLoad = value; }
        }

        private static BindingFlags _defalutFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic |
                                                    BindingFlags.Public | BindingFlags.Static;

        /// <summary>
        /// Default flags used to get a type's members.
        /// </summary>
        public static BindingFlags DefaultFlags
        {
            get { return _defalutFlags; }
            set { _defalutFlags = value; }
        }


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
        {
            get { return String.Format("Server={0};Database={1};Trusted_Connection=True;", SqlServer, SqlCatalog); }
        }

        /// <summary>
        /// Drafts an integrated security SSPI COM SqlServer Connection string using the 
        /// assigned values from <see cref="SqlServer"/> and <see cref="SqlCatalog"/>
        /// </summary>
        public static string SqlServerComConnString
        {
            get
            {
                return String.Format("Provider=SQLOLEDB;Data Source={0};Initial Catalog={1};Integrated Security=SSPI;",
                    SqlServer, SqlCatalog);
            }
        }

        /// <summary>
        /// Global paths used by NoFuture.Util.FxPointers.ResolveAssemblyEventHandler
        /// </summary>
        public static List<string> AssemblySearchPaths { get { return _searchAsmDirs; }}

        /// <summary>
        /// Path used as local directory within the environment variable '_NT_SYMBOL_PATH'
        /// </summary>
        public static string SymbolsPath { get { return _symbolsFolder; } set { _symbolsFolder = value; } }

        /// <summary>
        /// The location within the Registry where one may set 
        /// domains to a specific Zone.
        /// </summary>
        public const string RegistryZonePath = @"HKCU:\Software\Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\Domains";

        /// <summary>
        /// Useful link to a very extensive list of domains used 
        /// by advertisers.  Its original intention was for use in 
        /// 'Hosts' file.
        /// </summary>
        public const string HostTxt = "http://winhelp2002.mvps.org/hosts.txt";

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

        public static string[] CodeExtensions =
        {
            "asa", "asax", "ascx", "asmx", "asp", "aspx", "bas", "cls", "cpp", "c", "cs", "css", "frm", "h",
            "htm", "html", "java", "js", "master", "ps1", "sql", "vb", "vbs", "wsdl", "wsf", "xsd", "xsl", "xslt", "dsr",
            "cshtml", "vbhtml", "tt", "rpx", "g4", "gv", "json", "as", "mx", "ada", "ads", "adb", "asm", "sh", "bsh",
            "bat", "cmd", "nt",
            "hpp", "hxx", "cxx", "cc", "ml", "mli", "sml", "thy", "cbl", "cbd", "cdb", "cdc", "cob", "f", "for", "f90",
            "f95", "f2k",
            "hs", "lhs", "las", "shtml", "shtm", "xhtml", "hta", "lsp", "lisp", "lua", "m", "pas", "inc", "pl", "pm ",
            "plx", "php", "php3", "phtml", "ps",
            "psm1", "py", "pyw", "rb", "rbw", "scm", "smd", "ss", "st", "tcl", "tex", "xsml", "kml", "xlf", "xliff",
            "xaml", "fs", "fsx", "xsx", "vrg", "xst", "adml", "admx", "dtd", "fxh",
            "sep", //separator page files (for printing pages between pages)
            "scp", //Secure Copy Protocol? (C:\Windows\System32\ras)
            "wsc", //Windows Script Component
            "idl", //present in own C++ project

        };

        public static string[] ConfigExtensions =
        {
            "csproj", "vbproj", "fsproj", "vbp", "sln",
            "vdproj", "dsw", "vsmdi", "dtproj",
            "ini", "inf", "config", "xml", "sitemap",
            "dtsx", "database", "user", "edmx",
            "fcc", "pdm", "pp", "svc", "testsettings",
            "ps1xml", "content", "aml","nuspec",
            "acl", //AutoCorrect List Format
            "cva", "cfg", "din", "ecf", "ecf", "hkf", "iss", "gpd",
            "isp", "dun", "obe", //useful data at C:\Windows\System32\sppui
            "sam", //ini style cfg for WIN TCP/IP @ C:\Windows\System32\drivers\etc
            "sch", //SQLServer Replication Snapshot Schema Script,
            "h1k", //Microsoft Help Index File (is xml format)
            "stp", //only example is at C:\Windows\Help\Help\en-US\stopwrds.stp 
            "targets", "tasks", "overrideTasks", //xml which defines the steps to be used by MSBuild
            "theme", //ini-style to define the theme of Windows
            "manifest",
            "xbap", //XAML Browser Application
            "xrm-ms", //XrML Digital License
            "props", //Project Property File
            "psc1", //Powershell Console File
            "rsp", //command line options used by csc.exe, vbc.exe & MSBuild.exe
            "psd1", //Powershell Data File 
        };

        public static string[] BinaryExtensions =
        {
            "001","002","003","004","005","006","007","008","009","010","7z","aas",
            "acm","addin","adm","am","amx","ani","apk","apl","aps","aux","avi","ax",
            "bak","bcf","bcm","bin","bmp","bpd","bpl","browser","bsc","btr","bud",
            "cab","cache","camp","cap","cat","cch","ccu","cdf-ms","cdmp","chk","chm",
            "chr","chs","cht","clb","cmb","cmdline","cmf","com","comments","compiled",
            "compositefont","cov","cpa","cpl","cpx","crmlog","crt","csd","ctm","cty",
            "cur","cw","dat","data","db","dcf","dcr","default","delete","dem","desklink",
            "devicemetadata-ms","diagpkg","dic","dir","dll","dlm","dls","dmp","dnl","doc",
            "docx","drv","ds","dts","dub","dvd","dvr-ms","dxt","ebd","edb","efi","emf",
            "err","ess","etl","ev1","ev2","ev3","evm","evtx","ex_","exe","exp","fe",
            "fon","ftl","fx","gdl","gif","gmmp","grl","grm","gs_4_0","h1c","h1s","h1t",
            "hdr","hex","hit","hlp","hpi","hpx","iad","icc","icm","ico","id","idb",
            "idx","iec","if2","ilk","imd","ime","inf_loc","ins","inx","ipa","ird","jar",
            "jmx","jnt","job","jpeg","jpg","jpn","jrs","jtp","kor","ldo","lex",
            "lg1","lg2","lib","library-ms","lng","lnk","log","lrc","lts","lxa","mac",
            "man","map","mapimail","mdb","mdbx","mfl","mib","mid","mllr","mni","mof",
            "mp3","mp4","mpg","msc","msi","msm","msstyles","mst","msu","mui","mum",
            "mzz","ncb","ndx","ngr","nlp","nls","nlt","ntf","nupkg","obj","ocx",
            "olb","old","opt","out","pch","pdb","pdf","phn","plugin","pnf","png",
            "ppd","pptx","prm","prof","propdesc","prq","prx","ps_2_0","ps_4_0",
            "psd","ptxml","pub","que","rat","rdl","reg","res","resources","resx",
            "rld","rll","rom","rpo","rs","rtf","s3","sbr","scc","scr","sdb","sdi",
            "ses","shp","smp","sqm","ssm","stl","swf","sys","t4","tag","tar.gz",
            "tbl","tbr","tha","tif","tiff","tlb","tmp","toc","tpi","trie1","tsp",
            "ttc","ttf","tts","tx_","txt","uaq","uce","udt","uni","uninstall",
            "unt","url","vch","vdf","ver","vp","vs_1_1","vs_4_0","vsd","vspscc",
            "wav","wdf","web","wih","wim","win32manifest","wma","wmf","wmv","wmz",
            "wtv","wwd","x32","xex","xlb","xls","xlsx","zfsendtotarget","zip","pkc"
        };

        public static string[] ExcludeCodeDirectories = new[]
        {
            @"\bin\",
            @"\obj\",
            @"\Interop\",
            @"\TestResults\",
            @"\_svn\",
            @"\.svn\",
            @"\_ReSharper",
            @"\_TeamCity",
            @"\.git\",
            @"\lib\",
            @"\build\", //common to Java
            @"\dist\",  //common to Java
            @"\packages\",
        };

        #endregion
    }
}
