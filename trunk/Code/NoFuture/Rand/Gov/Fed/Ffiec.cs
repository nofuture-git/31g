using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data;

namespace NoFuture.Rand.Gov.Fed
{
    public class Ffiec
    {
        public const string SEARCH_URL_BASE = "https://www.ffiec.gov/nicpubweb/nicweb/";

        /// <summary>
        /// This will produce a URI which upon being requested from FFIEC will return html in 
        /// which the <see cref="rssd"/> will map to an official name.  This name will
        /// produce results when used in SEC queries.
        /// </summary>
        /// <param name="rssd"></param>
        /// <returns></returns>
        public static Uri UrlSearchByRssd(ResearchStatisticsSupervisionDiscount rssd)
        {
            return new Uri(SEARCH_URL_BASE + "InstitutionProfile.aspx?parID_Rssd=" + rssd + "&parDT_END=99991231");
        }

        /// <summary>
        /// The HTML is too dirty to be parsed into XML directly. So 
        /// this attempts to find the inner block between <see cref="startLookingOn"/> 
        /// and <see cref="endLookingOn"/> and load that into XML and perform a read.
        /// </summary>
        /// <param name="rawHtmlContent"></param>
        /// <param name="firmOut"></param>
        /// <param name="startLookingOn"></param>
        /// <param name="endLookingOn"></param>
        /// <returns></returns>
        /// <remarks>
        /// This is very precariously attempting to get some structured data out of the 
        /// badly formed HTML.
        /// </remarks>
        public static bool TryParseFfiecInstitutionProfileAspxHtml(string rawHtmlContent, out FinancialFirm firmOut,
            string startLookingOn = "id=\"Table2\"", string endLookingOn = "id=\"Table3\"")
        {
            firmOut = null;
            if (string.IsNullOrWhiteSpace(rawHtmlContent))
                return false;
            rawHtmlContent = rawHtmlContent.Replace((char) 0x0D, ' ');
            var htmlLines = rawHtmlContent.Split('\n');
            htmlLines = Util.Etc.GetContentBetweenMarkers(htmlLines, startLookingOn, endLookingOn);
            if (htmlLines == null || htmlLines.Length <= 0)
                return false;

            for (var i = 0; i < htmlLines.Length; i++)
            {
                if (htmlLines[i].Contains("&nbsp;"))
                    htmlLines[i] = htmlLines[i].Replace("&nbsp;", string.Empty);
                if (htmlLines[i].Contains("<br>"))
                    htmlLines[i] = htmlLines[i].Replace("<br>", "<br />");
                if (htmlLines[i].Contains(" noWrap"))
                    htmlLines[i] = htmlLines[i].Replace(" noWrap", " ");
                if (htmlLines[i].Contains("noWrap "))
                    htmlLines[i] = htmlLines[i].Replace("noWrap ", " ");
            }
            XmlDocument attemptedParse = new XmlDocument();
            var possiableXmlString = string.Join(Environment.NewLine, htmlLines);
            System.IO.File.WriteAllText(@"C:\Temp\TryParseFfiec.txt", possiableXmlString);
            try
            {
                attemptedParse.LoadXml(string.Join(Environment.NewLine, htmlLines));
            }
            catch (XmlException)
            {
                return false;
            }

            var nameNode = attemptedParse.SelectSingleNode("//*[@id='lblNm_lgl']//text()");
            if (nameNode == null || string.IsNullOrWhiteSpace(nameNode.Value))
            {
                return false;
            }

            var rssdNode = attemptedParse.SelectSingleNode("//*[@id='lblID_RSSD']//text()");
            if (rssdNode == null || string.IsNullOrWhiteSpace(rssdNode.Value))
            {
                return false;
            }

            var rssd = new ResearchStatisticsSupervisionDiscount {Value = rssdNode.Value};
            var bankName = nameNode.Value;

            if (TreeData.CommercialBankData != null && TreeData.CommercialBankData.Any(x => rssd.Equals(x.Rssd)))
            {
                firmOut = TreeData.CommercialBankData.First(x => rssd.Equals(x.Rssd));
            }
            else
            {
                firmOut = new FinancialFirm {Rssd = rssd};
            }

            firmOut.Name = bankName.Trim();

            var rtNumNode = attemptedParse.SelectSingleNode("//*[@id='lblID_ABA_PRIM']//text()");
            if (rtNumNode == null || string.IsNullOrWhiteSpace(rtNumNode.Value))
                return true;

            var rt = new RoutingTransitNumber {Value = rtNumNode.Value};
            firmOut.RoutingNumber = rt;

            return true;
        }
    }
}
