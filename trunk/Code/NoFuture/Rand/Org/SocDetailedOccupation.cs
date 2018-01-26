using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Org
{
    /// <summary>
    /// The most detailed level of the Standard Occupational Classification system
    /// </summary>
    [Serializable]
    public class SocDetailedOccupation : ClassificationBase<ClassificationOfInstructionalPrograms>
    {
        public override string LocalName => "ternary-group";

        /// <summary>
        /// Gets a Detailed Group of the SOC at random
        /// </summary>
        /// <param name="filterBy"></param>
        /// <returns></returns>
        [RandomFactory]
        public static SocDetailedOccupation RandomSocDetailedOccupation(
            Predicate<SocDetailedOccupation> filterBy = null)
        {
            var randParent = SocBoardGroup.RandomSocBoardGroup();

            return randParent?.GetRandomClassification(filterBy);
        }
    }
}