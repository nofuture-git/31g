using System;
using System.Collections;
using System.Collections.Generic;
using NoFuture.Rand.Data.Types;
using NoFuture.Shared;

namespace NoFuture.Rand.Data.NfHttp
{
    public class YhooFinSymbolLookup : INfDynData
    {
        public YhooFinSymbolLookup(Uri srcUri)
        {
            SourceUri = srcUri;
        }

        public Uri SourceUri { get; }

        public List<dynamic> ParseContent(object content)
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
