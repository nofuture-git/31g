using System;
using System.Collections.Generic;

namespace NoFuture.Tokens.ParseResults
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
    }
}