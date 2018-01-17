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
            var randPrimarySector = NaicsPrimarySector.RandomPrimarySector();

            var sectors = randPrimarySector.Divisions;
            if (sectors == null)
                return null;

            if (filterBy != null)
            {
                sectors = sectors.Where(s => filterBy(s)).ToList();
                if (!sectors.Any())
                    return null;
            }

            var pickOne = Etx.RandomInteger(0, sectors.Count - 1);
            return sectors[pickOne];
        }
    }
}