using System;

namespace NoFuture.Rand.Data.Endo.Grps
{
    [Serializable]
    public class NaicsSector : ClassificationBase<NaicsMarket>
    {
        public override string LocalName => "secondary-sector";
    }
}