using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Util.Gia.Args
{
    public class AssemblyLeftRightArgs
    {
        public string TargetWord { get; set; }
        public System.Reflection.Assembly Assembly { get; set; }
        public int MaxDepth { get; set; }
        public bool SplitNamesOnUpperCase { get; set; }
        public bool JustOnValueTypes { get; set; }
        public string LimitOnThisType { get; set; }
        public string Separator { get; set; }
        public bool UseTypeNames { get; set; }
    }

    public class FlattenTypeArgs
    {
        public const int MAX_DEPTH = 16;
        public const string DEFAULT_SEPARATOR = "-";
        
        private string _separator = DEFAULT_SEPARATOR;
        private int _depth = 16;
        
        public System.Reflection.Assembly Assembly { get; set; }
        public bool UseTypeNames { get; set; }
        public string TypeFullName { get; set; }
        public string Separator
        {
            get { return _separator; }
            set { _separator = value; }
        }
        public int Depth
        {
            get { return _depth; }
            set
            {
                if (_depth > MAX_DEPTH)
                    value = MAX_DEPTH;
                if (_depth < 0)
                    value = 0;
                _depth = value;
            }
        }
        public string LimitOnThisType { get; set; }
        public bool DisplayEnums { get; set; }
    }
}
