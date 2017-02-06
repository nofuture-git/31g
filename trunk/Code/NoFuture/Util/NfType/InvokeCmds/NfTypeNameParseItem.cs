using System;
using System.Collections.Generic;

namespace NoFuture.Util.NfType.InvokeCmds
{
    [Serializable]
    public class NfTypeNameParseItem
    {
        public string FullName { get; set; }
        public string AssemblyFullName { get; set; }
        public string PublicKeyTokenValue { get; set; }
        public byte? GenericCounter { get; set; }
        public IEnumerable<NfTypeNameParseItem> GenericItems { get; set; }
        public Exception Error { get; set; }
    }
}