﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using NoFuture.Shared.Core;
using NoFuture.Shared.Core.Cfg;

namespace NoFuture.Util.Core
{
    public class NfPath
    {
        /// <summary>
        /// Writes an xml document to file with formatting in UTF8 encoding
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="fileName"></param>
        /// <param name="encoding">Optionally define the encoding, defaults to UTF-8</param>
        /// <returns></returns>
        public static string SaveXml(XmlDocument xml, string fileName, Encoding encoding = null)
        {
            if (xml == null)
                return null;
            encoding = encoding ?? Encoding.UTF8;
            fileName = fileName ?? GetRandomFileFullName();
            using (var xmlWriter = new XmlTextWriter(fileName, encoding) { Formatting = Formatting.Indented })
            {
                xml.WriteContentTo(xmlWriter);
                xmlWriter.Flush();
                xmlWriter.Close();
            }
            return fileName;
        }

        /// <summary>
        /// Gets the number of lines in <see cref="fullName"/> file.
        /// </summary>
        /// <param name="fullName">Is required to have an extension.</param>
        /// <param name="runExtDirFilters">
        /// Filters out known binary files extensions and excludes 
        /// file in directories defined in <see cref="NfConfig.ExcludeCodeDirectories"/>
        /// </param>
        /// <returns></returns>
        public static int GetTxtFileLineCount(string fullName, bool runExtDirFilters = true)
        {
            if (String.IsNullOrWhiteSpace(fullName))
                return 0;

            if (String.IsNullOrWhiteSpace(Path.GetExtension(fullName)))
                return 0;

            if (!File.Exists(fullName))
                return 0;

            if(!runExtDirFilters)
                return File.ReadAllLines(fullName).Length;

            //if its a binary file or its in an exclude dir do not read it
            if (IsBinaryFileExtension(fullName) || ContainsExcludeCodeDirectory(fullName))
                return 0;

            return File.ReadAllLines(fullName).Length;
        }

        /// <summary>
        /// Writes the <see cref="content"/> to a temp file in <see cref="NfConfig.TempDirectories.AppData"/>
        /// </summary>
        /// <param name="fileNamePrefix"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string WriteToTempFile(string fileNamePrefix, string content)
        {
            var fl = GetRandomFileFullName(fileNamePrefix);
            File.WriteAllText(fl, content);
            return fl;
        }

        /// <summary>
        /// Copies the file at <see cref="path"/> to <see cref="dest"/>
        /// except in the case that there is already a file at <see cref="dest"/>
        /// AND it has a more recent LastWriteTime.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool CopyFileIfNewer(string path, string dest)
        {
            //need both paths
            if (String.IsNullOrWhiteSpace(path) || String.IsNullOrWhiteSpace(dest))
                return false;

            //src file must be present
            if (!File.Exists(path))
                return false;

            //remove any relative path leafs
            path = Path.GetFullPath(path);
            dest = Path.GetFullPath(dest);

            //when dest is a directory, resolve to full file name
            if (Directory.Exists(dest))
            {
                dest = Path.Combine(dest, Path.GetFileName(path));
            }

            //when dest file is present check last write time
            if (File.Exists(path) && File.Exists(dest))
            {
                var pathFs = new FileInfo(path);
                var destFs = new FileInfo(dest);

                if (pathFs.LastWriteTime > destFs.LastWriteTime)
                {
                    File.Copy(path, dest, true);
                    return true;
                }
            }

            //otherwise use std IO
            File.Copy(path, dest, true);
            return true;
        }

        /// <summary>
        /// Gets a full path to a random file name in directory <see cref="NfConfig.TempDirectories.AppData"/>
        /// </summary>
        /// <param name="fileNamePrefix"></param>
        /// <returns></returns>
        public static string GetRandomFileFullName(string fileNamePrefix = null)
        {
            return Path.Combine(NfConfig.TempDirectories.AppData,
                (fileNamePrefix ?? Etc.DEFAULT_NAME_PREFIX) + Path.GetRandomFileName());
        }

        /// <summary>
        /// End-of-line conversion.
        /// </summary>
        /// <param name="somePath"></param>
        public static void ConvertToCrLf(string somePath)
        {
            if (String.IsNullOrWhiteSpace(somePath))
            {
                return;
            }
            if (!File.Exists(somePath))
                return;

            var fileContent = File.ReadAllText(somePath);
            fileContent = Etc.ConvertToCrLf(fileContent);

            File.WriteAllText(somePath, fileContent);

        }

