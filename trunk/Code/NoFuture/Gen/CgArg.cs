using System;
using System.Reflection;
using NoFuture.Util;

namespace NoFuture.Gen
{
    /// <summary>
    /// For code generation of <see cref="MethodInfo"/> parameters.
    /// </summary>
    [Serializable]
    public class CgArg
    {
        public string ArgType { get; set; }
        public string ArgName { get; set; }

        public virtual bool Equals(CgArg arg)
        {
            return arg.ArgName == ArgName && arg.ArgType == ArgType;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", ArgType, ArgName);
        }

        public string AsInvokeRegexPattern
        {
            get { return @"([^\,]+?)"; }
        }

        public string ToGraphVizString()
        {
            return TypeName.GetTypeNameWithoutNamespace(ArgType);
        }
    }
}
