using System.IO;

namespace NoFuture.Rand.Data
{
    public class ListData
    {
        #region fields
        private static string[] _webdomains;
        #endregion

        #region
        private const string WEBMAIL_DOMAINS = "webmailDomains.txt";
        #endregion

        /// <summary>
        /// Src [https://github.com/tarr11/Webmail-Domains/blob/master/domains.txt]
        /// </summary>
        public static string[] UsWebmailDomains
        {
            get
            {
                if(_webdomains != null && _webdomains.Length > 0)
                    return _webdomains;

                if (!TreeData.TestDataFileIsPresent(WEBMAIL_DOMAINS))
                    return null;

                _webdomains = File.ReadAllLines(Path.Combine(BinDirectories.DataRoot, WEBMAIL_DOMAINS));
                return _webdomains;
            }
        }
    }
}
