﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;

namespace NoFuture.Read.Vs
{
    /// <summary>
    /// A NoFuture type for creating a solution file using the NoFuture's <see cref="ProjFile"/> types.
    /// </summary>
    public class SlnFile
    {
        #region constants
        private const string _vsSlnHeaderV14 = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 14
VisualStudioVersion = 14.0.25420.1
MinimumVisualStudioVersion = 10.0.40219.1
";
        private const string _vsSlnHeaderV15 = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 15
VisualStudioVersion = 15.0.27004.2005
MinimumVisualStudioVersion = 10.0.40219.1
";

        private const string emptySlnContent = @"
Global
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal
";

        private const string SLN_EXT = ".sln";

        internal const string WEB_APPLICATION = "web application";
        #endregion

        #region fields
        private readonly string _fullFileName;
        private readonly string _slnDir;
        private static readonly Dictionary<string, string> slnProjGuid2Description = new Dictionary<string, string>();

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
            if (!Path.HasExtension(slnFullFileName) ||
                !string.Equals(Path.GetExtension(slnFullFileName), SLN_EXT, StringComparison.OrdinalIgnoreCase))
            {
                slnFullFileName += SLN_EXT;
            }
            _fullFileName = slnFullFileName;
            _slnDir = Path.GetDirectoryName(_fullFileName);
        }
        #endregion

        #region methods
        /// <summary>
        /// The common NuGet entry found within VS .sln files, this entry is what 
        /// causes the .nuget folder to appear atop the solution explorer in VS.
        /// </summary>
        /// <returns></returns>
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
        /// Contains the common .NET project file extensions to thier project type guid.
        /// </summary>
        /// <remarks>
        /// Only contains a list for the four common .NET projects (viz. F#, VB.NET, C# and VC) 
        /// along with the guid for 'solution folder'.  
        /// An exhuastive list is available on a your machine at either 
        /// HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\(version)\Projects 
        ///  -or-
        /// HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\VisualStudio\(version)\Projects
        /// </remarks>
        public static Dictionary<string, string> VisualStudioProjTypeGuids
        {
            get
            {
                if (slnProjGuid2Description.Any())
                    return slnProjGuid2Description;

                slnProjGuid2Description.Add("solution folder", "{2150E333-8FDC-42A3-9474-1A3956D46DE8}");
                slnProjGuid2Description.Add(".csproj", "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}");
                slnProjGuid2Description.Add(".vbproj", "{F184B08F-C81C-45F6-A57F-5ABD9991F28F}");
                slnProjGuid2Description.Add(".fsproj", "{F2A71F9B-5D33-465A-A702-920D77279786}");
                slnProjGuid2Description.Add(".vcxproj", "{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}");
                slnProjGuid2Description.Add(WEB_APPLICATION, "{349C5851-65DF-11DA-9384-00065B846F21}");

                return slnProjGuid2Description;
            }
        }

        /// <summary>
        /// Helper method to get the project-type-guid for a web application
        /// </summary>
        /// <returns></returns>
        internal static string GetWebApplicationProjectType()
        {
            if (!VisualStudioProjTypeGuids.ContainsKey(WEB_APPLICATION))
                return null;
            var webAppProjTypeGuid = VisualStudioProjTypeGuids[WEB_APPLICATION];
            if (string.IsNullOrEmpty(webAppProjTypeGuid) ||
                webAppProjTypeGuid.Length > ProjFile.CURLY_OPEN_GUID_CURLY_CLOSE_LEN)
                return null;
            return webAppProjTypeGuid;
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
        /// Creates an empty version 15 .sln file.
        /// </summary>
        /// <param name="fullName"></param>
        public static void CreateEmptySlnVer15(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentNullException(nameof(fullName));

            var emptySln = _vsSlnHeaderV15 + emptySlnContent;
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
            if (projFiles == null || projFiles.Length <= 0)
            {
                CreateEmptySlnVer14(_fullFileName);
                return 0;
            }            
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
        /// <param name="projFile">
        /// If the projFile has its <see cref="ProjFile.GetMainProjectTypeGuid()"/> property assigned then 
        /// that value is used; elsewise, a match is looked up in <see cref="VisualStudioProjTypeGuids"/>
        /// </param>
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

            if (string.IsNullOrWhiteSpace(projFile.GetMainProjectTypeGuid()) && !VisualStudioProjTypeGuids.ContainsKey(projExt))
                throw new NotImplementedException("There is no VS Project Type Guid defined for " +
                                                  $"a '{projExt}' file type.  Assign the ProjFile's " +
                                                  "VsProjectTypeGuids directly.");

            var projTypeGuid = projFile.GetMainProjectTypeGuid() ?? VisualStudioProjTypeGuids[projExt];

            var projName = Path.GetFileNameWithoutExtension(projFile.FileName);

            var projFileFullName = Path.Combine(projFile.DirectoryName, projFile.FileName);
            var projRelFullName = projFileFullName;
            NfPath.TryGetRelPath(slnDir, ref projRelFullName);

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

                var projFileFullName = Path.Combine(projFile.DirectoryName, projFile.FileName);

                for (var j = 0; j < projFiles.Length; j++)
                {
                    //skip over yourself
                    if (i == j)
                        continue;

                    //the binary reference would have to been added by NoFuture to have an Aliases node containing the proj guid
                    var otherProjFile = projFiles[j];
                    var otherBinRefNode = otherProjFile.GetRefNodeByGuidComment(projGuid);

                    //if the proj guid comment was missing then just go to the next
                    if (otherBinRefNode?.ParentNode == null)
                        continue;

                    //drop the binary ref node
                    otherBinRefNode.ParentNode.RemoveChild(otherBinRefNode);

                    if (otherProjFile.AddProjectReferenceNode(projFileFullName))
                    {
                        counter += 1;
                    }
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
