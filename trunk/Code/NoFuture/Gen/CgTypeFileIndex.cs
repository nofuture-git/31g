using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using NoFuture.Exceptions;

namespace NoFuture.Gen
{
    public static class NameTransposition
    {
        internal static MD5 Md5 = MD5.Create();

        internal static string Md5Hash(this string someString)
        {
           if(string.IsNullOrWhiteSpace(someString)) 
               throw new ArgumentNullException("someString");

            var hashstring = new StringBuilder();
            var hashbytes = Md5.ComputeHash(Encoding.UTF8.GetBytes(someString));
            foreach (var b in hashbytes)
                hashstring.Append(b.ToString("x2"));

            return hashstring.ToString();
        }
    }

    /// <summary>
    /// Represents the index of the <see cref="CgTypeFiles"/>.
    /// Due to the problem of full assembly-qualified names being greater than the 
    /// 260 char max for file names.
    /// </summary>
    [Serializable]
    public class CgTypeFileIndex
    {
        #region ctors
        public CgTypeFileIndex() { PdbFilesHash = new Dictionary<string, PdbTargetLine>();}
        #endregion

        #region properties
        public string MyLocation { get; set; }
        public string OriginalSrcCodeFile { get; set; }
        public string AssemblyQualifiedTypeName { get; set; }
        public Dictionary<string, PdbTargetLine> PdbFilesHash { get; set; }

        /// <summary>
        /// Gets just the start and end line numbers from the <see cref="PdbFilesHash"/>
        /// </summary>
        public List<Tuple<int, int>> LineNumbers
        {
            get
            {
                if (PdbFilesHash == null || PdbFilesHash.Count <= 0)
                    return null;
                return PdbFilesHash.Keys.Select(n => PdbFilesHash[n])
                    .Select(p => new Tuple<int, int>(p.StartAt, p.EndAt))
                    .ToList();
            }
        }

        #endregion

        #region private constants

        private const string NF = "noFuture";
        private const string GEN = "gen";
        private const string CG_TYPE_FILE_INDEX = "cgTypeFileIndex";
        private const string ORIG_SRC_CODE_FILE = "originalSrcCodeFile";
        private const string ASM_QUAL_FULL_NAME = "assemblyQualifiedTypeName";
        private const string PDB_FILES_HASH = "pdbFilesHash";
        private const string ENTRY = "entry";
        private const string PDB_TARGET_LINE = "pdbTargetLine";
        private const string INDEX_ID_ATTR = "indexId";
        private const string START_AT_ATTR = "startAt";
        private const string END_AT_ATTR = "endAt";

        #endregion

        /// <summary>
        /// Writes this instance to disk
        /// </summary>
        public void WriteIndexToFile(string symbolFolder)
        {
            if(string.IsNullOrWhiteSpace(symbolFolder))
                throw new ArgumentNullException("symbolFolder");
            if(!Directory.Exists(symbolFolder))
                throw new DirectoryNotFoundException(string.Format("no such directory '{0}'",symbolFolder));

            var doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));
            var orgSrcFileNode = doc.CreateElement(ORIG_SRC_CODE_FILE);
            orgSrcFileNode.InnerText = OriginalSrcCodeFile;
            var asmQualNameNode = doc.CreateElement(ASM_QUAL_FULL_NAME);
            asmQualNameNode.InnerText = AssemblyQualifiedTypeName;

