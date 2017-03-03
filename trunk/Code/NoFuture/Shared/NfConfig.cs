using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using NoFuture.Exceptions;

namespace NoFuture.Shared
{

    public static class NfConfig
    {
        #region constants
        public const string FILE_NAME = "nfConfig.cfg.xml";
        internal const string MY_PS_HOME_VAR_NAME = "myPsHome";
        #endregion

        #region initialize
        public static string FindNfConfigFile()
        {
            var searchDirs = new[]
            {
                Assembly.GetExecutingAssembly().Location,
                Environment.CurrentDirectory,
                Environment.CurrentDirectory + "\bin",
                AppDomain.CurrentDomain.BaseDirectory,
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\NoFuture"
            };

            var nfCfg = "";
            foreach (var dir in searchDirs)
            {
                nfCfg = Path.Combine(dir, FILE_NAME);
                if (File.Exists(nfCfg))
                    break;
            }

            return nfCfg;
        }

        public static bool Init(string nfCfg)
        {
            if (string.IsNullOrWhiteSpace(nfCfg))
            {
                nfCfg = FindNfConfigFile();
                if(string.IsNullOrWhiteSpace(nfCfg))
                    throw new FileNotFoundException($"Cannot locate a copy of {FILE_NAME}");
            }
            var cfgXml = new XmlDocument();
            if (nfCfg.Trim().StartsWith("<"))
            {
                cfgXml.LoadXml(nfCfg);
            }
            else
            {
                cfgXml.Load(nfCfg);
            }
            
            var idValueHash = GetIdValueHash(cfgXml);
            ResolveIdValueHash(idValueHash);

            var assignmentHash = new Dictionary<string, Action<string>>
            {
                {"certFileNoFutureX509", s => SecurityKeys.NoFutureX509Cert = s},
                {"tempJsFile", s => TempFiles.JavaScript = s},
                {"tempHtmlFile", s => TempFiles.Html = s},
                {"tempCsvFile", s => TempFiles.Csv = s},
                {"tempStdOutFile", s => TempFiles.StdOut = s},
                {"tempT4TemplateFile", s => TempFiles.T4Template = s},
                {"tempNetStatFile", s => TempFiles.NetStat = s},
                {"tempWmiFile", s => TempFiles.Wmi = s},
                {"x64SvcUtilTool", s => X64.SvcUtil = s},
                {"x64IldasmTool", s => X64.Ildasm = s},
                {"x64WsdlTool", s => X64.Wsdl = s},
                {"x64ClrVerTool", s => X64.ClrVer = s},
                {"x64XsdExeTool", s => X64.XsdExe = s},
                {"x64CdbTool", s => X64.Cdb = s},
                {"x64TListTool", s => X64.TList = s},
                {"x64SymChkTool", s => X64.SymChk = s},
                {"x64DumpbinTool", s => X64.Dumpbin = s},
                {"x64SqlCmdTool", s => X64.SqlCmd = s},
                {"x64MdbgTool", s => X64.Mdbg = s},
                {"x86IldasmTool", s => X86.Ildasm = s},
                {"x86SqlMetalTool", s => X86.SqlMetal = s},
                {"x86SvcUtilTool", s => X86.SvcUtil = s},
                {"x86WsdlTool", s => X86.Wsdl = s},
                {"x86CdbTool", s => X86.Cdb = s},
                {"x86DependsTool", s => X86.Depends = s},
                {"x86DumpbinTool", s => X86.Dumpbin = s},
                {"x86TextTransformTool", s => X86.TextTransform = s},
                {"x86DotExeTool", s => X86.DotExe = s},
                {"javaJavacTool", s => JavaTools.Javac = s},
                {"javaJavaTool", s => JavaTools.Java = s},
                {"javaJavaDocTool", s => JavaTools.JavaDoc = s},
                {"javaJavaRtJarTool", s => JavaTools.JavaRtJar = s},
                {"javaJarTool", s => JavaTools.Jar = s},
                {"javaJRunScriptTool", s => JavaTools.JRunScript = s},
                {"javaAntlrTool", s => JavaTools.Antlr = s},
                {"javaStanfordPostTaggerTool", s => JavaTools.StanfordPostTagger = s},
                {"javaStanfordPostTaggerModelsTool", s => JavaTools.StanfordPostTaggerModels = s},
                {"customDia2DumpTool", s => CustomTools.Dia2Dump = s},
                {"customInvokeGetCgTypeTool", s => CustomTools.InvokeGetCgType = s},
                {"customInvokeGraphVizTool", s => CustomTools.InvokeGraphViz = s},
                {"customInvokeAssemblyAnalysisTool", s => CustomTools.InvokeAssemblyAnalysis = s},
                {"customInvokeFlattenTool", s => CustomTools.InvokeFlatten = s},
                {"customUtilPosHostTool", s => CustomTools.UtilPosHost = s},
                {"customInvokeDpxTool", s => CustomTools.InvokeDpx = s},
                {"customInvokeNfTypeNameTool", s => CustomTools.InvokeNfTypeName = s},
                {"binFfmpegTool", s => BinTools.Ffmpeg = s},
                {"binYoutubeDlTool", s => BinTools.YoutubeDl = s},
                {"tempRootDir", s => TempDirectories.Root = s},
                {"tempProcsDir", s => TempDirectories.StoredProcedures = s},
                {"tempCodeDir", s => TempDirectories.Code = s},
                {"tempTextDir", s => TempDirectories.Text = s},
                {"tempDebugsDir", s => TempDirectories.Debug = s},
                {"tempGraphDir", s => TempDirectories.Graph = s},
                {"tempSvcUtilDir", s => TempDirectories.SvcUtil = s},
                {"tempWsdlDir", s => TempDirectories.Wsdl = s},
                {"tempHbmDir", s => TempDirectories.Hbm = s},
                {"tempBinDir", s => TempDirectories.Binary = s},
                {"tempJavaSrcDir", s => TempDirectories.JavaSrc = s},
                {"tempJavaBuildDir", s => TempDirectories.JavaBuild = s},
                {"tempJavaDistDir", s => TempDirectories.JavaDist = s},
                {"tempJavaArchiveDir", s => TempDirectories.JavaArchive = s},
                {"tempCalendarDir", s => TempDirectories.Calendar = s},
                {"tempHttpAppDomainDir", s => TempDirectories.HttpAppDomain = s},
                {"tempTsvCsvDir", s => TempDirectories.TsvCsv = s},
                {"binRootDir", s => BinDirectories.Root = s},
                {"binX64RootDir", s => BinDirectories.X64Root = s},
                {"binX86RootDir", s => BinDirectories.X86Root = s},
                {"binJavaRootDir", s => BinDirectories.JavaRoot = s},
                {"binT4TemplatesDir", s => BinDirectories.T4Templates = s},
                {"binPhpRootDir", s => BinDirectories.PhpRoot = s},
                {"binDataRootDir", s => BinDirectories.DataRoot = s},
                {"portNsLookupPort", s => NfDefaultPorts.NsLookupPort = Convert.ToInt32(s)},
                {"portDomainEngine", s => NfDefaultPorts.DomainEngine = Convert.ToInt32(s)},
                {"portHostProc", s => NfDefaultPorts.HostProc = Convert.ToInt32(s)},
                {"portAssemblyAnalysis", s => NfDefaultPorts.AssemblyAnalysis = Convert.ToInt32(s)},
                {"portFlattenAssembly", s => NfDefaultPorts.FlattenAssembly = Convert.ToInt32(s)},
                {"portPartOfSpeechPaserHost", s => NfDefaultPorts.PartOfSpeechPaserHost = Convert.ToInt32(s)},
                {"portSjclToPlainText", s => NfDefaultPorts.SjclToPlainText = Convert.ToInt32(s)},
                {"portSjclToCipherText", s => NfDefaultPorts.SjclToCipherText = Convert.ToInt32(s)},
                {"portSjclHashPort", s => NfDefaultPorts.SjclHashPort = Convert.ToInt32(s)},
                {"portNfTypeNamePort", s => NfDefaultPorts.NfTypeNamePort = Convert.ToInt32(s)},
                {"portHbmInvokeStoredProcMgr", s => NfDefaultPorts.HbmInvokeStoredProcMgr = Convert.ToInt32(s)},
                {"switchPrintWebHeaders", s => Switches.PrintWebHeaders = Convert.ToBoolean(s)},
                {"switchSqlCmdHeadersOff", s => Switches.SqlCmdHeadersOff = Convert.ToBoolean(s)},
                {"switchSqlFiltersOff", s => Switches.SqlFiltersOff = Convert.ToBoolean(s)},
                {"switchSupressNpp", s => Switches.SupressNpp = Convert.ToBoolean(s)},
                {"keyAesEncryptionKey", s => SecurityKeys.AesEncryptionKey = s},
                {"keyAesIV", s => SecurityKeys.AesIV = s},
                {"keyHMACSHA1", s => SecurityKeys.HMACSHA1 = s},

            };
            var assignedSomething = false;
            var assignmentKeys = assignmentHash.Keys;
            foreach (var key in assignmentKeys)
            {
                if (!idValueHash.ContainsKey(key))
                    continue;
                var val = idValueHash[key];
                var act = assignmentHash[key];
                act(val);
                assignedSomething = true;
            }

            return assignedSomething;
        }

