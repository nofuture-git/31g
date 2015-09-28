using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using NoFuture.Exceptions;

namespace NoFuture.Read.Vs
{
    public class ProjFile
    {
        #region constants
        public const string DotNetProjXmlNs = "http://schemas.microsoft.com/developer/msbuild/2003";
        public const string NS = "vs";
        #endregion

        #region internal types
        public class ProjReference
        {
            private readonly ProjFile _projFile;
            public ProjReference(ProjFile owningFile)
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
                    return hintPathNode == null ? null : hintPathNode.InnerText;
                }
            }
        }

        #endregion

        #region fields
        private readonly XmlDocument _vsprojXml;
        private readonly XmlNamespaceManager _nsMgr;
        private readonly string _filePath;
        private readonly string _projDir;
        private readonly List<ProjReference> _asmRefCache = new List<ProjReference>();
        private bool _isChanged;
        #endregion

        #region ctors
        public ProjFile(string vsprojPath)
        {
            ValidateProjFile(vsprojPath);
            _filePath = vsprojPath;
            _vsprojXml = GetVsProjXml(vsprojPath, out _nsMgr);
            _projDir = Path.GetDirectoryName(_filePath) ?? TempDirectories.AppData;
        }
        #endregion

        #region properties
        public XmlDocument VsProjXml { get { return _vsprojXml; } }
        public XmlNamespaceManager NsMgr { get { return _nsMgr; } }
        public bool XmlContentChanged { get { return _isChanged; } }
        #endregion

        #region instance methods
        /// <summary>
        /// Attempts to add the assembly at <see cref="assemblyPath"/>
        /// to the project's references
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        /// <remarks>
        /// MsBuild style environment variables are handled (e.g. $(MY_ENV_VAR) )
        /// </remarks>
        public bool TryAddReferenceEntry(string assemblyPath)
        {
            if (string.IsNullOrWhiteSpace(assemblyPath))
                return false;

            //keep local copy to assign hintpath AS-IS 
            var tempPath = assemblyPath;

            var projRef = GetSingleRefernceNode(assemblyPath);

            if (projRef == null || string.IsNullOrWhiteSpace(projRef.AssemblyFullName))
                return false;

            if (projRef.Node == null)
            {
                projRef.Node = _vsprojXml.CreateElement("Reference", DotNetProjXmlNs);
                var includeAttr = _vsprojXml.CreateAttribute("Include");
                includeAttr.Value = projRef.AssemblyFullName;

                projRef.Node.Attributes.Append(includeAttr);

                var specificVerNode = _vsprojXml.CreateElement("SpecificVersion", DotNetProjXmlNs);
                specificVerNode.InnerText = bool.FalseString;

                var hintPathNode = _vsprojXml.CreateElement("HintPath", DotNetProjXmlNs);
                hintPathNode.InnerText = tempPath;

                projRef.Node.AppendChild(specificVerNode);
                projRef.Node.AppendChild(hintPathNode);

                var refItemGroup = GetLastReferenceNode().ParentNode;

                refItemGroup.InsertAfter(projRef.Node, GetLastReferenceNode());

                _isChanged = true;
            }
            else
            {
                var includeAttr = projRef.Node.Attributes["Include"];
                includeAttr.Value = projRef.AssemblyFullName;

                var specificVerNode =
                    _vsprojXml.SelectSingleNode(string.Format(
                        "//{0}:ItemGroup/{0}:Reference[contains(@Include,'{1}')]/{0}:SpecificVersion", NS, projRef.AssemblyName),
                        _nsMgr);
                if (specificVerNode == null)
                {
                    specificVerNode = _vsprojXml.CreateElement("SpecificVersion", DotNetProjXmlNs);
                    projRef.Node.AppendChild(specificVerNode);
                }

                specificVerNode.InnerText = bool.FalseString;

                var hintPathNode = _vsprojXml.SelectSingleNode(string.Format(
                        "//{0}:ItemGroup/{0}:Reference[contains(@Include,'{1}')]/{0}:HintPath", NS, projRef.AssemblyName),
                        _nsMgr);

                if (hintPathNode == null)
                {
                    hintPathNode = _vsprojXml.CreateElement("HintPath", DotNetProjXmlNs);
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
            const string DependentUpon = "DependentUpon";
            const string Include = "Include";
            const string SubType = "SubType";
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
                var newCompileItem = GetSingleCompileItemNode(compItem) ?? _vsprojXml.CreateElement("Compile", DotNetProjXmlNs);

                var includeAttr = HasExistingIncludeAttr(newCompileItem)
                    ? newCompileItem.Attributes[Include]
                    : _vsprojXml.CreateAttribute(Include);

                includeAttr.Value = compItem;
                if(!HasExistingIncludeAttr(newCompileItem))
                    newCompileItem.Attributes.Append(includeAttr);

                //asp files have some additional child nodes
                if (IsAspFile(Path.GetFileNameWithoutExtension(compItem)))
                {
                    var dependentUponNode = FirstChildNamed(newCompileItem, DependentUpon) ??
                                            _vsprojXml.CreateElement(DependentUpon, DotNetProjXmlNs);

                    dependentUponNode.InnerText = Path.GetFileNameWithoutExtension(compItem);

                    if (!HasChildNodeNamed(newCompileItem, DependentUpon))
                        newCompileItem.AppendChild(dependentUponNode);

                    if (!compItem.Contains(".designer"))
                    {
                        var subTypeNode = FirstChildNamed(newCompileItem, SubType) ??
                                          _vsprojXml.CreateElement(SubType, DotNetProjXmlNs);

                        subTypeNode.InnerText = "ASPXCodeBehind";

                        if (!HasChildNodeNamed(newCompileItem, SubType))
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
                                         _vsprojXml.CreateElement("Content", DotNetProjXmlNs);

                var includeAttr = HasExistingIncludeAttr(newContentItemNode)
                    ? newContentItemNode.Attributes[Include]
                    : _vsprojXml.CreateAttribute(Include);

                includeAttr.Value = contentItem;
                if(!HasExistingIncludeAttr(newContentItemNode))
                    newContentItemNode.Attributes.Append(includeAttr);

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
        /// <param name="relativePathsOut"></param>
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
        /// Writes the current in memory contents to file.
        /// </summary>
        public void WriteContentFile()
        {
            using (var xmlWriter = new XmlTextWriter(_filePath, Encoding.UTF8) {Formatting = Formatting.Indented})
            {
                _vsprojXml.WriteContentTo(xmlWriter);
                xmlWriter.Flush();
                xmlWriter.Close();
            }
        }

        /// <summary>
        /// Gets this <see cref="assemblyPath"/> as a property reference.
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        public ProjReference GetSingleRefernceNode(string assemblyPath)
        {

            if (string.IsNullOrWhiteSpace(assemblyPath))
                return null;

            if (_asmRefCache.Any(x => x.SearchPath == assemblyPath))
                return _asmRefCache.First(x => x.SearchPath == assemblyPath);

            _asmRefCache.Add(new ProjReference(this){SearchPath = assemblyPath});

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

            var assemblyName = AssemblyName.GetAssemblyName(assemblyPath);
            var refNode = _vsprojXml.SelectSingleNode(string.Format(
                    "//{0}:ItemGroup/{0}:Reference[contains(@Include,'{1}')]", NS, assemblyName.Name), _nsMgr);
            cache.AssemblyFullName = assemblyName.FullName;
            cache.AssemblyName = assemblyName.Name;

            if (refNode != null)
                cache.Node = refNode;

            return cache;
        }
        #endregion

        #region internal instance methods
        internal bool IsExistingCompileItem(string fl) { return GetSingleCompileItemNode(fl) != null; }

        internal bool IsExistingContentItem(string fl) { return GetSingleContentItemNode(fl) != null; }

        internal XmlNode GetSingleCompileItemNode(string fl)
        {
            return _vsprojXml.SelectSingleNode(string.Format("//{0}:ItemGroup/{0}:Compile[@Include='{1}']", NS, fl), _nsMgr);
        }

        internal List<XmlElement> GetListCompileItemNodes(string fl)
        {
            var dlk = _vsprojXml.SelectNodes(string.Format("//{0}:ItemGroup/{0}:Compile[contains(@Include,'{1}')]", NS, fl), _nsMgr);
            return dlk == null ? null : dlk.Cast<XmlElement>().ToList();
        }

        internal XmlNode GetSingleContentItemNode(string fl)
        {
            return _vsprojXml.SelectSingleNode(string.Format("//{0}:ItemGroup/{0}:Content[@Include='{1}']", NS, fl), _nsMgr);
        }

        internal List<XmlElement> GetListContentItemNodes(string fl)
        {
            var dlk = _vsprojXml.SelectNodes(string.Format("//{0}:ItemGroup/{0}:Content[contains(@Include,'{1}')]", NS, fl), _nsMgr);
            return dlk == null ? null : dlk.Cast<XmlElement>().ToList();
        }

        internal bool HasExistingIncludeAttr(XmlNode n)
        {
            return n != null && n.Attributes != null && n.Attributes["Include"] != null;
        }

        internal XmlElement FirstChildNamed(XmlNode n, string childNodeName)
        {
            return !HasChildNodeNamed(n, childNodeName) ? null : n.ChildNodes.Cast<XmlElement>().First(x => x.Name == childNodeName);
        }

        internal bool HasChildNodeNamed(XmlNode n, string childNodeName)
        {
            return n != null && n.HasChildNodes && n.ChildNodes.Cast<XmlElement>().Any(x => x.Name == childNodeName);
        }

        internal XmlNode GetLastCompileItem()
        {
            return _vsprojXml.SelectSingleNode(string.Format("//{0}:ItemGroup/{0}:Compile[last()]", NS), _nsMgr);
        }

        internal XmlNode GetLastContentItem()
        {
            return _vsprojXml.SelectSingleNode(string.Format("//{0}:ItemGroup/{0}:Content[last()]", NS), _nsMgr);
        }

        internal XmlNode GetLastReferenceNode()
        {
            return _vsprojXml.SelectSingleNode(string.Format("//{0}:ItemGroup/{0}:Reference[last()]", NS), _nsMgr);
        }

        internal XmlNode GetContentItemParent()
        {
            var lastItem = _vsprojXml.SelectSingleNode(string.Format("//{0}:ItemGroup/{0}:Content[last()]", NS), _nsMgr);
            if (lastItem != null && lastItem.ParentNode != null)
                return lastItem.ParentNode;

            var groups = _vsprojXml.SelectNodes(string.Format("//{0}:ItemGroup", NS), _nsMgr);
            return groups == null ? NewItemGroup() : groups.Cast<XmlElement>().FirstOrDefault(x => !x.HasChildNodes);
        }

        internal XmlNode GetCompileItemParent()
        {
            var lastItem = _vsprojXml.SelectSingleNode(string.Format("//{0}:ItemGroup/{0}:Compile[last()]", NS), _nsMgr);
            if (lastItem != null && lastItem.ParentNode != null)
                return lastItem.ParentNode;

            var groups = _vsprojXml.SelectNodes(string.Format("//{0}:ItemGroup", NS), _nsMgr);
            return groups == null ? NewItemGroup() : groups.Cast<XmlElement>().FirstOrDefault(x => !x.HasChildNodes);
        }

        internal XmlElement NewItemGroup()
        {
            var newGroup = _vsprojXml.CreateElement("ItemGroup", DotNetProjXmlNs);
            var rootNode = _vsprojXml.SelectSingleNode("/");
            rootNode.AppendChild(newGroup);
            return newGroup;
        }

        internal bool IsAspFile(string s)
        {
            s = s.Replace(".designer", string.Empty);
            return !string.IsNullOrWhiteSpace(s) &&
                    (new[] { ".asax", ".ascx", ".asmx", ".aspx" }).Contains(Path.GetExtension(s));
        }

        internal bool IsSrcCodeFile(string x)
        {
            return !string.IsNullOrWhiteSpace(x) && (new[] { ".vb", ".cs" }).Contains(Path.GetExtension(x));
        }

        internal List<string> SingleFileToManifold(string srcCodeFile)
        {
            if(srcCodeFile == null)
                throw new ArgumentNullException("srcCodeFile");
            var files = new List<string>();
            
            //when srcCodeFile is not currently present in the xml
            var allSuchNodes = GetListCompileItemNodes(srcCodeFile);
            if (allSuchNodes == null || allSuchNodes.Count <= 0)
            {
                if (!Path.IsPathRooted(srcCodeFile))
                    srcCodeFile = Path.Combine(_projDir, srcCodeFile);

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
                    files[i] = Path.Combine(_projDir, fl);
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
                if (Util.NfPath.TryGetRelPath(_projDir, ref fli))
                {
                    files[i] = fli;
                }
            }
        }

        internal List<string> ToExistingCompileNodes(List<string> files)
        {
            return files.Where(x => IsSrcCodeFile(x) && IsExistingCompileItem(x)).ToList();
        }
        #endregion

        #region static methods

        /// <summary>
        /// Reads a MsBuild project file as an XML document.
        /// </summary>
        /// <param name="vsprojPath"></param>
        /// <param name="nsMgr"></param>
        /// <returns></returns>
        public static XmlDocument GetVsProjXml(string vsprojPath, out XmlNamespaceManager nsMgr)
        {
            ValidateProjFile(vsprojPath);
            var proj = new XmlDocument();
            proj.Load(vsprojPath);
            nsMgr = new XmlNamespaceManager(proj.NameTable);
            nsMgr.AddNamespace(NS, DotNetProjXmlNs);

            return proj;
        }

        internal static void ValidateProjFile(string vsprojPath)
        {
            if (string.IsNullOrWhiteSpace(vsprojPath) || !File.Exists(vsprojPath))
                throw new ItsDeadJim(string.Format("Cannot find the Vs Project file '{0}'.", vsprojPath));

            if (!Path.HasExtension(vsprojPath))
                throw new ItsDeadJim("Specify a specific [cs|vb|fs]proj file, not a whole directory");

            if (!Regex.IsMatch(Path.GetExtension(vsprojPath), @"\.(vb|cs|fs)proj"))
                throw new ItsDeadJim(string.Format("The Extension '{0}' was unexpected", Path.GetExtension(vsprojPath)));
        }
        #endregion
    }
}
