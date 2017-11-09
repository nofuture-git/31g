using System;
using System.Linq;
using NoFuture.Rand.Com;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data;

namespace NoFuture.Rand.Gov.Fed
{
    public class Ffiec
    {
        public const string SEARCH_URL_BASE = "https://www.ffiec.gov/nicpubweb/nicweb/";

        #region methods
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

            var myDynData = DynamicDataFactory.GetDataParser(srcUri);
            var myDynDataRslt = myDynData.ParseContent(rawHtmlContent);

            if (myDynDataRslt == null || !myDynDataRslt.Any())
                return false;

            var pd = myDynDataRslt.First();

            firmOut.RoutingNumber = new RoutingTransitNumber
            {
                Value = pd.RoutingNumber,
                Src = myDynData.SourceUri.ToString()
            };
            if(string.IsNullOrWhiteSpace(firmOut.Rssd?.ToString()))
                firmOut.Rssd = new ResearchStatisticsSupervisionDiscount {Value = pd.Rssd};

            firmOut.UpsertName(KindsOfNames.Legal, pd.BankName);
            if (string.IsNullOrWhiteSpace(firmOut.FdicNumber?.ToString()))
                firmOut.FdicNumber = new FdicNum {Value = pd.FdicCert, Src = myDynData.SourceUri.ToString()};

            return !string.IsNullOrWhiteSpace(pd.RoutingNumber);
        }
        #endregion
    }
}
