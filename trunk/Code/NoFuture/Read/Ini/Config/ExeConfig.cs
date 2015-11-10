﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using NoFuture.Exceptions;
using System.Text.RegularExpressions;

namespace NoFuture.Read.Config
{
    public class ExeConfig
    {
        #region fields
        private readonly XmlDocument _configContent;
        private readonly XmlWriter _xmlWriter;
        private readonly string _existingConfigFullName;
        private readonly string _transformFileOut;
        #endregion

        #region ctors
        public ExeConfig(string configFile)
        {
            if (!File.Exists(configFile))
                throw new ItsDeadJim(string.Format("Bad Path or File Name at '{0}'", configFile));

            var configDir = Path.GetDirectoryName(configFile);
            if(string.IsNullOrEmpty(configDir))
                throw new ItsDeadJim(string.Format("Bad Path or File Name at '{0}'", configFile));

            _existingConfigFullName = configFile;

            _transformFileOut = Path.Combine(configDir, string.Format("{0:yyyyMMdd-HHmmss}.config", DateTime.Now));

            _configContent = new XmlDocument();
            _configContent.Load(configFile);

            var xmlWriterSettings = new XmlWriterSettings {Indent = true, IndentChars = "  ", OmitXmlDeclaration = true};

            _xmlWriter = XmlWriter.Create(_transformFileOut, xmlWriterSettings);
        }
        #endregion

        #region instance api
        public void SplitCustom(Action<XmlDocument, XmlWriter> customAction)
        {
            customAction.Invoke(_configContent, _xmlWriter);
        }

        public string WriteContentFile(Hashtable regex2Values, bool swapConnStrs)
        {
            _xmlWriter.WriteStartDocument();
            _xmlWriter.WriteComment(
                " For more information on using web.config transformation visit http://go.microsoft.com/fwlink/?LinkId=125889 ");
            _xmlWriter.WriteStartElement("configuration");
            _xmlWriter.WriteAttributeString("xmlns", "xdt", null, "http://schemas.microsoft.com/XML-Document-Transform");

            SplitAppSettings(regex2Values, swapConnStrs);
            SplitConnectionStringNodes(regex2Values);

            if (_configContent.SelectSingleNode("//system.serviceModel") != null)
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

            using (var xmlWriter = new XmlTextWriter(_existingConfigFullName, Encoding.UTF8) { Formatting = Formatting.Indented })
            {
                _configContent.WriteContentTo(xmlWriter);
                xmlWriter.Flush();
                xmlWriter.Close();
            }

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
            var appSettingsNodes = _configContent.SelectSingleNode("//appSettings");
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
                    appValAttr.Value = Shared.Constants.SqlServerDotNetConnString;
                    continue;
                }

                if (!AreAnyRegexMatch(appValAttr.Value, regex2Values))
                    continue;

                AppropriateAllRegex(appValAttr, regex2Values);

                AddAppSettingAddNodeToTransform(appKeyAttr.Value, originalVal);
            }

