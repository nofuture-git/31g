using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using NoFuture.Shared;
using NoFuture.Antlr.Grammers;
using NoFuture.Util;

namespace NoFuture.Tokens
{
    public class HtmlParseResults
    {
        public String HtmlOnly { get; set; }
        public List<String> ScriptBodies { get; } = new List<string>();
        public List<String> ScriptLets { get; } = new List<string>();
        public List<string> EmptyAttrs { get; } = new List<string>();
        public List<string> StyleBodies { get; } = new List<string>();
        public List<string> DtdNodes { get; } = new List<string>();
        public Dictionary<string, List<string>> DistinctTags { get; } = new Dictionary<string, List<string>>();
        public List<String> HtmlComments { get; } = new List<string>();
        public List<string> CharData { get; } = new List<string>();
    }
    public class AspNetParseTree : HTMLParserBaseListener
    {
        protected ParseTreeProperty<String> MyTreeProperty = new ParseTreeProperty<string>();
        private readonly HtmlParseResults _results = new HtmlParseResults();

        public HtmlParseResults Results => _results;

        public override void ExitScriptlet(HTMLParser.ScriptletContext ctx)
        {
            var scriptLetNode = ctx.SCRIPTLET();
            var scriptletText = scriptLetNode?.GetText();
            if (string.IsNullOrWhiteSpace(scriptletText))
                return;
            _results.ScriptLets.Add(scriptletText);
        }

        public override void ExitHtmlComment(HTMLParser.HtmlCommentContext context)
        {
            var htmlCommentNode = context.HTML_COMMENT();
            var htmlCommentText = htmlCommentNode?.GetText();
            if (string.IsNullOrWhiteSpace(htmlCommentText))
                return;
            _results.HtmlComments.Add(htmlCommentText);
        }

        public override void ExitScript(HTMLParser.ScriptContext ctx)
        {
            const string SHORT_BODY = "</>";
            const string BODY = "</script>";
            var scriptBodyNode = ctx.SCRIPT_BODY() ?? ctx.SCRIPT_SHORT_BODY();
            var scriptBodyText = scriptBodyNode?.GetText();
            if (string.IsNullOrWhiteSpace(scriptBodyText))
                return;
            if (scriptBodyText.EndsWith(SHORT_BODY))
                scriptBodyText = scriptBodyText.Substring(0, scriptBodyText.Length - (SHORT_BODY.Length));
            if (scriptBodyText.EndsWith(BODY))
                scriptBodyText = scriptBodyText.Substring(0, scriptBodyText.Length - (BODY.Length));

            scriptBodyText = scriptBodyText.Trim();
            _results.ScriptBodies.Add(scriptBodyText);
        }

        public override void ExitStyle(HTMLParser.StyleContext ctx)
        {
            const string SHORT_BODY = "</>";
            const string BODY = "</style>";
            var styleBody = ctx.STYLE_BODY() ?? ctx.STYLE_SHORT_BODY();
            var styleBodyText = styleBody?.GetText();
            if (string.IsNullOrWhiteSpace(styleBodyText))
                return;
            if (styleBodyText.EndsWith(SHORT_BODY))
                styleBodyText = styleBodyText.Substring(0, styleBodyText.Length - SHORT_BODY.Length);
            if (styleBodyText.EndsWith(BODY))
                styleBodyText = styleBodyText.Substring(0, styleBodyText.Length - BODY.Length);

            styleBodyText = styleBodyText.Trim();
            _results.StyleBodies.Add(styleBodyText);
        }

        public override void ExitDtd(HTMLParser.DtdContext context)
        {
            var dtdNode = context.DTD();
            var dtdNodeText = dtdNode?.GetText();
            if (string.IsNullOrWhiteSpace(dtdNodeText))
                return;
            _results.DtdNodes.Add(dtdNodeText);
        }

        public override void ExitHtmlAttributeValue(HTMLParser.HtmlAttributeValueContext ctx)
        {
            var attrValue = ctx.ATTVALUE_VALUE();
            if (attrValue == null)
            {
                return;
            }

            var attrValueText = attrValue.GetText();
            FilteredPut(ctx, attrValueText);
        }

