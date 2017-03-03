using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using NoFuture.Exceptions;

namespace NoFuture.Shared.DiaSdk
{
    /// <summary>
    /// The switch values which may be used in invocation's
    /// to Dia2Dump.exe (the modified version which is '
    /// apart of this assembly) for which valid JSON
    /// syntax will be returned on the standard output.
    /// </summary>
    public static class Dia2DumpAllSwitches
    {
        public const string Modules = "-m";
        public const string Globals = "-g";
        public const string Files = "-f";
        public const string Lines = "-l";
        public const string Symbols = "-s";
        public const string Sections = "-c";
    }

    /// <summary>
    /// The switch values which may be used in invocation's
    /// to Dia2Dump.exe (the modified version which is '
    /// apart of this assembly) for which valid JSON
    /// syntax will be returned on the standard output.
    /// </summary>
    public static class Dia2DumpSingleSwitches
    {
        public const string TypeFullName = "-compiland";
    }

    /// <summary>
    /// A type specifier for the variety of deserialized types from 
    /// the JSON result of calls to Dia2Dump.exe
    /// </summary>
    public interface IPdbJsonDeserializedTypes { }

    #region by single RVA

    /// <summary>
    /// Interface for DiaSdk types whose json data is small enough to 
    /// be passed in directly (as compared to <see cref="IPdbJsonDataFile"/>
    /// which handles very large 'dump all' json data).
    /// </summary>
    public interface IPdbJsonDataString
    {
        IPdbJsonDeserializedTypes GetData(string jsonData, PdbJsonDataFormat jsonDataFormat);
    }

    /// <summary>
    /// Used to describe the state of some Pdb Json 
    /// data.  When first returned from Dia2Dump.exe's
    /// standard output the <see cref="Path.DirectorySeparatorChar"/>
    /// is NOT escaped.
    /// </summary>
    public enum PdbJsonDataFormat
    {
        RawString,
        BackslashDoubled
    }

    /// <summary>
    /// Base implementation to immediately derserialize data from Dia2Dump.exe's standard output.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PdbJsonDataString<T> : IPdbJsonDataString
    {
        public abstract IPdbJsonDeserializedTypes GetData(string jsonData, PdbJsonDataFormat jsonDataFormat);
        protected virtual T GetDataByPdbType(string jsonData, PdbJsonDataFormat jsonDataFormat)
        {
            var dblSlash = Encoding.UTF8.GetString(new byte[] { 0x5C, 0x5C });
            var singleSlash = Encoding.UTF8.GetString(new byte[] { 0x5C });
            
            if (jsonDataFormat == PdbJsonDataFormat.RawString)
                jsonData = jsonData.Replace(singleSlash, dblSlash);

            var data = Encoding.UTF8.GetBytes(jsonData);
            var jsonSerializer = new DataContractJsonSerializer(typeof(T));
            using (var ms = new MemoryStream(data))
            {
                return (T)jsonSerializer.ReadObject(ms);
            }
        }
    }

    /// <summary>
    /// The data representing a single type.
    /// </summary>
    public class CompilandJsonData : PdbJsonDataString<LinesSwitch.PdbCompiland>
    {
        public override IPdbJsonDeserializedTypes GetData(string jsonData, PdbJsonDataFormat jsonDataFormat)
        {
            //the base type's GetDataByPdbType cannot return null so we check it in every extended type
            return string.IsNullOrWhiteSpace(jsonData) ? null : GetDataByPdbType(jsonData, jsonDataFormat);
        }

        /// <summary>
        /// Factory method to skip instantiating <see cref="CompilandJsonData"/>
        /// and just skip straight to the data.
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="jsonDataFormat"></param>
        /// <returns></returns>
        public static LinesSwitch.PdbCompiland Parse(string jsonData, PdbJsonDataFormat jsonDataFormat)
        {
            var cjd = new CompilandJsonData();
            return (LinesSwitch.PdbCompiland)cjd.GetData(jsonData, jsonDataFormat);
        }
    }

    #endregion

    #region dump all

    /// <summary>
    /// Represents the basic API for any type which 
    /// derserializes data from files which where written
    /// to disk by an invocation to Dia2Dump.exe
    /// </summary>
    public interface IPdbJsonDataFile
    {
        string FileName { get; set; }
        string FullFileName { get;}
        IPdbJsonDeserializedTypes GetData();
    }

    /// <summary>
    /// Base implementation of derserialized data from Dia2Dump.exe 
    /// whose content is written to file.
    /// </summary>
    /// <typeparam name="T">
    /// One of the various implementation of the <see cref="DataContract"/>
    /// decorated types which act as the class containers for json data.
    /// </typeparam>
    public abstract class PdbJsonDataFileBase<T> : IPdbJsonDataFile
    {
        protected string fileName;
        protected string directoryPath = NfConfig.TempDirectories.Code;

        /// <summary>
        /// The concrete implementor must specify a default file name.
        /// </summary>
        /// <param name="defaultFileName"></param>
        protected PdbJsonDataFileBase(string defaultFileName)
        {
            fileName = defaultFileName;
        }

        /// <summary>
        /// The calling assembly is expected to assign the
        /// <see cref="DirectoryPath"/> and <see cref="FileName"/>
        /// </summary>
        protected PdbJsonDataFileBase() { }

