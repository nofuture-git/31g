using System;

namespace NoFuture.Rand.Gov.Census
{
    [Serializable]
    public class MStatArea : Identifier
    {
        public override string Abbrev => "MSA";
        public UrbanCentric MsaType { get; set; }
    }

    [Serializable]
    public class ComboMStatArea : MStatArea
    {
        public override string Abbrev => "CBSA";
    }
}
