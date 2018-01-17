using System;
using System.Linq;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Org
{
    [Serializable]
    public class NaicsSuperSector : ClassificationBase<NaicsPrimarySector>
    {
        public override string LocalName => "category";

        /// <summary>
        /// Gets a NAICS Super Sector at random
        /// </summary>
        /// <param name="filterBy">
        /// Optional param to filter by
        /// </param>
        /// <returns></returns>
        [RandomFactory]
        public static NaicsSuperSector RandomSuperSector(Predicate<NaicsSuperSector> filterBy = null)
        {
            var superSectors = NorthAmericanIndustryClassification.AllSectors;
            if (superSectors == null)
                return null;

            if (filterBy != null)
            {
                superSectors = superSectors.Where(m => filterBy(m)).ToArray();
                if (!superSectors.Any())
                    return null;
            }

            var pickOne = Etx.RandomInteger(0, superSectors.Length - 1);
            return superSectors[pickOne];
        }
    }
}