using System;
using System.Collections.Generic;
using System.Xml;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Gov.Sec;

namespace NoFuture.Rand.Data.NfXml
{
    public class SecFullTxtSearch : INfDynData
    {
        public Uri SourceUri { get; private set; }

        public SecFullTxtSearch(Uri src)
        {
            SourceUri = src;
        }

        public List<dynamic> ParseContent(object content)
        {
            var rssContent = content as string;

            if (String.IsNullOrWhiteSpace(rssContent))
                return null;

            for (var i = 0; i < 9; i++)
                rssContent = rssContent.Replace(String.Format("<{0} ", i), "<val ");

            var rssXml = new XmlDocument();
            rssXml.LoadXml(rssContent);

            if (!rssXml.HasChildNodes)
                return null;

            var nsMgr = new XmlNamespaceManager(rssXml.NameTable);
            nsMgr.AddNamespace("atom", Edgar.ATOM_XML_NS);

            var entries = rssXml.SelectNodes("//atom:entry", nsMgr);

            if (entries == null || entries.Count <= 0)
                return null;
            var dynContent = new List<dynamic>();

            for (var i = 0; i < entries.Count; i++)
            {

                var xpathRoot = String.Format("//atom:entry[{0}]", i);

                var entry = entries.Item(i);

                if (entry == null)
                    continue;

                var titleNode = entry.SelectSingleNode(xpathRoot + "/atom:title", nsMgr);

                if (titleNode == null || String.IsNullOrWhiteSpace(titleNode.InnerText))
                    continue;

                var idNode = entry.SelectSingleNode(xpathRoot + "/atom:id", nsMgr);
                var linkNode = entry.SelectSingleNode(xpathRoot + "/atom:link", nsMgr);
                var form10KDtNode = entry.SelectSingleNode(xpathRoot + "/atom:updated", nsMgr);
                var summaryNode = entry.SelectSingleNode(xpathRoot + "/atom:summary", nsMgr);

                var linkHref = string.Empty;
                if (linkNode != null && linkNode.Attributes != null && linkNode.Attributes["href"] != null)
                {
                    linkHref = linkNode.Attributes["href"].Value;
                }

                dynContent.Add( new
                {
                    Title = titleNode.InnerText,
                    Id = idNode == null ? string.Empty : idNode.InnerText,
                    Link = linkHref,
                    Update = form10KDtNode == null ? string.Empty : form10KDtNode.InnerText,
                    Summary = summaryNode == null ? string.Empty : summaryNode.InnerText
                });
            }
            
            return dynContent;
        }
    }
}
