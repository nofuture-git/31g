using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Gov.Sec;
using NoFuture.Shared;

namespace NoFuture.Rand.Data.NfXml
{
    public class SecXbrlInstanceFile : INfDynData
    {
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
        }

        private const string DF_YEAR_PARSE_REGEX = "((19|20)[0-9]{2})";
        private const string YEAR_REGEX_APPDX = "Q[1-4]YTD([^\x5f]|$)";

        private XmlNamespaceManager _nsMgr;

        public SecXbrlInstanceFile(Uri src)
        {
            SourceUri = src;
        }

        public Uri SourceUri { get; }
        public List<dynamic> ParseContent(object content)
        {
            var xmlContent = content as string;
            if (string.IsNullOrWhiteSpace(xmlContent))
                return null;

            var xml = new XmlDocument();
            xml.LoadXml(xmlContent);

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

            _nsMgr = new XmlNamespaceManager(xml.NameTable);
            _nsMgr.AddNamespace(XmlNs.DEI,
                xmlNsDict.ContainsKey(XmlNs.DEI) ? xmlNsDict[XmlNs.DEI] : "http://xbrl.sec.gov/dei/2014-01-31");
            _nsMgr.AddNamespace(XmlNs.US_GAAP,
                xmlNsDict.ContainsKey(XmlNs.US_GAAP) ? xmlNsDict[XmlNs.US_GAAP] : "http://fasb.org/us-gaap/2015-01-31");

            var amendmentFlag = false;
            bool.TryParse(xml.SelectSingleNode($"//{XmlNs.DEI}:AmendmentFlag", _nsMgr)?.InnerText ?? bool.FalseString,
                out amendmentFlag);

            var endOfYear = 0;
            var endOfYearStr = xml.SelectSingleNode($"//{XmlNs.DEI}:CurrentFiscalYearEndDate", _nsMgr)?.InnerText ??
                               string.Empty;
            Edgar.TryGetDayOfYearFiscalEnd(endOfYearStr,out endOfYear);

            var cik = xml.SelectSingleNode($"//{XmlNs.DEI}:EntityCentralIndexKey", _nsMgr)?.InnerText;
            var numOfShares = 0;
            int.TryParse(
                xml.SelectSingleNode($"//{XmlNs.DEI}:EntityCommonStockSharesOutstanding", _nsMgr)?.InnerText ?? String.Empty,
                out numOfShares);
            var name = xml.SelectSingleNode($"//{XmlNs.DEI}:EntityRegistrantName", _nsMgr)?.InnerText;
            var ticker = xml.SelectSingleNode($"//{XmlNs.DEI}:TradingSymbol", _nsMgr)?.InnerText;

            var assets = GetNodeDollarYear(xml, $"//{XmlNs.US_GAAP}:Assets", _nsMgr);
            var liabilities = GetNodeDollarYear(xml, $"//{XmlNs.US_GAAP}:Liabilities", _nsMgr);

            var netIncome = GetNodeDollarYear(xml, $"//{XmlNs.US_GAAP}:NetIncomeLoss[contains(@contextRef,'YTD')]",
                _nsMgr, DF_YEAR_PARSE_REGEX + YEAR_REGEX_APPDX);

            List<Tuple<int, decimal>> operateIncome = new List<Tuple<int, decimal>>();
            foreach (var anotherName in new[] {"OperatingIncomeLoss", "BenefitsLossesAndExpenses"})
            {
                operateIncome = GetNodeDollarYear(xml,
                    $"//{XmlNs.US_GAAP}:{anotherName}[contains(@contextRef,'YTD')]", _nsMgr,
                    DF_YEAR_PARSE_REGEX + YEAR_REGEX_APPDX);
                if (operateIncome.Any())
                    break;
            }

            List<Tuple<int, decimal>> salesRev = new List<Tuple<int, decimal>>();
            foreach (var anotherName in new[] {"SalesRevenueServicesNet", "Revenues" })
            {
                salesRev = GetNodeDollarYear(xml,
                    $"//{XmlNs.US_GAAP}:{anotherName}[contains(@contextRef,'YTD')]", _nsMgr,
                    DF_YEAR_PARSE_REGEX + YEAR_REGEX_APPDX);
                if (salesRev.Any())
                    break;
            }

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
                    Revenue = salesRev
                }
            };
        }

        internal static List<Tuple<int, decimal>> GetNodeDollarYear(XmlDocument xml, string xpath,
            XmlNamespaceManager nsMgr, string contextRefValMatch = DF_YEAR_PARSE_REGEX)
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
            for (var i = 0; i < nodes.Count; i++)
            {
                var currContextRefLen = nodes[i]?.Attributes?["contextRef"]?.Value?.Length ?? 0;
                if (currContextRefLen != contextRefLen)
                    continue;
                Tuple<int, decimal> data = null;
                if (TryParseDollarYear(nodes[i], out data, contextRefValMatch))
                {
                    if (year2usd.Any(x => x.Item1 == data.Item1))
                        continue;
                    year2usd.Add(data);
                }
            }

            return year2usd;
        }

        internal static bool TryParseDollarYear(XmlNode node, out Tuple<int, decimal> val,
            string contextRefValMatch = DF_YEAR_PARSE_REGEX)
        {
            val = null;
            var elem = node as XmlElement;
            if (elem == null)
                return false;

            var moneyValue = 0M;
            if (!decimal.TryParse(elem.InnerText, out moneyValue))
                return false;

            var decPls = 0;
            int.TryParse(elem.Attributes["decimals"]?.Value ?? string.Empty, out decPls);
            for (var i = 0; i < Math.Abs(decPls); i++)
            {
                moneyValue *= 0.1M;
            }

            var contextRef = elem.Attributes["contextRef"]?.Value ?? string.Empty;

            if (!RegexCatalog.IsRegexMatch(contextRef, contextRefValMatch, out contextRef, 1))
                return false;
            var year = 0;
            if (!int.TryParse(contextRef, out year))
                return false;
            val = new Tuple<int, decimal>(year, moneyValue);
            return true;
        }
    }
}
