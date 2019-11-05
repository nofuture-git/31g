using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US
{
    /// <inheritdoc cref="ISexBilateral"/>
    public class SexBilateral : LegalConcept, ISexBilateral
    {
        public Predicate<ILegalPerson> IsOneOfTwo { get; set; } = lp => false;

        /// <summary>
        /// loosely defined as vaginal, anal or oral penetration of by somebody else body part (penis, finger, etc.)
        /// </summary>
        public Predicate<ILegalPerson> IsSexualIntercourse { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!IsOneOfTwo(defendant))
            {
                AddReasonEntry($"defendant {defendant.Name}, {nameof(IsOneOfTwo)} is false");
                return false;
            }

            if (!IsSexualIntercourse(defendant))
            {
                AddReasonEntry($"defendant {defendant.Name}, {nameof(IsSexualIntercourse)} is false");
                return false;
            }

            return true;
        }
    }
}
