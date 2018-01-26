using System;
using System.Linq;
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
        public static NaicsSector RandomSector(Predicate<NaicsSector> filterBy = null)
        {
            var randParent = NaicsPrimarySector.RandomPrimarySector();
            return randParent?.GetRandomClassification(filterBy);
        }
    }
}