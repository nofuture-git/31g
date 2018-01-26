using System;
using System.Linq;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Org
{
    /// <summary>
    /// The secondary level of Standard Occupational Classification system
    /// </summary>
    [Serializable]
    public class SocMinorGroup : ClassificationBase<SocBoardGroup>
    {
        public override string LocalName => "primary-group";

        /// <summary>
        /// Gets a Minor Group of the SOC at random
        /// </summary>
        /// <param name="filterBy"></param>
        /// <returns></returns>
        [RandomFactory]
        public static SocMinorGroup RandomSocMinorGroup(Predicate<SocMinorGroup> filterBy = null)
        {
            var randParent = SocMajorGroup.RandomSocMajorGroup();

            return randParent?.GetRandomClassification(filterBy);
        }
    }
}