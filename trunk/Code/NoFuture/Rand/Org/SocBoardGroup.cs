using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Org
{
    /// <summary>
    /// The ternary level of Standard Occupational Classification system
    /// </summary>
    [Serializable]
    public class SocBoardGroup : ClassificationBase<SocDetailedOccupation>
    {
        public override string LocalName => "secondary-group";

        /// <summary>
        /// Gets a Broad Group of the SOC at random
        /// </summary>
        /// <param name="filterBy"></param>
        /// <returns></returns>
        [RandomFactory]
        public static SocBoardGroup RandomSocBoardGroup(Predicate<SocBoardGroup> filterBy = null)
        {
            var randParent = SocMinorGroup.RandomSocMinorGroup();

            return randParent?.GetRandomClassification(filterBy);
        }
    }
}