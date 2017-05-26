using System;
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
        public const string ASM_ADJ_GRAPH = "asmAdjGraph";
        public const string ASM_OBJ_OUTLINE_NS = "withNamespaceOutline";
        public const string INVOKE_GRAPHVIZ_FLATTEN_MAX_DEPTH = "nfMaxDepth";
        #endregion

        #region settings

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

        public static ILangStyle LangStyle => _currentStyle;

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
        /// A Global switch which 'eats' any FileNotFoundException's 
        /// while invoking the 'ReturnType' on any <see cref="System.Reflection.MemberInfo"/>.
        /// </summary>
        public static bool IgnoreReflectionMissingAsmError { get; set; } = true;

        #endregion
    }
}
