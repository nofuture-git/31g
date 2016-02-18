using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Com
{
    [Serializable]
    public class PublicCorporation : Firm
    {
        #region fields
        private readonly List<Gov.Sec.Form10K> _annualReports = new List<Gov.Sec.Form10K>();
        #endregion

        #region Properties

        public Gov.Irs.EmployerIdentificationNumber EIN { get; set; }

        public Gov.Sec.CentralIndexKey CIK { get; set; }

        public List<Gov.Sec.Form10K> AnnualReports
        {
            get { return _annualReports; }
        }

        public Gov.UsState UsStateOfIncorporation { get; set; }

        public Uri[] WebDomains { get; set; }

        public List<Ticker> TickerSymbols { get; set; }

        #endregion

        #region Internal helpers
        internal const string TickerSearchStringEnclosure = "YAHOO.Finance.SymbolSuggest.ssCallback";

        #endregion

        /// <summary>
        /// Constructs a URI which may be used to lookup ticker symbols of a public company
        /// based on the the name of the company.
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public static Uri CtorTickerSymbolLookup(string companyName)
        {
            return
                new Uri("http://d.yimg.com/autoc.finance.yahoo.com/autoc?query=" + Uri.EscapeUriString(companyName) +
                        "&callback=" + TickerSearchStringEnclosure);
        }

        /// <summary>
        /// Merges the json data returned from <see cref="CtorTickerSymbolLookup"/> into an 
        /// instance of <see cref="PublicCorporation"/>
        /// </summary>
        /// <param name="webResponseBody"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static bool MergeTickerLookupFromJson(string webResponseBody, ref PublicCorporation pc)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(webResponseBody))
                    return false;
                if (webResponseBody.StartsWith(TickerSearchStringEnclosure))
                    webResponseBody = webResponseBody.Replace(TickerSearchStringEnclosure, string.Empty);

                if (webResponseBody.StartsWith("("))
                    webResponseBody = webResponseBody.Substring(1, webResponseBody.Length - 1);
                if (webResponseBody.EndsWith(")"))
                    webResponseBody = webResponseBody.Substring(0, webResponseBody.Length - 1);
                YahooTickerResultSet yrs;
                var data = Encoding.UTF8.GetBytes(webResponseBody);
                var jsonSerializer = new DataContractJsonSerializer(typeof(YahooTickerResultSet));
                using (var ms = new MemoryStream(data))
                {
                    yrs = (YahooTickerResultSet)jsonSerializer.ReadObject(ms);
                }

                if (yrs == null || yrs.ResultSet == null || yrs.ResultSet.Result == null || yrs.ResultSet.Result.Length <= 0)
                    return false;

                foreach (var sym in yrs.ResultSet.Result)
                {
                    pc.TickerSymbols.Add(new Ticker
                    {
                        Exchange = sym.exch,
                        Symbol = sym.symbol,
                        InstrumentType = sym.typeDisp
                    });
                }

                return pc.TickerSymbols.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        public override void GetXrefXmlData()
        {
            var xrefXml = TreeData.XRefXml;
            if (xrefXml == null)
                return;
            var myAssocNodes =
                xrefXml.SelectNodes(string.Format("//x-ref-group[@data-type='{0}']//x-ref-id[text()='{1}']/../../add",
                    GetType().FullName, Name));
            if (myAssocNodes == null || myAssocNodes.Count <= 0)
                return;

            foreach (var node in myAssocNodes)
            {
                var elem = node as XmlElement;
                if (elem == null)
                    continue;

                XRefGroup.SetTypeXrefValue(elem, this);
            }
        }
    }

}