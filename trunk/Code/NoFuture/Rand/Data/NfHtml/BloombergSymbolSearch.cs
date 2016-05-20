﻿using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Data.NfHtml
{
    public class BloombergSymbolSearch : INfDynData
    {
        private readonly Uri _srcUri;

        public BloombergSymbolSearch(Uri srcUri)
        {
            _srcUri = srcUri;
        }

        public Uri SourceUri { get { return _srcUri; } }

        public List<dynamic> ParseContent(object content)
        {
            var webResponseBody = content as string;
            if (webResponseBody == null)
                return null;

            string[] d = null;
            if (!Tokens.AspNetParseTree.TryGetCdata(webResponseBody, null, out d))
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
