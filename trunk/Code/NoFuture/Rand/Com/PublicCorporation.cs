using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Gov.Irs;
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
        private readonly List<SecForm> _secReports = new List<SecForm>();
        private List<Ticker> _tickerSymbols = new List<Ticker>();
        #endregion

        #region properties
        public EmployerIdentificationNumber EIN { get; set; }
        public CentralIndexKey CIK { get; set; }
        public List<SecForm> SecReports => _secReports;
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
        public string UrlEncodedName => Uri.EscapeUriString(Cusip.GetSearchCompanyName(Name));

        #endregion

        #region methods

        /// <summary>
        /// Parses the web response html content from <see cref="SecForm.HtmlFormLink"/> 
        /// locating the .xml Uri therein.
        /// </summary>
        /// <param name="webResponseBody"></param>
        /// <param name="srcUri"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static bool TryGetXmlLink(object webResponseBody, Uri srcUri, ref PublicCorporation pc)
        {
            var pcAnnualRpt = pc.SecReports?.FirstOrDefault(x => x.HtmlFormLink == srcUri);
            if (pcAnnualRpt == null)
                return false;
            var myDynData = Facit.DynamicDataFactory(srcUri);
            var myDynDataRslt = myDynData.ParseContent(webResponseBody);
            if (myDynDataRslt == null || !myDynDataRslt.Any())
                return false;

            var xrblUriStr = myDynDataRslt.First().XrblUri;

            pcAnnualRpt.XmlLink = new Uri(xrblUriStr);
            var irsId = myDynDataRslt.First().IrsId as string;
            if(!string.IsNullOrWhiteSpace(irsId))
                pc.EIN = new EmployerIdentificationNumber {Value = irsId};

            return true;
        }

        /// <summary>
        /// Merges the XBRL xml from the SEC filing for the given <see cref="pc"/> 
        /// </summary>
        /// <param name="webResponseBody">The raw XML content from the SEC</param>
        /// <param name="srcUri"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static bool TryMergeXbrlInto10K(object webResponseBody, Uri srcUri, ref PublicCorporation pc)
        {
            var rptTenK =
                pc?.SecReports.FirstOrDefault(x => x is Form10K && ((Form10K)x).XmlLink == srcUri) as Form10K;
            if (rptTenK == null)
                return false;

            var myDynData = Facit.DynamicDataFactory(srcUri);
            var myDynDataRslt = myDynData.ParseContent(webResponseBody);
            if (myDynDataRslt == null || !myDynDataRslt.Any())
                return false;

            if (rptTenK.FinancialData == null)
                rptTenK.FinancialData = new ComFinancialData
                {
                    Assets = new NetConAssets(),
                    Income = new NetConIncome()
                };

            var xbrlDyn = myDynDataRslt.First();
            var cik = xbrlDyn.Cik;
            if (pc.CIK.Value != cik)
                return false;
            if (rptTenK.CIK == null)
                rptTenK.CIK = new CentralIndexKey {Value = cik};

            var ticker = xbrlDyn.Ticker ??
                         srcUri?.LocalPath.Split('/').LastOrDefault()?.Split('-').FirstOrDefault()?.ToUpper();

            if (ticker != null &&
                pc.TickerSymbols.All(x => !string.Equals(x.Symbol, ticker, StringComparison.OrdinalIgnoreCase)))
            {
                ticker = ticker.ToUpper();
                pc.TickerSymbols.Add(new Ticker {Symbol = ticker, Country = "USA"});
            }

            var legalName = xbrlDyn.Name;
            pc.UpsertName(KindsOfNames.Legal, legalName);

            rptTenK.FinancialData.NumOfShares = xbrlDyn.NumOfShares;
            if (xbrlDyn.EndOfYear > 0)
                pc.FiscalYearEndDay = xbrlDyn.EndOfYear;

            var assets = xbrlDyn.Assets as List<Tuple<int, decimal>>;
            var rptVal = assets?.OrderByDescending(x => x.Item1).FirstOrDefault();
            if (rptVal != null)
            {
                rptTenK.FinancialData.Assets.TotalAssets = new Pecuniam(rptVal.Item2);
                rptTenK.FinancialData.FiscalYear = rptTenK.FinancialData.FiscalYear < rptVal.Item1
                    ? rptVal.Item1
                    : rptTenK.FinancialData.FiscalYear;
            }

            var lias = xbrlDyn.Liabilities as List<Tuple<int, decimal>>;
            rptVal = lias?.OrderByDescending(x => x.Item1).FirstOrDefault();
            if (rptVal != null)
            {
                rptTenK.FinancialData.Assets.TotalLiabilities = new Pecuniam(rptVal.Item2);
                rptTenK.FinancialData.FiscalYear = rptTenK.FinancialData.FiscalYear < rptVal.Item1
                    ? rptVal.Item1
                    : rptTenK.FinancialData.FiscalYear;
            }

            var nis = xbrlDyn.NetIncome as List<Tuple<int, decimal>>;
            rptVal = nis?.OrderByDescending(x => x.Item1).FirstOrDefault();
            if (rptVal != null)
            {
                rptTenK.FinancialData.Income.NetIncome = new Pecuniam(rptVal.Item2);
                rptTenK.FinancialData.FiscalYear = rptTenK.FinancialData.FiscalYear < rptVal.Item1
                    ? rptVal.Item1
                    : rptTenK.FinancialData.FiscalYear;
            }

            var ois = xbrlDyn.OperatingIncome as List<Tuple<int, decimal>>;
            rptVal = ois?.OrderByDescending(x => x.Item1).FirstOrDefault();
            if (rptVal != null)
            {
                rptTenK.FinancialData.Income.OperatingIncome = new Pecuniam(rptVal.Item2);
                rptTenK.FinancialData.FiscalYear = rptTenK.FinancialData.FiscalYear < rptVal.Item1
                    ? rptVal.Item1
                    : rptTenK.FinancialData.FiscalYear;
            }

            var revs = xbrlDyn.Revenue as List<Tuple<int, decimal>>;
            rptVal = revs?.OrderByDescending(x => x.Item1).FirstOrDefault();
            if (rptVal != null)
            {
                rptTenK.FinancialData.Income.Revenue = new Pecuniam(rptVal.Item2);
                rptTenK.FinancialData.FiscalYear = rptTenK.FinancialData.FiscalYear < rptVal.Item1
                    ? rptVal.Item1
                    : rptTenK.FinancialData.FiscalYear;
            }

            rptTenK.FinancialData.Income.Src = srcUri.ToString();
            rptTenK.FinancialData.Assets.Src = srcUri.ToString();

            return true;
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
                var myDynData = Facit.DynamicDataFactory(srcUri);
                var myDynDataRslt = myDynData.ParseContent(webResponseBody);
                if (myDynDataRslt == null)
                    return false;

                foreach (var dd in myDynDataRslt)
                {
                    var existing = pc.TickerSymbols.FirstOrDefault(x => x.Symbol == dd.Symbol && x.Country == dd.Country);
                    if (existing != null)
                    {
                        existing.Src = myDynData.SourceUri.ToString();
                        existing.InstrumentType = dd.InstrumentType;
                        continue;
                    }
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
            var firmName = Name;
            if (GetType() == typeof(Bank))
            {
                firmName = GetName(KindsOfNames.Abbrev) ?? Name;
            }
            var myAssocNodes = XRefGroup.GetXrefAddNodes(GetType(), firmName);
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