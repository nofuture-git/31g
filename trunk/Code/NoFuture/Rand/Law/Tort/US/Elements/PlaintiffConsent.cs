using System;
using System.Linq;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    public class PlaintiffConsent : LegalConcept, IConsent
    {
        public Predicate<ILegalPerson> IsCapableThereof { get; set; }
        public Predicate<ILegalPerson> IsApprovalExpressed { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (persons == null || !persons.Any())
            {
                AddReasonEntry($"{nameof(persons)} is null or empty");
                return false;
            }

            var plaintiff = persons.Plaintiff();
            if (plaintiff == null)
                return false;
            if (!IsCapableThereof(plaintiff))
            {
                AddReasonEntry($"plaintiff, {plaintiff.Name}, {nameof(IsCapableThereof)} is false");
                return false;
            }

            if (!IsApprovalExpressed(plaintiff))
            {
                AddReasonEntry($"plaintiff, {plaintiff.Name}, {nameof(IsApprovalExpressed)} is false");
                return false;
            }

            return true;
        }
    }
}
