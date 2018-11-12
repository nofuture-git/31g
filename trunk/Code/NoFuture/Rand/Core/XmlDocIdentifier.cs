using System;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// Base type to couple a local name with an identity
    /// </summary>
    [Serializable]
    public abstract class XmlDocIdentifier : Identifier
    {
        protected XmlDocIdentifier() { }
        protected XmlDocIdentifier(string localName)
        {
            _localName = localName;
        }
        protected internal string _localName = string.Empty;
        public virtual string LocalName => _localName;
    }
}