using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Org
{
    [Serializable]
    public class NaicsMarket : ClassificationBase<StandardIndustryClassification>
    {
        public override string LocalName => "ternary-sector";

        /// <summary>
        /// Gets a NAICS Market at random
        /// </summary>
        /// <param name="filterBy">
        /// Optional param to filter by
        /// </param>
        /// <returns></returns>
        [RandomFactory]
        public static NaicsMarket RandomNaicsMarket(Predicate<NaicsMarket> filterBy = null)
        {
            var randParent = NaicsSector.RandomNaicsSector();
            return randParent?.GetRandomClassification(filterBy);
        }
    }
}