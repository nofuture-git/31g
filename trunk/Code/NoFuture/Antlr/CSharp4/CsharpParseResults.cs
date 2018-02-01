using System.Collections.Generic;

namespace NoFuture.Antlr.CSharp4
{
    public class CsharpParseResults
    {
        private readonly List<string>  _catchBlocks = new List<string>();
        public List<string> CatchBlocks { get { return _catchBlocks; } }
    }
}