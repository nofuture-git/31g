using System;
using System.Collections.Generic;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Data.Exo.NfHtml
{
    public class FidelitySymbolSearch : NfHtmlDynDataBase
    {
        public FidelitySymbolSearch(Uri src) : base(src)
        {
        }

        public static Uri GetUri(Cusip securityId)
        {
            return new Uri("https://quotes.fidelity.com/mmnet/SymLookup.phtml?" +
                           "reqforlookup=REQUESTFORLOOKUP&productid=mmnet&" +
                           $"isLoggedIn=mmnet&rows=50&for=stock&by=cusip&criteria={securityId}&submit=Search");
        }

        public override IEnumerable<dynamic> ParseContent(object content)
        {
            var webResponseBody = content as string;
            if (webResponseBody == null)
                return null;

            var html = Tokens.Etx.GetHtmlDocument(webResponseBody);
            var companyName = html.DocumentNode.SelectSingleNode("//table[1]//table[3]//table[1]/tr[3]/td[1]")?.InnerText;
            if (string.IsNullOrWhiteSpace(companyName))
                return null;
            var ticker = html.DocumentNode.SelectSingleNode("//table[1]//table[3]//table[1]/tr[3]/td[2]")?.InnerText;

            return new List<dynamic> {new {CompanyName = companyName, TickerSymbol = ticker}};
        }
    }
}
