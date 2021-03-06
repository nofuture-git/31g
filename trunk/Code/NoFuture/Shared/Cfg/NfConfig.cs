﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using NoFuture.Shared.Core;

namespace NoFuture.Shared.Cfg
{
    /// <summary>
    /// Represents a central runtime hub from which all the other parts of the 
    /// namespace are dependent.
    /// </summary>
    public static class NfConfig
    {
        #region constants
        /// <summary>
        /// The file name from which <see cref="NfConfig"/> receives all its
        /// values.
        /// </summary>
        public const string FILE_NAME = "nfConfig.cfg.xml";

        /// <summary>
        /// A kind of &apos;root&apos; directory from which all the 
        /// other paths in <see cref="FILE_NAME"/> are leafs.
        /// </summary>
        internal const string MY_PS_HOME_VAR_NAME = "myPsHome";

        /// <summary>
        /// Common name give to constructors in runtime type defs
        /// </summary>
        public const string CTOR_NAME = ".ctor";

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

        #region fields
        /// <summary>
        /// Is the key-value hash which links the id's in <see cref="FILE_NAME"/> 
        /// to the properties of the <see cref="NfConfig"/> 
        /// </summary>
        private static Dictionary<string, Action<string>> _cfgIdName2PropertyAssigment = new Dictionary<string, Action<string>>
        {
            {"certFileNoFutureX509", s => SecurityKeys.NoFutureX509Cert = s},
            {"favicon", s => Favicon = s },
            {"tempNetStatFile", s => TempFiles.NetStat = s},
            {"x64SvcUtilTool", s => X64.SvcUtil = s},
            {"x64WsdlTool", s => X64.Wsdl = s},
            {"x64XsdExeTool", s => X64.XsdExe = s},
            {"x64DumpbinTool", s => X64.Dumpbin = s},
            {"x64SqlCmdTool", s => X64.SqlCmd = s},
            {"x86SqlMetalTool", s => X86.SqlMetal = s},
            {"x86SvcUtilTool", s => X86.SvcUtil = s},
            {"x86WsdlTool", s => X86.Wsdl = s},
            {"x86DotExeTool", s => X86.DotExe = s},
            {"javaJavacTool", s => JavaTools.Javac = s},
            {"javaJavaTool", s => JavaTools.Java = s},
            {"javaJarTool", s => JavaTools.Jar = s},
            {"javaAntlrTool", s => JavaTools.Antlr = s},
            {"javaStanfordPostTaggerTool", s => JavaTools.StanfordPostTagger = s},
            {"javaStanfordPostTaggerModelsTool", s => JavaTools.StanfordPostTaggerModels = s},
            {"customDia2DumpTool", s => CustomTools.Dia2Dump = s},
            {"customInvokeGetCgTypeTool", s => CustomTools.InvokeGetCgType = s},
            {"customInvokeGraphVizTool", s => CustomTools.InvokeGraphViz = s},
            {"customInvokeAssemblyAnalysisTool", s => CustomTools.InvokeAssemblyAnalysis = s},
            {"customInvokeFlattenTool", s => CustomTools.InvokeFlatten = s},
            {"customTokensPosHostTool", s => CustomTools.TokensPosHost = s},
            {"customInvokeDpxTool", s => CustomTools.InvokeDpx = s},
            {"customInvokeNfTypeNameTool", s => CustomTools.InvokeNfTypeName = s},
            {"ffmpegTool", s => PythonTools.Ffmpeg = s},
            {"youtubeDlTool", s => PythonTools.YoutubeDl = s},
            {"tempRootDir", s => TempDirectories.Root = s},
            {"tempSqlDir", s => TempDirectories.Sql = s },
            {"tempProcsDir", s => TempDirectories.StoredProx = s},
            {"tempCodeDir", s => TempDirectories.Code = s},
            {"tempDebugsDir", s => TempDirectories.Debug = s},
            {"tempGraphDir", s => TempDirectories.Graph = s},
            {"tempSvcUtilDir", s => TempDirectories.SvcUtil = s},
            {"tempWsdlDir", s => TempDirectories.Wsdl = s},
            {"tempHbmDir", s => TempDirectories.Hbm = s},
            {"tempJavaSrcDir", s => TempDirectories.JavaSrc = s},
            {"tempJavaBuildDir", s => TempDirectories.JavaBuild = s},
            {"tempJavaDistDir", s => TempDirectories.JavaDist = s},
            {"tempJavaArchiveDir", s => TempDirectories.JavaArchive = s},
            {"tempAudioDir", s => TempDirectories.Audio = s },
            {"binRootDir", s => BinDirectories.Root = s},
            {"binX64RootDir", s => BinDirectories.X64Root = s},
            {"binX86RootDir", s => BinDirectories.X86Root = s},
            {"binJavaRootDir", s => BinDirectories.JavaRoot = s},
            {"python36", s => BinDirectories.PythonRoot = s},
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
            {"code-file-extensions", s => NfSettings.CodeFileExtensions = s.Split(' ')},
            {"config-file-extensions", s => NfSettings.ConfigFileExtensions = s.Split(' ')},
            {"binary-file-extensions", s => NfSettings.BinaryFileExtensions = s.Split(' ')},
            {"search-directory-exclusions", s => NfSettings.ExcludeCodeDirectories = s.Split(' ')},
            {"default-block-size", s => DefaultBlockSize = Convert.ToInt32(s)},
            {"default-type-separator", s => NfSettings.DefaultTypeSeparator = Convert.ToChar(s)},
            {"default-char-separator", s => NfSettings.DefaultCharSeparator = Convert.ToChar(s)},
            {"cmd-line-arg-switch", s => CmdLineArgSwitch = s},
            {"cmd-line-arg-assign", s => CmdLineArgAssign = Convert.ToChar(s)},
            {"punctuation-chars", s => NfSettings.PunctuationChars = s.Split(' ').Select(Convert.ToChar).ToArray()},
            {"cscExe", s => DotNet.CscCompiler = s},
            {"vbcExe", s => DotNet.VbcCompiler = s},
            {"pythonExe", s => PythonTools.PythonExe = s},
            {"nfPythonPath", s => PythonTools.NfPythonPath = s}
        };

