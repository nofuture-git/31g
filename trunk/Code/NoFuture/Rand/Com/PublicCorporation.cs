using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Exceptions;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.NfHtml;
using NoFuture.Rand.Data.NfHttp;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Gov.Sec;

namespace NoFuture.Rand.Com
{
    /// <summary>
    /// Represents a publicly traded corporation
    /// </summary>
    [Serializable]
    public class PublicCorporation : Firm
    {
        #region fields
        private readonly List<Form10K> _annualReports = new List<Form10K>();
        private List<Ticker> _tickerSymbols = new List<Ticker>();
        #endregion

        #region properties
        public Gov.Irs.EmployerIdentificationNumber EIN { get; set; }

        public CentralIndexKey CIK { get; set; }

        public List<Form10K> AnnualReports => _annualReports;

        public Gov.UsState UsStateOfIncorporation { get; set; }

        public Uri[] WebDomains { get; set; }

        public List<Ticker> TickerSymbols
        {
            get
            {
                _tickerSymbols.Sort(new TickerComparer(Name));
                return _tickerSymbols;
            }
            set { _tickerSymbols = value; } //XRef.cs needs this as RW
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

        public Tuple<string, Uri>[] GetDataUris()
        {
            var dataUris = new List<Tuple<string, Uri>>();
            var tickerSymbol = 
                _tickerSymbols.FirstOrDefault();
            if (tickerSymbol != null)
            {
                dataUris.Add(new Tuple<string, Uri>(typeof(YhooFinBalanceSheet).Name,
                    new Uri("http://finance.yahoo.com/q/bs?s=" + tickerSymbol.Symbol + "&annual")));
                dataUris.Add(new Tuple<string, Uri>(typeof(YhooFinIncomeStmt).Name,
                    new Uri("http://finance.yahoo.com/q/is?s=" + tickerSymbol.Symbol + "&annual")));
            }
            dataUris.Add(new Tuple<string, Uri>(typeof(BloombergSymbolSearch).Name,
                new Uri("http://www.bloomberg.com/markets/symbolsearch?query=" + UrlEncodedName + "&commit=Find+Symbols")));
            dataUris.Add(new Tuple<string, Uri>(typeof(YhooFinSymbolLookup).Name,
                new Uri("http://finance.yahoo.com/q?s=" + UrlEncodedName + "&ql=1")));
            return dataUris.ToArray();
        }

        /// <summary>
        /// For a guaranteed ref to a <see cref="Form10K"/> 
        /// within this instances <see cref="AnnualReports"/>
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public Form10K GetForm10KByYear(int year)
        {
            Form10K form10K;
            if (_annualReports.All(x => x.FilingDate.Year - 1 != year))
            {
                form10K = new Form10K
                {
                    FilingDate = new DateTime(year + 1, 1, 2),
                    FinancialData =
                        new ComFinancialData
                        {
                            FiscalYear = year,
                            Assets = new NetConAssets(),
                            Income = new NetConIncome()
                        }
                };

                if (year < form10K.Statute.Year)
                    throw new RahRowRagee($"A Form 10-K for year {year} is invalid " +
                                          $"since the '{form10K.Statute.Name}' is from " +
                                          $"the year '{form10K.Statute.Year}'");

                _annualReports.Add(form10K);
            }
            form10K = _annualReports.First(x => x.FilingDate.Year - 1 == year);

            if (form10K.FinancialData == null)
                form10K.FinancialData = new ComFinancialData {FiscalYear = year};
            if(form10K.FinancialData.Assets == null)
                form10K.FinancialData.Assets = new NetConAssets();
            if(form10K.FinancialData.Income == null)
                form10K.FinancialData.Income = new NetConIncome();

            return form10K;
        }

        /// <summary>
        /// Merges the balance sheet data contained in <see cref="webResponseBody"/> into the instance of <see cref="PublicCorporation"/>
        /// </summary>
        /// <param name="webResponseBody">The raw html from a web request.</param>
        /// <param name="srcUri"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static bool TryMergeAssets(object webResponseBody, Uri srcUri, ref PublicCorporation pc)
        {
            try
            {
                if (pc == null)
                    return false;
                var myDynData = Etx.DynamicDataFactory(srcUri);
                var myDynDataRslt = myDynData.ParseContent(webResponseBody);
                if (myDynDataRslt == null || myDynDataRslt.Count <= 0)
                    return false;

                foreach (var cd in myDynDataRslt)
                {
                    var tenK = pc.GetForm10KByYear((int)cd.FiscalYearEndAt);
                    tenK.FinancialData.Assets.TotalAssets = new Pecuniam((decimal)cd.TotalAssets * ONE_THOUSAND);
                    tenK.FinancialData.Assets.TotalLiabilities = new Pecuniam((decimal) cd.TotalLiabilities*ONE_THOUSAND);
                    tenK.FinancialData.Assets.Src = myDynData.SourceUri.ToString();
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
        public static bool TryMergeIncome(object webResponseBody, Uri srcUri, ref PublicCorporation pc)
        {
            try
            {
                if (pc == null)
                    return false;
                var myDynData = Etx.DynamicDataFactory(srcUri);
                var myDynDataRslt = myDynData.ParseContent(webResponseBody);
                if (myDynDataRslt == null || myDynDataRslt.Count <= 0)
                    return false;

                foreach (var cd in myDynDataRslt)
                {
                    var tenK = pc.GetForm10KByYear((int) cd.FiscalYearEndAt);
                    tenK.FinancialData.Income.Revenue = new Pecuniam((Decimal) cd.TotalRevenue * ONE_THOUSAND);
                    tenK.FinancialData.Income.OperatingIncome = new Pecuniam((Decimal) cd.OperatingIncomeorLoss * ONE_THOUSAND);
                    tenK.FinancialData.Income.NetIncome = new Pecuniam((Decimal) cd.NetIncome * ONE_THOUSAND);
                    tenK.FinancialData.Income.Src = myDynData.SourceUri.ToString();
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
        public static bool TryMergeTickerLookup(object webResponseBody, Uri srcUri, ref PublicCorporation pc)
        {
            try
            {
                var myDynData = Etx.DynamicDataFactory(srcUri);
                var myDynDataRslt = myDynData.ParseContent(webResponseBody);
                if (myDynDataRslt == null)
                    return false;

                foreach (var dd in myDynDataRslt)
                {
                    pc.TickerSymbols.Add(new Ticker
                    {
                        Symbol = dd.Symbol,
                        InstrumentType = dd.InstrumentType,
                        Country = dd.Country,
                        Src = myDynData.SourceUri.ToString()
                    });
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
            var firmName = Name;
            if (GetType() == typeof(Bank))
            {
                firmName = ((Bank) this).FedRptBankName;
            }
            var myAssocNodes =
                xrefXml?.SelectNodes(
                    $"//x-ref-group[@data-type='{GetType().FullName}']//x-ref-id[text()='{firmName}']/../../add");
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