using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace NoFuture.Tokens
{
    public static class Etx
    {
        /// <summary>
        /// Helper method to get <see cref="rawHtml"/> as a <see cref="HtmlDocument"/>
        /// </summary>
        /// <param name="rawHtml"></param>
        /// <returns></returns>
        public static HtmlDocument GetHtmlDocument(string rawHtml)
        {
            if (string.IsNullOrWhiteSpace(rawHtml))
                return null;

            var html = new HtmlDocument
            {
                OptionFixNestedTags = true,
                OptionOutputAsXml = true
            };

            html.LoadHtml(rawHtml);
            return html;
        }

        /// <summary>
        /// Strips the content of <see cref="rawHtml"/> down to 
        /// just its html with no scrips, css styles, doc-types, etc.
        /// </summary>
        /// <param name="rawHtml"></param>
        /// <returns></returns>
        public static string GetHtmlOnly(string rawHtml)
        {
            if (string.IsNullOrWhiteSpace(rawHtml))
                return null;

            var antlrRslts = AspNetParseTree.InvokeParse(new MemoryStream(Encoding.UTF8.GetBytes(rawHtml)));
            return antlrRslts?.HtmlOnly;
        }

        /// <summary>
        /// Helper method to take a raw html string, strip it 
        /// down and convert this striped html into xml.
        /// </summary>
        /// <param name="rawHtml"></param>
        /// <returns></returns>
        public static XmlDocument GetHtmlAsXml(string rawHtml)
        {
            if (string.IsNullOrWhiteSpace(rawHtml))
                return null;

            var htmlOnly = GetHtmlOnly(rawHtml);
            if (string.IsNullOrWhiteSpace(htmlOnly))
                return null;
            var xml = new XmlDocument();
            xml.LoadXml(htmlOnly);
            return xml;
        }

        /// <summary>
        /// Helper method, chains together <see cref="GetHtmlAsXml"/>
        /// then converts the xml to json then the json to a dynamic
        /// </summary>
        /// <param name="rawHtml"></param>
        /// <returns></returns>
        public static dynamic GetHtmlAsJson(string rawHtml)
        {
            if (string.IsNullOrWhiteSpace(rawHtml))
                return null;
            var xml = GetHtmlAsXml(rawHtml);
            if (xml == null)
                return null;
            var json = JsonConvert.SerializeXmlNode(xml);

            return JsonConvert.DeserializeObject<dynamic>(json);
        }

        /// <summary>
        /// Gets only the cdata (inner text) for the html page returning it as a list.
        /// </summary>
        /// <param name="webResponseText"></param>
        /// <param name="filter">Optional filter, pass in null to skip</param>
        /// <param name="cdata"></param>
        /// <returns></returns>
        public static bool TryGetCdata(string webResponseText, Func<string, bool> filter, out string[] cdata)
        {
            cdata = null;
            if (string.IsNullOrWhiteSpace(webResponseText))
                return false;

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(webResponseText));
            var antlrHtml = AspNetParseTree.InvokeParse(ms);
            if (antlrHtml == null)
                return false;

            var innerText = antlrHtml.CharData;

            if (innerText.Count <= 0)
                return false;

            cdata = antlrHtml.CharData.ToArray();

            if (filter != null)
            {
                cdata = cdata.Where(filter).ToArray();
            }
            return cdata.Length > 0;
        }
    }
}
