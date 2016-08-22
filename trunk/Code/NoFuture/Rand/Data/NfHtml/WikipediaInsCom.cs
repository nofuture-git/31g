using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace NoFuture.Rand.Data.NfHtml
{
    public class WikipediaInsCom : NfHtmlDynDataBase
    {
        private static readonly string[] _skipThese = new[]
        {
            "Privacy policy", "About Wikipedia", "Disclaimers", "Contact Wikipedia", "Developers", "Cookie statement",
            "Mobile view"
        };

        public WikipediaInsCom(Uri src):base(src) { }

        public override List<dynamic> ParseContent(object content)
        {
            var webResponseBody = GetWebResponseBody(content);
            if (webResponseBody == null)
                return null;

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(webResponseBody));
            var htmlRslts = Tokens.AspNetParseTree.InvokeParse(ms);

            if (string.IsNullOrWhiteSpace(htmlRslts?.HtmlOnly))
                return null;
            var xml = new XmlDocument();
            xml.LoadXml(htmlRslts.HtmlOnly);

            var insComNameNodes = xml.SelectNodes("//div[contains(text(),'Main article')]/following-sibling::ul/li/a");
            if (insComNameNodes == null || insComNameNodes.Count <= 0)
                return null;

            var insComNames = new List<string>();
            foreach (var node in insComNameNodes)
            {
                var elem = node as XmlElement;

                var name = elem?.InnerText;
                if (string.IsNullOrWhiteSpace(name) || insComNames.Contains(name) 
                    || _skipThese.Contains(name))
                    continue;

                insComNames.Add(name);
            }
            insComNames.Sort();
            return new List<dynamic> {new {UsInsComNames = insComNames.ToArray()}};
        }
    }
}
