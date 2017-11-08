using System;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// Base type to couple a local name with an identity
    /// </summary>
    [Serializable]
    public abstract class NamedIdentifier : Identifier
    {
        protected NamedIdentifier() { }
        protected NamedIdentifier(string localName)
        {
            _localName = localName;
        }
        protected internal string _localName = string.Empty;
        public virtual string LocalName => _localName;
    }
}