using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NoFuture.Antlr.HTML;
using NoFuture.Rand.Exo.NfXml;
using NoFuture.Rand.Gov.US.Sec;
using NoFuture.Tokens;
using NoFuture.Tokens.ParseResults;

namespace NoFuture.Rand.Exo.NfHtml
{
    /// <summary>
    /// The uri to an individual XBRL xml file is embedded inside the
    /// html from <see cref="SecForm.HtmlFormLink"/>
    /// </summary>
    public class SecGetXbrlUri : NfHtmlDynDataBase
    {
        public SecGetXbrlUri(Uri src):base(src) { }

        public static Uri GetUri(SecForm secForm)
        {
            return secForm?.HtmlFormLink;
        }

        public override IEnumerable<dynamic> ParseContent(object content)
        {
            var webResponseBody = GetWebResponseBody(content);
            if (webResponseBody == null)
                return null;

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(webResponseBody));
            var htmlRslts = AspNetParseTree.InvokeParse(ms);
            if (htmlRslts?.Tags2Attrs == null || !htmlRslts.Tags2Attrs.ContainsKey("a"))
                return null;
            var xrblUri = GetXbrlXmlPartialUri(htmlRslts.Tags2Attrs["a"]);
            if (string.IsNullOrWhiteSpace(xrblUri))
                return null;

            string irsId;
            TryGetGetIrsId(htmlRslts, out irsId);

            return new List<dynamic>
            {
                new
                {
                    XrblUri = Edgar.SEC_ROOT_URL + xrblUri,
                    IrsId = irsId
                }
            };
        }

        internal static bool TryGetGetIrsId(HtmlParseResults html, out string irsId)
        {
            irsId = null;
            try
            {
                if (html?.CharData == null || html.CharData.Count <= 0)
                    return false;
                var idxStart =
                    html.CharData.FindIndex(x => string.Equals(x.Trim(), "IRS No.", StringComparison.OrdinalIgnoreCase));
                if (idxStart < 0)
                    return false;
                irsId =
                    html.CharData.Skip(idxStart)
                        .FirstOrDefault(x => Regex.IsMatch(x, "[0-9]+"));

                return irsId != null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return false;
            }
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
