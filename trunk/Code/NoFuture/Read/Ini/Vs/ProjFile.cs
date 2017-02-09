using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using NoFuture.Exceptions;
using NoFuture.Util;

namespace NoFuture.Read.Vs
{
    /// <summary>
    /// A NoFuture type for performing the typical .NET developer operations on a 
    /// common VS project file.
    /// </summary>
    public class ProjFile : BaseXmlDoc
    {
        #region constants
        /// <summary>
        /// The xml namespace VS proj files are decorated with
        /// </summary>
        public const string DOT_NET_PROJ_XML_NS = "http://schemas.microsoft.com/developer/msbuild/2003";
        /// <summary>
        /// The xml namespace alias to be used in all Xpaths
        /// </summary>
        public const string NS = "vs";
        #endregion

        #region internal types
        /// <summary>
        /// A wrapper for containing the xml node coupled with 
        /// dll assembly name and path
        /// </summary>
        public class BinReference
        {
            private readonly ProjFile _projFile;
            public BinReference(ProjFile owningFile)
            {
                _projFile = owningFile;
            }

            public string SearchPath { get; set; }
            public string AssemblyFullName { get; set; }
            public string AssemblyName { get; set; }

            public XmlNode Node { get; set; }

            public string HintPath
            {
                get
                {
                    if (Node == null || !Node.HasChildNodes)
                        return null;
                    var hintPathNode = _projFile.FirstChildNamed(Node, "HintPath");
                    return hintPathNode?.InnerText;
                }
            }
        }

        #endregion

        #region fields
        private readonly XmlNamespaceManager _nsMgr;
        private readonly List<BinReference> _asmRefCache = new List<BinReference>();
        private bool _isChanged;
        #endregion

        #region ctors
        public ProjFile(string vsprojPath):base(vsprojPath)
        {
            _nsMgr = new XmlNamespaceManager(_xmlDocument.NameTable);
            _nsMgr.AddNamespace(NS, DOT_NET_PROJ_XML_NS);

            ValidateProjFile(vsprojPath);
        }
        #endregion

        #region properties
        public XmlDocument VsProjXml => _xmlDocument;
        public XmlNamespaceManager NsMgr => _nsMgr;
        public bool XmlContentChanged => _isChanged;

        public string AssemblyName
        {
            get
            {
                return GetInnerText("//{0}:PropertyGroup/{0}:AssemblyName");
            }
            set
            {
                SetInnerText("//{0}:PropertyGroup/{0}:AssemblyName", value);
            }
        }

        public string OutputType
        {
            get
            {
                return GetInnerText("//{0}:PropertyGroup/{0}:OutputType");
            }
            set
            {
                SetInnerText("//{0}:PropertyGroup/{0}:OutputType", value);
            }
        }

        public string RootNamespace
        {
            get
            {
                return GetInnerText("//{0}:PropertyGroup/{0}:RootNamespace");
            }
            set
            {
                SetInnerText("//{0}:PropertyGroup/{0}:RootNamespace", value);
            }

        }

        public string TargetFrameworkVersion
        {
            get
            {
                return GetInnerText("//{0}:PropertyGroup/{0}:TargetFrameworkVersion");
            }
            set
            {
                SetInnerText("//{0}:PropertyGroup/{0}:TargetFrameworkVersion", value);
            }

        }

        public string PreBuildEvent
        {
            get
            {
                return GetInnerText("//{0}:PropertyGroup/{0}:PreBuildEvent");
            }
            set
            {
                SetInnerText("//{0}:PropertyGroup/{0}:PreBuildEvent", value);
            }
        }

        public string PostBuildEvent
        {
            get
            {
                return GetInnerText("//{0}:PropertyGroup/{0}:PostBuildEvent");
            }
            set
            {
                SetInnerText("//{0}:PropertyGroup/{0}:PostBuildEvent", value);
            }
        }

        public string DebugBin
        {
            get
            {
                var pPath = GetInnerText("//{0}:PropertyGroup[contains(@Condition,'Debug|AnyCPU')]/{0}:OutputPath");
                return !string.IsNullOrWhiteSpace(pPath) ? Path.Combine(DirectoryName, pPath) : null;
            }
        }

        public string ProjectGuid
        {
            get
            {
                var guidPath = GetInnerText("//{0}:PropertyGroup/{0}:ProjectGuid");
                return !string.IsNullOrWhiteSpace(guidPath) ? guidPath.ToUpper() : null;
            }
        }

        /// <summary>
        /// The VS project type guid, this is only needed with regards to drafting .sln files
        /// </summary>
        public string VsProjectTypeGuid { get; set; }

