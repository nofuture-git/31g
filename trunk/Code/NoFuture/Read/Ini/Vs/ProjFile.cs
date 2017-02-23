using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
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
        protected internal class BinReference
        {
            private readonly ProjFile _projFile;
            private string _hintPath;
            public BinReference(ProjFile owningFile)
            {
                _projFile = owningFile;
            }

            public string SearchPath { get; set; }
            public Tuple<FileSystemInfo, AssemblyName> DllOnDisk { get; set; }

            public XmlNode Node { get; set; }

            public string HintPath
            {
                get
                {
                    if(!string.IsNullOrWhiteSpace(_hintPath))
                        return _hintPath;
                    if (Node == null || !Node.HasChildNodes)
                        return null;
                    var hintPathNode = _projFile.FirstChildNamed(Node, "HintPath");
                    _hintPath =  hintPathNode?.InnerText;
                    return _hintPath;
                }
                set
                {
                    _hintPath = value;
                    if (Node == null || !Node.HasChildNodes)
                        return;
                    var hintPathNode = _projFile.FirstChildNamed(Node, "HintPath");
                    if (hintPathNode == null)
                        return;

                    hintPathNode.InnerText = _hintPath;
                }
            }
        }

        protected internal class ProjectReference
        {
            private readonly ProjFile _myProjFile;
            public ProjectReference(ProjFile myProjFile)
            {
                _myProjFile = myProjFile;
            }

            public Tuple<FileSystemInfo, AssemblyName> DllOnDisk { get; set; }
            public string ProjectGuid { get; set; }
            public string ProjectName { get; set; }
            public XmlNode Node { get; set; }
            public string ProjectPath { get; set; }
        }
        #endregion

        #region fields
        private readonly XmlNamespaceManager _nsMgr;
        private readonly List<BinReference> _asmRefCache = new List<BinReference>();
        private readonly List<ProjectReference> _projRefCache = new List<ProjectReference>();
        private bool _isChanged;
        private readonly List<Tuple<FileSystemInfo, AssemblyName>> _debugBinFsi2AsmName;
        private string _projTypeGuids;
        #endregion

        #region ctors
        public ProjFile(string vsprojPath):base(vsprojPath)
        {
            _nsMgr = new XmlNamespaceManager(_xmlDocument.NameTable);
            _nsMgr.AddNamespace(NS, DOT_NET_PROJ_XML_NS);

            ValidateProjFile(vsprojPath);

            _debugBinFsi2AsmName = new List<Tuple<FileSystemInfo, AssemblyName>>();
            if (string.IsNullOrWhiteSpace(DebugBin) || !Directory.Exists(DebugBin))
                return;
            var directoryInfo = new DirectoryInfo(DebugBin);
            foreach (var dllFile in directoryInfo.EnumerateFileSystemInfos("*.dll", SearchOption.TopDirectoryOnly))
            {
                var asmName = System.Reflection.AssemblyName.GetAssemblyName(dllFile.FullName);
                _debugBinFsi2AsmName.Add(new Tuple<FileSystemInfo, AssemblyName>(dllFile, asmName));
            }
        }
        #endregion

        #region properties
        public XmlDocument VsProjXml => _xmlDocument;
        public XmlNamespaceManager NsMgr => _nsMgr;
        public bool XmlContentChanged => _isChanged;
        public List<Tuple<FileSystemInfo, AssemblyName>> DebugBinFsi2AsmName => _debugBinFsi2AsmName;

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
        /// The VS project type guid, these are often not present in project files
        /// and mostly used by .sln files.  See the markup on <see cref="SlnFile.VisualStudioProjTypeGuids"/>
        /// </summary>
        public string VsProjectTypeGuids
        {
            get
            {
                _projTypeGuids = GetInnerText("//{0}:PropertyGroup/{0}:ProjectTypeGuids") ?? _projTypeGuids;
                //keep this value to only one proj type guid
                if (_projTypeGuids != null && _projTypeGuids.Length > ("{" + Guid.Empty + "}").Length)
                {
                    var ext = Path.GetExtension(FileName) ?? ".csproj";
                    _projTypeGuids = SlnFile.VisualStudioProjTypeGuids[ext];
                    SetInnerText("//{0}:PropertyGroup/{0}:ProjectTypeGuids", _projTypeGuids);

                }
                return _projTypeGuids;
            }
            set
            {
                SetInnerText("//{0}:PropertyGroup/{0}:ProjectTypeGuids", value);
                _projTypeGuids = value;
            }
        }

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

        public string DestLibDirectory { get; set; }
        #endregion

        #region instance methods

        /// <summary>
        /// Copies all Reference nodes in this instance over to <see cref="targetProjFile"/>
        /// </summary>
        /// <param name="targetProjFile"></param>
        /// <returns></returns>
        public int CopyAllReferenceNodesTo(ProjFile targetProjFile)
        {
            if (targetProjFile == null)
                return 0;
            var allMyReferenceNodes = _xmlDocument.SelectNodes($"//{NS}:ItemGroup/{NS}:Reference", _nsMgr);
            if (allMyReferenceNodes == null)
                return 0;
            var counter = 0;
            foreach (var projRefNode in allMyReferenceNodes)
            {
                var projRefElem = projRefNode as XmlElement;
                if (projRefElem == null)
                    continue;
                //is it a simple GAC reference
                if (!projRefElem.HasChildNodes && !string.IsNullOrWhiteSpace(projRefElem.Attributes["Include"]?.Value))
                {
                    targetProjFile.AddGacReferenceNode(projRefElem.Attributes["Include"].Value);
                    continue;
                }
                var hintPathNode = projRefElem.FirstChildNamed("HintPath");
                if (string.IsNullOrWhiteSpace(hintPathNode?.InnerText))
                    continue;
                //combine my directory to file, target will resolve it to a relative path
                var fullPath = Path.Combine(DirectoryName, hintPathNode.InnerText);
                if (targetProjFile.AddReferenceNode(fullPath, null, true, true))
                {
                    counter += 1;
                }
            }
            targetProjFile.Save();
            return counter;
        }

        /// <summary>
        /// Copies all ProjectReference nodes from this instance over to <see cref="targetProjFile"/>
        /// </summary>
        /// <param name="targetProjFile"></param>
        /// <returns></returns>
        public int CopyAllProjectReferenceNodesTo(ProjFile targetProjFile)
        {
            if (targetProjFile == null)
                return 0;
            var allMyProjRefNodes = _xmlDocument.SelectNodes($"//{NS}:ItemGroup/{NS}:ProjectReference", _nsMgr);
            if (allMyProjRefNodes == null)
                return 0;
            var counter = 0;
            foreach (var projRefNode in allMyProjRefNodes)
            {
                var projRefElem = projRefNode as XmlElement;
                if (projRefElem == null || !projRefElem.HasAttributes)
                    continue;
                var projPathAttr = projRefElem.Attributes["Include"];
                if (string.IsNullOrWhiteSpace(projPathAttr?.Value))
                    continue;
                //combine my directory to file, target will resolve it to a relative path
                var fullPath = Path.Combine(DirectoryName, projPathAttr.Value);
                if (targetProjFile.AddProjectReferenceNode(fullPath))
                {
                    counter += 1;
                }
            }
            targetProjFile.Save();
            return counter;
        }

        /// <summary>
        /// Locates a compiled assembly each of the ProjectReference nodes and
        /// copies it into <see cref="destDir"/>, defaults to <see cref="DestLibDirectory"/>
        /// </summary>
        /// <returns></returns>
        public int CopyAllProjRefDllTo(string destDir = null)
        {
            //setup a dir to save binary dll's to
            destDir = destDir ?? DestLibDirectory;
            destDir = string.IsNullOrWhiteSpace(destDir) ? Path.Combine(DirectoryName, "lib") : destDir;
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            var projNode = _xmlDocument.SelectSingleNode($"/{NS}:Project", _nsMgr);
            if (projNode == null)
                return 0;

            //find all the proj refs
            var projRefs = _xmlDocument.SelectNodes($"//{NS}:ItemGroup/{NS}:ProjectReference", _nsMgr);
            var counter = 0;
            
            if (projRefs == null)
                return counter;
            foreach (var projRefNode in projRefs)
            {
                var projRefElem = projRefNode as XmlElement;
                if (projRefElem == null || !projRefElem.HasAttributes)
                    continue;

                var includeAttr = projRefElem.Attributes["Include"];
                if (string.IsNullOrWhiteSpace(includeAttr?.Value))
                    continue;

                var projRef = GetProjRefByProjPath(includeAttr.Value);
                if (projRef?.DllOnDisk?.Item1 == null)
                    continue;

                //copy dll from .\debug\bin to new .\lib dir
                var libDestName = Path.Combine(destDir, projRef.DllOnDisk.Item1.Name);
                var libSrcName = projRef.DllOnDisk.Item1.FullName;
                NfPath.CopyFileIfNewer(libSrcName, libDestName);
                counter += 1;
            }
            return counter;
        }

        /// <summary>
        /// Locates a compiled assembly each of the Reference nodes and, using the HintPath,
        /// copies it into <see cref="destDir"/>, defaults to <see cref="DestLibDirectory"/>
        /// </summary>
        /// <returns></returns>
        public int CopyAllBinRefDllTo(string destDir = null)
        {
            //setup a dir to save binary dll's to
            destDir = destDir ?? DestLibDirectory;
            destDir = string.IsNullOrWhiteSpace(destDir) ? Path.Combine(DirectoryName, "lib") : destDir;

            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            Action<BinReference> myAction = reference =>
            {
                if (string.IsNullOrWhiteSpace(reference?.HintPath))
                    return;
                var libDestName = Path.Combine(destDir, Path.GetFileName(reference.HintPath));
                var libSrcName = Path.Combine(DirectoryName, reference.HintPath);
                NfPath.CopyFileIfNewer(libSrcName, libDestName);
            };
            return IterateBinReferences(myAction);
        }

        /// <summary>
        /// Attempts to replace all 'ProjectReference' with 'Reference'.
        /// </summary>
        /// <returns>The number of nodes replaced</returns>
        /// <remarks>
        /// Is expecting to find binary copies of each 'ProjectReference' in 
        /// the <see cref="DebugBin"/> folder.
        /// </remarks>
        public int SwapAllProjRef2BinRef(bool useExactAsmNameMatch = false)
        {
            var projNode = _xmlDocument.SelectSingleNode($"/{NS}:Project", _nsMgr);
            if (projNode == null)
                return 0;

            CopyAllProjRefDllTo();

            //find all the proj refs
            var projRefs = _xmlDocument.SelectNodes($"//{NS}:ItemGroup/{NS}:ProjectReference", _nsMgr);
            if (projRefs == null)
                return 0;

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

                var projRef = GetProjRefByProjPath(includeAttr.Value);
                if (projRef?.DllOnDisk?.Item1 == null)
                    continue;

                //copy dll from .\debug\bin to new .\lib dir
                var libFullName = Path.Combine(DestLibDirectory, projRef.DllOnDisk.Item1.Name);

                //add full filePath-to-projGuid to be worked later
                libBin.Add(new Tuple<string, string>(libFullName, projRef.ProjectGuid));

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
                if (!AddReferenceNode(dllLib.Item1, dllLib.Item2, useExactAsmNameMatch, true))
                    throw new RahRowRagee($"Could not add a Reference node for {dllLib}");
            }
            return libBin.Count;
        }

        /// <summary>
        /// Adds a ProjectReference node based the the [vb|fs|cs]proj file at <see cref="projPath"/>
        /// </summary>
        /// <param name="projPath"></param>
        /// <param name="resolveToPartialPath">
        /// Resolves the the full <see cref="projPath"/> to a relative path from <see cref="Directory"/>
        /// </param>
        /// <returns></returns>
        public bool AddProjectReferenceNode(string projPath, bool resolveToPartialPath = true)
        {
            if (string.IsNullOrWhiteSpace(projPath) || !File.Exists(projPath))
                return false;
            projPath = Path.GetFullPath(projPath);
            var nfProjFile = new ProjFile(projPath);
            var projGuid = nfProjFile.ProjectGuid;

            var tempPath = resolveToPartialPath ? GetAsRelPathFromMyDirectory(projPath) : projPath;

            var projFileName = Path.GetFileName(projPath);

            var projRefNode =
                _xmlDocument.SelectSingleNode(
                    $"//{NS}:ItemGroup/{NS}:ProjectReference[contains(@Include,'{projFileName}')]", _nsMgr);
            var xmlElem = projRefNode as XmlElement ??
                              _xmlDocument.CreateElement("ProjectReference", DOT_NET_PROJ_XML_NS);

            AddAttribute(xmlElem, "Include", tempPath);

            var projGuidNode = xmlElem.FirstChildNamed("Project");
            var projGuidElem = projGuidNode as XmlElement ?? _xmlDocument.CreateElement("Project", DOT_NET_PROJ_XML_NS);
            projGuidElem.InnerText = projGuid;
            if (projGuidNode == null)
                xmlElem.AppendChild(projGuidElem);

            var nameNode = xmlElem.FirstChildNamed("Name");
            var nameElem = nameNode as XmlElement ?? _xmlDocument.CreateElement("Name", DOT_NET_PROJ_XML_NS);
            nameElem.InnerText = nfProjFile.AssemblyName;
            if (nameNode == null)
                xmlElem.AppendChild(nameElem);

            if (projRefNode == null)
            {
                var itemGroup = GetLastProjectReferenceNode()?.ParentNode ?? NewItemGroup(true);
                itemGroup?.InsertAfter(xmlElem, GetLastProjectReferenceNode());
            }

            _isChanged = true;
            return true;
        }

        /// <summary>
        /// Adds a Reference node based on the assembly located at <see cref="assemblyPath"/> somewhere on the drive.
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
        public bool AddReferenceNode(string assemblyPath, string projGuid = null, bool useExactAsmNameMatch = false,
            bool resolveToPartialPath = false)
        {
            if (string.IsNullOrWhiteSpace(assemblyPath))
                return false;

            //keep local copy to assign hintpath AS-IS 
            var tempPath = resolveToPartialPath ? GetAsRelPathFromMyDirectory(assemblyPath) : assemblyPath;

            //get wrapper of node plus path and asm name
            var projRef = GetBinRefByAsmPath(assemblyPath, useExactAsmNameMatch);

            if (string.IsNullOrWhiteSpace(projRef?.DllOnDisk?.Item2.FullName))
                return false;

            //get or create Reference xml element
            var xmlElem = projRef.Node as XmlElement ?? _xmlDocument.CreateElement("Reference", DOT_NET_PROJ_XML_NS);

            //assign Include attr value 
            AddAttribute(xmlElem, "Include", projRef.DllOnDisk.Item2.FullName);

            //assign SpecificVersion inner text
            var specificVerNode = xmlElem.FirstChildNamed("SpecificVersion");
            var specificVerElem = specificVerNode as XmlElement ??
                                  _xmlDocument.CreateElement("SpecificVersion", DOT_NET_PROJ_XML_NS);
            specificVerElem.InnerText = bool.FalseString;
            if (specificVerNode == null)
                xmlElem.AppendChild(specificVerElem);

            //assign HintPath inner text
            var hintPathNode = xmlElem.FirstChildNamed("HintPath");
            var hintPathElem = hintPathNode as XmlElement ?? _xmlDocument.CreateElement("HintPath", DOT_NET_PROJ_XML_NS);
            hintPathElem.InnerText = tempPath;
            if (hintPathNode == null)
                xmlElem.AppendChild(hintPathElem);

            //add the project guid for debug\tracing
            if (!string.IsNullOrWhiteSpace(projGuid))
            {
                var guidComment = _xmlDocument.CreateComment(projGuid);
                xmlElem.AppendChild(guidComment);
            }

            if (projRef.Node == null)
            {
                var refItemGroup = GetLastReferenceNode()?.ParentNode ?? NewItemGroup(true);
                if (refItemGroup?.InsertAfter(xmlElem, GetLastReferenceNode()) == null)
                    return false;
            }

            _isChanged = true;
            return _isChanged;
        }

        /// <summary>
        /// Adds a simple Reference node having only the Include value as <see cref="assemblySimpleName"/>
        /// </summary>
        /// <param name="assemblySimpleName"></param>
        /// <returns></returns>
        public bool AddGacReferenceNode(string assemblySimpleName)
        {
            if (string.IsNullOrWhiteSpace(assemblySimpleName) || Util.NfType.NfTypeName.IsAssemblyFullName(assemblySimpleName))
                return false;

            var gacRefNode =
                _xmlDocument.SelectSingleNode($"//{NS}:ItemGroup/{NS}:Reference[@Include='{assemblySimpleName}']", _nsMgr);
            var gacRefElem = gacRefNode as XmlElement ?? _xmlDocument.CreateElement("Reference",DOT_NET_PROJ_XML_NS);
            var includeAttr = gacRefElem.Attributes["Include"] ?? _xmlDocument.CreateAttribute("Include");

            includeAttr.Value = assemblySimpleName;
            if (!gacRefElem.HasAttribute("Include"))
                gacRefElem.Attributes.Append(includeAttr);

            if (gacRefNode == null)
            {
                var refItemGroup = GetLastReferenceNode()?.ParentNode ?? NewItemGroup(true);
                if (refItemGroup?.InsertAfter(gacRefElem, GetLastReferenceNode()) == null)
                    return false;
            }
            return true;
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
                AddNewCompileItem(compItem, parentNode);
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
                AddContentItemNode(contentItem, parentNode);
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
        /// Changes all HintPath's to the assmeblies in <see cref="DestLibDirectory"/>
        /// whenever its LastWriteTime is more recent.
        /// </summary>
        /// <param name="destDir"></param>
        /// <returns></returns>
        public int UpdateHintPathTo(string destDir = null)
        {
            //setup a dir to save binary dll's to
            destDir = destDir ?? DestLibDirectory;
            destDir = string.IsNullOrWhiteSpace(destDir) ? Path.Combine(DirectoryName, "lib") : destDir;
            if (!Directory.Exists(destDir))
                return 0;
            Action<BinReference> myAction = reference =>
            {
                //verify hint path exist
                if (string.IsNullOrWhiteSpace(reference?.HintPath))
                    return;
                //look for the file name of the hint path in the destDir
                var fullLibDestName = Path.Combine(destDir, Path.GetFileName(reference.HintPath));

                //compare it to the file the hintpath is currently pointing to 
                var hintPath = Path.Combine(DirectoryName, reference.HintPath);
                if (!File.Exists(fullLibDestName) || !File.Exists(hintPath))
                    return;

                //remove any partial path segments
                fullLibDestName = Path.GetFullPath(fullLibDestName);
                hintPath = Path.GetFullPath(hintPath);

                //get file info
                var fsiDest = new FileInfo(Path.GetFullPath(fullLibDestName));
                var fsiHint = new FileInfo(Path.GetFullPath(hintPath));

                //if the copy in DestLib is newer then update the hint path to it
                if (fsiDest.LastWriteTime > fsiHint.LastWriteTime)
                {
                    var temp = fsiDest.FullName;
                    if (NfPath.TryGetRelPath(DirectoryName, ref temp))
                    {
                        reference.HintPath = temp;
                    }
                }
                _isChanged = true;
            };

            return IterateBinReferences(myAction);
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

        protected internal void AddNewCompileItem(string compItem, XmlNode parentNode)
        {
            var newCompileItem = NewCompileItem(compItem);

            AppendAspCompileItemChildNodes(compItem, newCompileItem);

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

        protected internal void AddContentItemNode(string contentItem, XmlNode parentNode)
        {
            var newContentItemNode = NewContentItemNode(contentItem);

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

        protected internal XmlNode NewContentItemNode(string contentItem)
        {
            var newContentItemNode = GetSingleContentItemNode(contentItem) ??
                                     _xmlDocument.CreateElement("Content", DOT_NET_PROJ_XML_NS);

            if (newContentItemNode.Attributes != null)
            {
                var includeAttr = HasExistingIncludeAttr(newContentItemNode)
                    ? newContentItemNode.Attributes["Include"]
                    : _xmlDocument.CreateAttribute("Include");

                includeAttr.Value = contentItem;
                if (!HasExistingIncludeAttr(newContentItemNode))
                    newContentItemNode.Attributes.Append(includeAttr);
            }
            return newContentItemNode;
        }

        protected internal void AppendAspCompileItemChildNodes(string compItem, XmlNode newCompileItem)
        {
            //asp files have some additional child nodes
            if (!IsAspFile(Path.GetFileNameWithoutExtension(compItem)))
                return;
            var dependentUponNode = FirstChildNamed(newCompileItem, "DependentUpon") ??
                                    _xmlDocument.CreateElement("DependentUpon", DOT_NET_PROJ_XML_NS);

            dependentUponNode.InnerText = Path.GetFileNameWithoutExtension(compItem);

            if (!HasChildNodeNamed(newCompileItem, "DependentUpon"))
                newCompileItem.AppendChild(dependentUponNode);

            if (!compItem.Contains(".designer"))
            {
                var subTypeNode = FirstChildNamed(newCompileItem, "SubType") ??
                                  _xmlDocument.CreateElement("SubType", DOT_NET_PROJ_XML_NS);

                subTypeNode.InnerText = "ASPXCodeBehind";

                if (!HasChildNodeNamed(newCompileItem, "SubType"))
                    newCompileItem.AppendChild(subTypeNode);
            }
        }

        protected internal XmlNode NewCompileItem(string compItem)
        {
            var newCompileItem = GetSingleCompileItemNode(compItem) ??
                                 _xmlDocument.CreateElement("Compile", DOT_NET_PROJ_XML_NS);

            if (newCompileItem.Attributes != null)
            {
                var includeAttr = HasExistingIncludeAttr(newCompileItem)
                    ? newCompileItem.Attributes["Include"]
                    : _xmlDocument.CreateAttribute("Include");

                includeAttr.Value = compItem;
                if (!HasExistingIncludeAttr(newCompileItem))
                    newCompileItem.Attributes.Append(includeAttr);
            }
            return newCompileItem;
        }

        protected internal int IterateBinReferences(Action<BinReference> action)
        {
            var projNode = _xmlDocument.SelectSingleNode($"/{NS}:Project", _nsMgr);
            if (projNode == null)
                return 0;

            //find all the bin refs
            var binRefs = _xmlDocument.SelectNodes($"//{NS}:ItemGroup/{NS}:Reference", _nsMgr);
            var counter = 0;

            if (binRefs == null)
                return counter;

            foreach (var binRefNode in binRefs)
            {
                var binRefElem = binRefNode as XmlElement;
                if (binRefElem == null || !binRefElem.HasAttributes)
                    continue;

                var includeAttr = binRefElem.Attributes["Include"];
                if (string.IsNullOrWhiteSpace(includeAttr?.Value) ||
                    !Util.NfType.NfTypeName.IsAssemblyFullName(includeAttr.Value))
                    continue;

                var binRef = GetBinRefByAsmName(new AssemblyName(includeAttr.Value));
                if (binRef == null)
                    continue;
                action(binRef);
                counter += 1;

            }
            return counter;
        }

        /// <summary>
        /// Gets this <see cref="assemblyPath"/> as a <see cref="BinReference"/>.
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="useExactAsmNameMatch">
        /// Optional, specify that the Assembly Names must be exact matches
        /// </param>
        /// <returns></returns>
        protected internal BinReference GetBinRefByAsmPath(string assemblyPath, bool useExactAsmNameMatch = false)
        {
            if (string.IsNullOrWhiteSpace(assemblyPath))
                return null;

            //if we looked this up before then return it again to save time
            if (_asmRefCache.Any(x => x.SearchPath == assemblyPath))
                return _asmRefCache.First(x => x.SearchPath == assemblyPath);

            _asmRefCache.Add(new BinReference(this) { SearchPath = assemblyPath });

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
            if (!Path.IsPathRooted(assemblyPath))
            {
                assemblyPath = Path.Combine(DirectoryName, assemblyPath);
            }
            if (!File.Exists(assemblyPath))
                return null;

            var assemblyName = System.Reflection.AssemblyName.GetAssemblyName(assemblyPath);
            var fsi = new FileInfo(assemblyPath);
            cache.DllOnDisk = new Tuple<FileSystemInfo, AssemblyName>(fsi, assemblyName);

            XmlNode refNode = null;
            var xpathString = $"//{NS}:ItemGroup/{NS}:Reference[contains(@Include,'{assemblyName.Name}')]";
            if (useExactAsmNameMatch && _xmlDocument.SelectNodes(xpathString, _nsMgr) != null)
            {
                //find all nodes which contain the assembly's simple name
                foreach (var cm in _xmlDocument.SelectNodes(xpathString, _nsMgr))
                {
                    var cmElem = cm as XmlElement;
                    var includeAttr = cmElem?.Attributes["Include"];
                    if (string.IsNullOrWhiteSpace(includeAttr?.Value))
                        continue;
                    var includeAsmName = new AssemblyName(includeAttr.Value);

                    //select the one whose full assembly names are the same.
                    if (!System.Reflection.AssemblyName.ReferenceMatchesDefinition(assemblyName, includeAsmName))
                        continue;

                    cache.DllOnDisk = new Tuple<FileSystemInfo, AssemblyName>(new FileInfo(includeAttr.Value), includeAsmName);
                    refNode = cmElem;
                    break;
                }
            }
            else
            {
                //take the first node that contains the assembly's simple name
                refNode = _xmlDocument.SelectSingleNode(xpathString, _nsMgr);
            }

            //and the the actual xml node if available
            if (refNode != null)
                cache.Node = refNode;

            return cache;
        }

        /// <summary>
        /// Gets this <see cref="asmName"/> as a <see cref="BinReference"/> when such a refernce 
        /// is present on at least one Reference node.
        /// </summary>
        /// <param name="asmName"></param>
        /// <returns></returns>
        protected internal BinReference GetBinRefByAsmName(AssemblyName asmName)
        {
            if (asmName == null)
                return null;

            //return what we have already found once before
            if (
                _asmRefCache.Any(
                    x => x.DllOnDisk?.Item2 != null &&
                         System.Reflection.AssemblyName.ReferenceMatchesDefinition(x.DllOnDisk?.Item2, asmName)))
                return
                    _asmRefCache.First(
                        x => x.DllOnDisk?.Item2 != null &&
                             System.Reflection.AssemblyName.ReferenceMatchesDefinition(x.DllOnDisk?.Item2, asmName));

            //find the Reference node by the assembly's full name
            var binRef = new BinReference(this);
            binRef.Node =
                _xmlDocument.SelectSingleNode(
                    $"//{NS}:ItemGroup/{NS}:Reference[contains(@Include,'{asmName.FullName}')]", _nsMgr);

            //require that the hint path is present 
            if (binRef.Node == null || string.IsNullOrWhiteSpace(binRef.HintPath))
                return null;

            //resolve hint path to full path
            var dllPath = binRef.HintPath;
            if (!Path.IsPathRooted(dllPath))
                dllPath = Path.Combine(DirectoryName, dllPath);

            //require is present on disk
            if (!File.Exists(dllPath))
                return null;

            binRef.DllOnDisk = new Tuple<FileSystemInfo, AssemblyName>(new FileInfo(dllPath),
                System.Reflection.AssemblyName.GetAssemblyName(dllPath));
            
            //add to cache to avoid another search
            _asmRefCache.Add(binRef);
            return binRef;
        }

        /// <summary>
        /// Gets this <see cref="relProjPath"/> as a <see cref="ProjectReference"/>.
        /// </summary>
        /// <param name="relProjPath"></param>
        /// <returns></returns>
        protected internal ProjectReference GetProjRefByProjPath(string relProjPath)
        {
            if (string.IsNullOrWhiteSpace(relProjPath))
                return null;

            //confirm the path in the Include attr points to an actual file on the drive
            var projPath = Path.Combine(DirectoryName, relProjPath);
            if (!File.Exists(projPath))
                throw new RahRowRagee($"The ProjectReference to '{projPath}' does not exisit.");

            if (_projRefCache.Any(x => x.ProjectPath == projPath))
            {
                return _projRefCache.First(x => x.ProjectPath == projPath);
            }

            _projRefCache.Add(new ProjectReference(this) {ProjectPath = projPath});

            var projRef = _projRefCache.First(x => x.ProjectPath == projPath);

            projRef.Node =
                _xmlDocument.SelectSingleNode(
                    $"//{NS}:ItemGroup/{NS}:ProjectReference[@Include='" + relProjPath + "']", _nsMgr);
            if (projRef.Node == null)
                return null;

            projRef.ProjectName = projRef.Node.FirstChildNamed("Name")?.InnerText ?? string.Empty;
            projRef.ProjectGuid = (projRef.Node.FirstChildNamed("Project")?.InnerText ?? string.Empty).ToUpper();

            //deal with odd name appearing like "Some.Asm.Name %28SomeDir\SomeOtherDir%29
            if (projRef.ProjectName.Contains(" "))
            {
                projRef.ProjectName = projRef.ProjectName.Split(' ')[0];
            }

            //do an easy search
            projRef.DllOnDisk = _debugBinFsi2AsmName.FirstOrDefault(x => x.Item2.Name == projRef.ProjectName);

            if (projRef.DllOnDisk != null)
            {
                return projRef;
            }

            //do a hard search
            var nfProjFile = new ProjFile(projPath);
            projRef.ProjectName = nfProjFile.AssemblyName;
            projRef.DllOnDisk = _debugBinFsi2AsmName.FirstOrDefault(x => x.Item2.Name == projRef.ProjectName) ??
                       nfProjFile.DebugBinFsi2AsmName.FirstOrDefault(x => x.Item2.Name == projRef.ProjectName);
            return projRef;
        }

        /// <summary>
        /// A dependent funtion to find a Reference node which was written <see cref="AddReferenceNode"/>
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

        internal XmlNode GetLastProjectReferenceNode()
        {
            return _xmlDocument.SelectSingleNode($"//{NS}:ItemGroup/{NS}:ProjectReference[last()]", _nsMgr);
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

        internal XmlElement NewItemGroup(bool appendAfterLastPropGroup = false)
        {
            var newGroup = _xmlDocument.CreateElement("ItemGroup", DOT_NET_PROJ_XML_NS);
            if (appendAfterLastPropGroup)
            {
                var lastPropertyGroup =_xmlDocument.SelectSingleNode($"//{ProjFile.NS}:PropertyGroup[last()]",
                                            NsMgr);
                if (lastPropertyGroup?.ParentNode == null)
                {
                    AppendToRootNode(newGroup);
                    return newGroup;
                }
                lastPropertyGroup.ParentNode.InsertAfter(newGroup, lastPropertyGroup);
                return newGroup;
            }
            var rootNode = _xmlDocument.SelectSingleNode("/");
            rootNode?.AppendChild(newGroup);
            return newGroup;
        }

        internal void AppendToRootNode(XmlElement elem)
        {
            var rootNode = _xmlDocument.SelectSingleNode("/");
            rootNode?.AppendChild(elem);
        }

        internal string GetAsRelPathFromMyDirectory(string somePath)
        {
            if(string.IsNullOrWhiteSpace( somePath))
                return somePath;
            var tempPath = somePath;
            NfPath.TryGetRelPath(DirectoryName, ref tempPath);
            if (!Path.IsPathRooted(tempPath) && !tempPath.StartsWith(".."))
                tempPath = ".\\" + tempPath;
            return tempPath;
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
