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
    [Serializable]
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
                if (String.IsNullOrWhiteSpace(webmailData))
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
        [RandomFactory]
        public static string RandomUriHost(bool withSubDomain = true, bool usCommonOnly = false)
        {
            var webDomains = usCommonOnly ? UsWebmailDomains : WebmailDomains;
            var host = new StringBuilder();

            if (withSubDomain)
            {
                var subdomain = Etx.RandomPickOne(Subdomains);
                host.Append(subdomain + ".");
            }

            if (webDomains != null)
            {
                host.Append(webDomains[Etx.RandomInteger(0, webDomains.Length - 1)]);
            }
            else
            {
                host.Append(Etx.RandomWord());
                host.Append(Etx.RandomPickOne(new[] { ".com", ".net", ".edu", ".org" }));
            }
            return host.ToString();
        }

        /// <summary>
        /// Create a random http scheme uri with optional query string.
        /// </summary>
        /// <param name="useHttps"></param>
        /// <param name="addQry"></param>
        /// <returns></returns>
        [RandomFactory]
        public static string RandomHttpUri(bool useHttps = false, bool addQry = false)
        {

            var pathSeg = new List<string>();
            var pathSegLen = Etx.RandomInteger(0, 5);
            for (var i = 0; i < pathSegLen; i++)
            {
                Etx.RandomPickOne(new Dictionary<string, double>()
                {
                    {Etx.RandomWord(), 72},
                    {Etx.RandomConsonant(false).ToString(), 11},
                    {Etx.RandomInteger(1, 9999).ToString(), 17}
                });
                pathSeg.Add(Etx.RandomWord());
            }

            if (Etx.RandomCoinToss())
            {
                pathSeg.Add(Etx.RandomWord() + Etx.RandomPickOne(new[] { ".php", ".aspx", ".html", ".txt", ".asp" }));
            }

            var uri = new UriBuilder
            {
                Scheme = useHttps ? "https" : "http",
                Host = RandomUriHost(),
                Path = String.Join("/", pathSeg.ToArray())
            };

            if (!addQry)
                return uri.ToString();

            var qry = new List<string>();
            var qryParms = Etx.RandomInteger(1, 5);
            for (var i = 0; i < qryParms; i++)
            {
                var len = Etx.RandomInteger(1, 4);
                var qryParam = new List<string>();
                for (var j = 0; j < len; j++)
                {
                    if (Etx.RandomCoinToss())
                    {
                        qryParam.Add(Etx.RandomWord());
                        continue;
                    }
                    if (Etx.RandomCoinToss())
                    {
                        qryParam.Add(Etx.RandomInteger(0, 99999).ToString());
                        continue;
                    }
                    qryParam.Add(Etx.RandomConsonant(Etx.RandomCoinToss()).ToString());

                }
                qry.Add(String.Join("_", qryParam) + "=" + Uri.EscapeDataString(Etx.RandomSuprise()));
            }
            
            uri.Query = String.Join("&", qry);
            return uri.ToString();
        }

        /// <summary>
        /// Create a computed application user name at random 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        [RandomFactory]
        public static string RandomUsername(string firstName = null, string lastName = null)
        {
            var username = new List<string>();
            firstName = firstName ?? Etx.RandomWord();
            lastName = lastName ?? Etx.RandomWord();
            username.Add(firstName);

            var hasDigit = Etx.RandomDouble() <= 0.2797293477;

            if (hasDigit)
            {
                var trailingDigits = Etx.RandomInteger(0, 9).ToString();
                var digitProbTable = new[] { 0.219754472, 0.134423069, 0.101839372, 0.056182959 };
                foreach (var dpt in digitProbTable)
                {
                    var addAnotherDigit = Etx.RandomDouble() <= dpt;
                    if (addAnotherDigit)
                    {
                        trailingDigits += Etx.RandomInteger(0, 9).ToString();
                    }
                    else
                    {
                        break;
                    }
                }

                username.Add(lastName + trailingDigits);
            }
            else
            {
                username.Add(lastName);
            }

            var sepTable = new Dictionary<string, double>
            {
                {"", 0.521730559},
                {".", 0.391696793},
                {"_", 0.073830063},
                {"-", 0.012742585}
            };
            var sep = Etx.RandomPickOne(sepTable);
            return String.Join(sep, username);
        }
    }
}