        /// <summary>
        /// Gets a list of just the build configuration string in the typical form of 'Debug|AnyCPU'
        /// </summary>
        public string[] ProjectConfigurations
        {
            get
            {
                const string REGEX_PATTERN = @"\x27\x20\x3d\x3d\x20\x27([a-zA-Z0-9_\x7c]+)\x27";
                var buildConfigPropNodes = _xmlDocument.SelectNodes($"//{NS}:PropertyGroup", _nsMgr);
                if (buildConfigPropNodes == null)
                    return null;
                var configurations = new List<string>();
                foreach (var node in buildConfigPropNodes)
                {
                    var elem = node as XmlElement;
                    if (elem == null || !elem.HasAttributes || !elem.HasAttribute("Condition"))
                        continue;
                    var conditionValue = elem.GetAttribute("Condition");
                    string configOut;
                    if(Shared.RegexCatalog.IsRegexMatch(conditionValue, REGEX_PATTERN, out configOut, 1))
                        configurations.Add(configOut);
                }
                return configurations.ToArray();
            }
        }
        #endregion

        #region instance methods

        /// <summary>
        /// Attempts to replace all 'ProjectReference' with 'Reference'.
        /// </summary>
        /// <returns>The number of nodes replaced</returns>
        /// <remarks>
        /// Is expecting to find binary copies of each 'ProjectReference' in 
        /// the <see cref="DebugBin"/> folder.
        /// </remarks>
        public int TryReplaceToBinaryRef(bool useExactAsmNameMatch = false, string libDir = null)
        {
            var debugBin = DebugBin;
            if(string.IsNullOrWhiteSpace(debugBin) || !Directory.Exists(debugBin))
                throw new ItsDeadJim("Cannot locate a 'bin' folder from which to make .dll copies.");

            var projNode = _xmlDocument.SelectSingleNode($"/{NS}:Project", _nsMgr);
            if (projNode == null)
                return 0;

            //find all the proj refs
            var projRefs = _xmlDocument.SelectNodes($"//{NS}:ItemGroup/{NS}:ProjectReference", _nsMgr);
            if (projRefs == null)
                return 0;

            //setup a dir to save binary dll's to
            var destLibDir = libDir ?? Path.Combine(DirectoryName, "lib");
            if (!Directory.Exists(destLibDir))
                Directory.CreateDirectory(destLibDir);

            //get a hash of files-to-simple asm names
            var binDict = GetProjRefPath2Name();
            if (!binDict.Any())
                throw new ItsDeadJim($"There are no .dll files located in {debugBin}.");

            //set up containers to hold values found in per-proj-node
            var libBin = new List<Tuple<string,string>>();
            var cacheOfItemGroupParents = new List<XmlNode>();

            foreach (var projRefNode in projRefs)
            {
                var projRefElem = projRefNode as XmlElement;
                if (projRefElem == null || !projRefElem.HasAttributes)
                    continue;
                
                //cache up the parent nodes to drop empty ones later
                if(cacheOfItemGroupParents.All(x => !x.Equals(projRefElem.ParentNode)))
                    cacheOfItemGroupParents.Add(projRefElem.ParentNode);

                var includeAttr = projRefElem.Attributes["Include"];
                if (string.IsNullOrWhiteSpace(includeAttr?.Value))
                    continue;

                //use Include attr value to find the file-to-simple asm name entry
                var dllTuple = GetFileSysInfo2AsmNameEntry(includeAttr, binDict);

                //pass this off so its easier to map assembly-paths back to .[cs|vb|fs]proj files
                var projGuid =
                    (GetInnerText("//{0}:ItemGroup/{0}:ProjectReference[@Include='" + includeAttr.Value +
                                 "']/{0}:Project") ?? string.Empty).ToUpper();

                //copy dll from .\debug\bin to new .\lib dir
                var libFullName = Path.Combine(destLibDir, dllTuple.Item1.Name);
                File.Copy(dllTuple.Item1.FullName, libFullName, true);

                //add full filePath-to-projGuid to be worked later
                libBin.Add(new Tuple<string, string>(libFullName, projGuid));

                //remove this ProjectReference node
                projRefElem.ParentNode.RemoveChild(projRefElem);
            }

            //drop any ItemGroup who has had all child nodes dropped.
            foreach (var dd in cacheOfItemGroupParents.Where(x => !x.HasChildNodes))
                projNode.RemoveChild(dd);
            _isChanged = true;

            //now add each lib\dll path as a Reference node
            foreach (var dllLib in libBin)
            {
                if (!TryAddReferenceEntry(dllLib.Item1, dllLib.Item2, useExactAsmNameMatch, true))
                    throw new RahRowRagee($"Could not add a Reference node for {dllLib}");
            }
            return libBin.Count;
        }