        /// <summary>
        /// End-of-line conversion with specific encoding.
        /// </summary>
        /// <param name="somePath"></param>
        /// <param name="encoder"></param>
        public static void ConvertToCrLf(string somePath, Encoding encoder)
        {
            if (String.IsNullOrWhiteSpace(somePath))
            {
                return;
            }
            if (!File.Exists(somePath))
                return;

            var fileContent = File.ReadAllText(somePath);
            fileContent = Etc.ConvertToCrLf(fileContent);

            File.WriteAllText(somePath, fileContent, encoder);

        }

        /// <summary>
        /// Simple method to eliminate any chars in <see cref="intendedFileName"/>
        /// of all of those found at <see cref="System.IO.Path.GetInvalidFileNameChars"/>
        /// </summary>
        /// <param name="intendedFileName"></param>
        /// <returns></returns>
        public static string SafeFilename(string intendedFileName)
        {
            if (String.IsNullOrWhiteSpace(intendedFileName))
                return String.Empty;

            intendedFileName = intendedFileName.Trim();

            return
                new String(
                    intendedFileName.ToCharArray()
                        .Where(i => !(Path.GetInvalidFileNameChars().Any(ic => ic == i)))
                        .ToArray());
        }

        /// <summary>
        /// This will break one file into many on the <see cref="tupleMarker"/> having either <see cref="maxLength"/> or less.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="maxLength"></param>
        /// <param name="tupleMarker">
        /// Delimiter, itself being limited to three chars.
        /// Pass in null to have it default to the char sequence of "}," (x7D x2C)
        /// </param>
        /// <returns>
        /// True if all splits happened on the <see cref="tupleMarker"/> - false otherwise.
        /// </returns>
        /// <remarks>
        /// NOTE: the split will proceed with or without a match to the marker.  If the <see cref="tupleMarker"/> never appears in the 
        /// <see cref="path"/> even once, then expect every file to be exactly the size of <see cref="maxLength"/>
        /// </remarks>
        public static bool TrySplitFileOnMarker(string path, Tuple<char?, char?, char?> tupleMarker, int maxLength = Constants.MAX_JSON_LEN)
        {
            String marker;
            if (tupleMarker == null)
                marker = "},";
            else
            {
                var mBldr = new StringBuilder();
                if (tupleMarker.Item1 != null)
                    mBldr.Append(tupleMarker.Item1);
                if (tupleMarker.Item2 != null)
                    mBldr.Append(tupleMarker.Item2);
                if (tupleMarker.Item3 != null)
                    mBldr.Append(tupleMarker.Item3);

                marker = mBldr.ToString();
            }
            //test the path is valid
            if (!File.Exists(path))
                return false;

            //test valid marker
            if (String.IsNullOrEmpty(marker))
                return false;

            //get the files current length
            var fi = new FileInfo(path);
            if (fi.Length <= maxLength)
                return false;

            if (maxLength <= 0)
                return false;

            var breakFiles = new List<string>();
            var bareFileName = Path.GetFileNameWithoutExtension(path);
            var fileExt = Path.GetExtension(path);
            var fileDir = Path.GetDirectoryName(path);
            var df = (fi.Length) % maxLength == 0 ? 0 : 1;//it may divide evenly...
            var splitEffort = true;
            var fileBreakCounter = (int)System.Math.Floor(((fi.Length) / (double)maxLength)) + df + 1;
            var breakCountIncrement = -1;
            //get the whole file as a buffer
            using (var fileStream = File.OpenRead(path))
            {
                //move cursor to the end of the file
                fileStream.Seek(0, SeekOrigin.End);

                //until the current position lies on the inside fo the max length boundary
                while (fileStream.Position > maxLength)
                {
                    fileBreakCounter += breakCountIncrement;

                    //we want file names to match thier order
                    if (fileBreakCounter == -1)
                    {
                        //we should only flip this once
                        if (breakCountIncrement > -1) //endless loop - break now
                            return false;

                        fileBreakCounter = -999;
                        breakCountIncrement = 1;
                    }

                    //where are we at right now
                    var loopEntryPosition = fileStream.Position;

                    //move the cursor back by max length from current relative position
                    fileStream.Position = loopEntryPosition - maxLength;
                    var markerBuffer = new byte[marker.Length];

                    //begin read foward until you find a match
                    while (Encoding.UTF8.GetString(markerBuffer) != marker)
                    {
                        //did we reach the end of the file or go all the way back to the last marker?
                        if (fileStream.Position >= fi.Length || fileStream.Position >= loopEntryPosition)
                            break;
                        var read = fileStream.Read(markerBuffer, 0, markerBuffer.Length);
                        if (read <= 0)
                            break;
                    }

                    var dumpFileName = Path.Combine(fileDir ?? NfConfig.TempDirectories.AppData,
                        String.Format("{0}.{1:000}{2}", fileBreakCounter >= 0 ? bareFileName : "_" + bareFileName,
                            fileBreakCounter, fileExt));

                    //never let dump File Position move up to nor past what this iteration started with
                    if (fileStream.Position >= fi.Length || fileStream.Position >= loopEntryPosition)
                    {
                        //couldn't make a clean break on the marker so make a rough one instead
                        fileStream.Position = loopEntryPosition - maxLength;
                        splitEffort = false;
                    }
                    var dumpFilePosition = fileStream.Position;

                    var dumpFileBuffer = new byte[loopEntryPosition - dumpFilePosition];

                    using (var dumpFileStream = File.Create(dumpFileName))
                    {
                        var read = fileStream.Read(dumpFileBuffer, 0, dumpFileBuffer.Length);
                        //something is eff'ed up
                        if (read <= 0)
                            break;
                        dumpFileStream.Write(dumpFileBuffer, 0, dumpFileBuffer.Length);
                        dumpFileStream.Flush();
                    }

                    //finalize loop and go again
                    fileStream.Position = dumpFilePosition;
                    breakFiles.Add(dumpFileName);
                }

                //we still need to drop the head of the file
                if (fileStream.Position > 0)
                {
                    var dumpFileName = Path.Combine(fileDir ?? NfConfig.TempDirectories.AppData,
                        String.Format("{0}.{1:000}{2}", fileBreakCounter >= 0 ? bareFileName : "_" + bareFileName,
                            fileBreakCounter, fileExt));

                    var dumpFilePosition = fileStream.Position;

                    //move position to the front of the file
                    fileStream.Seek(0, SeekOrigin.Begin);

                    using (var dumpFileStream = File.Create(dumpFileName))
                    {
                        var dumpFileBuffer = new byte[dumpFilePosition];
                        fileStream.Read(dumpFileBuffer, 0, dumpFileBuffer.Length);
                        dumpFileStream.Write(dumpFileBuffer, 0, dumpFileBuffer.Length);
                        dumpFileStream.Flush();
                    }

                    breakFiles.Add(dumpFileName);
                }
            }

            return splitEffort;
        }

