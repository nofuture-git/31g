using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using NoFuture.Exceptions;
using NoFuture.Shared;

namespace NoFuture.Read.Config
{
    public class ExeConfig : BaseXmlDoc
    {
        #region constants
        public const string XML_TRANSFORM_NS = "http://schemas.microsoft.com/XML-Document-Transform";
        #endregion

        #region fields
        private XmlWriter _xmlWriter;
        private readonly string _transformFileOut;
        #endregion

        #region ctors
        public ExeConfig(string configFile):base(configFile)
        {
            var configDir = Path.GetDirectoryName(configFile);
            if (string.IsNullOrEmpty(configDir))
                throw new ItsDeadJim($"Bad Path or File Name at '{configFile}'");

            _transformFileOut = Path.Combine(configDir, $"{DateTime.Now:yyyyMMdd-HHmmss}.config");
        }
        #endregion

        #region instance api

        /// <summary>
        /// Adds a new entry to the appSettings section of the *.config
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="commentAbove">optional comment to be placed above the new entry</param>
        public void AddAppSettingItem(string key, string value, string commentAbove = "")
        {
            if (_xmlDocument == null || _xmlDocument.DocumentElement == null)
                return;

            var appSettings = _xmlDocument.SelectSingleNode("//appSettings");

            if (appSettings == null)
            {
                //is the configSection node present
                var s = _xmlDocument.SelectSingleNode("//configSections") ??
                        _xmlDocument.DocumentElement.FirstChild;

                appSettings = _xmlDocument.CreateElement("appSettings");
                _xmlDocument.DocumentElement.InsertAfter(appSettings, s);
            }

            var addNode = _xmlDocument.CreateElement("add");
            var keyAttr = _xmlDocument.CreateAttribute("key");
            keyAttr.Value = key;
            var valAttr = _xmlDocument.CreateAttribute("value");
            valAttr.Value = value;

            addNode.Attributes.Append(keyAttr);
            addNode.Attributes.Append(valAttr);

            if (!string.IsNullOrWhiteSpace(commentAbove))
            {
                var rem = _xmlDocument.CreateComment(commentAbove);
                appSettings.AppendChild(rem);
            }

            appSettings.AppendChild(addNode);
        }

        /// <summary>
        /// Targets specific attributes of a .NET configuration file replacing any matches
        /// found in <see cref="regex2Values"/> Keys with the paired value.
        /// </summary>
        /// <param name="regex2Values"></param>
        /// <param name="swapConnStrs"></param>
        /// <returns>
        /// The filename of the config transformations which received all the changes 
        /// from the target.
        /// </returns>
        public string SplitAndSave(Hashtable regex2Values, bool swapConnStrs)
        {
            var xmlWriterSettings = new XmlWriterSettings { Indent = true, IndentChars = "  ", OmitXmlDeclaration = true };

            _xmlWriter = XmlWriter.Create(_transformFileOut, xmlWriterSettings);

            _xmlWriter.WriteStartDocument();
            _xmlWriter.WriteComment(
                " For more information on using web.config transformation visit http://go.microsoft.com/fwlink/?LinkId=125889 ");
            _xmlWriter.WriteStartElement("configuration");
            _xmlWriter.WriteAttributeString("xmlns", "xdt", null, XML_TRANSFORM_NS);

            SplitAppSettings(regex2Values, swapConnStrs);
            SplitConnectionStringNodes(regex2Values, swapConnStrs);

            if (_xmlDocument.SelectSingleNode("//system.serviceModel") != null)
            {
                _xmlWriter.WriteStartElement("system.serviceModel");
                SplitClientServiceModelNodes(regex2Values);
                SplitHostServiceModelNodes(regex2Values);
                _xmlWriter.WriteEndElement();
            }

            SplitLog4NetAppenderFileName(regex2Values);

            _xmlWriter.WriteEndElement(); //end configuration node
            _xmlWriter.Flush();
            _xmlWriter.Close();

            Save();

            return _transformFileOut;
        }
        #endregion

        #region static config types
        //found syntax by disassembling the WcfTestClient application
        public static System.Configuration.Configuration GetExeConfig(string exeConfigPath)
        {
            var exeConfigFileMap = new System.Configuration.ExeConfigurationFileMap();
            var machineConfig = System.Configuration.ConfigurationManager.OpenMachineConfiguration();
            exeConfigFileMap.MachineConfigFilename = machineConfig.FilePath;
            exeConfigFileMap.ExeConfigFilename = exeConfigPath;
            return System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(exeConfigFileMap,
                System.Configuration.ConfigurationUserLevel.None);
        }