        /// <summary>
        /// Attempts to add the assembly at <see cref="assemblyPath"/>
        /// to the project as a Reference node (NOTE, this differs from the 
        /// other type of reference called ProjectReference).
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="projGuid">
        /// Optional, will add in a child node Aliases.
        /// This is helpful if you want to later turn it back into a project reference.
        /// </param>
        /// <param name="useExactAsmNameMatch"></param>
        /// <param name="resolveToPartialPath">
        /// Will resolve the hint path to a relative path off the project's root 
        /// whenever the <see cref="assemblyPath"/> shares the <see cref="BaseXmlDoc.DirectoryName"/>
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// MsBuild style environment variables are handled (e.g. $(MY_ENV_VAR) )
        /// </remarks>
        public bool TryAddReferenceEntry(string assemblyPath, string projGuid = null, bool useExactAsmNameMatch = false, bool resolveToPartialPath = false)
        {
            if (string.IsNullOrWhiteSpace(assemblyPath))
                return false;

            //keep local copy to assign hintpath AS-IS 
            var tempPath = assemblyPath;
            if (resolveToPartialPath)
            {
                NfPath.TryGetRelPath(DirectoryName, ref tempPath);
                if (!Path.IsPathRooted(tempPath))
                    tempPath = ".\\" + tempPath;
            }

            //get wrapper of node plus path and asm name
            var projRef = GetBinRefByAsmPath(assemblyPath, useExactAsmNameMatch);

            if (string.IsNullOrWhiteSpace(projRef?.AssemblyFullName))
                return false;

            //get or create Reference xml element
            var xmlElem = projRef.Node as XmlElement ?? _xmlDocument.CreateElement("Reference", DOT_NET_PROJ_XML_NS);

            //assign Include attr value 
            var includeAttr = xmlElem.Attributes["Include"] ?? _xmlDocument.CreateAttribute("Include");
            includeAttr.Value = projRef.AssemblyFullName;
            if (!xmlElem.HasAttribute("Include"))
                xmlElem.Attributes.Append(includeAttr);

            var xpath2Ref = $"//{NS}:ItemGroup/{NS}:Reference[contains(@Include,'{projRef.AssemblyName}')]";

            //assign SpecificVersion inner text
            var specificVerNode = _xmlDocument.SelectSingleNode($"{xpath2Ref}/{NS}:SpecificVersion",_nsMgr);
            var specificVerElem = specificVerNode as XmlElement ??
                                  _xmlDocument.CreateElement("SpecificVersion", DOT_NET_PROJ_XML_NS);
            specificVerElem.InnerText = bool.FalseString;
            if(specificVerNode == null)
                xmlElem.AppendChild(specificVerElem);

            //assign HintPath inner text
            var hintPathNode = _xmlDocument.SelectSingleNode($"{xpath2Ref}/{NS}:HintPath",_nsMgr);
            var hintPathElem = hintPathNode as XmlElement ?? _xmlDocument.CreateElement("HintPath", DOT_NET_PROJ_XML_NS);
            hintPathElem.InnerText = tempPath;
            if (hintPathNode == null)
                xmlElem.AppendChild(hintPathElem);

            //add the project guid for debug\tracing
            if (!string.IsNullOrWhiteSpace(projGuid))
            {
                var guidComment = _xmlDocument.CreateComment(projGuid);
                projRef.Node.AppendChild(guidComment);
            }

            _isChanged = true;
            return _isChanged;
        }

