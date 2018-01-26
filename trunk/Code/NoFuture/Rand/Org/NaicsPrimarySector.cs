using System;
using System.Linq;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Org
{
    /// <summary>
    /// This represents the primary grouping level of the NAICS 
    /// </summary>
    [Serializable]
    public class NaicsPrimarySector : ClassificationBase<NaicsSector>
    {
        public override string LocalName => "primary-sector";

        /// <summary>
        /// Gets a NAICS Primary Sector at random
        /// </summary>
        /// <param name="filterBy">
        /// Optional param to filter by
        /// </param>
        /// <returns></returns>
        [RandomFactory]
        public static NaicsPrimarySector RandomPrimarySector(Predicate<NaicsPrimarySector> filterBy = null)
        {
            var randParent = NaicsSuperSector.RandomSuperSector();
            return randParent?.GetRandomClassification(filterBy);
        }
    }
}