        public static System.ServiceModel.Configuration.ServiceModelSectionGroup GetServiceModelSection(
            string exeConfigPath)
        {
            var svcConfig = GetExeConfig(exeConfigPath);
            return System.ServiceModel.Configuration.ServiceModelSectionGroup.GetSectionGroup(svcConfig);
        }

        public static System.ServiceModel.EndpointAddress GetEndpointAddress(string exeConfigPath, string upIdentity)
        {
            var svcSection = GetServiceModelSection(exeConfigPath);
            var endpointIdentity = System.ServiceModel.EndpointIdentity.CreateUpnIdentity(upIdentity);

            return new System.ServiceModel.EndpointAddress(svcSection.Client.Endpoints[0].Address, endpointIdentity);
        }
        #endregion

        #region internal helpers

        internal void SplitAppSettings(Hashtable regex2Values, bool swapConnStrs)
        {
            var appSettingsNodes = _xmlDocument.SelectSingleNode("//appSettings");
            if (appSettingsNodes == null || !appSettingsNodes.HasChildNodes)
                return;

            _xmlWriter.WriteStartElement("appSettings");

            foreach (var node in appSettingsNodes)
            {
                var cnode = node as XmlElement;
                if (cnode == null)
                    continue;
                var appValAttr = cnode.Attributes["value"];
                var appKeyAttr = cnode.Attributes["key"];
                if (appValAttr == null || appKeyAttr == null)
                    continue;
                var originalVal = appValAttr.Value;

                if (swapConnStrs && IsConnectionString(appValAttr.Value))
                {
                    AddAppSettingAddNodeToTransform(appKeyAttr.Value, appValAttr.Value);
                    appValAttr.Value = NfConfig.SqlServerDotNetConnString;
                    continue;
                }

                if (!RegexCatalog.AreAnyRegexMatch(appValAttr.Value, regex2Values.Keys.Cast<string>().ToArray()))
                    continue;

                AppropriateAllRegex(appValAttr, regex2Values);

                AddAppSettingAddNodeToTransform(appKeyAttr.Value, originalVal);
            }

            _xmlWriter.WriteEndElement();
        }

        internal void SplitConnectionStringNodes(Hashtable regex2Values, bool swapConnStrs)
        {
            var connectionStringNodes = _xmlDocument.SelectSingleNode("//connectionStrings");
            if (connectionStringNodes == null || !connectionStringNodes.HasChildNodes)
                return;
            _xmlWriter.WriteStartElement("connectionStrings");
            foreach (var node in connectionStringNodes.ChildNodes)
            {
                var constr = node as XmlElement;
                if (constr == null)
                    continue;
                var conStrAttr = constr.Attributes["connectionString"];
                var conStrNameAttr = constr.Attributes["name"];

                if (conStrAttr == null)
                    continue;

                var conStrVal = conStrAttr.Value;

                if (swapConnStrs ||
                    !RegexCatalog.AreAnyRegexMatch(conStrAttr.Value, regex2Values.Keys.Cast<string>().ToArray()))
                {

                    conStrAttr.Value = NfConfig.SqlServerDotNetConnString;

                    AddConnStringNodeToTransform(conStrNameAttr.Value, conStrVal);

                    continue;
                }

                AppropriateAllRegex(conStrAttr, regex2Values);

                AddConnStringNodeToTransform(conStrNameAttr.Value, conStrVal);

            }
            _xmlWriter.WriteEndElement();
        }

