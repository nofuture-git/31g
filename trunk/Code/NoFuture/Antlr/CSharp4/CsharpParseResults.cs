using System.Collections.Generic;

namespace NoFuture.Tokens.ParseResults
{
    public class CsharpParseResults
    {
        private readonly List<string>  _catchBlocks = new List<string>();
        public List<string> CatchBlocks { get { return _catchBlocks; } }
    }
}