using System;
using NoFuture.Rand.Law.Criminal.US.Elements;

namespace NoFuture.Rand.Law.Criminal.US
{
    public interface ISexBilateral : IBilateral
    {
        /// <summary>
        /// loosely defined as vaginal, anal or oral penetration of by somebody else body part (penis, finger, etc.)
        /// </summary>
        Predicate<ILegalPerson> IsSexualIntercourse { get; set; }
    }
}