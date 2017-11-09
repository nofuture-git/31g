using System;
using NoFuture.Rand.Com.NfHtml;
using NoFuture.Rand.Com.NfText;
using NoFuture.Rand.Com.NfXml;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Gov.Fed;
using NoFuture.Rand.Gov.Sec;

namespace NoFuture.Rand.Com
{
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

            if (uri.Host == "www.bloomberg.com")
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
            if (uri.Host == new Uri(Ffiec.SEARCH_URL_BASE).Host)
            {
                return new FfiecInstitProfile(uri);
            }

            throw new NotImplementedException();
        }
    }
}
