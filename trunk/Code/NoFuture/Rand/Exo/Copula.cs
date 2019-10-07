using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NoFuture.Rand.Com;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Exo.NfXml;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Geo.US;
using NoFuture.Rand.Gov.US.Fed;
using NoFuture.Rand.Gov.US.Irs;
using NoFuture.Rand.Gov.US.Sec;
using NoFuture.Rand.Org;
using NoFuture.Rand.Tele;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Exo
{
    /// <summary>
    /// Joins dynamic data, which may take almost any form, from source(s) and either 
    /// merges it to instantiated Nf Rand types or operates like a factory creating 
    /// the Nf Rand type.
    /// </summary>
    public static class Copula
    {
        /// <summary>
        /// Adds in company data returned from a call to the IEX Company RESTful method
        /// </summary>
        /// <param name="rawJsonContent">The raw JSON returned from IEX</param>
        /// <param name="srcUri"></param>
        /// <param name="corp"></param>
        /// <returns></returns>
        public static bool TryParseIexCompanyJson(string rawJsonContent, Uri srcUri, ref PublicCompany corp)
        {
            if (string.IsNullOrWhiteSpace(rawJsonContent))
                return false;
            if (corp == null)
                return false;

            var myDynData = DynamicDataFactory.GetDataParser(srcUri);
            var myDynDataRslt = myDynData.ParseContent(rawJsonContent);

            if (myDynDataRslt == null || !myDynDataRslt.Any())
                return false;

            var pd = myDynDataRslt.First();
            var tickerSymbol = pd.symbol.Value;
            var exchange = pd.exchange.Value;
            var description = pd.description.Value;
            var website = pd.website.Value;

            if (string.IsNullOrWhiteSpace(corp.Name))
                corp.Name = pd.companyName.Value;

            var nfTicker = corp.TickerSymbols.FirstOrDefault(t => t.Symbol == tickerSymbol) ??
                           new TickerSymbol {Symbol = tickerSymbol, Src = srcUri.ToString()};
            nfTicker.Exchange = exchange;
            if(corp.TickerSymbols.All(t => t.Symbol != tickerSymbol))
                corp.AddTickerSymbol(nfTicker);
            corp.Description = description;
            corp.AddUri((string)website);

            return true;
        }

        /// <summary>
        /// Adds in key statistics data returned from a call to the IEX keystats RESTful method into the <see cref="corp"/>&apos;s URI collection.
        /// </summary>
        /// <param name="rawJsonContent">The raw JSON returned from IEX</param>
        /// <param name="srcUri"></param>
        /// <param name="corp"></param>
        /// <param name="atTime">Optional, allows caller to assign the timestamp, defaults to universal time</param>
        /// <returns></returns>
        public static bool TryParseIexKeyStatsJson(string rawJsonContent, Uri srcUri, ref PublicCompany corp, DateTime? atTime = null)
        {
            if (string.IsNullOrWhiteSpace(rawJsonContent))
                return false;
            if (corp == null)
                return false;

            var dt = Util.Core.Etc.ToUnixTime(atTime.GetValueOrDefault(DateTime.UtcNow));

            var myDynData = DynamicDataFactory.GetDataParser(srcUri);
            var myDynDataRslt = myDynData.ParseContent(rawJsonContent);

            if (myDynDataRslt == null || !myDynDataRslt.Any())
                return false;

            var pd = myDynDataRslt.First();
            var symbol = pd.symbol;

            if (string.IsNullOrWhiteSpace(corp.Name))
                corp.Name = pd.companyName;
            corp.AddUri(new NetUri {Value = $"iex://keystats/{symbol}/year5ChangePercent?value={pd.year5ChangePercent}&dt={dt}"});
            corp.AddUri(new NetUri {Value = $"iex://keystats/{symbol}/year2ChangePercent?value={pd.year2ChangePercent}&dt={dt}"});
            corp.AddUri(new NetUri {Value = $"iex://keystats/{symbol}/year1ChangePercent?value={pd.year1ChangePercent}&dt={dt}"});
            corp.AddUri(new NetUri {Value = $"iex://keystats/{symbol}/day200MovingAvg?value={pd.day200MovingAvg}&dt={dt}"});
            corp.AddUri(new NetUri {Value = $"iex://keystats/{symbol}/day50MovingAvg?value={pd.day50MovingAvg}&dt={dt}"});
            corp.AddUri(new NetUri {Value = $"iex://keystats/{symbol}/beta?value={pd.beta}&dt={dt}"});
            corp.AddUri(new NetUri {Value = $"iex://keystats/{symbol}/week52high?value={pd.week52high}&dt={dt}"});
            corp.AddUri(new NetUri {Value = $"iex://keystats/{symbol}/week52low?value={pd.week52low}&dt={dt}"});
            corp.AddUri(new NetUri {Value = $"iex://keystats/{symbol}/week52change?value={pd.week52change}&dt={dt}"});

            return true;
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
            if (string.IsNullOrWhiteSpace(firmOut.Rssd?.ToString()))
                firmOut.Rssd = new ResearchStatisticsSupervisionDiscount { Value = pd.Rssd };

            firmOut.AddName(KindsOfNames.Legal, pd.BankName);
            if (string.IsNullOrWhiteSpace(firmOut.FdicNumber?.ToString()))
                firmOut.FdicNumber = new FdicNum { Value = pd.FdicCert, Src = myDynData.SourceUri.ToString() };

            return !string.IsNullOrWhiteSpace(pd.RoutingNumber);
        }

        /// <summary>
        /// Parses the web response html content from <see cref="SecForm.HtmlFormLink"/> 
        /// locating the .xml Uri therein.
        /// </summary>
        /// <param name="webResponseBody"></param>
        /// <param name="srcUri"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static bool TryGetXmlLink(object webResponseBody, Uri srcUri, ref PublicCompany pc)
        {
            var pcAnnualRpt = pc.SecReports?.FirstOrDefault(x => x.HtmlFormLink == srcUri);
            if (pcAnnualRpt == null)
                return false;
            var myDynData = DynamicDataFactory.GetDataParser(srcUri);
            var myDynDataRslt = myDynData.ParseContent(webResponseBody);
            if (myDynDataRslt == null || !myDynDataRslt.Any())
                return false;

            var xrblUriStr = myDynDataRslt.First().XrblUri;

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
        public static bool TryMergeXbrlInto10K(object webResponseBody, Uri srcUri, ref PublicCompany pc)
        {
            var rptTenK =
                pc?.SecReports.FirstOrDefault(x => x is Form10K && ((Form10K)x).XmlLink == srcUri) as Form10K;
            if (rptTenK == null)
                return false;

            var myDynData = DynamicDataFactory.GetDataParser(srcUri);
            var myDynDataRslt = myDynData.ParseContent(webResponseBody);
            if (myDynDataRslt == null || !myDynDataRslt.Any())
                return false;

            var xbrlDyn = myDynDataRslt.First();
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
                pc.AddTickerSymbol(new TickerSymbol { Symbol = ticker, Country = "USA", Src = srcUri.ToString()});
            }

            var legalName = xbrlDyn.Name;
            pc.AddName(KindsOfNames.Legal, legalName);

            rptTenK.NumOfShares = xbrlDyn.NumOfShares;
            if (xbrlDyn.EndOfYear > 0)
                pc.FiscalYearEndDay = xbrlDyn.EndOfYear;

            var assets = xbrlDyn.Assets as List<Tuple<int, decimal>>;
            var rptVal = assets?.OrderByDescending(x => x.Item1).FirstOrDefault();
            if (rptVal != null)
            {
                rptTenK.TotalAssets = rptVal.Item2;
                rptTenK.FiscalYear = rptTenK.FiscalYear < rptVal.Item1
                    ? rptVal.Item1
                    : rptTenK.FiscalYear;
            }

            var lias = xbrlDyn.Liabilities as List<Tuple<int, decimal>>;
            rptVal = lias?.OrderByDescending(x => x.Item1).FirstOrDefault();
            if (rptVal != null)
            {
                rptTenK.TotalLiabilities = rptVal.Item2;
                rptTenK.FiscalYear = rptTenK.FiscalYear < rptVal.Item1
                    ? rptVal.Item1
                    : rptTenK.FiscalYear;
            }

            var nis = xbrlDyn.NetIncome as List<Tuple<int, decimal>>;
            rptVal = nis?.OrderByDescending(x => x.Item1).FirstOrDefault();
            if (rptVal != null)
            {
                rptTenK.NetIncome = rptVal.Item2;
                rptTenK.FiscalYear = rptTenK.FiscalYear < rptVal.Item1
                    ? rptVal.Item1
                    : rptTenK.FiscalYear;
            }

            var ois = xbrlDyn.OperatingIncome as List<Tuple<int, decimal>>;
            rptVal = ois?.OrderByDescending(x => x.Item1).FirstOrDefault();
            if (rptVal != null)
            {
                rptTenK.OperatingIncome = rptVal.Item2;
                rptTenK.FiscalYear = rptTenK.FiscalYear < rptVal.Item1
                    ? rptVal.Item1
                    : rptTenK.FiscalYear;
            }

            var revs = xbrlDyn.Revenue as List<Tuple<int, decimal>>;
            rptVal = revs?.OrderByDescending(x => x.Item1).FirstOrDefault();
            if (rptVal != null)
            {
                rptTenK.Revenue = rptVal.Item2;
                rptTenK.FiscalYear = rptTenK.FiscalYear < rptVal.Item1
                    ? rptVal.Item1
                    : rptTenK.FiscalYear;
            }

            rptTenK.Src = srcUri?.ToString();

            if(xbrlDyn.TextBlocks is IEnumerable<Tuple<string, string>> textBlocks && textBlocks.Any())
                rptTenK.AddRangeTextBlocks(textBlocks);

            return true;
        }

        /// <summary>
        /// Merges the ticker symbol data contained in <see cref="webResponseBody"/> into the instance of <see cref="PublicCompany"/>
        /// </summary>
        /// <param name="webResponseBody"></param>
        /// <param name="srcUri"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static bool TryMergeTickerLookup(object webResponseBody, Uri srcUri, ref PublicCompany pc)
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
                    pc.AddTickerSymbol(new TickerSymbol
                    {
                        Symbol = dd.Symbol,
                        InstrumentType = dd.InstrumentType,
                        Country = dd.Country,
                        Src = myDynData.SourceUri.ToString()
                    });
                }

                return pc.TickerSymbols.ToArray().Length > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Parses the xml (atom) content from the SEC&apos;s full-text search into and 
        /// array of corporations.
        /// </summary>
        /// <param name="rssContent"></param>
        /// <param name="srcUri"></param>
        /// <returns></returns>
        public static PublicCompany[] ParseSecEdgarFullTextSearch(string rssContent, Uri srcUri)
        {
            if (String.IsNullOrWhiteSpace(rssContent))
                return null;

            var myDynData = DynamicDataFactory.GetDataParser(srcUri);
            var myDynDataRslts = myDynData.ParseContent(rssContent);

            if (myDynDataRslts == null || !myDynDataRslts.Any<object>())
                return null;

            var corporations = new List<PublicCompany>();
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
                    : new PublicCompany();

                corp.AddName(KindsOfNames.Legal, corpName);

                //annual report
                var secForm = nameData?.Item1 ?? new Form10K { Src = myDynData.SourceUri.ToString() };

                if (titleNode.StartsWith(SecForm.NOTIFICATION_OF_INABILITY_TO_TIMELY_FILE))
                    secForm.IsLate = true;

                var parseRslt = DateTime.UtcNow;
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

                corp.AddSecReport(secForm);
                if (!corporations.Any(x => String.Equals(x.Name, corpName, StringComparison.OrdinalIgnoreCase)))
                    corporations.Add(corp);
            }
            return corporations.ToArray();

        }

        /// <summary>
        /// Try to parse the xml (atom) content from the SEC returned from a search on a CIK.
        /// </summary>
        /// <param name="xmlContent"></param>
        /// <param name="srcUri"></param>
        /// <param name="publicCompany"></param>
        /// <returns></returns>
        public static bool TryParseSecEdgarCikSearch(string xmlContent, Uri srcUri, ref PublicCompany publicCompany)
        {
            if (String.IsNullOrWhiteSpace(xmlContent))
                return false;

            var myDynData = DynamicDataFactory.GetDataParser(srcUri);
            var myDynDataRslt = myDynData.ParseContent(xmlContent);

            if (myDynDataRslt == null || !myDynDataRslt.Any<dynamic>())
                return false;

            var pr = myDynDataRslt.First<dynamic>();

            if (publicCompany == null)
                publicCompany = new PublicCompany();

            if (!String.IsNullOrWhiteSpace(pr.Name))
                publicCompany.AddName(KindsOfNames.Legal, pr.Name);
            publicCompany.CIK = String.IsNullOrWhiteSpace(pr.Cik)
                ? publicCompany.CIK
                : new CentralIndexKey { Value = pr.Cik, Src = myDynData.SourceUri.ToString() };
            publicCompany.SIC = String.IsNullOrWhiteSpace(pr.Sic)
                ? publicCompany.SIC
                : new StandardIndustryClassification { Value = pr.Sic, Src = myDynData.SourceUri.ToString() };

            publicCompany.UsStateOfIncorporation = pr.IncorpState;

            if (publicCompany.SIC != null && !String.IsNullOrWhiteSpace(pr.SicDesc))
                publicCompany.SIC.Description = pr.SicDesc;

            var bizAddr = new AddressData();
            if (!String.IsNullOrWhiteSpace(pr.BizAddrStreet))
            {
                UsStreetPo temp;
                if (UsStreetPo.TryParse(pr.BizAddrStreet, out temp))
                    bizAddr = temp.GetData();
            }

            if (!String.IsNullOrWhiteSpace(pr.BizAddrCity))
                bizAddr.Locality = pr.BizAddrCity;

            if (!String.IsNullOrWhiteSpace(pr.BizAddrState))
                bizAddr.RegionAbbrev = pr.BizAddrState;

            if (!String.IsNullOrWhiteSpace(pr.BizPostalCode))
                bizAddr.PostalCode = pr.BizPostalCode;

            publicCompany.BusinessAddress = new PostalAddress
            {
                Street = new UsStreetPo(bizAddr) {Src = myDynData.SourceUri.ToString()},
                CityArea = new UsCityStateZip(bizAddr) {Src = myDynData.SourceUri.ToString()}
            };

            var mailAddr = new AddressData();
            if (!String.IsNullOrWhiteSpace(pr.MailAddrStreet))
            {
                UsStreetPo temp;
                if (UsStreetPo.TryParse(pr.MailAddrStreet, out temp))
                    mailAddr = temp.GetData();
            }

            if (!String.IsNullOrWhiteSpace(pr.MailAddrCity))
                mailAddr.Locality = pr.MailAddrCity;

            if (!String.IsNullOrWhiteSpace(pr.MailAddrState))
                mailAddr.RegionAbbrev = pr.MailAddrState;

            if (!String.IsNullOrWhiteSpace(pr.MailPostalCode))
                mailAddr.PostalCode = pr.MailPostalCode;

            publicCompany.MailingAddress = new PostalAddress
            {
                Street = new UsStreetPo(mailAddr) {Src = myDynData.SourceUri.ToString()},
                CityArea = new UsCityStateZip(mailAddr) {Src = myDynData.SourceUri.ToString()}
            };

            NorthAmericanPhone phOut;
            if (NorthAmericanPhone.TryParse(pr.BizPhone, out phOut) && publicCompany.PhoneNumbers.All(x => !x.Equals(phOut)))
            {
                phOut.Src = myDynData.SourceUri.ToString();
                publicCompany.AddPhone(phOut);
            }

            if (NorthAmericanPhone.TryParse(pr.MailPhone, out phOut) && publicCompany.PhoneNumbers.All(x => !x.Equals(phOut)))
            {
                phOut.Src = myDynData.SourceUri.ToString();
                publicCompany.AddPhone(phOut);
            }

            if (pr.FormerNames != null)
            {
                foreach (var fn in pr.FormerNames)
                {
                    var strFnDt = fn.FormerDate ?? String.Empty;
                    var strFnVal = fn.FormerName ?? String.Empty;

                    publicCompany.AddName(KindsOfNames.Former, $"{strFnVal}[{strFnDt}]");
                }
            }

            if (pr.FiscalYearEnd != null && pr.FiscalYearEnd.Length == 4)
            {
                int fyed;
                if (TryGetDayOfYearFiscalEnd(pr.FiscalYearEnd, out fyed))
                    publicCompany.FiscalYearEndDay = fyed;
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