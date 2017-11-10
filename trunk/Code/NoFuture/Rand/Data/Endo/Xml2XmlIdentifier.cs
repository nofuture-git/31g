using System;
using System.Collections.Generic;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Data.Endo
{
    [Serializable]
    public class Xml2XmlIdentifier : XmlDocXrefIdentifier
    {
        #region fields
        private readonly XmlDocument _dataFile;
        private readonly string _nodeName;
        #endregion

        #region properties
        
        public string NodeName => _nodeName;
        public override string Abbrev => string.Empty;

        #endregion

        #region ctor
        public Xml2XmlIdentifier(string dataFileName, string nodeName)
        {
            if(string.IsNullOrWhiteSpace(dataFileName))
                throw new ArgumentNullException(nameof(dataFileName));
            if(string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentNullException(nameof(nodeName));

            TreeData.GetXmlDataSource(dataFileName, ref _dataFile);

            _nodeName = nodeName;
            _localName = X_DATA_REFERENCE;
        }
        #endregion

        #region methods
        public override bool TryThisParseXml(XmlElement elem)
        {
            if (!base.TryThisParseXml(elem))
                return false;

            if (!elem.HasChildNodes)
                return true;

            var tempIdList = new List<XrefIdentifier>();
            foreach (var cnode in elem.ChildNodes)
            {
                var cElem = cnode as XmlElement;
                if (cElem == null)
                    continue;

                if(string.Equals(cElem.Name, X_REF, StringComparison.OrdinalIgnoreCase))
                {
                    if(!cElem.HasChildNodes)
                        throw new RahRowRagee($"The '{X_REF}' node must have at least one child " +
                                              "element to be a valid cross reference");

                    foreach (var xRefIdNode in cElem.ChildNodes)
                    {
                        var xRefIdElem = xRefIdNode as XmlElement;

                        var idValue = xRefIdElem?.InnerText;
                        if (string.IsNullOrWhiteSpace(idValue))
                            continue;

                        var dataFileXrefNode = _dataFile.SelectSingleNode($"//{_nodeName}[@ID='{idValue}']");

                        var dataFileXrefElem = dataFileXrefNode as XmlElement;

                        var xmlDocId = new XmlDocXrefIdentifier(_nodeName);
                        if (xmlDocId.TryThisParseXml(dataFileXrefElem))
                        {
                            tempIdList.Add(xmlDocId);
                        }
                    }

                    continue;
                }

                var nameAttr = cElem.Attributes[NAME];
                var valueAttr = cElem.Attributes[VALUE];

                if (nameAttr == null || valueAttr == null)
                    continue;
                if(string.IsNullOrWhiteSpace(nameAttr.Value) || string.IsNullOrWhiteSpace(valueAttr.Value))
                    continue;

                ReferenceDictionary.Add(nameAttr.Value, valueAttr.Value);
            }

            XrefIds = new NamedIdentifier[tempIdList.Count];
            Array.Copy(tempIdList.ToArray(), XrefIds, XrefIds.Length);

            return XrefIds.Length > 0;
        }
        #endregion
    }
}