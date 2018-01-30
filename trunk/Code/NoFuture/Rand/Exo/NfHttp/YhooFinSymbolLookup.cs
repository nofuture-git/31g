using System;
using System.Collections;
using System.Collections.Generic;
using NoFuture.Rand.Com;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Exo.NfHttp
{
    public class YhooFinSymbolLookup : NfDynDataBase
    {
        public YhooFinSymbolLookup(Uri srcUri):base(srcUri) { }

        public static Uri GetUri(PublicCorporation com)
        {
            return new Uri("http://finance.yahoo.com/q?s=" + com.UrlEncodedName + "&ql=1");
        }

        public override IEnumerable<dynamic> ParseContent(object content)
        {
            var headers = content as Hashtable;
            if (headers == null)
                return null;

            if (!headers.ContainsKey("Set-Cookie"))
                return null;

            var tVal = headers["Set-Cookie"];
            if (string.IsNullOrWhiteSpace(tVal?.ToString()))
                return null;

            string tickerSymbol;
            if (!RegexCatalog.IsRegexMatch(tVal.ToString(), "\x26t\x3d([A-Z]+)", out tickerSymbol, 1))
                return null;

            return new List<dynamic>
            {
                new
                {
                    Symbol = tickerSymbol,
                    Name = string.Empty,
                    Country = string.Empty,
                    InstrumentType = "Common Stock",
                    Industry = string.Empty
                }
            };
        }
    }
}
