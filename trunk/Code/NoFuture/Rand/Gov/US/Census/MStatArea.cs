using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.Census
{
    [Serializable]
    public class MStatArea : Identifier
    {
        public override string Abbrev => "MSA";
        public UrbanCentric MsaType { get; set; }
    }
}