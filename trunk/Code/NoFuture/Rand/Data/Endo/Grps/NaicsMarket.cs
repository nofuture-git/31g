using System;

namespace NoFuture.Rand.Data.Endo.Grps
{
    [Serializable]
    public class NaicsMarket : ClassificationBase<StandardIndustryClassification>
    {
        public override string LocalName => "ternary-sector";
    }
}