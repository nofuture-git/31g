using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NoFuture.Rand.Core;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Tele
{
    public class Net
    {
        #region fields
        internal const string WEB_MAIL_DOMAINS = "webmailDomains.txt";
        private static string[] _webMaildomains;
        #endregion

        /// <summary>
        /// Src [https://bitquark.co.uk/blog/2016/02/29/the_most_popular_subdomains_on_the_internet]
        /// </summary>
        public static string[] Subdomains { get; } = {
            "www", "mail", "remote", "blog",
            "webmail", "server", "ns1", "ns2",
            "smtp", "secure", "vpn", "m", "shop",
            "ftp", "mail2", "test", "portal", "ns",
            "ww1", "host", "support", "dev", "web",
            "bbs", "ww42", "mx", "email", "cloud", "1",
            "mail1", "2", "forum", "owa", "www2", "gw",
            "admin","store", "mx1", "cdn", "api",
            "exchange","app", "gov", "2tty", "vps",
            "news"
        };

        /// <summary>
        /// Loads web mail domains into an array.
        /// Src [https://github.com/tarr11/Webmail-Domains/blob/master/domains.txt]
        /// </summary>
        public static string[] WebmailDomains
        {
            get
            {
                if (_webMaildomains != null && _webMaildomains.Length > 0)
                    return _webMaildomains;

                var asm = Assembly.GetExecutingAssembly();

                var data = asm.GetManifestResourceStream($"{asm.GetName().Name}.Data.{WEB_MAIL_DOMAINS}");
                if (data == null)
                    return null;

                var strmRdr = new StreamReader(data);
                var webmailData = strmRdr.ReadToEnd();
                if (string.IsNullOrWhiteSpace(webmailData))
                    return null;

                _webMaildomains = webmailData.Split(Constants.LF).ToArray();
                return _webMaildomains;

            }
        }

        /// <summary>
        /// Loads only the common web mail domains in the US
        /// Src [https://github.com/mailcheck/mailcheck/wiki/List-of-Popular-Domains]
        /// </summary>
        public static string[] UsWebmailDomains { get; } = {
            "aol.com", "att.net", "comcast.net", "facebook.com", "gmail.com",
            "gmx.com", "googlemail.com","google.com", "hotmail.com", "mac.com",
            "me.com", "mail.com", "msn.com","live.com", "sbcglobal.net",
            "verizon.net", "yahoo.com","bellsouth.net", "charter.net", "cox.net",
            "earthlink.net", "juno.com"
        };

        /// <summary>
        /// Gets a random Uri host.
        /// </summary>
        /// <returns></returns>
        public static string RandomUriHost(bool withSubDomain = true, bool usCommonOnly = false)
        {
            var webDomains = usCommonOnly ? UsWebmailDomains : WebmailDomains;
            var host = new StringBuilder();

            if (withSubDomain)
            {
                var subdomain = Etx.DiscreteRange(Subdomains);
                host.Append(subdomain + ".");
            }

            if (webDomains != null)
            {
                host.Append(webDomains[Etx.IntNumber(0, webDomains.Length - 1)]);
            }
            else
            {
                host.Append(Etx.Word());
                host.Append(Etx.DiscreteRange(new[] { ".com", ".net", ".edu", ".org" }));
            }
            return host.ToString();
        }

        /// <summary>
        /// Create a random http scheme uri with optional query string.
        /// </summary>
        /// <param name="useHttps"></param>
        /// <param name="addQry"></param>
        /// <returns></returns>
        public static Uri RandomHttpUri(bool useHttps = false, bool addQry = false)
        {

            var pathSeg = new List<string>();
            var pathSegLen = Etx.IntNumber(0, 5);
            for (var i = 0; i < pathSegLen; i++)
            {
                Etx.DiscreteRange(new Dictionary<string, double>()
                {
                    {Etx.Word(), 72},
                    {Etx.Consonant(false).ToString(), 11},
                    {Etx.IntNumber(1, 9999).ToString(), 17}
                });
                pathSeg.Add(Etx.Word());
            }

            if (Etx.CoinToss)
            {
                pathSeg.Add(Etx.Word() + Etx.DiscreteRange(new[] { ".php", ".aspx", ".html", ".txt", ".asp" }));
            }

            var uri = new UriBuilder
            {
                Scheme = useHttps ? "https" : "http",
                Host = RandomUriHost(),
                Path = String.Join("/", pathSeg.ToArray())
            };

            if (!addQry)
                return uri.Uri;

            var qry = new List<string>();
            var qryParms = Etx.IntNumber(1, 5);
            for (var i = 0; i < qryParms; i++)
            {
                var len = Etx.IntNumber(1, 4);
                var qryParam = new List<string>();
                for (var j = 0; j < len; j++)
                {
                    if (Etx.CoinToss)
                    {
                        qryParam.Add(Etx.Word());
                        continue;
                    }
                    if (Etx.CoinToss)
                    {
                        qryParam.Add(Etx.IntNumber(0, 99999).ToString());
                        continue;
                    }
                    qryParam.Add(Etx.Consonant(Etx.CoinToss).ToString());

                }
                qry.Add(String.Join("_", qryParam) + "=" + Etx.SupriseMe());
            }

            uri.Query = String.Join("&", qry);
            return uri.Uri;
        }
    }
}
