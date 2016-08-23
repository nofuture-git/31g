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
                yield return new List<dynamic>();
            const string NS = "ns";
            const string XMLNS = "http://www.sec.gov/edgar/document/thirteenf/informationtable";

            var infoTable = new XmlDocument();
            infoTable.LoadXml(xmlContent);

            if (!infoTable.HasChildNodes)
                yield return new List<dynamic>();

            var nsMgr = new XmlNamespaceManager(infoTable.NameTable);
            nsMgr.AddNamespace(NS, XMLNS);

            var cusipIds = new Dictionary<string, long>();

            var allCusips = infoTable.SelectNodes($"//{NS}:cusip",nsMgr);
            if (allCusips == null || allCusips.Count <= 0)
                yield return new List<dynamic>();
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
                if (cusipIds.ContainsKey(cusipId))
                    cusipIds[cusipId] += usd;
                else
                    cusipIds.Add(cusipId, usd);
            }

            yield return cusipIds;
        }
    }
}