        /// <summary>
        /// Reduces the project's build configurations to only those listed in 
        /// <see cref="buildConfigNames"/>.
        /// </summary>
        /// <param name="buildConfigNames">
        /// Optional, pass in null to have it default to the VS common "Debug|AnyCPU", "Release|AnyCPU"
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// Build config values should appear in the standard VS form as 'name|Cpu'
        /// </remarks>
        public int ReduceToOnlyBuildConfig(params string[] buildConfigNames)
        {
            buildConfigNames = buildConfigNames ?? new[] {"Debug|AnyCPU", "Release|AnyCPU"};
            var buildConfigPropNodes = _xmlDocument.SelectNodes($"//{NS}:PropertyGroup", _nsMgr);
            var counter = 0;
            //remove any that are not on the list
            if (buildConfigPropNodes != null)
            {
                foreach (var node in buildConfigPropNodes)
                {
                    var elem = node as XmlElement;
                    if (elem == null || !elem.HasAttributes || !elem.HasAttribute("Condition"))
                        continue;
                    var conditionValue = elem.GetAttribute("Condition");
                    if (buildConfigNames.All(x => !conditionValue.Contains(x)))
                    {
                        _xmlDocument.DocumentElement.RemoveChild(elem);
                        counter += 1;
                    }
                }
            }

            //add any on the list but not in xml
            foreach (var config in buildConfigNames)
            {
                if (HasBuildConfig(config))
                    continue;
                var outputPath = config.Contains("|") ? config.Split('|')[0] : config;

                var buildConfigNode = _xmlDocument.CreateElement("PropertyGroup",DOT_NET_PROJ_XML_NS);
                var conditionAttr = _xmlDocument.CreateAttribute("Condition");
                conditionAttr.Value = $" '$(Configuration)|$(Platform)' == '{config}' ";
                buildConfigNode.Attributes.Append(conditionAttr);

                var debugSymbolsNode = _xmlDocument.CreateElement("DebugSymbols", DOT_NET_PROJ_XML_NS);
                debugSymbolsNode.InnerText = "true";
                buildConfigNode.AppendChild(debugSymbolsNode);

                var debugTypeNode = _xmlDocument.CreateElement("DebugType", DOT_NET_PROJ_XML_NS);
                debugTypeNode.InnerText = "full";
                buildConfigNode.AppendChild(debugTypeNode);

                var optimizeNode = _xmlDocument.CreateElement("Optimize", DOT_NET_PROJ_XML_NS);
                optimizeNode.InnerText = "false";
                buildConfigNode.AppendChild(optimizeNode);

                var outputPathNode = _xmlDocument.CreateElement("OutputPath", DOT_NET_PROJ_XML_NS);
                outputPathNode.InnerText = $"bin\\{outputPath}";
                buildConfigNode.AppendChild(outputPathNode);

                var defineConstantsNode = _xmlDocument.CreateElement("DefineConstants", DOT_NET_PROJ_XML_NS);
                defineConstantsNode.InnerText = "DEBUG;TRACE";
                buildConfigNode.AppendChild(defineConstantsNode);

                var errorReportNode = _xmlDocument.CreateElement("ErrorReport", DOT_NET_PROJ_XML_NS);
                errorReportNode.InnerText = "prompt";
                buildConfigNode.AppendChild(errorReportNode);

                var warningLevelNode = _xmlDocument.CreateElement("WarningLevel", DOT_NET_PROJ_XML_NS);
                warningLevelNode.InnerText = "4";
                buildConfigNode.AppendChild(warningLevelNode);

                var firstDocChildNode = _xmlDocument.DocumentElement.FirstChild;
                _xmlDocument.InsertAfter(buildConfigNode, firstDocChildNode);
            }
            return counter;
        }

        /// <summary>
        /// Add the <see cref="srcCodeFile"/> to the MsBuild xml (project file)
        /// </summary>
        /// <param name="srcCodeFile"></param>
        /// <returns></returns>
        /// <remarks>
        /// Will add the '.designer.[cs|vb]' and '.[cs|vb]' files, when they exist,
        /// for any '.as[ax|cx|mx|px]' 
        /// </remarks>
        public bool TryAddSrcCodeFile(string srcCodeFile)
        {
            const string DEPENDENT_UPON = "DependentUpon";
            const string INCLUDE = "Include";
            const string SUB_TYPE = "SubType";
            if (string.IsNullOrWhiteSpace(srcCodeFile))
                return false;

            //filter to only those that exist
            var files = SingleFileToManifold(srcCodeFile);

            if (files.Count <= 0)
                return false;

            //reduce actuals to relative path
            ToRelativePaths(files);

            //split between compile items and content items
            var compileItems = files.Where(IsSrcCodeFile).ToList();
            var contentItems = files.Where(IsAspFile).ToList();

            //get parent to attach new nodes to
            var parentNode = GetCompileItemParent();
            if (parentNode == null)
            {
                return false;
            }

            //add each valid compile item
            foreach (var compItem in compileItems)
            {
                var newCompileItem = GetSingleCompileItemNode(compItem) ?? _xmlDocument.CreateElement("Compile", DOT_NET_PROJ_XML_NS);

                if (newCompileItem.Attributes != null)
                {
                    var includeAttr = HasExistingIncludeAttr(newCompileItem)
                        ? newCompileItem.Attributes[INCLUDE]
                        : _xmlDocument.CreateAttribute(INCLUDE);

                    includeAttr.Value = compItem;
                    if(!HasExistingIncludeAttr(newCompileItem))
                        newCompileItem.Attributes.Append(includeAttr);
                }

                //asp files have some additional child nodes
                if (IsAspFile(Path.GetFileNameWithoutExtension(compItem)))
                {
                    var dependentUponNode = FirstChildNamed(newCompileItem, DEPENDENT_UPON) ??
                                            _xmlDocument.CreateElement(DEPENDENT_UPON, DOT_NET_PROJ_XML_NS);

                    dependentUponNode.InnerText = Path.GetFileNameWithoutExtension(compItem);

                    if (!HasChildNodeNamed(newCompileItem, DEPENDENT_UPON))
                        newCompileItem.AppendChild(dependentUponNode);

                    if (!compItem.Contains(".designer"))
                    {
                        var subTypeNode = FirstChildNamed(newCompileItem, SUB_TYPE) ??
                                          _xmlDocument.CreateElement(SUB_TYPE, DOT_NET_PROJ_XML_NS);

                        subTypeNode.InnerText = "ASPXCodeBehind";

                        if (!HasChildNodeNamed(newCompileItem, SUB_TYPE))
                            newCompileItem.AppendChild(subTypeNode);
                    }
                }

                if (!IsExistingCompileItem(compItem))
                {
                    var lastCompileItemNode = GetLastCompileItem();
                    if (lastCompileItemNode != null)
                    {
                        parentNode.InsertAfter(newCompileItem, lastCompileItemNode);
                    }
                    else
                    {
                        parentNode.AppendChild(newCompileItem);
                    }
                    
                }
                    
                _isChanged = true;
            }

            //add asp as content
            if (contentItems.Count <= 0)
            {
                return _isChanged;
            }
            parentNode = GetContentItemParent();
            if (parentNode == null)
            {
                return _isChanged;
            }

            foreach (var contentItem in contentItems)
            {
                var newContentItemNode = GetSingleContentItemNode(contentItem) ??
                                         _xmlDocument.CreateElement("Content", DOT_NET_PROJ_XML_NS);

                if (newContentItemNode.Attributes != null)
                {
                    var includeAttr = HasExistingIncludeAttr(newContentItemNode)
                        ? newContentItemNode.Attributes[INCLUDE]
                        : _xmlDocument.CreateAttribute(INCLUDE);

                    includeAttr.Value = contentItem;
                    if(!HasExistingIncludeAttr(newContentItemNode))
                        newContentItemNode.Attributes.Append(includeAttr);
                }

                if (!IsExistingContentItem(contentItem))
                {
                    var lastConentItemNode = GetLastContentItem();
                    if (lastConentItemNode != null)
                    {
                        parentNode.InsertAfter(newContentItemNode, lastConentItemNode);
                    }
                    else
                    {
                        parentNode.AppendChild(newContentItemNode);
                    }
                }

                _isChanged = true;
            }
            return _isChanged;
        }

