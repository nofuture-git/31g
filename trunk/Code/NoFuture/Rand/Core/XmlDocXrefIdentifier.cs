using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// Type coupled to XML's ID attribute
    /// </summary>
    [Serializable]
    public class XmlDocXrefIdentifier : XrefIdentifier
    {
        public XmlDocXrefIdentifier() { }
        public XmlDocXrefIdentifier(string localName) : base(localName) { }

        #region constants

        protected const string X_REF_GROUP = "x-ref-group";
        protected const string X_REF = "x-ref";
        protected const string X_DATA_REFERENCE = "x-data-reference";
        protected const string X_REF_ID = "x-ref-id";
        protected const string DATA_TYPE = "data-type";
        protected const string ADD = "add";
        protected const string NAME = "name";
        protected const string VALUE = "value";
        protected const string NODE_NAME = "node-name";
        protected const string DATA_FILE = "data-file";
        protected const string ID = "Id";
        #endregion 

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
            var attr = (elem.Attributes[ID.ToUpper()] ?? elem.Attributes[ID.ToLower()]) ?? elem.Attributes[ID];
            if (attr != null)
                Value = attr.Value;
            return true;
        }

        /// <summary>
        /// Returns the same value as <see cref="NamedIdentifier.LocalName"/>
        /// </summary>
        public override string Abbrev => LocalName;

        public override bool Equals(object obj)
        {
            var xdib = obj as XmlDocXrefIdentifier;
            if (xdib == null)
                return false;

            return string.Equals(LocalName, xdib.LocalName) &&
                   string.Equals(Value, xdib.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            var d = LocalName?.GetHashCode() ?? 1;
            var id = Value?.GetHashCode() ?? 0;
            return d + id;
        }

        protected internal static XmlDocument GetEmbeddedXmlDoc(string xmlDataFileName)
        {
            if (string.IsNullOrWhiteSpace(xmlDataFileName))
                return null;

            var asm = Assembly.GetExecutingAssembly();

            var data = asm.GetManifestResourceStream($"{asm.GetName().Name}.{xmlDataFileName}");
            if (data == null)
                return null;

            var strmRdr = new StreamReader(data);
            var xmlData = strmRdr.ReadToEnd();
            var assignTo = new XmlDocument();
            assignTo.LoadXml(xmlData);
            return assignTo;
        }

        /// <summary>
        /// Helper method to get a cumulative probability table from XML
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="parentNodeName"></param>
        /// <param name="entryNodeName"></param>
        /// <returns></returns>
        public static Dictionary<string, double> GetProbTable(XmlDocument xml, string parentNodeName,
            string entryNodeName)
        {
            const string WEIGHT = "prob";
            var dictOut = new Dictionary<string, double>();
            var elems = xml?.SelectSingleNode($"//{parentNodeName}");

            if (elems == null || !elems.HasChildNodes)
                return dictOut;

            foreach (var node in elems.ChildNodes)
            {
                var elem = node as XmlElement;
                if (elem == null)
                    continue;
                var key = elem.Attributes[entryNodeName]?.Value;
                var strVal = elem.Attributes[WEIGHT]?.Value;

                if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(strVal))
                    continue;

                if (!double.TryParse(strVal, out var val))
                    continue;

                if (dictOut.ContainsKey(key))
                    dictOut[key] = val;
                else
                    dictOut.Add(key, val);
            }
            return dictOut;
        }
    }
}
