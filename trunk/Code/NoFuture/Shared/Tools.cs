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
        public static string InvokeFlatten { get; set; }
        public static string UtilPosHost { get; set; }
        public static string InvokeDpx { get; set; }
    }

    public static class NfDefaultPorts
    {
        public const int DOMAIN_ENGINE = 1138;
        public const int HOST_PROC = 780;
        public const int ASSEMBLY_ANALYSIS = 5059;//will add to this two times
        public const int FLATTEN_ASSEMBLY = 5062;
        public const int PART_OF_SPEECH_PARSER_HOST = 5063;
        public const int SJCL_TO_PLAIN_TXT = 5064;
        public const int SJCL_TO_CIPHER_TXT = 5065;
        public const int SJCL_HASH_PORT = 5066;
        public const int HBM_INVOKE_STORED_PROC_MGR = 45121;
    }

    public class BinTools
    {
        public static string Ffmpeg { get; set; }
        public static string YoutubeDl { get; set; }
    }
}