        internal void SplitClientServiceModelNodes(Hashtable regex2Values)
        {
            var svcClientNode = _xmlDocument.SelectSingleNode("//system.serviceModel/client");
            if (svcClientNode == null || !svcClientNode.HasChildNodes)
                return;

            _xmlWriter.WriteStartElement("client");

            foreach (var node in svcClientNode.ChildNodes)
            {
                var endPtNode = node as XmlElement;
                var addrAttr = endPtNode?.Attributes["address"];
                if (addrAttr == null)
                    continue;

                if (!Uri.IsWellFormedUriString(addrAttr.Value, UriKind.RelativeOrAbsolute))
                    continue;

                AddEndpointNodeToTransform(
                    endPtNode.Attributes["address"]?.Value,
                    endPtNode.Attributes["binding"]?.Value,
                    endPtNode.Attributes["bindingConfiguration"]?.Value,
                    endPtNode.Attributes["contract"]?.Value,
                    endPtNode.Attributes["name"]?.Value);

                if (!Shared.RegexCatalog.AreAnyRegexMatch(addrAttr.Value, regex2Values.Keys.Cast<string>().ToArray()))
                {
                    var addUri = new UriBuilder(addrAttr.Value) { Host = "localhost" };

                    addrAttr.Value = addUri.ToString();
                }
                else
                {
                    AppropriateAllRegex(addrAttr, regex2Values);
                }
            }
            _xmlWriter.WriteEndElement();
        }

        internal void SplitHostServiceModelNodes(Hashtable regex2Values)
        {
            var svcServicesNode = _xmlDocument.SelectNodes("//system.serviceModel/services/service");
            if (svcServicesNode == null || svcServicesNode.Count <= 0)
                return;

            _xmlWriter.WriteStartElement("services");

            foreach (var node in svcServicesNode)
            {
                var svcNode = node as XmlElement;
                if (svcNode == null)
                    continue;
                var behConfigAttr = svcNode.Attributes["behaviorConfiguration"];
                var nameAttr = svcNode.Attributes["name"];

                if (string.IsNullOrWhiteSpace(nameAttr?.Value))
                    continue;


                _xmlWriter.WriteStartElement("service");

                _xmlWriter.WriteAttributeString("name", nameAttr.Value);

                if (!string.IsNullOrWhiteSpace(behConfigAttr?.Value))
                    _xmlWriter.WriteAttributeString("behaviorConfiguration", behConfigAttr.Value);

                _xmlWriter.WriteAttributeString("Locator", XML_TRANSFORM_NS,
                    "Match(name)");

                var endPtNodes = _xmlDocument.SelectNodes(
                    $"//system.serviceModel/services/service[@name='{nameAttr.Value}']/endpoint");

                var hostNode = _xmlDocument.SelectSingleNode(
                    $"//system.serviceModel/services/service[@name='{nameAttr.Value}']/host/baseAddresses");

                if ((endPtNodes != null && endPtNodes.Count > 0))
                {
                    foreach (var nodei in endPtNodes)
                    {
                        var endPtNode = nodei as XmlElement;
                        if (endPtNode == null)
                            continue;

                        if (endPtNode.Attributes["address"] == null ||
                            string.IsNullOrWhiteSpace(endPtNode.Attributes["address"].Value) ||
                            string.Equals(endPtNode.Attributes["address"].Value, "mex",
                                StringComparison.OrdinalIgnoreCase) ||
                            !Uri.IsWellFormedUriString(endPtNode.Attributes["address"].Value, UriKind.RelativeOrAbsolute))
                        {
                            WriteEndpointIdDnsClosed(regex2Values, endPtNode, nameAttr);
                            continue;
                        }

                        WriteEndpointAddressClosed(regex2Values, endPtNode);
                    }
                }

                if (hostNode != null) //address is specified in the host/baseAddresses
                {
                    WriteServiceHostNodeClosed(regex2Values, hostNode);
                }

                _xmlWriter.WriteEndElement();//end service node
            }

            _xmlWriter.WriteEndElement();//end serviceS node
        }

        private void WriteEndpointIdDnsClosed(Hashtable regex2Values, XmlElement endPtNode, XmlAttribute nameAttr)
        {
            var endPtContractAttr = endPtNode.Attributes["contract"];
            if (string.IsNullOrWhiteSpace(endPtContractAttr?.Value))
                return;

            //yet another way serviceModel lets you define an address...
            var nodeVii =
                _xmlDocument.SelectSingleNode(
                    $"//system.serviceModel/services/service[@name='{nameAttr.Value}']/endpoint[@contract='{endPtContractAttr.Value}']/identity/dns");

            var dnsNode = nodeVii as XmlElement;

            var dnsValueAttr = dnsNode?.Attributes["value"];
            if (string.IsNullOrWhiteSpace(dnsValueAttr?.Value))
                return;

            var origDnsVal = dnsValueAttr.Value;
            AppropriateAllRegex(dnsValueAttr, regex2Values);

            WriteEndPointXmlUnclosed(
                endPtNode.Attributes["address"]?.Value,
                endPtNode.Attributes["binding"]?.Value,
                endPtNode.Attributes["bindingConfiguration"]?.Value,
                endPtNode.Attributes["contract"]?.Value,
                endPtNode.Attributes["name"]?.Value);

            _xmlWriter.WriteAttributeString("Transform",
                XML_TRANSFORM_NS,
                "Replace");

            _xmlWriter.WriteStartElement("identity");
            _xmlWriter.WriteStartElement("dns");
            _xmlWriter.WriteAttributeString("value", origDnsVal);

            _xmlWriter.WriteEndElement(); //end dns node
            _xmlWriter.WriteEndElement(); //end identity node
            _xmlWriter.WriteEndElement(); //end endpoint node
        }

