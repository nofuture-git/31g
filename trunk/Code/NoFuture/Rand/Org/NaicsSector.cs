using System;

namespace NoFuture.Rand.Org
{
    [Serializable]
    public class NaicsSector : ClassificationBase<NaicsMarket>
    {
        public override string LocalName => "secondary-sector";
    }
}