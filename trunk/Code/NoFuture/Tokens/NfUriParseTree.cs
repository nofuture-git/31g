using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public static UriRef Parse(string uri)
        {
            return NfUriParseTree.InvokeParse(uri);
        }

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
        public OpaqueUriValue OpaquePart { get; set; }
    }

    public class HierPart
    {
        public string UriQuery { get; set; }
        public NetPath NetPath { get; set; }
        public AbsPath AbsPath { get; set; }
    }

    public class OpaqueUriValue
    {
        public char Delimiter { get; set; }
        public string Value { get; set; }
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
        public string RegistryNameAuth { get; set; }
        public UriServer ServerNameAuth { get; set; }
    }

    public class UriServer
    {
        public string UserInfo { get; set; }
        public UriHost Host { get; set; }
        public int Port { get; set; }
    }

    public enum HostNameKind
    {
        Ipv4,
        Ipv6,
        Dns
    }
    public class UriHost
    {
        public string HostName { get; set; }
        public HostNameKind HostKind { get; set; }
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
        #region fields
        private UriRef _results;
        private readonly ParseTreeProperty<UriHost> _uriHostPtp = new ParseTreeProperty<UriHost>();
        private readonly ParseTreeProperty<UriServer> _uriServerPtp = new ParseTreeProperty<UriServer>();
        private readonly ParseTreeProperty<UriAuthority> _uriAuthPtp = new ParseTreeProperty<UriAuthority>();
        private readonly ParseTreeProperty<RelPath> _relPathPtp = new ParseTreeProperty<RelPath>();
        private readonly ParseTreeProperty<AbsPath> _absPathPtp = new ParseTreeProperty<AbsPath>();
        private readonly ParseTreeProperty<NetPath> _netPathPtp = new ParseTreeProperty<NetPath>();
        private readonly ParseTreeProperty<HierPart> _hierPartPtp = new ParseTreeProperty<HierPart>();
        private readonly ParseTreeProperty<RelUri> _relUriPtp = new ParseTreeProperty<RelUri>();
        private readonly ParseTreeProperty<AbsUri> _absUriPtp = new ParseTreeProperty<AbsUri>();
        private readonly ParseTreeProperty<string> _ipv6Ptp = new ParseTreeProperty<string>();
        private readonly ParseTreeProperty<string> _ipv4Ptp = new ParseTreeProperty<string>();
        private readonly ParseTreeProperty<string> _uriParmPtp = new ParseTreeProperty<string>();
        private readonly ParseTreeProperty<string> _uriQryPtp = new ParseTreeProperty<string>();
        private readonly ParseTreeProperty<string> _uriFragPtp = new ParseTreeProperty<string>();
        private readonly ParseTreeProperty<string> _hostNamePtp = new ParseTreeProperty<string>();
        private readonly ParseTreeProperty<UriSegments> _uriSegmentPtp = new ParseTreeProperty<UriSegments>();
        private readonly ParseTreeProperty<OpaqueUriValue> _opaquePtp = new ParseTreeProperty<OpaqueUriValue>();
        #endregion

        #region properties
        public UriRef Results { get { return _results; } }
        #endregion

        #region ctors
        public NfUriParseTree(string originalString)
        {
            _results = new UriRef {OriginalString = originalString};
        }
        #endregion

        #region overrides
        public override void ExitIpv6Address(NfUriParser.Ipv6AddressContext context)
        {
            _ipv6Ptp.Put(context, context.GetText());
        }
        public override void ExitIpv4Address(NfUriParser.Ipv4AddressContext context)
        {
            _ipv4Ptp.Put(context, context.GetText());
        }

        public override void ExitUriParam(NfUriParser.UriParamContext context)
        {
            _uriParmPtp.Put(context, context.GetText());
        }

        public override void ExitUriQuery(NfUriParser.UriQueryContext context)
        {
            _uriQryPtp.Put(context, context.GetText());
        }

        public override void ExitUriFragment(NfUriParser.UriFragmentContext context)
        {
            _uriFragPtp.Put(context, context.GetText());
        }

        public override void ExitHostName(NfUriParser.HostNameContext context)
        {
            _hostNamePtp.Put(context, context.GetText());
        }

        public override void ExitUriSegment(NfUriParser.UriSegmentContext context)
        {
            var nfUriSeg = new UriSegments() {Params = new List<string>()};
            if (context.PCHAR_STRING() != null)
                nfUriSeg.Value = context.PCHAR_STRING().ToString();
            if (context.uriParam() != null && context.uriParam().Length > 0)
            {
                foreach (
                    var paramVal in
                        context.uriParam()
                            .Select(uriParam => uriParam.GetText())
                            .Where(paramVal => !string.IsNullOrWhiteSpace(paramVal)))
                {
                    nfUriSeg.Params.Add(paramVal);
                }
            }

            _uriSegmentPtp.Put(context, nfUriSeg);
        }

        public override void ExitUriHost(NfUriParser.UriHostContext context)
        {
            var nfUriHost = new UriHost();
            if (context.ipv4Address() != null)
            {
                nfUriHost.HostName = context.ipv4Address().GetText();
                nfUriHost.HostKind = HostNameKind.Ipv4;
            }
            if (context.ipv6Address() != null)
            {
                nfUriHost.HostName = context.ipv6Address().GetText();
                nfUriHost.HostKind = HostNameKind.Ipv6;
            }
            if (context.hostName() != null)
            {
                nfUriHost.HostName = context.hostName().GetText();
                nfUriHost.HostKind = HostNameKind.Dns;
            }

            _uriHostPtp.Put(context, nfUriHost);
        }

        public override void ExitUriServer(NfUriParser.UriServerContext context)
        {
            var nfUriServer = new UriServer();

            if (context.uriHost() != null)
                nfUriServer.Host = _uriHostPtp.Get(context.uriHost());
            if (context.URI_NUM() != null)
            {
                var uriPort = context.URI_NUM().GetText();
                int port;
                if (int.TryParse(uriPort, out port))
                    nfUriServer.Port = port;
            }

            if (context.USER_INFO() != null)
            {
                nfUriServer.UserInfo = context.USER_INFO().GetText();
            }

            _uriServerPtp.Put(context, nfUriServer);
        }

        public override void ExitAuthority(NfUriParser.AuthorityContext context)
        {
            var nfAuth = new UriAuthority();
            if (context.REG_NAME() != null)
            {
                nfAuth.RegistryNameAuth = context.REG_NAME().GetText();
            }
            if (context.uriServer() != null)
            {
                nfAuth.ServerNameAuth = _uriServerPtp.Get(context.uriServer());
            }

            _uriAuthPtp.Put(context, nfAuth);
        }

        public override void ExitRelPath(NfUriParser.RelPathContext context)
        {
            var nfRelPath = new RelPath();
            if (context.REL_SEGMENT() != null)
                nfRelPath.RelSegment = context.REL_SEGMENT().GetText();

            if (context.absPath() != null)
            {
                nfRelPath.AbsPath = _absPathPtp.Get(context.absPath());
            }

            _relPathPtp.Put(context, nfRelPath);
        }

        public override void ExitAbsPath(NfUriParser.AbsPathContext context)
        {
            var nfAbsPath = new AbsPath {Segments = new List<UriSegments>()};
            if (context.uriSegment() != null && context.uriSegment().Length > 0)
            {
                foreach (
                    var ptpSeg in
                        context.uriSegment().Select(uriSeg => _uriSegmentPtp.Get(uriSeg)).Where(ptpSeg => ptpSeg != null)
                    )
                {
                    nfAbsPath.Segments.Add(ptpSeg);
                }
            }

            _absPathPtp.Put(context, nfAbsPath);
        }

        public override void ExitNetPath(NfUriParser.NetPathContext context)
        {
            var nfNetPath = new NetPath();
            if (context.authority() != null)
            {
                nfNetPath.Authority = _uriAuthPtp.Get(context.authority());
            }
            if (context.absPath() != null)
                nfNetPath.AbsPath = _absPathPtp.Get(context.absPath());

            _netPathPtp.Put(context, nfNetPath);
        }

        public override void ExitOpaquePart(NfUriParser.OpaquePartContext context)
        {
            var nfOpaque = new OpaqueUriValue();
            if (context.URIC_NOSLASH() != null)
            {
                var delimiter = context.URIC_NOSLASH().GetText();
                if (!string.IsNullOrEmpty(delimiter))
                    nfOpaque.Delimiter = delimiter.ToCharArray()[0];
            }

            if (context.URIC_STRING() != null)
                nfOpaque.Value = context.URIC_STRING().GetText();

            _opaquePtp.Put(context, nfOpaque);
        }

        public override void ExitHierPart(NfUriParser.HierPartContext context)
        {
            var nfHier = new HierPart();
            if (context.netPath() != null)
                nfHier.NetPath = _netPathPtp.Get(context.netPath());
            if (context.absPath() != null)
                nfHier.AbsPath = _absPathPtp.Get(context.absPath());

            if (context.uriQuery() != null)
                nfHier.UriQuery = context.uriQuery().GetText();

            _hierPartPtp.Put(context, nfHier);
        }

        public override void ExitUriRelative(NfUriParser.UriRelativeContext context)
        {
            var nfRelUri = new RelUri();
            if (context.netPath() != null)
                nfRelUri.NetPath = _netPathPtp.Get(context.netPath());
            if (context.absPath() != null)
                nfRelUri.AbsPath = _absPathPtp.Get(context.absPath());
            if (context.relPath() != null)
                nfRelUri.RelPath = _relPathPtp.Get(context.relPath());

            if (context.uriQuery() != null)
                nfRelUri.UriQuery = context.uriQuery().GetText();

            _relUriPtp.Put(context, nfRelUri);
        }

        public override void ExitUriAboslute(NfUriParser.UriAbosluteContext context)
        {
            var nfAbsUri = new AbsUri();
            if (context.URI_SCHEMA() != null)
                nfAbsUri.UriScheme = context.URI_SCHEMA().GetText();
            if (context.hierPart() != null)
                nfAbsUri.HierPart = _hierPartPtp.Get(context.hierPart());
            if (context.opaquePart() != null)
            {
                nfAbsUri.OpaquePart = _opaquePtp.Get(context.opaquePart());
            }

            _absUriPtp.Put(context, nfAbsUri);
        }

        public override void ExitUriReference(NfUriParser.UriReferenceContext context)
        {
            var nfUriRef = new UriRef();
            if (context.uriAboslute() != null)
                nfUriRef.AbsUri = _absUriPtp.Get(context.uriAboslute());
            if (context.uriRelative() != null)
                nfUriRef.RelUri = _relUriPtp.Get(context.uriRelative());
            if (context.uriFragment() != null)
                nfUriRef.Fragment = context.uriFragment().GetText();

            _results = nfUriRef;
        }
        #endregion

        public static UriRef InvokeParse(string uriString)
        {
            if (string.IsNullOrEmpty(uriString))
                return null;

            var ms = new MemoryStream(Encoding.ASCII.GetBytes(uriString));

            var input = new AntlrInputStream(ms);
            var lexer = new NfUriLexer(input);
            var tokens = new CommonTokenStream(lexer);
            var parser = new NfUriParser(tokens);

            var tree = parser.uriAboslute();

            var walker = new ParseTreeWalker();
            var loader = new NfUriParseTree(uriString);

            walker.Walk(loader, tree);

            var results = loader.Results;
            results.OriginalString = uriString;

            ms.Close();

            return results;
        }
    }
}
