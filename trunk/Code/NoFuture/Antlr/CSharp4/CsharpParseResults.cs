using System;
using System.Collections.Generic;

namespace NoFuture.Antlr.CSharp4
{
    public class CsharpParseResults
    {
        public List<string> CatchBlocks { get; } = new List<string>();
        public List<string> MethodNames { get; } = new List<string>();
        public List<string> NamespaceNames { get; } = new List<string>();
        public List<string> ClassNames { get; } = new List<string>();
        public List<CsharpParseItem> ClassMemberBodies { get; } = new List<CsharpParseItem>();
    }

    public class CsharpParseItem
    {
        public List<string> Attributes { get; } = new List<string>();
        public List<string> AccessModifiers { get; } = new List<string>();
        public string Name { get; set; }
        public Tuple<int, int> Start { get; set; }
        public Tuple<int, int> End { get; set; }
        public List<string> Parameters { get; } = new List<string>();
    }
}