﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace NoFuture.Shared
{

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

        public const char DEFAULT_TYPE_SEPARATOR = '.';

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

        public const long DEFAULT_BLOCK_SIZE = 256;

        public static char[] PyPunctuationChars = {
            '!', '"', '#', '$', '%', '&', '\\', '\'', '(', ')',
            '*', '+', ',', '-', '.', '/', ':', ';', '<', '=', '>',
            '?','@', '[', ']', '^', '_', '`', '{', '|', '}', '~'
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
            "r",   //R syntax file   

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
            @"\.nuget\",
            @"\.vs\",
            @"\lib\",
            @"\build\", //common to Java
            @"\dist\",  //common to Java
            @"\packages\",
            @"\__pycache__\" //common to python in VS
        };

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
            public static string TsvCsv { get; set; }
        }

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
            public static string DataRoot { get; set; }
        }

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
            public static string JRunScript { get; set; }
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
            public static string InvokeFlatten { get; set; }
            public static string UtilPosHost { get; set; }
            public static string InvokeDpx { get; set; }
            public static string InvokeNfTypeName { get; set; }
        }

        public static class NfDefaultPorts
        {
            public static int NsLookupPort { get; set; } = 799;
            public static int DomainEngine { get; set; } = 1138;
            public static int HostProc { get; set; } = 780;
            public static int AssemblyAnalysis { get; set; } = 5059;//will add to this two times
            public static int FlattenAssembly { get; set; } = 5062;
            public static int PartOfSpeechPaserHost { get; set; } = 5063;
            public static int SjclToPlainText { get; set; } = 5064;
            public static int SjclToCipherText { get; set; } = 5065;
            public static int SjclHashPort { get; set; } = 5066;
            public static int NfTypeNamePort { get; set; } = 5067;
            public static int HbmInvokeStoredProcMgr { get; set; } = 45121;
        }

        public class BinTools
        {
            public static string Ffmpeg { get; set; }
            public static string YoutubeDl { get; set; }
        }

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
    }
}
