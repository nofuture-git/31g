﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using NoFuture.Shared;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;

namespace NoFuture.Read
{
    public abstract class BaseXmlDoc
    {
        #region fields
        protected readonly XmlDocument _xmlDocument;

        private readonly string _originalReadFile;
        private string _fileFullName;
        public const string LIB = "_lib";
        private readonly System.Security.Cryptography.MD5 _chkSum = System.Security.Cryptography.MD5.Create();
        #endregion

        #region ctor
        protected BaseXmlDoc(string xmlDoc)
        {
            _originalReadFile = xmlDoc;
            _fileFullName = xmlDoc;
            if (string.IsNullOrWhiteSpace(xmlDoc))
            {
                throw new ItsDeadJim("Bad Path or File Name");
            }
            if (!File.Exists(xmlDoc))
            {
                throw new ItsDeadJim($"Bad Path or File Name at '{xmlDoc}'");
            }
            _xmlDocument = new XmlDocument();
            _xmlDocument.Load(xmlDoc);
        }
        #endregion

        #region base api
        public virtual string DirectoryName => Path.GetDirectoryName(_fileFullName) ?? NfSettings.AppData;
        public virtual string FileName => Path.GetFileName(_fileFullName);
        protected internal string OriginalReadFileFullName => _originalReadFile;
        internal XmlDocument Xml => _xmlDocument;
        /// <summary>
        /// Writes the current in memory contents to file.
        /// </summary>
        public virtual void Save(Encoding encoding = null)
        {
            NfPath.SaveXml(_xmlDocument, _fileFullName, encoding);
        }

        public virtual void SaveAs(string fullName, Encoding encoding = null)
        {
            if(string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentNullException(nameof(fullName));
            _fileFullName = fullName;
            NfPath.SaveXml(_xmlDocument, fullName, encoding);
        }

        /// <summary>
        /// Utility method to get inner text of some node
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public virtual string GetInnerText(string xpath)
        {
            if (string.IsNullOrWhiteSpace(xpath))
                return null;
            var singleNode = _xmlDocument.SelectSingleNode(xpath);
            return singleNode?.InnerText;
        }

        /// <summary>
        /// Utility method to set the inner text of some node
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="theValue"></param>
        public virtual void SetInnerText(string xpath, string theValue)
        {
            if (string.IsNullOrWhiteSpace(xpath) || theValue == null)
                return;
            var singleNode = _xmlDocument.SelectSingleNode(xpath);
            if (singleNode == null)
                return;
            singleNode.InnerText = theValue;
        }

        /// <summary>
        /// Utility method to get a single attributes value at the given xpath
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public virtual string GetAttrValue(string xpath, string attrName)
        {
            var attr = GetAttribute(xpath, attrName);
            return attr?.Value;
        }

        /// <summary>
        /// Utility method to set a single attributes value at the given xpath
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="attrName"></param>
        /// <param name="newValue"></param>
        public virtual void SetAttrValue(string xpath, string attrName, string newValue)
        {
            var attr = GetAttribute(xpath, attrName);
            if (attr == null)
                return;
            attr.Value = newValue;
        }

        /// <summary>
        /// Simple helper method around DOM RemoveChild
        /// </summary>
        /// <param name="someName"></param>
        /// <returns></returns>
        public virtual int DropNodesNamed(string someName)
        {
            if (string.IsNullOrWhiteSpace(someName))
                return 0;
            var nodesByName = _xmlDocument.GetElementsByTagName(someName);
            if (nodesByName.Count <= 0)
                return 0;
            var counter = 0;
            for (var i = nodesByName.Count-1; i >= 0; i--)
            {
                var tNode = nodesByName.Item(i);
                if (tNode?.ParentNode?.RemoveChild(tNode) != null)
                {
                    counter += 1;
                }
            }
            return counter;
        }

        /// <summary>
        /// Like its counterpart <see cref="DropNodesNamed"/> only more selective
        /// in use of an xpath.
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public virtual int DropNodesAtXpath(string xpath)
        {
            if (string.IsNullOrWhiteSpace(xpath))
                return 0;
            var nodesByName = _xmlDocument.SelectNodes(xpath);
            if (nodesByName == null || nodesByName.Count <= 0)
                return 0;
            var counter = 0;
            for (var i = nodesByName.Count - 1; i >= 0; i--)
            {
                var tNode = nodesByName.Item(i);
                if (tNode?.ParentNode?.RemoveChild(tNode) != null)
                {
                    counter += 1;
                }
            }
            return counter;
        }

        protected virtual XmlAttribute GetAttribute(string xpath, string attrName)
        {
            if (string.IsNullOrWhiteSpace(xpath) || string.IsNullOrWhiteSpace(attrName))
                return null;

            var node = _xmlDocument.SelectSingleNode(xpath);

            var elem = node as XmlElement;
            return elem?.Attributes[attrName];
        }

        public virtual bool AddAttribute(XmlElement elem, string attrName, string attrValue)
        {
            if (elem == null || string.IsNullOrWhiteSpace(attrName))
                return false;
            var attr = elem.Attributes[attrName] ?? _xmlDocument.CreateAttribute(attrName);
            attr.Value = attrValue;
            if (!elem.HasAttribute(attrName))
                elem.Attributes.Append(attr);
            return true;
        }

        public override int GetHashCode()
        {
            return _originalReadFile.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var bXml = obj as BaseXmlDoc;
            if (bXml == null)
                return false;

            return bXml.GetMd5ChkSum() == GetMd5ChkSum();
        }

        public string GetMd5ChkSum()
        {
            var bytes = Encoding.UTF8.GetBytes(_xmlDocument.OuterXml);
            var hash = "";
            foreach (var b in _chkSum.ComputeHash(bytes))
            {
                hash += b.ToString("x2");
            }
            return hash;
        }

        #endregion

        #region static utilities
        public static string SpliceInXmlNs(string xpath, string xmlNs)
        {
            if (string.IsNullOrWhiteSpace(xpath) || string.IsNullOrWhiteSpace(xmlNs))
                return xpath;

            if (xpath.Contains(":")) return xpath;

            var xPathParts = xpath.Split('/').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var newXpath = xmlNs + ":" + string.Join($"/{xmlNs}:", xPathParts);
            if (xpath.StartsWith("//"))
                xpath = $"//{newXpath}";

            return xpath;
        }
        #endregion
    }
}
