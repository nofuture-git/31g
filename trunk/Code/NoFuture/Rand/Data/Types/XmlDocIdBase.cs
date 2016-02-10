using System;
using System.Xml;

namespace NoFuture.Rand.Data.Types
{
    [Serializable]
    public class XmlDocIdBase : Identifier
    {
        protected internal string _xmlLocalName = string.Empty;
        public virtual string XmlLocalName { get { return _xmlLocalName; } }

        public virtual bool TryThisParseXml(XmlElement elem)
        {
            if (elem == null)
                return false;
            if (elem.LocalName != XmlLocalName)
                return false;

            var attr = elem.Attributes["ID"] ?? elem.Attributes["id"];
            if (attr != null)
                Value = attr.Value;
            return true;
        }

        public override string Abbrev
        {
            get { return string.Empty; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var xdib = obj as XmlDocIdBase;
            if (xdib == null)
                return false;

            return string.Equals(XmlLocalName, xdib.XmlLocalName) &&
                   string.Equals(Value, xdib.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            var d = XmlLocalName == null ? 1 : XmlLocalName.GetHashCode();
            var id = Value == null ? 0 : Value.GetHashCode();
            return d + id;
        }
    }
}
