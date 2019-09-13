using System;
using System.Collections.Generic;

namespace NoFuture.Antlr.CSharp
{
    public class CsharpParseResults
    {
        public List<string> CatchBlocks { get; } = new List<string>();
        public List<string> MethodNames { get; } = new List<string>();
        public List<string> NamespaceNames { get; } = new List<string>();
        public List<string> TypeNames { get; } = new List<string>();
        public List<CsharpParseItem> TypeMemberBodies { get; } = new List<CsharpParseItem>();
        public string SourceFile { get; set; }
    }

    public class CsharpParseItem
    {
        public List<string> Attributes { get; } = new List<string>();
        public List<string> AccessModifiers { get; } = new List<string>();
        public string Name { get; set; }
        public Tuple<int, int> Start { get; set; }
        public Tuple<int, int> End { get; set; }
        public Tuple<int, int> BodyStart { get; set; }
        public List<string> Parameters { get; } = new List<string>();
        public string DeclTypeName { get; set; }
        public string Namespace { get; set; }
        public Tuple<int, int> DeclStart { get; set; }
        public Tuple<int, int> DeclBodyStart { get; set; }
        public Tuple<int, int> DeclEnd { get; set; }
        public Tuple<int, int> NamespaceStart { get; set; }
        public Tuple<int, int> NamespaceBodyStart { get; set; }
        public Tuple<int, int> NamespaceEnd { get; set; }
    }
}