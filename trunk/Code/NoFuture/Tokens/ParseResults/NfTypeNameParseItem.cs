using System.Collections.Generic;

namespace NoFuture.Tokens.ParseResults
{
    public class NfTypeNameParseItem
    {
        public string FullName { get; set; }
        public System.Reflection.AssemblyName AssemblyFullName { get; set; }
        public byte? GenericCounter { get; set; }
        public IEnumerable<NfTypeNameParseItem> GenericItems { get; set; }

    }
}