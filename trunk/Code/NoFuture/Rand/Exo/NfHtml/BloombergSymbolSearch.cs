﻿using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Com;

namespace NoFuture.Rand.Exo.NfHtml
{
    /// <summary>
    /// Used to cross-reference a company&apos;s name to its public ticker symbol
    /// </summary>
    [Serializable]
    public class BloombergSymbolSearch : NfHtmlDynDataBase
    {
        public const string BLOOMBERG_HOST = "www.bloomberg.com";
        public BloombergSymbolSearch(Uri srcUri):base(srcUri) { }

        public static Uri GetUri(PublicCompany com)
        {
            return new Uri($"http://{BLOOMBERG_HOST}/markets/symbolsearch?query={com.UrlEncodedName}&commit=Find+Symbols");
        }

        public override IEnumerable<dynamic> ParseContent(object content)
        {
            var webResponseBody = GetWebResponseBody(content);
            if (webResponseBody == null)
                return null;

            string[] d;
            if (!Antlr.HTML.HtmlParseResults.TryGetCdata(webResponseBody, null, out d))
                return null;
            var innerText = d.ToList();
            if (innerText.Count <= 0)
                return null;

            var st =
                innerText.FindIndex(
                    x => string.Equals(x.Trim(), "Symbol Lookup", StringComparison.OrdinalIgnoreCase));
            var ed =
                innerText.FindIndex(
                    x => string.Equals(x.Trim(), "Sponsored Link", StringComparison.OrdinalIgnoreCase));

            if (st < 0 || ed < 0 || st > ed)
                return null;

            var targetData = innerText.Skip(st).Take(ed - st).ToList();
            if (targetData.Count <= 0)
                return null;

            if (targetData.Any(x => x.Contains(" no matches ")))
                return null;

            st =
                targetData.FindIndex(
                    x => string.Equals(x.Trim(), "Symbol", StringComparison.OrdinalIgnoreCase));

            targetData = targetData.Skip(st).ToList();

            var isDivisible = targetData.Count % 5 == 0;

            if (!isDivisible)
                return null;

            var rowCount = Math.Floor(targetData.Count / 5M);

            if (rowCount <= 1)
                return null;

            rowCount -= 1;
            var cdataOut = new List<dynamic>();
            for (var i = 1; i <= rowCount; i++)
            {
                var te = targetData.Skip(i * 5).Take(5).ToArray();
                cdataOut.Add(new { Symbol = te[0], Name = te[1], Country = te[2], InstrumentType = te[3], Industry = te[4] });
            }
            return cdataOut;
        }
    }
}
