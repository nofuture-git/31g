using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using NoFuture.Rand.Gov.US.Sec;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Exo.NfXml
{
    /// <summary>
    /// Parses the content of a eXtensible Business Reporting Language filed with the SEC
    /// see [https://en.wikipedia.org/wiki/XBRL]
    /// </summary>
    [Serializable]
    public class SecXbrlInstanceFile : NfDynDataBase
    {
        #region inner types
        public static class XmlNs
        {
            public const string COUNTRY = "country";
            public const string CURRENCY = "currency";
            public const string DEI = "dei";
            public const string EXCH = "exch";
            public const string INVEST = "invest";
            public const string ISO4217 = "iso4217";
            public const string LINK = "link";
            public const string NAICS = "naics";
            public const string NONNUM = "nonnum";
            public const string NUM = "num";
            public const string REF = "ref";
            public const string SIC = "sic";
            public const string STPR = "stpr";
            public const string TBI = "tbi";
            public const string US_GAAP = "us-gaap";
            public const string US_ROLES = "us-roles";
            public const string US_TYPES = "us-types";
            public const string UTREG = "utreg";
            public const string XBR_LD_I = "xbrldi";
            public const string XBR_LD_T = "xbrldt";
            public const string XBR_LI = "xbrli";
            public const string XLINK = "xlink";
            public const string XSD = "xsd";
            public const string XSI = "xsi";
            public const string ROOTNS = "xbrl";
        }
        #endregion

        #region constants
        private const string DF_YEAR_PARSE_REGEX = "((19|20)[0-9]{2})";
        #endregion

        #region fields
        private XmlNamespaceManager _nsMgr;
        private XmlDocument _xml;
        #endregion

        #region ctor
        public SecXbrlInstanceFile(Uri src):base(src) { }
        #endregion

        #region methods

        public static Uri GetUri(SecForm secForm)
        {
            return secForm?.XmlLink;
        }

        public override IEnumerable<dynamic> ParseContent(object content)
        {
            var xmlContent = content as string;
            if (string.IsNullOrWhiteSpace(xmlContent))
                return null;
            var xmlNsMgr = GetXmlAndNsMgr(xmlContent);

            if (xmlNsMgr?.Item1 == null || xmlNsMgr.Item2 == null)
                return null;

            _xml = xmlNsMgr.Item1;
            _nsMgr = xmlNsMgr.Item2;

            bool.TryParse(_xml.SelectSingleNode($"//{XmlNs.DEI}:AmendmentFlag", _nsMgr)?.InnerText ?? bool.FalseString,
                out var amendmentFlag);

            var endOfYearStr = _xml.SelectSingleNode($"//{XmlNs.DEI}:CurrentFiscalYearEndDate", _nsMgr)?.InnerText ??
                               string.Empty;
            Copula.TryGetDayOfYearFiscalEnd(endOfYearStr,out var endOfYear);

            var cik = _xml.SelectSingleNode($"//{XmlNs.DEI}:EntityCentralIndexKey", _nsMgr)?.InnerText;
            int.TryParse(
                _xml.SelectSingleNode($"//{XmlNs.DEI}:EntityCommonStockSharesOutstanding", _nsMgr)?.InnerText ?? String.Empty,
                out var numOfShares);
            var name = _xml.SelectSingleNode($"//{XmlNs.DEI}:EntityRegistrantName", _nsMgr)?.InnerText;
            var ticker = _xml.SelectSingleNode($"//{XmlNs.DEI}:TradingSymbol", _nsMgr)?.InnerText;

            var assets = GetNodeDollarYear(_xml, $"//{XmlNs.US_GAAP}:Assets", _nsMgr);
            var liabilities = GetNodeDollarYear(_xml, $"//{XmlNs.US_GAAP}:Liabilities", _nsMgr);

            var netIncome = GetNodeDollarYear(_xml, $"//{XmlNs.US_GAAP}:NetIncomeLoss",_nsMgr);

            List<Tuple<int, decimal>> operateIncome = new List<Tuple<int, decimal>>();
            foreach (var anotherName in new[] {"OperatingIncomeLoss", "BenefitsLossesAndExpenses"})
            {
                operateIncome = GetNodeDollarYear(_xml,
                    $"//{XmlNs.US_GAAP}:{anotherName}", _nsMgr);
                if (operateIncome.Any())
                    break;
            }

            List<Tuple<int, decimal>> salesRev = new List<Tuple<int, decimal>>();
            foreach (
                var anotherName in
                    new[]
                    {"Revenues", "RetailRevenue", "SalesRevenueNet", "SalesRevenueServicesNet", "SalesRevenueGoodsNet"})
            {
                salesRev = GetNodeDollarYear(_xml,
                    $"//{XmlNs.US_GAAP}:{anotherName}", _nsMgr);
                if (salesRev.Any())
                    break;
            }

            var textBlocks = GetTextBlocks();

            return new List<dynamic>
            {
                new
                {
                    IsAmended = amendmentFlag,
                    EndOfYear = endOfYear,
                    Cik = cik,
                    NumOfShares = numOfShares,
                    Name = name,
                    Ticker = ticker,
                    Assets = assets,
                    Liabilities = liabilities,
                    NetIncome = netIncome,
                    OperatingIncome = operateIncome,
                    Revenue = salesRev, 
                    TextBlocks = textBlocks
                }
            };
        }

        /// <summary>
        /// Loads the SEC XBRL xml doc with namespace manager returning both coupled together
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal static Tuple<XmlDocument, XmlNamespaceManager> GetXmlAndNsMgr(string content)
        {
            var xmlContent = content as string;
            if (string.IsNullOrWhiteSpace(xmlContent))
                return null;

            var xml = new XmlDocument();
            xml.LoadXml(xmlContent);

            //namespace url's can change from file to file
            var xmlNsDict = GetXmlNamespaces(xml);

            var nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace(XmlNs.DEI,
                xmlNsDict.ContainsKey(XmlNs.DEI) ? xmlNsDict[XmlNs.DEI] : "http://xbrl.sec.gov/dei/2014-01-31");
            nsMgr.AddNamespace(XmlNs.US_GAAP,
                xmlNsDict.ContainsKey(XmlNs.US_GAAP) ? xmlNsDict[XmlNs.US_GAAP] : "http://fasb.org/us-gaap/2015-01-31");
            if (xmlNsDict.ContainsKey("xmlns"))
            {
                nsMgr.AddNamespace(XmlNs.ROOTNS, xmlNsDict["xmlns"] ?? "http://www.xbrl.org/2003/instance");
            }
            else if (xmlNsDict.ContainsKey(XmlNs.ROOTNS))
            {
                nsMgr.AddNamespace(XmlNs.ROOTNS, xmlNsDict[XmlNs.ROOTNS] ?? "http://www.xbrl.org/2003/instance");
            }
            else if (xmlNsDict.ContainsKey(XmlNs.XBR_LI))
            {
                nsMgr.AddNamespace(XmlNs.ROOTNS, xmlNsDict[XmlNs.XBR_LI] ?? "http://www.xbrl.org/2003/instance");
            }

            return new Tuple<XmlDocument, XmlNamespaceManager>(xml, nsMgr);
        }

        /// <summary>
        /// Gets the raw text content of all the xml elements whose element name contains 
        /// the key word &apos;TextBlock&apos;
        /// </summary>
        /// <param name="xmlContent"></param>
        /// <returns>
        /// Each item represents the content of one TextBlock&apos;s InnerText content
        /// </returns>
        public IEnumerable<Tuple<string,string>> GetTextBlocks(string xmlContent = null)
        {
            if (!string.IsNullOrWhiteSpace(xmlContent))
            {
                var xmlNsMgr = GetXmlAndNsMgr(xmlContent);

                _xml = xmlNsMgr.Item1;
                _nsMgr = xmlNsMgr.Item2;
            }

            if(_xml == null || _nsMgr == null)
                throw new RahRowRagee("You must either pass in the xml content yourself," +
                                      " call this method after having made a call to ParseContent.");

            var textBlockNodes = _xml.SelectNodes("//*[contains(local-name(),'TextBlock')]");
            if (textBlockNodes == null || textBlockNodes.Count <= 0)
                return null;

            var textBlocks = new List<Tuple<string, string>>();
            for (var i = 0; i < textBlockNodes.Count; i++)
            {
                if (!(textBlockNodes[i] is XmlElement tbnElem))
                    continue;
                if (!Antlr.HTML.HtmlParseResults.TryGetCdata(tbnElem.InnerText, null, out var cdata))
                    continue;
                textBlocks.Add(new Tuple<string, string>(tbnElem.Name, string.Join("  ", cdata)));
            }

            return textBlocks;
        }

        /// <summary>
        /// Helper method to get the xml namespaces to uri from the document element
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        internal static Dictionary<string, string> GetXmlNamespaces(XmlDocument xml)
        {
            if (!xml.HasChildNodes)
                return null;
            var rootElement = xml.DocumentElement;
            if (rootElement == null)
                return null;

            //namespace url's can change from file to file
            var xmlNsDict = new Dictionary<string, string>();
            foreach (var xmlNs in rootElement.Attributes.Cast<XmlAttribute>())
            {
                var nsName = xmlNs.Name.Contains(":") ? xmlNs.Name.Split(':')[1] : xmlNs.Name;
                var nsVal = xmlNs.Value;
                if (xmlNsDict.ContainsKey(nsName))
                    continue;
                xmlNsDict.Add(nsName, nsVal);
            }

            return xmlNsDict;
        }

        /// <summary>
        /// Selects nodes on <see cref="xpath"/> and returns the year\dollar amount found for 
        /// each.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="xpath"></param>
        /// <param name="nsMgr"></param>
        /// <returns></returns>
        internal static List<Tuple<int, decimal>> GetNodeDollarYear(XmlDocument xml, string xpath,
            XmlNamespaceManager nsMgr)
        {
            var year2usd = new List<Tuple<int, decimal>>();
            if (xml == null || string.IsNullOrWhiteSpace(xpath))
                return year2usd;

            var nodes = xml.SelectNodes(xpath, nsMgr);
            if (nodes == null || nodes.Count <= 0)
                return year2usd;

            //find the shortest context ref attr value's length
            var contextRefLen = int.MaxValue-1;
            for (var i = 0; i < nodes.Count; i++)
            {
                var currContextRefLen = nodes[i]?.Attributes?["contextRef"]?.Value?.Length ?? 0;
                if (currContextRefLen >= contextRefLen)
                    continue;
                contextRefLen = currContextRefLen;
            }
            if(contextRefLen == int.MaxValue-1)
                return year2usd;

            //now only target the nodes with this context ref len
            var year2usdMoreInfo = new List<Tuple<int, decimal, string>>();
            for (var i = 0; i < nodes.Count; i++)
            {
                var contextRefVal = nodes[i]?.Attributes?["contextRef"]?.Value ?? string.Empty;
                var currContextRefLen = contextRefVal.Length;

                //allow for +-3
                var diffInLen = Math.Abs(currContextRefLen - contextRefLen);
                if (diffInLen > 4)
                    continue;

                if (TryParseDollar(nodes[i], out var usd) && TryGetYear(contextRefVal, xml, nsMgr, out var yyyy))
                {
                    year2usdMoreInfo.Add(new Tuple<int, decimal, string>(yyyy, usd, contextRefVal));
                }
            }
            if (!year2usdMoreInfo.Any())
                return year2usd;

            //decide which one to pick when more than one has same year (Item1)
            var years = year2usdMoreInfo.Select(x => x.Item1).Distinct();
            foreach (var year in years)
            {
                var byYearItems = year2usdMoreInfo.Where(x => x.Item1 == year).ToList();
                //when only one for that year just take it and go
                if (byYearItems.Count == 1)
                {
                    year2usd.Add(new Tuple<int, decimal>(byYearItems[0].Item1, byYearItems[0].Item2));
                    continue;
                }

                //when more than one look for one ending with year-to-date indicator
                var pickBySuffix = byYearItems.FirstOrDefault(x => x.Item3.EndsWith("YTD"));
                if (pickBySuffix != null)
                {
                    year2usd.Add(new Tuple<int, decimal>(pickBySuffix.Item1, pickBySuffix.Item2));
                    continue;
                }

                //look for one with a number that appears to be the days-in-a-year
                var pickWithYearWorthOfDays =
                    byYearItems.FirstOrDefault(x => Regex.IsMatch(x.Item3, "[^0-9]36[0-5][^0-9]"));
                if (pickWithYearWorthOfDays != null)
                {
                    year2usd.Add(new Tuple<int, decimal>(pickWithYearWorthOfDays.Item1, pickWithYearWorthOfDays.Item2));
                    continue;
                }

                //last, just take the first one
                var dfPick = byYearItems.FirstOrDefault(x => x.Item3.Length == contextRefLen) ??
                             byYearItems.FirstOrDefault();
                if (dfPick != null && year2usd.All(x => x.Item1 != dfPick.Item1))
                {
                    year2usd.Add(new Tuple<int, decimal>(dfPick.Item1, dfPick.Item2));
                }
            }

            return year2usd;
        }

        /// <summary>
        /// Parses the dollar amount from <see cref="node"/>
        /// </summary>
        /// <param name="node"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        internal static bool TryParseDollar(XmlNode node, out decimal val)
        {
            val = 0M;
            if (!(node is XmlElement elem))
                return false;

            if (!decimal.TryParse(elem.InnerText, out var moneyValue))
                return false;

            int.TryParse(elem.Attributes["decimals"]?.Value ?? string.Empty, out var decPls);
            for (var i = 0; i < Math.Abs(decPls); i++)
            {
                moneyValue *= 0.1M;
            }
            val = moneyValue;
            return true;
        }

        /// <summary>
        /// Determines the year for the given <see cref="contextRef"/> within this <see cref="xml"/> doc.
        /// </summary>
        /// <param name="contextRef"></param>
        /// <param name="xml"></param>
        /// <param name="nsMgr"></param>
        /// <param name="yyyy"></param>
        /// <returns></returns>
        internal static bool TryGetYear(string contextRef, XmlDocument xml, XmlNamespaceManager nsMgr, out int yyyy)
        {
            yyyy = 0;
            if (xml == null)
                return false;
            if (string.IsNullOrWhiteSpace(contextRef))
                return false;

            //allow for some kind of fall-back value based on the contextRef text itself
            if (RegexCatalog.IsRegexMatch(contextRef, DF_YEAR_PARSE_REGEX, out var regexParsed, 1))
            {
                int.TryParse(regexParsed, out yyyy);
            }

            //use the contextRef back to the node to which its a ref
            var contextPeriodEndNode =
                xml.SelectSingleNode(
                    $"//{XmlNs.ROOTNS}:context[@id='{contextRef}']/{XmlNs.ROOTNS}:period/{XmlNs.ROOTNS}:endDate", nsMgr);
            if (contextPeriodEndNode == null || !contextPeriodEndNode.HasChildNodes ||
                string.IsNullOrWhiteSpace(contextPeriodEndNode.InnerText))
                return yyyy > 0;

            DateTime date;
            if (DateTime.TryParse(contextPeriodEndNode.InnerText, out date))
            {
                yyyy = date.Year;
            }

            return yyyy > 0;
        }
        #endregion
    }
}