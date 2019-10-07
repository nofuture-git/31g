using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.Fed
{
    /// <summary>
    /// Identifier assigned by Federal Deposit Insurance Corporation
    /// see [https://research.fdic.gov/bankfind/]
    /// </summary>
    [Serializable]
    public class FdicNum : Identifier
    {
        public override string Abbrev => "FDIC #";
    }
}