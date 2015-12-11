using System.Collections.Generic;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using NoFuture.Antlr.Grammers;
using NoFuture.Tokens.NfUri;

namespace NoFuture.Tokens.NfUri
{
    public class UriRef
    {
        public string OriginalString { get; set; }
        public AbsUri AbsUri { get; set; }
        public RelUri RelUri { get; set; }
        public string Fragment { get; set; }

        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(OriginalString) ? OriginalString : base.ToString();
        }
    }

    public class RelUri
    {
        public string UriQuery { get; set; }
        public NetPath NetPath { get; set; }
        public AbsPath AbsPath { get; set; }
        public RelPath RelPath { get; set; }
    }

    public class AbsUri
    {
        public string UriScheme { get; set; }
        public HierPart HierPart { get; set; }
        public string OpaquePart { get; set; }
    }

    public class HierPart
    {
        public string UriQuery { get; set; }
        public NetPath NetPath { get; set; }
        public AbsPath AbsPath { get; set; }
    }

    public class NetPath
    {
        public UriAuthority Authority { get; set; }
        public AbsPath AbsPath { get; set; }
    }

    public class AbsPath
    {
        public List<UriSegments> Segments { get; set; }
    }

    public class RelPath
    {
        public string RelSegment { get; set; }
        public AbsPath AbsPath { get; set; }
    }

    public class UriAuthority
    {
        public string RegName { get; set; }
        public UriServer Server { get; set; }
    }

    public class UriServer
    {
        public string UserInfo { get; set; }
        public UriHost Host { get; set; }
        public int Port { get; set; }
    }

    public class UriHost
    {
        public NfHostName HostName { get; set; }
        public string Ipv4 { get; set; }
        public string Ipv6 { get; set; }
    }

    public class NfHostName
    {
        public string TopLabel { get; set; }
        public List<string> DomainLabels { get; set; }
    }

    public class UriSegments
    {
        public string Value { get; set; }
        public List<string> Params { get; set; }
    }

}

namespace NoFuture.Tokens
{
    //data:image/png;base64,iVBORw0KGgoAAAANSUh .... 
    public class NfUriParseTree : NfUriBaseListener
    {
        private UriRef _results;
        public UriRef Results { get { return _results; } }

        public NfUriParseTree(string originalString)
        {
            _results = new UriRef {OriginalString = originalString};
        }

        public override void ExitUriReference(NfUriParser.UriReferenceContext context)
        {
            //TODO parse the ANTLR context into NfUri.UriRef
        }

        public static UriRef InvokeParse(string uriString)
        {
            if (string.IsNullOrEmpty(uriString))
                return null;

            var ms = new MemoryStream(Encoding.ASCII.GetBytes(uriString));

            var input = new AntlrInputStream(ms);
            var lexer = new NfUriLexer(input);
            var tokens = new CommonTokenStream(lexer);
            var parser = new NfUriParser(tokens);

            var tree = parser.uriReference();

            var walker = new ParseTreeWalker();
            var loader = new NfUriParseTree(uriString);

            walker.Walk(loader, tree);

            var results = loader.Results;

            ms.Close();

            return results;
        }
    }
}
