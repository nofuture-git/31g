using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Credible
{
    public abstract class CredibleBase : LegalConcept, IAgitate
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            var ifs = IsCauseToFearSafety(defendant);
            var ised = IsSubstantialEmotionalDistress(defendant);

            if (!ifs && !ised)
            {
                AddReasonEntry($"{GetType().Name} defendant, {defendant.Name}, {nameof(IsCauseToFearSafety)} is false");
                AddReasonEntry($"{GetType().Name} defendant, {defendant.Name}, {nameof(IsSubstantialEmotionalDistress)} is false");
                return false;
            }

            return true;
        }

        public Predicate<ILegalPerson> IsCauseToFearSafety { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsSubstantialEmotionalDistress { get; set; } = lp => false;
    }
}