        /// <summary>
        /// Removes the <see cref="srcCodeFile"/> from the MsBuild xml (project file)
        /// </summary>
        /// <param name="srcCodeFile"></param>
        /// <param name="relativePathsOut">pass in a ref to get the paths of the items removed.</param>
        /// <returns></returns>
        /// <remarks>
        /// Will remove the '.designer.[cs|vb]' and '.[cs|vb]' files, when they exist,
        /// for any '.as[ax|cx|mx|px]' 
        /// </remarks>
        public bool TryRemoveSrcCodeFile(string srcCodeFile, List<string> relativePathsOut)
        {
            if(relativePathsOut == null)
                relativePathsOut = new List<string>();

            if (string.IsNullOrWhiteSpace(srcCodeFile))
                return false;

            //filter to only those that exist
            var files = SingleFileToManifold(srcCodeFile);

            if (files.Count <= 0)
                return false;

            //reduce actuals to relative path
            ToRelativePaths(files);

            //split between compile items and content items
            var compileItems = files.Where(x => IsSrcCodeFile(x) && IsExistingCompileItem(x)).ToList();
            var contentItems = files.Where(x => IsAspFile(x) && IsExistingContentItem(x)).ToList();

            foreach (var compileItem in compileItems)
            {
                var itemNode = GetSingleCompileItemNode(compileItem);
                if (itemNode == null)
                    continue;

                var parentNode = itemNode.ParentNode;
                if (parentNode == null)
                    break;

                parentNode.RemoveChild(itemNode);
                relativePathsOut.Add(compileItem);
                _isChanged = true;
            }

            foreach (var contentItem in contentItems)
            {
                var itemNode = GetSingleContentItemNode(contentItem);
                if (itemNode == null)
                    continue;

                var parentNode = itemNode.ParentNode;
                if (parentNode == null)
                    break;

                parentNode.RemoveChild(itemNode);
                relativePathsOut.Add(contentItem);
                _isChanged = true;
            }

            return _isChanged;
        }

