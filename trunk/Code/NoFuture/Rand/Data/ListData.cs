using System.Linq;
using NoFuture.Rand.Data.Source;
using NoFuture.Shared;

namespace NoFuture.Rand.Data
{
    public class ListData
    {
        #region fields
        private static string[] _webdomains;
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
        public static string[] UsWebmailDomains
        {
            get
            {
                if(_webdomains != null && _webdomains.Length > 0)
                    return _webdomains;

                _webdomains = DataFiles.GetByName(DataFiles.WEBMAIL_DOMAINS).Split(Constants.LF).ToArray();
                return _webdomains;

            }
        }
    }
}
