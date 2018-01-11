using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Core
{
    [Serializable]
    public class XRefGroup : XmlDocXrefIdentifier
    {
        #region fields
        internal static XmlDocument XrefXml;
        #endregion

        #region properties
        public string NodeName { get; set; }
        public XrefIdentifier[] XRefData { get; set; }
        public override string LocalName => X_REF_GROUP;
        #endregion

        #region ctor
        public XRefGroup()
        {
            _localName = X_REF_GROUP;
        }
        #endregion

        #region methods

        /// <summary>
        /// Adds a simple Identifier name value to <see cref="TreeData.XRefXml"/>
        /// </summary>
        /// <param name="xRefId"></param>
        /// <param name="id"></param>
        /// <param name="propertyName">
        /// Optional, if null then defaults to <see cref="id"/> Type name.
        /// </param>
        public static bool AddXrefValues(Tuple<Type, string> xRefId, Identifier id, string propertyName = null)
        {
            const string SOURCE = "source";
            if (string.IsNullOrWhiteSpace(xRefId?.Item2) || xRefId.Item1 == null)
                return false;
            if(id == null)
                return false;
            var asmOfId = xRefId.Item1.Assembly;
            XrefXml = XrefXml ?? GetEmbeddedXmlDoc(XREF_XML_DATA_FILE, asmOfId);
            if (XrefXml == null)
                return false;
            var xrefSrcNode = XrefXml.SelectSingleNode($"//{SOURCE}");
            if (xrefSrcNode == null)
                return false;

            var simpleNameValuePair = new Tuple<string, string>(propertyName ?? id.GetType().Name, id.Value);

            //heirarchy of xpaths
            var xRefGroupXPath = $"//{X_REF_GROUP}[@{DATA_TYPE}='{xRefId.Item1.FullName}']";
            var xDataReferenceXPath = xRefGroupXPath + $"//{X_REF_ID}[text()='{xRefId.Item2}']/../..";
            var xRefAddXPath = xDataReferenceXPath + $"/{ADD}[@{NAME}='{simpleNameValuePair.Item1}']";
            var xRefAddChildXPath = xRefAddXPath + $"/{ADD}[@{NAME}='Value']";

            //get the x-ref-group node
            var xRefGroupNode = XrefXml.SelectSingleNode(xRefGroupXPath) as XmlElement;
            if (xRefGroupNode == null)
            {
                xRefGroupNode = XrefXml.CreateElement(X_REF_GROUP);
                var xRefGroupAttr = XrefXml.CreateAttribute(DATA_TYPE);
                xRefGroupAttr.Value = xRefId.Item1.FullName;
                xRefGroupNode.Attributes.Append(xRefGroupAttr);
                

                if (xrefSrcNode.HasChildNodes)
                    xrefSrcNode.InsertAfter(xRefGroupNode, xrefSrcNode.LastChild);
                else
                    xrefSrcNode.AppendChild(xRefGroupNode);
            }

            //get the x-data-reference node
            var xDataReferenceNode = XrefXml.SelectSingleNode(xDataReferenceXPath);
            if (xDataReferenceNode == null)
            {
                xDataReferenceNode = XrefXml.CreateElement(X_DATA_REFERENCE);
                var xRefNode = XrefXml.CreateElement(X_REF);
                var xRefIdNode = XrefXml.CreateElement(X_REF_ID);
                xRefIdNode.InnerText = xRefId.Item2;
                xRefNode.AppendChild(xRefIdNode);
                xDataReferenceNode.AppendChild(xRefNode);
                xRefGroupNode.AppendChild(xDataReferenceNode);
            }

            //get the outer add node
            var xrefAdd = XrefXml.SelectSingleNode(xRefAddXPath) as XmlElement;
            if (xrefAdd == null)
            {
                xrefAdd = XrefXml.CreateElement(ADD);
                if (!xDataReferenceNode.HasChildNodes)
                    xDataReferenceNode.AppendChild(xrefAdd);
                else
                    xDataReferenceNode.InsertAfter(xrefAdd, xDataReferenceNode.LastChild);
            }

            var xrefNameAttr = xrefAdd.Attributes[NAME] ?? XrefXml.CreateAttribute(NAME);
            xrefNameAttr.Value = simpleNameValuePair.Item1;

            if (xrefAdd.Attributes[NAME] == null)
                xrefAdd.Attributes.Append(xrefNameAttr);

            //get the inner add node
            var childNode = XrefXml.SelectSingleNode(xRefAddChildXPath) as XmlElement;
            if (childNode == null)
            {
                childNode = XrefXml.CreateElement(ADD);
                if (!xrefAdd.HasChildNodes)
                    xrefAdd.AppendChild(childNode);
                else
                    xrefAdd.InsertAfter(childNode, xrefAdd.LastChild);
            }

            var nameAttr = childNode.Attributes[NAME] ?? XrefXml.CreateAttribute(NAME);
            nameAttr.Value = "Value";
            var valAttr = childNode.Attributes[VALUE] ?? XrefXml.CreateAttribute(VALUE);
            valAttr.Value = simpleNameValuePair.Item2;

            if (childNode.Attributes[NAME] == null)
                childNode.Attributes.Append(nameAttr);
            if (childNode.Attributes[VALUE] == null)
                childNode.Attributes.Append(valAttr);

            return true;
        }

        public static XmlNodeList GetXrefAddNodes(Type t, string xrefId)
        {
            if (t == null)
                return null;
            if (string.IsNullOrWhiteSpace(xrefId))
                return null;
            var asmOfT = t.Assembly;
            XrefXml = XrefXml ?? GetEmbeddedXmlDoc(XREF_XML_DATA_FILE, asmOfT);
            return
                XrefXml?.SelectNodes(
                    $"//{X_REF_GROUP}[@{DATA_TYPE}='{t.FullName}']//{X_REF_ID}[text()='{xrefId}']/../../{ADD}");
        }

        /// <summary>
        /// Assigns property values on the <see cref="rtInstance"/> from 
        /// the x-ref data source.
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="rtInstance"></param>
        public static void SetTypeXrefValue(XmlElement elem, object rtInstance)
        {
            var nameAttr = elem.Attributes[NAME];
            if (string.IsNullOrWhiteSpace(nameAttr?.Value))
                return;

            if (rtInstance == null)
                return;

            var propertyName = nameAttr.Value;
            var pi = rtInstance.GetType().GetProperty(propertyName);

            var valueAttr = elem.Attributes[VALUE];
            if (!string.IsNullOrWhiteSpace(valueAttr?.Value))
            {
                pi.SetValue(rtInstance, valueAttr.Value, null);
                return;
            }

            if (!elem.HasChildNodes || pi == null) return;

            var piType = pi.PropertyType;
            var propertyInstance = Activator.CreateInstance(piType);

            //expecting the type to have a getter and setter of generic List<T>
            if (NfReflect.IsEnumerableReturnType(piType))
            {
                var addMi = piType.GetMethod("Add");
                if (addMi == null)
                    return;
                var enumerableTypeName = NfReflect.GetLastTypeNameFromArrayAndGeneric(piType);
                if (string.IsNullOrWhiteSpace(enumerableTypeName))
                    return;

                var asm = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.GetType(enumerableTypeName) != null);
                if (asm == null)
                    return;

                var enumerableType = asm.GetType(enumerableTypeName);
                if (enumerableType == null)
                    return;

                var enumerableInstance = Activator.CreateInstance(enumerableType);
                foreach (var cNode in elem.ChildNodes)
                {
                    var cElem = cNode as XmlElement;
                    if (cElem == null)
                        continue;

                    SetTypeXrefValue(cElem, enumerableInstance);
                    
                }
                addMi.Invoke(propertyInstance, new[] { enumerableInstance });
            }
            else//no generic type
            {
                foreach (var cNode in elem.ChildNodes)
                {
                    var cElem = cNode as XmlElement;
                    if (cElem == null)
                        continue;

                    SetTypeXrefValue(cElem, propertyInstance);
                }
                
            }
            pi.SetValue(rtInstance, propertyInstance, null);
        }

        public override bool TryThisParseXml(XmlElement elem)
        {
            if (!base.TryThisParseXml(elem))
                return false;

            if (elem.Attributes[NODE_NAME] != null && elem.Attributes[DATA_FILE] != null)
                return TryParseXml2Xml(elem);

            return false;
        }

        protected bool TryParseXml2Xml(XmlElement elem)
        {
            var dataFileAttr = elem.Attributes[DATA_FILE];
            if (string.IsNullOrWhiteSpace(dataFileAttr?.Value))
                throw new InvalidOperationException($"This {LocalName} is not an XML-to-XML kind of cross-reference");

            var nodeNameAttr = elem.Attributes[NODE_NAME];

            if (string.IsNullOrWhiteSpace(nodeNameAttr?.Value))
                throw new InvalidOperationException($"This {LocalName} is not an XML-to-XML kind of cross-reference");

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

                if (!String.Equals(Path.GetExtension(externalFile), "xml", StringComparison.OrdinalIgnoreCase))
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
}