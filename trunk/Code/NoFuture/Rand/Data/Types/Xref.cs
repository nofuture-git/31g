using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using NoFuture.Exceptions;

namespace NoFuture.Rand.Data.Types
{
    [Serializable]
    public class XRefGroup : XmlDocXrefIdentifier
    {
        #region fields
        internal const string X_REF_GROUP = "x-ref-group";
        private static XRefGroup[] _allXref;
        #endregion

        #region properties
        public string NodeName { get; set; }
        public XrefIdentifier[] XRefData { get; set; }
        public override string LocalName
        {
            get { return X_REF_GROUP; }
        }
        #endregion

        #region ctors 

        public XRefGroup()
        {
            _localName = X_REF_GROUP;
        }
        #endregion

        public static XRefGroup[] AllXrefGroups
        {
            get
            {
                if (_allXref != null)
                    return _allXref;
                if (string.IsNullOrWhiteSpace(BinDirectories.Root))
                    return null;
                if (TreeData.XRefXml == null)
                    return null;

                var elems = TreeData.XRefXml.SelectNodes(string.Format("//{0}", X_REF_GROUP));
                if (elems == null || elems.Count <= 0)
                    return null;

                var tempList = new List<XRefGroup>();
                foreach (var node in elems)
                {
                    var elm = node as XmlElement;
                    if (elm == null)
                        continue;
                    var xrefGrp = new XRefGroup();
                    if(xrefGrp.TryThisParseXml(elm))
                        tempList.Add(xrefGrp);
                }

                _allXref = tempList.ToArray();
                return _allXref;
            }
        }

        #region methods
        public override bool TryThisParseXml(XmlElement elem)
        {
            if (!base.TryThisParseXml(elem))
                return false;

            if (elem.Attributes["node-name"] != null && elem.Attributes["data-file"] != null)
                return TryParseXml2Xml(elem);

            return false;
        }

        protected bool TryParseXml2Xml(XmlElement elem)
        {
            var dataFileAttr = elem.Attributes["data-file"];
            if (dataFileAttr == null || string.IsNullOrWhiteSpace(dataFileAttr.Value))
                throw new InvalidOperationException(string.Format("This {0} is not an XML-to-XML kind of cross-reference", LocalName));

            var nodeNameAttr = elem.Attributes["node-name"];

            if (nodeNameAttr == null || string.IsNullOrWhiteSpace(nodeNameAttr.Value))
                throw new InvalidOperationException(string.Format("This {0} is not an XML-to-XML kind of cross-reference", LocalName));

            NodeName = nodeNameAttr.Value;

            if (!elem.HasChildNodes)
                return true;

            var tempList = new List<Xml2XmlIdentifier>();
            foreach (var cnode in elem.ChildNodes)
            {
                var cElem = cnode as XmlElement;
                if (cElem == null)
                    continue;

                var externalFile = dataFileAttr.Value;

                if (!string.Equals(Path.GetExtension(externalFile), "xml", StringComparison.OrdinalIgnoreCase))
                    continue;

                var xdata = new Xml2XmlIdentifier(dataFileAttr.Value, NodeName);
                if (xdata.TryThisParseXml(cElem))
                    tempList.Add(xdata);
            }
            XRefData = new XrefIdentifier[tempList.Count];
            Array.Copy(tempList.ToArray(), XRefData, XRefData.Length);
            return true;            
        }

        #endregion
    }

    [Serializable]
    public class Xml2XmlIdentifier : XmlDocXrefIdentifier
    {
        #region fields
        private readonly XmlDocument _dataFile;
        private readonly string _nodeName;
        #endregion

        #region properties
        
        public string NodeName { get { return _nodeName; } }
        public override string Abbrev
        {
            get { return string.Empty; }
        }
        #endregion

        #region ctors
        public Xml2XmlIdentifier(string dataFileName, string nodeName)
        {
            if(string.IsNullOrWhiteSpace(dataFileName))
                throw new ArgumentNullException("dataFileName");
            if(string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentNullException("nodeName");

            TreeData.GetXmlDataSource(TreeData.GetDataDocPath(dataFileName), ref _dataFile);

            _nodeName = nodeName;
            _localName = "x-data-reference";
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

                if(string.Equals(cElem.Name,"x-ref", StringComparison.OrdinalIgnoreCase))
                {
                    if(!cElem.HasChildNodes)
                        throw new RahRowRagee("The 'x-ref' node must have at least one child " +
                                              "element to be a valid cross reference");

                    foreach (var xRefIdNode in cElem.ChildNodes)
                    {
                        var xRefIdElem = xRefIdNode as XmlElement;
                        if (xRefIdElem == null)
                            continue;

                        var idValue = xRefIdElem.InnerText;
                        if (string.IsNullOrWhiteSpace(idValue))
                            continue;

                        var dataFileXrefNode = _dataFile.SelectSingleNode(string.Format("//{0}[@ID='{1}']", _nodeName, idValue));

                        var dataFileXrefElem = dataFileXrefNode as XmlElement;

                        var xmlDocId = new XmlDocXrefIdentifier {_localName = _nodeName};
                        if (xmlDocId.TryThisParseXml(dataFileXrefElem))
                        {
                            tempIdList.Add(xmlDocId);
                        }
                    }

                    continue;
                }

                var nameAttr = cElem.Attributes["name"];
                var valueAttr = cElem.Attributes["value"];

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
