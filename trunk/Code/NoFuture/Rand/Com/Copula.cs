using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.Irs;
using NoFuture.Rand.Gov.Sec;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Com
{
    /// <summary>
    /// Joins dynamic data from source to the Nf Rand type
    /// </summary>
    public static class Copula
    {
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
            var myDynData = DynamicDataFactory.GetDataParser(srcUri);
            var myDynDataRslt = myDynData.ParseContent(webResponseBody);
            if (myDynDataRslt == null || !myDynDataRslt.Any<dynamic>())
                return false;

            var xrblUriStr = myDynDataRslt.First<dynamic>().XrblUri;

            pcAnnualRpt.XmlLink = new Uri(xrblUriStr);
            var irsId = myDynDataRslt.First<dynamic>().IrsId as string;
            if (!String.IsNullOrWhiteSpace(irsId))
                pc.EIN = new EmployerIdentificationNumber { Value = irsId };

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

            var myDynData = DynamicDataFactory.GetDataParser(srcUri);
            var myDynDataRslt = myDynData.ParseContent(webResponseBody);
            if (myDynDataRslt == null || !myDynDataRslt.Any<dynamic>())
                return false;

            if (rptTenK.FinancialData == null)
                rptTenK.FinancialData = new ComFinancialData
                {
                    Assets = new NetConAssets(),
                    Income = new NetConIncome()
                };

            var xbrlDyn = myDynDataRslt.First<dynamic>();
            var cik = xbrlDyn.Cik;
            if (pc.CIK.Value != cik)
                return false;
            if (rptTenK.CIK == null)
                rptTenK.CIK = new CentralIndexKey { Value = cik };

            var ticker = xbrlDyn.Ticker ??
                         srcUri?.LocalPath.Split('/').LastOrDefault()?.Split('-').FirstOrDefault()?.ToUpper();

            if (ticker != null &&
                pc.TickerSymbols.All(x => !String.Equals(x.Symbol, ticker, StringComparison.OrdinalIgnoreCase)))
            {
                ticker = ticker.ToUpper();
                pc.TickerSymbols.Add(new Ticker { Symbol = ticker, Country = "USA" });
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
                var myDynData = DynamicDataFactory.GetDataParser(srcUri);
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

        public static PublicCorporation[] ParseSecEdgarFullTextSearch(string rssContent, Uri srcUri)
        {
            if (String.IsNullOrWhiteSpace(rssContent))
                return null;

            var myDynData = DynamicDataFactory.GetDataParser(srcUri);
            var myDynDataRslts = myDynData.ParseContent(rssContent);

            if (myDynDataRslts == null || !myDynDataRslts.Any<object>())
                return null;

            var corporations = new List<PublicCorporation>();
            foreach (var dd in myDynDataRslts)
            {

                var titleNode = dd.Title;

                if (titleNode == null || String.IsNullOrWhiteSpace(titleNode))
                    continue;

                var idNode = dd.Id;
                var linkVal = dd.Link;
                var formDtNode = dd.Update;
                var summaryNode = dd.Summary;

                //this will be an id while looping
                var nameData = ParseNameFromTitle(titleNode) as Tuple<SecForm, string>;
                var corpName = nameData?.Item2;
                var corp = corporations.Any(x => String.Equals(x.Name, corpName, StringComparison.OrdinalIgnoreCase))
                    ? corporations.First(
                        x => String.Equals(x.Name, corpName, StringComparison.OrdinalIgnoreCase))
                    : new PublicCorporation();

                corp.UpsertName(KindsOfNames.Legal, corpName);

                //annual report
                var secForm = nameData?.Item1 ?? new Form10K { Src = myDynData.SourceUri.ToString() };

                if (titleNode.StartsWith(SecForm.NotificationOfInabilityToTimelyFile))
                    secForm.IsLate = true;

                var parseRslt = DateTime.Now;
                if (formDtNode != null && !String.IsNullOrWhiteSpace(formDtNode) &&
                    DateTime.TryParse(formDtNode.ToString(), out parseRslt))
                {
                    secForm.FilingDate = parseRslt;
                }

                secForm.HtmlFormLink = new Uri(Edgar.SEC_ROOT_URL + linkVal);
                //this is only presented within the html link s
                corp.CIK = new CentralIndexKey { Value = idNode == String.Empty ? String.Empty : ParseCikFromUriPath(linkVal) };

                if (summaryNode != null && !String.IsNullOrWhiteSpace(summaryNode))
                {
                    var summaryText = summaryNode;
                    secForm.AccessionNumber = ParseAccessionNumFromSummary(summaryText);
                    secForm.CIK = corp.CIK;
                }

                corp.SecReports.Add(secForm);
                if (!corporations.Any(x => String.Equals(x.Name, corpName, StringComparison.OrdinalIgnoreCase)))
                    corporations.Add(corp);
            }
            return corporations.ToArray();

        }

        public static bool TryParseSecEdgarCikSearch(string xmlContent, Uri srcUri, ref PublicCorporation publicCorporation)
        {
            if (String.IsNullOrWhiteSpace(xmlContent))
                return false;

            var myDynData = DynamicDataFactory.GetDataParser(srcUri);
            var myDynDataRslt = myDynData.ParseContent(xmlContent);

            if (myDynDataRslt == null || !myDynDataRslt.Any<dynamic>())
                return false;

            var pr = myDynDataRslt.First<dynamic>();

            if (publicCorporation == null)
                publicCorporation = new PublicCorporation();

            if (!String.IsNullOrWhiteSpace(pr.Name))
                publicCorporation.UpsertName(KindsOfNames.Legal, pr.Name);
            publicCorporation.CIK = String.IsNullOrWhiteSpace(pr.Cik)
                ? publicCorporation.CIK
                : new CentralIndexKey { Value = pr.Cik, Src = myDynData.SourceUri.ToString() };
            publicCorporation.SIC = String.IsNullOrWhiteSpace(pr.Sic)
                ? publicCorporation.SIC
                : new StandardIndustryClassification { Value = pr.Sic, Src = myDynData.SourceUri.ToString() };

            publicCorporation.UsStateOfIncorporation = String.IsNullOrWhiteSpace(pr.IncorpState)
                ? publicCorporation.UsStateOfIncorporation
                : UsState.GetStateByPostalCode(pr.IncorpState);

            if (publicCorporation.SIC != null && !String.IsNullOrWhiteSpace(pr.SicDesc))
                publicCorporation.SIC.Description = pr.SicDesc;

            var bizAddr = new AddressData();
            if (!String.IsNullOrWhiteSpace(pr.BizAddrStreet))
            {
                UsStreetPo temp;
                if (UsStreetPo.TryParse(pr.BizAddrStreet, out temp))
                    bizAddr = temp.Data;
            }

            if (!String.IsNullOrWhiteSpace(pr.BizAddrCity))
                bizAddr.City = pr.BizAddrCity;

            if (!String.IsNullOrWhiteSpace(pr.BizAddrState))
                bizAddr.StateAbbrv = pr.BizAddrState;

            if (!String.IsNullOrWhiteSpace(pr.BizPostalCode))
                bizAddr.PostalCode = pr.BizPostalCode;

            publicCorporation.BusinessAddress =
                new Tuple<UsStreetPo, UsCityStateZip>(new UsStreetPo(bizAddr) { Src = myDynData.SourceUri.ToString() },
                    new UsCityStateZip(bizAddr, false) { Src = myDynData.SourceUri.ToString() });

            var mailAddr = new AddressData();
            if (!String.IsNullOrWhiteSpace(pr.MailAddrStreet))
            {
                UsStreetPo temp;
                if (UsStreetPo.TryParse(pr.MailAddrStreet, out temp))
                    mailAddr = temp.Data;
            }

            if (!String.IsNullOrWhiteSpace(pr.MailAddrCity))
                mailAddr.City = pr.MailAddrCity;

            if (!String.IsNullOrWhiteSpace(pr.MailAddrState))
                mailAddr.StateAbbrv = pr.MailAddrState;

            if (!String.IsNullOrWhiteSpace(pr.MailPostalCode))
                mailAddr.PostalCode = pr.MailPostalCode;

            publicCorporation.MailingAddress = new Tuple<UsStreetPo, UsCityStateZip>(new UsStreetPo(mailAddr) { Src = myDynData.SourceUri.ToString() },
                new UsCityStateZip(mailAddr, false) { Src = myDynData.SourceUri.ToString() });

            var phs = new List<NorthAmericanPhone>();
            if (publicCorporation.Phone != null && publicCorporation.Phone.Length > 0)
            {
                phs.AddRange(publicCorporation.Phone);
            }
            NorthAmericanPhone phOut;
            if (NorthAmericanPhone.TryParse(pr.BizPhone, out phOut) && phs.All(x => !x.Equals(phOut)))
            {
                phOut.Src = myDynData.SourceUri.ToString();
                phs.Add(phOut);
            }

            if (NorthAmericanPhone.TryParse(pr.MailPhone, out phOut) && phs.All(x => !x.Equals(phOut)))
            {
                phOut.Src = myDynData.SourceUri.ToString();
                phs.Add(phOut);
            }
            publicCorporation.Phone = phs.ToArray();

            if (pr.FormerNames != null)
            {
                foreach (var fn in pr.FormerNames)
                {
                    var strFnDt = fn.FormerDate ?? String.Empty;
                    var strFnVal = fn.FormerName ?? String.Empty;

                    publicCorporation.UpsertName(KindsOfNames.Former, $"{strFnVal}[{strFnDt}]");
                }
            }

            if (pr.FiscalYearEnd != null && pr.FiscalYearEnd.Length == 4)
            {
                int fyed;
                if (TryGetDayOfYearFiscalEnd(pr.FiscalYearEnd, out fyed))
                    publicCorporation.FiscalYearEndDay = fyed;
            }

            return true;
        }

        internal static string ParseAccessionNumFromSummary(string summaryText)
        {
            if (String.IsNullOrWhiteSpace(summaryText))
                return String.Empty;

            if (!summaryText.Contains("<b>"))
                return String.Empty;

            var summaryParts = summaryText.Split(new[] { "<b>" }, StringSplitOptions.RemoveEmptyEntries);
            if (!summaryParts.Any(x => x.Contains("Accession Number")))
                return String.Empty;

            var accessiontext = summaryParts.First(x => x.Contains("Accession Number"));
            if (!accessiontext.Contains(">"))
                return String.Empty;
            summaryParts = accessiontext.Split('>');
            if (summaryParts.Length <= 1)
                return String.Empty;
            return summaryParts[1].Trim();

        }

        internal static Tuple<SecForm, string> ParseNameFromTitle(string titleText)
        {
            var blank = new Tuple<SecForm, string>(new Form10K(), String.Empty);
            if (String.IsNullOrWhiteSpace(titleText))
                return blank;
            if (!titleText.Contains("-"))
                return blank;
            string name;
            if (!RegexCatalog.IsRegexMatch(titleText, @"(.*)?\x20\x2D\x20(.*)?", out name, 2))
                return blank;
            string formAbbrev;
            RegexCatalog.IsRegexMatch(titleText, @"(.*)?\x20\x2D\x20(.*)?", out formAbbrev, 1);

            var secForm = SecForm.SecFormFactory(formAbbrev) ?? new Form10K();

            return new Tuple<SecForm, string>(secForm, (name ?? String.Empty).Trim());
        }

        internal static string ParseCikFromUriPath(string htmlLink)
        {
            if (String.IsNullOrWhiteSpace(htmlLink) || !htmlLink.Contains("/"))
                return String.Empty;

            var s = htmlLink.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            return s.Length <= 3 ? String.Empty : s[3];
        }

        internal static bool TryGetDayOfYearFiscalEnd(string rawValue, out int dayOfYear)
        {
            dayOfYear = 0;

            if (String.IsNullOrEmpty(rawValue))
                return false;

            rawValue = new string(rawValue.ToCharArray().Where(Char.IsDigit).ToArray());

            var sm = rawValue.Substring(0, 2);
            var sd = rawValue.Substring(2, 2);

            int month;
            int day;

            if (!Int32.TryParse(sm, out month) || month > 12)
            {
                return false;
            }

            if (!Int32.TryParse(sd, out day) || day > 31)
            {
                return false;
            }

            var year = DateTime.Today.Year;

            var fiscalDt = new DateTime(year, month, day);

            dayOfYear = fiscalDt.DayOfYear;
            return dayOfYear > 0;
        }
    }
}