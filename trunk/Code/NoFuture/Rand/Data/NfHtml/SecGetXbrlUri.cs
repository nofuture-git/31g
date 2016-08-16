using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Data.NfHtml
{
    public class SecGetXbrlUri : INfDynData
    {
        public SecGetXbrlUri(Uri src)
        {
            SourceUri = src;
        }

        public Uri SourceUri { get; }
        public List<dynamic> ParseContent(object content)
        {
            var webResponseBody = content as string;
            if (webResponseBody == null)
                return null;

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(webResponseBody));
            var htmlRslts = Tokens.AspNetParseTree.InvokeParse(ms);
            if (htmlRslts?.DistinctTags == null || !htmlRslts.DistinctTags.ContainsKey("a"))
                return null;
            var xrblUri = GetXbrlXmlPartialUri(htmlRslts.DistinctTags["a"]);
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
