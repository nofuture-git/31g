using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NoFuture.Exceptions;

namespace NoFuture.Read.Vs
{
    public class SlnFile
    {
        #region constants
        private const string _vsSlnHeaderV14 = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 14
VisualStudioVersion = 14.0.25420.1
MinimumVisualStudioVersion = 10.0.40219.1
";
        #endregion

        #region fields
        private readonly string _fullFileName;
        private readonly string _slnDir;
        private static Dictionary<string, string> _slnProjGuid2Description = new Dictionary<string, string>();

        #endregion

        #region ctor
        /// <summary>
        /// Creates the instance to draft a .sln version 14 file
        /// </summary>
        /// <param name="slnFullFileName">
        /// This does NOT need to exist on the drive, it must only be a valid accessible path.
        /// </param>
        public SlnFile(string slnFullFileName)
        {
            if (string.IsNullOrWhiteSpace(slnFullFileName))
            {
                throw new ArgumentNullException(nameof(slnFullFileName));
            }
            _fullFileName = slnFullFileName;
            _slnDir = Path.GetDirectoryName(_fullFileName);
        }
        #endregion

        #region methods
        internal static string GetNuGetSlnFolderProject()
        {
            var bldr = new StringBuilder();
            bldr.AppendLine("Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \".nuget\", \".nuget\", \"{" +
                        Guid.NewGuid().ToString().ToUpper() + "}\"");
            bldr.AppendLine("	ProjectSection(SolutionItems) = preProject");
            bldr.AppendLine("		.nuget\\NuGet.Config = .nuget\\NuGet.Config");
            bldr.AppendLine("		.nuget\\NuGet.exe = .nuget\\NuGet.exe");
            bldr.AppendLine("		.nuget\\NuGet.targets = .nuget\\NuGet.targets");
            bldr.AppendLine("	EndProjectSection");
            bldr.AppendLine("EndProject");
            return bldr.ToString();
        }

        /// <summary>
        /// Contains the common .NET project files to thier project type guid.
        /// </summary>
        public static Dictionary<string, string> VisualStudioProjTypeGuids
        {
            get
            {
                if (_slnProjGuid2Description.Any())
                    return _slnProjGuid2Description;

                _slnProjGuid2Description.Add("solution folder", "{2150E333-8FDC-42A3-9474-1A3956D46DE8}");
                _slnProjGuid2Description.Add(".csproj", "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}");
                _slnProjGuid2Description.Add(".vbproj", "{F184B08F-C81C-45F6-A57F-5ABD9991F28F}");
                _slnProjGuid2Description.Add(".fsproj", "{F2A71F9B-5D33-465A-A702-920D77279786}");
                _slnProjGuid2Description.Add(".vcproj", "{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}");

                return _slnProjGuid2Description;
            }
        }

        /// <summary>
        /// Creates an empty version 14 .sln file.
        /// </summary>
        /// <param name="fullName"></param>
        public static void CreateEmptySlnVer14(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentNullException(nameof(fullName));
            }
            const string emptySlnContent = @"
Global
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal
";

            var emptySln = _vsSlnHeaderV14 + emptySlnContent;
            File.WriteAllText(fullName, emptySln);
        }

        /// <summary>
        /// Drafts the content of a new .sln file using the project files present in <see cref="projFiles"/>
        /// </summary>
        /// <param name="projFiles"></param>
        /// <param name="updateToProjRef">
        /// Optional, will attempt consolidate the <see cref="projFiles"/> references to each other as 
        /// ProjectReference(s).
        /// </param>
        /// <returns></returns>
        public int SaveSlnVer14(ProjFile[] projFiles, bool updateToProjRef = false)
        {
            var slnFileContent = CreateSlnVer14Content(projFiles) ?? string.Empty;
            var encoding = new UTF8Encoding(true);
            File.WriteAllText(_fullFileName, slnFileContent, encoding);
            if (updateToProjRef)
                SwapOutBinRefForProjRefs(projFiles);
            return string.IsNullOrWhiteSpace(slnFileContent) ? 0 : projFiles.Count();
        }