            var pdbFilesHashNode = doc.CreateElement(PDB_FILES_HASH);
            foreach (var pdbKey in PdbFilesHash.Keys)
            {
                var entryNode = doc.CreateElement(ENTRY);
                var entryIdAttr = doc.CreateAttribute("id");
                entryIdAttr.Value = pdbKey;
                entryNode.Attributes.Append(entryIdAttr);
                var pdbTarget = PdbFilesHash[pdbKey];

                var pdbLineNode = doc.CreateElement(PDB_TARGET_LINE);
                var pdbIdxAttr = doc.CreateAttribute(INDEX_ID_ATTR);
                pdbIdxAttr.Value = pdbTarget.IndexId.ToString(CultureInfo.InvariantCulture);
                var pdbStartAttr = doc.CreateAttribute(START_AT_ATTR);
                pdbStartAttr.Value = pdbTarget.StartAt.ToString(CultureInfo.InvariantCulture);
                var pdbEndAttr = doc.CreateAttribute(END_AT_ATTR);
                pdbEndAttr.Value = pdbTarget.EndAt.ToString(CultureInfo.InvariantCulture);

                pdbLineNode.Attributes.Append(pdbIdxAttr);
                pdbLineNode.Attributes.Append(pdbStartAttr);
                pdbLineNode.Attributes.Append(pdbEndAttr);

                var memberNameCdata = doc.CreateCDataSection(pdbTarget.MemberName);
                pdbLineNode.AppendChild(memberNameCdata);

                entryNode.AppendChild(pdbLineNode);

                pdbFilesHashNode.AppendChild(entryNode);
            }

            var cgFileIdxNode = doc.CreateElement(CG_TYPE_FILE_INDEX);
            cgFileIdxNode.AppendChild(orgSrcFileNode);
            cgFileIdxNode.AppendChild(asmQualNameNode);
            cgFileIdxNode.AppendChild(pdbFilesHashNode);

            var genNode = doc.CreateElement(GEN);
            genNode.AppendChild(cgFileIdxNode);

            var nfRoot = doc.CreateElement(NF);
            nfRoot.AppendChild(genNode);
            doc.AppendChild(nfRoot);

            var xmlFilePath = Path.Combine(symbolFolder, Settings.DefaultFileIndexName);
            if(File.Exists(xmlFilePath))
                File.Delete(xmlFilePath);

