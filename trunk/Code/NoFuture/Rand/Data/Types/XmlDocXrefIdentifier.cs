using System;
using System.Xml;

namespace NoFuture.Rand.Data.Types
{
    /// <summary>
    /// Type coupled to XML's ID attribute
    /// </summary>
    [Serializable]
    public class XmlDocXrefIdentifier : XrefIdentifier
    {
        /// <summary>
        /// Looks for an attribute in <see cref="elem"/> whose name is 
        /// 'ID', 'id' or 'Id'
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        /// <remarks>
        /// The <see cref="XmlElement.LocalName"/> must match 
        /// this instances <see cref="NamedIdentifier.LocalName"/>
        /// </remarks>
        public virtual bool TryThisParseXml(XmlElement elem)
        {
            if (elem == null)
                return false;
            if (elem.LocalName != LocalName)
                return false;

            var attr = (elem.Attributes["ID"] ?? elem.Attributes["id"]) ?? elem.Attributes["Id"];
            if (attr != null)
                Value = attr.Value;
            return true;
        }

        /// <summary>
        /// Returns the same value as <see cref="NamedIdentifier.LocalName"/>
        /// </summary>
        public override string Abbrev
        {
            get { return LocalName; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var xdib = obj as XmlDocXrefIdentifier;
            if (xdib == null)
                return false;

            return string.Equals(LocalName, xdib.LocalName) &&
                   string.Equals(Value, xdib.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            var d = LocalName == null ? 1 : LocalName.GetHashCode();
            var id = Value == null ? 0 : Value.GetHashCode();
            return d + id;
        }
    }
}
