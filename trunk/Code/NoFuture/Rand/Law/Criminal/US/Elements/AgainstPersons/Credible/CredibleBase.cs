using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.AgainstPersons.Credible
{
    public abstract class CredibleBase : LegalConcept, IAgitate
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            var ifs = IsCauseToFearSafety(defendant);
            var ised = IsSubstantialEmotionalDistress(defendant);

            if (!ifs && !ised)
            {
                AddReasonEntry($"{GetType().Name} {title} {defendant.Name}, {nameof(IsCauseToFearSafety)} is false");
                AddReasonEntry($"{GetType().Name} {title} {defendant.Name}, {nameof(IsSubstantialEmotionalDistress)} is false");
                return false;
            }

            return true;
        }

        public Predicate<ILegalPerson> IsCauseToFearSafety { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsSubstantialEmotionalDistress { get; set; } = lp => false;
    }
}