            _xmlWriter.WriteEndElement();
        }

        internal void SplitConnectionStringNodes(Hashtable regex2Values)
        {
            var connectionStringNodes = _configContent.SelectSingleNode("//connectionStrings");
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

                if (!AreAnyRegexMatch(conStrAttr.Value, regex2Values))
                {
                    conStrAttr.Value = Shared.Constants.SqlServerDotNetConnString;

                    AddConnStringNodeToTransform(conStrNameAttr.Value, conStrAttr.Value);

                    continue;
                }

                AppropriateAllRegex(conStrAttr, regex2Values);

                AddConnStringNodeToTransform(conStrNameAttr.Value, conStrAttr.Value);

            }
            _xmlWriter.WriteEndElement();
        }

        internal void SplitClientServiceModelNodes(Hashtable regex2Values)
        {
            var svcClientNode = _configContent.SelectSingleNode("//system.serviceModel/client");
            if (svcClientNode == null || !svcClientNode.HasChildNodes)
                return;

            _xmlWriter.WriteStartElement("client");

            foreach (var node in svcClientNode.ChildNodes)
            {
                var endPtNode = node as XmlElement;
                if (endPtNode == null)
                    continue;
                var addrAttr = endPtNode.Attributes["address"];
                if (addrAttr == null)
                    continue;

                if (!Uri.IsWellFormedUriString(addrAttr.Value, UriKind.RelativeOrAbsolute))
                    continue;

                AddEndpointNodeToTransform(
                    endPtNode.Attributes["address"] == null ? null : endPtNode.Attributes["address"].Value,
                    endPtNode.Attributes["binding"] == null ? null : endPtNode.Attributes["binding"].Value,
                    endPtNode.Attributes["bindingConfiguration"] == null ? null : endPtNode.Attributes["bindingConfiguration"].Value,
                    endPtNode.Attributes["contract"] == null ? null : endPtNode.Attributes["contract"].Value,
                    endPtNode.Attributes["name"] == null ? null : endPtNode.Attributes["name"].Value);

                if (!AreAnyRegexMatch(addrAttr.Value, regex2Values))
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
            var svcServicesNode = _configContent.SelectNodes("//system.serviceModel/services/service");
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

                if (nameAttr == null || string.IsNullOrWhiteSpace(nameAttr.Value))
                    continue;

                var hostNode =
                    _configContent.SelectSingleNode(string.Format(
                        "//system.serviceModel/services/service[@name='{0}']/host/baseAddresses", nameAttr.Value));

                _xmlWriter.WriteStartElement("service");

                _xmlWriter.WriteAttributeString("name", nameAttr.Value);

                if (behConfigAttr != null && !string.IsNullOrWhiteSpace(behConfigAttr.Value))
                    _xmlWriter.WriteAttributeString("behaviorConfiguration", behConfigAttr.Value);

                if (hostNode == null) //address attribute is being used
                {
                    var endPtNodes = _configContent.SelectNodes(string.Format(
                        "//system.serviceModel/services/service[@name='{0}']/endpoint", nameAttr.Value));
                    if (endPtNodes == null || endPtNodes.Count <= 0)
                    {
                        _xmlWriter.WriteEndElement();
                        continue;
                    }

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
                            continue; //next endpoint

                        var endPtAttr = endPtNode.Attributes["address"];

                        AddEndpointNodeToTransform(
                            endPtAttr == null ? null : endPtAttr.Value,
                            endPtNode.Attributes["binding"] == null ? null : endPtNode.Attributes["binding"].Value,
                            endPtNode.Attributes["bindingConfiguration"] == null
                                ? null
                                : endPtNode.Attributes["bindingConfiguration"].Value,
                            endPtNode.Attributes["contract"] == null ? null : endPtNode.Attributes["contract"].Value,
                            endPtNode.Attributes["name"] == null ? null : endPtNode.Attributes["name"].Value);

                        if (!AreAnyRegexMatch(endPtAttr.Value, regex2Values))
                        {
                            var addUri = new UriBuilder(endPtAttr.Value) { Host = "localhost" };

                            endPtAttr.Value = addUri.ToString();
                        }
                        else
                        {
                            AppropriateAllRegex(endPtAttr, regex2Values);
                        }
                    }
                }
                else //address is specified in the host/baseAddresses
                {
                    _xmlWriter.WriteStartElement("host");
                    _xmlWriter.WriteStartElement("baseAddresses");
                    foreach (var nodeV in hostNode.ChildNodes)
                    {
                        var bAddrNode = nodeV as XmlElement;
                        if (bAddrNode == null)
                            continue;
                        var baseAddrAttr = bAddrNode.Attributes["baseAddress"];
                        if (baseAddrAttr == null || string.IsNullOrEmpty(baseAddrAttr.Value) ||
                            !Uri.IsWellFormedUriString(baseAddrAttr.Value, UriKind.RelativeOrAbsolute))
                        {
                            _xmlWriter.WriteEndElement();
                            _xmlWriter.WriteEndElement();
                            continue;
                        }

                        _xmlWriter.WriteStartElement("add");
                        _xmlWriter.WriteAttributeString("baseAddress", baseAddrAttr.Value);

                        _xmlWriter.WriteAttributeString("Transform", "http://schemas.microsoft.com/XML-Document-Transform",
                            "SetAttributes");
                        _xmlWriter.WriteAttributeString("Locator", "http://schemas.microsoft.com/XML-Document-Transform",
                            "Match(baseAddress)");

                        if (!AreAnyRegexMatch(baseAddrAttr.Value, regex2Values))
                        {
                            var bAddrUri = new UriBuilder(baseAddrAttr.Value) { Host = "localhost" };
                            baseAddrAttr.Value = bAddrUri.ToString();
                        }
                        else
                        {
                            AppropriateAllRegex(baseAddrAttr, regex2Values);
                        }
                        _xmlWriter.WriteEndElement();
                    }
                    _xmlWriter.WriteEndElement();
                    _xmlWriter.WriteEndElement();
                }

                _xmlWriter.WriteEndElement();//end service node
            }

            _xmlWriter.WriteEndElement();//end serviceS node
        }

        internal void SplitLog4NetAppenderFileName(Hashtable regex2Values)
        {
            var appenderNodes = _configContent.SelectNodes("//log4net/appender");
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

                if (nameAttr == null || string.IsNullOrWhiteSpace(nameAttr.Value))
                    continue;
                if (typeAttr == null || string.IsNullOrWhiteSpace(typeAttr.Value))
                    continue;

                var appenderFileNode =
                    _configContent.SelectSingleNode(string.Format("//log4net/appender[@name='{0}']/file", nameAttr.Value));
                var appenderFileElem = appenderFileNode as XmlElement;

                if (appenderFileElem == null)
                    continue;
                var origFile = appenderFileElem.Attributes["value"];
                var origFileVal = origFile.Value;
                AppropriateAllRegex(origFile, regex2Values);

                _xmlWriter.WriteStartElement("appender");
                _xmlWriter.WriteAttributeString("name", nameAttr.Value);
                _xmlWriter.WriteAttributeString("type", typeAttr.Value);

                _xmlWriter.WriteAttributeString("Locator", "http://schemas.microsoft.com/XML-Document-Transform",
                    "Match(name)");

                _xmlWriter.WriteStartElement("file");
                _xmlWriter.WriteAttributeString("value", origFileVal);

                _xmlWriter.WriteAttributeString("Transform", "http://schemas.microsoft.com/XML-Document-Transform",
                    "Replace");
                _xmlWriter.WriteEndElement();//end file node

                _xmlWriter.WriteEndElement();//end appender node

            }

            _xmlWriter.WriteEndElement();//end log4net node
        }

        internal void AddEndpointNodeToTransform(string addr, string binding, string bindingConfig, string contract,
            string name)
        {
            if (string.IsNullOrWhiteSpace(addr))
                return;

            _xmlWriter.WriteStartElement("endpoint");
            _xmlWriter.WriteAttributeString("address", addr);
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

            _xmlWriter.WriteAttributeString("Transform", "http://schemas.microsoft.com/XML-Document-Transform",
                "SetAttributes");
            _xmlWriter.WriteAttributeString("Locator", "http://schemas.microsoft.com/XML-Document-Transform",
                "Match(name)");
            _xmlWriter.WriteEndElement();
        }

        internal void AddConnStringNodeToTransform(string name, string connStr)
        {
            _xmlWriter.WriteStartElement("add");
            _xmlWriter.WriteAttributeString("name", name);
            _xmlWriter.WriteAttributeString("connectionString", connStr);
            _xmlWriter.WriteAttributeString("Transform", "http://schemas.microsoft.com/XML-Document-Transform",
                "SetAttributes");
            _xmlWriter.WriteAttributeString("Locator", "http://schemas.microsoft.com/XML-Document-Transform",
                "Match(name)");
            _xmlWriter.WriteEndElement();
        }

        internal void AddAppSettingAddNodeToTransform(string key, string val)
        {
            _xmlWriter.WriteStartElement("add");
            _xmlWriter.WriteAttributeString("key", key);
            _xmlWriter.WriteAttributeString("value", val);
            _xmlWriter.WriteAttributeString("Transform", "http://schemas.microsoft.com/XML-Document-Transform",
                "SetAttributes");
            _xmlWriter.WriteAttributeString("Locator", "http://schemas.microsoft.com/XML-Document-Transform",
                "Match(key)");
            _xmlWriter.WriteEndElement();
            
        }

        internal static bool IsRegexMatch(string subject, string pattern, out string match)
        {
            match = null;
            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(pattern))
                return false;

            var regex = new Regex(pattern);

            if (!regex.IsMatch(subject))
                return false;

            var grp = regex.Matches(subject)[0];
            if (!grp.Success)
                return false;
            match = grp.Groups[0].Value;
            return true;

        }

        internal static bool AreAnyRegexMatch(string subject, Hashtable regex2Values)
        {
            if (string.IsNullOrEmpty(subject))
                return false;
            if (regex2Values == null || regex2Values.Count <= 0)
                return false;

            return regex2Values.Keys.Cast<string>().Any(x => Regex.IsMatch(subject, x));
        }

        internal static void AppropriateAllRegex(XmlAttribute attr, Hashtable regex2Values)
        {
            var newAppVal = attr.Value;
            if (string.IsNullOrEmpty(newAppVal))
                return;

            foreach (var regexKey in regex2Values.Keys.Cast<string>())
            {
                string matchedVal;

                var isRegexKeyMatch = IsRegexMatch(newAppVal, regexKey, out matchedVal);

                if (!isRegexKeyMatch)
                    continue;

                newAppVal = newAppVal.Replace(matchedVal, regex2Values[regexKey].ToString());
            }

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
