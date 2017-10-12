using System;
using System.Linq;
using NoFuture.Rand.Data.Source;
using NoFuture.Shared;

namespace NoFuture.Rand.Data
{
    public class ListData
    {
        #region fields
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
        /// Loads <see cref="DataFiles.WEBMAIL_DOMAINS"/> into an array.
        /// Src [https://github.com/tarr11/Webmail-Domains/blob/master/domains.txt]
        /// </summary>
        public static string[] WebmailDomains
        {
            get
            {
                if(_webMaildomains != null && _webMaildomains.Length > 0)
                    return _webMaildomains;

                _webMaildomains = DataFiles.GetByName(DataFiles.WEBMAIL_DOMAINS).Split(Constants.LF).ToArray();
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

    }
}