            using (var xmlWriter = new XmlTextWriter(xmlFilePath, Encoding.UTF8) {Formatting = Formatting.Indented})
            {
                doc.WriteContentTo(xmlWriter);
                xmlWriter.Flush();
            }
            MyLocation = xmlFilePath;
        }

        /// <summary>
        /// Operates as the counterpart of <see cref="WriteIndexToFile"/> allow this object to 
        /// be reconstituted from a file.
        /// </summary>
        /// <returns></returns>
        public static CgTypeFileIndex ReadIndexFromFile(string nfCgTypeFileIndexPath)
        {
            if (String.IsNullOrWhiteSpace(nfCgTypeFileIndexPath))
                throw new ArgumentNullException(nfCgTypeFileIndexPath);
            if(!File.Exists(nfCgTypeFileIndexPath))
                throw new FileNotFoundException(string.Format("no such file at '{0}'",nfCgTypeFileIndexPath));
            
            var cgTypeFileOut = new CgTypeFileIndex {MyLocation = nfCgTypeFileIndexPath};
            var symbolFolder = Path.GetDirectoryName(cgTypeFileOut.MyLocation);

            using (var xmlFile = File.OpenRead(nfCgTypeFileIndexPath))
            {
                using (var xmlRdr = new XmlTextReader(xmlFile))
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(xmlRdr);
                    var orgSrcFileNode = xmlDoc.SelectSingleNode(string.Format("//{0}", ORIG_SRC_CODE_FILE));
                    if(orgSrcFileNode == null)
                        throw new ItsDeadJim(string.Format("couldn't locate the xml node named '{0}'",ORIG_SRC_CODE_FILE));
                    var orgSrcFileTxt = orgSrcFileNode.InnerText;
                    if(string.IsNullOrWhiteSpace(orgSrcFileTxt))
                        throw new ItsDeadJim(string.Format("the xml node named '{0}' has no path in its text section", ORIG_SRC_CODE_FILE));

                    cgTypeFileOut.OriginalSrcCodeFile = orgSrcFileTxt;

                    var asmQualNameNode = xmlDoc.SelectSingleNode(string.Format("//{0}", ASM_QUAL_FULL_NAME));
                    if (asmQualNameNode == null)
                        throw new ItsDeadJim(string.Format("couldn't locate the xml node named '{0}'",
                            ASM_QUAL_FULL_NAME));
                    var asmQualNameTxt = asmQualNameNode.InnerText;
                    if (string.IsNullOrWhiteSpace(asmQualNameTxt))
                        throw new ItsDeadJim(string.Format("the xml node named '{0}' has no path in its text section",
                            ASM_QUAL_FULL_NAME));

                    cgTypeFileOut.AssemblyQualifiedTypeName = asmQualNameTxt;
                    var owningTypeName = (new Util.TypeName(asmQualNameTxt)).FullName;

                    var entryXpath = string.Format("//{0}/{1}", PDB_FILES_HASH, ENTRY);
                    var entryNodes = xmlDoc.SelectNodes(entryXpath);
                    if (entryNodes == null || entryNodes.Count <= 0)
                        throw new ItsDeadJim(string.Format("there aren't any nodes at the xpath '{0}'", entryXpath));
                    foreach (XmlElement node in entryNodes)
                    {
                        var hashIdAttr = node.Attributes["id"];
                        if (hashIdAttr == null)
                            throw new ItsDeadJim(
                                string.Format("there is a node in the collection at xpath '{0}' missing an id attr",
                                    entryXpath));
                        var hashId = hashIdAttr.Value;
                        if (string.IsNullOrWhiteSpace(hashId))
                            throw new ItsDeadJim(
                                string.Format(
                                    "there is a node in the collection at xpath '{0}' with an empty id value",
                                    entryXpath));

                        var pdbTlineNode = node.FirstChild;
                        if (pdbTlineNode == null)
                            throw new ItsDeadJim(
                                string.Format(
                                    "there is a node in the collection at xpath '{0}' by id '{1}' having no child nodes",
                                    entryXpath, hashId));
                        if (pdbTlineNode.Attributes == null)
                            throw new ItsDeadJim(
                                string.Format(
                                    "there is a node in the collection at xpath '{0}' by id '{1}' whose first child node has no attributes",
                                    entryXpath, hashId));

                        //test that the indexId is present, assigned and is an integer
                        var pdbTLineIdxAttr = pdbTlineNode.Attributes[INDEX_ID_ATTR];
                        if (pdbTLineIdxAttr == null)
                            throw new ItsDeadJim(
                                string.Format(
                                    "there is a node in the collection at xpath '{0}' by id '{1}' whose first child node is missing the '{2}' attr",
                                    entryXpath, hashId, INDEX_ID_ATTR));

                        var pdbTLineIdxTxt = pdbTLineIdxAttr.Value;
                        if(string.IsNullOrWhiteSpace(pdbTLineIdxTxt))
                            throw new ItsDeadJim(
                                string.Format(
                                    "there is a node in the collection at xpath '{0}' by id '{1}' whose first child node's attr value '{2}' is empty",
                                    entryXpath, hashId, INDEX_ID_ATTR));

                        int indexIdOut;
                        if (!int.TryParse(pdbTLineIdxTxt, out indexIdOut))
                            throw new ItsDeadJim(
                                string.Format(
                                    "the entry node by id '{0}' has its first child's attr named {1} whose value is not an integer",
                                    hashId, INDEX_ID_ATTR));

                        //test that the startAt is present, assigned and is an integer
                        var pdbTLineStartAtAttr = pdbTlineNode.Attributes[START_AT_ATTR];
                        if (pdbTLineStartAtAttr == null)
                            throw new ItsDeadJim(
                                string.Format(
                                    "there is a node in the collection at xpath '{0}' by id '{1}' whose first child node is missing the '{2}' attr",
                                    entryXpath, hashId, START_AT_ATTR));

                        var pdbTLineStartAtTxt = pdbTLineStartAtAttr.Value;
                        if (string.IsNullOrWhiteSpace(pdbTLineStartAtTxt))
                            throw new ItsDeadJim(
                                string.Format(
                                    "there is a node in the collection at xpath '{0}' by id '{1}' whose first child node's attr value '{2}' is empty",
                                    entryXpath, hashId, START_AT_ATTR));


                        int startAtOut;
                        if(!int.TryParse(pdbTLineStartAtTxt, out startAtOut))
                            throw new ItsDeadJim(
                                string.Format(
                                    "there is a node in the collection at xpath '{0}' by id '{1}' whose first child node's attr value '{2}' of '{3}' isn't an integer",
                                    entryXpath, hashId, START_AT_ATTR, pdbTLineStartAtTxt));

                        //test that the endAt is present, assigned and is an integer
                        var pdbTLineEndAtAttr = pdbTlineNode.Attributes[END_AT_ATTR];
                        if (pdbTLineEndAtAttr == null)
                            throw new ItsDeadJim(
                                string.Format(
                                    "there is a node in the collection at xpath '{0}' by id '{1}' whose first child node is missing the '{2}' attr",
                                    entryXpath, hashId, END_AT_ATTR));

                        var pdbTLineEndAtTxt = pdbTLineEndAtAttr.Value;
                        if (string.IsNullOrWhiteSpace(pdbTLineEndAtTxt))
                            throw new ItsDeadJim(
                                string.Format(
                                    "there is a node in the collection at xpath '{0}' by id '{1}' whose first child node's attr value '{2}' is empty",
                                    entryXpath, hashId, END_AT_ATTR));

                        int endAtOut;
                        if (!int.TryParse(pdbTLineEndAtTxt, out endAtOut))
                            throw new ItsDeadJim(
                                string.Format(
                                    "there is a node in the collection at xpath '{0}' by id '{1}' whose first child node's attr value '{2}' of '{3}' isn't an integer",
                                    entryXpath, hashId, END_AT_ATTR, pdbTLineEndAtTxt));

                        //test that there is a member name in the CDATA section and its not empty
                        var pdbTLineCdataNode = pdbTlineNode.FirstChild;
                        if (pdbTLineCdataNode == null)
                            throw new ItsDeadJim(
                                string.Format(
                                    "The entry node by id '{0}' was expected to itself have a single cdata child node but its missing",
                                    hashId));

                        var memberName = pdbTLineCdataNode.InnerText;
                        if (string.IsNullOrWhiteSpace(memberName))
                            throw new ItsDeadJim(
                                string.Format(
                                    "The entry node by id '{0}' was expected to itself have a single cdata child node but its empty",
                                    hashId));

                        var pdbTLine = new PdbTargetLine
                        {
                            StartAt = startAtOut,
                            EndAt = endAtOut,
                            IndexId = indexIdOut,
                            MemberName = memberName,
                            OwningTypeFullName = owningTypeName,
                            SymbolFolder = symbolFolder
                        };

                        cgTypeFileOut.PdbFilesHash.Add(hashId, pdbTLine);
                    }
                }
            }
            return cgTypeFileOut;
        }

        /// <summary>
        /// Finds the <see cref="PdbTargetLine"/> 
        /// where <see cref="lineNumber"/> falls or 
        /// between <see cref="PdbTargetLine.StartAt"/> 
        /// and<see cref="PdbTargetLine.EndAt"/>
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <param name="pdbTargetOut"></param>
        /// <returns>True when a match was found, false otherwise.</returns>
        public bool TryFindPdbTargetLine(int lineNumber, out PdbTargetLine pdbTargetOut)
        {
            pdbTargetOut = null;

            var ff = PdbFilesHash.FirstOrDefault(x => x.Value.StartAt <= lineNumber && x.Value.EndAt >= lineNumber);
            if (ff.Value == null)
                return false;
            pdbTargetOut = ff.Value;

            return pdbTargetOut != null;
        }
    }

}
