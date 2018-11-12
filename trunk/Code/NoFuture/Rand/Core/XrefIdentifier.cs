using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// Base type to interrelate identities
    /// </summary>
    [Serializable]
    public abstract class XrefIdentifier : XmlDocIdentifier
    {
        public const string XREF_XML_DATA_FILE = "XRef.xml";
        protected XrefIdentifier() { }
        protected XrefIdentifier(string localName) : base(localName) { }
        private readonly Dictionary<string, string> _refDict = new Dictionary<string, string>();
        public virtual Dictionary<string, string> ReferenceDictionary => _refDict;
        public virtual XmlDocIdentifier[] XrefIds { get; set; }

    }
}