        /// <summary>
        /// Gets this <see cref="assemblyPath"/> as a property reference.
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="useExactAsmNameMatch">
        /// Optional, specify that the Assembly Names must be exact matches
        /// </param>
        /// <returns></returns>
        public BinReference GetBinRefByAsmPath(string assemblyPath, bool useExactAsmNameMatch = false)
        {

            if (string.IsNullOrWhiteSpace(assemblyPath))
                return null;

            //if we looked this up before then return it again to save time
            if (_asmRefCache.Any(x => x.SearchPath == assemblyPath))
                return _asmRefCache.First(x => x.SearchPath == assemblyPath);

            _asmRefCache.Add(new BinReference(this){SearchPath = assemblyPath});

            var cache = _asmRefCache.First(x => x.SearchPath == assemblyPath);

            //resolve vs style env vars 
            var tempPath = assemblyPath;
            if (tempPath.Contains("$("))
            {
                if (!NfPath.TryResolveEnvVar(tempPath, ref assemblyPath))
                {
                    return null;
                }
            }

            //test full file path for existence
            if (!File.Exists(assemblyPath))
                return null;

            var assemblyName = System.Reflection.AssemblyName.GetAssemblyName(assemblyPath);

            XmlNode refNode = null;
            var xpathString = $"//{NS}:ItemGroup/{NS}:Reference[contains(@Include,'{assemblyName.Name}')]";
            if (useExactAsmNameMatch)
            {
                //find all nodes which contain the assembly's simple name
                var closeMatches = _xmlDocument.SelectNodes(xpathString, _nsMgr);

                if (closeMatches != null)
                {
                    foreach (var cm in closeMatches)
                    {
                        var cmElem = cm as XmlElement;
                        var includeAttr = cmElem?.Attributes["Include"];
                        if (string.IsNullOrWhiteSpace(includeAttr?.Value))
                            continue;
                        var includeAsmName = new AssemblyName(includeAttr.Value);

                        //select the one whose full assembly names are the same.
                        if (System.Reflection.AssemblyName.ReferenceMatchesDefinition(assemblyName, includeAsmName))
                        {
                            refNode = cmElem;
                            break;
                        }
                    }
                }
            }
            else
            {
                //take the first node that contains the assembly's simple name
                refNode = _xmlDocument.SelectSingleNode(xpathString, _nsMgr);
            }

            //finish off the BinRef wrapper with extra info
            cache.AssemblyFullName = assemblyName.FullName;
            cache.AssemblyName = assemblyName.Name;

            //and the the actual xml node if available
            if (refNode != null)
                cache.Node = refNode;

            return cache;
        }

        public override string GetInnerText(string xpath)
        {
            var singleNode = _xmlDocument.SelectSingleNode(string.Format(xpath, NS), NsMgr);
            return singleNode?.InnerText;
        }

        public override void SetInnerText(string xpath, string theValue)
        {
            var singleNode = _xmlDocument.SelectSingleNode(string.Format(xpath, NS), NsMgr);
            if (singleNode == null)
                return;
            singleNode.InnerText = theValue;
        }

        #endregion

        #region internal instance methods

        /// <summary>
        /// Dependent function to get back a single entry from the output of <see cref="GetProjRefPath2Name"/>
        /// </summary>
        /// <param name="includeAttr"></param>
        /// <param name="binDict"></param>
        /// <returns></returns>
        protected Tuple<FileSystemInfo, string> GetFileSysInfo2AsmNameEntry(XmlAttribute includeAttr, List<Tuple<FileSystemInfo, string>> binDict)
        {
            //confirm the path in the Include attr points to an actual file on the drive
            var projPath = Path.Combine(DirectoryName, includeAttr.Value);
            if (!File.Exists(projPath))
                throw new RahRowRagee($"The ProjectReference to '{projPath}' does not exisit.");

            var simpleAsmName =
                GetInnerText("//{0}:ItemGroup/{0}:ProjectReference[@Include='" + includeAttr.Value + "']/{0}:Name");

            //deal with odd name appearing like "Some.Asm.Name %28SomeDir\SomeOtherDir%29
            if (simpleAsmName.Contains(" "))
            {
                simpleAsmName = simpleAsmName.Split(' ')[0];
            }

            //do an easy search
            var dllTuple = binDict.FirstOrDefault(x => x.Item2 == simpleAsmName);

            if (dllTuple != null)
                return dllTuple;

            //do a hard search
            var nfProjFile = new ProjFile(projPath);
            simpleAsmName = nfProjFile.AssemblyName;
            dllTuple = binDict.FirstOrDefault(x => x.Item2 == simpleAsmName);
            if (dllTuple == null)
                throw new RahRowRagee(
                    $"Could not find a matching .dll for the ProjectRefernce with the Name of '{simpleAsmName}'");
            return dllTuple;
        }

        /// <summary>
        /// A dependent funtion to find a Reference node which was written <see cref="TryAddReferenceEntry"/>
        /// when its the case that a value of 'projGuid' was passed in and written 
        /// as an xml comment at the bottom of the node.
        /// </summary>
        /// <param name="guidValue"></param>
        /// <returns></returns>
        protected internal XmlNode GetRefNodeByGuidComment(string guidValue)
        {
            if (string.IsNullOrWhiteSpace(guidValue))
                return null;
            var rawGuidValue = guidValue.Replace("{", string.Empty).Replace("}", string.Empty);
            Guid guid;
            if (!Guid.TryParse(rawGuidValue, out guid))
                throw new RahRowRagee($"The value {rawGuidValue} could not be parsed to a Guid.");
            rawGuidValue = rawGuidValue.ToUpper();
            var allReferenceNodes = _xmlDocument.SelectNodes($"//{NS}:Reference", _nsMgr);
            if (allReferenceNodes == null)
                return null;
            foreach (var nodeListItem in allReferenceNodes)
            {
                var node = nodeListItem as XmlNode;
                if (node == null || !node.HasChildNodes)
                    continue;
                foreach (var childNodeItem in node.ChildNodes)
                {
                    var xmlComment = childNodeItem as XmlComment;
                    if (string.IsNullOrWhiteSpace(xmlComment?.InnerText))
                        continue;
                    if (xmlComment.InnerText.ToUpper().Contains(rawGuidValue))
                        return node;
                }

            }
            return null;
        }

