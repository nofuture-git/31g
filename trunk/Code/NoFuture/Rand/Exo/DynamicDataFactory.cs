using System;
using NoFuture.Rand.Exo.NfHtml;
using NoFuture.Rand.Exo.NfJson;
using NoFuture.Rand.Exo.NfText;
using NoFuture.Rand.Exo.NfXml;

namespace NoFuture.Rand.Exo
{
    /// <summary>
    /// A factory class to get the various forms of exogenous data based on the 
    /// source URI.
    /// </summary>
    public static class DynamicDataFactory
    {
        /// <summary>
        /// Factory method to get a concrete implementation of <see cref="INfDynData"/>
        /// based on a Uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static INfDynData GetDataParser(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            if (uri.Host == BloombergSymbolSearch.BLOOMBERG_HOST)
            {
                return new BloombergSymbolSearch(uri);
            }
            if (uri.Host == Edgar.SEC_HOST)
            {
                if (uri.LocalPath == "/cgi-bin/srch-edgar")
                {
                    return new SecFullTxtSearch(uri);
                }
                if (uri.LocalPath == "/cgi-bin/browse-edgar" && uri.Query.StartsWith("?action=getcompany&CIK="))
                {
                    return new SecCikSearch(uri);
                }
                if (uri.LocalPath.StartsWith("/Archives/edgar/data"))
                {
                    if (uri.LocalPath.EndsWith("index.htm"))
                        return new SecGetXbrlUri(uri);
                    if (uri.LocalPath.EndsWith(".xml"))
                        return new SecXbrlInstanceFile(uri);
                }
            }
            if (uri.Host == new Uri(FedLrgBnk.RELEASE_URL).Host)
            {
                return new FedLrgBnk();
            }
            if (uri.Host == new Uri(UsGov.Links.Ffiec.SEARCH_URL_BASE).Host)
            {
                return new FfiecInstitProfile(uri);
            }

            if (uri.Host == GoogleFinanceStockPrice.GOOG_FIN_HOST)
            {
                return new GoogleFinanceStockPrice(uri);
            }

            throw new NotImplementedException();
        }
    }
}
