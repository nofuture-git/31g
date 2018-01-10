using System.Collections.Generic;
using System.Xml;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Org
{
    /// <summary>
    /// An intermediate base type to keep from having alot of redundant code for the TryThisParseXml
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ClassificationBase<T> : XmlDocXrefIdentifier where T:XmlDocXrefIdentifier, new()
    {
        protected readonly List<T> divisions = new List<T>();

        public string Description { get; set; }

        public List<T> Divisions => divisions;

        public override bool TryThisParseXml(XmlElement elem)
        {
            if (!base.TryThisParseXml(elem))
                return false;

            const string DESCRIPTION = "Description";
            if (!base.TryThisParseXml(elem))
                return false;

            var attr = elem.Attributes[DESCRIPTION];
            if (attr != null)
                Description = attr.Value;

            if (!elem.HasChildNodes)
                return true;

            foreach (var cNode in elem.ChildNodes)
            {
                var cElem = cNode as XmlElement;
                if (cElem == null)
                    continue;
                var sector = new T();
                if (sector.TryThisParseXml(cElem))
                    divisions.Add(sector);
            }

            return true;
        }

        public override string ToString()
        {
            //Value Description
            return string.Join("-", Value, Description);
        }
    }
}