        /// <summary>
        /// Searches the <see cref="DebugBin"/> getting a mapping of file info to 
        /// simple assmebly names.
        /// </summary>
        /// <returns></returns>
        protected internal List<Tuple<FileSystemInfo, string>> GetProjRefPath2Name()
        {
            var binDict = new List<Tuple<FileSystemInfo, string>>();
            var directoryInfo = new DirectoryInfo(DebugBin);
            foreach (var dllFile in directoryInfo.EnumerateFileSystemInfos("*.dll", SearchOption.TopDirectoryOnly))
            {
                var asmName = System.Reflection.AssemblyName.GetAssemblyName(dllFile.FullName);
                binDict.Add(new Tuple<FileSystemInfo, string>(dllFile, asmName.Name));
            }
            return binDict;
        }

        internal bool IsExistingCompileItem(string fl) { return GetSingleCompileItemNode(fl) != null; }

        internal bool IsExistingContentItem(string fl) { return GetSingleContentItemNode(fl) != null; }

        internal XmlNode GetSingleCompileItemNode(string fl)
        {
            return _xmlDocument.SelectSingleNode($"//{NS}:ItemGroup/{NS}:Compile[@Include='{fl}']", _nsMgr);
        }

        internal List<XmlElement> GetListCompileItemNodes(string fl)
        {
            var dlk = _xmlDocument.SelectNodes($"//{NS}:ItemGroup/{NS}:Compile[contains(@Include,'{fl}')]", _nsMgr);
            return dlk?.Cast<XmlElement>().ToList();
        }

        internal XmlNode GetSingleContentItemNode(string fl)
        {
            return _xmlDocument.SelectSingleNode($"//{NS}:ItemGroup/{NS}:Content[@Include='{fl}']", _nsMgr);
        }

        internal List<XmlElement> GetListContentItemNodes(string fl)
        {
            var dlk = _xmlDocument.SelectNodes($"//{NS}:ItemGroup/{NS}:Content[contains(@Include,'{fl}')]", _nsMgr);
            return dlk?.Cast<XmlElement>().ToList();
        }

        internal bool HasExistingIncludeAttr(XmlNode n)
        {
            return n?.Attributes?["Include"] != null;
        }

        internal XmlElement FirstChildNamed(XmlNode n, string childNodeName)
        {
            return !HasChildNodeNamed(n, childNodeName)
                ? null
                : n.ChildNodes.Cast<XmlElement>().First(x => x.Name == childNodeName);
        }

        internal bool HasChildNodeNamed(XmlNode n, string childNodeName)
        {
            return n != null && n.HasChildNodes && n.ChildNodes.Cast<XmlElement>().Any(x => x.Name == childNodeName);
        }

        internal XmlNode GetLastCompileItem()
        {
            return _xmlDocument.SelectSingleNode($"//{NS}:ItemGroup/{NS}:Compile[last()]", _nsMgr);
        }

        internal XmlNode GetLastContentItem()
        {
            return _xmlDocument.SelectSingleNode($"//{NS}:ItemGroup/{NS}:Content[last()]", _nsMgr);
        }

        internal XmlNode GetLastReferenceNode()
        {
            return _xmlDocument.SelectSingleNode($"//{NS}:ItemGroup/{NS}:Reference[last()]", _nsMgr);
        }

        internal XmlNode GetContentItemParent()
        {
            var lastItem = _xmlDocument.SelectSingleNode($"//{NS}:ItemGroup/{NS}:Content[last()]", _nsMgr);
            if (lastItem?.ParentNode != null)
                return lastItem.ParentNode;

            var groups = _xmlDocument.SelectNodes($"//{NS}:ItemGroup", _nsMgr);
            return groups == null ? NewItemGroup() : groups.Cast<XmlElement>().FirstOrDefault(x => !x.HasChildNodes);
        }

        internal XmlNode GetCompileItemParent()
        {
            var lastItem = _xmlDocument.SelectSingleNode($"//{NS}:ItemGroup/{NS}:Compile[last()]", _nsMgr);
            if (lastItem?.ParentNode != null)
                return lastItem.ParentNode;

            var groups = _xmlDocument.SelectNodes($"//{NS}:ItemGroup", _nsMgr);
            return groups == null ? NewItemGroup() : groups.Cast<XmlElement>().FirstOrDefault(x => !x.HasChildNodes);
        }

