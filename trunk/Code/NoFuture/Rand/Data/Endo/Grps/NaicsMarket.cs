using System;

namespace NoFuture.Rand.Data.Endo
{
    [Serializable]
    public class NaicsMarket : ClassificationBase<StandardIndustryClassification>
    {
        public override string LocalName => "ternary-sector";
    }
}