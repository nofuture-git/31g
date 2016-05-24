using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Gov.Fed;

namespace NoFuture.Rand.Data.NfHtml
{
    public class FfiecInstitProfile : INfDynData
    {
        public FfiecInstitProfile(Uri srcUri)
        {
            SourceUri = srcUri;
        }

        public Uri SourceUri { get; private set; }
        public List<dynamic> ParseContent(object content)
        {
            var webResponseBody = content as string;
            if (webResponseBody == null)
                return null;

            string[] d = null;
            if (!Tokens.AspNetParseTree.TryGetCdata(webResponseBody, null, out d))
            {
                return null;
            }
            var innerText = d.ToList();
            if (innerText.Count <= 0)
                return null;
            var rt = string.Empty;
            var nm = string.Empty;
            var rssd = string.Empty;

            var st =
                innerText.FindIndex(
                    x => string.Equals(x.Trim(), "lblID_ABA_PRIM", StringComparison.OrdinalIgnoreCase));

            var fromHere = innerText.Skip(st);

            foreach (var val in fromHere)
                if (Shared.RegexCatalog.IsRegexMatch(val.Trim(), RoutingTransitNumber.REGEX_PATTERN, out rt))
                    break;

            st =
                innerText.FindIndex(
                    x => string.Equals(x.Trim(), "lblNm_lgl", StringComparison.OrdinalIgnoreCase));

            fromHere = innerText.Skip(st);
            foreach (var val in fromHere)
                if (Shared.RegexCatalog.IsRegexMatch(val.Trim(), @"[a-zA-Z\x2C\x20]+", out nm))
                    break;

            st =
                innerText.FindIndex(
                    x => string.Equals(x.Trim(), "lblID_RSSD", StringComparison.OrdinalIgnoreCase));

            fromHere = innerText.Skip(st);
            foreach (var val in fromHere)
                if (Shared.RegexCatalog.IsRegexMatch(val.Trim(), @"^[0-9]+$", out rssd))
                    break;

            return new List<dynamic>
            {
                new {RoutingNumber = rt, Rssd = rssd, BankName = nm}
            };
        }
    }
}
