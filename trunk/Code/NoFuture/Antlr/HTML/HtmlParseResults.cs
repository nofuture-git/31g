using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NoFuture.Antlr.HTML
{
    public class HtmlParseResults
    {
        public String HtmlOnly { get; set; }
        public List<String> ScriptBodies { get; } = new List<string>();
        public List<String> ScriptLets { get; } = new List<string>();
        public List<string> EmptyAttrs { get; } = new List<string>();
        public List<string> StyleBodies { get; } = new List<string>();
        public List<string> DtdNodes { get; } = new List<string>();
        public Dictionary<string, List<string>> Tags2Attrs { get; } = new Dictionary<string, List<string>>();
        public List<String> HtmlComments { get; } = new List<string>();
        public List<string> CharData { get; } = new List<string>();

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