        public override void ExitHtmlAttributeName(HTMLParser.HtmlAttributeNameContext ctx)
        {
            var attrName = ctx.TAG_NAME();
            if (attrName == null)
            {
                return;
            }

            var attrNameText = attrName.GetText();
            FilteredPut(ctx, attrNameText);
        }

        public override void ExitAssignedAttr(HTMLParser.AssignedAttrContext ctx)
        {
            var attrNameCtx = ctx.htmlAttributeName();
            if (attrNameCtx == null)
            {
                return;
            }
            var attrValueCtx = ctx.htmlAttributeValue();
            if (attrValueCtx == null)
            {
                return;
            }

            var attrNameText = MyTreeProperty.Get(attrNameCtx);
            if (string.IsNullOrEmpty(attrNameText))
            {
                return;
            }
            var attrValueText = MyTreeProperty.Get(attrValueCtx);
            if (string.IsNullOrEmpty(attrValueText))
            {
                return;
            }

            //TODO swap attributes name & values here
            if (attrValueText.Length <= 2)
                FilteredPut(ctx, attrNameText + "=" + attrValueText);
            else
            {
                var openQuot = attrValueText.Substring(0, 1);
                var closeQuot = attrValueText.Substring(attrValueText.Length - 1, 1);
                var attrInnerValue =
                    attrValueText.Substring(1, attrValueText.Length - 2).EscapeString(EscapeStringType.XML);
                FilteredPut(ctx, attrNameText + "=" + openQuot + attrInnerValue + closeQuot);
            }
        }

        public override void ExitEmptyAttr(HTMLParser.EmptyAttrContext ctx)
        {
            var attrNameCtx = ctx.htmlAttributeName();
            if (attrNameCtx == null)
            {
                return;
            }
            var attrNameText = MyTreeProperty.Get(attrNameCtx);
            if (string.IsNullOrEmpty(attrNameText))
            {
                return;
            }

            //TODO swap attributes name with name-value pair here
            if(_results.EmptyAttrs.All(x => attrNameText != x))
                _results.EmptyAttrs.Add(attrNameText);

            FilteredPut(ctx, attrNameText + "='true'");
        }

        public override void ExitHtmlContent(HTMLParser.HtmlContentContext ctx)
        {
			var textContent = new StringBuilder();
			
			foreach(var elemCtx in ctx.htmlElement()){
				var elemText = MyTreeProperty.Get(elemCtx);
				if(string.IsNullOrEmpty(elemText)){
					continue;
				}
				textContent.Append(elemText);
			}

            FilteredPut(ctx, textContent.ToString());
        }

        public override void ExitPairElement(HTMLParser.PairElementContext ctx)
        {
			var textContent = new StringBuilder();
			textContent.Append("<");
			
			var tagNameCtx = ctx.htmlTagName(0);
            var tagNameNode = tagNameCtx?.TAG_NAME();

            var tagNameText = tagNameNode?.GetText();
			
			if(string.IsNullOrEmpty(tagNameText)){
				return;
			}

            //add this tag name to the list if its not already present
            if (!_results.DistinctTags.ContainsKey(tagNameText))
            {
                _results.DistinctTags.Add(tagNameText, new List<string>());
            }

            var tagContents = new List<string> {tagNameText};

            foreach(var attrCtx in ctx.htmlAttribute()){
				var attrText = MyTreeProperty.Get(attrCtx);
				if(string.IsNullOrEmpty(attrText)){
					continue;
				}
                //add this tag's attributes if its not already present as-is
                if (_results.DistinctTags[tagNameText].All(a => a != attrText))
                {
                    _results.DistinctTags[tagNameText].Add(attrText);
                }
                tagContents.Add(attrText);
			}

            textContent.Append(string.Join(" ", tagContents));

			textContent.Append(">");
			
			var contentCtx = ctx.htmlContent();
			if(contentCtx != null){
				var contentsText = MyTreeProperty.Get(contentCtx);
				if(!string.IsNullOrEmpty(contentsText)){
					textContent.Append(contentsText);
				}
			}
			
			textContent.Append("</");
			textContent.Append(tagNameText);
			textContent.Append(">");
			textContent.Append("\n");

            FilteredPut(ctx, textContent.ToString());
        }

