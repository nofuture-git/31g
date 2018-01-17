using System;
using System.Linq;
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
        public static NaicsMarket RandomMarket(Predicate<NaicsMarket> filterBy = null)
        {
            var randSector = NaicsSector.RandomSector();

            var markets = randSector.Divisions;
            if (markets == null)
                return null;

            if (filterBy != null)
            {
                markets = markets.Where(m => filterBy(m)).ToList();
                if (!markets.Any())
                    return null;
            }
            var pickOne = Etx.RandomInteger(0, markets.Count - 1);
            return markets[pickOne];
        }
    }
}