        internal XmlElement NewItemGroup()
        {
            var newGroup = _xmlDocument.CreateElement("ItemGroup", DOT_NET_PROJ_XML_NS);
            var rootNode = _xmlDocument.SelectSingleNode("/");
            rootNode?.AppendChild(newGroup);
            return newGroup;
        }

        internal bool IsAspFile(string s)
        {
            s = s.Replace(".designer", string.Empty);
            return !string.IsNullOrWhiteSpace(s) &&
                    new[] { ".asax", ".ascx", ".asmx", ".aspx" }.Contains(Path.GetExtension(s));
        }

        internal bool IsSrcCodeFile(string x)
        {
            return !string.IsNullOrWhiteSpace(x) && new[] { ".vb", ".cs" }.Contains(Path.GetExtension(x));
        }

        internal List<string> SingleFileToManifold(string srcCodeFile)
        {
            if(srcCodeFile == null)
                throw new ArgumentNullException(nameof(srcCodeFile));
            var files = new List<string>();
            
            //when srcCodeFile is not currently present in the xml
            var allSuchNodes = GetListCompileItemNodes(srcCodeFile);
            if (allSuchNodes == null || allSuchNodes.Count <= 0)
            {
                if (!Path.IsPathRooted(srcCodeFile))
                    srcCodeFile = Path.Combine(DirectoryName, srcCodeFile);

                files.Add(srcCodeFile);

                if (!IsAspFile(srcCodeFile))
                    return files;

                foreach (var ext in (new[] {".vb", ".cs"}))
                {
                    files.Add(srcCodeFile + ".designer" + ext);
                    files.Add(srcCodeFile + ext);
                }

                return files.Where(File.Exists).Distinct().ToList();
            }

            allSuchNodes.AddRange(GetListContentItemNodes(srcCodeFile));

            files.AddRange(
                allSuchNodes.Where(x => x != null && x.Attributes.Count > 0 && x.Attributes["Include"] != null)
                    .Select(matchedFile => matchedFile.Attributes["Include"].Value));

            //add any asp releated files as needed
            for (var i = 0; i<files.Count; i++)
            {
                if (!IsAspFile(files[i])) continue;

                if (!files.Contains(files[i] + ".designer.cs"))
                    files.Add(files[i] + ".designer.cs");
                if (!files.Contains(files[i] + ".cs"))
                    files.Add(files[i] + ".cs");

                if (!files.Contains(files[i] + ".designer.vb"))
                    files.Add(files[i] + ".designer.vb");
                if (!files.Contains(files[i] + ".vb"))
                    files.Add(files[i] + ".vb");
            }
            for (var i = 0; i < files.Count; i++)
            {
                var fl = files[i];
                if (!Path.IsPathRooted(fl))
                    files[i] = Path.Combine(DirectoryName, fl);
            }

            //filter to only those that exist and are distinct
            files = files.Where(File.Exists).Distinct().ToList();
            return files;
        }

        internal void ToRelativePaths(List<string> files)
        {
            //reduce actuals to relative path
            for (var i = 0; i < files.Count; i++)
            {
                var fli = files[i];
                if (Util.NfPath.TryGetRelPath(DirectoryName, ref fli))
                {
                    files[i] = fli;
                }
            }
        }

        internal List<string> ToExistingCompileNodes(List<string> files)
        {
            return files.Where(x => IsSrcCodeFile(x) && IsExistingCompileItem(x)).ToList();
        }

        protected override XmlAttribute GetAttribute(string xpath, string attrName)
        {
            if (string.IsNullOrWhiteSpace(xpath) || string.IsNullOrWhiteSpace(attrName))
                return null;

            xpath = BaseXmlDoc.SpliceInXmlNs(xpath, NS);

            var node = _xmlDocument.SelectSingleNode(xpath, NsMgr);

            var elem = node as XmlElement;
            return elem?.Attributes[attrName];
        }

        protected internal bool HasBuildConfig(string buildConfigName)
        {
            var buildNode = _xmlDocument.SelectSingleNode($"//{NS}:PropertyGroup[contains(@Condition,'{buildConfigName}')]", _nsMgr);
            return buildNode != null;
        }
        #endregion

        #region static methods

        internal static void ValidateProjFile(string vsprojPath)
        {
            if (string.IsNullOrWhiteSpace(vsprojPath) || !File.Exists(vsprojPath))
                throw new ItsDeadJim($"Cannot find the Vs Project file '{vsprojPath}'.");

            if (!Path.HasExtension(vsprojPath))
                throw new ItsDeadJim("Specify a specific [cs|vb|fs]proj file, not a whole directory");

            if (!Regex.IsMatch(Path.GetExtension(vsprojPath), @"\.(vb|cs|fs)proj"))
                throw new ItsDeadJim($"The Extension '{Path.GetExtension(vsprojPath)}' was unexpected");
        }
        #endregion
    }
}
