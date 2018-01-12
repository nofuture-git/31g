using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.Fed
{
    [Serializable]
    public class FdicNum : Identifier
    {
        //https://research.fdic.gov/bankfind/
        public override string Abbrev => "FDIC #";
    }
}