using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Endo.Enums;

namespace NoFuture.Rand.Gov.Census
{
    [Serializable]
    public class MStatArea : Identifier
    {
        public override string Abbrev => "MSA";
        public UrbanCentric MsaType { get; set; }
    }
}