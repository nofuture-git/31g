using System;
using System.Security.Cryptography;
using NoFuture.Exceptions;
using NoFuture.Gen.LangRules;

namespace NoFuture.Gen
{
    /// <summary>
    /// The code generation overall settings.
    /// </summary>
    public class Settings
    {
        #region constants
        public const string INVOKE_ASM_PATH_SWITCH = "asmPath";
        public const string INVOKE_FULL_TYPE_NAME_SWITCH = "typeName";
        public const string INVOKE_GRAPHVIZ_DIAGRAM_TYPE = "nfGraphType";
        public const string INVOKE_GRAPHVIZ_FLATTENED_LIMIT_TYPE = "nfTypesNamed";
        public const string INVOKE_GRAPHVIZ_DISPLAY_ENUMS = "nfDisplayEnums";
        public const string INVOKE_RESOLVE_ALL_DEPENDENCIES = "nfResolveDependencies";
        public const string CLASS_DIAGRAM = "classDia";
        public const string FLATTENED_DIAGRAM = "flattened";
        public const string ASM_OBJ_GRAPH_DIAGRAM = "asmObjGraph";
        #endregion

        #region settings
        private static string _pdbSourceRefFileName = "__sourcePath.txt";

        private static int _pdbLinesStartAddition = -1;

        private static string _defaultFileIndexName = "__nf.gen.directory.xml";

        private static string _pdbLinesExtension = "pdbLines";

        private static string _usingExtension = "using";

        private static CgLangs _defaultLang = CgLangs.Cs;

        private static ILangStyle _currentStyle = new Cs();

        public static CgLangs DefaultLang
        {
            get { return _defaultLang; }
            set
            {
                switch (_defaultLang)
                {
                    case CgLangs.Cs:
                        _currentStyle = new Cs();
                        break;
                    default:
                        throw new NotImplementedException();
                }
                _defaultLang = value;
            }
        }

        public static ILangStyle LangStyle
        {
            get { return _currentStyle; }
        }

        public static string NoImplementationDefault
        {
            get
            {
                switch (DefaultLang)
                {
                    case CgLangs.Cs:
                        return LangStyle.NoImplementationDefault;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Within directories containing files and or folder created by <see cref="CgTypeFiles"/>
        /// there is expected to be exactly one file by this name.
        /// </summary>
        public static string DefaultFileIndexName
        {
            get { return _defaultFileIndexName; }
            set { _defaultFileIndexName = value; }
        }

        /// <summary>
        /// This is a global setting which will be added to any <see cref="PdbTargetLine.StartAt"/>
        /// value prior to being used to parse line from the original source code file.
        /// </summary>
        /// <remarks>
        /// This performs a useful purpose in that Pdb Lines will often not include 
        /// the opening curly brace but will almost always include the closing one.
        /// </remarks>
        public static int PdbLinesStartAtAddition 
        {
            get { return _pdbLinesStartAddition; } 
            set { _pdbLinesStartAddition = value;} 
        }
        /// <summary>
        /// This is a global setting which will be added to any <see cref="PdbTargetLine.EndAt"/>
        /// value prior to being used to parse line from the original source code file.
        /// </summary>
        /// <remarks>
        /// This performs a useful purpose in that Pdb Lines will often not include 
        /// the opening curly brace but will almost always include the closing one.
        /// </remarks>
        public static int PdbLinesEndAtAddition { get; set; }

        /// <summary>
        /// This is a file name of where .pdbLines files were sourced from.
        /// </summary>
        public static string PdbSourceRefFileName
        {
            get { return _pdbSourceRefFileName; }
            set { _pdbSourceRefFileName = value; }
        }

        private static bool _eatMissingAsmError = true;

        /// <summary>
        /// A Global switch which 'eats' any FileNotFoundException's 
        /// while invoking the 'ReturnType' on any <see cref="System.Reflection.MemberInfo"/>.
        /// </summary>
        public static bool IgnoreReflectionMissingAsmError
        {
            get { return _eatMissingAsmError; }
            set { _eatMissingAsmError = value; }
        }

        /// <summary>
        /// The file extension for files generated using <see cref="CgTypeFiles"/>
        /// </summary>
        public static string PdbLinesExtension
        {
            get { return _pdbLinesExtension; }
            set { _pdbLinesExtension = value; }
        }

        /// <summary>
        /// The extension containing the original source code file's "using" statements 
        /// having been parsed by <see cref="CgTypeFiles"/>
        /// </summary>
        public static string PdbUsingStmtExtension
        {
            get { return _usingExtension; }
            set { _usingExtension = value; }
        }
        #endregion
    }
}