        /// <summary>
        /// Returns true when a relative path was found from <see cref="currentWorkingDir"/>
        /// to <see cref="somePath"/>.
        /// </summary>
        /// <param name="currentWorkingDir"></param>
        /// <param name="somePath"></param>
        /// <returns></returns>
        public static bool TryGetRelPath(string currentWorkingDir, ref string somePath)
        {
            if (String.IsNullOrWhiteSpace(currentWorkingDir))
                return false;

            if (String.IsNullOrWhiteSpace(somePath))
                return false;

            var somePathTemp = somePath;

            //resolve envrio vars if present
            var tempPath = String.Empty;
            if (TryResolveEnvVar(currentWorkingDir, ref tempPath))
                currentWorkingDir = tempPath;

            tempPath = String.Empty;
            if (TryResolveEnvVar(somePathTemp, ref tempPath))
                somePathTemp = tempPath;

            //must be absolute paths
            if (!Path.IsPathRooted(somePathTemp) || !Path.IsPathRooted(currentWorkingDir))
            {
                return false;
            }
            //remove any relative path leafs 
            somePathTemp = Path.GetFullPath(somePathTemp);
            currentWorkingDir = Path.GetFullPath(currentWorkingDir);

            //remove files from end if present
            if (HasKnownExtension(currentWorkingDir))
                currentWorkingDir = Path.GetDirectoryName(currentWorkingDir);

            string somePathFile = null;
            if (HasKnownExtension(somePathTemp))
            {
                somePathFile = Path.GetFileName(somePathTemp);
                somePathTemp = Path.GetDirectoryName(somePathTemp);
            }

            //must share the same drive
            if (!String.Equals(Path.GetPathRoot(currentWorkingDir), Path.GetPathRoot(somePathTemp),
                StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (String.IsNullOrWhiteSpace(currentWorkingDir) || String.IsNullOrWhiteSpace(somePathTemp))
                return false;
            var p = currentWorkingDir.Split(Path.DirectorySeparatorChar);
            var q = somePathTemp.Split(Path.DirectorySeparatorChar);

            var pathOut = new List<string>();

            for (var i = p.Length - 1; i > 0; i--)
            {
                for (var j = q.Length - 1; j > 0; j--)
                {
                    var pVal = p[i];
                    var qVal = q[j];

                    if (!String.Equals(pVal, qVal, StringComparison.OrdinalIgnoreCase)) 
                        continue;

                    for(var m = (p.Length - 1 - i); m > 0; m--)
                        pathOut.Add("..");

                    for(var k = j + 1; k < q.Length; k++)
                        pathOut.Add(q[k]);

                    if(!String.IsNullOrWhiteSpace(somePathFile))
                        pathOut.Add(somePathFile);

                    somePath = String.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), pathOut);

                    somePath = RemoveRedundantPathLeafs(somePath);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Deals with the odd path like ..\NotNeeded\..\ActualFolder\somefile.txt
        /// and ..\Somewhere\.\Andthen\somefile.txt
        /// which is technically valid.
        /// </summary>
        /// <param name="somePath"></param>
        /// <returns></returns>
        public static string RemoveRedundantPathLeafs(string somePath)
        {
            if (String.IsNullOrWhiteSpace(somePath))
                return null;
            const string PATH_SEP = @"\x5c\x2f";
            const string DOT_DOT = @"\x2e\x2e";

            var invalidPathChars = new string(Path.GetInvalidPathChars()).EscapeString();
            
            //anywhere we find a pattern like ..\AnyNameHere\..
            var regexPattern = "(" + DOT_DOT + "[" + PATH_SEP + "]" +
                               "[^" + invalidPathChars + DOT_DOT.EscapeString() + "]+?" +
                               "[" + PATH_SEP + "])" + DOT_DOT;

            string matchText;
            if (RegexCatalog.IsRegexMatch(somePath, regexPattern, out matchText, 1) &&
                !String.Equals(@"..\..\", matchText))
            {
                somePath = somePath.Replace(matchText, String.Empty);
            }

            somePath = somePath.Replace(@"\.\", @"\").Replace("/./", "/");

            return somePath;
        }

        /// <summary>
        /// Asserts that <see cref="somePath"/> has an extension 
        /// which is represented by the lists at <see cref="NfConfig.CodeFileExtensions"/>
        /// or <see cref="NfConfig.ConfigFileExtensions"/> 
        /// or <see cref="NfConfig.BinaryFileExtensions"/>
        /// </summary>
        /// <param name="somePath"></param>
        /// <returns></returns>
        public static bool HasKnownExtension(string somePath)
        {
            var ext = Path.GetExtension(somePath);
            return !String.IsNullOrEmpty(ext) &&
                   (NfConfig.CodeFileExtensions.Select(x => "." + x).Contains(ext) ||
                    NfConfig.ConfigFileExtensions.Select(x => "." + x).Contains(ext) || 
                    NfConfig.BinaryFileExtensions.Select(x => "." + x).Contains(ext));
        }

        /// <summary>
        /// Asserts the <see cref="somePath"/> is a file path
        /// with one of the extensions in <see cref="NfConfig.CodeFileExtensions"/>
        /// </summary>
        /// <param name="somePath"></param>
        /// <returns></returns>
        public static bool IsCodeFileExtension(string somePath)
        {
            return IsExtensionType(somePath, NfConfig.CodeFileExtensions);
        }

        /// <summary>
        /// Asserts the <see cref="somePath"/> is a file path
        /// with one of the extensions in <see cref="NfConfig.ConfigFileExtensions"/>
        /// </summary>
        /// <param name="somePath"></param>
        /// <returns></returns>
        public static bool IsConfigFileExtension(string somePath)
        {
            return IsExtensionType(somePath, NfConfig.ConfigFileExtensions);
        }

        /// <summary>
        /// Asserts the <see cref="somePath"/> is a file path
        /// with one of the extensions in <see cref="NfConfig.BinaryFileExtensions"/>
        /// </summary>
        /// <param name="somePath"></param>
        /// <returns></returns>
        public static bool IsBinaryFileExtension(string somePath)
        {
            return IsExtensionType(somePath, NfConfig.BinaryFileExtensions);
        }

        /// <summary>
        /// Asserts the <see cref="somePath"/> contains at least
        /// one of the values in <see cref="NfConfig.ExcludeCodeDirectories"/>
        /// </summary>
        /// <param name="somePath"></param>
        /// <returns></returns>
        public static bool ContainsExcludeCodeDirectory(string somePath)
        {
            if (String.IsNullOrWhiteSpace(somePath))
                return false;
            return somePath.Contains(new string(new[] {Path.DirectorySeparatorChar})) &&
                   NfConfig.ExcludeCodeDirectories.Any(somePath.Contains);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static bool IsExtensionType(string somePath, string[] possiableExtensions)
        {
            if (String.IsNullOrWhiteSpace(somePath) || possiableExtensions == null)
                return false;
            if(somePath.StartsWith("."))
                return possiableExtensions.Any(x => Regex.IsMatch(somePath, x, RegexOptions.IgnoreCase));
            var ext = Path.GetExtension(somePath);
            return !String.IsNullOrWhiteSpace(ext) &&
                   possiableExtensions.Any(x => Regex.IsMatch(ext, x, RegexOptions.IgnoreCase));
        }

        /// <summary>
        /// Resolves paths containing an environment variable(s).
        /// </summary>
        /// <param name="somePath"></param>
        /// <param name="pathOut"></param>
        /// <returns></returns>
        /// <remarks>
        /// Handle three styles:
        /// (1) MsBuild style (e.g. $(MY_VAR) )
        /// (2) Batch style (e.g. %MY_VAR% )
        /// (3) Powershell style (e.g. $env:MY_VAR )
        /// </remarks>
        public static bool TryResolveEnvVar(string somePath, ref string pathOut)
        { 
            if (String.IsNullOrWhiteSpace(somePath) || (!somePath.Contains("$") && !somePath.Contains("%")))
                return false;

            //just invoke the framework if it returns something
            if (!String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(somePath)))
            {
                pathOut = Environment.GetEnvironmentVariable(somePath);
                return true;
            }
            var lowerSomePath = somePath.ToLower();

            var envVars = Environment.GetEnvironmentVariables();

            Func<string, string> msBuildStyle = s => "$(" + s.ToLower() + ")";
            Func<string, string> psStyle = s => "$env:" + s.ToLower();
            Func<string, string> batStyle = s => "%" + s.ToLower() + "%";

            var tempPath = lowerSomePath;

            foreach (var envVar in envVars.Keys.Cast<string>().ToArray())
            {
                var envVarStyles = new[] {msBuildStyle(envVar), psStyle(envVar), batStyle(envVar)};

                foreach (var envStyle in envVarStyles)
                {
                    if (!lowerSomePath.Contains(envVar)) continue;

                    var resolvedValue = Environment.GetEnvironmentVariable(envVar);
                    tempPath = tempPath.Replace(envStyle, resolvedValue);
                }
            }

            if (tempPath == lowerSomePath) return false;

            pathOut = tempPath;
            return true;
        }

        /// <summary>
        /// Utility method to remove redundant blank lines interleaving one blank between 
        /// each line with content.
        /// </summary>
        /// <param name="fileFullName"></param>
        public static void ToDoubleSpaced(string fileFullName)
        {
            if (String.IsNullOrWhiteSpace(fileFullName) || !File.Exists(fileFullName))
                return;
            var redux = Etc.ToDoubleSpaceLines(File.ReadAllLines(fileFullName));

            File.WriteAllText(fileFullName, redux);
        }

        /// <summary>
        /// Utility method to remove all blank lines from the <see cref="fileFullName"/>
        /// </summary>
        /// <param name="fileFullName"></param>
        public static void ToSingleSpaced(string fileFullName)
        {
            if (String.IsNullOrWhiteSpace(fileFullName) || !File.Exists(fileFullName))
                return;
            var output = new List<string>();
            foreach (var ln in File.ReadAllLines(fileFullName))
            {
                if (String.IsNullOrWhiteSpace(ln))
                    continue;
                output.Add(ln);
            }
            File.WriteAllLines(fileFullName, output);
        }


        /// <summary>
        /// Removes the <see cref="Path.GetInvalidPathChars"/> from <see cref="outFile"/>
        /// </summary>
        /// <param name="outFile"></param>
        /// <returns></returns>
        public static string RemoveInvalidPathChars(string outFile)
        {
            if (String.IsNullOrWhiteSpace(outFile))
                return outFile;

            return Path.GetInvalidPathChars().Aggregate(outFile,
                (current, invalidChar) =>
                    current.Replace(invalidChar.ToString(CultureInfo.InvariantCulture), String.Empty));

        }
    }
}