        internal static void ResolveIdValueHash(Dictionary<string, string> idValueHash)
        {
            if(!idValueHash.ContainsKey(MY_PS_HOME_VAR_NAME))
                idValueHash.Add(MY_PS_HOME_VAR_NAME, Environment.CurrentDirectory);

            var keys = idValueHash.Keys.ToArray();
            foreach (var id in keys)
            {
                idValueHash[id] = ExpandCfgValue(idValueHash, idValueHash[id]);
            }
        }

        internal static string ExpandCfgValue(Dictionary<string, string> idValueHash, string value)
        {
            var xRefId = "";
            if (!RegexCatalog.IsRegexMatch(value, @"\x24\x28([a-zA-Z0-9_\-\x25\x28\x29]+)\x29", out xRefId, 1))
                return value;

            var valueLessXrefId = value.Replace($"$({xRefId})", "");

            if (xRefId.StartsWith("%") && xRefId.EndsWith("%")) 
            {
                xRefId = Environment.ExpandEnvironmentVariables(xRefId);
                return $"{xRefId}{valueLessXrefId}";
            }

            if (!idValueHash.ContainsKey(xRefId))
                return $"{Environment.CurrentDirectory}{valueLessXrefId}";

            if (idValueHash[xRefId].Contains("$"))
            {
                var rValue = ExpandCfgValue(idValueHash, idValueHash[xRefId]);
                return $"{rValue}{valueLessXrefId}";
            }

            return $"{idValueHash[xRefId]}{valueLessXrefId}";
        }