        /// <summary>
        /// Creates the content of a .sln file for the given <see cref="projFiles"/>
        /// </summary>
        /// <param name="projFiles"></param>
        /// <returns></returns>
        public string CreateSlnVer14Content(ProjFile[] projFiles)
        {

            if (string.IsNullOrWhiteSpace(_slnDir))
            {
                throw new RahRowRagee("You must specifiy a full .sln file name (full path and file name).");
            }

            if (projFiles == null || !projFiles.Any())
            {
                CreateEmptySlnVer14(_fullFileName);
                return null;
            }
            var projSlnEntries = new List<string>();
            var slnConfigPlatforms = new List<string>();
            var projGuids = new List<string>();

            foreach (var projFile in projFiles)
            {
                projSlnEntries.Add(GetSlnProjectEntry(projFile, _slnDir));
                slnConfigPlatforms.AddRange(projFile.ProjectConfigurations);
                projGuids.Add(projFile.ProjectGuid);
            }
            if (!projSlnEntries.Any() && !slnConfigPlatforms.Any() && !projGuids.Any())
                return null;

            projSlnEntries = projSlnEntries.Distinct().ToList();
            slnConfigPlatforms = slnConfigPlatforms.Distinct().ToList();
            projGuids = projGuids.Distinct().ToList();

            var slnBldr = new StringBuilder();
            slnBldr.Append(_vsSlnHeaderV14);

            foreach (var projSlnItem in projSlnEntries)
            {
                slnBldr.Append(projSlnItem);
            }
            if (Directory.Exists(Path.Combine(_slnDir, ".nuget")))
            {
                slnBldr.Append(GetNuGetSlnFolderProject());
            }
            slnBldr.AppendLine("Global");

            slnBldr.AppendLine("	GlobalSection(SolutionConfigurationPlatforms) = preSolution");
            foreach (var projConfig in slnConfigPlatforms)
            {
                slnBldr.AppendLine($"		{projConfig} = {projConfig}");
            }
            slnBldr.AppendLine("	EndGlobalSection");

            slnBldr.AppendLine("	GlobalSection(ProjectConfigurationPlatforms) = postSolution");
            foreach (var projGuid in projGuids)
            {
                foreach (var projConfig in slnConfigPlatforms)
                {
                    slnBldr.AppendLine($"		{projGuid}.{projConfig}.ActiveCfg = {projConfig}");
                    slnBldr.AppendLine($"		{projGuid}.{projConfig}.Build.0 = {projConfig}");
                }
            }
            slnBldr.AppendLine("	EndGlobalSection");
            slnBldr.AppendLine("	GlobalSection(SolutionProperties) = preSolution");
            slnBldr.AppendLine("		HideSolutionNode = FALSE");
            slnBldr.AppendLine("	EndGlobalSection");
            slnBldr.Append("EndGlobal");
            return slnBldr.ToString();
        }

        /// <summary>
        /// Drafts the two typical lines found, per project, in a .sln file.
        /// </summary>
        /// <param name="projFile"></param>
        /// <param name="slnDir"></param>
        /// <returns></returns>
        public static string GetSlnProjectEntry(ProjFile projFile, string slnDir)
        {
            var projExt = Path.GetExtension(projFile.FileName);
            if (string.IsNullOrWhiteSpace(projExt))
                return null;

            var projGuid = projFile.ProjectGuid;
            if (string.IsNullOrWhiteSpace(projGuid))
                return null;

            if (!VisualStudioProjTypeGuids.ContainsKey(projExt))
                return null;

            var projTypeGuid = VisualStudioProjTypeGuids[projExt];

            var projName = Path.GetFileNameWithoutExtension(projFile.FileName);

            var projFileFullName = Path.Combine(projFile.DirectoryName, projFile.FileName);
            var projRelFullName = projFileFullName;
            Util.NfPath.TryGetRelPath(slnDir, ref projRelFullName);

            var projBldr = new StringBuilder();
            projBldr.AppendLine(
                $"Project(\"{projTypeGuid}\") = \"{projName}\", \"{projRelFullName}\", \"{projGuid}\"");
            projBldr.AppendLine("EndProject");
            return projBldr.ToString();
        }