        /// <summary>
        /// This receives a default value based on the concrete implemenation.
        /// </summary>
        public virtual string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// This will default to <see cref="NfConfig.TempDirectories.Code"/>
        /// </summary>
        public virtual string DirectoryPath
        {
            get { return directoryPath; }
            set { directoryPath = value; }
        }

        /// <summary>
        /// This is a <see cref="Path.Combine(string, string)"/> of 
        /// <see cref="DirectoryPath"/> and <see cref="FileName"/>
        /// An exception occurs if the <see cref="DirectoryPath"/> is null or empty.
        /// </summary>
        public virtual string FullFileName
        {
            get
            {
                if(string.IsNullOrWhiteSpace(DirectoryPath))
                    throw new ItsDeadJim("The DirectoryPath has no default value set.");

                return Path.Combine(NfConfig.TempDirectories.Code, FileName);
            }
        }

        public abstract IPdbJsonDeserializedTypes GetData();

        private T _myData;

        protected virtual T GetDataByPdbType()
        {
            if (!File.Exists(FullFileName))
                throw new RahRowRagee(string.Format("There is not file from which to read at '{0}'.",FullFileName));

            if (!object.Equals(_myData, null))
                return _myData;

            var jsonSerializer = new DataContractJsonSerializer(typeof(T));
            _myData = (T)jsonSerializer.ReadObject(File.OpenRead(FullFileName));
            return _myData;
        }
    }

    /// <summary>
    /// This is the concrete implementation for the modules switch to Dia2Dump
    /// </summary>
    public class AllModulesJsonDataFile : PdbJsonDataFileBase<ModulesSwitch.PdbAllModules>
    {
        public AllModulesJsonDataFile(string outFile)
        {
            directoryPath = Path.GetDirectoryName(outFile);
            fileName = Path.GetFileName(outFile);
        }
        public AllModulesJsonDataFile() : base("diaSdkData.modules.json") { }
        public override IPdbJsonDeserializedTypes GetData() { return GetDataByPdbType(); }
    }

    /// <summary>
    /// This is the concrete implementation for the globals switch to Dia2Dump
    /// </summary>
    public class AllGlobalsJsonDataFile : PdbJsonDataFileBase<GlobalsSwtich.PdbAllGlobals>
    {
        public AllGlobalsJsonDataFile(string outFile)
        {
            directoryPath = Path.GetDirectoryName(outFile);
            fileName = Path.GetFileName(outFile);
        }
        public AllGlobalsJsonDataFile() : base("diaSdkData.globals.json") { }
        public override IPdbJsonDeserializedTypes GetData() { return GetDataByPdbType(); }
    }

    /// <summary>
    /// This is the concrete implementation for the files switch to Dia2Dump
    /// </summary>
    public class AllFilesJsonDataFile : PdbJsonDataFileBase<FilesSwitch.PdbAllFiles> 
    {
        public AllFilesJsonDataFile(string outFile)
        {
            directoryPath = Path.GetDirectoryName(outFile);
            fileName = Path.GetFileName(outFile);
        }
        public AllFilesJsonDataFile() : base("diaSdkData.files.json") { }
        public override IPdbJsonDeserializedTypes GetData() { return GetDataByPdbType(); }
    }

    /// <summary>
    /// This is the concrete implementation for the all lines switch to Dia2Dump
    /// </summary>
    public class AllLinesJsonDataFile : PdbJsonDataFileBase<LinesSwitch.PdbAllLines>
    {
        public AllLinesJsonDataFile(string outFile)
        {
            directoryPath = Path.GetDirectoryName(outFile);
            fileName = Path.GetFileName(outFile);
        }
        public AllLinesJsonDataFile() : base("diaSdkData.lines.json") { }
        public override IPdbJsonDeserializedTypes GetData() { return GetDataByPdbType(); }
    }

    /// <summary>
    /// This is the concrete implementation for the symbols switch to Dia2Dump
    /// </summary>
    public class AllSymbolsJsonDataFile : PdbJsonDataFileBase<SymbolsSwitch.PdbAllSymbols> 
    {
        public AllSymbolsJsonDataFile(string outFile)
        {
            directoryPath = Path.GetDirectoryName(outFile);
            fileName = Path.GetFileName(outFile);
        }
        public AllSymbolsJsonDataFile() : base("diaSdkData.symbols.json") { }
        public override IPdbJsonDeserializedTypes GetData() { return GetDataByPdbType(); }
    }

    /// <summary>
    /// This is the concrete implementation for the sections switch to Dia2Dump
    /// </summary>
    public class AllSectionsJsonDataFile : PdbJsonDataFileBase<SectionSwitch.PdbAllSections> 
    {
        public AllSectionsJsonDataFile(string outFile)
        {
            directoryPath = Path.GetDirectoryName(outFile);
            fileName = Path.GetFileName(outFile);
        }
        public AllSectionsJsonDataFile() : base("diaSdkData.sections.json") { }
        public override IPdbJsonDeserializedTypes GetData() { return GetDataByPdbType(); }
    }

    /// <summary>
    /// A container type used to strongly define the various implementations of <see cref="IPdbJsonDataFile"/>.
    /// </summary>
    public class DumpAll
    {
        public AllLinesJsonDataFile Lines;
        public AllGlobalsJsonDataFile Globals;
        public AllSectionsJsonDataFile Sections;
        public AllFilesJsonDataFile Files;
        public AllSymbolsJsonDataFile Symbols;
        public AllModulesJsonDataFile Modules;
    }

    #endregion
}