        internal static Dictionary<string, string> GetIdValueHash(XmlDocument cfgXml)
        {
            var idNodes = cfgXml.SelectNodes("//*[@id]");
            if (idNodes == null)
                throw new RahRowRagee($"The {FILE_NAME} has no 'id' " +
                                      $"attributes and is probably not the correct file");

            var idValueHash = new Dictionary<string, string>();
            foreach (var idNode in idNodes)
            {
                var idElem = idNode as XmlElement;
                if (idElem == null
                    || !idElem.HasAttributes
                    || string.IsNullOrWhiteSpace(idElem.Attributes["id"]?.Value)
                    || string.IsNullOrWhiteSpace(idElem.Attributes["value"]?.Value))
                    continue;
                var id = idElem.Attributes["id"].Value;
                var val = idElem.Attributes["value"].Value;
                if (idValueHash.ContainsKey(id))
                    idValueHash[id] = val;
                else
                    idValueHash.Add(id, val);

            }
            return idValueHash;
        }
        #endregion

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

        public static char DefaultTypeSeparator { get; set; } = '.';

        /// <summary>
        /// The comma is a typical delimiter in many programming constructs.
        /// </summary>
        public static char DefaultCharSeparator { get; set; } = ',';

        /// <summary>
        /// Typical char of '-' used to delimit the start of a command line switch.
        /// </summary>
        public static string CmdLineArgSwitch { get; set; } = "-";

        /// <summary>
        /// Not so typical char of '=' used to right of a command line switch 
        /// to represent that said switch's assignment
        /// </summary>
        public static char CmdLineArgAssign { get; set; } = '=';

        public static long DefaultBlockSize { get; set; } = 256;

        public static char[] PyPunctuationChars { get; set; } = {
            '!', '"', '#', '$', '%', '&', '\\', '\'', '(', ')',
            '*', '+', ',', '-', '.', '/', ':', ';', '<', '=', '>',
            '?','@', '[', ']', '^', '_', '`', '{', '|', '}', '~'
        };

        public static string[] CodeExtensions { get; set; } =
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

        public static string[] ConfigExtensions { get; set; } =
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

        public static string[] BinaryExtensions { get; set; } =
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
