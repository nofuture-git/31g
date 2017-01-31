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
    public class ProjFile : BaseXmlDoc
    {
        #region constants
        public const string DOT_NET_PROJ_XML_NS = "http://schemas.microsoft.com/developer/msbuild/2003";
        public const string NS = "vs";
        #endregion

        #region internal types
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
        public int TryReplaceToBinaryRef()
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

            //setup a dir to binary dll's to
            var destLibDir = Path.Combine(DirectoryName, "lib");
            if (!Directory.Exists(destLibDir))
                Directory.CreateDirectory(destLibDir);

            //get a hash of files to proj ref names
            var binDict = GetProjRefPath2Name();
            if (!binDict.Any())
                throw new ItsDeadJim($"There are no .dll files located in {debugBin}.");

            var libBin = new List<string>();

            //set aside mem for container node of all proj refs
            XmlNode projRefItemGrpNode = null;

            //copy each proj ref from debug bin to lib
            foreach (var projRefNode in projRefs)
            {
                var projRefElem = projRefNode as XmlElement;
                if (projRefElem == null || !projRefElem.HasAttributes)
                    continue;
                //cache this for later, only need it once
                if(projRefItemGrpNode == null)
                    projRefItemGrpNode = projRefElem.ParentNode;

                var includeAttr = projRefElem.Attributes["Include"];
                if (string.IsNullOrWhiteSpace(includeAttr?.Value))
                    continue;
                var projPath = Path.Combine(DirectoryName, includeAttr.Value);
                if(!File.Exists(projPath))
                    throw new RahRowRagee($"The ProjectReference to '{projPath}' does not exisit.");

                var simpleAsmName =
                    GetInnerText("//{0}:ItemGroup/{0}:ProjectReference[@Include='" + includeAttr.Value + "']/{0}:Name");
                var dllTuple = binDict.FirstOrDefault(x => x.Item2 == simpleAsmName);
                if(dllTuple == null)
                    throw new RahRowRagee($"Could not find a matching .dll for the ProjectRefernce with the Name of '{simpleAsmName}'");

                var libFullName = Path.Combine(destLibDir, dllTuple.Item1.Name);
                libBin.Add(libFullName);

                File.Copy(dllTuple.Item1.FullName, libFullName, true);
            }
            if (projRefItemGrpNode == null)
                return 0;

            //drop the entire item group for ProjectReference
            
            projNode.RemoveChild(projRefItemGrpNode);
            _isChanged = true;

            //now add each lib\dll path as a binary ref
            foreach (var dllLib in libBin)
            {
                if (!TryAddReferenceEntry(dllLib, true))
                    throw new RahRowRagee($"Could not add a Reference node for {dllLib}");
            }
            return libBin.Count;
        }

        /// <summary>
        /// Attempts to add the assembly at <see cref="assemblyPath"/>
        /// to the project's references
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="resolveToPartialPath">
        /// Will resolve the hint path to a relative path off the project's root 
        /// whenever the <see cref="assemblyPath"/> shares the <see cref="BaseXmlDoc.DirectoryName"/>
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// MsBuild style environment variables are handled (e.g. $(MY_ENV_VAR) )
        /// </remarks>
        public bool TryAddReferenceEntry(string assemblyPath, bool resolveToPartialPath = false)
        {
            if (string.IsNullOrWhiteSpace(assemblyPath))
                return false;

            //keep local copy to assign hintpath AS-IS 
            var tempPath = assemblyPath;
            if (resolveToPartialPath)
            {
                NfPath.TryGetRelPath(DirectoryName, ref tempPath);
            }

            var projRef = GetSingleBinRefernceNode(assemblyPath);

            if (string.IsNullOrWhiteSpace(projRef?.AssemblyFullName))
                return false;

            if (projRef.Node == null)
            {
                projRef.Node = _xmlDocument.CreateElement("Reference", DOT_NET_PROJ_XML_NS);
                var includeAttr = _xmlDocument.CreateAttribute("Include");
                includeAttr.Value = projRef.AssemblyFullName;

                projRef.Node.Attributes?.Append(includeAttr);

                var specificVerNode = _xmlDocument.CreateElement("SpecificVersion", DOT_NET_PROJ_XML_NS);
                specificVerNode.InnerText = bool.FalseString;

                var hintPathNode = _xmlDocument.CreateElement("HintPath", DOT_NET_PROJ_XML_NS);
                hintPathNode.InnerText = tempPath;

                projRef.Node.AppendChild(specificVerNode);
                projRef.Node.AppendChild(hintPathNode);

                var refItemGroup = GetLastReferenceNode().ParentNode;

                refItemGroup?.InsertAfter(projRef.Node, GetLastReferenceNode());

                _isChanged = true;
            }
            else
            {
                if (projRef.Node.Attributes != null)
                {
                    var includeAttr = projRef.Node.Attributes["Include"];
                    includeAttr.Value = projRef.AssemblyFullName;
                }

                var specificVerNode =
                    _xmlDocument.SelectSingleNode(string.Format(
                        "//{0}:ItemGroup/{0}:Reference[contains(@Include,'{1}')]/{0}:SpecificVersion", NS, projRef.AssemblyName),
                        _nsMgr);
                if (specificVerNode == null)
                {
                    specificVerNode = _xmlDocument.CreateElement("SpecificVersion", DOT_NET_PROJ_XML_NS);
                    projRef.Node.AppendChild(specificVerNode);
                }

                specificVerNode.InnerText = bool.FalseString;

                var hintPathNode = _xmlDocument.SelectSingleNode(string.Format(
                        "//{0}:ItemGroup/{0}:Reference[contains(@Include,'{1}')]/{0}:HintPath", NS, projRef.AssemblyName),
                        _nsMgr);

                if (hintPathNode == null)
                {
                    hintPathNode = _xmlDocument.CreateElement("HintPath", DOT_NET_PROJ_XML_NS);
                    projRef.Node.AppendChild(hintPathNode);
                }

                hintPathNode.InnerText = tempPath;

                _isChanged = true;
            }

            return _isChanged;
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
        /// <returns></returns>
        public BinReference GetSingleBinRefernceNode(string assemblyPath)
        {

            if (string.IsNullOrWhiteSpace(assemblyPath))
                return null;

            if (_asmRefCache.Any(x => x.SearchPath == assemblyPath))
                return _asmRefCache.First(x => x.SearchPath == assemblyPath);

            _asmRefCache.Add(new BinReference(this){SearchPath = assemblyPath});

            var cache = _asmRefCache.First(x => x.SearchPath == assemblyPath);

            //resolve vs style env vars 
            var tempPath = assemblyPath;
            if (tempPath.Contains("$("))
            {
                if (!Util.NfPath.TryResolveEnvVar(tempPath, ref assemblyPath))
                {
                    return null;
                }
            }

            //test full file path for existence
            if (!File.Exists(assemblyPath))
                return null;

            var assemblyName = System.Reflection.AssemblyName.GetAssemblyName(assemblyPath);
            var refNode = _xmlDocument.SelectSingleNode(string.Format(
                    "//{0}:ItemGroup/{0}:Reference[contains(@Include,'{1}')]", NS, assemblyName.Name), _nsMgr);
            cache.AssemblyFullName = assemblyName.FullName;
            cache.AssemblyName = assemblyName.Name;

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

        internal List<Tuple<FileSystemInfo, string>> GetProjRefPath2Name()
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
            return _xmlDocument.SelectSingleNode(string.Format("//{0}:ItemGroup/{0}:Compile[@Include='{1}']", NS, fl), _nsMgr);
        }

        internal List<XmlElement> GetListCompileItemNodes(string fl)
        {
            var dlk = _xmlDocument.SelectNodes(string.Format("//{0}:ItemGroup/{0}:Compile[contains(@Include,'{1}')]", NS, fl), _nsMgr);
            return dlk?.Cast<XmlElement>().ToList();
        }

        internal XmlNode GetSingleContentItemNode(string fl)
        {
            return _xmlDocument.SelectSingleNode(string.Format("//{0}:ItemGroup/{0}:Content[@Include='{1}']", NS, fl), _nsMgr);
        }

        internal List<XmlElement> GetListContentItemNodes(string fl)
        {
            var dlk = _xmlDocument.SelectNodes(string.Format("//{0}:ItemGroup/{0}:Content[contains(@Include,'{1}')]", NS, fl), _nsMgr);
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
            return _xmlDocument.SelectSingleNode(string.Format("//{0}:ItemGroup/{0}:Compile[last()]", NS), _nsMgr);
        }

        internal XmlNode GetLastContentItem()
        {
            return _xmlDocument.SelectSingleNode(string.Format("//{0}:ItemGroup/{0}:Content[last()]", NS), _nsMgr);
        }

        internal XmlNode GetLastReferenceNode()
        {
            return _xmlDocument.SelectSingleNode(string.Format("//{0}:ItemGroup/{0}:Reference[last()]", NS), _nsMgr);
        }

        internal XmlNode GetContentItemParent()
        {
            var lastItem = _xmlDocument.SelectSingleNode(string.Format("//{0}:ItemGroup/{0}:Content[last()]", NS), _nsMgr);
            if (lastItem?.ParentNode != null)
                return lastItem.ParentNode;

            var groups = _xmlDocument.SelectNodes($"//{NS}:ItemGroup", _nsMgr);
            return groups == null ? NewItemGroup() : groups.Cast<XmlElement>().FirstOrDefault(x => !x.HasChildNodes);
        }

        internal XmlNode GetCompileItemParent()
        {
            var lastItem = _xmlDocument.SelectSingleNode(string.Format("//{0}:ItemGroup/{0}:Compile[last()]", NS), _nsMgr);
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
