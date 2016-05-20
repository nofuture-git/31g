using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.NfHtml;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Gov.Sec;

namespace NoFuture.Rand.Com
{
    /// <summary>
    /// Represents a publicly traded corporation
    /// </summary>
    /// <example>
    /// <![CDATA[
    ///  #example in PowerShell
    /// 
    ///  #get a sic code 
    ///  $sic = [NoFuture.Rand.Data.Types.StandardIndustryClassification]::RandomSic();
    /// 
    ///  #get corps by sic code
    ///  $searchCrit = New-Object NoFuture.Rand.Gov.Sec.Edgar+FullTextSearch
    ///  $searchCrit.SicCode = $sic.Value
    ///  $rssContent = Request-File -Url ([NoFuture.Rand.Gov.Sec.Edgar]::GetUriFullTextSearch($searchCrit))
    /// 
    ///  #pick one
    ///  $randomCorp = $publicCorps[3]
    /// 
    ///  #get details from SEC
    ///  $xmlContent = Request-File -Url ([NoFuture.Rand.Gov.Sec.Edgar]::GetUriCikSearch($randomCorp.CIK))
    ///  [NoFuture.Rand.Gov.Sec.Edgar]::TryParseCorpData($xmlContent, [ref] $randomCorp)
    /// 
    ///  #get more details from yahoo finance
    ///  $yahooData = Request-File -Url ([NoFuture.Rand.Com.PublicCorporation]::GetUriTickerSymbolLookup($randomCorp.Name))
    ///  [NoFuture.Rand.Com.PublicCorporation]::TryMergeTickerLookup($yahooData, [ref] $randomCorp)
    /// 
    ///  #get any data you defined in the local XRef.xml
    ///  $randomCorp.LoadXrefXmlData()
    /// ]]>
    /// </example>
    [Serializable]
    public class PublicCorporation : Firm
    {
        #region fields
        private readonly List<Form10K> _annualReports = new List<Gov.Sec.Form10K>();
        #endregion

        #region properties
        public Gov.Irs.EmployerIdentificationNumber EIN { get; set; }

        public CentralIndexKey CIK { get; set; }

        public List<Form10K> AnnualReports
        {
            get { return _annualReports; }
        }

        public Gov.UsState UsStateOfIncorporation { get; set; }

        public Uri[] WebDomains { get; set; }

        public List<Ticker> TickerSymbols { get; set; }
        #endregion

        #region methods
        /// <summary>
        /// Constructs a URI which may be used to lookup ticker symbols of a public company
        /// based on the the name of the company.
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public static Uri GetUriTickerSymbolLookup(string companyName)
        {
            return
                new Uri("http://www.bloomberg.com/markets/symbolsearch?query=" +
                        Uri.EscapeUriString(companyName.Replace(",", "").Replace(".", "")) + "&commit=Find+Symbols");
        }

        /// <summary>
        /// Constructs a URI to use for getting balance sheet data by ticker symbol.
        /// </summary>
        /// <param name="tickerSymbol"></param>
        /// <returns></returns>
        public static Uri GetUriAnnualBalanceSheet(Ticker tickerSymbol)
        {
            return new Uri("http://finance.yahoo.com/q/bs?s=" + tickerSymbol.Symbol + "&annual");
        }

        /// <summary>
        /// Merges the data returned from <see cref="GetUriAnnualBalanceSheet"/> into an 
        /// instance of <see cref="PublicCorporation"/>
        /// </summary>
        /// <param name="webResponseBody"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static bool TryMergeFinancialData(string webResponseBody, ref PublicCorporation pc)
        {

            var nfChardata = new YhooFinIncomeStmt();
            var nfChardataRslts = nfChardata.ParseContent(webResponseBody);
            if (nfChardataRslts == null || nfChardataRslts.Count <= 0)
                return false;

            pc.AnnualReports.Add(new Form10K
            {
                FinancialData =
                    new FinancialData
                    {
                        FiscalYear = nfChardataRslts[0].FiscalYearEndAt,
                        Assets =
                            new Assets
                            {
                                Src = nfChardata.SourceUri.ToString(),
                                TotalAssets = new Pecuniam(nfChardataRslts[0].TotalAssets),
                                TotalLiabilities = new Pecuniam(nfChardataRslts[0].TotalLiabilities)
                            }
                    }
            });

            return true;
        }

        /// <summary>
        /// Merges the data returned from <see cref="GetUriTickerSymbolLookup"/> into an 
        /// instance of <see cref="PublicCorporation"/>
        /// </summary>
        /// <param name="webResponseBody"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static bool TryMergeTickerLookup(string webResponseBody, ref PublicCorporation pc)
        {
            try
            {
                var myNfCdata = new BloombergSymbolSearch();
                var myNfCdataRslts = myNfCdata.ParseContent(webResponseBody);
                if (myNfCdataRslts == null)
                    return false;

                pc.TickerSymbols = new List<Ticker>();
                foreach (var dd in myNfCdataRslts)
                {
                    pc.TickerSymbols.Add(new Ticker { Symbol = dd.Symbol, InstrumentType = dd.InstrumentType });
                }

                return pc.TickerSymbols.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        public override void LoadXrefXmlData()
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
        #endregion
    }

}