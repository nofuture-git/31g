using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Util.Gia.Args
{
    /// <summary>
    /// Represent the args needed when flattening.
    /// </summary>
    public class FlattenLineArgs
    {
        private string _separator = DEFAULT_SEPARATOR;
        private int _depth = 16;

        public const int MAX_DEPTH = 16;
        public const string DEFAULT_SEPARATOR = "-";

        public System.Reflection.Assembly Assembly { get; set; }
        public bool UseTypeNames { get; set; }

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

    /// <summary>
    /// Command arg for <see cref="NoFuture.Util.Gia.Flatten"/>
    /// </summary>
    public class FlattenTypeArgs : FlattenLineArgs
    {
        public string TypeFullName { get; set; }
    }
}
