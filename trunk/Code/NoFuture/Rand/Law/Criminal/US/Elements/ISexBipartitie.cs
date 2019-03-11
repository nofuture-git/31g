using System;

namespace NoFuture.Rand.Law.Criminal.US.Elements
{
    public interface ISexBipartitie : IBipartite
    {
        /// <summary>
        /// loosely defined as vaginal, anal or oral penetration of by somebody else body part (penis, finger, etc.)
        /// </summary>
        Predicate<ILegalPerson> IsSexualIntercourse { get; set; }
    }
}