        public override void ExitEmptyElement(HTMLParser.EmptyElementContext ctx)
        {
			var textContent = new StringBuilder();
			textContent.Append("<");
			
			var tagNameCtx = ctx.htmlTagName();
			if(tagNameCtx == null){
				return;
			}
			var tagNameNode = tagNameCtx.TAG_NAME();
			if(tagNameNode == null){
				return;
			}
			
			var tagNameText = tagNameNode.GetText();

			if(string.IsNullOrEmpty(tagNameText)){
				return;
			}

            //add this tag name to the list if its not already present
            if (!_results.DistinctTags.ContainsKey(tagNameText))
            {
                _results.DistinctTags.Add(tagNameText, new List<string>());
            }

            var tagContents = new List<string> {tagNameText};

            foreach(var attrCtx in ctx.htmlAttribute()){
				var attrText = MyTreeProperty.Get(attrCtx);
				if(string.IsNullOrEmpty(attrText)){
					continue;
				}
                //add this tag's attributes if its not already present as-is
			    if (_results.DistinctTags[tagNameText].All(a => a != attrText))
			    {
			        _results.DistinctTags[tagNameText].Add(attrText);
			    }
                tagContents.Add(attrText);
			}
            textContent.Append(string.Join(" ", tagContents));
			textContent.Append("/>");
			textContent.Append("\n");

            FilteredPut(ctx, textContent.ToString());
        }

        public override void ExitHtmlChardata(HTMLParser.HtmlChardataContext context)
        {
            if(!string.IsNullOrWhiteSpace(context.GetText()))
                _results.CharData.Add(context.GetText().Trim());
        }

        public override void ExitHtmlDocument(HTMLParser.HtmlDocumentContext ctx)
        {
			var textContent = new StringBuilder();
			
			foreach(var elemsCtx in ctx.htmlElements()){
				var markup = MyTreeProperty.Get(elemsCtx);
				if(string.IsNullOrEmpty(markup)){
					continue;
				}
				
				textContent.Append(markup);
			}
            _results.HtmlOnly = textContent.ToString();
        }

        public override void ExitHtmlElements(HTMLParser.HtmlElementsContext ctx)
        {
            var topNodeCtx = ctx.htmlElement();
            var expectedFullMarkup = MyTreeProperty.Get(topNodeCtx);
            FilteredPut(ctx, expectedFullMarkup);
        }

        protected internal void FilteredPut(ParserRuleContext ctx, string val)
        {
            if(MyTreeProperty.Get(ctx) == null)
                MyTreeProperty.Put(ctx, val);
        }

        public static HtmlParseResults InvokeParse(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;
            if (!File.Exists(fileName))
                return null;
            var tr = File.OpenRead(fileName);
            var results = InvokeParse(tr);
            tr.Close();
            return results;
        }

        public static HtmlParseResults InvokeParse(Stream stream)
        {
            var input = new AntlrInputStream(stream);
            var lexer = new HTMLLexer(input);
            var tokens = new CommonTokenStream(lexer);
            var parser = new HTMLParser(tokens);

            var tree = parser.htmlDocument();

            var walker = new ParseTreeWalker();
            var loader = new AspNetParseTree();

            walker.Walk(loader, tree);

            return loader.Results;
        }

        public static bool TryGetCdata(string webResponseText, Func<string, bool> filter, out string[] cdata)
        {
            cdata = null;
            if (string.IsNullOrWhiteSpace(webResponseText))
                return false;
            var tempFile = Path.Combine(TempDirectories.AppData, NfTypeName.GetNfRandomName());
            File.WriteAllText(tempFile, webResponseText);
            System.Threading.Thread.Sleep(NfConfig.ThreadSleepTime);

            var antlrHtml = InvokeParse(tempFile);

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
