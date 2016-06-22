using System;
using System.Runtime.Serialization;

namespace NoFuture.Shared.DiaSdk.LinesSwitch
{
    /// <summary>
    /// The result object from Dia2Dump.exe using the <see cref="Dia2DumpAllSwitches.Lines"/> switch
    /// </summary>
    [Serializable]
    [DataContract]
    public class PdbAllLines : IPdbJsonDeserializedTypes
    {
        [DataMember]
        public PdbCompiland[] allLines;
    }

    /// <summary>
    /// The result object from Dia2Dump.exe using the <see cref="Dia2DumpSingleSwitches.TypeFullName"/> switch
    /// </summary>
    [Serializable]
    [DataContract]
    public class PdbCompiland : IPdbJsonDeserializedTypes
    {
        [DataMember]
        public string moduleName;
        [DataMember]
        public ModuleSymbols[] moduleSymbols;
    }

    [Serializable]
    [DataContract]
    public class ModuleSymbols
    {
        [DataMember]
        public string symbolName;
        [DataMember]
        public int symIndexId;
        [DataMember]
        public string[] locals;
        [DataMember]
        public string file;
        [DataMember]
        public PdbLineNumber firstLine;
        [DataMember]
        public PdbLineNumber lastLine;
    }

    [Serializable]
    [DataContract]
    public class PdbLineNumber
    {
        [DataMember]
        public int lineNumber;
        [DataMember]
        public string relativeVirtualAddress;
        [DataMember]
        public string section;
        [DataMember]
        public string offset;
        [DataMember]
        public int length;

        public override string ToString()
        {
            return lineNumber.ToString();
        }
    }
}

namespace NoFuture.Shared.DiaSdk.GlobalsSwtich
{

    /// <summary>
    /// The result object from Dia2Dump.exe using the <see cref="Dia2DumpAllSwitches.Globals"/> switch
    /// </summary>
    [Serializable]
    [DataContract]
    public class PdbAllGlobals : IPdbJsonDeserializedTypes
    {
        [DataMember]
        public PdbGlobal[] globals;
    }

    [Serializable]
    [DataContract]
    public class PdbGlobal
    {
        [DataMember]
        public string name;
        [DataMember]
        public string type;
        [DataMember]
        public string relativeVirtualAddress;
        [DataMember]
        public string section;
        [DataMember]
        public string offset;
    }
}

namespace NoFuture.Shared.DiaSdk.SectionSwitch
{
    /// <summary>
    /// The result object from Dia2Dump.exe using the <see cref="Dia2DumpAllSwitches.Sections"/> switch
    /// </summary>
    [Serializable]
    [DataContract]
    public class PdbAllSections : IPdbJsonDeserializedTypes
    {
        [DataMember]
        public PdbSection[] sectionContribution;
    }

    [Serializable]
    [DataContract]
    public class PdbSection
    {
        [DataMember]
        public string relativeVirtualAddress;
        [DataMember]
        public string section;
        [DataMember]
        public string offset;
        [DataMember]
        public string length;
        [DataMember]
        public string name;
    }
}

namespace NoFuture.Shared.DiaSdk.FilesSwitch
{
    /// <summary>
    /// The result object from Dia2Dump.exe using the <see cref="Dia2DumpAllSwitches.Files"/> switch
    /// </summary>
    [Serializable]
    [DataContract]
    public class PdbAllFiles : IPdbJsonDeserializedTypes
    {
        [DataMember]
        public PdbFile[] files;
    }

    [Serializable]
    [DataContract]
    public class PdbFile
    {
        [DataMember]
        public string moduleName;
        [DataMember]
        public string fileName;
    }
}

namespace NoFuture.Shared.DiaSdk.SymbolsSwitch
{
    /// <summary>
    /// The result object from Dia2Dump.exe using the <see cref="Dia2DumpAllSwitches.Symbols"/> switch
    /// </summary>
    [Serializable]
    [DataContract]
    public class PdbAllSymbols : IPdbJsonDeserializedTypes
    {
        [DataMember]
        public PdbModuleName[] symbols;
    }

    [Serializable]
    [DataContract]
    public class PdbModuleName
    {
        [DataMember]
        public string moduleName;
        [DataMember]
        public PdbModuleSymbol[] moduleSymbols;
    }

    [Serializable]
    [DataContract]
    public class PdbModuleSymbol
    {
        [DataMember]
        public string name;
        [DataMember(Name = "In MetaData")]
        public PdbInMetadata inMetadata;
        [DataMember]
        public string length;
        [DataMember]
        public string undName;
    }

    [Serializable]
    [DataContract]
    public class PdbInMetadata
    {
        [DataMember]
        public string relativeVirtualAddress;
        [DataMember]
        public string section;
        [DataMember]
        public string offset;
    }
}

namespace NoFuture.Shared.DiaSdk.ModulesSwitch
{
    /// <summary>
    /// The result object from Dia2Dump.exe using the <see cref="Dia2DumpAllSwitches.Modules"/> switch
    /// </summary>
    [Serializable]
    [DataContract]
    public class PdbAllModules : IPdbJsonDeserializedTypes
    {
        [DataMember]
        public PdbModule[] modules;
    }

    [Serializable]
    [DataContract]
    public class PdbModule
    {
        [DataMember]
        public string id;
        [DataMember]
        public string name;
    }
}