using System.Collections.Generic;

namespace NoFuture.Antlr.CSharp4
{
    public class CsharpParseResults
    {
        public List<string> CatchBlocks { get; } = new List<string>();
        public List<string> MethodNames { get; } = new List<string>();
        public List<string> NamespaceNames { get; } = new List<string>();
        public List<string> ClassNames { get; } = new List<string>();
    }
}