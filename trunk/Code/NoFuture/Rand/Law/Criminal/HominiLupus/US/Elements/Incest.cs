using System;
using System.Linq;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <inheritdoc cref="ISexBipartitie"/>
    /// <summary>
    /// This seems to imply that the parties know they have some family relation
    /// </summary>
    public class Incest : LegalConcept, IActusReus, ISexBipartitie
    {
        public Predicate<ILegalPerson> IsSexualIntercourse { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsOneOfTwo { get; set; } = lp => false;

        /// <summary>
        /// Typically being any family member one could not marry
        /// </summary>
        public Func<ILegalPerson, ILegalPerson, bool> IsFamilyRelation { get; set; } = (lp1, lp2) => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            var victim = persons.FirstOrDefault(p => IsOneOfTwo(p) && !ReferenceEquals(p, defendant));
            if (victim == null)
            {
                AddReasonEntry($"the {nameof(IsOneOfTwo)} returned null for the other person");
                return false;
            }

            if (!IsSexualIntercourse(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsSexualIntercourse)} is false");
                return false;
            }

            var isFamily = IsFamilyRelation(defendant, victim);

            AddReasonEntry($"{nameof(IsFamilyRelation)} is {isFamily} for {defendant.Name} and {victim.Name}");
            return isFamily;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return criminalIntent != null;
        }
    }
}
