using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NoFuture.Rand.Data.NfHtml
{
    /// <summary>
    /// The uri to an individual XBRL xml file is embedded inside the
    /// html from <see cref="Gov.Sec.SecForm.HtmlFormLink"/>
    /// </summary>
    public class SecGetXbrlUri : NfHtmlDynDataBase
    {
        public SecGetXbrlUri(Uri src):base(src) { }

        public override List<dynamic> ParseContent(object content)
        {
            var webResponseBody = GetWebResponseBody(content);
            if (webResponseBody == null)
                return null;

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(webResponseBody));
            var htmlRslts = Tokens.AspNetParseTree.InvokeParse(ms);
            if (htmlRslts?.Tags2Attrs == null || !htmlRslts.Tags2Attrs.ContainsKey("a"))
                return null;
            var xrblUri = GetXbrlXmlPartialUri(htmlRslts.Tags2Attrs["a"]);
            if (string.IsNullOrWhiteSpace(xrblUri))
                return null;

            return new List<dynamic>
            {
                new {XrblUri = Gov.Sec.Edgar.SEC_ROOT_URL + xrblUri}
            };
        }

        internal static string GetXbrlXmlPartialUri(List<string> links)
        {
            if (links == null)
                return null;
            var hrefs = links.Where(x => x.StartsWith("href") && x.Contains("=")).ToArray();
            if (!hrefs.Any())
                return null;
            foreach (var href in hrefs)
            {
                var partialUri = href.Split('=')[1].Replace("\"", "");
                if (!partialUri.EndsWith(".xml"))
                    continue;
                if (Path.GetFileNameWithoutExtension(partialUri).Contains("_"))
                    continue;
                return partialUri;
            }
            return null;
        }
    }
}