        private void WriteEndpointAddressClosed(Hashtable regex2Values, XmlElement endPtNode)
        {
            var endPtAddrAttr = endPtNode.Attributes["address"];

            //implies the a contract with no end point address whatsoever.
            if (string.IsNullOrWhiteSpace(endPtAddrAttr?.Value))
                return;

            AddEndpointNodeToTransform(
                endPtAddrAttr.Value,
                endPtNode.Attributes["binding"]?.Value,
                endPtNode.Attributes["bindingConfiguration"]?.Value,
                endPtNode.Attributes["contract"]?.Value,
                endPtNode.Attributes["name"]?.Value);

            if (!Shared.RegexCatalog.AreAnyRegexMatch(endPtAddrAttr.Value, regex2Values.Keys.Cast<string>().ToArray()))
            {
                var addUri = new UriBuilder(endPtAddrAttr.Value) {Host = "localhost"};

                endPtAddrAttr.Value = addUri.ToString();
            }
            else
            {
                AppropriateAllRegex(endPtAddrAttr, regex2Values);
            }
        }

        private void WriteServiceHostNodeClosed(Hashtable regex2Values, XmlNode hostNode)
        {
            _xmlWriter.WriteStartElement("host");
            _xmlWriter.WriteAttributeString("Transform", XML_TRANSFORM_NS,
                "Replace");

            _xmlWriter.WriteStartElement("baseAddresses");
            foreach (var nodeV in hostNode.ChildNodes)
            {
                var bAddrNode = nodeV as XmlElement;
                if (bAddrNode == null)
                    continue;
                var baseAddrAttr = bAddrNode.Attributes["baseAddress"];
                if (string.IsNullOrEmpty(baseAddrAttr?.Value) ||
                    !Uri.IsWellFormedUriString(baseAddrAttr.Value, UriKind.RelativeOrAbsolute))
                {
                    _xmlWriter.WriteEndElement();
                    _xmlWriter.WriteEndElement();
                    continue;
                }

                _xmlWriter.WriteStartElement("add");
                _xmlWriter.WriteAttributeString("baseAddress", baseAddrAttr.Value);


                if (!Shared.RegexCatalog.AreAnyRegexMatch(baseAddrAttr.Value, regex2Values.Keys.Cast<string>().ToArray()))
                {
                    var bAddrUri = new UriBuilder(baseAddrAttr.Value) {Host = "localhost"};
                    baseAddrAttr.Value = bAddrUri.ToString();
                }
                else
                {
                    AppropriateAllRegex(baseAddrAttr, regex2Values);
                }
                _xmlWriter.WriteEndElement();
            }
            _xmlWriter.WriteEndElement();//end baseAddresses node
            _xmlWriter.WriteEndElement();//end host node
        }

