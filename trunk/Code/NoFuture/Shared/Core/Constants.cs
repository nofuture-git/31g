using System;
using System.IO;
using System.ComponentModel;
using System.Reflection;

namespace NoFuture.Shared.Core
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
        URI,
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
        [EditorBrowsable(EditorBrowsableState.Never)]
        public const string ENUM = "System.Enum";

        public const string NF_CRYPTO_EXT = ".nfk"; //nofuture kruptos
        public const char LF = (char)0xA;
        public const char CR = (char)0xD;
        public const string TYPE_METHOD_NAME_SPLIT_ON = "::";

        public const int SOCKET_LISTEN_NUM = 5;
        /// <summary>
        /// The max size allowed by PowerShell 3.0's ConvertTo-Json cmdlet.
        /// </summary>
        public const int MAX_JSON_LEN = 2097152;

        public const double DBL_TROPICAL_YEAR = 365.24255;

        public static TimeSpan TropicalYear = new TimeSpan(365, 5, 49, 16, 320);


        public static char DefaultTypeSeparator { get; set; } = '.';

        /// <summary>
        /// The comma is a typical delimiter in many programming constructs.
        /// </summary>
        public static char DefaultCharSeparator { get; set; } = ',';

        public static char[] PunctuationChars { get; set; } = {
            '!', '"', '#', '$', '%', '&', '\\', '\'', '(', ')',
            '*', '+', ',', '-', '.', '/', ':', ';', '<', '=', '>',
            '?','@', '[', ']', '^', '_', '`', '{', '|', '}', '~'
        };


        public static string[] CodeFileExtensions { get; set; } =
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

        public static string[] ConfigFileExtensions { get; set; } =
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

        public static string[] BinaryFileExtensions { get; set; } =
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

        public static string[] ExcludeCodeDirectories { get; set; } = new[]
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
        /// The NoFuture's AppData folder contained within 
        /// this environment's <see cref="Environment.SpecialFolder.ApplicationData"/>
        /// </summary>
        public static string AppData
        {
            get
            {
                var nfAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                if (String.IsNullOrWhiteSpace(nfAppData) || !Directory.Exists(nfAppData))
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

        /// <summary>
        /// Default flags used to get a type's members.
        /// </summary>
        public static BindingFlags DefaultFlags { get; set; } = BindingFlags.DeclaredOnly | BindingFlags.Instance |
                                                                BindingFlags.NonPublic |
                                                                BindingFlags.Public | BindingFlags.Static;
    }
}
