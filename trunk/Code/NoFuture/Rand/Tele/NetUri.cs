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
    /// <inheritdoc cref="LabeledIdentifier" />
    /// <summary>
    /// NoFuture Rand container for the general-use URI entity
    /// </summary>
    [Serializable]
    public class NetUri : LabeledIdentifier
    {
        #region fields
        internal const string WEB_MAIL_DOMAINS = "webmailDomains.txt";
        internal const string SUBDOMAINS = "subdomains.txt";
        internal const string US_WEBMAIL_DOMAINS = "usWebmailDomains.txt";
        internal const string CHILDISH_USER_NAMES = "childishUserNames.txt";
        private static string[] _webMaildomains;
        private static string[] _subDomains;
        private static string[] _usWebmailDomains;
        private static string[] _childishUserNames;
        #endregion

        public string Scheme => ToUri()?.Scheme;

        public override string Abbrev => "Uri";

        public virtual Uri ToUri()
        {
            return string.IsNullOrWhiteSpace(Value) ? null : new Uri(Value);
        }

        /// <summary>
        /// Src [https://bitquark.co.uk/blog/2016/02/29/the_most_popular_subdomains_on_the_internet]
        /// </summary>
        public static string[] Subdomains
        {
            get
            {
                if (_subDomains != null && _subDomains.Length > 0)
                    return _subDomains;
                _subDomains = ReadTextFileData(SUBDOMAINS);
                return _subDomains;
            }
        }

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
                _webMaildomains = ReadTextFileData(WEB_MAIL_DOMAINS);
                return _webMaildomains;
            }
        }

        /// <summary>
        /// Loads only the common web mail domains in the US
        /// Src [https://github.com/mailcheck/mailcheck/wiki/List-of-Popular-Domains]
        /// </summary>
        public static string[] UsWebmailDomains
        {
            get
            {
                if (_usWebmailDomains != null && _usWebmailDomains.Length > 0)
                    return _usWebmailDomains;
                _usWebmailDomains = ReadTextFileData(US_WEBMAIL_DOMAINS);
                return _usWebmailDomains;
            }
        }

        /// <summary>
        /// These are just made-up to look silly
        /// </summary>
        public static string[] ChildishUserNames
        {
            get
            {
                if (_childishUserNames != null && _childishUserNames.Any())
                    return _childishUserNames;
                _childishUserNames = ReadTextFileData(CHILDISH_USER_NAMES)
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .ToArray();
                return _childishUserNames;
            }
        }

        internal static string[] ReadTextFileData(string name)
        {
            var asm = Assembly.GetExecutingAssembly();

            var data = asm.GetManifestResourceStream($"{asm.GetName().Name}.Data.{name}");
            if (data == null)
                return null;

            var strmRdr = new StreamReader(data);
            var webmailData = strmRdr.ReadToEnd();
            if (string.IsNullOrWhiteSpace(webmailData))
                return null;

            var txtData = webmailData.Split(Constants.LF).ToArray();
            return txtData;
        }

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
                qry.Add(String.Join("_", qryParam) + "=" + Uri.EscapeDataString(Etx.RandomSurprise()));
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
