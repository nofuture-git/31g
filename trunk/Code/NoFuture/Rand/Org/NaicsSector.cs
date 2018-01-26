using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Org
{
    [Serializable]
    public class NaicsSector : ClassificationBase<NaicsMarket>
    {
        public override string LocalName => "secondary-sector";

        /// <summary>
        /// Gets a NAICS Sector at random
        /// </summary>
        /// <param name="filterBy">
        /// Optional param to filter by
        /// </param>
        /// <returns></returns>
        [RandomFactory]
        public static NaicsSector RandomNaicsSector(Predicate<NaicsSector> filterBy = null)
        {
            var randParent = NaicsPrimarySector.RandomNaicsPrimarySector();
            return randParent?.GetRandomClassification(filterBy);
        }
    }
}