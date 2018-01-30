using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Gov.US.Fed;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Exo.NfHtml
{
    public class FfiecInstitProfile : NfHtmlDynDataBase
    {
        public FfiecInstitProfile(Uri srcUri):base(srcUri) { }

        /// <summary>
        /// This will produce a URI which upon being requested from FFIEC will return html in 
        /// which the <see cref="rssd"/> will map to an official name.  This name will
        /// produce results when used in SEC queries.
        /// </summary>
        /// <param name="rssd"></param>
        /// <returns></returns>
        public static Uri GetUri(ResearchStatisticsSupervisionDiscount rssd)
        {
            return new Uri(UsGov.Links.Ffiec.SEARCH_URL_BASE + "InstitutionProfile.aspx?parID_Rssd=" + rssd + "&parDT_END=99991231");
        }

        public override IEnumerable<dynamic> ParseContent(object content)
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
            var rt = String.Empty;
            var nm = String.Empty;
            var rssd = String.Empty;
            var fdicCert = String.Empty;

            var st =
                innerText.FindIndex(
                    x => x.Trim().StartsWith("Branch Locator"));
            var fromHere = innerText.Skip(st + 1);
            foreach (var val in fromHere.Where(x => !x.StartsWith("&nbsp")))
                if (RegexCatalog.IsRegexMatch(val.Trim(), @"[a-zA-Z\x2C\x20]+", out nm))
                    break;

            st =
                innerText.FindIndex(
                    x => x.Trim().StartsWith("RSSD ID"));
            fromHere = innerText.Skip(st);
            foreach (var val in fromHere)
                if (RegexCatalog.IsRegexMatch(val.Trim(), @"^[0-9]+$", out rssd))
                    break;

            st =
                innerText.FindIndex(
                    x => x.Trim().StartsWith("FDIC Certificate"));
            fromHere = innerText.Skip(st);
            foreach (var val in fromHere)
                if (RegexCatalog.IsRegexMatch(val.Trim(), @"^[0-9]+$", out fdicCert))
                    break;

            st =
                innerText.FindIndex(
                    x => x.Trim().StartsWith("Routing Transit Number"));
            fromHere = innerText.Skip(st);
            foreach (var val in fromHere)
                if (RegexCatalog.IsRegexMatch(val.Trim(), RoutingTransitNumber.REGEX_PATTERN, out rt))
                    break;

            return new List<dynamic>
            {
                new {RoutingNumber = rt, Rssd = rssd, BankName = nm, FdicCert = fdicCert}
            };
        }
    }
}