        internal void SplitLog4NetAppenderFileName(Hashtable regex2Values)
        {
            var appenderNodes = _xmlDocument.SelectNodes("//log4net/appender");
            if (appenderNodes == null || appenderNodes.Count <= 0)
                return;

            _xmlWriter.WriteStartElement("log4net");

            foreach (var node in appenderNodes)
            {
                var appenderNode = node as XmlElement;
                if (appenderNode == null)
                    continue;
                var nameAttr = appenderNode.Attributes["name"];
                var typeAttr = appenderNode.Attributes["type"];

                if (string.IsNullOrWhiteSpace(nameAttr?.Value))
                    continue;
                if (string.IsNullOrWhiteSpace(typeAttr?.Value))
                    continue;

                var appenderFileNode =
                    _xmlDocument.SelectSingleNode($"//log4net/appender[@name='{nameAttr.Value}']/file");
                var appenderFileElem = appenderFileNode as XmlElement;

                if (appenderFileElem == null)
                    continue;
                var origFile = appenderFileElem.Attributes["value"];
                var origFileVal = origFile.Value;
                AppropriateAllRegex(origFile, regex2Values);

                _xmlWriter.WriteStartElement("appender");
                _xmlWriter.WriteAttributeString("name", nameAttr.Value);
                _xmlWriter.WriteAttributeString("type", typeAttr.Value);

                _xmlWriter.WriteAttributeString("Locator", XML_TRANSFORM_NS,
                    "Match(name)");

                _xmlWriter.WriteStartElement("file");
                _xmlWriter.WriteAttributeString("value", origFileVal);

                _xmlWriter.WriteAttributeString("Transform", XML_TRANSFORM_NS,
                    "Replace");
                _xmlWriter.WriteEndElement();//end file node

                _xmlWriter.WriteEndElement();//end appender node

            }

            _xmlWriter.WriteEndElement();//end log4net node
        }

        internal void WriteEndPointXmlUnclosed(string addr, string binding, string bindingConfig, string contract,
            string name)
        {

            _xmlWriter.WriteStartElement("endpoint");
            if (!string.IsNullOrWhiteSpace(addr))
            {
                _xmlWriter.WriteAttributeString("address", addr);
            }
            if (!string.IsNullOrWhiteSpace(binding))
            {
                _xmlWriter.WriteAttributeString("binding", binding);
            }
            if (!string.IsNullOrWhiteSpace(bindingConfig))
            {
                _xmlWriter.WriteAttributeString("bindingConfiguration", bindingConfig);
            }
            if (!string.IsNullOrWhiteSpace(contract))
            {
                _xmlWriter.WriteAttributeString("contract", contract);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                _xmlWriter.WriteAttributeString("name", name);
            }
            
        }

        internal void AddEndpointNodeToTransform(string addr, string binding, string bindingConfig, string contract,
            string name)
        {

            WriteEndPointXmlUnclosed(addr, binding,bindingConfig,contract,name);

            if (!string.IsNullOrWhiteSpace(name))
            {
                _xmlWriter.WriteAttributeString("Transform", XML_TRANSFORM_NS,
                    "SetAttributes");
                _xmlWriter.WriteAttributeString("Locator", XML_TRANSFORM_NS,
                    "Match(name)");
            }
            else
            {
                _xmlWriter.WriteAttributeString("Transform", XML_TRANSFORM_NS,
                    "Replace");
            }

            _xmlWriter.WriteEndElement();
        }

        internal void AddConnStringNodeToTransform(string name, string connStr)
        {
            _xmlWriter.WriteStartElement("add");
            _xmlWriter.WriteAttributeString("name", name);
            _xmlWriter.WriteAttributeString("connectionString", connStr);
            _xmlWriter.WriteAttributeString("Transform", XML_TRANSFORM_NS,
                "SetAttributes");
            _xmlWriter.WriteAttributeString("Locator", XML_TRANSFORM_NS,
                "Match(name)");
            _xmlWriter.WriteEndElement();
        }

        internal void AddAppSettingAddNodeToTransform(string key, string val)
        {
            _xmlWriter.WriteStartElement("add");
            _xmlWriter.WriteAttributeString("key", key);
            _xmlWriter.WriteAttributeString("value", val);
            _xmlWriter.WriteAttributeString("Transform", XML_TRANSFORM_NS,
                "SetAttributes");
            _xmlWriter.WriteAttributeString("Locator", XML_TRANSFORM_NS,
                "Match(key)");
            _xmlWriter.WriteEndElement();
            
        }


        internal static void AppropriateAllRegex(XmlAttribute attr, Hashtable regex2Values)
        {
            var newAppVal = attr?.Value;
            if (string.IsNullOrEmpty(newAppVal))
                return;
            if (regex2Values == null || regex2Values.Count <= 0)
                return;

            RegexCatalog.AppropriateAllRegex(ref newAppVal, regex2Values);
            attr.Value = newAppVal;
        }

        internal static bool IsConnectionString(string someValue)
        {
            var partsA = someValue.Split(';');
            var partsB = someValue.Split('=');

            return partsA.Length > 1 && partsA.Length == partsB.Length;
        }
        #endregion
    }
}
