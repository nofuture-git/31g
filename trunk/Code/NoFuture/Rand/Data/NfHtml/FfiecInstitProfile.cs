using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Gov.Fed;

namespace NoFuture.Rand.Data.NfHtml
{
    public class FfiecInstitProfile : NfHtmlDynDataBase
    {
        public FfiecInstitProfile(Uri srcUri):base(srcUri) { }

        public override List<dynamic> ParseContent(object content)
        {
            var webResponseBody = GetWebResponseBody(content);
            if (webResponseBody == null)
                return null;
            string[] d;
            if (!Tokens.Etx.TryGetCdata(webResponseBody, null, out d))
            {
                return null;
            }
            var innerText = d.ToList();
            if (innerText.Count <= 0)
                return null;
            var rt = string.Empty;
            var nm = string.Empty;
            var rssd = string.Empty;
            var fdicCert = string.Empty;

            var st =
                innerText.FindIndex(
                    x => x.Trim().StartsWith("Branch Locator"));
            var fromHere = innerText.Skip(st + 1);
            foreach (var val in fromHere.Where(x => !x.StartsWith("&nbsp")))
                if (Shared.RegexCatalog.IsRegexMatch(val.Trim(), @"[a-zA-Z\x2C\x20]+", out nm))
                    break;

            st =
                innerText.FindIndex(
                    x => x.Trim().StartsWith("RSSD ID"));
            fromHere = innerText.Skip(st);
            foreach (var val in fromHere)
                if (Shared.RegexCatalog.IsRegexMatch(val.Trim(), @"^[0-9]+$", out rssd))
                    break;

            st =
                innerText.FindIndex(
                    x => x.Trim().StartsWith("FDIC Certificate"));
            fromHere = innerText.Skip(st);
            foreach (var val in fromHere)
                if (Shared.RegexCatalog.IsRegexMatch(val.Trim(), @"^[0-9]+$", out fdicCert))
                    break;

            st =
                innerText.FindIndex(
                    x => x.Trim().StartsWith("Routing Transit Number"));
            fromHere = innerText.Skip(st);
            foreach (var val in fromHere)
                if (Shared.RegexCatalog.IsRegexMatch(val.Trim(), RoutingTransitNumber.REGEX_PATTERN, out rt))
                    break;

            return new List<dynamic>
            {
                new {RoutingNumber = rt, Rssd = rssd, BankName = nm, FdicCert = fdicCert}
            };
        }
    }
}
