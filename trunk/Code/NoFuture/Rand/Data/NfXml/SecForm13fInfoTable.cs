using System;
using System.Collections.Generic;
using System.Xml;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Data.NfXml
{
    public class SecForm13FInfoTable : NfDynDataBase
    {
        public SecForm13FInfoTable(Uri src) : base(src) { }

        public override IEnumerable<dynamic> ParseContent(object content)
        {
            var xmlContent = content as string;
            if (xmlContent == null)
                return new List<dynamic>();
            const string NS = "ns";
            const string XMLNS = "http://www.sec.gov/edgar/document/thirteenf/informationtable";

            var infoTable = new XmlDocument();
            infoTable.LoadXml(xmlContent);

            if (!infoTable.HasChildNodes)
                return new List<dynamic>();

            var nsMgr = new XmlNamespaceManager(infoTable.NameTable);
            nsMgr.AddNamespace(NS, XMLNS);

            var cusipIds = new List<dynamic>();

            var allCusips = infoTable.SelectNodes($"//{NS}:cusip",nsMgr);
            if (allCusips == null || allCusips.Count <= 0)
                return new List<dynamic>();
            foreach (var cusipNode in allCusips)
            {
                var cusipElem = cusipNode as XmlElement;
                var cusipId = cusipElem?.InnerText;
                if(string.IsNullOrWhiteSpace(cusipId))
                    continue;
                var usdValNode = cusipElem.NextSibling;
                var usdVal = usdValNode?.InnerText;
                if (string.IsNullOrWhiteSpace(usdVal))
                    continue;
                long usd;
                if (!long.TryParse(usdVal, out usd))
                    continue;
                var shrsOrPrnNode = usdValNode.NextSibling;
                if (shrsOrPrnNode == null || !shrsOrPrnNode.HasChildNodes)
                    continue;
                var valueNode = shrsOrPrnNode.FirstChild;
                if (string.IsNullOrWhiteSpace(valueNode?.InnerText))
                    continue;
                long totalNumber;
                if (!long.TryParse(valueNode.InnerText.Trim(), out totalNumber))
                    continue;

                var secAbbrevNode = valueNode.NextSibling;
                var secAbbrev = secAbbrevNode?.InnerText.Trim() ?? "SH";
                cusipIds.Add(GetSingleItemEntry(cusipId, usd, totalNumber, secAbbrev));
            }

            return cusipIds;
        }

        internal static dynamic GetSingleItemEntry(string cusipId, long usd, long totalNumber, string securityType)
        {
            return
                new {CusipId = cusipId, MarketValue = usd, TotalNumberOf = totalNumber, SecurityAbbrev = securityType};
        }
    }
}
