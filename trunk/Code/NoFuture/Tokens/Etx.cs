using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using HtmlAgilityPack;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Newtonsoft.Json;
using NoFuture.Antlr.HTML;

namespace NoFuture.Tokens
{
    /// <summary>
    /// General utility class to concentrate various parsers into one place
    /// </summary>
    public static class Etx
    {
        /// <summary>
        /// Helper method to quick get the text out of a PDF file.
        /// </summary>
        /// <param name="pdfBuffer"></param>
        /// <returns></returns>
        public static string GetPdfText(byte[] pdfBuffer)
        {
            var pdfText = new StringBuilder();
            if (pdfBuffer == null || pdfBuffer.Length <= 0)
                return null;
            using (var reader = new PdfReader(pdfBuffer))
            {
                var numOfPages = reader.NumberOfPages;
                for (var i = 1; i <= numOfPages; i++)
                {
                    var pageText = PdfTextExtractor.GetTextFromPage(reader, i);
                    if (string.IsNullOrWhiteSpace(pageText))
                        continue;
                    pdfText.Append(pageText);
                }
            }

            return pdfText.ToString();
        }

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
        /// Gets only the body of all script tags from the web response content
        /// </summary>
        /// <param name="webResponseText"></param>
        /// <param name="filter"></param>
        /// <param name="scriptBodies"></param>
        /// <returns></returns>
        public static bool TryGetScriptBodies(string webResponseText, Func<string, bool> filter, out string[] scriptBodies)
        {
            scriptBodies = null;
            if (string.IsNullOrWhiteSpace(webResponseText))
                return false;

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(webResponseText));
            var antlrHtml = AspNetParseTree.InvokeParse(ms);
            if (antlrHtml == null)
                return false;

            var innerText = antlrHtml.CharData;

            if (innerText.Count <= 0)
                return false;

            scriptBodies = antlrHtml.ScriptBodies.ToArray();

            if (filter != null)
            {
                scriptBodies = scriptBodies.Where(filter).ToArray();
            }
            return scriptBodies.Length > 0;
        }
    }
}
