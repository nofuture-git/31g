using System;
using System.Linq;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Org
{
    /// <summary>
    /// The primary level of Standard Occupational Classification system
    /// </summary>
    [Serializable]
    public class SocMajorGroup : ClassificationBase<SocMinorGroup>
    {
        public override string LocalName => "category";

        /// <summary>
        /// Get a SOC Major Group at random
        /// </summary>
        /// <param name="filterBy">
        /// Optional param to filter by
        /// </param>
        /// <returns></returns>
        [RandomFactory]
        public static SocMajorGroup RandomSocMajorGroup(Predicate<SocMajorGroup> filterBy = null)
        {
            var allGroups = StandardOccupationalClassification.AllGroups;
            if (allGroups == null)
                return null;

            if (filterBy != null)
            {
                allGroups = allGroups.Where(m => filterBy(m)).ToArray();
                if (!allGroups.Any())
                    return null;
            }

            var pickOne = Etx.RandomInteger(0, allGroups.Length - 1);
            return allGroups[pickOne];
        }
    }
}