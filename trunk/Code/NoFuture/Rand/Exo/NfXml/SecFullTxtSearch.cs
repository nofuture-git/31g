using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NoFuture.Rand.Exo.NfXml
{
    public class SecFullTxtSearch : NfDynDataBase
    {
        public SecFullTxtSearch(Uri src):base(src) { }

        public static Uri GetUri(Edgar.FullTextSearch fts)
        {
            if (string.IsNullOrWhiteSpace(fts.ToString()))
                return null;
            var urlFullText = new StringBuilder();

            urlFullText.Append(Edgar.SEC_ROOT_URL);
            urlFullText.AppendFormat("/cgi-bin/srch-edgar?text={0}", fts);
            urlFullText.Append($"&start={fts.PagingStartAt}&count={fts.PagingCount}");
            urlFullText.Append($"&first={fts.YearStart}&last={fts.YearEnd}&output=atom");
            return new Uri(urlFullText.ToString());
        }

        public override IEnumerable<dynamic> ParseContent(object content)
        {
            const string ATOM = "atom";
            var rssContent = content as string;

            if (string.IsNullOrWhiteSpace(rssContent))
                return null;

            for (var i = 0; i < 9; i++)
                rssContent = rssContent.Replace($"<{i} ", "<val ");

            var rssXml = new XmlDocument();
            rssXml.LoadXml(rssContent);

            if (!rssXml.HasChildNodes)
                return null;

            var nsMgr = new XmlNamespaceManager(rssXml.NameTable);
            nsMgr.AddNamespace(ATOM, Edgar.ATOM_XML_NS);

            var entries = rssXml.SelectNodes($"//{ATOM}:entry", nsMgr);

            if (entries == null || entries.Count <= 0)
                return null;
            var dynContent = new List<dynamic>();

            for (var i = 0; i < entries.Count; i++)
            {

                var xpathRoot = $"//{ATOM}:entry[{i+1}]";//xpath idx is one based.

                var entry = entries.Item(i);

                var titleNode = entry?.SelectSingleNode($"{xpathRoot}/{ATOM}:title", nsMgr);

                if (string.IsNullOrWhiteSpace(titleNode?.InnerText))
                    continue;

                var idNode = entry.SelectSingleNode($"{xpathRoot}/{ATOM}:id", nsMgr);
                var linkNode = entry.SelectSingleNode($"{xpathRoot}/{ATOM}:link", nsMgr);
                var form10KDtNode = entry.SelectSingleNode($"{xpathRoot}/{ATOM}:updated", nsMgr);
                var summaryNode = entry.SelectSingleNode($"{xpathRoot}/{ATOM}:summary", nsMgr);

                var linkHref = string.Empty;
                if (linkNode?.Attributes?["href"] != null)
                {
                    linkHref = linkNode.Attributes["href"].Value;
                }

                dynContent.Add( new
                {
                    Title = titleNode.InnerText,
                    Id = idNode?.InnerText ?? string.Empty,
                    Link = linkHref,
                    Update = form10KDtNode?.InnerText ?? string.Empty,
                    Summary = summaryNode?.InnerText ?? string.Empty
                });
            }
            
            return dynContent;
        }
    }
}
