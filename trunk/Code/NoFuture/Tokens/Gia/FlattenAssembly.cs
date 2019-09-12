using System.Collections.Generic;

namespace NoFuture.Tokens.Gia
{
    public class FlattenAssembly
    {
        public List<FlattenedLine> AllLines { get; set; }
        public string AssemblyName { get; set; }
        public string Path { get; set; }
    }
}
