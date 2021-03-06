//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.6.4
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:\Projects\31g\trunk\Code\NoFuture\Antlr\Grammers\HTMLParser.g4 by ANTLR 4.6.4

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace NoFuture.Antlr.HTML
{

using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IHTMLParserListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.6.4")]
[System.CLSCompliant(false)]
public partial class HTMLParserBaseListener : IHTMLParserListener {
	/// <summary>
	/// Enter a parse tree produced by the <c>pairElement</c>
	/// labeled alternative in <see cref="HTMLParser.htmlElement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPairElement([NotNull] HTMLParser.PairElementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>pairElement</c>
	/// labeled alternative in <see cref="HTMLParser.htmlElement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPairElement([NotNull] HTMLParser.PairElementContext context) { }

	/// <summary>
	/// Enter a parse tree produced by the <c>nonElement</c>
	/// labeled alternative in <see cref="HTMLParser.htmlElement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNonElement([NotNull] HTMLParser.NonElementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>nonElement</c>
	/// labeled alternative in <see cref="HTMLParser.htmlElement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNonElement([NotNull] HTMLParser.NonElementContext context) { }

	/// <summary>
	/// Enter a parse tree produced by the <c>emptyElement</c>
	/// labeled alternative in <see cref="HTMLParser.htmlElement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterEmptyElement([NotNull] HTMLParser.EmptyElementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>emptyElement</c>
	/// labeled alternative in <see cref="HTMLParser.htmlElement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitEmptyElement([NotNull] HTMLParser.EmptyElementContext context) { }

	/// <summary>
	/// Enter a parse tree produced by the <c>assignedAttr</c>
	/// labeled alternative in <see cref="HTMLParser.htmlAttribute"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAssignedAttr([NotNull] HTMLParser.AssignedAttrContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>assignedAttr</c>
	/// labeled alternative in <see cref="HTMLParser.htmlAttribute"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAssignedAttr([NotNull] HTMLParser.AssignedAttrContext context) { }

	/// <summary>
	/// Enter a parse tree produced by the <c>emptyAttr</c>
	/// labeled alternative in <see cref="HTMLParser.htmlAttribute"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterEmptyAttr([NotNull] HTMLParser.EmptyAttrContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>emptyAttr</c>
	/// labeled alternative in <see cref="HTMLParser.htmlAttribute"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitEmptyAttr([NotNull] HTMLParser.EmptyAttrContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="HTMLParser.htmlDocument"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterHtmlDocument([NotNull] HTMLParser.HtmlDocumentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HTMLParser.htmlDocument"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitHtmlDocument([NotNull] HTMLParser.HtmlDocumentContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="HTMLParser.htmlElements"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterHtmlElements([NotNull] HTMLParser.HtmlElementsContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HTMLParser.htmlElements"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitHtmlElements([NotNull] HTMLParser.HtmlElementsContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="HTMLParser.htmlElement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterHtmlElement([NotNull] HTMLParser.HtmlElementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HTMLParser.htmlElement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitHtmlElement([NotNull] HTMLParser.HtmlElementContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="HTMLParser.htmlContent"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterHtmlContent([NotNull] HTMLParser.HtmlContentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HTMLParser.htmlContent"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitHtmlContent([NotNull] HTMLParser.HtmlContentContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="HTMLParser.htmlAttribute"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterHtmlAttribute([NotNull] HTMLParser.HtmlAttributeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HTMLParser.htmlAttribute"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitHtmlAttribute([NotNull] HTMLParser.HtmlAttributeContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="HTMLParser.htmlAttributeName"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterHtmlAttributeName([NotNull] HTMLParser.HtmlAttributeNameContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HTMLParser.htmlAttributeName"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitHtmlAttributeName([NotNull] HTMLParser.HtmlAttributeNameContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="HTMLParser.htmlAttributeValue"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterHtmlAttributeValue([NotNull] HTMLParser.HtmlAttributeValueContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HTMLParser.htmlAttributeValue"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitHtmlAttributeValue([NotNull] HTMLParser.HtmlAttributeValueContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="HTMLParser.htmlTagName"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterHtmlTagName([NotNull] HTMLParser.HtmlTagNameContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HTMLParser.htmlTagName"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitHtmlTagName([NotNull] HTMLParser.HtmlTagNameContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="HTMLParser.htmlChardata"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterHtmlChardata([NotNull] HTMLParser.HtmlChardataContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HTMLParser.htmlChardata"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitHtmlChardata([NotNull] HTMLParser.HtmlChardataContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="HTMLParser.htmlMisc"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterHtmlMisc([NotNull] HTMLParser.HtmlMiscContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HTMLParser.htmlMisc"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitHtmlMisc([NotNull] HTMLParser.HtmlMiscContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="HTMLParser.htmlComment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterHtmlComment([NotNull] HTMLParser.HtmlCommentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HTMLParser.htmlComment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitHtmlComment([NotNull] HTMLParser.HtmlCommentContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="HTMLParser.xhtmlCDATA"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterXhtmlCDATA([NotNull] HTMLParser.XhtmlCDATAContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HTMLParser.xhtmlCDATA"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitXhtmlCDATA([NotNull] HTMLParser.XhtmlCDATAContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="HTMLParser.dtd"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDtd([NotNull] HTMLParser.DtdContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HTMLParser.dtd"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDtd([NotNull] HTMLParser.DtdContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="HTMLParser.xml"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterXml([NotNull] HTMLParser.XmlContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HTMLParser.xml"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitXml([NotNull] HTMLParser.XmlContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="HTMLParser.scriptlet"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterScriptlet([NotNull] HTMLParser.ScriptletContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HTMLParser.scriptlet"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitScriptlet([NotNull] HTMLParser.ScriptletContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="HTMLParser.script"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterScript([NotNull] HTMLParser.ScriptContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HTMLParser.script"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitScript([NotNull] HTMLParser.ScriptContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="HTMLParser.style"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStyle([NotNull] HTMLParser.StyleContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="HTMLParser.style"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStyle([NotNull] HTMLParser.StyleContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
} // namespace NoFuture.Antlr.Grammers
