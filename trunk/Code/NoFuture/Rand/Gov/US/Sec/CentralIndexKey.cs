using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.Sec
{
    /// <summary>
    /// A unique id assigned by the SEC
    /// </summary>
    [Serializable]
    public class CentralIndexKey : Identifier
    {
        public override string Abbrev => "CIK";
    }
}