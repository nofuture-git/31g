using System;

namespace NoFuture.Rand.Org
{
    [Serializable]
    public class NaicsMarket : ClassificationBase<StandardIndustryClassification>
    {
        public override string LocalName => "ternary-sector";
    }
}