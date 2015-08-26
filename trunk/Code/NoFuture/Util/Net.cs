using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NoFuture.Util
{
    public class Net
    {
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private static Tuple<int, string>[] _htmlEscStrings;
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private static Dictionary<Tuple<string,string>, int> _tcpPorts;
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private static Dictionary<Tuple<string, string>, int> _udpPorts;

        /// <summary>
        /// Concat's the enviornment variables 'COMPUTERNAME' with 'USERDNSDOMAIN' 
        /// with which local DNS could resolve to an IP
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            var userHost = Environment.GetEnvironmentVariable("COMPUTERNAME");
            var userDnsDomain = Environment.GetEnvironmentVariable("USERDNSDOMAIN");
            if (!string.IsNullOrWhiteSpace(userDnsDomain))
                userHost = userHost + "." + userDnsDomain;

            return userHost;
            
        }

        /// <summary>
        /// Returns true if either the 'Content-Transfer-Encoding' is 'Binary'
        /// or the 'Content-Type' equals any of the values in <see cref="BinaryMimeTypes"/>.
        /// </summary>
        /// <param name="responsHeaders">HTTP Response Headers as a simple hashtable.</param>
        /// <returns></returns>
        public static bool IsBinaryContent(Hashtable responsHeaders)
        {
            if (responsHeaders.ContainsKey("Content-Transfer-Encoding") &&
                string.Equals(responsHeaders["Content-Transfer-Encoding"].ToString(), "Binary",
                    StringComparison.OrdinalIgnoreCase))
                return true;

            if (responsHeaders.ContainsKey("Content-Type") &&
                BinaryMimeTypes.Any(
                    binMime =>
                        string.Equals(responsHeaders["Content-Type"].ToString(), binMime,
                            StringComparison.OrdinalIgnoreCase)))
                return true;

            return false;
        }

        /// <summary>
        /// Asserts that <see cref="cmdPort"/> is not null, is numerical
        /// and is within range of 1 to 65536
        /// </summary>
        /// <param name="cmdPort"></param>
        /// <returns></returns>
        public static bool IsValidPortNumber(int? cmdPort)
        {
            return (cmdPort != null && cmdPort > 0 && cmdPort < (System.Math.Pow(2, 16)));
        }

        //application/x-www-form-urlencoded

        /// <summary>
        /// Intended to determine if the response stream from a web request should
        /// be returned as-is or written to a file and opened by some local application.
        /// </summary>
        public static string[] BinaryMimeTypes
        {
            get
            {
                return new[]
                {
                    "application/octet-stream", 
                    "application/ogg",
                    "application/zip", 
                    "application/gzip", 
                    "application/x-nacl",
                    "application/x-pnacl", 
                    "audio/basic", 
                    "audio/L24", 
                    "audio/mpeg",
                    "audio/ogg", 
                    "audio/vorbis", 
                    "audio/vnd.rn-realaudio", 
                    "audio/vnd.wave",
                    "audio/webm", 
                    "image/gif", 
                    "image/jpeg", 
                    "image/pjpeg", 
                    "image/png", 
                    "image/vnd.djvu",
                    "video/avi", 
                    "video/mpeg", 
                    "video/mp4", 
                    "video/ogg", 
                    "video/quicktime", 
                    "video/webm",
                    "video/x-matroska", 
                    "video/x-ms-wmv", 
                    "video/x-flv", 
                    "application/vnd.debian.binary-package",
                    "application/vnd.oasis.opendocument.text",
                    "application/vnd.oasis.opendocument.spreadsheet",
                    "application/vnd.oasis.opendocument.presentation",
                    "application/vnd.oasis.opendocument.graphics",
                    "application/vnd.ms-excel",
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "application/vnd.ms-powerpoint",
                    "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "application/vnd.google-earth.kml+xml",
                    "application/vnd.google-earth.kmz",
                    "application/vnd.android.package-archive",
                    "application/vnd.ms-xpsdocument",
                    "application/x-7z-compressed",
                    "application/x-chrome-extension",
                    "application/x-dvi",
                    "application/x-font-ttf",
                    "application/x-latex",
                    "application/x-rar-compressed",
                    "application/x-shockwave-flash",
                    "application/x-stuffit",
                    "application/x-tar",
                    "application/x-xpinstall",
                    "audio/x-aac",
                    "audio/x-caf",
                    "image/x-xcf",
                    "application/x-pkcs12"
                };

            }
            
        }

        /// <summary>
        /// These are intended as my own additions to the contents from <see cref="NoFuture.Shared.Constants.HostTxt"/>.
        /// </summary>
        public static string[] MyRestrictedDomains { get
        {
            return new[] {
			"*.imwx.com", 
			"*.amazon-adsystem.com", 
			"*.wfxtriggers.com", 
			"*.insightexpressai.com",
			"*.abajurlight.ru",
			"*.arskat.ru",
			"*.cookingok.ruyasitemap",
			"*.domik-derevne.ru",
			"*.dresswoman.ru",
			"*.golden-praga.ru",
			"*.homesp.ru",
			"*.iqfood.ru",
			"*.jjbabskoe.ru",
			"*.komputernaya-pomosh-moscow.ru",
			"*.kulinapko.ruyasitemap",
			"*.master-muznachas.ru",
			"*.myfavoritebag.ru",
			"*.pechatay-prosto.ru",
			"*.pornovkoz.ru",
			"*.psi-lab.ru",
			"*.reklama1.ru",
			"*.remont-comp-pomosh.ru",
			"*.remont-fridge-tv.ru",
			"*.remont-komputerov-notebook.ru",
			"*.sdelai-prosto.ru",
			"*.ucoz.ru",
			"*.tiu.ru",
			"*.vita-power.ru",
			"*.vroze.ru",
			"*.vsepravda.ru",
			"*.afina-lingerie.ru",
			"*.artparquet.ru",
			"*.edunok.ru",
			"*.kbaptupa.ru",
			"*.video12.ru",
			"*.xxart.ru",
			"*.ary.kz",
			"*.mastercash.co.il",
			"*.prorok.pw",
			"*.10-casino.net",
			"*.cvety24.by",
			"*.fastxak.com",
			"*.gungamesz.com",
			"*.pornowife.tv",
			"*.prorok.pw",
			"*.statuses.su",
			"*.pornboobsy.com",
			"*.infoprosto.com",
			"*.droidlook.net",
			"*.banan.tv",
			"*.7zap.pro",
			"*.hdporno7.com",
			"*.lampokrat.ws",
			"*.ridder.asia",
			"*.flowwwers.com",
            "*.tinyurl.com",
            "*.avto-mir.dp.ua",
            "*.diplom-nk.com",
            "*.tbncom.com",
            "*.weblinkvalidator.com",
            "*.streamingvideoslive.com",
            "*.duckduckgo.com"
			};
        } }

        /// <summary>
        /// This is intended, but no limited, to the use of calls to netStat.exe
        /// where the IPv4 and IPv6 address will be in various forms that need 
        /// to be inspected prior to straight calls to <see cref="System.Net.IPAddress.TryParse"/>.
        /// </summary>
        /// <param name="netStatAddress"></param>
        /// <returns></returns>
        public static System.Net.IPAddress GetNetStatIp(string netStatAddress)
        {
            
            if (string.IsNullOrWhiteSpace(netStatAddress))
                return System.Net.IPAddress.Loopback;

            netStatAddress = netStatAddress.Trim();

            if (Regex.IsMatch(netStatAddress, @"\x2a\x3a\x2a")) //*:*
                return System.Net.IPAddress.Loopback;
            if (Regex.IsMatch(netStatAddress, @"\x30\x2e\x30\x2e\x30\x2e\x30"))//0.0.0.0
                return System.Net.IPAddress.Loopback;
            if (Regex.IsMatch(netStatAddress, @"\x31\x32\x37\x2e\x30\x2e\x30\x2e\x31"))//127.0.0.1
                return System.Net.IPAddress.Loopback;
            if (Regex.IsMatch(netStatAddress, @"\x5b\x3a\x3a\x5d")) //[::]
                return System.Net.IPAddress.IPv6Loopback;
            if (Regex.IsMatch(netStatAddress, @"\x5b\x3a\x3a\x31\x5d")) //[::1]
                return System.Net.IPAddress.IPv6Loopback;


            if (netStatAddress.Contains(":"))
            {
                var lastColon = netStatAddress.LastIndexOf(':');
                netStatAddress = netStatAddress.Substring(0, lastColon);
            }

            System.Net.IPAddress ipOut;
            return System.Net.IPAddress.TryParse(netStatAddress, out ipOut) ? ipOut : System.Net.IPAddress.Loopback;

        }

        /// <summary>
        /// This is also intended, but not limited, to be used with the data returned from
        /// calls to netstat.exe.  The <see cref="protocol"/> is required and must be 
        /// either TCP or UDP.  Given a protocol the port number is pulled off the <see cref="netStatAddress"/>
        /// and cross-ref'ed against either the <see cref="CommonTcpPorts"/> or <see cref="CommonUdpPorts"/>.
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="netStatAddress"></param>
        /// <returns></returns>
        public static Tuple<string, string> GetNetStatServiceByPort(string protocol, string netStatAddress)
        {
            const string UDP = "UDP";
            const string TCP = "TCP";
            var emptyResult = new Tuple<string, string>(string.Empty, string.Empty);
            if (string.IsNullOrWhiteSpace(netStatAddress) || string.IsNullOrWhiteSpace(protocol))
                return emptyResult;

            protocol = protocol.Trim();
            netStatAddress = netStatAddress.Trim();

            if(!netStatAddress.Contains(":"))
                return emptyResult;
            var addrParts = netStatAddress.Split(':');
            var strPort = addrParts[(addrParts.Length - 1)];
            int port;
            if (!int.TryParse(strPort, out port))
                return emptyResult;

            if (string.Equals(protocol, UDP, StringComparison.OrdinalIgnoreCase))
            {
                if (CommonUdpPorts.All(udp => udp.Value != port))
                    return emptyResult;
                var udpSvc = CommonUdpPorts.First(udp => udp.Value == port);
                return udpSvc.Key;
            }
            else if (string.Equals(protocol, TCP, StringComparison.OrdinalIgnoreCase))
            {
                if (CommonTcpPorts.All(tcp => tcp.Value != port))
                    return emptyResult;
                var tcpSvc = CommonTcpPorts.First(tcp => tcp.Value == port);
                return tcpSvc.Key;
            }
            return emptyResult;
        }

        /// <summary>
        /// Finds and returns the Uri of a web request according to 
        /// the definition @
        ///   1.  "Message Syntax and Routing" [https://tools.ietf.org/html/rfc7231]
        ///   2.  "Semantics and Content" [https://tools.ietf.org/html/rfc7231]
        ///   3.  "Conditional Requests" [https://tools.ietf.org/html/rfc7232]
        ///   4.  "Range Requests" [https://tools.ietf.org/html/rfc7233]
        ///   5.  "Caching" [https://tools.ietf.org/html/rfc7234]
        ///   6.  "Authentication" [https://tools.ietf.org/html/rfc7235]
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Uri GetRequestUri(byte[] data)
        {
            var stringData = System.Text.Encoding.UTF8.GetString(data);
            if (!stringData.Contains(System.Environment.NewLine))
            {
                return null;
            }
            var dataLines = stringData.Split(new string[] {System.Environment.NewLine},
                                             StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < dataLines.Length;i++ )
            {
                var line = dataLines[i].Trim();
                if (!line.Contains(" ") && line.Split(' ').Length < 2) { continue; }
                foreach (var httpMethod in HttpMethods)
                {
                    if (!line.StartsWith(httpMethod)) { continue; }
                    var expected = line.Split(' ')[1];

                    Uri uriResult = null;
                    if (Uri.TryCreate(expected, UriKind.Absolute, out uriResult))
                    {
                        return uriResult;
                    }

                    if (!Uri.TryCreate(expected, UriKind.Relative, out uriResult) || dataLines.Length == i + 1)
                    {
                        continue;
                    }

                    for(var j = i+1;j<dataLines.Length;j++)
                    {
                        var searchHostLine = dataLines[j].Trim();
                        if(!searchHostLine.StartsWith("Host:")){continue;}

                        searchHostLine = searchHostLine.Replace("Host:", "").Trim();
                        expected = string.Format("{0}{1}{2}{3}",
                                                 Uri.UriSchemeHttp,
                                                 Uri.SchemeDelimiter,
                                                 searchHostLine,
                                                 uriResult.ToString());
                        if(Uri.TryCreate(expected,UriKind.Absolute, out uriResult))
                        {
                            return uriResult;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// http://www.w3.org/Protocols/rfc2616/rfc2616-sec5.html#sec5 section 5.1.1
        /// </summary>
        public static string[] HttpMethods
        {
            get
            {
                return new string[]
                           {
                               "OPTIONS",
                               "GET",
                               "HEAD",
                               "POST",
                               "PUT",
                               "DELETE",
                               "TRACE",
                               "CONNECT"
                           };
            }
        }

        /// <summary>
        /// Gets HTML escape sequences as data bundle of integer value to escape sequence.
        /// <example>
        /// <![CDATA[
        /// 160 = "&nbsp;"
        /// ]]>
        /// </example>
        /// </summary>
        public static Tuple<int, string>[] HtmlEscStrings
        {
            get
            {
                if(_htmlEscStrings == null)
                    _htmlEscStrings = new[]
                       {
                           new Tuple<int, string>(162, "&cent;"),
                           new Tuple<int, string>(161, "&iexcl;"),
                           new Tuple<int, string>(160, "&nbsp;"),
                           new Tuple<int, string>(229, "&aring;"),
                           new Tuple<int, string>(202, "&Ecirc;"),
                           new Tuple<int, string>(227, "&atilde;"),
                           new Tuple<int, string>(197, "&Aring;"),
                           new Tuple<int, string>(225, "&aacute;"),
                           new Tuple<int, string>(255, "&yuml"),
                           new Tuple<int, string>(254, "&thorn;"),
                           new Tuple<int, string>(253, "&yacute;"),
                           new Tuple<int, string>(252, "&uuml;"),
                           new Tuple<int, string>(251, "&ucirc;"),
                           new Tuple<int, string>(250, "&uacute;"),
                           new Tuple<int, string>(249, "&ugrave;"),
                           new Tuple<int, string>(248, "&oslash;"),
                           new Tuple<int, string>(247, "&divide;"),
                           new Tuple<int, string>(246, "&ouml;"),
                           new Tuple<int, string>(245, "&otilde;"),
                           new Tuple<int, string>(244, "&ocirc;"),
                           new Tuple<int, string>(243, "&oacute;"),
                           new Tuple<int, string>(242, "&ograve;"),
                           new Tuple<int, string>(241, "&ntilde;"),
                           new Tuple<int, string>(240, "&eth;"),
                           new Tuple<int, string>(239, "&iuml;"),
                           new Tuple<int, string>(238, "&icirc;"),
                           new Tuple<int, string>(237, "&iacute;"),
                           new Tuple<int, string>(236, "&igrave;"),
                           new Tuple<int, string>(235, "&euml;"),
                           new Tuple<int, string>(234, "&ecirc;"),
                           new Tuple<int, string>(233, "&eacute;"),
                           new Tuple<int, string>(232, "&egrave;"),
                           new Tuple<int, string>(231, "&ccedil;"),
                           new Tuple<int, string>(230, "&aelig;"),
                           new Tuple<int, string>(223, "&szlig;"),
                           new Tuple<int, string>(228, "&auml;"),
                           new Tuple<int, string>(64, "&amp;"),
                           new Tuple<int, string>(226, "&acirc;"),
                           new Tuple<int, string>(62, "&gt;"),
                           new Tuple<int, string>(224, "&agrave;"),
                           new Tuple<int, string>(60, "&lt;"),
                           new Tuple<int, string>(222, "&THORN;"),
                           new Tuple<int, string>(221, "&Yacute;"),
                           new Tuple<int, string>(220, "&Uuml;"),
                           new Tuple<int, string>(219, "&Ucirc;"),
                           new Tuple<int, string>(218, "&Uacute;"),
                           new Tuple<int, string>(217, "&Ugrave;"),
                           new Tuple<int, string>(216, "&Oslash;"),
                           new Tuple<int, string>(215, "&times;"),
                           new Tuple<int, string>(214, "&Ouml;"),
                           new Tuple<int, string>(213, "&Otilde;"),
                           new Tuple<int, string>(212, "&Ocirc;"),
                           new Tuple<int, string>(211, "&Oacute;"),
                           new Tuple<int, string>(210, "&Ograve;"),
                           new Tuple<int, string>(209, "&Ntilde;"),
                           new Tuple<int, string>(208, "&ETH;"),
                           new Tuple<int, string>(207, "&Iuml;"),
                           new Tuple<int, string>(206, "&Icirc;"),
                           new Tuple<int, string>(205, "&Iacute;"),
                           new Tuple<int, string>(204, "&Igrave;"),
                           new Tuple<int, string>(203, "&Euml;"),
                           new Tuple<int, string>(39, "&apos;"),
                           new Tuple<int, string>(201, "&Eacute;"),
                           new Tuple<int, string>(200, "&Egrave;"),
                           new Tuple<int, string>(199, "&Ccedil;"),
                           new Tuple<int, string>(198, "&AElig;"),
                           new Tuple<int, string>(34, "&quot;"),
                           new Tuple<int, string>(196, "&Auml;"),
                           new Tuple<int, string>(195, "&Atilde;"),
                           new Tuple<int, string>(194, "&Acirc;"),
                           new Tuple<int, string>(193, "&Aacute;"),
                           new Tuple<int, string>(192, "&Agrave;"),
                           new Tuple<int, string>(191, "&iquest;"),
                           new Tuple<int, string>(190, "&frac34;"),
                           new Tuple<int, string>(189, "&frac12;"),
                           new Tuple<int, string>(188, "&frac14;"),
                           new Tuple<int, string>(187, "&raquo;"),
                           new Tuple<int, string>(186, "&ordm;"),
                           new Tuple<int, string>(185, "&sup1;"),
                           new Tuple<int, string>(184, "&cedil;"),
                           new Tuple<int, string>(183, "&middot;"),
                           new Tuple<int, string>(182, "&para;"),
                           new Tuple<int, string>(181, "&micro;"),
                           new Tuple<int, string>(180, "&acute;"),
                           new Tuple<int, string>(179, "&sup3;"),
                           new Tuple<int, string>(178, "&sup2;"),
                           new Tuple<int, string>(177, "&plusmn;"),
                           new Tuple<int, string>(176, "&deg;"),
                           new Tuple<int, string>(175, "&macr;"),
                           new Tuple<int, string>(174, "&reg;"),
                           new Tuple<int, string>(173, "&shy;"),
                           new Tuple<int, string>(172, "&not;"),
                           new Tuple<int, string>(171, "&laquo;"),
                           new Tuple<int, string>(170, "&ordf;"),
                           new Tuple<int, string>(169, "&copy;"),
                           new Tuple<int, string>(168, "&uml;"),
                           new Tuple<int, string>(167, "&sect;"),
                           new Tuple<int, string>(166, "&brvbar;"),
                           new Tuple<int, string>(165, "&yen;"),
                           new Tuple<int, string>(164, "&curren;"),
                           new Tuple<int, string>(163, "&pound;")
                       };
                return _htmlEscStrings;
            }
        }

        /// <summary>
        /// Returns the the various schema types associated to socket ports
        /// for the TCP protocal.
        /// The inventory was a one time parse from 
        /// %SYSTEM_ROOT%\System32\drivers\etc\services
        /// </summary>
        public static Dictionary<Tuple<string, string>, int> CommonTcpPorts
        {
            get
            {
                if (_tcpPorts == null)
                    _tcpPorts = new Dictionary<Tuple<string, string>, int>
                    {
                        {new Tuple<string, string>("echo", ""), 7},
                        {new Tuple<string, string>("discard", ""), 9},
                        {new Tuple<string, string>("systat", "Active users"), 11},
                        {new Tuple<string, string>("daytime", ""), 13},
                        {new Tuple<string, string>("qotd", "Quote of the day"), 17},
                        {new Tuple<string, string>("chargen", "Character generator"), 19},
                        {new Tuple<string, string>("ftp-data", "FTP, data"), 20},
                        {new Tuple<string, string>("ftp", "FTP. control"), 21},
                        {new Tuple<string, string>("ssh", "SSH Remote Login Protocol"), 22},
                        {new Tuple<string, string>("telnet", ""), 23},
                        {new Tuple<string, string>("smtp", "Simple Mail Transfer Protocol"), 25},
                        {new Tuple<string, string>("time", ""), 37},
                        {new Tuple<string, string>("nameserver", "Host Name Server"), 42},
                        {new Tuple<string, string>("nicname", ""), 43},
                        {new Tuple<string, string>("domain", "Domain Name Server"), 53},
                        {new Tuple<string, string>("gopher", ""), 70},
                        {new Tuple<string, string>("finger", ""), 79},
                        {new Tuple<string, string>("http", "World Wide Web"), 80},
                        {new Tuple<string, string>("hosts2-ns", "HOSTS2 Name Server"), 81},
                        {new Tuple<string, string>("kerberos", "Kerberos"), 88},
                        {new Tuple<string, string>("hostname", "NIC Host Name Server"), 101},
                        {new Tuple<string, string>("iso-tsap", "ISO-TSAP Class 0"), 102},
                        {new Tuple<string, string>("rtelnet", "Remote Telnet Service"), 107},
                        {new Tuple<string, string>("pop2", "Post Office Protocol - Version 2"), 109},
                        {new Tuple<string, string>("pop3", "Post Office Protocol - Version 3"), 110},
                        {new Tuple<string, string>("sunrpc", "SUN Remote Procedure Call"), 111},
                        {new Tuple<string, string>("auth", "Identification Protocol"), 113},
                        {new Tuple<string, string>("uucp-path", ""), 117},
                        {new Tuple<string, string>("sqlserv", "SQL Services"), 118},
                        {new Tuple<string, string>("nntp", "Network News Transfer Protocol"), 119},
                        {new Tuple<string, string>("epmap", "DCE endpoint resolution"), 135},
                        {new Tuple<string, string>("netbios-ns", "NETBIOS Name Service"), 137},
                        {new Tuple<string, string>("netbios-ssn", "NETBIOS Session Service"), 139},
                        {new Tuple<string, string>("imap", "Internet Message Access Protocol"), 143},
                        {new Tuple<string, string>("sql-net", ""), 150},
                        {new Tuple<string, string>("sqlsrv", ""), 156},
                        {new Tuple<string, string>("pcmail-srv", "PCMail Server"), 158},
                        {new Tuple<string, string>("print-srv", "Network PostScript"), 170},
                        {new Tuple<string, string>("bgp", "Border Gateway Protocol"), 179},
                        {new Tuple<string, string>("irc", "Internet Relay Chat Protocol "), 194},
                        {new Tuple<string, string>("rtsps", ""), 322},
                        {new Tuple<string, string>("mftp", ""), 349},
                        {new Tuple<string, string>("ldap", "Lightweight Directory Access Protocol"), 389},
                        {new Tuple<string, string>("https", "HTTP over TLS/SSL"), 443},
                        {new Tuple<string, string>("microsoft-ds", ""), 445},
                        {new Tuple<string, string>("kpasswd", " Kerberos (v5)"), 464},
                        {new Tuple<string, string>("crs", "Content Replication System"), 507},
                        {new Tuple<string, string>("exec", "Remote Process Execution"), 512},
                        {new Tuple<string, string>("login", "Remote Login"), 513},
                        {new Tuple<string, string>("cmd", ""), 514},
                        {new Tuple<string, string>("printer", ""), 515},
                        {new Tuple<string, string>("efs", "Extended File Name Server"), 520},
                        {new Tuple<string, string>("ulp", ""), 522},
                        {new Tuple<string, string>("tempo", ""), 526},
                        {new Tuple<string, string>("irc-serv", ""), 529},
                        {new Tuple<string, string>("courier", ""), 530},
                        {new Tuple<string, string>("conference", ""), 531},
                        {new Tuple<string, string>("netnews", ""), 532},
                        {new Tuple<string, string>("uucp", ""), 540},
                        {new Tuple<string, string>("klogin", "Kerberos login"), 543},
                        {new Tuple<string, string>("kshell", "Kerberos remote shell"), 544},
                        {new Tuple<string, string>("dhcpv6-client", "DHCPv6 Client"), 546},
                        {new Tuple<string, string>("dhcpv6-server", "DHCPv6 Server"), 547},
                        {new Tuple<string, string>("afpovertcp", "AFP over TCP"), 548},
                        {new Tuple<string, string>("rtsp", "Real Time Stream Control Protocol"), 554},
                        {new Tuple<string, string>("remotefs", ""), 556},
                        {new Tuple<string, string>("nntps", "NNTP over TLS/SSL"), 563},
                        {new Tuple<string, string>("whoami", ""), 565},
                        {new Tuple<string, string>("ms-shuttle", "Microsoft shuttle"), 568},
                        {new Tuple<string, string>("ms-rome", "Microsoft rome"), 569},
                        {new Tuple<string, string>("http-rpc-epmap", "HTTP RPC Ep Map"), 593},
                        {new Tuple<string, string>("hmmp-ind", "HMMP Indication"), 612},
                        {new Tuple<string, string>("hmmp-op", "HMMP Operation"), 613},
                        {new Tuple<string, string>("ldaps", "LDAP over TLS/SSL"), 636},
                        {new Tuple<string, string>("doom", "Doom Id Software"), 666},
                        {new Tuple<string, string>("msexch-routing", "MS Exchange Routing"), 691},
                        {new Tuple<string, string>("kerberos-adm", "Kerberos administration"), 749},
                        {new Tuple<string, string>("mdbs_daemon", ""), 800},
                        {new Tuple<string, string>("ftps-data", "FTP data, over TLS/SSL"), 989},
                        {new Tuple<string, string>("ftps", "FTP control, over TLS/SSL"), 990},
                        {new Tuple<string, string>("telnets", "Telnet protocol over TLS/SSL"), 992},
                        {new Tuple<string, string>("imaps", "IMAP4 protocol over TLS/SSL"), 993},
                        {new Tuple<string, string>("ircs", "IRC protocol over TLS/SSL"), 994},
                        {new Tuple<string, string>("pop3s", "pop3 protocol over TLS/SSL (was spop3)"), 995},
                        {new Tuple<string, string>("kpop", "Kerberos POP"), 1109},
                        {new Tuple<string, string>("nfsd-status", "Cluster status info"), 1110},
                        {new Tuple<string, string>("nfa", "Network File Access"), 1155},
                        {new Tuple<string, string>("activesync", "ActiveSync Notifications"), 1034},
                        {new Tuple<string, string>("opsmgr", "Microsoft Operations Manager"), 1270},
                        {new Tuple<string, string>("ms-sql-s", "Microsoft-SQL-Server "), 1433},
                        {new Tuple<string, string>("ms-sql-m", "Microsoft-SQL-Monitor"), 1434},
                        {new Tuple<string, string>("ms-sna-server", ""), 1477},
                        {new Tuple<string, string>("ms-sna-base", ""), 1478},
                        {new Tuple<string, string>("wins", "Microsoft Windows Internet Name Service"), 1512},
                        {new Tuple<string, string>("ingreslock", ""), 1524},
                        {new Tuple<string, string>("stt", ""), 1607},
                        {new Tuple<string, string>("pptconference", ""), 1711},
                        {new Tuple<string, string>("pptp", "Point-to-point tunnelling protocol"), 1723},
                        {new Tuple<string, string>("msiccp", ""), 1731},
                        {new Tuple<string, string>("remote-winsock", ""), 1745},
                        {new Tuple<string, string>("ms-streaming", ""), 1755},
                        {new Tuple<string, string>("msmq", "Microsoft Message Queue"), 1801},
                        {new Tuple<string, string>("msnp", ""), 1863},
                        {new Tuple<string, string>("ssdp", ""), 1900},
                        {new Tuple<string, string>("close-combat", ""), 1944},
                        {new Tuple<string, string>("knetd", "Kerberos de-multiplexor"), 2053},
                        {new Tuple<string, string>("mzap", "Multicast-Scope Zone Announcement Protocol"), 2106},
                        {new Tuple<string, string>("qwave", "QWAVE"), 2177},
                        {new Tuple<string, string>("directplay", "DirectPlay"), 2234},
                        {new Tuple<string, string>("ms-olap3", "Microsoft OLAP 3"), 2382},
                        {new Tuple<string, string>("ms-olap4", "Microsoft OLAP 4"), 2383},
                        {new Tuple<string, string>("ms-olap1", "Microsoft OLAP 1"), 2393},
                        {new Tuple<string, string>("ms-olap2", "Microsoft OLAP 2"), 2394},
                        {new Tuple<string, string>("ms-theater", ""), 2460},
                        {new Tuple<string, string>("wlbs", "Microsoft Windows Load Balancing Server"), 2504},
                        {new Tuple<string, string>("ms-v-worlds", "Microsoft V-Worlds "), 2525},
                        {new Tuple<string, string>("sms-rcinfo", "SMS RCINFO"), 2701},
                        {new Tuple<string, string>("sms-xfer", "SMS XFER"), 2702},
                        {new Tuple<string, string>("sms-chat", "SMS CHAT"), 2703},
                        {new Tuple<string, string>("sms-remctrl", "SMS REMCTRL"), 2704},
                        {new Tuple<string, string>("msolap-ptp2", "MSOLAP PTP2"), 2725},
                        {new Tuple<string, string>("icslap", ""), 2869},
                        {new Tuple<string, string>("cifs", ""), 3020},
                        {new Tuple<string, string>("xbox", "Microsoft Xbox game port"), 3074},
                        {new Tuple<string, string>("ms-dotnetster", "Microsoft .NET ster port"), 3126},
                        {
                            new Tuple<string, string>("ms-rule-engine", "Microsoft Business Rule Engine Update Service"),
                            3132
                        },
                        {new Tuple<string, string>("msft-gc", "Microsoft Global Catalog"), 3268},
                        {new Tuple<string, string>("msft-gc-ssl", "Microsoft Global Catalog with LDAP/SSL"), 3269},
                        {new Tuple<string, string>("ms-cluster-net", "Microsoft Cluster Net"), 3343},
                        {new Tuple<string, string>("ms-wbt-server", "MS WBT Server"), 3389},
                        {new Tuple<string, string>("ms-la", "Microsoft Class Server"), 3535},
                        {new Tuple<string, string>("pnrp-port", "PNRP User Port"), 3540},
                        {new Tuple<string, string>("teredo", "Teredo Port"), 3544},
                        {new Tuple<string, string>("p2pgroup", "Peer to Peer Grouping"), 3587},
                        {new Tuple<string, string>("ws-discovery", "WS-Discovery"), 3702},
                        {new Tuple<string, string>("dvcprov-port", "Device Provisioning Port"), 3776},
                        {new Tuple<string, string>("msfw-control", "Microsoft Firewall Control"), 3847},
                        {new Tuple<string, string>("msdts1", "DTS Service Port"), 3882},
                        {new Tuple<string, string>("sdp-portmapper", "SDP Port Mapper Protocol"), 3935},
                        {new Tuple<string, string>("net-device", "Net Device"), 4350},
                        {new Tuple<string, string>("epmd", "Erlang Port Mapper Daemon"), 4369},
                        {new Tuple<string, string>("ipsec-msft", "Microsoft IPsec NAT-T"), 4500},
                        {new Tuple<string, string>("sip","Session Initiation Protocol over TLS"), 5061},
                        {new Tuple<string, string>("llmnr", "LLMNR "), 5355},
                        {new Tuple<string, string>("wsd", "Web Services on devices "), 5357},
                        {new Tuple<string, string>("wsd", "Web Services on devices"), 5358},
                        {new Tuple<string, string>("rrac", "Remote Replication Agent Connection"), 5678},
                        {new Tuple<string, string>("dccm", "Direct Cable Connect Manager"), 5679},
                        {new Tuple<string, string>("ms-licensing", "Microsoft Licensing"), 5720},
                        {new Tuple<string, string>("directplay8", "DirectPlay8"), 6073},
                        {new Tuple<string, string>("man", "Remote Man Server"), 9535},
                        {new Tuple<string, string>("rasadv", ""), 9753},
                        {new Tuple<string, string>("imip-channels", "IMIP Channels Port"), 11320},
                        {new Tuple<string, string>("directplaysrvr", "Direct Play Server"), 47624},
                        {new Tuple<string, string>("azure","Microsoft Azure"), 5671},
                        {new Tuple<string, string>("erl","RabbitMq SSL Main Port"), 5672},
                        {new Tuple<string, string>("proxy-server","Unofficial Proxy Server"), 9090}
                        
                    };
                return _tcpPorts;
            }
        }

        /// <summary>
        /// Returns the the various schema types associated to socket ports
        /// for the UDP protocal.
        /// The inventory was a one time parse from 
        /// %SYSTEM_ROOT%\System32\drivers\etc\services
        /// </summary>
        public static Dictionary<Tuple<string, string>, int> CommonUdpPorts
        {
            get
            {
                if (_udpPorts == null)
                    _udpPorts = new Dictionary<Tuple<string, string>, int>
                    {
                        {new Tuple<string, string>("echo", ""), 7},
                        {new Tuple<string, string>("discard", ""), 9},
                        {new Tuple<string, string>("systat", "Active users"), 11},
                        {new Tuple<string, string>("daytime", ""), 13},
                        {new Tuple<string, string>("qotd", "Quote of the day"), 17},
                        {new Tuple<string, string>("chargen", "Character generator"), 19},
                        {new Tuple<string, string>("time", ""), 37},
                        {new Tuple<string, string>("rlp", "Resource Location Protocol"), 39},
                        {new Tuple<string, string>("nameserver", "Host Name Server"), 42},
                        {new Tuple<string, string>("domain", "Domain Name Server"), 53},
                        {new Tuple<string, string>("bootps", "Bootstrap Protocol Server"), 67},
                        {new Tuple<string, string>("bootpc", "Bootstrap Protocol Client"), 68},
                        {new Tuple<string, string>("tftp", "Trivial File Transfer"), 69},
                        {new Tuple<string, string>("hosts2-ns", "HOSTS2 Name Server"), 81},
                        {new Tuple<string, string>("kerberos", "Kerberos"), 88},
                        {new Tuple<string, string>("sunrpc", "SUN Remote Procedure Call"), 111},
                        {new Tuple<string, string>("ntp", "Network Time Protocol"), 123},
                        {new Tuple<string, string>("epmap", "DCE endpoint resolution"), 135},
                        {new Tuple<string, string>("netbios-ns", "NETBIOS Name Service"), 137},
                        {new Tuple<string, string>("netbios-dgm", "NETBIOS Datagram Service"), 138},
                        {new Tuple<string, string>("snmp", "SNMP"), 161},
                        {new Tuple<string, string>("snmptrap", "SNMP trap"), 162},
                        {new Tuple<string, string>("ipx", "IPX over IP"), 213},
                        {new Tuple<string, string>("rtsps", ""), 322},
                        {new Tuple<string, string>("mftp", ""), 349},
                        {new Tuple<string, string>("https", "HTTP over TLS/SSL"), 443},
                        {new Tuple<string, string>("microsoft-ds", ""), 445},
                        {new Tuple<string, string>("kpasswd", " Kerberos (v5)"), 464},
                        {new Tuple<string, string>("isakmp", "Internet Key Exchange"), 500},
                        {new Tuple<string, string>("crs", "Content Replication System"), 507},
                        {new Tuple<string, string>("biff", ""), 512},
                        {new Tuple<string, string>("who", ""), 513},
                        {new Tuple<string, string>("syslog", ""), 514},
                        {new Tuple<string, string>("talk", ""), 517},
                        {new Tuple<string, string>("ntalk", ""), 518},
                        {new Tuple<string, string>("router", ""), 520},
                        {new Tuple<string, string>("ulp", ""), 522},
                        {new Tuple<string, string>("timed", ""), 525},
                        {new Tuple<string, string>("irc-serv", ""), 529},
                        {new Tuple<string, string>("netwall", "For emergency broadcasts"), 533},
                        {new Tuple<string, string>("dhcpv6-client", "DHCPv6 Client"), 546},
                        {new Tuple<string, string>("dhcpv6-server", "DHCPv6 Server"), 547},
                        {new Tuple<string, string>("afpovertcp", "AFP over TCP"), 548},
                        {new Tuple<string, string>("new-rwho", ""), 550},
                        {new Tuple<string, string>("rtsp", "Real Time Stream Control Protocol"), 554},
                        {new Tuple<string, string>("rmonitor", ""), 560},
                        {new Tuple<string, string>("monitor", ""), 561},
                        {new Tuple<string, string>("nntps", "NNTP over TLS/SSL"), 563},
                        {new Tuple<string, string>("whoami", ""), 565},
                        {new Tuple<string, string>("ms-shuttle", "Microsoft shuttle"), 568},
                        {new Tuple<string, string>("ms-rome", "Microsoft rome"), 569},
                        {new Tuple<string, string>("http-rpc-epmap", "HTTP RPC Ep Map"), 593},
                        {new Tuple<string, string>("hmmp-ind", "HMMP Indication"), 612},
                        {new Tuple<string, string>("hmmp-op", "HMMP Operation"), 613},
                        {new Tuple<string, string>("doom", "Doom Id Software"), 666},
                        {new Tuple<string, string>("msexch-routing", "MS Exchange Routing"), 691},
                        {new Tuple<string, string>("kerberos-adm", "Kerberos administration"), 749},
                        {new Tuple<string, string>("kerberos-iv", "Kerberos version IV"), 750},
                        {new Tuple<string, string>("mdbs_daemon", ""), 800},
                        {new Tuple<string, string>("pop3s", "pop3 protocol over TLS/SSL (was spop3)"), 995},
                        {new Tuple<string, string>("nfsd-keepalive", "Client status info"), 1110},
                        {new Tuple<string, string>("nfa", "Network File Access"), 1155},
                        {new Tuple<string, string>("phone", "Conference calling"), 1167},
                        {new Tuple<string, string>("opsmgr", "Microsoft Operations Manager"), 1270},
                        {new Tuple<string, string>("ms-sql-s", "Microsoft-SQL-Server "), 1433},
                        {new Tuple<string, string>("ms-sql-m", "Microsoft-SQL-Monitor "), 1434},
                        {new Tuple<string, string>("ms-sna-server", ""), 1477},
                        {new Tuple<string, string>("ms-sna-base", ""), 1478},
                        {new Tuple<string, string>("wins", "Microsoft Windows Internet Name Service"), 1512},
                        {new Tuple<string, string>("stt", ""), 1607},
                        {new Tuple<string, string>("l2tp", "Layer Two Tunneling Protocol"), 1701},
                        {new Tuple<string, string>("pptconference", ""), 1711},
                        {new Tuple<string, string>("msiccp", ""), 1731},
                        {new Tuple<string, string>("remote-winsock", ""), 1745},
                        {new Tuple<string, string>("ms-streaming", ""), 1755},
                        {new Tuple<string, string>("msmq", "Microsoft Message Queue"), 1801},
                        {new Tuple<string, string>("radius", "RADIUS authentication protocol"), 1812},
                        {new Tuple<string, string>("radacct", "RADIUS accounting protocol"), 1813},
                        {new Tuple<string, string>("msnp", ""), 1863},
                        {new Tuple<string, string>("ssdp", ""), 1900},
                        {new Tuple<string, string>("close-combat", ""), 1944},
                        {new Tuple<string, string>("nfsd", "NFS server"), 2049},
                        {new Tuple<string, string>("mzap", "Multicast-Scope Zone Announcement Protocol"), 2106},
                        {new Tuple<string, string>("qwave", "QWAVE Experiment Port"), 2177},
                        {new Tuple<string, string>("directplay", "DirectPlay"), 2234},
                        {new Tuple<string, string>("ms-olap3", "Microsoft OLAP 3"), 2382},
                        {new Tuple<string, string>("ms-olap4", "Microsoft OLAP 4"), 2383},
                        {new Tuple<string, string>("ms-olap1", "Microsoft OLAP 1"), 2393},
                        {new Tuple<string, string>("ms-olap2", "Microsoft OLAP 2"), 2394},
                        {new Tuple<string, string>("ms-theater", ""), 2460},
                        {new Tuple<string, string>("wlbs", "Microsoft Windows Load Balancing Server"), 2504},
                        {new Tuple<string, string>("ms-v-worlds", "Microsoft V-Worlds "), 2525},
                        {new Tuple<string, string>("sms-rcinfo", "SMS RCINFO"), 2701},
                        {new Tuple<string, string>("sms-xfer", "SMS XFER"), 2702},
                        {new Tuple<string, string>("sms-chat", "SMS CHAT"), 2703},
                        {new Tuple<string, string>("sms-remctrl", "SMS REMCTRL"), 2704},
                        {new Tuple<string, string>("msolap-ptp2", "MSOLAP PTP2"), 2725},
                        {new Tuple<string, string>("icslap", ""), 2869},
                        {new Tuple<string, string>("cifs", ""), 3020},
                        {new Tuple<string, string>("xbox", "Microsoft Xbox game port"), 3074},
                        {new Tuple<string, string>("ms-dotnetster", "Microsoft .NET ster port"), 3126},
                        {
                            new Tuple<string, string>("ms-rule-engine", "Microsoft Business Rule Engine Update Service"),
                            3132
                        },
                        {new Tuple<string, string>("msft-gc", "Microsoft Global Catalog"), 3268},
                        {new Tuple<string, string>("msft-gc-ssl", "Microsoft Global Catalog with LDAP/SSL"), 3269},
                        {new Tuple<string, string>("ms-cluster-net", "Microsoft Cluster Net"), 3343},
                        {new Tuple<string, string>("ms-wbt-server", "MS WBT Server"), 3389},
                        {new Tuple<string, string>("ms-la", "Microsoft Class Server"), 3535},
                        {new Tuple<string, string>("pnrp-port", "PNRP User Port"), 3540},
                        {new Tuple<string, string>("teredo", "Teredo Port"), 3544},
                        {new Tuple<string, string>("p2pgroup", "Peer to Peer Grouping"), 3587},
                        {new Tuple<string, string>("ws-discovery", "WS-Discovery"), 3702},
                        {new Tuple<string, string>("dvcprov-port", "Device Provisioning Port"), 3776},
                        {new Tuple<string, string>("sdp-portmapper", "SDP Port Mapper Protocol"), 3935},
                        {new Tuple<string, string>("net-device", "Net Device"), 4350},
                        {new Tuple<string, string>("ipsec-msft", "Microsoft IPsec NAT-T"), 4500},
                        {new Tuple<string, string>("llmnr", "LLMNR "), 5355},
                        {new Tuple<string, string>("rrac", "Remote Replication Agent Connection"), 5678},
                        {new Tuple<string, string>("dccm", "Direct Cable Connect Manager"), 5679},
                        {new Tuple<string, string>("ms-licensing", "Microsoft Licensing "), 5720},
                        {new Tuple<string, string>("directplay8", "DirectPlay8"), 6073},
                        {new Tuple<string, string>("rasadv", ""), 9753},
                        {new Tuple<string, string>("imip-channels", "IMIP Channels Port"), 11320},
                        {new Tuple<string, string>("directplaysrvr", "Direct Play Server"), 47624}
                    };
                return _udpPorts;
            }
        }

        /// <summary>
        /// String values used in calls to Google Places API
        /// </summary>
        public static string[] GooglePlaces
        {
            get
            {
                return new[]
                {
                    "accounting", "airport", "amusement_park", "aquarium",
                    "art_gallery", "atm", "bakery", "bank",
                    "bar", "beauty_salon", "bicycle_store", "book_store",
                    "bowling_alley", "bus_station", "cafe", "campground",
                    "car_dealer", "car_rental", "car_repair", "car_wash",
                    "casino", "cemetery", "church", "city_hall",
                    "clothing_store", "convenience_store", "courthouse", "dentist",
                    "department_store", "doctor", "electrician", "electronics_store",
                    "embassy", "establishment", "finance", "fire_station",
                    "florist", "food", "funeral_home", "furniture_store",
                    "gas_station", "general_contractor", "grocery_or_supermarket", "gym",
                    "hair_care", "hardware_store", "health", "hindu_temple",
                    "home_goods_store", "hospital", "insurance_agency", "jewelry_store",
                    "laundry", "lawyer", "library", "liquor_store",
                    "local_government_office", "locksmith", "lodging", "meal_delivery",
                    "meal_takeaway", "mosque", "movie_rental", "movie_theater",
                    "moving_company", "museum", "night_club", "painter",
                    "park", "parking", "pet_store", "pharmacy",
                    "physiotherapist", "place_of_worship", "plumber", "police",
                    "post_office", "real_estate_agency", "restaurant", "roofing_contractor",
                    "rv_park", "school", "shoe_store", "shopping_mall",
                    "spa", "stadium", "storage", "store",
                    "subway_station", "synagogue", "taxi_stand", "train_station",
                    "travel_agency", "university", "veterinary_care", "zoo"
                };
            }
        }
    }//end Net
}//end Util