        private static int _threadSleepTime;

        #endregion

        #region initialize
        /// <summary>
        /// A helper function which looks for the <see cref="FILE_NAME"/> in the 
        /// normal .NET runtime directories (e.g. Current Domain's base directory, 
        /// the current working directory, etc).
        /// </summary>
        /// <returns></returns>
        public static string FindNfConfigFile(string pwd = null)
        {
            var searchDirs = new[]
            {
                pwd,
                Assembly.GetExecutingAssembly().Location,
                Environment.CurrentDirectory,
                Environment.CurrentDirectory + "\bin",
                AppDomain.CurrentDomain.BaseDirectory,
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\NoFuture",
            };

            var nfCfg = "";
            foreach (var dir in searchDirs.Where(x => !String.IsNullOrWhiteSpace(x)))
            {
                nfCfg = Path.Combine(dir, FILE_NAME);
                if (File.Exists(nfCfg))
                    break;
            }

            return nfCfg;
        }

        /// <summary>
        /// The static ctor of <see cref="NfConfig"/>.
        /// </summary>
        /// <param name="nfCfg">
        /// This may be either a path to <see cref="FILE_NAME"/> 
        /// or the actual full xml content thereof.
        /// </param>
        /// <returns></returns>
        public static bool Init(string nfCfg)
        {
            if (String.IsNullOrWhiteSpace(nfCfg))
            {
                nfCfg = FindNfConfigFile();
                if(String.IsNullOrWhiteSpace(nfCfg))
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

            var assignmentHash = _cfgIdName2PropertyAssigment;
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

        /// <summary>
        /// Resolves all the place holders found in the <see cref="FILE_NAME"/> to 
        /// their fully-expanded representation.
        /// </summary>
        /// <param name="idValueHash"></param>
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

        /// <summary>
        /// A recursive function to turn the place-holders of <see cref="value"/>
        /// into their fully expanded form.
        /// </summary>
        /// <param name="idValueHash"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>
        /// See the xml comment atop of <see cref="FILE_NAME"/> for an explanation 
        /// of what a placeholder is and how it works.
        /// </remarks>
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

        /// <summary>
        /// Resolves the contents of <see cref="FILE_NAME"/> into a hashtable of
        /// id-to-value pairs.
        /// </summary>
        /// <param name="cfgXml"></param>
        /// <returns></returns>
        internal static Dictionary<string, string> GetIdValueHash(XmlDocument cfgXml)
        {
            var idNodes = cfgXml.SelectNodes("//*[@id]");
            if (idNodes == null)
                throw new RahRowRagee($"The {FILE_NAME} has no 'id' " +
                                      "attributes and is probably not the correct file");

            var idValueHash = new Dictionary<string, string>();
            foreach (var idNode in idNodes)
            {
                var idElem = idNode as XmlElement;
                if (idElem == null
                    || !idElem.HasAttributes
                    || String.IsNullOrWhiteSpace(idElem.Attributes["id"]?.Value))
                    continue;

                var hasValueAttr = idElem.HasAttribute("value");
                var hasValueAttrWithAssignment = hasValueAttr &&
                                                 !String.IsNullOrWhiteSpace(idElem.Attributes["value"]?.Value);

                //has value attr assigned to empty string
                if (hasValueAttr && !hasValueAttrWithAssignment)
                    continue;
                var id = idElem.Attributes["id"].Value;
                string val;
                if (!hasValueAttr)
                {
                    //expecting CDATA values
                    var cDataNode = idElem.FirstChild;
                    var cDataElem = cDataNode as XmlCDataSection;
                    if (String.IsNullOrWhiteSpace(cDataElem?.Value))
                        continue;
                    val = cDataElem.Value;
                }
                else
                {
                    //expecting value attr values
                    val = idElem.Attributes["value"].Value;
                }

                if (idValueHash.ContainsKey(id))
                    idValueHash[id] = val;
                else
                    idValueHash.Add(id, val);

            }
            return idValueHash;
        }

        /// <summary>
        /// A peripheral function to resolve the value of the id-value pair
        /// where the value is assigned as Cdata child node instead of the 
        /// typical 'value' attribute.
        /// </summary>
        /// <param name="cfgXml"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        internal static string GetCdataValue(XmlDocument cfgXml, string nodeName)
        {
            if (cfgXml == null || String.IsNullOrWhiteSpace(nodeName))
                return String.Empty;

            var n = cfgXml.SelectSingleNode($"//{nodeName}");
            var e = n as XmlElement;
            if (e == null || !e.HasChildNodes)
                return String.Empty;
            var cdataNode = e.FirstChild;
            var cdata = cdataNode as XmlCDataSection;

            return cdata?.Value ?? String.Empty;
        }
        #endregion

        #region properties
        public static string Favicon { get; set; }

        public static string NfLoggerName { get; set; } = "NfConsoleLogger";


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


        /// <summary>
        /// The op char used to delimit the start of a command line switch.
        /// </summary>
        public static string CmdLineArgSwitch { get; set; } = "-";

        /// <summary>
        /// The op char used in a command line to assign a value to a swtich.
        /// </summary>
        public static char CmdLineArgAssign { get; set; } = '=';

        public static long DefaultBlockSize { get; set; } = 256;

        #endregion

        #region inner types
        /// <summary>
        /// Paths to directories used for storing temp results of NoFuture powershell scripts.
        /// </summary>
        public class TempDirectories
        {
            public static string Root { get; set; }
            public static string Sql { get; set; }
            public static string StoredProx { get; set; }
            public static string Code { get; set; }
            public static string Graph { get; set; }
            public static string Debug { get; set; }
            public static string SvcUtil { get; set; }
            public static string Wsdl { get; set; }
            public static string Hbm { get; set; }
            public static string JavaSrc { get; set; }
            public static string JavaBuild { get; set; }
            public static string JavaDist { get; set; }
            public static string JavaArchive { get; set; }
            public static string Audio { get; set; }
        }

        /// <summary>
        /// A reference class used to store the names of NoFuture PowerShell functions.
        /// </summary>
        public class MyFunctions
        {
            /// <summary>
            /// At the top of most NoFuture PowerShell scripts a try\catch used to map the 
            /// PowerShell function to the file in which its defined.  All of the NoFuture PowerShell 
            /// script files are expected to be loaded into a console from a single call to the start.ps1
            /// </summary>
            public static Dictionary<string, string> FunctionFiles = new Dictionary<string, string>();
        }

        /// <summary>
        /// Reusable temp file paths used by various NoFuture PowerShell scripts.
        /// </summary>
        public class TempFiles
        {
            public static string NetStat { get; set; }
        }
        /// <summary>
        /// Paths to specific directories used by PowerShell scripts, 
        /// <see cref="Root"/> is expected to be in a &quot;bin&quot; folder directly 
        /// below the location of the PowerShell scripts themselves.
        /// </summary>
        public class BinDirectories
        {
            public static string Root { get; set; }
            public static string X64Root { get; set; }
            public static string X86Root { get; set; }
            public static string JavaRoot { get; set; }
            public static string PythonRoot { get; set; }
        }

        /// <summary>
        /// The file path to exe&apos;s referenced by NoFuture PowerShell scripts
        /// </summary>
        public class X64
        {
            public static string SvcUtil { get; set; }
            public static string Dumpbin { get; set; }
            public static string SqlCmd { get; set; }
            public static string Wsdl { get; set; }
            public static string XsdExe { get; set; }
        }
        /// <summary>
        /// The file path to exe&apos;s referenced by NoFuture PowerShell scripts
        /// </summary>
        public class X86
        {
            public static string SqlMetal { get; set; }
            public static string SvcUtil { get; set; }
            public static string Wsdl { get; set; }
            public static string DotExe { get; set; }
        }

        public class DotNet
        {
            public static string CscCompiler { get; set; }
            public static string VbcCompiler { get; set; }
        }

        /// <summary>
        /// The file path to exe&apos;s, and java JAR files referenced by NoFuture PowerShell scripts
        /// </summary>
        public class JavaTools
        {
            public static string Javac { get; set; }
            public static string Java { get; set; }
            public static string Jar { get; set; }
            public static string Antlr { get; set; }
            public static string StanfordPostTagger { get; set; }
            public static string StanfordPostTaggerModels { get; set; }
        }
        /// <summary>
        /// The file path to exe&apos;s and dll&apos;s produced by this solution.
        /// </summary>
        public class CustomTools
        {
            public static string HostProc { get; set; }
            public static string RunTransparent { get; set; }
            public static string CodeBase { get; set; }
            public static string Dia2Dump { get; set; }
            public static string InvokeGetCgType { get; set; }
            public static string InvokeGraphViz { get; set; }
            public static string InvokeAssemblyAnalysis { get; set; }
            public static string InvokeFlatten { get; set; }
            public static string TokensPosHost { get; set; }
            public static string InvokeDpx { get; set; }
            public static string InvokeNfTypeName { get; set; }
        }

        public static class NfDefaultPorts
        {
            public static int NsLookupPort { get; set; } = 799;
            public static int DomainEngine { get; set; } = 1138;
            public static int HostProc { get; set; } = 780;
            public static int AssemblyAnalysis { get; set; } = 5057;//will add to this five times
            public static int FlattenAssembly { get; set; } = 5062;
            public static int PartOfSpeechPaserHost { get; set; } = 5063;
            public static int SjclToPlainText { get; set; } = 5064;
            public static int SjclToCipherText { get; set; } = 5065;
            public static int SjclHashPort { get; set; } = 5066;
            public static int NfTypeNamePort { get; set; } = 5067;
            public static int HbmInvokeStoredProcMgr { get; set; } = 45121;
        }

        public class PythonTools
        {
            public static string PythonExe { get; set; }
            public static string Ffmpeg { get; set; }
            public static string YoutubeDl { get; set; }
            public static string NfPythonPath { get; set; }
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
        /// the rest is just for flippin&apos; bits.
        /// </summary>
        public class SecurityKeys
        {
            public static Uri ProxyServer;
            public static string GoogleCodeApiKey { get; set; }
            public static string BeaDataApiKey { get; set; }
            public static string CensusDataApiKey { get; set; }
            public static string NoFutureX509Cert { get; set; }
            public static string BlsApiRegistrationKey { get; set; }
        }
        #endregion
    }
}
