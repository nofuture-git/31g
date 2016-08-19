using System;
using System.Linq;
using NoFuture.Rand.Com;

namespace NoFuture.Rand.Gov.Fed
{
    public class Ffiec
    {
        public const string SEARCH_URL_BASE = "https://www.ffiec.gov/nicpubweb/nicweb/";

        #region methods
        /// <summary>
        /// This will produce a URI which upon being requested from FFIEC will return html in 
        /// which the <see cref="rssd"/> will map to an official name.  This name will
        /// produce results when used in SEC queries.
        /// </summary>
        /// <param name="rssd"></param>
        /// <returns></returns>
        public static Uri GetUriSearchByRssd(ResearchStatisticsSupervisionDiscount rssd)
        {
            return new Uri(SEARCH_URL_BASE + "InstitutionProfile.aspx?parID_Rssd=" + rssd + "&parDT_END=99991231");
        }

        /// <summary>
        /// Attempts to get the ABA number based on RSSD 
        /// </summary>
        /// <param name="rawHtmlContent"></param>
        /// <param name="srcUri"></param>
        /// <param name="firmOut"></param>
        /// <returns></returns>
        public static bool TryParseFfiecInstitutionProfileAspxHtml(string rawHtmlContent, Uri srcUri, ref Bank firmOut)
        {
            if (string.IsNullOrWhiteSpace(rawHtmlContent))
                return false;
            if (firmOut == null)
                return false;

            var myDynData = Etx.DynamicDataFactory(srcUri);
            var myDynDataRslt = myDynData.ParseContent(rawHtmlContent);

            if (myDynDataRslt == null || myDynDataRslt.Count <= 0)
                return false;

            var pd = myDynDataRslt.First();

            firmOut.RoutingNumber = new RoutingTransitNumber
            {
                Value = pd.RoutingNumber,
                Src = myDynData.SourceUri.ToString()
            };
            if(string.IsNullOrWhiteSpace(firmOut.Rssd?.ToString()))
                firmOut.Rssd = new ResearchStatisticsSupervisionDiscount {Value = pd.Rssd};

            firmOut.UpsertName(KindsOfNames.Abbrev, firmOut.Name);
            firmOut.Name = pd.BankName;
            if (string.IsNullOrWhiteSpace(firmOut.FdicNumber?.ToString()))
                firmOut.FdicNumber = new FdicNum {Value = pd.FdicCert, Src = myDynData.SourceUri.ToString()};

            return !string.IsNullOrWhiteSpace(pd.RoutingNumber);
        }
        #endregion
    }
}
