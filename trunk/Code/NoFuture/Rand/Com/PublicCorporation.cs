using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Exceptions;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.NfHtml;
using NoFuture.Rand.Data.NfHttp;
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
    ///  $publicCorps = ([NoFuture.Rand.Gov.Sec.Edgar]::ParseFullTextSearch($rssContent))
    /// 
    ///  #pick one
    ///  $randomCorp = $publicCorps[3]
    /// 
    ///  #get details from SEC
    ///  $xmlContent = Request-File -Url ([NoFuture.Rand.Gov.Sec.Edgar]::GetUriCikSearch($randomCorp.CIK))
    ///  [NoFuture.Rand.Gov.Sec.Edgar]::TryParseCorpData($xmlContent, [ref] $randomCorp)
    /// 
    ///  #get more details from yahoo finance
    ///  $rawHtml = Request-File -Url ([NoFuture.Rand.Com.PublicCorporation]::GetUriTickerSymbolLookup($randomCorp.Name))
    ///  if( [NoFuture.Rand.Com.PublicCorporation]::TryMergeTickerLookup($rawHtml, [ref] $randomCorp)){
    ///     $ticker = $randomCorp.TickerSymbols | Select-Object -First 1
    /// 
    ///     $rawHtml = Request-File -Url ([NoFuture.Rand.Com.PublicCorporation]::GetUriAnnualBalanceSheet($ticker))
    ///     [NoFuture.Rand.Com.PublicCorporation]::TryMergeAssets($rawHtml, [ref] $randomCorp)
    /// 
    ///     $rawHtml = Request-File -Url ([NoFuture.Rand.Com.PublicCorporation]::GetUriAnnualIncomeStmt($ticker))
    ///     [NoFuture.Rand.Com.PublicCorporation]::TryMergeIncome($rawHtml, [ref] $randomCorp)
    ///  }
    /// 
    ///  #get any data you defined in the local XRef.xml
    ///  $randomCorp.LoadXrefXmlData()
    /// ]]>
    /// </example>
    [Serializable]
    public class PublicCorporation : Firm
    {
        #region fields
        private readonly List<Form10K> _annualReports = new List<Form10K>();
        private readonly List<Ticker> _tickerSymbols = new List<Ticker>();
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

        public List<Ticker> TickerSymbols
        {
            get { return _tickerSymbols; }
        }

        /// <summary>
        /// Strips periods and comma's and any whole words which 
        /// do not begin with a letter or digit and encodes these results.
        /// </summary>
        public string UrlEncodedName
        {
            get
            {
                var searchCompanyName = Name.Replace(",", string.Empty);
                searchCompanyName = searchCompanyName.Replace(".", string.Empty);
                searchCompanyName = searchCompanyName.ToUpper().Trim();

                var nameparts = searchCompanyName.Split(' ');

                searchCompanyName = string.Join(" ",
                    nameparts.Where(x => !string.IsNullOrWhiteSpace(x) && char.IsLetterOrDigit(x.ToCharArray()[0])));

                return Uri.EscapeUriString(searchCompanyName);
            }
        }



        #endregion

        #region methods

        public Tuple<Type, Uri>[] GetDataUris()
        {
            var dataUris = new List<Tuple<Type, Uri>>();
            var tickerSymbol = _tickerSymbols.FirstOrDefault();
            if (tickerSymbol != null)
            {
                dataUris.Add(new Tuple<Type, Uri>(typeof(YhooFinBalanceSheet),
                    new Uri("http://finance.yahoo.com/q/bs?s=" + tickerSymbol.Symbol + "&annual")));
                dataUris.Add(new Tuple<Type, Uri>(typeof(YhooFinIncomeStmt),
                    new Uri("http://finance.yahoo.com/q/is?s=" + tickerSymbol.Symbol + "&annual")));
            }
            dataUris.Add(new Tuple<Type, Uri>(typeof(BloombergSymbolSearch),
                new Uri("http://www.bloomberg.com/markets/symbolsearch?query=" + UrlEncodedName + "&commit=Find+Symbols")));
            dataUris.Add(new Tuple<Type, Uri>(typeof(YhooFinSymbolLookup),
                new Uri("http://finance.yahoo.com/q?s=" + UrlEncodedName + "&ql=1")));
            return dataUris.ToArray();
        }

        /// <summary>
        /// For a guaranteed ref to a <see cref="Form10K"/> 
        /// within this instances <see cref="AnnualReports"/>
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        protected internal Form10K GetForm10KByYear(int year)
        {
            if (_annualReports.All(x => x.FilingDate.Year - 1 != year))
            {
                var form10K = new Form10K
                {
                    FilingDate = new DateTime(year - 1, 1, 2),
                    FinancialData =
                        new FinancialData
                        {
                            FiscalYear = year,
                            Assets = new Assets(),
                            Income = new Income()
                        }
                };

                if (year < form10K.Statute.Year)
                    throw new RahRowRagee(string.Format("A Form 10-K for year {0} is invalid " +
                                                        "since the '{1}' is from the year '{2}'", year,
                        form10K.Statute.Name, form10K.Statute.Year));

                _annualReports.Add(form10K); 
            }
            return _annualReports.First(x => x.FilingDate.Year - 1 != year);
        }

        /// <summary>
        /// Merges the balance sheet data contained in <see cref="webResponseBody"/> into the instance of <see cref="PublicCorporation"/>
        /// </summary>
        /// <param name="webResponseBody">The raw html from a web request.</param>
        /// <param name="srcUri"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static bool TryMergeAssets(string webResponseBody, Uri srcUri, ref PublicCorporation pc)
        {
            try
            {
                if (pc == null)
                    return false;
                var nfChardata = new YhooFinBalanceSheet(srcUri);
                var nfChardataRslts = nfChardata.ParseContent(webResponseBody);
                if (nfChardataRslts == null || nfChardataRslts.Count <= 0)
                    return false;

                foreach (var cd in nfChardataRslts)
                {
                    var tenK = pc.GetForm10KByYear((int)cd.FiscalYearEndAt);
                    tenK.FinancialData.Assets.TotalAssets = new Pecuniam((Decimal)cd.TotalAssets);
                    tenK.FinancialData.Assets.TotalAssets = new Pecuniam((Decimal)cd.TotalLiabilities);
                    tenK.FinancialData.Assets.Src = nfChardata.SourceUri.ToString();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Merges the income statement data contained in <see cref="webResponseBody"/> into the instance of <see cref="PublicCorporation"/>
        /// </summary>
        /// <param name="webResponseBody">The raw html from a web request.</param>
        /// <param name="srcUri"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static bool TryMergeIncome(string webResponseBody, Uri srcUri, ref PublicCorporation pc)
        {
            try
            {
                if (pc == null)
                    return false;
                var nfChardata = new YhooFinIncomeStmt(srcUri);
                var nfChardataRslts = nfChardata.ParseContent(webResponseBody);
                if (nfChardataRslts == null || nfChardataRslts.Count <= 0)
                    return false;

                foreach (var cd in nfChardataRslts)
                {
                    var tenK = pc.GetForm10KByYear((int) cd.FiscalYearEndAt);
                    tenK.FinancialData.Income.Revenue = new Pecuniam((Decimal) cd.TotalRevenue);
                    tenK.FinancialData.Income.OperatingIncome = new Pecuniam((Decimal) cd.OperatingIncomeorLoss);
                    tenK.FinancialData.Income.NetIncome = new Pecuniam((Decimal) cd.NetIncome);
                    tenK.FinancialData.Income.Src = nfChardata.SourceUri.ToString();
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Merges the ticker symbol data contained in <see cref="webResponseBody"/> into the instance of <see cref="PublicCorporation"/>
        /// </summary>
        /// <param name="webResponseBody"></param>
        /// <param name="srcUri"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static bool TryMergeTickerLookup(string webResponseBody, Uri srcUri, ref PublicCorporation pc)
        {
            try
            {
                
                var myNfCdata = new BloombergSymbolSearch(srcUri);
                var myNfCdataRslts = myNfCdata.ParseContent(webResponseBody);
                if (myNfCdataRslts == null)
                    return false;

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