        /// <summary>
        /// Operates on each <see cref="projFiles"/> swapping out 'Reference' nodes
        /// for 'ProjectReference' comparing each one to each other.
        /// </summary>
        /// <param name="projFiles"></param>
        /// <returns></returns>
        protected internal int SwapOutBinRefForProjRefs(ProjFile[] projFiles)
        {
            if (projFiles == null || !projFiles.Any())
                return 0;
            var counter = 0;

            //compare every projfile to every other projfile
            for (var i = 0; i < projFiles.Length; i++)
            {
                var projFile = projFiles[i];

                var projGuid = projFile.ProjectGuid;
                if (string.IsNullOrWhiteSpace(projGuid))
                    continue;

                var projName = Path.GetFileNameWithoutExtension(projFile.FileName);
                var projFileFullName = Path.Combine(projFile.DirectoryName, projFile.FileName);


                for (var j = 0; j < projFiles.Length; j++)
                {
                    //skip over yourself
                    if (i == j)
                        continue;

                    //the binary reference would have to been added by NoFuture to have an Aliases node containing the proj guid
                    var otherProjFile = projFiles[j];
                    var otherBinRefAliasNode =
                        otherProjFile.VsProjXml.SelectSingleNode($"//{ProjFile.NS}:Reference/{ProjFile.NS}:Aliases[contains(text(),'{projGuid}')]",
                            otherProjFile.NsMgr);

                    //if the Aliases node was missing then just go to the next
                    if (otherBinRefAliasNode?.ParentNode == null)
                        continue;
                    //step up to the whole item group containing this bin ref node
                    var parentOfOtherBinRefNode =
                        otherProjFile.VsProjXml.SelectSingleNode(
                            $"//{ProjFile.NS}:Reference/{ProjFile.NS}:Aliases[contains(text(),'{projGuid}')]/../..",
                            otherProjFile.NsMgr);
                    if (parentOfOtherBinRefNode == null)
                        continue;

                    //drop the binary ref node
                    parentOfOtherBinRefNode.RemoveChild(otherBinRefAliasNode.ParentNode);

                    var otherProjItemGroup = otherProjFile.VsProjXml.CreateElement("ItemGroup",
                        ProjFile.DOT_NET_PROJ_XML_NS);
                    var lastPropertyGroup =
                        otherProjFile.VsProjXml.SelectSingleNode($"//{ProjFile.NS}:PropertyGroup[last()]",
                            otherProjFile.NsMgr);
                    if (lastPropertyGroup?.ParentNode == null)
                        continue;
                    lastPropertyGroup.ParentNode.InsertAfter(otherProjItemGroup, lastPropertyGroup);

                    //create the replacement project ref node for the binary ref node we dropped
                    var newProjRefNode = otherProjFile.VsProjXml.CreateElement("ProjectReference",
                        ProjFile.DOT_NET_PROJ_XML_NS);
                    var includeAttr = otherProjFile.VsProjXml.CreateAttribute("Include");

                    //where is 'projFile' in relation to 'otherProjFile'?
                    var includeAttrValue = projFileFullName;
                    Util.NfPath.TryGetRelPath(otherProjFile.DirectoryName, ref includeAttrValue);

                    includeAttr.Value = includeAttrValue;
                    newProjRefNode.Attributes.Append(includeAttr);

                    var newProjRefGuidNode = otherProjFile.VsProjXml.CreateElement("Project",
                        ProjFile.DOT_NET_PROJ_XML_NS);
                    newProjRefGuidNode.InnerText = projGuid;
                    newProjRefNode.AppendChild(newProjRefGuidNode);

                    var newProjNameNode = otherProjFile.VsProjXml.CreateElement("Name", ProjFile.DOT_NET_PROJ_XML_NS);
                    newProjNameNode.InnerText = projName;
                    newProjRefNode.AppendChild(newProjNameNode);

                    //add the new project ref node to the new Item Group node
                    otherProjItemGroup.AppendChild(newProjRefNode);
                    counter += 1;

                }
            }

            //proceed to save each file with changes
            foreach(var projFile in projFiles)
                projFile.Save();

            return counter;
        }
        #endregion
    }
}
